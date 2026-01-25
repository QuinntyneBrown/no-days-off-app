namespace Api;

public sealed record VideoDto(
    int VideoId,
    string Title,
    string? Category,
    string? SubTitle,
    string Slug,
    string YouTubeVideoId,
    string? Abstract,
    string? Description,
    int DurationInSeconds,
    decimal Rating,
    bool IsPublished,
    DateTime? PublishedOn,
    string? PublishedBy,
    DateTime CreatedOn,
    string CreatedBy);
