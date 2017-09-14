using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.Profiles
{
    public interface IProfilesEventBusMessageHandler: IEventBusMessageHandler { }

    public class ProfilesEventBusMessageHandler: IProfilesEventBusMessageHandler
    {
        public ProfilesEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == ProfilesEventBusMessages.AddedOrUpdatedProfileMessage)
                {
                    _cache.Remove(ProfilesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == ProfilesEventBusMessages.RemovedProfileMessage)
                {
                    _cache.Remove(ProfilesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
