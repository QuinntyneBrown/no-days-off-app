using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.DashboardTiles
{
    public interface IDashboardTilesEventBusMessageHandler: IEventBusMessageHandler { }

    public class DashboardTilesEventBusMessageHandler: IDashboardTilesEventBusMessageHandler
    {
        public DashboardTilesEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == DashboardTilesEventBusMessages.AddedOrUpdatedDashboardTileMessage)
                {
                    _cache.Remove(DashboardTilesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == DashboardTilesEventBusMessages.RemovedDashboardTileMessage)
                {
                    _cache.Remove(DashboardTilesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
