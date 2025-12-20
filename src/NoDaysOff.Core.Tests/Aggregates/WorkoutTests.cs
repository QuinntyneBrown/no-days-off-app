using FluentAssertions;
using NoDaysOff.Core.Aggregates.WorkoutAggregate;

namespace NoDaysOff.Core.Tests.Aggregates;

public class WorkoutTests
{
    [Fact]
    public void Create_ShouldCreateWorkout()
    {
        // Arrange & Act
        var workout = Workout.Create(1, "system");

        // Assert
        workout.TenantId.Should().Be(1);
        workout.CreatedBy.Should().Be("system");
        workout.BodyPartIds.Should().BeEmpty();
    }

    [Fact]
    public void AddBodyPart_ShouldAddBodyPartId()
    {
        // Arrange
        var workout = Workout.Create(1, "system");

        // Act
        workout.AddBodyPart(1, "admin");
        workout.AddBodyPart(2, "admin");

        // Assert
        workout.BodyPartIds.Should().HaveCount(2);
        workout.BodyPartIds.Should().Contain(new[] { 1, 2 });
    }

    [Fact]
    public void AddBodyPart_DuplicateId_ShouldNotAddAgain()
    {
        // Arrange
        var workout = Workout.Create(1, "system");

        // Act
        workout.AddBodyPart(1, "admin");
        workout.AddBodyPart(1, "admin");

        // Assert
        workout.BodyPartIds.Should().HaveCount(1);
    }

    [Fact]
    public void RemoveBodyPart_ShouldRemoveBodyPartId()
    {
        // Arrange
        var workout = Workout.Create(1, "system");
        workout.AddBodyPart(1, "admin");
        workout.AddBodyPart(2, "admin");

        // Act
        workout.RemoveBodyPart(1, "admin");

        // Assert
        workout.BodyPartIds.Should().HaveCount(1);
        workout.BodyPartIds.Should().NotContain(1);
    }

    [Fact]
    public void ClearBodyParts_ShouldRemoveAllBodyParts()
    {
        // Arrange
        var workout = Workout.Create(1, "system");
        workout.AddBodyPart(1, "admin");
        workout.AddBodyPart(2, "admin");

        // Act
        workout.ClearBodyParts("admin");

        // Assert
        workout.BodyPartIds.Should().BeEmpty();
    }

    [Fact]
    public void HasBodyPart_WithExistingBodyPart_ShouldReturnTrue()
    {
        // Arrange
        var workout = Workout.Create(1, "system");
        workout.AddBodyPart(1, "admin");

        // Act & Assert
        workout.HasBodyPart(1).Should().BeTrue();
    }

    [Fact]
    public void HasBodyPart_WithNonExistingBodyPart_ShouldReturnFalse()
    {
        // Arrange
        var workout = Workout.Create(1, "system");

        // Act & Assert
        workout.HasBodyPart(1).Should().BeFalse();
    }
}
