namespace Shared.Contracts.Workouts;

public sealed record DayDto(
    int DayId,
    string Name,
    IEnumerable<int> BodyPartIds,
    int? TenantId,
    DateTime CreatedOn);

public sealed record CreateDayDto(
    string Name,
    IEnumerable<int>? BodyPartIds = null,
    int? TenantId = null);

public sealed record UpdateDayDto(
    int DayId,
    string Name,
    IEnumerable<int>? BodyPartIds = null);
