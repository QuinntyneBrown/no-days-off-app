using MediatR;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Tenants.GetTenants;

public sealed record GetTenantsQuery : IRequest<IEnumerable<TenantDto>>;
