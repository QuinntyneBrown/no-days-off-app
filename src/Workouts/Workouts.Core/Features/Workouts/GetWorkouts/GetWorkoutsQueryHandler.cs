using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Workouts;

namespace Workouts.Core.Features.Workouts.GetWorkouts;

public sealed class GetWorkoutsQueryHandler : IRequestHandler<GetWorkoutsQuery, IEnumerable<WorkoutDto>>
{
    private readonly IWorkoutsDbContext _context;

    public GetWorkoutsQueryHandler(IWorkoutsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkoutDto>> Handle(GetWorkoutsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Workouts.AsNoTracking().Where(w => !w.IsDeleted);

        if (request.TenantId.HasValue)
            query = query.Where(w => w.TenantId == request.TenantId.Value);

        var workouts = await query.ToListAsync(cancellationToken);
        return workouts.Select(w => w.ToDto());
    }
}
