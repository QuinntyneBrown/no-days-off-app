using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetBodyPartsQueryHandler : IRequestHandler<GetBodyPartsQuery, IEnumerable<BodyPartDto>>
{
    private readonly INoDaysOffContext _context;

    public GetBodyPartsQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BodyPartDto>> Handle(GetBodyPartsQuery request, CancellationToken cancellationToken)
    {
        return await _context.BodyParts
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
