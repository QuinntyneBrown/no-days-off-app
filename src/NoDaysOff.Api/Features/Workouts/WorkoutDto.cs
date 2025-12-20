namespace NoDaysOff.Api;

public sealed record WorkoutDto(
    int WorkoutId,
    IEnumerable<int> BodyPartIds,
    DateTime CreatedOn,
    string CreatedBy);
