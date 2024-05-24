using Domain.Models.Exchanges;
using Domain.Models.Instruments;
using HotChocolate.Subscriptions;
using Infrastructure.Channels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PriceSignal.GraphQL.Subscriptions;

namespace PriceSignal.BackgroundServices;

public class BinanceProcessingService(
    ILogger<BinancePriceFetcherService> logger,
    IServiceProvider serviceProvider,
    TimeProvider timeProvider
    ) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = WebsocketChannel.SocketChannel;
        await foreach (var message in channel.Reader.ReadAllAsync(stoppingToken))
        {
            await ProcessMessageAsync(message);
        }
    }
    
    private async Task ProcessMessageAsync(string message)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var topicEventSender = scope.ServiceProvider.GetRequiredService<ITopicEventSender>();

        var exchange = dbContext.Exchanges.First(e => e.Name == "Binance");
        
        var jsonArray = JArray.Parse(message);
        var valuesList = new List<string>();

        
        foreach (var token in jsonArray)
        {
            var symbol = token["s"]?.ToString();
            var open = token["o"]?.ToObject<decimal>();
            var high = token["h"]?.ToObject<decimal>();
            var low = token["l"]?.ToObject<decimal>();
            var close = token["c"]?.ToObject<decimal>();
            var quantity = token["q"]?.ToObject<decimal>();
            var volume = token["v"]?.ToObject<decimal>();

            if (symbol == null || close == null) continue;
            
            var timestamp = timeProvider.GetUtcNow();
            var truncated = timestamp.Date.AddHours(timestamp.Hour).AddMinutes(timestamp.Minute);
            var exchangeId = exchange.Id;
            var values = $"('{symbol}', {close}, {volume ?? 0m}, {quantity ?? 0m}, '{timestamp:s}', {exchangeId})";
            valuesList.Add(values);
            await topicEventSender.SendAsync(nameof(PriceSubscriptions.OnPriceUpdated), new Price
            {
                Symbol = symbol,
                Open = open ?? 0m,
                High = high ?? 0m,
                Low = low ?? 0m,
                Close = close ?? 0m,
                Volume = volume ?? 0m,
                Bucket = truncated,
            });

        }

        if (valuesList.Any())
        {
            var values = string.Join(',', valuesList);
#pragma warning disable EF1002
            await dbContext.Database.ExecuteSqlRawAsync(
#pragma warning restore EF1002
                $"""
                 INSERT INTO instrument_prices (symbol, price, volume, quantity, timestamp, exchange_id)
                 VALUES {values}
                 """);
            
        }
        
    }
}