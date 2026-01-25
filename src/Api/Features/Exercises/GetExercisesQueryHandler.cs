using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, IEnumerable<ExerciseDto>>
{
    private readonly INoDaysOffContext _context;

    public GetExercisesQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExerciseDto>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Exercises
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
