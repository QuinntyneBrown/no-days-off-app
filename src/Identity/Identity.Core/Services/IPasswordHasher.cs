namespace Identity.Core.Services;

/// <summary>
/// Interface for password hashing operations
/// </summary>
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
