using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Athletes
{
    public class RemovedAthleteMessage : BaseEventBusMessage
    {
        public RemovedAthleteMessage(int athleteId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = athleteId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = AthletesEventBusMessages.RemovedAthleteMessage;        
    }
}
