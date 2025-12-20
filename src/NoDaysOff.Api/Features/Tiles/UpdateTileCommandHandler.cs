using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class UpdateTileCommandHandler : IRequestHandler<UpdateTileCommand, TileDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateTileCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<TileDto> Handle(UpdateTileCommand request, CancellationToken cancellationToken)
    {
        var tile = await _context.Tiles
            .FirstOrDefaultAsync(x => x.Id == request.TileId, cancellationToken)
            ?? throw new InvalidOperationException($"Tile with id {request.TileId} not found.");

        tile.UpdateName(request.Name, request.ModifiedBy);
        tile.UpdateDefaultDimensions(request.DefaultWidth, request.DefaultHeight, request.ModifiedBy);

        await _context.SaveChangesAsync(cancellationToken);

        return tile.ToDto();
    }
}
