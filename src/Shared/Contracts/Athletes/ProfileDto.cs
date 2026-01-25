namespace Shared.Contracts.Athletes;

public sealed record ProfileDto(
    int ProfileId,
    string Name,
    string Username,
    string? ImageUrl,
    int? TenantId,
    DateTime CreatedOn);

public sealed record CreateProfileDto(
    string Name,
    string Username,
    int? TenantId = null);

public sealed record UpdateProfileDto(
    int ProfileId,
    string Name,
    string Username,
    string? ImageUrl);
