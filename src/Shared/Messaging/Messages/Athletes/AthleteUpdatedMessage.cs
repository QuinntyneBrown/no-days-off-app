using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Athletes;

/// <summary>
/// Message sent when an athlete is updated
/// </summary>
[MessagePackObject]
public class AthleteUpdatedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int AthleteId { get; set; }

    [Key(3)]
    public string Name { get; set; } = string.Empty;

    [Key(4)]
    public string Username { get; set; } = string.Empty;

    [Key(5)]
    public int? TenantId { get; set; }
}
