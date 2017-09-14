using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Conversations
{

    public class AddedOrUpdatedConversationMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedConversationMessage(Conversation conversation, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = conversation, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = ConversationsEventBusMessages.AddedOrUpdatedConversationMessage;        
    }
}
