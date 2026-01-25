namespace Shared.Contracts.Identity;

public sealed record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserDto User);

public sealed record LoginRequestDto(
    string Email,
    string Password);

public sealed record RegisterRequestDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    int? TenantId = null);

public sealed record RefreshTokenRequestDto(
    string RefreshToken);
