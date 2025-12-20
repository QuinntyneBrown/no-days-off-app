using FluentAssertions;
using NoDaysOff.Core.Aggregates.TenantAggregate;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Tests.Aggregates;

public class TenantTests
{
    [Fact]
    public void Create_WithValidName_ShouldCreateTenant()
    {
        // Arrange & Act
        var tenant = Tenant.Create("Test Tenant", "system");

        // Assert
        tenant.Name.Should().Be("Test Tenant");
        tenant.UniqueId.Should().NotBeEmpty();
        tenant.CreatedBy.Should().Be("system");
        tenant.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Tenant.Create("", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Tenant name is required");
    }

    [Fact]
    public void Create_WithWhitespaceName_ShouldThrowValidationException()
    {
        // Arrange & Act
        var act = () => Tenant.Create("   ", "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Tenant name is required");
    }

    [Fact]
    public void Create_WithNameExceedingMaxLength_ShouldThrowValidationException()
    {
        // Arrange
        var longName = new string('x', Tenant.MaxNameLength + 1);

        // Act
        var act = () => Tenant.Create(longName, "system");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage($"Tenant name cannot exceed {Tenant.MaxNameLength} characters");
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var tenant = Tenant.Create("Original Name", "system");

        // Act
        tenant.UpdateName("Updated Name", "admin");

        // Assert
        tenant.Name.Should().Be("Updated Name");
        tenant.LastModifiedBy.Should().Be("admin");
    }

    [Fact]
    public void Delete_ShouldMarkAsDeleted()
    {
        // Arrange
        var tenant = Tenant.Create("Test Tenant", "system");

        // Act
        tenant.Delete();

        // Assert
        tenant.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Restore_ShouldMarkAsNotDeleted()
    {
        // Arrange
        var tenant = Tenant.Create("Test Tenant", "system");
        tenant.Delete();

        // Act
        tenant.Restore();

        // Assert
        tenant.IsDeleted.Should().BeFalse();
    }
}
