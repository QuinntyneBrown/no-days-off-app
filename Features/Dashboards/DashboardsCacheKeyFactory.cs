using System;

namespace NoDaysOffApp.Features.Dashboards
{
    public class DashboardsCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[Dashboards] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[Dashboards] GetById {tenantId}-{id}";
    }
}
