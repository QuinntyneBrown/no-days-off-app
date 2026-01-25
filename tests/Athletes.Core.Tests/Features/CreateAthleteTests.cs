using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Athletes.Core.Features.Athletes.CreateAthlete;
using Athletes.Infrastructure.Data;
using NoDaysOff.Tests.Common.Mocks;
using Xunit;

namespace Athletes.Core.Tests.Features;

public class CreateAthleteTests
{
    private readonly AthletesDbContext _context;
    private readonly MockMessageBus _messageBus;

    public CreateAthleteTests()
    {
        var options = new DbContextOptionsBuilder<AthletesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AthletesDbContext(options);
        _messageBus = new MockMessageBus();
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesAthlete()
    {
        // Arrange
        var handler = new CreateAthleteHandler(_context, _messageBus);
        var command = new CreateAthleteCommand("John", "Doe", 1, "test@test.com");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.TenantId.Should().Be(1);

        var athleteInDb = await _context.Athletes.FirstOrDefaultAsync();
        athleteInDb.Should().NotBeNull();
        athleteInDb!.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task Handle_ValidCommand_PublishesMessage()
    {
        // Arrange
        var handler = new CreateAthleteHandler(_context, _messageBus);
        var command = new CreateAthleteCommand("John", "Doe", 1, "test@test.com");

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _messageBus.PublishedMessages.Should().ContainSingle();
        _messageBus.PublishedMessages[0].Topic.Should().Be("athlete.created");
    }
}
