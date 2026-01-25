using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Identity.Core.Features.Auth.Register;
using Identity.Core.Services;
using Identity.Infrastructure.Data;
using NoDaysOff.Tests.Common.Mocks;
using Xunit;

namespace Identity.Core.Tests.Features;

public class RegisterUserTests
{
    private readonly IdentityDbContext _context;
    private readonly MockMessageBus _messageBus;
    private readonly Mock<IPasswordHasher> _passwordHasher;
    private readonly Mock<ITokenService> _tokenService;

    public RegisterUserTests()
    {
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new IdentityDbContext(options);
        _messageBus = new MockMessageBus();
        _passwordHasher = new Mock<IPasswordHasher>();
        _tokenService = new Mock<ITokenService>();

        _passwordHasher.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed_password");
        _tokenService.Setup(x => x.GenerateAccessToken(It.IsAny<Identity.Core.Aggregates.User.User>()))
            .Returns("access_token");
        _tokenService.Setup(x => x.GenerateRefreshToken()).Returns("refresh_token");
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesUser()
    {
        // Arrange
        var handler = new RegisterHandler(_context, _passwordHasher.Object, _tokenService.Object, _messageBus);
        var command = new RegisterCommand("test@test.com", "password123", "John", "Doe");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access_token");
        result.RefreshToken.Should().Be("refresh_token");

        var userInDb = await _context.Users.FirstOrDefaultAsync();
        userInDb.Should().NotBeNull();
        userInDb!.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ThrowsException()
    {
        // Arrange
        var existingUser = new Identity.Core.Aggregates.User.User("test@test.com", "existing_hash", "Existing", "User", 1);
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var handler = new RegisterHandler(_context, _passwordHasher.Object, _tokenService.Object, _messageBus);
        var command = new RegisterCommand("test@test.com", "password123", "John", "Doe");

        // Act & Assert
        var act = () => handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
    }
}
