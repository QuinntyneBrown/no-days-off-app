using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Identity;

/// <summary>
/// Message sent when a user is created/registered
/// </summary>
[MessagePackObject]
public class UserCreatedMessage : IMessage
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
    public string FirstName { get; set; } = string.Empty;

    [Key(5)]
    public string LastName { get; set; } = string.Empty;

    [Key(6)]
    public int? TenantId { get; set; }
}
