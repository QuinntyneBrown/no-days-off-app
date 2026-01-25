using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Dashboard;
using Dashboard.Core.Aggregates.DashboardStats;

namespace Dashboard.Core.Features.Stats.UpdateDashboardStats;

public record UpdateDashboardStatsCommand(
    int TenantId,
    int UserId,
    int TotalWorkouts,
    int TotalExercises,
    int TotalAthletes,
    int WorkoutsThisWeek,
    int WorkoutsThisMonth) : IRequest<DashboardStatsDto>;

public class UpdateDashboardStatsHandler : IRequestHandler<UpdateDashboardStatsCommand, DashboardStatsDto>
{
    private readonly IDashboardDbContext _context;

    public UpdateDashboardStatsHandler(IDashboardDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsDto> Handle(UpdateDashboardStatsCommand request, CancellationToken cancellationToken)
    {
        var stats = await _context.DashboardStats
            .Where(s => s.TenantId == request.TenantId && s.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (stats == null)
        {
            stats = new DashboardStats(request.TenantId, request.UserId);
            _context.DashboardStats.Add(stats);
        }

        stats.UpdateStats(
            request.TotalWorkouts,
            request.TotalExercises,
            request.TotalAthletes,
            request.WorkoutsThisWeek,
            request.WorkoutsThisMonth);

        await _context.SaveChangesAsync(cancellationToken);

        return new DashboardStatsDto
        {
            TenantId = stats.TenantId,
            UserId = stats.UserId,
            TotalWorkouts = stats.TotalWorkouts,
            TotalExercises = stats.TotalExercises,
            TotalAthletes = stats.TotalAthletes,
            WorkoutsThisWeek = stats.WorkoutsThisWeek,
            WorkoutsThisMonth = stats.WorkoutsThisMonth,
            LastUpdated = stats.LastUpdated
        };
    }
}
