using System.Collections.Concurrent;
using Application.Services.Binance;
using Application.Services.Binance.Models;
using Domain.Models.Instruments;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PriceSignal.BackgroundServices;

public class BinanceBackfillService(IServiceProvider serviceProvider, ILogger<BinanceBackfillService> logger,ConcurrentBag<string> symbols) : BackgroundService
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
            var existingCount = await dbContext.OneMinCandle
                .Where(p => p.Symbol == symbol && p.Exchange.Id == binanceExchange.Id &&
                            p.Bucket >= DateTimeOffset.FromUnixTimeMilliseconds(startTime) &&
                            p.Bucket <= DateTimeOffset.FromUnixTimeMilliseconds(endTime))
                .CountAsync(cancellationToken: cancellationToken);
                // .ToListAsync(cancellationToken);
                if (existingCount >= 1000)
                {
                    continue;
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