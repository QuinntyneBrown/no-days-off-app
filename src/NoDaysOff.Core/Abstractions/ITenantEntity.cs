namespace NoDaysOff.Core.Abstractions;

/// <summary>
/// Interface for entities that belong to a tenant (multi-tenancy support)
/// </summary>
public interface ITenantEntity
{
    int? TenantId { get; }
}
