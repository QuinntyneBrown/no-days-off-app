using Core.Model.DayAggregate;

namespace Api;

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
