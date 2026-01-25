using MediatR;

namespace Api;

public sealed record CreateTenantCommand(
    string Name,
    string CreatedBy) : IRequest<TenantDto>;
