using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Communication;

/// <summary>
/// Message sent when a chat message is sent
/// </summary>
[MessagePackObject]
public class MessageSentMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int ConversationId { get; set; }

    [Key(3)]
    public int ChatMessageId { get; set; }

    [Key(4)]
    public string SenderId { get; set; } = string.Empty;

    [Key(5)]
    public string Content { get; set; } = string.Empty;

    [Key(6)]
    public int? TenantId { get; set; }
}
