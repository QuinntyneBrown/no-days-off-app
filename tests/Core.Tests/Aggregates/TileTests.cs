using FluentAssertions;
using Core.Model.TileAggregate;
using Core.Exceptions;

namespace Core.Tests.Aggregates;

public class TileTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateTile()
    {
        // Arrange & Act
        var tile = Tile.Create(1, "Weather Tile", 200, 150, "system");

        // Assert
        tile.Name.Should().Be("Weather Tile");
        tile.DefaultWidth.Should().Be(200);
        tile.DefaultHeight.Should().Be(150);
        tile.IsVisibleInCatalog.Should().BeTrue();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Tile.Create(1, "", 200, 150, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Tile name is required");
    }

    [Fact]
    public void Create_WithZeroWidth_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Tile.Create(1, "Weather Tile", 0, 150, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Default width must be greater than zero");
    }

    [Fact]
    public void Create_WithZeroHeight_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Tile.Create(1, "Weather Tile", 200, 0, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Default height must be greater than zero");
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var tile = Tile.Create(1, "Weather Tile", 200, 150, "system");

        // Act
        tile.UpdateName("Calendar Tile", "admin");

        // Assert
        tile.Name.Should().Be("Calendar Tile");
    }

    [Fact]
    public void UpdateDefaultDimensions_WithValidDimensions_ShouldUpdateDimensions()
    {
        // Arrange
        var tile = Tile.Create(1, "Weather Tile", 200, 150, "system");

        // Act
        tile.UpdateDefaultDimensions(300, 200, "admin");

        // Assert
        tile.DefaultWidth.Should().Be(300);
        tile.DefaultHeight.Should().Be(200);
    }

    [Fact]
    public void ShowInCatalog_ShouldSetVisibleToTrue()
    {
        // Arrange
        var tile = Tile.Create(1, "Weather Tile", 200, 150, "system");
        tile.HideFromCatalog("admin");

        // Act
        tile.ShowInCatalog("admin");

        // Assert
        tile.IsVisibleInCatalog.Should().BeTrue();
    }

    [Fact]
    public void HideFromCatalog_ShouldSetVisibleToFalse()
    {
        // Arrange
        var tile = Tile.Create(1, "Weather Tile", 200, 150, "system");

        // Act
        tile.HideFromCatalog("admin");

        // Assert
        tile.IsVisibleInCatalog.Should().BeFalse();
    }
}
