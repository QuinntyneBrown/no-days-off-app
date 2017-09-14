using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.BoundedContexts
{
    public interface IBoundedContextsEventBusMessageHandler: IEventBusMessageHandler { }

    public class BoundedContextsEventBusMessageHandler: IBoundedContextsEventBusMessageHandler
    {
        public BoundedContextsEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["Type"]}" == BoundedContextsEventBusMessages.AddedOrUpdatedBoundedContextMessage)
                {
                    _cache.Remove(BoundedContextsCacheKeyFactory.Get(new Guid(message["TenantUniqueId"].ToString())));
                }

                if ($"{message["Type"]}" == BoundedContextsEventBusMessages.RemovedBoundedContextMessage)
                {
                    _cache.Remove(BoundedContextsCacheKeyFactory.Get(new Guid(message["TenantUniqueId"].ToString())));
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
