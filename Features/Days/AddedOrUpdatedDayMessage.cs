using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Days
{

    public class AddedOrUpdatedDayMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedDayMessage(Day day, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = day, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = DaysEventBusMessages.AddedOrUpdatedDayMessage;        
    }
}
