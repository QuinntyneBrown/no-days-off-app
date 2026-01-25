using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Core.Features.Auth.RevokeToken;

public sealed class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, bool>
{
    private readonly IIdentityDbContext _context;

    public RevokeTokenCommandHandler(IIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (refreshToken == null || !refreshToken.IsActive)
            return false;

        refreshToken.Revoke();
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
