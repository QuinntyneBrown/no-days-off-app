using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.Dashboards
{
    public interface IDashboardsEventBusMessageHandler: IEventBusMessageHandler { }

    public class DashboardsEventBusMessageHandler: IDashboardsEventBusMessageHandler
    {
        public DashboardsEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == DashboardsEventBusMessages.AddedOrUpdatedDashboardMessage)
                {
                    _cache.Remove(DashboardsCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == DashboardsEventBusMessages.RemovedDashboardMessage)
                {
                    _cache.Remove(DashboardsCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
