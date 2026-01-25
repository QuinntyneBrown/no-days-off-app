using MediatR;

namespace Api;

public sealed record GetVideoByIdQuery(int VideoId) : IRequest<VideoDto?>;
