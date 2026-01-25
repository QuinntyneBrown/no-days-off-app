using Identity.Core.Aggregates.User;
using Shared.Contracts.Identity;

namespace Identity.Core.Features;

public static class UserExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.TenantId,
            user.CreatedOn,
            user.UserRoles.Select(ur => ur.Role.Name));
    }
}
