using System.Collections.Concurrent;
using Application.Price;
using Application.Services.Binance;
using Application.Services.Binance.Models;
using Domain.Models.Instruments;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PriceSignal.BackgroundServices;

public class BinanceBackfillService(IServiceProvider serviceProvider, ILogger<BinanceBackfillService> logger,ConcurrentBag<string> symbols, PriceHistoryCache priceHistoryCache) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("BinanceBackfillService is starting.");
        return BackfillDataAsync(stoppingToken);
    }
    
    private async Task BackfillDataAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var binanceApi = scope.ServiceProvider.GetRequiredService<IBinanceApi>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var binanceExchange = await dbContext.Exchanges.AsNoTracking().FirstOrDefaultAsync(e => e.Name == "Binance", cancellationToken: cancellationToken);
        if (binanceExchange is null)
        {
            return;
        }

        foreach (var symbol in symbols)
        {
            var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var startTime = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds();
            
            // check if we have data for the symbol within this time frame. if we do, skip
            var existingData = await dbContext.OneMinCandle
                .Where(p => p.Symbol == symbol && p.Exchange.Id == binanceExchange.Id &&
                            p.Bucket >= DateTimeOffset.FromUnixTimeMilliseconds(startTime) &&
                            p.Bucket <= DateTimeOffset.FromUnixTimeMilliseconds(endTime))
                .OrderBy(p=>p.Bucket)
                //.CountAsync(cancellationToken: cancellationToken);
                .ToListAsync(cancellationToken);
                if (existingData.Count >= 1439)
                {
                    continue;
                }

                // if we have some data, but not enough, we need to backfill. find gaps in the data and set the start time to the last known data point. there should be more than 1 minute of data missing
                for (var i = 0; i < existingData.Count - 1; i++)
                {
                    var diff = existingData[i + 1].Bucket - existingData[i].Bucket;
                    if (!(diff.TotalMinutes > 1)) continue;
                    startTime = existingData[i].Bucket.ToUnixTimeMilliseconds();
                    break;
                }
                // if (existingData.Count != 0)
            // {
            //     continue;
            // }
            var moreData = true;
            
            logger.LogInformation($"Back-filling data for {symbol}");

            while (moreData && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var response = await binanceApi.GetHistoricalCandlesAsync(new KlineParams()
                    {
                        Symbol = symbol,
                        Interval = KlineInterval.OneMinute,
                        StartTime = startTime,
                        EndTime = endTime
                    }, cancellationToken);
                    var prices = response as InstrumentPrice[] ?? response.ToArray();
                    
                    if (prices.Length != 0)
                    {
                        startTime = prices.Last().Timestamp.ToUnixTimeMilliseconds();
                        
                        if (prices.Length < 1000)
                        {
                            moreData = false;
                        }
                    }
                    else
                    {
                        moreData = false;
                    }
                    
                    foreach (var price in prices)
                    {
                        price.SetExchange(binanceExchange);
                        var insertQuery = $"""
                                           INSERT INTO instrument_prices (price, symbol, volume, quantity, timestamp, exchange_id)
                                           VALUES ({price.Price}, '{price.Symbol}', {price.Volume}, {price.Quantity}, '{price.Timestamp:s}', {binanceExchange.Id})
                                           """;
                        await dbContext.Database.ExecuteSqlRawAsync(insertQuery, cancellationToken: cancellationToken);
                    }

                    var sqlStartTime = $"{DateOnly.FromDateTime(DateTimeOffset.UtcNow.AddDays(-2).UtcDateTime):O}";
                    var sqlEndTime = $"{DateOnly.FromDateTime(DateTime.UtcNow):O}";
                    await dbContext.Database.ExecuteSqlRawAsync($"""
                                                                 call refresh_continuous_aggregate('one_min_candle','{sqlStartTime}','{sqlEndTime}');
                                                                 """, cancellationToken: cancellationToken);
                    
                    var history = new List<IPrice>(dbContext.OneMinCandle
                        .Where(o => o.Symbol == symbol)
                        .OrderByDescending(o => o.Bucket.DateTime)
                        .Take(500).Select(
                            o=> new PriceQuote(new Price
                            {
                                Symbol = o.Symbol,
                                Open = o.Open,
                                High = o.High,
                                Low = o.High,
                                Close = o.Close,
                                Volume = o.Volume,
                                Bucket = o.Bucket,
                            })
                        ).ToList());
                    priceHistoryCache.LoadPriceHistory(symbol,history);



                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Failed to back-fill data for {symbol}");
                    moreData = false;
                }
            }
            logger.LogInformation($"Back-filling data for {symbol} completed");
        }
        
        logger.LogInformation("Back-filling data completed");
        
    }
}