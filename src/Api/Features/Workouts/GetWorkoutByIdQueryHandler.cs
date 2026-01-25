using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class GetWorkoutByIdQueryHandler : IRequestHandler<GetWorkoutByIdQuery, WorkoutDto?>
{
    private readonly INoDaysOffContext _context;

    public GetWorkoutByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<WorkoutDto?> Handle(GetWorkoutByIdQuery request, CancellationToken cancellationToken)
    {
        var workout = await _context.Workouts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.WorkoutId && !x.IsDeleted, cancellationToken);

        return workout?.ToDto();
    }
}
