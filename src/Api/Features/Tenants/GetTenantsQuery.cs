using MediatR;

namespace Api;

public sealed record GetTenantsQuery : IRequest<IEnumerable<TenantDto>>;
