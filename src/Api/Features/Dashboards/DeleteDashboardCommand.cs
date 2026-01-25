using MediatR;

namespace Api;

public sealed record DeleteDashboardCommand(int DashboardId, string DeletedBy) : IRequest;
