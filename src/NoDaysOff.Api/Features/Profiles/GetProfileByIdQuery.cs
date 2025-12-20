using MediatR;

namespace NoDaysOff.Api;

public sealed record GetProfileByIdQuery(int ProfileId) : IRequest<ProfileDto?>;
