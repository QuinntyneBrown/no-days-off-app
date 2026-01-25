namespace Shared.Contracts.Athletes;

public sealed record AthleteDto(
    int AthleteId,
    string Name,
    string Username,
    string? ImageUrl,
    int? CurrentWeight,
    DateTime? LastWeighedOn,
    int? TenantId,
    DateTime CreatedOn,
    string CreatedBy);

public sealed record CreateAthleteDto(
    string Name,
    string Username,
    int? TenantId = null);

public sealed record UpdateAthleteDto(
    int AthleteId,
    string Name,
    string Username);
