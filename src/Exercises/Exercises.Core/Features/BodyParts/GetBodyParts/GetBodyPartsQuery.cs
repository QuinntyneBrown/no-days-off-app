using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Exercises;

namespace Exercises.Core.Features.BodyParts.GetBodyParts;

public record GetBodyPartsQuery(int TenantId) : IRequest<IEnumerable<BodyPartDto>>;

public class GetBodyPartsHandler : IRequestHandler<GetBodyPartsQuery, IEnumerable<BodyPartDto>>
{
    private readonly IExercisesDbContext _context;

    public GetBodyPartsHandler(IExercisesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BodyPartDto>> Handle(GetBodyPartsQuery request, CancellationToken cancellationToken)
    {
        return await _context.BodyParts
            .Where(b => b.TenantId == request.TenantId)
            .Select(b => new BodyPartDto
            {
                BodyPartId = b.Id,
                Name = b.Name,
                Description = b.Description,
                TenantId = b.TenantId
            })
            .ToListAsync(cancellationToken);
    }
}
