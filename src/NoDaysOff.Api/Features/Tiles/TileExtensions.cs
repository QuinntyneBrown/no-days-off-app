using NoDaysOff.Core.Model.TileAggregate;

namespace NoDaysOff.Api;

public static class TileExtensions
{
    public static TileDto ToDto(this Tile tile)
    {
        return new TileDto(
            tile.Id,
            tile.Name,
            tile.DefaultHeight,
            tile.DefaultWidth,
            tile.IsVisibleInCatalog,
            tile.CreatedOn,
            tile.CreatedBy);
    }
}
