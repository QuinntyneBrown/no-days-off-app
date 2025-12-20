using NoDaysOff.Core.Abstractions;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Model.ScheduledExerciseAggregate;

/// <summary>
/// Entity representing a set within a scheduled exercise
/// </summary>
public sealed class ScheduledExerciseSet : Entity
{
    public int WeightInKgs { get; private set; }
    public int Repetitions { get; private set; }
    public int SetNumber { get; private set; }

    private ScheduledExerciseSet()
    {
    }

    internal static ScheduledExerciseSet Create(int setNumber, int weightInKgs, int repetitions)
    {
        if (setNumber < 1)
        {
            throw new ValidationException("Set number must be at least 1");
        }

        if (weightInKgs < 0)
        {
            throw new ValidationException("Weight cannot be negative");
        }

        if (repetitions < 0)
        {
            throw new ValidationException("Repetitions cannot be negative");
        }

        return new ScheduledExerciseSet
        {
            SetNumber = setNumber,
            WeightInKgs = weightInKgs,
            Repetitions = repetitions
        };
    }

    internal void Update(int weightInKgs, int repetitions)
    {
        if (weightInKgs < 0)
        {
            throw new ValidationException("Weight cannot be negative");
        }

        if (repetitions < 0)
        {
            throw new ValidationException("Repetitions cannot be negative");
        }

        WeightInKgs = weightInKgs;
        Repetitions = repetitions;
    }
}
