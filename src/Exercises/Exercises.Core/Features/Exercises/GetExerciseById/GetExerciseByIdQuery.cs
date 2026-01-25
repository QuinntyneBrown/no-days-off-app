using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Exercises;

namespace Exercises.Core.Features.Exercises.GetExerciseById;

public record GetExerciseByIdQuery(int ExerciseId, int TenantId) : IRequest<ExerciseDto?>;

public class GetExerciseByIdHandler : IRequestHandler<GetExerciseByIdQuery, ExerciseDto?>
{
    private readonly IExercisesDbContext _context;

    public GetExerciseByIdHandler(IExercisesDbContext context)
    {
        _context = context;
    }

    public async Task<ExerciseDto?> Handle(GetExerciseByIdQuery request, CancellationToken cancellationToken)
    {
        var exercise = await _context.Exercises
            .Where(e => e.Id == request.ExerciseId && e.TenantId == request.TenantId)
            .FirstOrDefaultAsync(cancellationToken);

        if (exercise == null)
            return null;

        return new ExerciseDto
        {
            ExerciseId = exercise.Id,
            Name = exercise.Name,
            Description = exercise.Description,
            TenantId = exercise.TenantId,
            BodyPartId = exercise.BodyPartId,
            VideoUrl = exercise.VideoUrl,
            ImageUrl = exercise.ImageUrl,
            Instructions = exercise.Instructions,
            Type = (int)exercise.Type
        };
    }
}
