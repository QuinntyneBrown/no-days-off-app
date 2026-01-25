using Shared.Domain;

namespace Workouts.Core.Aggregates.ScheduledExercise;

/// <summary>
/// Entity for individual sets within a scheduled exercise
/// </summary>
public class ScheduledExerciseSet : Entity
{
    public int SetNumber { get; private set; }
    public int Repetitions { get; private set; }
    public int? WeightInKgs { get; private set; }

    private ScheduledExerciseSet() { }

    public static ScheduledExerciseSet Create(int setNumber, int repetitions, int? weightInKgs)
    {
        return new ScheduledExerciseSet
        {
            SetNumber = setNumber,
            Repetitions = repetitions,
            WeightInKgs = weightInKgs
        };
    }
}
