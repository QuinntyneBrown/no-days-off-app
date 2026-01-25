using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Workouts.Core.Features.Workouts.CreateWorkout;
using Workouts.Infrastructure.Data;
using NoDaysOff.Tests.Common.Mocks;
using Xunit;

namespace Workouts.Core.Tests.Features;

public class CreateWorkoutTests
{
    private readonly WorkoutsDbContext _context;
    private readonly MockMessageBus _messageBus;

    public CreateWorkoutTests()
    {
        var options = new DbContextOptionsBuilder<WorkoutsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new WorkoutsDbContext(options);
        _messageBus = new MockMessageBus();
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesWorkout()
    {
        // Arrange
        var handler = new CreateWorkoutHandler(_context, _messageBus);
        var command = new CreateWorkoutCommand("Upper Body", 1, "test@test.com");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Upper Body");
        result.TenantId.Should().Be(1);

        var workoutInDb = await _context.Workouts.FirstOrDefaultAsync();
        workoutInDb.Should().NotBeNull();
        workoutInDb!.Name.Should().Be("Upper Body");
    }

    [Fact]
    public async Task Handle_ValidCommand_PublishesWorkoutCreatedMessage()
    {
        // Arrange
        var handler = new CreateWorkoutHandler(_context, _messageBus);
        var command = new CreateWorkoutCommand("Upper Body", 1, "test@test.com");

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _messageBus.PublishedMessages.Should().ContainSingle();
        _messageBus.PublishedMessages[0].Topic.Should().Be("workout.created");
    }
}
