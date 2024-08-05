using System.Text.Json;
using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

namespace Infrastructure.PubSub;

public class NatsService : IPubSub, IAsyncDisposable
{
    private readonly NatsConnection _connection;
    private readonly INatsJSContext _jetStream;
    private readonly ILogger<NatsService> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;


    public NatsService(ILogger<NatsService> logger)
    {
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        
        _logger = logger;
        _connection = new NatsConnection(new NatsOpts(){Url = "nats://localhost:4222"});
        _jetStream = new NatsJSContext(_connection);
        _jetStream.CreateStreamAsync(new StreamConfig(name: "notifications",
                subjects: new[]
                {
                    "notifications.>"
                }))
            .GetAwaiter()
            .GetResult();
    }
    public async Task PublishAsync<T>(string subject, T message)
    {
        var content = JsonSerializer.Serialize(message, _jsonSerializerOptions);
        var ack = await _jetStream.PublishAsync(subject, content);
        ack.EnsureSuccess();

        _logger.LogInformation("Published message to {Subject}: {Message}", subject,content);
    }

    public async void Subscribe<T>(string stream, Func<T, Task> handler, string? subject = null)
    {
        var consumer = await _jetStream.CreateOrUpdateConsumerAsync(stream, new ConsumerConfig($"{stream}_processor_dotnet"));
        await foreach (var jsMsg in consumer.ConsumeAsync<string>())
        {
            if (jsMsg.Data == null) continue;
            if (subject != null && !jsMsg.Subject.Contains(subject)) continue;
            var message = JsonSerializer.Deserialize<T>(jsMsg.Data, _jsonSerializerOptions);
            if (message != null) await handler(message);
            await jsMsg.AckAsync();
        }
        _logger.LogInformation("Subscribed to {Stream}", stream);
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        _logger.LogInformation("NatsService disposed");
    }
}