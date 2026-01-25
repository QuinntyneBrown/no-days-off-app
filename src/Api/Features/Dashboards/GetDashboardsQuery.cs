using MediatR;

namespace Api;

public sealed record GetDashboardsQuery : IRequest<IEnumerable<DashboardDto>>;
