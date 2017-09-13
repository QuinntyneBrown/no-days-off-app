using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.CompletedScheduledExercises
{

    public class AddedOrUpdatedCompletedScheduledExerciseMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedCompletedScheduledExerciseMessage(CompletedScheduledExercise completedScheduledExercise, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = completedScheduledExercise, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = CompletedScheduledExercisesEventBusMessages.AddedOrUpdatedCompletedScheduledExerciseMessage;        
    }
}
