using Identity.Core.Aggregates.User;
using Identity.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Identity;
using Shared.Domain.Exceptions;
using Shared.Messaging;
using Shared.Messaging.Messages.Identity;

namespace Identity.Core.Features.Auth.RegisterUser;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    private readonly IIdentityDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IMessageBus _messageBus;

    public RegisterUserCommandHandler(
        IIdentityDbContext context,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IMessageBus messageBus)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _messageBus = messageBus;
    }

    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email.ToLowerInvariant(), cancellationToken);

        if (existingUser != null)
            throw new ConflictException($"User with email '{request.Email}' already exists");

        // Hash password
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        // Create user
        var user = User.Create(
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName,
            request.TenantId,
            request.Email);

        // Add default User role
        var userRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == Role.Names.User, cancellationToken);

        if (userRole != null)
        {
            user.AddRole(userRole);
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        // Generate tokens
        var (accessToken, expiresAt) = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenEntity = user.AddRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        await _messageBus.PublishAsync(new UserCreatedMessage
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            TenantId = user.TenantId
        }, MessageTopics.Identity.UserCreated, cancellationToken);

        return new AuthResponseDto(
            accessToken,
            refreshToken,
            expiresAt,
            user.ToDto());
    }
}
