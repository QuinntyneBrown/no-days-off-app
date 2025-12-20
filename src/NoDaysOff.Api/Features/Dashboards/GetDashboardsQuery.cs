using MediatR;

namespace NoDaysOff.Api;

public sealed record GetDashboardsQuery : IRequest<IEnumerable<DashboardDto>>;
