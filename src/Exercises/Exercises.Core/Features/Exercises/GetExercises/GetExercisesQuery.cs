using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Exercises;

namespace Exercises.Core.Features.Exercises.GetExercises;

public record GetExercisesQuery(int TenantId, int? BodyPartId = null) : IRequest<IEnumerable<ExerciseDto>>;

public class GetExercisesHandler : IRequestHandler<GetExercisesQuery, IEnumerable<ExerciseDto>>
{
    private readonly IExercisesDbContext _context;

    public GetExercisesHandler(IExercisesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExerciseDto>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Exercises.Where(e => e.TenantId == request.TenantId);

        if (request.BodyPartId.HasValue)
        {
            query = query.Where(e => e.BodyPartId == request.BodyPartId);
        }

        return await query.Select(e => new ExerciseDto
        {
            ExerciseId = e.Id,
            Name = e.Name,
            Description = e.Description,
            TenantId = e.TenantId,
            BodyPartId = e.BodyPartId,
            VideoUrl = e.VideoUrl,
            ImageUrl = e.ImageUrl,
            Instructions = e.Instructions,
            Type = (int)e.Type
        }).ToListAsync(cancellationToken);
    }
}
