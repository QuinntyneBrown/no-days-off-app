using Shared.Domain;
using Shared.Domain.Exceptions;

namespace Identity.Core.Aggregates.User;

/// <summary>
/// Role entity for authorization
/// </summary>
public class Role : Entity
{
    public const int MaxNameLength = 100;

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private Role() { }

    public static Role Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Role name is required");

        if (name.Length > MaxNameLength)
            throw new ValidationException($"Role name cannot exceed {MaxNameLength} characters");

        return new Role
        {
            Name = name.Trim(),
            Description = description?.Trim()
        };
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Role name is required");

        Name = name.Trim();
        Description = description?.Trim();
    }

    // Standard roles
    public static class Names
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Coach = "Coach";
        public const string Athlete = "Athlete";
    }
}
