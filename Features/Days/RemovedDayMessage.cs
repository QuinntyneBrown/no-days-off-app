using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Days
{
    public class RemovedDayMessage : BaseEventBusMessage
    {
        public RemovedDayMessage(int dayId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = dayId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = DaysEventBusMessages.RemovedDayMessage;        
    }
}
