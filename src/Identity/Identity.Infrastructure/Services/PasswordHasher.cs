using System.Security.Cryptography;
using Identity.Core.Services;

namespace Identity.Infrastructure.Services;

/// <summary>
/// BCrypt-like password hashing implementation using PBKDF2
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        var hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, string hash)
    {
        var hashBytes = Convert.FromBase64String(hash);

        if (hashBytes.Length != SaltSize + HashSize)
            return false;

        var salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        var computedHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        for (int i = 0; i < HashSize; i++)
        {
            if (hashBytes[SaltSize + i] != computedHash[i])
                return false;
        }

        return true;
    }
}
