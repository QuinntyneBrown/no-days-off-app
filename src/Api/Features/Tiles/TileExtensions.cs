using Core.Model.TileAggregate;

namespace Api;

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
