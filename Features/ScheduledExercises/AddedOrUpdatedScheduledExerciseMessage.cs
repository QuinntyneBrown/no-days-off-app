using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.ScheduledExercises
{

    public class AddedOrUpdatedScheduledExerciseMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedScheduledExerciseMessage(ScheduledExercise scheduledExercise, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = scheduledExercise, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = ScheduledExercisesEventBusMessages.AddedOrUpdatedScheduledExerciseMessage;        
    }
}
