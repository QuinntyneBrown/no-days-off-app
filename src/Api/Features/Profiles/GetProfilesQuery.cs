using MediatR;

namespace Api;

public sealed record GetProfilesQuery : IRequest<IEnumerable<ProfileDto>>;
