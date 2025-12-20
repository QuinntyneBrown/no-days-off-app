using NoDaysOff.Core.Model.WorkoutAggregate;

namespace NoDaysOff.Api;

public static class WorkoutExtensions
{
    public static WorkoutDto ToDto(this Workout workout)
    {
        return new WorkoutDto(
            workout.Id,
            workout.BodyPartIds,
            workout.CreatedOn,
            workout.CreatedBy);
    }
}
