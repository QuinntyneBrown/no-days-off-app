using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.Tiles
{
    public interface ITilesEventBusMessageHandler: IEventBusMessageHandler { }

    public class TilesEventBusMessageHandler: ITilesEventBusMessageHandler
    {
        public TilesEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == TilesEventBusMessages.AddedOrUpdatedTileMessage)
                {
                    _cache.Remove(TilesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == TilesEventBusMessages.RemovedTileMessage)
                {
                    _cache.Remove(TilesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
