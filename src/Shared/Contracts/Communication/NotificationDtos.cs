namespace Shared.Contracts.Communication;

public class NotificationDto
{
    public int NotificationId { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public int Type { get; set; }
    public int TenantId { get; set; }
    public int UserId { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public string? ActionUrl { get; set; }
    public string? EntityType { get; set; }
    public int? EntityId { get; set; }
}

public class CreateNotificationDto
{
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public int Type { get; set; }
    public int? UserId { get; set; }
    public string? ActionUrl { get; set; }
    public string? EntityType { get; set; }
    public int? EntityId { get; set; }
}
