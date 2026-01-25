using MediatR;

namespace Api;

public sealed record GetProfileByIdQuery(int ProfileId) : IRequest<ProfileDto?>;
