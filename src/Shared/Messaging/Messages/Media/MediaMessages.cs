using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Media;

[MessagePackObject]
public class MediaUploadedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int MediaId { get; set; }

    [Key(3)]
    public string FileName { get; set; } = string.Empty;

    [Key(4)]
    public int TenantId { get; set; }

    public MediaUploadedMessage() { }

    public MediaUploadedMessage(int mediaId, string fileName, int tenantId)
    {
        MediaId = mediaId;
        FileName = fileName;
        TenantId = tenantId;
    }
}

[MessagePackObject]
public class MediaDeletedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int MediaId { get; set; }

    [Key(3)]
    public int TenantId { get; set; }

    public MediaDeletedMessage() { }

    public MediaDeletedMessage(int mediaId, int tenantId)
    {
        MediaId = mediaId;
        TenantId = tenantId;
    }
}
