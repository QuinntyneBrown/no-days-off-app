using NoDaysOff.Shared.Messages;

namespace NoDaysOff.Shared.Messaging;

/// <summary>
/// Interface for message bus operations
/// </summary>
public interface IMessageBus
{
    Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken = default) where T : IMessage;
    
    Task SubscribeAsync<T>(string topic, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : IMessage;
    
    Task UnsubscribeAsync(string topic);
}
