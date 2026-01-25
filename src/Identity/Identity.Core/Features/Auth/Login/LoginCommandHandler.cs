using Identity.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Identity;
using Shared.Domain.Exceptions;
using Shared.Messaging;
using Shared.Messaging.Messages.Identity;

namespace Identity.Core.Features.Auth.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IIdentityDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IMessageBus _messageBus;

    public LoginCommandHandler(
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

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email.ToLowerInvariant(), cancellationToken);

        if (user == null)
            throw new UnauthorizedException("Invalid email or password");

        if (!user.IsActive)
            throw new UnauthorizedException("User account is deactivated");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid email or password");

        // Record login
        user.RecordLogin();

        // Generate tokens
        var (accessToken, expiresAt) = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.AddRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        await _messageBus.PublishAsync(new UserAuthenticatedMessage
        {
            UserId = user.Id,
            Email = user.Email,
            TenantId = user.TenantId
        }, MessageTopics.Identity.UserAuthenticated, cancellationToken);

        return new AuthResponseDto(
            accessToken,
            refreshToken,
            expiresAt,
            user.ToDto());
    }
}
