using Core.Model.TenantAggregate;

namespace Api;

public static class TenantExtensions
{
    public static TenantDto ToDto(this Tenant tenant)
    {
        return new TenantDto(
            tenant.Id,
            tenant.UniqueId,
            tenant.Name,
            tenant.CreatedOn,
            tenant.CreatedBy);
    }
}
