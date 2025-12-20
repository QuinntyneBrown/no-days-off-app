using NoDaysOff.Core.Abstractions;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Model.ExerciseAggregate;

/// <summary>
/// Entity representing a set configuration for an exercise
/// </summary>
public sealed class ExerciseSet : Entity
{
    public int WeightInKgs { get; private set; }
    public int Repetitions { get; private set; }

    private ExerciseSet()
    {
    }

    internal static ExerciseSet Create(int weightInKgs, int repetitions)
    {
        Validate(weightInKgs, repetitions);

        return new ExerciseSet
        {
            WeightInKgs = weightInKgs,
            Repetitions = repetitions
        };
    }

    internal void Update(int weightInKgs, int repetitions)
    {
        Validate(weightInKgs, repetitions);
        WeightInKgs = weightInKgs;
        Repetitions = repetitions;
    }

    private static void Validate(int weightInKgs, int repetitions)
    {
        if (weightInKgs < 0)
        {
            throw new ValidationException("Weight cannot be negative");
        }

        if (repetitions < 0)
        {
            throw new ValidationException("Repetitions cannot be negative");
        }
    }
}
