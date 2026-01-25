using MediatR;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Tenants.GetTenantById;

public sealed record GetTenantByIdQuery(int TenantId) : IRequest<TenantDto?>;
