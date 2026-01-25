namespace Shared.Domain;

/// <summary>
/// Interface for entities that belong to a tenant (multi-tenancy support)
/// </summary>
public interface ITenantEntity
{
    int? TenantId { get; }
}
