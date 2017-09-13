using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.BodyParts
{
    public interface IBodyPartsEventBusMessageHandler: IEventBusMessageHandler { }

    public class BodyPartsEventBusMessageHandler: IBodyPartsEventBusMessageHandler
    {
        public BodyPartsEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == BodyPartsEventBusMessages.AddedOrUpdatedBodyPartMessage)
                {
                    _cache.Remove(BodyPartsCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == BodyPartsEventBusMessages.RemovedBodyPartMessage)
                {
                    _cache.Remove(BodyPartsCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
