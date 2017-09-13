using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Dashboards
{
    public class RemovedDashboardMessage : BaseEventBusMessage
    {
        public RemovedDashboardMessage(int dashboardId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = dashboardId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = DashboardsEventBusMessages.RemovedDashboardMessage;        
    }
}
