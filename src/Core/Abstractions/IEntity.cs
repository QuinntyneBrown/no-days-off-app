namespace Core.Abstractions;

/// <summary>
/// Base interface for all domain entities
/// </summary>
public interface IEntity
{
    int Id { get; }
}

/// <summary>
/// Generic interface for entities with typed identifiers
/// </summary>
/// <typeparam name="TId">The type of the entity identifier</typeparam>
public interface IEntity<TId> : IEntity where TId : notnull
{
    new TId Id { get; }
}
