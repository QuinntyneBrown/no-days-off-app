using System;

namespace NoDaysOffApp.Features.DashboardTiles
{
    public class DashboardTilesCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[DashboardTiles] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[DashboardTiles] GetById {tenantId}-{id}";
    }
}
