using MediatR;

namespace NoDaysOff.Api;

public sealed record GetVideosQuery : IRequest<IEnumerable<VideoDto>>;
