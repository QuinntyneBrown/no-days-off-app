using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Exercises
{

    public class AddedOrUpdatedExerciseMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedExerciseMessage(Exercise exercise, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = exercise, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = ExercisesEventBusMessages.AddedOrUpdatedExerciseMessage;        
    }
}
