using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.BoundedContexts
{
    public class RemovedBoundedContextMessage : BaseEventBusMessage
    {
        public RemovedBoundedContextMessage(int boundedContextId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = boundedContextId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = BoundedContextsEventBusMessages.RemovedBoundedContextMessage;        
    }
}
