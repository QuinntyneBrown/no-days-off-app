using MediatR;
using Shared.Contracts.Communication;
using Communication.Core.Aggregates.Notification;
using Communication.Core.Services;

namespace Communication.Core.Features.CreateNotification;

public record CreateNotificationCommand(
    string Title,
    string Message,
    int Type,
    int TenantId,
    int UserId,
    string? ActionUrl = null,
    string? EntityType = null,
    int? EntityId = null) : IRequest<NotificationDto>;

public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand, NotificationDto>
{
    private readonly ICommunicationDbContext _context;
    private readonly INotificationService _notificationService;

    public CreateNotificationHandler(ICommunicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification(
            request.Title,
            request.Message,
            (NotificationType)request.Type,
            request.TenantId,
            request.UserId,
            request.ActionUrl,
            request.EntityType,
            request.EntityId);

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.SendToUserAsync(
            request.UserId,
            request.Title,
            request.Message,
            (NotificationType)request.Type,
            cancellationToken);

        return new NotificationDto
        {
            NotificationId = notification.Id,
            Title = notification.Title,
            Message = notification.Message,
            Type = (int)notification.Type,
            TenantId = notification.TenantId,
            UserId = notification.UserId,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt,
            ActionUrl = notification.ActionUrl,
            EntityType = notification.EntityType,
            EntityId = notification.EntityId
        };
    }
}
