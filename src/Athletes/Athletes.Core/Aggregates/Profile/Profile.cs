using Shared.Domain;
using Shared.Domain.Exceptions;

namespace Athletes.Core.Aggregates.Profile;

/// <summary>
/// Aggregate root for user profiles
/// </summary>
public class Profile : AggregateRoot
{
    public const int MaxNameLength = 256;

    public string Name { get; protected set; } = string.Empty;
    public string Username { get; protected set; } = string.Empty;
    public string ImageUrl { get; protected set; } = string.Empty;

    protected Profile() : base()
    {
    }

    protected Profile(int? tenantId) : base(tenantId)
    {
    }

    public static Profile Create(int? tenantId, string name, string username, string createdBy)
    {
        ValidateName(name);
        ValidateUsername(username);

        var profile = new Profile(tenantId)
        {
            Name = name,
            Username = username
        };
        profile.SetAuditInfo(createdBy);

        return profile;
    }

    public virtual void UpdateName(string name, string modifiedBy)
    {
        ValidateName(name);
        Name = name;
        UpdateModified(modifiedBy);
    }

    public void UpdateUsername(string username, string modifiedBy)
    {
        ValidateUsername(username);
        Username = username;
        UpdateModified(modifiedBy);
    }

    public void UpdateImageUrl(string imageUrl, string modifiedBy)
    {
        ImageUrl = imageUrl ?? string.Empty;
        UpdateModified(modifiedBy);
    }

    protected static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Profile name is required");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ValidationException($"Profile name cannot exceed {MaxNameLength} characters");
        }
    }

    protected static void ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ValidationException("Username is required");
        }
    }
}
