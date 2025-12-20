using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetTileByIdQueryHandler : IRequestHandler<GetTileByIdQuery, TileDto?>
{
    private readonly INoDaysOffContext _context;

    public GetTileByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<TileDto?> Handle(GetTileByIdQuery request, CancellationToken cancellationToken)
    {
        var tile = await _context.Tiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.TileId && !x.IsDeleted, cancellationToken);

        return tile?.ToDto();
    }
}
