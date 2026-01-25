namespace Api;

public sealed record DigitalAssetDto(
    int DigitalAssetId,
    string Name,
    string FileName,
    string? Description,
    string? Folder,
    string ContentType,
    long Size,
    Guid UniqueId,
    string RelativePath,
    DateTime CreatedOn,
    string CreatedBy);
