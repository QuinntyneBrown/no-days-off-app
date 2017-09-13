using NoDaysOffApp.Features.Tiles;
using NoDaysOffApp.Model;

namespace NoDaysOffApp.Features.DashboardTiles
{
    public class DashboardTileApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public int? DashboardId { get; set; }
        public int? TileId { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public TileApiModel Tile { get; set; }

        public static TModel FromDashboardTile<TModel>(DashboardTile dashboardTile) where
            TModel : DashboardTileApiModel, new()
        {
            var model = new TModel();
            model.Id = dashboardTile.Id;
            model.TenantId = dashboardTile.TenantId;
            model.Name = dashboardTile.Name;
            model.DashboardId = dashboardTile.DashboardId;
            model.TileId = dashboardTile.TileId;
            model.Top = dashboardTile.Top;
            model.Left = dashboardTile.Left;
            model.Width = dashboardTile.Width;
            model.Height = dashboardTile.Height;
            model.Tile = TileApiModel.FromTile(dashboardTile.Tile);
            return model;
        }

        public static DashboardTileApiModel FromDashboardTile(DashboardTile dashboardTile)
            => FromDashboardTile<DashboardTileApiModel>(dashboardTile);

    }
}
