using Microsoft.AspNetCore.SignalR;
using Communication.Core.Aggregates.Notification;
using Communication.Core.Services;
using Communication.Infrastructure.Hubs;

namespace Communication.Infrastructure.Services;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

    public SignalRNotificationService(IHubContext<NotificationHub, INotificationClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendToUserAsync(int userId, string title, string message, NotificationType type, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.Group($"user-{userId}")
            .ReceiveNotification(new NotificationMessage(title, message, (int)type));
    }

    public async Task SendToTenantAsync(int tenantId, string title, string message, NotificationType type, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.Group($"tenant-{tenantId}")
            .ReceiveNotification(new NotificationMessage(title, message, (int)type));
    }

    public async Task SendToAllAsync(string title, string message, NotificationType type, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.All
            .ReceiveNotification(new NotificationMessage(title, message, (int)type));
    }
}

public record NotificationMessage(string Title, string Message, int Type);
