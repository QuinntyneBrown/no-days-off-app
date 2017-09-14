using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.AthleteWeights
{
    public class RemovedAthleteWeightMessage : BaseEventBusMessage
    {
        public RemovedAthleteWeightMessage(int athleteWeightId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = athleteWeightId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = AthleteWeightsEventBusMessages.RemovedAthleteWeightMessage;        
    }
}
