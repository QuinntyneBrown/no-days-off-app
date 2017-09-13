using Microsoft.AspNet.SignalR.Hubs;

namespace NoDaysOffApp.Features.Core
{
    [HubName("eventHub")]
    public class EventHub: BaseHub  { }
}