using FluentAssertions;
using NoDaysOff.Core.Model.DayAggregate;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Tests.Aggregates;

public class DayTests
{
    [Fact]
    public void Create_WithValidName_ShouldCreateDay()
    {
        // Arrange & Act
        var day = Day.Create(1, "Monday", "system");

        // Assert
        day.Name.Should().Be("Monday");
        day.TenantId.Should().Be(1);
        day.BodyPartIds.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Day.Create(1, "", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Day name is required");
    }

    [Fact]
    public void AssignBodyPart_ShouldAddBodyPartId()
    {
        // Arrange
        var day = Day.Create(1, "Monday", "system");

        // Act
        day.AssignBodyPart(1, "admin");
        day.AssignBodyPart(2, "admin");

        // Assert
        day.BodyPartIds.Should().HaveCount(2);
        day.BodyPartIds.Should().Contain(new[] { 1, 2 });
    }

    [Fact]
    public void AssignBodyPart_DuplicateId_ShouldNotAddAgain()
    {
        // Arrange
        var day = Day.Create(1, "Monday", "system");

        // Act
        day.AssignBodyPart(1, "admin");
        day.AssignBodyPart(1, "admin");

        // Assert
        day.BodyPartIds.Should().HaveCount(1);
    }

    [Fact]
    public void RemoveBodyPart_ShouldRemoveBodyPartId()
    {
        // Arrange
        var day = Day.Create(1, "Monday", "system");
        day.AssignBodyPart(1, "admin");
        day.AssignBodyPart(2, "admin");

        // Act
        day.RemoveBodyPart(1, "admin");

        // Assert
        day.BodyPartIds.Should().HaveCount(1);
        day.BodyPartIds.Should().NotContain(1);
    }

    [Fact]
    public void ClearBodyParts_ShouldRemoveAllBodyParts()
    {
        // Arrange
        var day = Day.Create(1, "Monday", "system");
        day.AssignBodyPart(1, "admin");
        day.AssignBodyPart(2, "admin");

        // Act
        day.ClearBodyParts("admin");

        // Assert
        day.BodyPartIds.Should().BeEmpty();
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var day = Day.Create(1, "Monday", "system");

        // Act
        day.UpdateName("Push Day", "admin");

        // Assert
        day.Name.Should().Be("Push Day");
    }
}
