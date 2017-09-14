using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.Conversations
{
    public interface IConversationsEventBusMessageHandler: IEventBusMessageHandler { }

    public class ConversationsEventBusMessageHandler: IConversationsEventBusMessageHandler
    {
        public ConversationsEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == ConversationsEventBusMessages.AddedOrUpdatedConversationMessage)
                {
                    _cache.Remove(ConversationsCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == ConversationsEventBusMessages.RemovedConversationMessage)
                {
                    _cache.Remove(ConversationsCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
