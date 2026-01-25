namespace Shared.Contracts.Workouts;

public sealed record WorkoutDto(
    int WorkoutId,
    string Name,
    IEnumerable<int> BodyPartIds,
    int? TenantId,
    DateTime CreatedOn);

public sealed record CreateWorkoutDto(
    string Name,
    IEnumerable<int>? BodyPartIds = null,
    int? TenantId = null);

public sealed record UpdateWorkoutDto(
    int WorkoutId,
    string Name,
    IEnumerable<int>? BodyPartIds = null);
