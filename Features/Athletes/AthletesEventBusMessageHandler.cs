using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.Athletes
{
    public interface IAthletesEventBusMessageHandler: IEventBusMessageHandler { }

    public class AthletesEventBusMessageHandler: IAthletesEventBusMessageHandler
    {
        public AthletesEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == AthletesEventBusMessages.AddedOrUpdatedAthleteMessage)
                {
                    _cache.Remove(AthletesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == AthletesEventBusMessages.RemovedAthleteMessage)
                {
                    _cache.Remove(AthletesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
