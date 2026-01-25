using MediatR;

namespace Api;

public sealed record GetBodyPartByIdQuery(int BodyPartId) : IRequest<BodyPartDto?>;
