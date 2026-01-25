using MediatR;

namespace Api;

public sealed record UpdateTenantCommand(
    int TenantId,
    string Name,
    string ModifiedBy) : IRequest<TenantDto>;
