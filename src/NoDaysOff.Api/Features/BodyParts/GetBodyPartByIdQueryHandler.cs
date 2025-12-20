using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetBodyPartByIdQueryHandler : IRequestHandler<GetBodyPartByIdQuery, BodyPartDto?>
{
    private readonly INoDaysOffContext _context;

    public GetBodyPartByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<BodyPartDto?> Handle(GetBodyPartByIdQuery request, CancellationToken cancellationToken)
    {
        var bodyPart = await _context.BodyParts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.BodyPartId && !x.IsDeleted, cancellationToken);

        return bodyPart?.ToDto();
    }
}
