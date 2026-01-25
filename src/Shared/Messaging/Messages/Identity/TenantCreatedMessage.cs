using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Identity;

/// <summary>
/// Message sent when a tenant is created
/// </summary>
[MessagePackObject]
public class TenantCreatedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int TenantId { get; set; }

    [Key(3)]
    public Guid UniqueId { get; set; }

    [Key(4)]
    public string Name { get; set; } = string.Empty;
}
