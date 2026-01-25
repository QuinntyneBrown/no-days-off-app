using MediatR;

namespace Api;

public sealed record CreateVideoCommand(
    int TenantId,
    string Title,
    string YouTubeVideoId,
    string CreatedBy) : IRequest<VideoDto>;
