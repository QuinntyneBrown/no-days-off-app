using Microsoft.EntityFrameworkCore;

namespace Communication.Core;

public interface ICommunicationDbContext
{
    DbSet<Aggregates.Notification.Notification> Notifications { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
