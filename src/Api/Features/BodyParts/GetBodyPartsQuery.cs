using MediatR;

namespace Api;

public sealed record GetBodyPartsQuery : IRequest<IEnumerable<BodyPartDto>>;
