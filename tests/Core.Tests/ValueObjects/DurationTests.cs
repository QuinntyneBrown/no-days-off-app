using FluentAssertions;
using Core.Exceptions;
using Core.ValueObjects;

namespace Core.Tests.ValueObjects;

public class DurationTests
{
    [Fact]
    public void FromSeconds_WithValidDuration_ShouldCreateDuration()
    {
        // Arrange & Act
        var duration = Duration.FromSeconds(120);

        // Assert
        duration.Seconds.Should().Be(120);
    }

    [Fact]
    public void FromSeconds_WithZero_ShouldCreateDuration()
    {
        // Arrange & Act
        var duration = Duration.FromSeconds(0);

        // Assert
        duration.Seconds.Should().Be(0);
    }

    [Fact]
    public void FromSeconds_WithNegativeValue_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Duration.FromSeconds(-1);

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Duration cannot be negative");
    }

    [Fact]
    public void FromMinutes_ShouldConvertToSeconds()
    {
        // Arrange & Act
        var duration = Duration.FromMinutes(5);

        // Assert
        duration.Seconds.Should().Be(300);
    }

    [Fact]
    public void ToMinutes_ShouldConvertCorrectly()
    {
        // Arrange
        var duration = Duration.FromSeconds(150);

        // Act
        var minutes = duration.ToMinutes();

        // Assert
        minutes.Should().Be(2); // 150 / 60 = 2 (integer division)
    }

    [Fact]
    public void ToTimeSpan_ShouldReturnCorrectTimeSpan()
    {
        // Arrange
        var duration = Duration.FromSeconds(3665); // 1 hour, 1 minute, 5 seconds

        // Act
        var timeSpan = duration.ToTimeSpan();

        // Assert
        timeSpan.Hours.Should().Be(1);
        timeSpan.Minutes.Should().Be(1);
        timeSpan.Seconds.Should().Be(5);
    }

    [Fact]
    public void Zero_ShouldReturnZeroDuration()
    {
        // Arrange & Act
        var duration = Duration.Zero;

        // Assert
        duration.Seconds.Should().Be(0);
    }

    [Fact]
    public void Equals_WithSameDuration_ShouldReturnTrue()
    {
        // Arrange
        var duration1 = Duration.FromSeconds(120);
        var duration2 = Duration.FromSeconds(120);

        // Act & Assert
        duration1.Should().Be(duration2);
    }

    [Fact]
    public void ToString_WithHours_ShouldReturnFormattedString()
    {
        // Arrange
        var duration = Duration.FromSeconds(3665); // 1:01:05

        // Act
        var result = duration.ToString();

        // Assert
        result.Should().Be("01:01:05");
    }

    [Fact]
    public void ToString_WithoutHours_ShouldReturnFormattedString()
    {
        // Arrange
        var duration = Duration.FromSeconds(125); // 2:05

        // Act
        var result = duration.ToString();

        // Assert
        result.Should().Be("02:05");
    }
}
