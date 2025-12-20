using MediatR;

namespace NoDaysOff.Api;

public sealed record GetDashboardByIdQuery(int DashboardId) : IRequest<DashboardDto?>;
