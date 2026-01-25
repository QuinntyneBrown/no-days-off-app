using Core.Model.AthleteAggregate;

namespace Api;

public static class AthleteExtensions
{
    public static AthleteDto ToDto(this Athlete athlete)
    {
        return new AthleteDto(
            athlete.Id,
            athlete.Name,
            athlete.Username,
            athlete.ImageUrl,
            athlete.CurrentWeight,
            athlete.LastWeighedOn,
            athlete.CreatedOn,
            athlete.CreatedBy);
    }
}
