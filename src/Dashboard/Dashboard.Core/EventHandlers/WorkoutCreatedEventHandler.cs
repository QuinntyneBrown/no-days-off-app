using Microsoft.EntityFrameworkCore;
using Shared.Messaging;
using Shared.Messaging.Messages.Workouts;
using Dashboard.Core.Aggregates.DashboardStats;

namespace Dashboard.Core.EventHandlers;

public class WorkoutCreatedEventHandler : IMessageHandler<WorkoutCreatedMessage>
{
    private readonly IDashboardDbContext _context;

    public WorkoutCreatedEventHandler(IDashboardDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(WorkoutCreatedMessage message, CancellationToken cancellationToken)
    {
        if (message.TenantId == null)
            return;

        var tenantId = message.TenantId.Value;

        var stats = await _context.DashboardStats
            .Where(s => s.TenantId == tenantId)
            .FirstOrDefaultAsync(cancellationToken);

        if (stats == null)
        {
            stats = new DashboardStats(tenantId, 0);
            _context.DashboardStats.Add(stats);
        }

        stats.IncrementWorkouts();
        await _context.SaveChangesAsync(cancellationToken);
    }
}
