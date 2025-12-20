using NoDaysOff.Core.Abstractions;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Model.VideoAggregate;

/// <summary>
/// Aggregate root for video content management
/// </summary>
public sealed class Video : AggregateRoot
{
    public const int MaxTitleLength = 256;

    public string Title { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string SubTitle { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string YouTubeVideoId { get; private set; } = string.Empty;
    public string Abstract { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int DurationInSeconds { get; private set; }
    public decimal Rating { get; private set; }
    public DateTime? PublishedOn { get; private set; }
    public string PublishedBy { get; private set; } = string.Empty;

    private Video() : base()
    {
    }

    private Video(int? tenantId) : base(tenantId)
    {
    }

    public static Video Create(int? tenantId, string title, string youTubeVideoId, string createdBy)
    {
        ValidateTitle(title);
        ValidateYouTubeVideoId(youTubeVideoId);

        var video = new Video(tenantId)
        {
            Title = title,
            YouTubeVideoId = youTubeVideoId,
            Slug = GenerateSlug(title)
        };
        video.SetAuditInfo(createdBy);

        return video;
    }

    public void UpdateTitle(string title, string modifiedBy)
    {
        ValidateTitle(title);
        Title = title;
        Slug = GenerateSlug(title);
        UpdateModified(modifiedBy);
    }

    public void UpdateContent(string subTitle, string @abstract, string description, string modifiedBy)
    {
        SubTitle = subTitle ?? string.Empty;
        Abstract = @abstract ?? string.Empty;
        Description = description ?? string.Empty;
        UpdateModified(modifiedBy);
    }

    public void UpdateCategory(string category, string modifiedBy)
    {
        Category = category ?? string.Empty;
        UpdateModified(modifiedBy);
    }

    public void UpdateYouTubeVideoId(string youTubeVideoId, string modifiedBy)
    {
        ValidateYouTubeVideoId(youTubeVideoId);
        YouTubeVideoId = youTubeVideoId;
        UpdateModified(modifiedBy);
    }

    public void UpdateDuration(int durationInSeconds, string modifiedBy)
    {
        if (durationInSeconds < 0)
        {
            throw new ValidationException("Duration cannot be negative");
        }

        DurationInSeconds = durationInSeconds;
        UpdateModified(modifiedBy);
    }

    public void UpdateRating(decimal rating, string modifiedBy)
    {
        if (rating < 0 || rating > 5)
        {
            throw new ValidationException("Rating must be between 0 and 5");
        }

        Rating = rating;
        UpdateModified(modifiedBy);
    }

    public void Publish(string publishedBy)
    {
        PublishedOn = DateTime.UtcNow;
        PublishedBy = publishedBy;
        UpdateModified(publishedBy);
    }

    public void Unpublish(string modifiedBy)
    {
        PublishedOn = null;
        PublishedBy = string.Empty;
        UpdateModified(modifiedBy);
    }

    public bool IsPublished => PublishedOn.HasValue;

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ValidationException("Video title is required");
        }

        if (title.Length > MaxTitleLength)
        {
            throw new ValidationException($"Video title cannot exceed {MaxTitleLength} characters");
        }
    }

    private static void ValidateYouTubeVideoId(string youTubeVideoId)
    {
        if (string.IsNullOrWhiteSpace(youTubeVideoId))
        {
            throw new ValidationException("YouTube video ID is required");
        }
    }

    private static string GenerateSlug(string title)
    {
        return title
            .ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("--", "-");
    }
}
