using NoDaysOffApp.Features.BodyParts;
using NoDaysOffApp.Features.CompletedScheduledExercises;
using NoDaysOffApp.Features.Core;
using NoDaysOffApp.Features.Dashboards;
using NoDaysOffApp.Features.DashboardTiles;
using NoDaysOffApp.Features.Days;
using NoDaysOffApp.Features.Exercises;
using NoDaysOffApp.Features.ScheduledExercises;
using NoDaysOffApp.Features.Tiles;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Web.Http;
using Unity.WebApi;

using static Newtonsoft.Json.JsonConvert;
using NoDaysOffApp.Features.Athletes;

[assembly: OwinStartup(typeof(NoDaysOffApp.Startup))]

namespace NoDaysOffApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(config =>
            {
                var container = UnityConfiguration.GetContainer();
                config.DependencyResolver = new UnityDependencyResolver(container);
                ApiConfiguration.Install(config, app);
                
                var client = SubscriptionClient.CreateFromConnectionString(CoreConfiguration.Config.EventQueueConnectionString, CoreConfiguration.Config.TopicName, CoreConfiguration.Config.SubscriptionName);

                var athletesEventBusMessageHandler = container.Resolve<IAthletesEventBusMessageHandler>();
                var bodyPartsEventBusMessageHandler = container.Resolve<IBodyPartsEventBusMessageHandler>();
                var completedScheduledExercisesEventBusMessageHandler = container.Resolve<ICompletedScheduledExercisesEventBusMessageHandler>();
                var daysEventBusMessageHandler = container.Resolve<IDaysEventBusMessageHandler>();
                var exercisesEventBusMessageHandler = container.Resolve<IExercisesEventBusMessageHandler>();
                var scheduledExercisesEventBusMessageHandler = container.Resolve<IScheduledExercisesEventBusMessageHandler>();
                var dashboardsEventBusMessageHandler = container.Resolve<IDashboardsEventBusMessageHandler>();
                var dashboardTilesEventBusMessageHandler = container.Resolve<IDashboardTilesEventBusMessageHandler>();
                var tilesEventBusMessageHandler = container.Resolve<ITilesEventBusMessageHandler>();

                client.OnMessage(message =>
                {
                    var messageBody = ((BrokeredMessage)message).GetBody<string>();
                    var messageBodyObject = DeserializeObject<JObject>(messageBody, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                        TypeNameHandling = TypeNameHandling.All,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                    athletesEventBusMessageHandler.Handle(messageBodyObject);
                    dashboardsEventBusMessageHandler.Handle(messageBodyObject);
                    dashboardTilesEventBusMessageHandler.Handle(messageBodyObject);
                    tilesEventBusMessageHandler.Handle(messageBodyObject);
                    bodyPartsEventBusMessageHandler.Handle(messageBodyObject);
                    completedScheduledExercisesEventBusMessageHandler.Handle(messageBodyObject);
                    daysEventBusMessageHandler.Handle(messageBodyObject);
                    exercisesEventBusMessageHandler.Handle(messageBodyObject);
                    scheduledExercisesEventBusMessageHandler.Handle(messageBodyObject);
                    GlobalHost.ConnectionManager.GetHubContext<EventHub>().Clients.All.events(messageBodyObject);
                });
            });
        }
    }
}