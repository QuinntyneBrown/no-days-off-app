using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateTenantCommand(
    string Name,
    string CreatedBy) : IRequest<TenantDto>;
