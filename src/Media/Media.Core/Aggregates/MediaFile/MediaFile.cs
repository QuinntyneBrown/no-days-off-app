using Shared.Domain;

namespace Media.Core.Aggregates.MediaFile;

public class MediaFile : AggregateRoot
{
    public string FileName { get; private set; } = null!;
    public string OriginalFileName { get; private set; } = null!;
    public string ContentType { get; private set; } = null!;
    public long Size { get; private set; }
    public string StoragePath { get; private set; } = null!;
    public MediaType Type { get; private set; }
    public int TenantId { get; private set; }
    public int? EntityId { get; private set; }
    public string? EntityType { get; private set; }
    public DateTime UploadedAt { get; private set; }
    public string UploadedBy { get; private set; } = null!;

    private MediaFile() { }

    public MediaFile(
        string fileName,
        string originalFileName,
        string contentType,
        long size,
        string storagePath,
        MediaType type,
        int tenantId,
        string uploadedBy,
        int? entityId = null,
        string? entityType = null)
    {
        FileName = fileName;
        OriginalFileName = originalFileName;
        ContentType = contentType;
        Size = size;
        StoragePath = storagePath;
        Type = type;
        TenantId = tenantId;
        UploadedBy = uploadedBy;
        EntityId = entityId;
        EntityType = entityType;
        UploadedAt = DateTime.UtcNow;
    }

    public void AssociateWithEntity(int entityId, string entityType)
    {
        EntityId = entityId;
        EntityType = entityType;
    }
}

public enum MediaType
{
    Image = 0,
    Video = 1,
    Document = 2,
    Other = 3
}
