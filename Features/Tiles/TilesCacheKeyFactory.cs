using System;

namespace NoDaysOffApp.Features.Tiles
{
    public class TilesCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[Tiles] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[Tiles] GetById {tenantId}-{id}";
    }
}
