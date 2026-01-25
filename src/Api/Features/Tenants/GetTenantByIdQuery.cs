using MediatR;

namespace Api;

public sealed record GetTenantByIdQuery(int TenantId) : IRequest<TenantDto?>;
