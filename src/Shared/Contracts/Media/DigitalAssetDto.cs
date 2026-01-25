namespace Shared.Contracts.Media;

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
    int? TenantId,
    DateTime CreatedOn);

public sealed record CreateDigitalAssetDto(
    string Name,
    string FileName,
    string ContentType,
    long Size,
    string? Description = null,
    string? Folder = null,
    int? TenantId = null);

public sealed record UpdateDigitalAssetDto(
    int DigitalAssetId,
    string Name,
    string? Description,
    string? Folder);
