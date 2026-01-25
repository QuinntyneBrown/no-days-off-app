using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Exercises;

/// <summary>
/// Message sent when a body part is created
/// </summary>
[MessagePackObject]
public class BodyPartCreatedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int BodyPartId { get; set; }

    [Key(3)]
    public string Name { get; set; } = string.Empty;

    [Key(4)]
    public int? TenantId { get; set; }
}
