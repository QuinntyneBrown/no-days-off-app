using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.Tiles
{
    public class TileApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public bool IsVisibleInCatalog { get; set; }
        public static TModel FromTile<TModel>(Tile tile) where
            TModel : TileApiModel, new()
        {
            var model = new TModel();
            model.Id = tile.Id;
            model.TenantId = tile.TenantId;
            model.Name = tile.Name;
            model.IsVisibleInCatalog = tile.IsVisibleInCatalog;
            return model;
        }

        public static TileApiModel FromTile(Tile tile)
            => FromTile<TileApiModel>(tile);

    }
}
