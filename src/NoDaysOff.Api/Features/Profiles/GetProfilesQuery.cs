using MediatR;

namespace NoDaysOff.Api;

public sealed record GetProfilesQuery : IRequest<IEnumerable<ProfileDto>>;
