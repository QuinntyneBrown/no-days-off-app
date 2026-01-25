using MediatR;

namespace Api;

public sealed record UpdateDashboardCommand(
    int DashboardId,
    string Name,
    string ModifiedBy) : IRequest<DashboardDto>;
