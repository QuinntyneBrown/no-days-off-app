using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteDashboardCommand(int DashboardId, string DeletedBy) : IRequest;
