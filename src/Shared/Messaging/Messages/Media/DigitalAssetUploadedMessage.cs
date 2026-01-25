using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Media;

/// <summary>
/// Message sent when a digital asset is uploaded
/// </summary>
[MessagePackObject]
public class DigitalAssetUploadedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int DigitalAssetId { get; set; }

    [Key(3)]
    public string Name { get; set; } = string.Empty;

    [Key(4)]
    public string FileName { get; set; } = string.Empty;

    [Key(5)]
    public string ContentType { get; set; } = string.Empty;

    [Key(6)]
    public long Size { get; set; }

    [Key(7)]
    public int? TenantId { get; set; }
}
