
using System.Collections.Concurrent;
using Application.Common.Interfaces;
using Infrastructure.Channels;
using Infrastructure.Data;

namespace PriceSignal.BackgroundServices;

public class BinancePriceFetcherService(IWebsocketClientProvider websocketClientProvider, IServiceProvider serviceProvider)
    : BackgroundService
{
    private ConcurrentDictionary<string, bool> _activeStreams = new();
    private ConcurrentBag<string> symbols = new() {"BTCUSDT"};


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        websocketClientProvider.Start(async message =>
        {
            await WebsocketChannel.SocketChannel.Writer.WriteAsync(message, stoppingToken);
        });
        return Task.CompletedTask;

    }
    
    public async Task UpdateSubscriptionsAsync()
    {
        using var scope = serviceProvider.CreateScope();
        //var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // var symbols = await dbContext.UserRules.Select(r => r.Symbol).Distinct().ToListAsync();
        symbols.Add("ETHUSDT");

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

        if (obsoleteStreams.Count != 0)
        {
            websocketClientProvider.Unsubscribe(obsoleteStreams);
            foreach (var stream in obsoleteStreams)
            {
                _activeStreams.TryRemove(stream, out _);
            }
        }
    }
    
    public void AddSymbol(string symbol)
    {
        symbols.Add(symbol);
    }

    public void RemoveSymbol(string symbol)
    {
        symbols = new ConcurrentBag<string>(symbols.Except(new[] { symbol }));
    }
    
    public override Task StopAsync(CancellationToken stoppingToken)
    {
        websocketClientProvider.Stop();
        return base.StopAsync(stoppingToken);
    }
    
    
}