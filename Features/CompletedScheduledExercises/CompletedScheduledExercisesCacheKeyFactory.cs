using System;

namespace NoDaysOffApp.Features.CompletedScheduledExercises
{
    public class CompletedScheduledExercisesCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[CompletedScheduledExercises] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[CompletedScheduledExercises] GetById {tenantId}-{id}";
    }
}
