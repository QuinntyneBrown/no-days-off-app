using MediatR;

namespace Api;

public sealed record GetVideosQuery : IRequest<IEnumerable<VideoDto>>;
