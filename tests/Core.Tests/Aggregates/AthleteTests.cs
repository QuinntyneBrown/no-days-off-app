using FluentAssertions;
using Core.Model.AthleteAggregate;
using Core.Exceptions;

namespace Core.Tests.Aggregates;

public class AthleteTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateAthlete()
    {
        // Arrange & Act
        var athlete = Athlete.Create(1, "John Doe", "johndoe", "system");

        // Assert
        athlete.Name.Should().Be("John Doe");
        athlete.Username.Should().Be("johndoe");
        athlete.TenantId.Should().Be(1);
        athlete.CreatedBy.Should().Be("system");
        athlete.CurrentWeight.Should().BeNull();
        athlete.Weights.Should().BeEmpty();
        athlete.CompletedExercises.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Athlete.Create(1, "", "johndoe", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Profile name is required");
    }

    [Fact]
    public void Create_WithEmptyUsername_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Athlete.Create(1, "John Doe", "", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Username is required");
    }

    [Fact]
    public void RecordWeight_WithValidWeight_ShouldAddWeight()
    {
        // Arrange
        var athlete = Athlete.Create(1, "John Doe", "johndoe", "system");
        var weighedOn = DateTime.UtcNow;

        // Act
        athlete.RecordWeight(75, weighedOn, "johndoe");

        // Assert
        athlete.Weights.Should().HaveCount(1);
        athlete.CurrentWeight.Should().Be(75);
        athlete.LastWeighedOn.Should().Be(weighedOn);
    }

    [Fact]
    public void RecordWeight_WithZeroWeight_ShouldThrowValidationException()
    {
        // Arrange
        var athlete = Athlete.Create(1, "John Doe", "johndoe", "system");

        // Act
        var act = () => athlete.RecordWeight(0, DateTime.UtcNow, "johndoe");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Weight must be greater than zero");
    }

    [Fact]
    public void RecordWeight_WithNegativeWeight_ShouldThrowValidationException()
    {
        // Arrange
        var athlete = Athlete.Create(1, "John Doe", "johndoe", "system");

        // Act
        var act = () => athlete.RecordWeight(-1, DateTime.UtcNow, "johndoe");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Weight must be greater than zero");
    }

    [Fact]
    public void RecordWeight_MultipleRecords_ShouldUpdateCurrentWeightToMostRecent()
    {
        // Arrange
        var athlete = Athlete.Create(1, "John Doe", "johndoe", "system");
        var oldDate = DateTime.UtcNow.AddDays(-7);
        var newDate = DateTime.UtcNow;

        // Act
        athlete.RecordWeight(70, oldDate, "johndoe");
        athlete.RecordWeight(75, newDate, "johndoe");

        // Assert
        athlete.Weights.Should().HaveCount(2);
        athlete.CurrentWeight.Should().Be(75);
        athlete.LastWeighedOn.Should().Be(newDate);
    }

    [Fact]
    public void RecordCompletedExercise_WithValidData_ShouldAddExercise()
    {
        // Arrange
        var athlete = Athlete.Create(1, "John Doe", "johndoe", "system");
        var completionTime = DateTime.UtcNow;

        // Act
        athlete.RecordCompletedExercise(1, 50, 10, 3, 0, 0, completionTime, "johndoe");

        // Assert
        athlete.CompletedExercises.Should().HaveCount(1);
        var exercise = athlete.CompletedExercises.First();
        exercise.ScheduledExerciseId.Should().Be(1);
        exercise.WeightInKgs.Should().Be(50);
        exercise.Reps.Should().Be(10);
        exercise.Sets.Should().Be(3);
    }

    [Fact]
    public void RecordCompletedExercise_WithNegativeReps_ShouldThrowValidationException()
    {
        // Arrange
        var athlete = Athlete.Create(1, "John Doe", "johndoe", "system");

        // Act
        var act = () => athlete.RecordCompletedExercise(1, 50, -1, 3, 0, 0, DateTime.UtcNow, "johndoe");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Reps cannot be negative");
    }

    [Fact]
    public void GetWeightHistory_ShouldReturnWeightsOrderedByDate()
    {
        // Arrange
        var athlete = Athlete.Create(1, "John Doe", "johndoe", "system");
        athlete.RecordWeight(70, DateTime.UtcNow.AddDays(-14), "johndoe");
        athlete.RecordWeight(72, DateTime.UtcNow.AddDays(-7), "johndoe");
        athlete.RecordWeight(75, DateTime.UtcNow, "johndoe");

        // Act
        var history = athlete.GetWeightHistory(2).ToList();

        // Assert
        history.Should().HaveCount(2);
        history[0].WeightInKgs.Should().Be(75);
        history[1].WeightInKgs.Should().Be(72);
    }

    [Fact]
    public void GetCompletedExercisesByDate_ShouldReturnExercisesForDate()
    {
        // Arrange
        var athlete = Athlete.Create(1, "John Doe", "johndoe", "system");
        var today = DateTime.UtcNow.Date;
        var yesterday = today.AddDays(-1);

        athlete.RecordCompletedExercise(1, 50, 10, 3, 0, 0, today.AddHours(9), "johndoe");
        athlete.RecordCompletedExercise(2, 60, 8, 4, 0, 0, today.AddHours(10), "johndoe");
        athlete.RecordCompletedExercise(3, 40, 12, 3, 0, 0, yesterday.AddHours(9), "johndoe");

        // Act
        var todayExercises = athlete.GetCompletedExercisesByDate(today).ToList();

        // Assert
        todayExercises.Should().HaveCount(2);
    }
}
