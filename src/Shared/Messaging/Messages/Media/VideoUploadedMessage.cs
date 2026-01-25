using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Media;

/// <summary>
/// Message sent when a video is uploaded
/// </summary>
[MessagePackObject]
public class VideoUploadedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int VideoId { get; set; }

    [Key(3)]
    public string Title { get; set; } = string.Empty;

    [Key(4)]
    public string? YouTubeVideoId { get; set; }

    [Key(5)]
    public int DurationInSeconds { get; set; }

    [Key(6)]
    public int? TenantId { get; set; }
}
