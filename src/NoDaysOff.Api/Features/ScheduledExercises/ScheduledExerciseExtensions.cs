using NoDaysOff.Core.Model.ScheduledExerciseAggregate;

namespace NoDaysOff.Api;

public static class ScheduledExerciseExtensions
{
    public static ScheduledExerciseDto ToDto(this ScheduledExercise scheduledExercise)
    {
        return new ScheduledExerciseDto(
            scheduledExercise.Id,
            scheduledExercise.Name,
            scheduledExercise.DayId,
            scheduledExercise.ExerciseId,
            scheduledExercise.Sort,
            scheduledExercise.Repetitions,
            scheduledExercise.WeightInKgs,
            scheduledExercise.Sets,
            scheduledExercise.Distance,
            scheduledExercise.TimeInSeconds,
            scheduledExercise.CreatedOn,
            scheduledExercise.CreatedBy);
    }
}
