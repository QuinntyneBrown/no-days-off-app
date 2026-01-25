namespace Shared.Domain;

/// <summary>
/// Marker interface for aggregate roots
/// </summary>
public interface IAggregateRoot : IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
