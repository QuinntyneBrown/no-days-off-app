using FluentAssertions;
using NoDaysOff.Core.Model.ExerciseAggregate;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Tests.Aggregates;

public class ExerciseTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateExercise()
    {
        // Arrange & Act
        var exercise = Exercise.Create(1, "Bench Press", 1, 3, 10, "system");

        // Assert
        exercise.Name.Should().Be("Bench Press");
        exercise.BodyPartId.Should().Be(1);
        exercise.DefaultSets.Should().Be(3);
        exercise.DefaultRepetitions.Should().Be(10);
        exercise.TenantId.Should().Be(1);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Exercise.Create(1, "", 1, 3, 10, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Exercise name is required");
    }

    [Fact]
    public void Create_WithNegativeDefaultSets_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Exercise.Create(1, "Bench Press", 1, -1, 10, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Default sets cannot be negative");
    }

    [Fact]
    public void Create_WithNegativeDefaultRepetitions_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Exercise.Create(1, "Bench Press", 1, 3, -1, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Default repetitions cannot be negative");
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var exercise = Exercise.Create(1, "Bench Press", 1, 3, 10, "system");

        // Act
        exercise.UpdateName("Incline Bench Press", "admin");

        // Assert
        exercise.Name.Should().Be("Incline Bench Press");
        exercise.LastModifiedBy.Should().Be("admin");
    }

    [Fact]
    public void UpdateBodyPart_ShouldUpdateBodyPartId()
    {
        // Arrange
        var exercise = Exercise.Create(1, "Bench Press", 1, 3, 10, "system");

        // Act
        exercise.UpdateBodyPart(2, "admin");

        // Assert
        exercise.BodyPartId.Should().Be(2);
    }

    [Fact]
    public void UpdateDefaults_WithValidValues_ShouldUpdateDefaults()
    {
        // Arrange
        var exercise = Exercise.Create(1, "Bench Press", 1, 3, 10, "system");

        // Act
        exercise.UpdateDefaults(4, 12, "admin");

        // Assert
        exercise.DefaultSets.Should().Be(4);
        exercise.DefaultRepetitions.Should().Be(12);
    }

    [Fact]
    public void AddDefaultSet_ShouldAddSetToCollection()
    {
        // Arrange
        var exercise = Exercise.Create(1, "Bench Press", 1, 3, 10, "system");

        // Act
        exercise.AddDefaultSet(50, 10, "admin");
        exercise.AddDefaultSet(55, 8, "admin");

        // Assert
        exercise.DefaultSetCollection.Should().HaveCount(2);
    }

    [Fact]
    public void RemoveDefaultSet_WithValidIndex_ShouldRemoveSet()
    {
        // Arrange
        var exercise = Exercise.Create(1, "Bench Press", 1, 3, 10, "system");
        exercise.AddDefaultSet(50, 10, "admin");
        exercise.AddDefaultSet(55, 8, "admin");

        // Act
        exercise.RemoveDefaultSet(0, "admin");

        // Assert
        exercise.DefaultSetCollection.Should().HaveCount(1);
    }

    [Fact]
    public void ClearDefaultSets_ShouldRemoveAllSets()
    {
        // Arrange
        var exercise = Exercise.Create(1, "Bench Press", 1, 3, 10, "system");
        exercise.AddDefaultSet(50, 10, "admin");
        exercise.AddDefaultSet(55, 8, "admin");

        // Act
        exercise.ClearDefaultSets("admin");

        // Assert
        exercise.DefaultSetCollection.Should().BeEmpty();
    }

    [Fact]
    public void AddDigitalAsset_ShouldAddAssetId()
    {
        // Arrange
        var exercise = Exercise.Create(1, "Bench Press", 1, 3, 10, "system");

        // Act
        exercise.AddDigitalAsset(1, "admin");
        exercise.AddDigitalAsset(2, "admin");

        // Assert
        exercise.DigitalAssetIds.Should().HaveCount(2);
        exercise.DigitalAssetIds.Should().Contain(new[] { 1, 2 });
    }

    [Fact]
    public void AddDigitalAsset_DuplicateId_ShouldNotAddAgain()
    {
        // Arrange
        var exercise = Exercise.Create(1, "Bench Press", 1, 3, 10, "system");

        // Act
        exercise.AddDigitalAsset(1, "admin");
        exercise.AddDigitalAsset(1, "admin");

        // Assert
        exercise.DigitalAssetIds.Should().HaveCount(1);
    }

    [Fact]
    public void RemoveDigitalAsset_ShouldRemoveAssetId()
    {
        // Arrange
        var exercise = Exercise.Create(1, "Bench Press", 1, 3, 10, "system");
        exercise.AddDigitalAsset(1, "admin");
        exercise.AddDigitalAsset(2, "admin");

        // Act
        exercise.RemoveDigitalAsset(1, "admin");

        // Assert
        exercise.DigitalAssetIds.Should().HaveCount(1);
        exercise.DigitalAssetIds.Should().NotContain(1);
    }
}
