using System;

namespace NoDaysOffApp.Features.Exercises
{
    public class ExercisesCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[Exercises] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[Exercises] GetById {tenantId}-{id}";
    }
}
