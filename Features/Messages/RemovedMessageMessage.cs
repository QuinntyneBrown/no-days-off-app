using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Messages
{
    public class RemovedMessageMessage : BaseEventBusMessage
    {
        public RemovedMessageMessage(int messageId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = messageId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = MessagesEventBusMessages.RemovedMessageMessage;        
    }
}
