namespace Shared.Authentication;

/// <summary>
/// JWT configuration settings
/// </summary>
public class JwtConfiguration
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "NoDaysOff";
    public string Audience { get; set; } = "NoDaysOff";
    public int ExpirationMinutes { get; set; } = 60;
    public int RefreshTokenExpirationDays { get; set; } = 7;
}
