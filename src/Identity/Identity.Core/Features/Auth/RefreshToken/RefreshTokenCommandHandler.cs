using Identity.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Identity;
using Shared.Domain.Exceptions;

namespace Identity.Core.Features.Auth.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IIdentityDbContext _context;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(
        IIdentityDbContext context,
        ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (refreshToken == null)
            throw new UnauthorizedException("Invalid refresh token");

        if (!refreshToken.IsActive)
            throw new UnauthorizedException("Refresh token is no longer valid");

        var user = refreshToken.User;

        if (!user.IsActive)
            throw new UnauthorizedException("User account is deactivated");

        // Revoke old token and create new one
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        refreshToken.Revoke(newRefreshToken);
        user.AddRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));

        // Generate new access token
        var (accessToken, expiresAt) = _tokenService.GenerateAccessToken(user);

        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(
            accessToken,
            newRefreshToken,
            expiresAt,
            user.ToDto());
    }
}
