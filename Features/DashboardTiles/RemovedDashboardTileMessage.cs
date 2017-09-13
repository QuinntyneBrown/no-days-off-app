using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.DashboardTiles
{
    public class RemovedDashboardTileMessage : BaseEventBusMessage
    {
        public RemovedDashboardTileMessage(int dashboardTileId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = dashboardTileId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = DashboardTilesEventBusMessages.RemovedDashboardTileMessage;        
    }
}
