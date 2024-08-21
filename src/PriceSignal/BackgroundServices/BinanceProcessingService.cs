using System.Collections.Concurrent;
using Application.Common.Interfaces;
using Application.Price;
using Application.Rules;
using Application.Rules.Common;
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
    ILogger<BinanceProcessingService> logger,
    IServiceProvider serviceProvider,
    TimeProvider timeProvider,
    IWebsocketClientProvider websocketClientProvider)
    : BackgroundService
{
    private ConcurrentDictionary<string, Price> _currentPriceData = new();
    private long ExchangeId { get; set; }
    private Timer updateTimer;
    private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private RuleEngine ruleEngine;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        ruleEngine = scope.ServiceProvider.GetRequiredService<RuleEngine>();
        var exchange =
            dbContext.Exchanges.FirstOrDefaultAsync(e => e.Name == "Binance", cancellationToken: stoppingToken);
        ExchangeId = exchange.Id;
        var channel = WebsocketChannel.SocketChannel;
        try
        {
            await foreach (var message in channel.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    await ProcessMessageAsync(message);    
                } catch (Exception e)
                {
                    logger.LogError(e, "Failed to process message");
                }
            
            }
            logger.LogInformation("Stopped processing messages");
        } catch(Exception e)
        {
            logger.LogError(e, "Failed to read message from channel");
        }
        
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        updateTimer = new(PushCurrentOhlcData, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        websocketClientProvider.Stop();
        updateTimer?.Dispose();
        return base.StopAsync(stoppingToken);
    }

    private async Task ProcessMessageAsync(string message)
    {
        logger.LogTrace("Processing message: {message}", message);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var topicEventSender = scope.ServiceProvider.GetRequiredService<ITopicEventSender>();

        var json = JObject.Parse(message);
        var stream = json["stream"]?.ToString();
        var data = json["data"];

        if (stream == null || data == null)
        {
            return;
        }

        var eventType = data["e"]?.ToString();
        if (eventType != "aggTrade")
        {
            return;
        }

        var symbol = data["s"]?.ToString();
        var price = data["p"]?.ToObject<decimal>();
        var quantity = data["q"]?.ToObject<decimal>();
        var tradeTime = data["T"]?.ToObject<long>();

        if (symbol == null || price == null || quantity == null || tradeTime == null)
        {
            return;
        }

        var timestamp = timeProvider.GetUtcNow();
        var truncated = timestamp.Date.AddHours(timestamp.Hour).AddMinutes(timestamp.Minute);

        if (_currentPriceData.TryGetValue(symbol, out var ohlcData))
        {
            // Check if the trade is in a new minute
            if (ohlcData.Bucket.Minute != truncated.Minute)
            {
                // Finalize and reset OHLC data for the new minute
                FinalizeAndResetOhlcData(symbol, ohlcData, ExchangeId, dbContext);

                // Create new OHLC data for the new minute
                ohlcData = new Price
                {
                    Symbol = symbol,
                    Open = price.Value,
                    High = price.Value,
                    Low = price.Value,
                    Close = price.Value,
                    Volume = quantity.Value,
                    Bucket = truncated,
                };
                _currentPriceData[symbol] = ohlcData;
                await topicEventSender.SendAsync(nameof(PriceSubscriptions.OnPriceUpdated), ohlcData);
                //ruleEngine.EvaluateRules(new PriceQuote(ohlcData));
            }
            else
            {
                // Update the existing OHLC data
                ohlcData.Update(price.Value, quantity.Value);
                await topicEventSender.SendAsync(nameof(PriceSubscriptions.OnPriceUpdated), ohlcData);
                //ruleEngine.EvaluateRules(new PriceQuote(ohlcData));
            }
        }
        else
        {
            // Create new OHLC data for the symbol
            ohlcData = new Price
            {
                Symbol = symbol,
                Open = price.Value,
                High = price.Value,
                Low = price.Value,
                Close = price.Value,
                Volume = quantity.Value,
                Bucket = truncated,
            };
            _currentPriceData[symbol] = ohlcData;
            await topicEventSender.SendAsync(nameof(PriceSubscriptions.OnPriceUpdated), ohlcData);
            //ruleEngine.EvaluateRules(new PriceQuote(ohlcData));
        }
    }

    private async void PushCurrentOhlcData(object? state)
    {
        await _semaphore.WaitAsync();
        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var valuesList = new List<string>();
            var timestamp = timeProvider.GetUtcNow();
            foreach (var (symbol, ohlcData) in _currentPriceData)
            {
                var values =
                    $"('{symbol}', {ohlcData.Close}, {ohlcData.Volume}, {ohlcData.Volume}, '{timestamp:s}', {ExchangeId})";
                valuesList.Add(values);
//                 var insertQuery = $"""
//                                    INSERT INTO instrument_prices (symbol, price, volume, quantity, timestamp, exchange_id)
//                                    VALUES {values}
//                                    """;
//                 await dbContext.Database.ExecuteSqlRawAsync(insertQuery);
            }
            if (valuesList.Count > 0)
            {
                var insertQuery = $"""
                                   INSERT INTO instrument_prices (symbol, price, volume, quantity, timestamp, exchange_id)
                                   VALUES {string.Join(",", valuesList)}
                                   """;
                await dbContext.Database.ExecuteSqlRawAsync(insertQuery);
            }
            _currentPriceData.Clear();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to insert OHLC data");
        }
        finally
        {
            _semaphore.Release();
        }
    }


    private async void FinalizeAndResetOhlcData(string symbol, Price ohlcData, long exchangeId, AppDbContext dbContext)
    {
        var timestamp = timeProvider.GetUtcNow();
        var values =
            $"('{symbol}', {ohlcData.Close}, {ohlcData.Volume}, {ohlcData.Volume}, '{timestamp:s}', {exchangeId})";
        var insertQuery = $"""
                           INSERT INTO instrument_prices (symbol, price, volume, quantity, timestamp, exchange_id)
                           VALUES {values}
                           """;
        try
        {
            //await dbContext.Database.ExecuteSqlRawAsync(insertQuery);
            var lastPrice =  dbContext.OneMinCandle
                .Where(o => o.Symbol == symbol)
                .OrderByDescending(o => o.Bucket)
                .First();
            
            var latestPrice = new PriceQuote(new Price
            {
                Symbol = symbol,
                Open = lastPrice.Open,
                High = lastPrice.High,
                Low = lastPrice.Low,
                Close = lastPrice.Close,
                Volume = lastPrice.Volume,
                Bucket = lastPrice.Bucket,
            });
            ruleEngine.EvaluateRules(latestPrice);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to insert OHLC data");
        }

        // Remove the finalized OHLC data
        _currentPriceData.TryRemove(symbol, out _);
    }
}