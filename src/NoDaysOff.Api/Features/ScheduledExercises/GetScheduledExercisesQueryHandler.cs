using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetScheduledExercisesQueryHandler : IRequestHandler<GetScheduledExercisesQuery, IEnumerable<ScheduledExerciseDto>>
{
    private readonly INoDaysOffContext _context;

    public GetScheduledExercisesQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ScheduledExerciseDto>> Handle(GetScheduledExercisesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ScheduledExercises
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
