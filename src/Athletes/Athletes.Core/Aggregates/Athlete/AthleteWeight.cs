using Shared.Domain;
using Shared.Domain.Exceptions;

namespace Athletes.Core.Aggregates.Athlete;

/// <summary>
/// Entity for tracking athlete weight history
/// </summary>
public class AthleteWeight : Entity
{
    public int WeightInKgs { get; private set; }
    public DateTime WeighedOn { get; private set; }
    public string RecordedBy { get; private set; } = string.Empty;

    private AthleteWeight() { }

    public static AthleteWeight Create(int weightInKgs, DateTime weighedOn, string recordedBy)
    {
        if (weightInKgs <= 0)
            throw new ValidationException("Weight must be greater than zero");

        if (string.IsNullOrWhiteSpace(recordedBy))
            throw new ValidationException("RecordedBy is required");

        return new AthleteWeight
        {
            WeightInKgs = weightInKgs,
            WeighedOn = weighedOn,
            RecordedBy = recordedBy
        };
    }
}
