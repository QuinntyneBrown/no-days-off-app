using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class GetWorkoutsQueryHandler : IRequestHandler<GetWorkoutsQuery, IEnumerable<WorkoutDto>>
{
    private readonly INoDaysOffContext _context;

    public GetWorkoutsQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkoutDto>> Handle(GetWorkoutsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Workouts
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
