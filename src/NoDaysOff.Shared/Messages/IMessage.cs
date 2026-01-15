namespace NoDaysOff.Shared.Messages;

/// <summary>
/// Base interface for all messages in the system
/// </summary>
public interface IMessage
{
    string MessageId { get; set; }
    DateTime Timestamp { get; set; }
}
