using NoDaysOff.Core.Model.DashboardAggregate;

namespace NoDaysOff.Api;

public static class DashboardExtensions
{
    public static DashboardDto ToDto(this Dashboard dashboard)
    {
        return new DashboardDto(
            dashboard.Id,
            dashboard.Name,
            dashboard.Username,
            dashboard.IsDefault,
            dashboard.CreatedOn,
            dashboard.CreatedBy);
    }
}
