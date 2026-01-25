using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Athletes.Core.Aggregates.Athlete;
using Athletes.Core.Features.Athletes.GetAthletes;
using Athletes.Infrastructure.Data;
using Xunit;

namespace Athletes.Core.Tests.Features;

public class GetAthletesTests
{
    private readonly AthletesDbContext _context;

    public GetAthletesTests()
    {
        var options = new DbContextOptionsBuilder<AthletesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AthletesDbContext(options);
    }

    [Fact]
    public async Task Handle_WithAthletes_ReturnsFilteredByTenant()
    {
        // Arrange
        _context.Athletes.AddRange(
            new Athlete("John", "Doe", 1),
            new Athlete("Jane", "Doe", 1),
            new Athlete("Bob", "Smith", 2)
        );
        await _context.SaveChangesAsync();

        var handler = new GetAthletesHandler(_context);
        var query = new GetAthletesQuery(1);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(a => a.TenantId.Should().Be(1));
    }

    [Fact]
    public async Task Handle_EmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        var handler = new GetAthletesHandler(_context);
        var query = new GetAthletesQuery(1);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
