using FluentAssertions;
using NoDaysOff.Core.Model.ScheduledExerciseAggregate;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Tests.Aggregates;

public class ScheduledExerciseTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateScheduledExercise()
    {
        // Arrange & Act
        var scheduledExercise = ScheduledExercise.Create(1, "Morning Bench Press", 1, 1, 0, "system");

        // Assert
        scheduledExercise.Name.Should().Be("Morning Bench Press");
        scheduledExercise.DayId.Should().Be(1);
        scheduledExercise.ExerciseId.Should().Be(1);
        scheduledExercise.Sort.Should().Be(0);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => ScheduledExercise.Create(1, "", 1, 1, 0, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Scheduled exercise name is required");
    }

    [Fact]
    public void Create_WithNegativeSort_ShouldSetSortToZero()
    {
        // Arrange & Act
        var scheduledExercise = ScheduledExercise.Create(1, "Bench Press", 1, 1, -5, "system");

        // Assert
        scheduledExercise.Sort.Should().Be(0);
    }

    [Fact]
    public void UpdatePerformanceTargets_WithValidData_ShouldUpdateTargets()
    {
        // Arrange
        var scheduledExercise = ScheduledExercise.Create(1, "Bench Press", 1, 1, 0, "system");

        // Act
        scheduledExercise.UpdatePerformanceTargets(10, 3, 50, "admin");

        // Assert
        scheduledExercise.Repetitions.Should().Be(10);
        scheduledExercise.Sets.Should().Be(3);
        scheduledExercise.WeightInKgs.Should().Be(50);
    }

    [Fact]
    public void UpdatePerformanceTargets_WithNegativeRepetitions_ShouldThrowValidationException()
    {
        // Arrange
        var scheduledExercise = ScheduledExercise.Create(1, "Bench Press", 1, 1, 0, "system");

        // Act
        var act = () => scheduledExercise.UpdatePerformanceTargets(-1, 3, 50, "admin");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Repetitions cannot be negative");
    }

    [Fact]
    public void UpdateCardioTargets_WithValidData_ShouldUpdateTargets()
    {
        // Arrange
        var scheduledExercise = ScheduledExercise.Create(1, "Treadmill", 1, 1, 0, "system");

        // Act
        scheduledExercise.UpdateCardioTargets(5000, 1800, "admin");

        // Assert
        scheduledExercise.Distance.Should().Be(5000);
        scheduledExercise.TimeInSeconds.Should().Be(1800);
    }

    [Fact]
    public void UpdateCardioTargets_WithNegativeDistance_ShouldThrowValidationException()
    {
        // Arrange
        var scheduledExercise = ScheduledExercise.Create(1, "Treadmill", 1, 1, 0, "system");

        // Act
        var act = () => scheduledExercise.UpdateCardioTargets(-1, 1800, "admin");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Distance cannot be negative");
    }

    [Fact]
    public void AddSet_ShouldAddSetToCollection()
    {
        // Arrange
        var scheduledExercise = ScheduledExercise.Create(1, "Bench Press", 1, 1, 0, "system");

        // Act
        scheduledExercise.AddSet(50, 10, "admin");
        scheduledExercise.AddSet(55, 8, "admin");

        // Assert
        scheduledExercise.SetCollection.Should().HaveCount(2);
    }

    [Fact]
    public void RemoveSet_WithValidIndex_ShouldRemoveSet()
    {
        // Arrange
        var scheduledExercise = ScheduledExercise.Create(1, "Bench Press", 1, 1, 0, "system");
        scheduledExercise.AddSet(50, 10, "admin");
        scheduledExercise.AddSet(55, 8, "admin");

        // Act
        scheduledExercise.RemoveSet(0, "admin");

        // Assert
        scheduledExercise.SetCollection.Should().HaveCount(1);
    }

    [Fact]
    public void ClearSets_ShouldRemoveAllSets()
    {
        // Arrange
        var scheduledExercise = ScheduledExercise.Create(1, "Bench Press", 1, 1, 0, "system");
        scheduledExercise.AddSet(50, 10, "admin");
        scheduledExercise.AddSet(55, 8, "admin");

        // Act
        scheduledExercise.ClearSets("admin");

        // Assert
        scheduledExercise.SetCollection.Should().BeEmpty();
    }

    [Fact]
    public void UpdateSchedule_ShouldUpdateDayAndSort()
    {
        // Arrange
        var scheduledExercise = ScheduledExercise.Create(1, "Bench Press", 1, 1, 0, "system");

        // Act
        scheduledExercise.UpdateSchedule(2, 5, "admin");

        // Assert
        scheduledExercise.DayId.Should().Be(2);
        scheduledExercise.Sort.Should().Be(5);
    }
}
