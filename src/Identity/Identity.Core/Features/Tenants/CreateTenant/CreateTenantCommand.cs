using MediatR;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Tenants.CreateTenant;

public sealed record CreateTenantCommand(
    string Name,
    string CreatedBy) : IRequest<TenantDto>;
