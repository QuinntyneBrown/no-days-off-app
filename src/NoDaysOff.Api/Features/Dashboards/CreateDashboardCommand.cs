using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateDashboardCommand(
    int TenantId,
    string Name,
    string Username,
    bool IsDefault,
    string CreatedBy) : IRequest<DashboardDto>;
