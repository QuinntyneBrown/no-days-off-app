using NoDaysOffApp.Model;
using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Videos
{

    public class AddedOrUpdatedVideoMessage : BaseEventBusMessage
    {
        public AddedOrUpdatedVideoMessage(Video video, Guid correlationId, Guid tenantId)
        {
            Payload = new { Entity = video, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = VideosEventBusMessages.AddedOrUpdatedVideoMessage;        
    }
}
