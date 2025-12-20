using MediatR;

namespace NoDaysOff.Api;

public sealed record GetVideoByIdQuery(int VideoId) : IRequest<VideoDto?>;
