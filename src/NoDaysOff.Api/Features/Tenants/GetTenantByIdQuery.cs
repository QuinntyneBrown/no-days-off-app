using MediatR;

namespace NoDaysOff.Api;

public sealed record GetTenantByIdQuery(int TenantId) : IRequest<TenantDto?>;
