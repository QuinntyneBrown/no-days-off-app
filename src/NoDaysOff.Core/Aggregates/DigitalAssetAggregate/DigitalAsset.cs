using NoDaysOff.Core.Abstractions;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Aggregates.DigitalAssetAggregate;

/// <summary>
/// Aggregate root for digital asset management (files, images, etc.)
/// </summary>
public sealed class DigitalAsset : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string FileName { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Folder { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long Size { get; private set; }
    public DateTime? FileCreated { get; private set; }
    public DateTime? FileModified { get; private set; }
    public Guid UniqueId { get; private set; }

    public string RelativePath => $"{Folder}/{UniqueId}/{FileName}";

    private DigitalAsset() : base()
    {
        UniqueId = Guid.NewGuid();
    }

    public static DigitalAsset Create(string name, string fileName, string contentType, long size, string createdBy)
    {
        ValidateName(name);
        ValidateFileName(fileName);

        var asset = new DigitalAsset
        {
            Name = name,
            FileName = fileName,
            ContentType = contentType,
            Size = size,
            FileCreated = DateTime.UtcNow,
            FileModified = DateTime.UtcNow,
            UniqueId = Guid.NewGuid()
        };
        asset.SetAuditInfo(createdBy);

        return asset;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        ValidateName(name);
        Name = name;
        UpdateModified(modifiedBy);
    }

    public void UpdateDescription(string description, string modifiedBy)
    {
        Description = description ?? string.Empty;
        UpdateModified(modifiedBy);
    }

    public void UpdateFolder(string folder, string modifiedBy)
    {
        Folder = folder ?? string.Empty;
        UpdateModified(modifiedBy);
    }

    public void UpdateFileInfo(string fileName, string contentType, long size, string modifiedBy)
    {
        ValidateFileName(fileName);

        FileName = fileName;
        ContentType = contentType ?? string.Empty;
        Size = size;
        FileModified = DateTime.UtcNow;
        UpdateModified(modifiedBy);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Digital asset name is required");
        }
    }

    private static void ValidateFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ValidationException("File name is required");
        }

        var invalidChars = Path.GetInvalidFileNameChars();
        if (fileName.Any(c => invalidChars.Contains(c)))
        {
            throw new ValidationException("File name contains invalid characters");
        }
    }
}
