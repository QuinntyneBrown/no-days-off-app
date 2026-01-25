using Identity.Core.Aggregates.User;

namespace Identity.Core.Services;

/// <summary>
/// Interface for JWT token operations
/// </summary>
public interface ITokenService
{
    (string Token, DateTime ExpiresAt) GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
