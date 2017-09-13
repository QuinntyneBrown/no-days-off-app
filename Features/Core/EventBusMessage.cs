using System;

namespace NoDaysOffApp.Features.Core
{
    public interface IEventBusMessage
    {
        string Type { get; set; }
        dynamic Payload { get; set; }
        Guid TenantUniqueId { get; set; }
    }

    public class EventBusMessage: IEventBusMessage {
        public string Type { get; set; }
        public dynamic Payload { get; set; }
        public Guid TenantUniqueId { get; set; }
    }
}