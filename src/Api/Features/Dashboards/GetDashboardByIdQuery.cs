using MediatR;

namespace Api;

public sealed record GetDashboardByIdQuery(int DashboardId) : IRequest<DashboardDto?>;
