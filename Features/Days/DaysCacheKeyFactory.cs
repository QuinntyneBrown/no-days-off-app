using System;

namespace NoDaysOffApp.Features.Days
{
    public class DaysCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[Days] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[Days] GetById {tenantId}-{id}";
    }
}
