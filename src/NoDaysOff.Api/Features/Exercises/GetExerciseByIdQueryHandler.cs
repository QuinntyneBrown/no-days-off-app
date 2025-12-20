using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetExerciseByIdQueryHandler : IRequestHandler<GetExerciseByIdQuery, ExerciseDto?>
{
    private readonly INoDaysOffContext _context;

    public GetExerciseByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ExerciseDto?> Handle(GetExerciseByIdQuery request, CancellationToken cancellationToken)
    {
        var exercise = await _context.Exercises
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ExerciseId && !x.IsDeleted, cancellationToken);

        return exercise?.ToDto();
    }
}
