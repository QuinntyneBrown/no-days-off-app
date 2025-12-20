using MediatR;

namespace NoDaysOff.Api;

public sealed record UpdateTenantCommand(
    int TenantId,
    string Name,
    string ModifiedBy) : IRequest<TenantDto>;
