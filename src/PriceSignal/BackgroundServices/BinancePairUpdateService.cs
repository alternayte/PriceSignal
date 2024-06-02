using Application.Services.Binance;
using Domain.Models.Instruments;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PriceSignal.BackgroundServices;

public class BinancePairUpdateService(IServiceProvider serviceProvider, ILogger<BinancePairUpdateService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("BinancePairUpdateService is starting.");
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var binanceApi = scope.ServiceProvider.GetRequiredService<IBinanceApi>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var tradingPairs = await binanceApi.GetTradingPairs(stoppingToken);
            var instruments = tradingPairs as Instrument[] ?? tradingPairs.ToArray();
            var binanceExchange = await dbContext.Exchanges.FirstOrDefaultAsync(e => e.Name == "Binance", stoppingToken);
            if (binanceExchange is null)
            {
                return;
            }
            
            foreach (var instrument in instruments)
            {
                instrument.SetExchange(binanceExchange);
            }
            var dbPairs = await dbContext.Instruments.Where(i=>i.Exchange == binanceExchange).ToListAsync(stoppingToken);
            var newPairs = instruments.Where(p => dbPairs.All(dbPair => dbPair.Symbol != p.Symbol)).ToList();
            
            foreach (var dbPair in dbPairs)
            {
                if (instruments.All(p => p.Symbol != dbPair.Symbol))
                {
                    dbPair.DeletedAt = DateTime.UtcNow;
                }
                else
                {
                    dbPair.DeletedAt = null;
                }
            }
            if (newPairs.Count != 0)
            {
                await dbContext.Instruments.AddRangeAsync(newPairs, stoppingToken);
            }
            
            await dbContext.SaveChangesAsync(stoppingToken);
            
            logger.LogInformation("BinancePairUpdateService is running.");

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            logger.LogInformation("BinancePairUpdateService will run again in 1 day.");
        }
        logger.LogInformation("BinancePairUpdateService is stopping.");
    }
    }