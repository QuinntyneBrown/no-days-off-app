using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Communication.Core.Features.MarkAllAsRead;

public record MarkAllAsReadCommand(int TenantId, int UserId) : IRequest<int>;

public class MarkAllAsReadHandler : IRequestHandler<MarkAllAsReadCommand, int>
{
    private readonly ICommunicationDbContext _context;

    public MarkAllAsReadHandler(ICommunicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications
            .Where(n => n.TenantId == request.TenantId && n.UserId == request.UserId && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in notifications)
        {
            notification.MarkAsRead();
        }

        await _context.SaveChangesAsync(cancellationToken);
        return notifications.Count;
    }
}
