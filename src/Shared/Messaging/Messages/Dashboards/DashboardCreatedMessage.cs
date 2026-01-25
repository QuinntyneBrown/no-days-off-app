using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Dashboards;

/// <summary>
/// Message sent when a dashboard is created
/// </summary>
[MessagePackObject]
public class DashboardCreatedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int DashboardId { get; set; }

    [Key(3)]
    public string Name { get; set; } = string.Empty;

    [Key(4)]
    public string Username { get; set; } = string.Empty;

    [Key(5)]
    public bool IsDefault { get; set; }

    [Key(6)]
    public int? TenantId { get; set; }
}
