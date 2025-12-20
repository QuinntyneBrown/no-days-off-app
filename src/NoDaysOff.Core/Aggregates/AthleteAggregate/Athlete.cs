using NoDaysOff.Core.Aggregates.ProfileAggregate;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Aggregates.AthleteAggregate;

/// <summary>
/// Aggregate root for athlete management, extending Profile with fitness tracking
/// </summary>
public sealed class Athlete : Profile
{
    private readonly List<AthleteWeight> _weights = new();
    private readonly List<CompletedExercise> _completedExercises = new();

    public int? CurrentWeight { get; private set; }
    public DateTime? LastWeighedOn { get; private set; }

    public IReadOnlyCollection<AthleteWeight> Weights => _weights.AsReadOnly();
    public IReadOnlyCollection<CompletedExercise> CompletedExercises => _completedExercises.AsReadOnly();

    private Athlete() : base()
    {
    }

    private Athlete(int? tenantId) : base(tenantId)
    {
    }

    public static Athlete Create(int? tenantId, string name, string username, string createdBy)
    {
        ValidateName(name);
        ValidateUsername(username);

        var athlete = new Athlete(tenantId)
        {
            Name = name,
            Username = username
        };
        athlete.SetAuditInfo(createdBy);

        return athlete;
    }

    public void RecordWeight(int weightInKgs, DateTime weighedOn, string recordedBy)
    {
        var weight = AthleteWeight.Create(weightInKgs, weighedOn, recordedBy);
        _weights.Add(weight);

        if (!LastWeighedOn.HasValue || weighedOn >= LastWeighedOn.Value)
        {
            CurrentWeight = weightInKgs;
            LastWeighedOn = weighedOn;
        }

        UpdateModified(recordedBy);
    }

    public void RecordCompletedExercise(
        int scheduledExerciseId,
        int weightInKgs,
        int reps,
        int sets,
        int distance,
        int timeInSeconds,
        DateTime completionDateTime,
        string recordedBy)
    {
        var completedExercise = CompletedExercise.Create(
            scheduledExerciseId,
            weightInKgs,
            reps,
            sets,
            distance,
            timeInSeconds,
            completionDateTime,
            recordedBy);

        _completedExercises.Add(completedExercise);
        UpdateModified(recordedBy);
    }

    public IEnumerable<CompletedExercise> GetCompletedExercisesByDate(DateTime date)
    {
        return _completedExercises.Where(e => e.CompletionDateTime.Date == date.Date);
    }

    public IEnumerable<AthleteWeight> GetWeightHistory(int count = 10)
    {
        return _weights.OrderByDescending(w => w.WeighedOn).Take(count);
    }

    public int? CalculateWeightChange(int days)
    {
        if (_weights.Count < 2)
        {
            return null;
        }

        var cutoffDate = DateTime.UtcNow.AddDays(-days);
        var oldWeight = _weights
            .Where(w => w.WeighedOn <= cutoffDate)
            .OrderByDescending(w => w.WeighedOn)
            .FirstOrDefault();

        if (oldWeight == null || !CurrentWeight.HasValue)
        {
            return null;
        }

        return CurrentWeight.Value - oldWeight.WeightInKgs;
    }
}
