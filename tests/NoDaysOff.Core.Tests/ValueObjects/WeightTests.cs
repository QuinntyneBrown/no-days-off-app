using FluentAssertions;
using NoDaysOff.Core.Exceptions;
using NoDaysOff.Core.ValueObjects;

namespace NoDaysOff.Core.Tests.ValueObjects;

public class WeightTests
{
    [Fact]
    public void FromKilograms_WithValidWeight_ShouldCreateWeight()
    {
        // Arrange & Act
        var weight = Weight.FromKilograms(75);

        // Assert
        weight.Kilograms.Should().Be(75);
    }

    [Fact]
    public void FromKilograms_WithZeroWeight_ShouldCreateWeight()
    {
        // Arrange & Act
        var weight = Weight.FromKilograms(0);

        // Assert
        weight.Kilograms.Should().Be(0);
    }

    [Fact]
    public void FromKilograms_WithNegativeWeight_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Weight.FromKilograms(-1);

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Weight cannot be negative");
    }

    [Fact]
    public void ToPounds_ShouldConvertCorrectly()
    {
        // Arrange
        var weight = Weight.FromKilograms(100);

        // Act
        var pounds = weight.ToPounds();

        // Assert
        pounds.Should().BeApproximately(220.462, 0.001);
    }

    [Fact]
    public void Zero_ShouldReturnZeroWeight()
    {
        // Arrange & Act
        var weight = Weight.Zero;

        // Assert
        weight.Kilograms.Should().Be(0);
    }

    [Fact]
    public void Equals_WithSameWeight_ShouldReturnTrue()
    {
        // Arrange
        var weight1 = Weight.FromKilograms(75);
        var weight2 = Weight.FromKilograms(75);

        // Act & Assert
        weight1.Should().Be(weight2);
        (weight1 == weight2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentWeight_ShouldReturnFalse()
    {
        // Arrange
        var weight1 = Weight.FromKilograms(75);
        var weight2 = Weight.FromKilograms(80);

        // Act & Assert
        weight1.Should().NotBe(weight2);
        (weight1 != weight2).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var weight = Weight.FromKilograms(75);

        // Act
        var result = weight.ToString();

        // Assert
        result.Should().Be("75 kg");
    }
}
