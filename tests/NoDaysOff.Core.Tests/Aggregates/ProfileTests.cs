using FluentAssertions;
using NoDaysOff.Core.Model.ProfileAggregate;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Tests.Aggregates;

public class ProfileTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateProfile()
    {
        // Arrange & Act
        var profile = Profile.Create(1, "John Doe", "johndoe", "system");

        // Assert
        profile.Name.Should().Be("John Doe");
        profile.Username.Should().Be("johndoe");
        profile.TenantId.Should().Be(1);
        profile.ImageUrl.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Profile.Create(1, "", "johndoe", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Profile name is required");
    }

    [Fact]
    public void Create_WithEmptyUsername_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Profile.Create(1, "John Doe", "", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Username is required");
    }

    [Fact]
    public void Create_WithNameExceedingMaxLength_ShouldThrowValidationException()
    {
        // Arrange
        var longName = new string('x', Profile.MaxNameLength + 1);

        // Act
        var act = () => Profile.Create(1, longName, "johndoe", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage($"Profile name cannot exceed {Profile.MaxNameLength} characters");
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var profile = Profile.Create(1, "John Doe", "johndoe", "system");

        // Act
        profile.UpdateName("Jane Doe", "admin");

        // Assert
        profile.Name.Should().Be("Jane Doe");
        profile.LastModifiedBy.Should().Be("admin");
    }

    [Fact]
    public void UpdateUsername_WithValidUsername_ShouldUpdateUsername()
    {
        // Arrange
        var profile = Profile.Create(1, "John Doe", "johndoe", "system");

        // Act
        profile.UpdateUsername("johnnyd", "admin");

        // Assert
        profile.Username.Should().Be("johnnyd");
    }

    [Fact]
    public void UpdateImageUrl_ShouldUpdateImageUrl()
    {
        // Arrange
        var profile = Profile.Create(1, "John Doe", "johndoe", "system");

        // Act
        profile.UpdateImageUrl("https://example.com/avatar.jpg", "admin");

        // Assert
        profile.ImageUrl.Should().Be("https://example.com/avatar.jpg");
    }

    [Fact]
    public void UpdateImageUrl_WithNull_ShouldSetToEmpty()
    {
        // Arrange
        var profile = Profile.Create(1, "John Doe", "johndoe", "system");
        profile.UpdateImageUrl("https://example.com/avatar.jpg", "admin");

        // Act
        profile.UpdateImageUrl(null!, "admin");

        // Assert
        profile.ImageUrl.Should().BeEmpty();
    }
}
