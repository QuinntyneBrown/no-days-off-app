using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.BodyParts
{

    public class AddedOrUpdatedBodyPartMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedBodyPartMessage(BodyPart bodyPart, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = bodyPart, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = BodyPartsEventBusMessages.AddedOrUpdatedBodyPartMessage;        
    }
}
