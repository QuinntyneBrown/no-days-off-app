using NoDaysOff.Core.Events;

namespace NoDaysOff.Core.Abstractions;

/// <summary>
/// Marker interface for aggregate roots
/// </summary>
public interface IAggregateRoot : IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
