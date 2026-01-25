using Shared.Domain;
using Shared.Domain.Exceptions;

namespace Identity.Core.Aggregates.User;

/// <summary>
/// User aggregate root for authentication
/// </summary>
public class User : AggregateRoot
{
    public const int MaxEmailLength = 256;
    public const int MaxNameLength = 256;

    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public DateTime? LastLoginAt { get; private set; }

    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private User() { }

    public static User Create(
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        int? tenantId,
        string createdBy)
    {
        ValidateEmail(email);
        ValidateName(firstName, nameof(firstName));
        ValidateName(lastName, nameof(lastName));

        var user = new User
        {
            Email = email.ToLowerInvariant().Trim(),
            PasswordHash = passwordHash,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            TenantId = tenantId,
            IsActive = true
        };

        user.SetAuditInfo(createdBy);
        return user;
    }

    public void UpdateProfile(string firstName, string lastName, string modifiedBy)
    {
        ValidateName(firstName, nameof(firstName));
        ValidateName(lastName, nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        UpdateModified(modifiedBy);
    }

    public void ChangePassword(string newPasswordHash, string modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ValidationException("Password hash cannot be empty");

        PasswordHash = newPasswordHash;
        UpdateModified(modifiedBy);
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void Deactivate(string modifiedBy)
    {
        IsActive = false;
        UpdateModified(modifiedBy);
    }

    public void Activate(string modifiedBy)
    {
        IsActive = true;
        UpdateModified(modifiedBy);
    }

    public RefreshToken AddRefreshToken(string token, DateTime expiresAt)
    {
        var refreshToken = RefreshToken.Create(Id, token, expiresAt);
        _refreshTokens.Add(refreshToken);
        return refreshToken;
    }

    public void RevokeRefreshToken(string token)
    {
        var refreshToken = _refreshTokens.FirstOrDefault(t => t.Token == token);
        refreshToken?.Revoke();
    }

    public void RevokeAllRefreshTokens()
    {
        foreach (var token in _refreshTokens.Where(t => !t.IsRevoked))
        {
            token.Revoke();
        }
    }

    public void AddRole(Role role)
    {
        if (_userRoles.All(ur => ur.RoleId != role.Id))
        {
            _userRoles.Add(new UserRole { UserId = Id, RoleId = role.Id });
        }
    }

    public void RemoveRole(int roleId)
    {
        var userRole = _userRoles.FirstOrDefault(ur => ur.RoleId == roleId);
        if (userRole != null)
        {
            _userRoles.Remove(userRole);
        }
    }

    public string FullName => $"{FirstName} {LastName}".Trim();

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("Email is required");

        if (email.Length > MaxEmailLength)
            throw new ValidationException($"Email cannot exceed {MaxEmailLength} characters");

        if (!email.Contains('@'))
            throw new ValidationException("Invalid email format");
    }

    private static void ValidateName(string name, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException($"{fieldName} is required");

        if (name.Length > MaxNameLength)
            throw new ValidationException($"{fieldName} cannot exceed {MaxNameLength} characters");
    }
}
