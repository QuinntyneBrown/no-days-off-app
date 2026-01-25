using Communication.Core.Aggregates.Notification;

namespace Communication.Core.Services;

public interface INotificationService
{
    Task SendToUserAsync(int userId, string title, string message, NotificationType type, CancellationToken cancellationToken = default);
    Task SendToTenantAsync(int tenantId, string title, string message, NotificationType type, CancellationToken cancellationToken = default);
    Task SendToAllAsync(string title, string message, NotificationType type, CancellationToken cancellationToken = default);
}
