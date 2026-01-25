using Shared.Messages;

namespace Shared.Messaging;

/// <summary>
/// Interface for message handlers that process messages from the message bus
/// </summary>
public interface IMessageHandler<TMessage> where TMessage : IMessage
{
    Task HandleAsync(TMessage message, CancellationToken cancellationToken = default);
}
