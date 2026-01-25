namespace Shared.Contracts.Exercises;

public sealed record BodyPartDto(
    int BodyPartId,
    string Name,
    int? TenantId,
    DateTime CreatedOn);

public sealed record CreateBodyPartDto(
    string Name,
    int? TenantId = null);

public sealed record UpdateBodyPartDto(
    int BodyPartId,
    string Name);
