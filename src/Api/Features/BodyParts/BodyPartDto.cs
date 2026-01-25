namespace Api;

public sealed record BodyPartDto(
    int BodyPartId,
    string Name,
    DateTime CreatedOn,
    string CreatedBy);
