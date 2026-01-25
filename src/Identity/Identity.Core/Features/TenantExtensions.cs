using Identity.Core.Aggregates.Tenant;
using Shared.Contracts.Identity;

namespace Identity.Core.Features;

public static class TenantExtensions
{
    public static TenantDto ToDto(this Tenant tenant)
    {
        return new TenantDto(
            tenant.Id,
            tenant.UniqueId,
            tenant.Name,
            tenant.CreatedOn);
    }
}
