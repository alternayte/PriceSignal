
using Application.Common.Interfaces;
using Domain.Models.Instruments;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace PriceSignal.BackgroundServices;

public class BinancePriceFetcherService(
    ILogger<BinancePriceFetcherService> logger,
    IServiceProvider serviceProvider,
    TimeProvider timeProvider,
    IWebsocketClientProvider websocketClientProvider)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        websocketClientProvider.Start(async message=> await ProcessMessageAsync(message));
        return Task.CompletedTask;
    }
    
    public override Task StopAsync(CancellationToken stoppingToken)
    {
        websocketClientProvider.Stop();
        return base.StopAsync(stoppingToken);
    }
    
    private async Task ProcessMessageAsync(string message)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var exchange = dbContext.Exchanges.First(e => e.Name == "Binance");
        
        // var jsonObject = JObject.Parse(message);
        //
        // var symbol = jsonObject.Value<string>("s");
        // var price = jsonObject.Value<decimal>("p");
        // var quantity = jsonObject.Value<decimal>("q");
        // var volume = jsonObject.Value<decimal>("v");
        //
        // if (symbol != null)
        // {
        //
        //     await dbContext.InstrumentPrices.AddAsync(new InstrumentPrice()
        //     {
        //         Symbol = symbol,
        //         Price = price,
        //         Quantity = quantity,
        //         Volume = volume,
        //         Timestamp = timeProvider.GetUtcNow(),
        //         Exchange = exchange
        //     });
        //
        //     logger.LogInformation($"Received price update from Binance: {symbol} = {price}");
        // }
        
        var jsonArray = JArray.Parse(message);
        
        foreach (var token in jsonArray)
        {
            var symbol = token["s"]?.ToString();
            var price = token["c"]?.ToObject<decimal>();
            var quantity = token["q"]?.ToObject<decimal>();
            var volume = token["v"]?.ToObject<decimal>();

            if (symbol == null || price == null) continue;
            // await dbContext.InstrumentPrices.AddAsync(new InstrumentPrice
            // {
            //     Symbol = symbol,
            //     Price = (decimal) price,
            //     Quantity = quantity ?? 0m,
            //     Volume = volume ?? 0m,
            //     Timestamp = timeProvider.GetUtcNow(),
            //     Exchange = exchange
            // });

            await dbContext.Database.ExecuteSqlAsync(
                $"""
                 INSERT INTO instrument_prices (symbol, price, volume, quantity, timestamp, exchange_id)
                 VALUES ({symbol}, {price}, {volume}, {quantity}, {timeProvider.GetUtcNow()}, {exchange.Id})
                 """);
            
            logger.LogInformation($"Received price update from Binance: {symbol} = {price}");
        }
        

        // await dbContext.SaveChangesAsync();
    }
}