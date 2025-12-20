using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteTileCommandHandler : IRequestHandler<DeleteTileCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteTileCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTileCommand request, CancellationToken cancellationToken)
    {
        var tile = await _context.Tiles
            .FirstOrDefaultAsync(x => x.Id == request.TileId, cancellationToken)
            ?? throw new InvalidOperationException($"Tile with id {request.TileId} not found.");

        tile.Delete(request.DeletedBy);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
