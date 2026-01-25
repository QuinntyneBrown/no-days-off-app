namespace Shared.Domain;

/// <summary>
/// Base interface for all domain events
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    Guid EventId { get; }
}
