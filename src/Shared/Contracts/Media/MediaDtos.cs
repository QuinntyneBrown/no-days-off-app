namespace Shared.Contracts.Media;

public class MediaFileDto
{
    public int MediaFileId { get; set; }
    public string FileName { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public int Type { get; set; }
    public int TenantId { get; set; }
    public int? EntityId { get; set; }
    public string? EntityType { get; set; }
    public DateTime UploadedAt { get; set; }
    public string UploadedBy { get; set; } = null!;
    public string? Url { get; set; }
}
