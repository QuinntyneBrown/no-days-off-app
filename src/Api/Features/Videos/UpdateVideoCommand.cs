using MediatR;

namespace Api;

public sealed record UpdateVideoCommand(
    int VideoId,
    string Title,
    string? SubTitle,
    string? Abstract,
    string? Description,
    string? Category,
    string ModifiedBy) : IRequest<VideoDto>;
