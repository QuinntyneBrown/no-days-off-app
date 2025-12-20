using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteTenantCommand(int TenantId, string DeletedBy) : IRequest;
