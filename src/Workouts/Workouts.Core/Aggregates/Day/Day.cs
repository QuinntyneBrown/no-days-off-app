using Shared.Domain;
using Shared.Domain.Exceptions;

namespace Workouts.Core.Aggregates.Day;

/// <summary>
/// Aggregate root for workout days
/// </summary>
public class Day : AggregateRoot
{
    public const int MaxNameLength = 256;

    private readonly List<int> _bodyPartIds = new();

    public string Name { get; private set; } = string.Empty;
    public IReadOnlyCollection<int> BodyPartIds => _bodyPartIds.AsReadOnly();

    private Day() { }

    public static Day Create(int? tenantId, string name, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Day name is required");

        var day = new Day
        {
            Name = name.Trim(),
            TenantId = tenantId
        };
        day.SetAuditInfo(createdBy);
        return day;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Day name is required");

        Name = name.Trim();
        UpdateModified(modifiedBy);
    }

    public void AssignBodyPart(int bodyPartId)
    {
        if (!_bodyPartIds.Contains(bodyPartId))
            _bodyPartIds.Add(bodyPartId);
    }

    public void RemoveBodyPart(int bodyPartId)
    {
        _bodyPartIds.Remove(bodyPartId);
    }
}
