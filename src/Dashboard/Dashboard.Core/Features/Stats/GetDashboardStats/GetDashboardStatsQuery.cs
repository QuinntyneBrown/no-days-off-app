using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Dashboard;

namespace Dashboard.Core.Features.Stats.GetDashboardStats;

public record GetDashboardStatsQuery(int TenantId, int UserId) : IRequest<DashboardStatsDto?>;

public class GetDashboardStatsHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto?>
{
    private readonly IDashboardDbContext _context;

    public GetDashboardStatsHandler(IDashboardDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsDto?> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var stats = await _context.DashboardStats
            .Where(s => s.TenantId == request.TenantId && s.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (stats == null)
            return null;

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
