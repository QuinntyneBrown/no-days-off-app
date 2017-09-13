using System;

namespace NoDaysOffApp.Features.Athletes
{
    public class AthletesCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[Athletes] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[Athletes] GetById {tenantId}-{id}";
    }
}
