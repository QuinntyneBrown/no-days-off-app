using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Athletes;

/// <summary>
/// Message sent when an athlete is deleted
/// </summary>
[MessagePackObject]
public class AthleteDeletedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int AthleteId { get; set; }

    [Key(3)]
    public int? TenantId { get; set; }

    [Key(4)]
    public string DeletedBy { get; set; } = string.Empty;
}
