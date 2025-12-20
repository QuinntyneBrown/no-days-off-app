using NoDaysOff.Core.Model.DayAggregate;

namespace NoDaysOff.Api;

public static class DayExtensions
{
    public static DayDto ToDto(this Day day)
    {
        return new DayDto(
            day.Id,
            day.Name,
            day.BodyPartIds,
            day.CreatedOn,
            day.CreatedBy);
    }
}
