using MediatR;

namespace NoDaysOff.Api;

public sealed record GetBodyPartByIdQuery(int BodyPartId) : IRequest<BodyPartDto?>;
