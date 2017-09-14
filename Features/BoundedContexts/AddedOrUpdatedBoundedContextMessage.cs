using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.BoundedContexts
{

    public class AddedOrUpdatedBoundedContextMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedBoundedContextMessage(BoundedContext boundedContext, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = boundedContext, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = BoundedContextsEventBusMessages.AddedOrUpdatedBoundedContextMessage;        
    }
}
