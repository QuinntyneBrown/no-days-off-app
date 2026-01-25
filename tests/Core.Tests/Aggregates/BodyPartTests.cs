using FluentAssertions;
using Core.Model.BodyPartAggregate;
using Core.Exceptions;

namespace Core.Tests.Aggregates;

public class BodyPartTests
{
    [Fact]
    public void Create_WithValidName_ShouldCreateBodyPart()
    {
        // Arrange & Act
        var bodyPart = BodyPart.Create(1, "Chest", "system");

        // Assert
        bodyPart.Name.Should().Be("Chest");
        bodyPart.TenantId.Should().Be(1);
        bodyPart.CreatedBy.Should().Be("system");
        bodyPart.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => BodyPart.Create(1, "", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Body part name is required");
    }

    [Fact]
    public void Create_WithNameExceedingMaxLength_ShouldThrowValidationException()
    {
        // Arrange
        var longName = new string('x', BodyPart.MaxNameLength + 1);

        // Act
        var act = () => BodyPart.Create(1, longName, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage($"Body part name cannot exceed {BodyPart.MaxNameLength} characters");
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var bodyPart = BodyPart.Create(1, "Chest", "system");

        // Act
        bodyPart.UpdateName("Upper Chest", "admin");

        // Assert
        bodyPart.Name.Should().Be("Upper Chest");
        bodyPart.LastModifiedBy.Should().Be("admin");
    }

    [Fact]
    public void Delete_ShouldMarkAsDeleted()
    {
        // Arrange
        var bodyPart = BodyPart.Create(1, "Chest", "system");

        // Act
        bodyPart.Delete();

        // Assert
        bodyPart.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Restore_ShouldMarkAsNotDeleted()
    {
        // Arrange
        var bodyPart = BodyPart.Create(1, "Chest", "system");
        bodyPart.Delete();

        // Act
        bodyPart.Restore();

        // Assert
        bodyPart.IsDeleted.Should().BeFalse();
    }
}
