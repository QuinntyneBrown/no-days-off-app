using NoDaysOff.Core.Abstractions;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Aggregates.DayAggregate;

/// <summary>
/// Aggregate root for day definitions (workout days of the week)
/// </summary>
public sealed class Day : AggregateRoot
{
    public const int MaxNameLength = 256;

    private readonly List<int> _bodyPartIds = new();

    public string Name { get; private set; } = string.Empty;
    public IReadOnlyCollection<int> BodyPartIds => _bodyPartIds.AsReadOnly();

    private Day() : base()
    {
    }

    private Day(int? tenantId) : base(tenantId)
    {
    }

    public static Day Create(int? tenantId, string name, string createdBy)
    {
        ValidateName(name);

        var day = new Day(tenantId)
        {
            Name = name
        };
        day.SetAuditInfo(createdBy);

        return day;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        ValidateName(name);
        Name = name;
        UpdateModified(modifiedBy);
    }

    public void AssignBodyPart(int bodyPartId, string modifiedBy)
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

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Day name is required");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ValidationException($"Day name cannot exceed {MaxNameLength} characters");
        }
    }
}
