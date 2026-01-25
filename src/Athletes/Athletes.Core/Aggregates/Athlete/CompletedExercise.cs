using Shared.Domain;

namespace Athletes.Core.Aggregates.Athlete;

/// <summary>
/// Entity for tracking completed exercises
/// </summary>
public class CompletedExercise : Entity
{
    public int ScheduledExerciseId { get; private set; }
    public int WeightInKgs { get; private set; }
    public int Reps { get; private set; }
    public int Sets { get; private set; }
    public int Distance { get; private set; }
    public int TimeInSeconds { get; private set; }
    public DateTime CompletionDateTime { get; private set; }
    public string RecordedBy { get; private set; } = string.Empty;

    private CompletedExercise() { }

    public static CompletedExercise Create(
        int scheduledExerciseId,
        int weightInKgs,
        int reps,
        int sets,
        int distance,
        int timeInSeconds,
        DateTime completionDateTime,
        string recordedBy)
    {
        return new CompletedExercise
        {
            ScheduledExerciseId = scheduledExerciseId,
            WeightInKgs = weightInKgs,
            Reps = reps,
            Sets = sets,
            Distance = distance,
            TimeInSeconds = timeInSeconds,
            CompletionDateTime = completionDateTime,
            RecordedBy = recordedBy
        };
    }
}
