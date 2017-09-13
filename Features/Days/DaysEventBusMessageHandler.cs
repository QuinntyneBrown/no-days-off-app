using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.Days
{
    public interface IDaysEventBusMessageHandler: IEventBusMessageHandler { }

    public class DaysEventBusMessageHandler: IDaysEventBusMessageHandler
    {
        public DaysEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == DaysEventBusMessages.AddedOrUpdatedDayMessage)
                {
                    _cache.Remove(DaysCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == DaysEventBusMessages.RemovedDayMessage)
                {
                    _cache.Remove(DaysCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
