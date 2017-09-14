using System;

namespace NoDaysOffApp.Features.Conversations
{
    public class ConversationsCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[Conversations] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[Conversations] GetById {tenantId}-{id}";
    }
}
