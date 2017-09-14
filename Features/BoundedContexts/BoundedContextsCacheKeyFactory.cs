using System;

namespace NoDaysOffApp.Features.BoundedContexts
{
    public class BoundedContextsCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[BoundedContexts] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[BoundedContexts] GetById {tenantId}-{id}";
    }
}
