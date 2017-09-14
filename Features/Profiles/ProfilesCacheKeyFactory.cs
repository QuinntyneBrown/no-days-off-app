using System;

namespace NoDaysOffApp.Features.Profiles
{
    public class ProfilesCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[Profiles] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[Profiles] GetById {tenantId}-{id}";
    }
}
