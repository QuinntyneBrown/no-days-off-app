using MediatR;

namespace NoDaysOff.Api;

public sealed record GetTenantsQuery : IRequest<IEnumerable<TenantDto>>;
