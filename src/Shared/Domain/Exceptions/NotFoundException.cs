namespace Shared.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested entity is not found
/// </summary>
public class NotFoundException : DomainException
{
    public string EntityType { get; }
    public object EntityId { get; }

    public NotFoundException(string entityType, object entityId)
        : base($"{entityType} with id '{entityId}' was not found.")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public static NotFoundException For<T>(object id) where T : class
    {
        return new NotFoundException(typeof(T).Name, id);
    }
}
