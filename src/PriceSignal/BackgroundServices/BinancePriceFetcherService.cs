
using Application.Common.Interfaces;
using Infrastructure.Channels;

namespace PriceSignal.BackgroundServices;

public class BinancePriceFetcherService(IWebsocketClientProvider websocketClientProvider)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        websocketClientProvider.Start(async message =>
        {
            await WebsocketChannel.SocketChannel.Writer.WriteAsync(message, stoppingToken);
        });
        return Task.CompletedTask;

    }
    
    public override Task StopAsync(CancellationToken stoppingToken)
    {
        websocketClientProvider.Stop();
        return base.StopAsync(stoppingToken);
    }
    
    
}