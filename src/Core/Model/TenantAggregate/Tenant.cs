using Core.Abstractions;
using Core.Exceptions;

namespace Core.Model.TenantAggregate;

/// <summary>
/// Aggregate root for tenant management (multi-tenancy)
/// </summary>
public sealed class Tenant : AggregateRoot
{
    public const int MaxNameLength = 256;

    public Guid UniqueId { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Tenant() : base(null)
    {
        UniqueId = Guid.NewGuid();
    }

    public static Tenant Create(string name, string createdBy)
    {
        ValidateName(name);

        var tenant = new Tenant
        {
            Name = name,
            UniqueId = Guid.NewGuid()
        };
        tenant.SetAuditInfo(createdBy);

        return tenant;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        ValidateName(name);
        Name = name;
        UpdateModified(modifiedBy);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Tenant name is required");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ValidationException($"Tenant name cannot exceed {MaxNameLength} characters");
        }
    }
}
