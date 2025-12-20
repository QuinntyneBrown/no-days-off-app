using MediatR;

namespace NoDaysOff.Api;

public sealed record UpdateBodyPartCommand(
    int BodyPartId,
    string Name,
    string ModifiedBy) : IRequest<BodyPartDto>;
