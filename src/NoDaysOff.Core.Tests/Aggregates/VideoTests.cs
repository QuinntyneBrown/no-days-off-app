using FluentAssertions;
using NoDaysOff.Core.Aggregates.VideoAggregate;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Tests.Aggregates;

public class VideoTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateVideo()
    {
        // Arrange & Act
        var video = Video.Create(1, "Chest Workout Tutorial", "abc123", "system");

        // Assert
        video.Title.Should().Be("Chest Workout Tutorial");
        video.YouTubeVideoId.Should().Be("abc123");
        video.Slug.Should().Be("chest-workout-tutorial");
        video.IsPublished.Should().BeFalse();
    }

    [Fact]
    public void Create_WithEmptyTitle_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Video.Create(1, "", "abc123", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Video title is required");
    }

    [Fact]
    public void Create_WithEmptyYouTubeVideoId_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Video.Create(1, "Chest Workout", "", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("YouTube video ID is required");
    }

    [Fact]
    public void UpdateTitle_ShouldUpdateTitleAndSlug()
    {
        // Arrange
        var video = Video.Create(1, "Chest Workout Tutorial", "abc123", "system");

        // Act
        video.UpdateTitle("Leg Day Tutorial", "admin");

        // Assert
        video.Title.Should().Be("Leg Day Tutorial");
        video.Slug.Should().Be("leg-day-tutorial");
    }

    [Fact]
    public void UpdateContent_ShouldUpdateContentFields()
    {
        // Arrange
        var video = Video.Create(1, "Chest Workout", "abc123", "system");

        // Act
        video.UpdateContent("Subtitle", "Abstract text", "Full description", "admin");

        // Assert
        video.SubTitle.Should().Be("Subtitle");
        video.Abstract.Should().Be("Abstract text");
        video.Description.Should().Be("Full description");
    }

    [Fact]
    public void UpdateDuration_WithValidDuration_ShouldUpdateDuration()
    {
        // Arrange
        var video = Video.Create(1, "Chest Workout", "abc123", "system");

        // Act
        video.UpdateDuration(600, "admin");

        // Assert
        video.DurationInSeconds.Should().Be(600);
    }

    [Fact]
    public void UpdateDuration_WithNegativeDuration_ShouldThrowValidationException()
    {
        // Arrange
        var video = Video.Create(1, "Chest Workout", "abc123", "system");

        // Act
        var act = () => video.UpdateDuration(-1, "admin");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Duration cannot be negative");
    }

    [Fact]
    public void UpdateRating_WithValidRating_ShouldUpdateRating()
    {
        // Arrange
        var video = Video.Create(1, "Chest Workout", "abc123", "system");

        // Act
        video.UpdateRating(4.5m, "admin");

        // Assert
        video.Rating.Should().Be(4.5m);
    }

    [Fact]
    public void UpdateRating_WithRatingAbove5_ShouldThrowValidationException()
    {
        // Arrange
        var video = Video.Create(1, "Chest Workout", "abc123", "system");

        // Act
        var act = () => video.UpdateRating(5.5m, "admin");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Rating must be between 0 and 5");
    }

    [Fact]
    public void Publish_ShouldSetPublishedOnAndPublishedBy()
    {
        // Arrange
        var video = Video.Create(1, "Chest Workout", "abc123", "system");

        // Act
        video.Publish("admin");

        // Assert
        video.IsPublished.Should().BeTrue();
        video.PublishedOn.Should().NotBeNull();
        video.PublishedBy.Should().Be("admin");
    }

    [Fact]
    public void Unpublish_ShouldClearPublishedInfo()
    {
        // Arrange
        var video = Video.Create(1, "Chest Workout", "abc123", "system");
        video.Publish("admin");

        // Act
        video.Unpublish("admin");

        // Assert
        video.IsPublished.Should().BeFalse();
        video.PublishedOn.Should().BeNull();
        video.PublishedBy.Should().BeEmpty();
    }
}
