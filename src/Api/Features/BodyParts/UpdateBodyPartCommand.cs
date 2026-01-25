using MediatR;

namespace Api;

public sealed record UpdateBodyPartCommand(
    int BodyPartId,
    string Name,
    string ModifiedBy) : IRequest<BodyPartDto>;
