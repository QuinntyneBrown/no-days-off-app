namespace Core.Abstractions;

/// <summary>
/// Interface for entities that support soft deletion
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; }
    void Delete();
    void Restore();
}
