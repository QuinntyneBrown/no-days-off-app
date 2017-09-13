using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Dashboards
{

    public class AddedOrUpdatedDashboardMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedDashboardMessage(Dashboard dashboard, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = dashboard, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = DashboardsEventBusMessages.AddedOrUpdatedDashboardMessage;        
    }
}
