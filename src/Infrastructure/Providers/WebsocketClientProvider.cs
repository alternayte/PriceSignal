using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using Application.Common.Interfaces;
using Infrastructure.Channels;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace Infrastructure.Providers;

public class WebsocketClientProvider(string url, ILogger<WebsocketClientProvider> logger) : IWebsocketClientProvider
{
    private readonly Uri _uri = new(url);
    private WebsocketClient _client = null!;

    public void Start(Func<string, Task> onMessageReceived)
    {
        _client = new WebsocketClient(_uri)
        {
            ReconnectTimeout = TimeSpan.FromSeconds(30),
            ErrorReconnectTimeout = TimeSpan.FromSeconds(30)
        };

        _client.MessageReceived
            .Where(msg => msg.MessageType == WebSocketMessageType.Text)
            .Select(msg => msg.Text ?? string.Empty)
            .Where(msg => !string.IsNullOrWhiteSpace(msg))
            .Subscribe(OnNext);

        _client.DisconnectionHappened.Subscribe(info =>
        {
            logger.LogWarning("Disconnection happened, type: {Type}", info.Type);
        });

        _client.ReconnectionHappened.Subscribe(info =>
        {
            logger.LogInformation("Reconnection happened, type: {Type}", info.Type);
        });

        _client.Start();
        return;

        async void OnNext(string message)
        {
            //logger.LogInformation("Message received: {Message}", message);
            await onMessageReceived(message);
        }
    }

    public void Stop()
    {
        _client.Dispose();
    }
}