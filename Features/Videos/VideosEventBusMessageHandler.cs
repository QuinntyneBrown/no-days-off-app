using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.Videos
{
    public interface IVideosEventBusMessageHandler: IEventBusMessageHandler { }

    public class VideosEventBusMessageHandler: IVideosEventBusMessageHandler
    {
        public VideosEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == VideosEventBusMessages.AddedOrUpdatedVideoMessage)
                {
                    _cache.Remove(VideosCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == VideosEventBusMessages.RemovedVideoMessage)
                {
                    _cache.Remove(VideosCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
