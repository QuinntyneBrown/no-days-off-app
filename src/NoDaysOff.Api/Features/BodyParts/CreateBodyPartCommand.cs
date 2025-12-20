using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateBodyPartCommand(
    int TenantId,
    string Name,
    string CreatedBy) : IRequest<BodyPartDto>;
