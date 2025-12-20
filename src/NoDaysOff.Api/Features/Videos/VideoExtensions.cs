using NoDaysOff.Core.Model.VideoAggregate;

namespace NoDaysOff.Api;

public static class VideoExtensions
{
    public static VideoDto ToDto(this Video video)
    {
        return new VideoDto(
            video.Id,
            video.Title,
            video.Category,
            video.SubTitle,
            video.Slug,
            video.YouTubeVideoId,
            video.Abstract,
            video.Description,
            video.DurationInSeconds,
            video.Rating,
            video.IsPublished,
            video.PublishedOn,
            video.PublishedBy,
            video.CreatedOn,
            video.CreatedBy);
    }
}
