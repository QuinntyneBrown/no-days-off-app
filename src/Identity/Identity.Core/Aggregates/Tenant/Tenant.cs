using Shared.Domain;
using Shared.Domain.Exceptions;

namespace Identity.Core.Aggregates.Tenant;

/// <summary>
/// Tenant aggregate root for multi-tenancy
/// </summary>
public class Tenant : AggregateRoot
{
    public const int MaxNameLength = 256;

    public Guid UniqueId { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Tenant()
    {
        // Tenant doesn't belong to a tenant
        TenantId = null;
    }

    public static Tenant Create(string name, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Tenant name is required");

        if (name.Length > MaxNameLength)
            throw new ValidationException($"Tenant name cannot exceed {MaxNameLength} characters");

        var tenant = new Tenant
        {
            UniqueId = Guid.NewGuid(),
            Name = name.Trim()
        };

        tenant.SetAuditInfo(createdBy);
        return tenant;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Tenant name is required");

        if (name.Length > MaxNameLength)
            throw new ValidationException($"Tenant name cannot exceed {MaxNameLength} characters");

        Name = name.Trim();
        UpdateModified(modifiedBy);
    }
}
