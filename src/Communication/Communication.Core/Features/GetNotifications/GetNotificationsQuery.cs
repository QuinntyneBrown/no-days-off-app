using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Communication;

namespace Communication.Core.Features.GetNotifications;

public record GetNotificationsQuery(int TenantId, int UserId, bool UnreadOnly = false) : IRequest<IEnumerable<NotificationDto>>;

public class GetNotificationsHandler : IRequestHandler<GetNotificationsQuery, IEnumerable<NotificationDto>>
{
    private readonly ICommunicationDbContext _context;

    public GetNotificationsHandler(ICommunicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Notifications
            .Where(n => n.TenantId == request.TenantId && n.UserId == request.UserId);

        if (request.UnreadOnly)
            query = query.Where(n => !n.IsRead);

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .Select(n => new NotificationDto
            {
                NotificationId = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = (int)n.Type,
                TenantId = n.TenantId,
                UserId = n.UserId,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                ReadAt = n.ReadAt,
                ActionUrl = n.ActionUrl,
                EntityType = n.EntityType,
                EntityId = n.EntityId
            })
            .ToListAsync(cancellationToken);
    }
}
