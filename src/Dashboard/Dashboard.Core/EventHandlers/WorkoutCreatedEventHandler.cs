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
        var stats = await _context.DashboardStats
            .Where(s => s.TenantId == message.TenantId)
            .FirstOrDefaultAsync(cancellationToken);

        if (stats == null)
        {
            stats = new DashboardStats(message.TenantId, 0);
            _context.DashboardStats.Add(stats);
        }

        stats.IncrementWorkouts();
        await _context.SaveChangesAsync(cancellationToken);
    }
}
