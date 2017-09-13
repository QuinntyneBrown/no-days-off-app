using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Tiles
{
    public class RemovedTileMessage : BaseEventBusMessage
    {
        public RemovedTileMessage(int tileId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = tileId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = TilesEventBusMessages.RemovedTileMessage;        
    }
}
