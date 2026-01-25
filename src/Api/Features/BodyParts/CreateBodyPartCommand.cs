using MediatR;

namespace Api;

public sealed record CreateBodyPartCommand(
    int TenantId,
    string Name,
    string CreatedBy) : IRequest<BodyPartDto>;
