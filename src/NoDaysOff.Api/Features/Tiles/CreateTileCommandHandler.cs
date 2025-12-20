using MediatR;
using NoDaysOff.Core;
using NoDaysOff.Core.Model.TileAggregate;

namespace NoDaysOff.Api;

public sealed class CreateTileCommandHandler : IRequestHandler<CreateTileCommand, TileDto>
{
    private readonly INoDaysOffContext _context;

    public CreateTileCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<TileDto> Handle(CreateTileCommand request, CancellationToken cancellationToken)
    {
        var tile = Tile.Create(
            request.TenantId,
            request.Name,
            request.DefaultWidth,
            request.DefaultHeight,
            request.CreatedBy);

        _context.Tiles.Add(tile);

        await _context.SaveChangesAsync(cancellationToken);

        return tile.ToDto();
    }
}
