using NoDaysOff.Core.Abstractions;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Aggregates.AthleteAggregate;

/// <summary>
/// Entity representing an athlete's weight record
/// </summary>
public sealed class AthleteWeight : Entity, IAuditableEntity
{
    public int WeightInKgs { get; private set; }
    public DateTime WeighedOn { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }
    public string LastModifiedBy { get; private set; } = string.Empty;

    private AthleteWeight()
    {
    }

    internal static AthleteWeight Create(int weightInKgs, DateTime weighedOn, string createdBy)
    {
        if (weightInKgs <= 0)
        {
            throw new ValidationException("Weight must be greater than zero");
        }

        return new AthleteWeight
        {
            WeightInKgs = weightInKgs,
            WeighedOn = weighedOn,
            CreatedOn = DateTime.UtcNow,
            LastModifiedOn = DateTime.UtcNow,
            CreatedBy = createdBy,
            LastModifiedBy = createdBy
        };
    }

    internal void Update(int weightInKgs, DateTime weighedOn, string modifiedBy)
    {
        if (weightInKgs <= 0)
        {
            throw new ValidationException("Weight must be greater than zero");
        }

        WeightInKgs = weightInKgs;
        WeighedOn = weighedOn;
        LastModifiedOn = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}
