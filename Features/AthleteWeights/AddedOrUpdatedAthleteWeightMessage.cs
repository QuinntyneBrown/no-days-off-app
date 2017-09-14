using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.AthleteWeights
{

    public class AddedOrUpdatedAthleteWeightMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedAthleteWeightMessage(dynamic athleteWeight, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = athleteWeight, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = AthleteWeightsEventBusMessages.AddedOrUpdatedAthleteWeightMessage;        
    }
}
