using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.Messages
{
    public interface IMessagesEventBusMessageHandler: IEventBusMessageHandler { }

    public class MessagesEventBusMessageHandler: IMessagesEventBusMessageHandler
    {
        public MessagesEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == MessagesEventBusMessages.AddedOrUpdatedMessageMessage)
                {
                    _cache.Remove(MessagesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == MessagesEventBusMessages.RemovedMessageMessage)
                {
                    _cache.Remove(MessagesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
