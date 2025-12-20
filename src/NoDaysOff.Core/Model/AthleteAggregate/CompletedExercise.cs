using NoDaysOff.Core.Abstractions;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Model.AthleteAggregate;

/// <summary>
/// Entity representing a completed scheduled exercise
/// </summary>
public sealed class CompletedExercise : Entity, IAuditableEntity
{
    public int ScheduledExerciseId { get; private set; }
    public int WeightInKgs { get; private set; }
    public int Reps { get; private set; }
    public int Sets { get; private set; }
    public int Distance { get; private set; }
    public int TimeInSeconds { get; private set; }
    public DateTime CompletionDateTime { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }
    public string LastModifiedBy { get; private set; } = string.Empty;

    private CompletedExercise()
    {
    }

    internal static CompletedExercise Create(
        int scheduledExerciseId,
        int weightInKgs,
        int reps,
        int sets,
        int distance,
        int timeInSeconds,
        DateTime completionDateTime,
        string createdBy)
    {
        Validate(reps, sets);

        return new CompletedExercise
        {
            ScheduledExerciseId = scheduledExerciseId,
            WeightInKgs = weightInKgs,
            Reps = reps,
            Sets = sets,
            Distance = distance,
            TimeInSeconds = timeInSeconds,
            CompletionDateTime = completionDateTime,
            CreatedOn = DateTime.UtcNow,
            LastModifiedOn = DateTime.UtcNow,
            CreatedBy = createdBy,
            LastModifiedBy = createdBy
        };
    }

    private static void Validate(int reps, int sets)
    {
        if (reps < 0)
        {
            throw new ValidationException("Reps cannot be negative");
        }

        if (sets < 0)
        {
            throw new ValidationException("Sets cannot be negative");
        }
    }
}
