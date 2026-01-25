using Shared.Contracts.Workouts;
using Workouts.Core.Aggregates.Day;
using Workouts.Core.Aggregates.ScheduledExercise;
using Workouts.Core.Aggregates.Workout;

namespace Workouts.Core.Features;

public static class WorkoutExtensions
{
    public static WorkoutDto ToDto(this Workout workout)
    {
        return new WorkoutDto(
            workout.Id,
            workout.Name,
            workout.BodyPartIds,
            workout.TenantId,
            workout.CreatedOn);
    }

    public static DayDto ToDto(this Day day)
    {
        return new DayDto(
            day.Id,
            day.Name,
            day.BodyPartIds,
            day.TenantId,
            day.CreatedOn);
    }

    public static ScheduledExerciseDto ToDto(this ScheduledExercise scheduled)
    {
        return new ScheduledExerciseDto(
            scheduled.Id,
            scheduled.Name,
            scheduled.DayId,
            scheduled.ExerciseId,
            scheduled.Sort,
            scheduled.Repetitions,
            scheduled.WeightInKgs,
            scheduled.Sets,
            scheduled.Distance,
            scheduled.TimeInSeconds,
            scheduled.TenantId,
            scheduled.CreatedOn);
    }
}
