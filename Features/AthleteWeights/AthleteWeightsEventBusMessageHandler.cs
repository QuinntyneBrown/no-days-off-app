using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.AthleteWeights
{
    public interface IAthleteWeightsEventBusMessageHandler: IEventBusMessageHandler { }

    public class AthleteWeightsEventBusMessageHandler: IAthleteWeightsEventBusMessageHandler
    {
        public AthleteWeightsEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == AthleteWeightsEventBusMessages.AddedOrUpdatedAthleteWeightMessage)
                {
                    _cache.Remove(AthleteWeightsCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == AthleteWeightsEventBusMessages.RemovedAthleteWeightMessage)
                {
                    _cache.Remove(AthleteWeightsCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private readonly ICache _cache;
    }
}
