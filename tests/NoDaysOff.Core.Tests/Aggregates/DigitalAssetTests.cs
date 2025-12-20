using FluentAssertions;
using NoDaysOff.Core.Model.DigitalAssetAggregate;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Tests.Aggregates;

public class DigitalAssetTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateDigitalAsset()
    {
        // Arrange & Act
        var asset = DigitalAsset.Create("Workout Image", "workout.jpg", "image/jpeg", 1024, "system");

        // Assert
        asset.Name.Should().Be("Workout Image");
        asset.FileName.Should().Be("workout.jpg");
        asset.ContentType.Should().Be("image/jpeg");
        asset.Size.Should().Be(1024);
        asset.UniqueId.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => DigitalAsset.Create("", "workout.jpg", "image/jpeg", 1024, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Digital asset name is required");
    }

    [Fact]
    public void Create_WithEmptyFileName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => DigitalAsset.Create("Workout Image", "", "image/jpeg", 1024, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("File name is required");
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var asset = DigitalAsset.Create("Workout Image", "workout.jpg", "image/jpeg", 1024, "system");

        // Act
        asset.UpdateName("New Workout Image", "admin");

        // Assert
        asset.Name.Should().Be("New Workout Image");
    }

    [Fact]
    public void UpdateDescription_ShouldUpdateDescription()
    {
        // Arrange
        var asset = DigitalAsset.Create("Workout Image", "workout.jpg", "image/jpeg", 1024, "system");

        // Act
        asset.UpdateDescription("A great workout image", "admin");

        // Assert
        asset.Description.Should().Be("A great workout image");
    }

    [Fact]
    public void UpdateFolder_ShouldUpdateFolder()
    {
        // Arrange
        var asset = DigitalAsset.Create("Workout Image", "workout.jpg", "image/jpeg", 1024, "system");

        // Act
        asset.UpdateFolder("images/workouts", "admin");

        // Assert
        asset.Folder.Should().Be("images/workouts");
    }

    [Fact]
    public void UpdateFileInfo_ShouldUpdateFileInfo()
    {
        // Arrange
        var asset = DigitalAsset.Create("Workout Image", "workout.jpg", "image/jpeg", 1024, "system");

        // Act
        asset.UpdateFileInfo("new-workout.png", "image/png", 2048, "admin");

        // Assert
        asset.FileName.Should().Be("new-workout.png");
        asset.ContentType.Should().Be("image/png");
        asset.Size.Should().Be(2048);
    }

    [Fact]
    public void RelativePath_ShouldReturnCorrectPath()
    {
        // Arrange
        var asset = DigitalAsset.Create("Workout Image", "workout.jpg", "image/jpeg", 1024, "system");
        asset.UpdateFolder("images", "admin");

        // Act
        var relativePath = asset.RelativePath;

        // Assert
        relativePath.Should().Contain("images");
        relativePath.Should().Contain("workout.jpg");
        relativePath.Should().Contain(asset.UniqueId.ToString());
    }
}
