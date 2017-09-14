using NoDaysOffApp.Features.Core;
using System;

namespace NoDaysOffApp.Features.Videos
{
    public class RemovedVideoMessage : BaseEventBusMessage
    {
        public RemovedVideoMessage(int videoId, Guid correlationId, Guid tenantId)
        {
            Payload = new { Id = videoId, CorrelationId = correlationId };
            TenantUniqueId = tenantId;
        }
        public override string Type { get; set; } = VideosEventBusMessages.RemovedVideoMessage;        
    }
}
