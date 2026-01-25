using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Identity;

/// <summary>
/// Message sent when a user logs in
/// </summary>
[MessagePackObject]
public class UserAuthenticatedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int UserId { get; set; }

    [Key(3)]
    public string Email { get; set; } = string.Empty;

    [Key(4)]
    public int? TenantId { get; set; }

    [Key(5)]
    public string IpAddress { get; set; } = string.Empty;
}
