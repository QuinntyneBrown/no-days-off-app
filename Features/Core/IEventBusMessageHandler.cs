using Newtonsoft.Json.Linq;

namespace NoDaysOffApp.Features.Core
{
    public interface IEventBusMessageHandler
    {
        void Handle(JObject message);
    }
}