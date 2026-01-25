using Shared.Domain;
using Shared.Domain.Exceptions;

namespace Workouts.Core.Aggregates.ScheduledExercise;

/// <summary>
/// Aggregate root for scheduled exercises within workout plans
/// </summary>
public class ScheduledExercise : AggregateRoot
{
    public const int MaxNameLength = 256;

    public string Name { get; private set; } = string.Empty;
    public int DayId { get; private set; }
    public int ExerciseId { get; private set; }
    public int Sort { get; private set; }
    public int? Repetitions { get; private set; }
    public int? WeightInKgs { get; private set; }
    public int? Sets { get; private set; }
    public int? Distance { get; private set; }
    public int? TimeInSeconds { get; private set; }

    private readonly List<ScheduledExerciseSet> _sets = new();
    public IReadOnlyCollection<ScheduledExerciseSet> ExerciseSets => _sets.AsReadOnly();

    private ScheduledExercise() { }

    public static ScheduledExercise Create(
        int? tenantId,
        string name,
        int dayId,
        int exerciseId,
        int sort,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Exercise name is required");

        var scheduled = new ScheduledExercise
        {
            Name = name.Trim(),
            DayId = dayId,
            ExerciseId = exerciseId,
            Sort = sort,
            TenantId = tenantId
        };
        scheduled.SetAuditInfo(createdBy);
        return scheduled;
    }

    public void UpdatePerformanceTargets(int? repetitions, int? weightInKgs, int? sets, string modifiedBy)
    {
        Repetitions = repetitions;
        WeightInKgs = weightInKgs;
        Sets = sets;
        UpdateModified(modifiedBy);
    }

    public void UpdateCardioTargets(int? distance, int? timeInSeconds, string modifiedBy)
    {
        Distance = distance;
        TimeInSeconds = timeInSeconds;
        UpdateModified(modifiedBy);
    }

    public void AddSet(int setNumber, int repetitions, int? weightInKgs)
    {
        var set = ScheduledExerciseSet.Create(setNumber, repetitions, weightInKgs);
        _sets.Add(set);
    }

    public void RemoveSet(int setId)
    {
        var set = _sets.FirstOrDefault(s => s.Id == setId);
        if (set != null)
            _sets.Remove(set);
    }
}
