using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Messages
{

    public class AddedOrUpdatedMessageMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedMessageMessage(Message message, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = message, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = MessagesEventBusMessages.AddedOrUpdatedMessageMessage;        
    }
}
