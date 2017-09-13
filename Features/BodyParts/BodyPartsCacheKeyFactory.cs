using System;

namespace NoDaysOffApp.Features.BodyParts
{
    public class BodyPartsCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[BodyParts] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[BodyParts] GetById {tenantId}-{id}";
    }
}
