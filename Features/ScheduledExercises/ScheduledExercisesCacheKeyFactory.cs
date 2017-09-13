using System;

namespace NoDaysOffApp.Features.ScheduledExercises
{
    public class ScheduledExercisesCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[ScheduledExercises] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[ScheduledExercises] GetById {tenantId}-{id}";
    }
}
