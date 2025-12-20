using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetTilesQueryHandler : IRequestHandler<GetTilesQuery, IEnumerable<TileDto>>
{
    private readonly INoDaysOffContext _context;

    public GetTilesQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TileDto>> Handle(GetTilesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tiles
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
