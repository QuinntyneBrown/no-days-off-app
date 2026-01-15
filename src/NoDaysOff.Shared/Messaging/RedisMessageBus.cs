using MessagePack;
using Microsoft.Extensions.Logging;
using NoDaysOff.Shared.Messages;
using StackExchange.Redis;

namespace NoDaysOff.Shared.Messaging;

/// <summary>
/// Redis-based message bus for production
/// </summary>
public class RedisMessageBus : IMessageBus, IDisposable
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ISubscriber _subscriber;
    private readonly ILogger<RedisMessageBus> _logger;
    private bool _disposed;

    public RedisMessageBus(IConnectionMultiplexer redis, ILogger<RedisMessageBus> logger)
    {
        _redis = redis;
        _subscriber = redis.GetSubscriber();
        _logger = logger;
    }

    public async Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken = default) where T : IMessage
    {
        try
        {
            var data = MessagePackSerializer.Serialize(message);
            await _subscriber.PublishAsync(new RedisChannel(topic, RedisChannel.PatternMode.Literal), data);
            _logger.LogInformation("Published message {MessageId} to topic {Topic}", message.MessageId, topic);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to topic {Topic}", topic);
            throw;
        }
    }

    public async Task SubscribeAsync<T>(string topic, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : IMessage
    {
        await _subscriber.SubscribeAsync(new RedisChannel(topic, RedisChannel.PatternMode.Literal), async (channel, value) =>
        {
            try
            {
                var message = MessagePackSerializer.Deserialize<T>((byte[])value!);
                _logger.LogInformation("Received message {MessageId} on topic {Topic}", message.MessageId, topic);
                await handler(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message on topic {Topic}", topic);
            }
        });

        _logger.LogInformation("Subscribed to topic {Topic}", topic);
    }

    public async Task UnsubscribeAsync(string topic)
    {
        await _subscriber.UnsubscribeAsync(new RedisChannel(topic, RedisChannel.PatternMode.Literal));
        _logger.LogInformation("Unsubscribed from topic {Topic}", topic);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        // Don't dispose _redis as it's managed by the DI container
        _disposed = true;
    }
}
