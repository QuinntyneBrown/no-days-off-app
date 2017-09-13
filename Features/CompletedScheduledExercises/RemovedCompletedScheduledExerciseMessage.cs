using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.CompletedScheduledExercises
{
    public class RemovedCompletedScheduledExerciseMessage : BaseEventBusMessage
    {
        public RemovedCompletedScheduledExerciseMessage(int completedScheduledExerciseId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = completedScheduledExerciseId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = CompletedScheduledExercisesEventBusMessages.RemovedCompletedScheduledExerciseMessage;        
    }
}
