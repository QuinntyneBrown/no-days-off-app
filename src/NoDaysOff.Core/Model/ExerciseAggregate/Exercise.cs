using NoDaysOff.Core.Abstractions;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Model.ExerciseAggregate;

/// <summary>
/// Aggregate root for exercise definitions
/// </summary>
public sealed class Exercise : AggregateRoot
{
    public const int MaxNameLength = 256;

    private readonly List<ExerciseSet> _defaultSets = new();
    private readonly List<int> _digitalAssetIds = new();

    public string Name { get; private set; } = string.Empty;
    public int? BodyPartId { get; private set; }
    public int DefaultSets { get; private set; }
    public int DefaultRepetitions { get; private set; }

    public IReadOnlyCollection<ExerciseSet> DefaultSetCollection => _defaultSets.AsReadOnly();
    public IReadOnlyCollection<int> DigitalAssetIds => _digitalAssetIds.AsReadOnly();

    private Exercise() : base()
    {
    }

    private Exercise(int? tenantId) : base(tenantId)
    {
    }

    public static Exercise Create(
        int? tenantId,
        string name,
        int? bodyPartId,
        int defaultSets,
        int defaultRepetitions,
        string createdBy)
    {
        ValidateName(name);
        ValidateDefaults(defaultSets, defaultRepetitions);

        var exercise = new Exercise(tenantId)
        {
            Name = name,
            BodyPartId = bodyPartId,
            DefaultSets = defaultSets,
            DefaultRepetitions = defaultRepetitions
        };
        exercise.SetAuditInfo(createdBy);

        return exercise;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        ValidateName(name);
        Name = name;
        UpdateModified(modifiedBy);
    }

    public void UpdateBodyPart(int? bodyPartId, string modifiedBy)
    {
        BodyPartId = bodyPartId;
        UpdateModified(modifiedBy);
    }

    public void UpdateDefaults(int defaultSets, int defaultRepetitions, string modifiedBy)
    {
        ValidateDefaults(defaultSets, defaultRepetitions);
        DefaultSets = defaultSets;
        DefaultRepetitions = defaultRepetitions;
        UpdateModified(modifiedBy);
    }

    public void AddDefaultSet(int weightInKgs, int repetitions, string modifiedBy)
    {
        var set = ExerciseSet.Create(weightInKgs, repetitions);
        _defaultSets.Add(set);
        UpdateModified(modifiedBy);
    }

    public void RemoveDefaultSet(int index, string modifiedBy)
    {
        if (index >= 0 && index < _defaultSets.Count)
        {
            _defaultSets.RemoveAt(index);
            UpdateModified(modifiedBy);
        }
    }

    public void ClearDefaultSets(string modifiedBy)
    {
        if (_defaultSets.Count > 0)
        {
            _defaultSets.Clear();
            UpdateModified(modifiedBy);
        }
    }

    public void AddDigitalAsset(int digitalAssetId, string modifiedBy)
    {
        if (!_digitalAssetIds.Contains(digitalAssetId))
        {
            _digitalAssetIds.Add(digitalAssetId);
            UpdateModified(modifiedBy);
        }
    }

    public void RemoveDigitalAsset(int digitalAssetId, string modifiedBy)
    {
        if (_digitalAssetIds.Remove(digitalAssetId))
        {
            UpdateModified(modifiedBy);
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Exercise name is required");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ValidationException($"Exercise name cannot exceed {MaxNameLength} characters");
        }
    }

    private static void ValidateDefaults(int defaultSets, int defaultRepetitions)
    {
        if (defaultSets < 0)
        {
            throw new ValidationException("Default sets cannot be negative");
        }

        if (defaultRepetitions < 0)
        {
            throw new ValidationException("Default repetitions cannot be negative");
        }
    }
}
