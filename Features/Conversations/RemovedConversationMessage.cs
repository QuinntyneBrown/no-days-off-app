using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Conversations
{
    public class RemovedConversationMessage : BaseEventBusMessage
    {
        public RemovedConversationMessage(int conversationId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = conversationId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = ConversationsEventBusMessages.RemovedConversationMessage;        
    }
}
