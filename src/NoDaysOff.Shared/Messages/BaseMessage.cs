namespace NoDaysOff.Shared.Messages;

/// <summary>
/// Base class for all messages with common properties
/// </summary>
public abstract class BaseMessage : IMessage
{
    public string MessageId { get; set; } = Guid.NewGuid().ToString();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
