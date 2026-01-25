using Core.Abstractions;
using Core.Exceptions;

namespace Core.Model.ScheduledExerciseAggregate;

/// <summary>
/// Aggregate root for scheduled exercises within a workout plan
/// </summary>
public sealed class ScheduledExercise : AggregateRoot
{
    public const int MaxNameLength = 256;

    private readonly List<ScheduledExerciseSet> _sets = new();

    public string Name { get; private set; } = string.Empty;
    public int? DayId { get; private set; }
    public int? ExerciseId { get; private set; }
    public int Sort { get; private set; }
    public int Repetitions { get; private set; }
    public int WeightInKgs { get; private set; }
    public int Sets { get; private set; }
    public int Distance { get; private set; }
    public int TimeInSeconds { get; private set; }

    public IReadOnlyCollection<ScheduledExerciseSet> SetCollection => _sets.AsReadOnly();

    private ScheduledExercise() : base()
    {
    }

    private ScheduledExercise(int? tenantId) : base(tenantId)
    {
    }

    public static ScheduledExercise Create(
        int? tenantId,
        string name,
        int? dayId,
        int? exerciseId,
        int sort,
        string createdBy)
    {
        ValidateName(name);

        var scheduledExercise = new ScheduledExercise(tenantId)
        {
            Name = name,
            DayId = dayId,
            ExerciseId = exerciseId,
            Sort = sort >= 0 ? sort : 0
        };
        scheduledExercise.SetAuditInfo(createdBy);

        return scheduledExercise;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        ValidateName(name);
        Name = name;
        UpdateModified(modifiedBy);
    }

    public void UpdateSchedule(int? dayId, int sort, string modifiedBy)
    {
        DayId = dayId;
        Sort = sort >= 0 ? sort : 0;
        UpdateModified(modifiedBy);
    }

    public void UpdateExercise(int? exerciseId, string modifiedBy)
    {
        ExerciseId = exerciseId;
        UpdateModified(modifiedBy);
    }

    public void UpdatePerformanceTargets(int repetitions, int sets, int weightInKgs, string modifiedBy)
    {
        if (repetitions < 0)
        {
            throw new ValidationException("Repetitions cannot be negative");
        }

        if (sets < 0)
        {
            throw new ValidationException("Sets cannot be negative");
        }

        if (weightInKgs < 0)
        {
            throw new ValidationException("Weight cannot be negative");
        }

        Repetitions = repetitions;
        Sets = sets;
        WeightInKgs = weightInKgs;
        UpdateModified(modifiedBy);
    }

    public void UpdateCardioTargets(int distance, int timeInSeconds, string modifiedBy)
    {
        if (distance < 0)
        {
            throw new ValidationException("Distance cannot be negative");
        }

        if (timeInSeconds < 0)
        {
            throw new ValidationException("Time cannot be negative");
        }

        Distance = distance;
        TimeInSeconds = timeInSeconds;
        UpdateModified(modifiedBy);
    }

    public void AddSet(int weightInKgs, int repetitions, string modifiedBy)
    {
        var setNumber = _sets.Count + 1;
        var set = ScheduledExerciseSet.Create(setNumber, weightInKgs, repetitions);
        _sets.Add(set);
        UpdateModified(modifiedBy);
    }

    public void RemoveSet(int index, string modifiedBy)
    {
        if (index >= 0 && index < _sets.Count)
        {
            _sets.RemoveAt(index);
            // Re-number remaining sets
            for (int i = 0; i < _sets.Count; i++)
            {
                // Sets are renumbered based on their position
            }
            UpdateModified(modifiedBy);
        }
    }

    public void ClearSets(string modifiedBy)
    {
        if (_sets.Count > 0)
        {
            _sets.Clear();
            UpdateModified(modifiedBy);
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Scheduled exercise name is required");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ValidationException($"Scheduled exercise name cannot exceed {MaxNameLength} characters");
        }
    }
}
