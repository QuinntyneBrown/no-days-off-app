using Athletes.Core.Aggregates.Athlete;
using Athletes.Core.Aggregates.Profile;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features;

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
            athlete.TenantId,
            athlete.CreatedOn,
            athlete.CreatedBy);
    }

    public static ProfileDto ToDto(this Profile profile)
    {
        return new ProfileDto(
            profile.Id,
            profile.Name,
            profile.Username,
            profile.ImageUrl,
            profile.TenantId,
            profile.CreatedOn);
    }
}
