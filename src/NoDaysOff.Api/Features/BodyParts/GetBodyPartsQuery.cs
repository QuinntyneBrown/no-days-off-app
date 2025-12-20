using MediatR;

namespace NoDaysOff.Api;

public sealed record GetBodyPartsQuery : IRequest<IEnumerable<BodyPartDto>>;
