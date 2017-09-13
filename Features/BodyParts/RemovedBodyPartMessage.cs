using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.BodyParts
{
    public class RemovedBodyPartMessage : BaseEventBusMessage
    {
        public RemovedBodyPartMessage(int bodyPartId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = bodyPartId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = BodyPartsEventBusMessages.RemovedBodyPartMessage;        
    }
}
