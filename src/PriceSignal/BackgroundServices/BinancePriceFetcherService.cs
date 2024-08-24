using System.Collections.Concurrent;
using System.Text;
using Application.Common.Interfaces;
using Application.Price;
using Application.Services.Binance;
using Application.Services.Binance.Models;
using Domain.Models.Instruments;
using Infrastructure.Channels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PriceSignal.BackgroundServices;

public class BinancePriceFetcherService(
    IWebsocketClientProvider websocketClientProvider,
    IServiceProvider serviceProvider,
    PriceHistoryCache priceHistoryCache,
    ConcurrentBag<string> symbols)
    : BackgroundService
{
    private ConcurrentDictionary<string, bool> _activeStreams = new();
    //private ConcurrentBag<string> symbols = new() {"BTCUSDT", "ETHUSDT"};


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var webSocketUrl = websocketClientProvider.GetUri();

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var dBsymbols = await dbContext.PriceRules.Where(x => x.IsEnabled).Select(r => r.Instrument.Symbol).Distinct()
            .ToListAsync(cancellationToken: stoppingToken);

        if (dBsymbols.Any())
        {
            var symbolStreams = dBsymbols.Select(s => $"{s.ToLower()}@aggTrade").Aggregate((s1, s2) => $"{s1}/{s2}");
            var currentStreams = webSocketUrl.Split("streams=").Last().Split("/").Select(s => s).ToList();
            var appendedStreams = symbolStreams.Split("/").Where(s => !currentStreams.Contains(s))
                .Aggregate((s1, s2) => $"{s1}/{s2}");

            string newUrl = new StringBuilder(webSocketUrl).Append('/').Append(appendedStreams).ToString();
            websocketClientProvider.SetUri(newUrl);
        }

        foreach (var symbol in dBsymbols)
        {
            symbols.Add(symbol);
            var history = new List<IPrice>(dbContext.OneMinCandle
                .Where(o => o.Symbol == symbol)
                .OrderByDescending(o => o.Bucket.DateTime)
                .Take(500).Select(
                    o => new PriceQuote(new Price
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
            priceHistoryCache.LoadPriceHistory(symbol, history);
        }

        websocketClientProvider.Start(async message =>
        {
            try
            {
                await WebsocketChannel.SocketChannel.Writer.WriteAsync(message, stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }

    public void UpdateSubscriptionsAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var dBsymbols = dbContext.PriceRules.Select(r => r.Instrument.Symbol).Except(symbols).Distinct().ToList();

        // add DB symbols to the list
        foreach (var symbol in dBsymbols)
        {
            symbols.Add(symbol);
            var history = new List<IPrice>(dbContext.OneMinCandle.Where(o => o.Symbol == symbol).Take(500).Select(
                o => new PriceQuote(new Price
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
            priceHistoryCache.LoadPriceHistory(symbol, history);
        }

        var currentStreams = _activeStreams.Keys.ToList();
        var newStreams = symbols.Except(currentStreams).Select(s => $"{s.ToLower()}@aggTrade").ToList();
        var obsoleteStreams = currentStreams.Except(symbols.Select(s => $"{s.ToLower()}@aggTrade")).ToList();

        if (newStreams.Count != 0)
        {
            websocketClientProvider.Subscribe(newStreams);
            foreach (var stream in newStreams)
            {
                _activeStreams[stream] = true;
            }
        }

        if (obsoleteStreams.Count == 0) return;

        websocketClientProvider.Unsubscribe(obsoleteStreams);
        foreach (var stream in obsoleteStreams)
        {
            _activeStreams.TryRemove(stream, out _);
        }
    }


    public void AddSymbol(string symbol)
    {
        symbols.Add(symbol);
    }

    public void RemoveSymbol(string symbol)
    {
        symbols = new ConcurrentBag<string>(symbols.Except(new[] {symbol}));
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        websocketClientProvider.Stop();
        return base.StopAsync(stoppingToken);
    }
}