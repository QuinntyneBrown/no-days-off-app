using System;

namespace NoDaysOffApp.Features.Messages
{
    public class MessagesCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[Messages] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[Messages] GetById {tenantId}-{id}";
    }
}
