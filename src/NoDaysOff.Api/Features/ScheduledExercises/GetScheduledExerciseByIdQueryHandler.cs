using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetScheduledExerciseByIdQueryHandler : IRequestHandler<GetScheduledExerciseByIdQuery, ScheduledExerciseDto?>
{
    private readonly INoDaysOffContext _context;

    public GetScheduledExerciseByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ScheduledExerciseDto?> Handle(GetScheduledExerciseByIdQuery request, CancellationToken cancellationToken)
    {
        var scheduledExercise = await _context.ScheduledExercises
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ScheduledExerciseId && !x.IsDeleted, cancellationToken);

        return scheduledExercise?.ToDto();
    }
}
