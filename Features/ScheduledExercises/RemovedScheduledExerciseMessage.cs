using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.ScheduledExercises
{
    public class RemovedScheduledExerciseMessage : BaseEventBusMessage
    {
        public RemovedScheduledExerciseMessage(int scheduledExerciseId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = scheduledExerciseId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = ScheduledExercisesEventBusMessages.RemovedScheduledExerciseMessage;        
    }
}
