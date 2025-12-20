using FluentAssertions;
using NoDaysOff.Core.Model.DashboardAggregate;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Tests.Aggregates;

public class DashboardTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateDashboard()
    {
        // Arrange & Act
        var dashboard = Dashboard.Create(1, "Main Dashboard", "johndoe", true, "system");

        // Assert
        dashboard.Name.Should().Be("Main Dashboard");
        dashboard.Username.Should().Be("johndoe");
        dashboard.IsDefault.Should().BeTrue();
        dashboard.Tiles.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Dashboard.Create(1, "", "johndoe", false, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Dashboard name is required");
    }

    [Fact]
    public void AddTile_WithValidData_ShouldAddTile()
    {
        // Arrange
        var dashboard = Dashboard.Create(1, "Main Dashboard", "johndoe", true, "system");

        // Act
        dashboard.AddTile(1, "Weather Tile", 0, 0, 2, 2, "admin");

        // Assert
        dashboard.Tiles.Should().HaveCount(1);
        var tile = dashboard.Tiles.First();
        tile.Name.Should().Be("Weather Tile");
        tile.TileId.Should().Be(1);
        tile.Top.Should().Be(0);
        tile.Left.Should().Be(0);
        tile.Width.Should().Be(2);
        tile.Height.Should().Be(2);
    }

    [Fact]
    public void AddTile_WithZeroWidth_ShouldThrowValidationException()
    {
        // Arrange
        var dashboard = Dashboard.Create(1, "Main Dashboard", "johndoe", true, "system");

        // Act
        var act = () => dashboard.AddTile(1, "Weather Tile", 0, 0, 0, 2, "admin");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Width must be greater than zero");
    }

    [Fact]
    public void RemoveTile_WithValidIndex_ShouldRemoveTile()
    {
        // Arrange
        var dashboard = Dashboard.Create(1, "Main Dashboard", "johndoe", true, "system");
        dashboard.AddTile(1, "Weather Tile", 0, 0, 2, 2, "admin");
        dashboard.AddTile(2, "Stats Tile", 0, 2, 2, 2, "admin");

        // Act
        dashboard.RemoveTile(0, "admin");

        // Assert
        dashboard.Tiles.Should().HaveCount(1);
    }

    [Fact]
    public void UpdateTilePosition_ShouldUpdatePosition()
    {
        // Arrange
        var dashboard = Dashboard.Create(1, "Main Dashboard", "johndoe", true, "system");
        dashboard.AddTile(1, "Weather Tile", 0, 0, 2, 2, "admin");

        // Act
        dashboard.UpdateTilePosition(0, 5, 10, "admin");

        // Assert
        var tile = dashboard.Tiles.First();
        tile.Top.Should().Be(5);
        tile.Left.Should().Be(10);
    }

    [Fact]
    public void UpdateTileSize_ShouldUpdateSize()
    {
        // Arrange
        var dashboard = Dashboard.Create(1, "Main Dashboard", "johndoe", true, "system");
        dashboard.AddTile(1, "Weather Tile", 0, 0, 2, 2, "admin");

        // Act
        dashboard.UpdateTileSize(0, 4, 3, "admin");

        // Assert
        var tile = dashboard.Tiles.First();
        tile.Width.Should().Be(4);
        tile.Height.Should().Be(3);
    }

    [Fact]
    public void SetAsDefault_ShouldSetIsDefaultToTrue()
    {
        // Arrange
        var dashboard = Dashboard.Create(1, "Main Dashboard", "johndoe", false, "system");

        // Act
        dashboard.SetAsDefault("admin");

        // Assert
        dashboard.IsDefault.Should().BeTrue();
    }

    [Fact]
    public void ClearDefault_ShouldSetIsDefaultToFalse()
    {
        // Arrange
        var dashboard = Dashboard.Create(1, "Main Dashboard", "johndoe", true, "system");

        // Act
        dashboard.ClearDefault("admin");

        // Assert
        dashboard.IsDefault.Should().BeFalse();
    }

    [Fact]
    public void ClearTiles_ShouldRemoveAllTiles()
    {
        // Arrange
        var dashboard = Dashboard.Create(1, "Main Dashboard", "johndoe", true, "system");
        dashboard.AddTile(1, "Weather Tile", 0, 0, 2, 2, "admin");
        dashboard.AddTile(2, "Stats Tile", 0, 2, 2, 2, "admin");

        // Act
        dashboard.ClearTiles("admin");

        // Assert
        dashboard.Tiles.Should().BeEmpty();
    }
}
