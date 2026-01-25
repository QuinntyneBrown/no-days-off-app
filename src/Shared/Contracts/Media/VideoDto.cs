namespace Shared.Contracts.Media;

public sealed record VideoDto(
    int VideoId,
    string Title,
    string? Category,
    string? SubTitle,
    string? Slug,
    string? YouTubeVideoId,
    string? Abstract,
    string? Description,
    int DurationInSeconds,
    int Rating,
    DateTime? PublishedOn,
    string? PublishedBy,
    int? TenantId,
    DateTime CreatedOn);

public sealed record CreateVideoDto(
    string Title,
    string? Category = null,
    string? SubTitle = null,
    string? YouTubeVideoId = null,
    string? Abstract = null,
    string? Description = null,
    int DurationInSeconds = 0,
    int? TenantId = null);

public sealed record UpdateVideoDto(
    int VideoId,
    string Title,
    string? Category,
    string? SubTitle,
    string? YouTubeVideoId,
    string? Abstract,
    string? Description,
    int DurationInSeconds);
