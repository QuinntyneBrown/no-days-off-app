using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateVideoCommand(
    int TenantId,
    string Title,
    string YouTubeVideoId,
    string CreatedBy) : IRequest<VideoDto>;
