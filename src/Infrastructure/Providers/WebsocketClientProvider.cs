using System.Net.WebSockets;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using Application.Common.Interfaces;
using Infrastructure.Channels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Infrastructure.Providers;

public class WebsocketClientProvider(string url, ILogger<WebsocketClientProvider> logger) : IWebsocketClientProvider
{
    private Uri _uri = new(url);
    private WebsocketClient _client = null!;

    public string GetUri()
    {
        return _uri.ToString();
    }
    public void SetUri(string url)
    {
        _uri = new Uri(url);
    }
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
            .Throttle(TimeSpan.FromMilliseconds(200))  // Throttle to one message every 200ms (5 per second)
            .ObserveOn(TaskPoolScheduler.Default)
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
    
    public void Subscribe(IEnumerable<string> streams)
    {
        var request = new
        {
            method = "SUBSCRIBE",
            @params = streams,
            id = GetRequestId()
        };
        _client.Send(JObject.FromObject(request).ToString());
    }

    public void Unsubscribe(IEnumerable<string> streams)
    {
        var request = new
        {
            method = "UNSUBSCRIBE",
            @params = streams,
            id = GetRequestId()
        };
        _client.Send(JObject.FromObject(request).ToString());
    }

    private int GetRequestId()
    {
        // Ensure thread-safe incrementing of the request ID.
        return Interlocked.Increment(ref _requestId);
    }

    private static int _requestId = 0;
}