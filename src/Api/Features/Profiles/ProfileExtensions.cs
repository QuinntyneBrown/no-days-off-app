using Core.Model.ProfileAggregate;

namespace Api;

public static class ProfileExtensions
{
    public static ProfileDto ToDto(this Profile profile)
    {
        return new ProfileDto(
            profile.Id,
            profile.Name,
            profile.Username,
            profile.ImageUrl,
            profile.CreatedOn,
            profile.CreatedBy);
    }
}
