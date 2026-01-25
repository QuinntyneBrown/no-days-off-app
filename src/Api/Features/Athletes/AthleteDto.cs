namespace Api;

public sealed record AthleteDto(
    int AthleteId,
    string Name,
    string Username,
    string? ImageUrl,
    int? CurrentWeight,
    DateTime? LastWeighedOn,
    DateTime CreatedOn,
    string CreatedBy);
