using System;

namespace NoDaysOffApp.Features.AthleteWeights
{
    public class AthleteWeightsCacheKeyFactory
    {
        public static string Get(Guid tenantId) => $"[AthleteWeights] Get {tenantId}";
        public static string GetById(Guid tenantId, int id) => $"[AthleteWeights] GetById {tenantId}-{id}";
    }
}
