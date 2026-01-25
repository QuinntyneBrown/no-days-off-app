using Shared.Domain;
using Shared.Domain.Exceptions;

namespace Workouts.Core.Aggregates.Workout;

/// <summary>
/// Aggregate root for workout management
/// </summary>
public class Workout : AggregateRoot
{
    public const int MaxNameLength = 256;

    private readonly List<int> _bodyPartIds = new();

    public string Name { get; private set; } = string.Empty;
    public IReadOnlyCollection<int> BodyPartIds => _bodyPartIds.AsReadOnly();

    private Workout() { }

    public static Workout Create(int? tenantId, string name, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Workout name is required");

        if (name.Length > MaxNameLength)
            throw new ValidationException($"Workout name cannot exceed {MaxNameLength} characters");

        var workout = new Workout
        {
            Name = name.Trim(),
            TenantId = tenantId
        };
        workout.SetAuditInfo(createdBy);
        return workout;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Workout name is required");

        Name = name.Trim();
        UpdateModified(modifiedBy);
    }

    public void AddBodyPart(int bodyPartId)
    {
        if (!_bodyPartIds.Contains(bodyPartId))
            _bodyPartIds.Add(bodyPartId);
    }

    public void RemoveBodyPart(int bodyPartId)
    {
        _bodyPartIds.Remove(bodyPartId);
    }

    public void ClearBodyParts()
    {
        _bodyPartIds.Clear();
    }
}
