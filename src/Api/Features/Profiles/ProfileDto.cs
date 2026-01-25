namespace Api;

public sealed record ProfileDto(
    int ProfileId,
    string Name,
    string Username,
    string? ImageUrl,
    DateTime CreatedOn,
    string CreatedBy);
