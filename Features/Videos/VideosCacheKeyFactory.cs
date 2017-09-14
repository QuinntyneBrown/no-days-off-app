using System;

namespace NoDaysOffApp.Features.Videos
{
    public class VideosCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[Videos] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[Videos] GetById {tenantId}-{id}";
    }
}
