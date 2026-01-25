using Microsoft.EntityFrameworkCore;

namespace Dashboard.Core;

public interface IDashboardDbContext
{
    DbSet<Aggregates.Widget.Widget> Widgets { get; }
    DbSet<Aggregates.DashboardStats.DashboardStats> DashboardStats { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
