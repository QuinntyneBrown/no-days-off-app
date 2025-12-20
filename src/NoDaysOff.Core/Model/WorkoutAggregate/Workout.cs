using NoDaysOff.Core.Abstractions;

namespace NoDaysOff.Core.Model.WorkoutAggregate;

/// <summary>
/// Aggregate root for workout management
/// </summary>
public sealed class Workout : AggregateRoot
{
    private readonly List<int> _bodyPartIds = new();

    public IReadOnlyCollection<int> BodyPartIds => _bodyPartIds.AsReadOnly();

    private Workout() : base()
    {
    }

    private Workout(int? tenantId) : base(tenantId)
    {
    }

    public static Workout Create(int? tenantId, string createdBy)
    {
        var workout = new Workout(tenantId);
        workout.SetAuditInfo(createdBy);

        return workout;
    }

    public void AddBodyPart(int bodyPartId, string modifiedBy)
    {
        if (!_bodyPartIds.Contains(bodyPartId))
        {
            _bodyPartIds.Add(bodyPartId);
            UpdateModified(modifiedBy);
        }
    }

    public void RemoveBodyPart(int bodyPartId, string modifiedBy)
    {
        if (_bodyPartIds.Remove(bodyPartId))
        {
            UpdateModified(modifiedBy);
        }
    }

    public void ClearBodyParts(string modifiedBy)
    {
        if (_bodyPartIds.Count > 0)
        {
            _bodyPartIds.Clear();
            UpdateModified(modifiedBy);
        }
    }

    public bool HasBodyPart(int bodyPartId) => _bodyPartIds.Contains(bodyPartId);
}
