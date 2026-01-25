using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;
using Communication.Core.Aggregates.Notification;

namespace Communication.Core.Features.MarkAsRead;

public record MarkAsReadCommand(int NotificationId, int TenantId, int UserId) : IRequest<bool>;

public class MarkAsReadHandler : IRequestHandler<MarkAsReadCommand, bool>
{
    private readonly ICommunicationDbContext _context;

    public MarkAsReadHandler(ICommunicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications
            .Where(n => n.Id == request.NotificationId && n.TenantId == request.TenantId && n.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (notification == null)
            throw new NotFoundException(nameof(Notification), request.NotificationId);

        notification.MarkAsRead();
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
