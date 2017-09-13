using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Exercises
{
    public class RemovedExerciseMessage : BaseEventBusMessage
    {
        public RemovedExerciseMessage(int exerciseId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = exerciseId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = ExercisesEventBusMessages.RemovedExerciseMessage;        
    }
}
