using NoDaysOff.Core.Events;

namespace NoDaysOff.Core.Abstractions;

/// <summary>
/// Base class for aggregate roots with domain event support
/// </summary>
public abstract class AggregateRoot : Entity, IAggregateRoot, IAuditableEntity, ISoftDeletable, ITenantEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public DateTime CreatedOn { get; protected set; }
    public string CreatedBy { get; protected set; } = string.Empty;
    public DateTime LastModifiedOn { get; protected set; }
    public string LastModifiedBy { get; protected set; } = string.Empty;
    public bool IsDeleted { get; protected set; }
    public int? TenantId { get; protected set; }

    protected AggregateRoot()
    {
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }

    protected AggregateRoot(int? tenantId) : this()
    {
        TenantId = tenantId;
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    protected void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public virtual void Delete()
    {
        IsDeleted = true;
        UpdateModified();
    }

    public virtual void Restore()
    {
        IsDeleted = false;
        UpdateModified();
    }

    protected void UpdateModified(string? modifiedBy = null)
    {
        LastModifiedOn = DateTime.UtcNow;
        if (!string.IsNullOrEmpty(modifiedBy))
        {
            LastModifiedBy = modifiedBy;
        }
    }

    protected void SetAuditInfo(string createdBy)
    {
        CreatedBy = createdBy;
        LastModifiedBy = createdBy;
    }
}
