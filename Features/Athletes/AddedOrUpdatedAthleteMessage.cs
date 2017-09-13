using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Athletes
{

    public class AddedOrUpdatedAthleteMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedAthleteMessage(Athlete athlete, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = athlete, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = AthletesEventBusMessages.AddedOrUpdatedAthleteMessage;        
    }
}
