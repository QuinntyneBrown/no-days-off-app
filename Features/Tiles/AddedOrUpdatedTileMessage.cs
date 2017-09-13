using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Tiles
{

    public class AddedOrUpdatedTileMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedTileMessage(Tile tile, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = tile, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = TilesEventBusMessages.AddedOrUpdatedTileMessage;        
    }
}
