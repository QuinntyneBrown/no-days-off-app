using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace NoDaysOffApp.Features.Core
{
    public interface IEventBus {
        void Publish(IEventBusMessage message);
    }

    public class EventBus: IEventBus
    {
        public EventBus()
        {
            _topicClient = TopicClient.CreateFromConnectionString(CoreConfiguration.Config.EventQueueConnectionString);
        }

        private static IEventBus _instance;

        public static IEventBus Instance { get {
                return _instance; 
            }
        }

        public void Publish(IEventBusMessage message) {
            _topicClient
                .SendAsync(new BrokeredMessage(Newtonsoft.Json.JsonConvert.SerializeObject(message, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                })))
                .GetAwaiter()
                .GetResult();
        }

        private TopicClient _topicClient;
        private SubscriptionClient _subscriptionClient;
    }

    public interface ISubscriptionManager {
        void AddSubscription<T, TH>();
    }

    public class SubscriptionManager : ISubscriptionManager
    {
        public void AddSubscription<T, TH>()
        {
            throw new NotImplementedException();
        }

        public void RemoveSubscription<T, TH>()
        {
            throw new NotImplementedException();
        }
    }
}