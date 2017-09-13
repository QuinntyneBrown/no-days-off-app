using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.DashboardTiles
{

    public class AddedOrUpdatedDashboardTileMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedDashboardTileMessage(dynamic dashboardTile, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = dashboardTile, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = DashboardTilesEventBusMessages.AddedOrUpdatedDashboardTileMessage;        
    }
}
