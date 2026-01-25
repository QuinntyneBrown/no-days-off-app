using Shared.Domain;

namespace Communication.Core.Aggregates.Notification;

public class Notification : AggregateRoot
{
    public string Title { get; private set; } = null!;
    public string Message { get; private set; } = null!;
    public NotificationType Type { get; private set; }
    public int TenantId { get; private set; }
    public int UserId { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ReadAt { get; private set; }
    public string? ActionUrl { get; private set; }
    public string? EntityType { get; private set; }
    public int? EntityId { get; private set; }

    private Notification() { }

    public Notification(
        string title,
        string message,
        NotificationType type,
        int tenantId,
        int userId,
        string? actionUrl = null,
        string? entityType = null,
        int? entityId = null)
    {
        Title = title;
        Message = message;
        Type = type;
        TenantId = tenantId;
        UserId = userId;
        ActionUrl = actionUrl;
        EntityType = entityType;
        EntityId = entityId;
        IsRead = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
    }
}

public enum NotificationType
{
    Info = 0,
    Success = 1,
    Warning = 2,
    Error = 3,
    WorkoutAssigned = 4,
    WorkoutCompleted = 5,
    AthleteJoined = 6,
    System = 7
}
