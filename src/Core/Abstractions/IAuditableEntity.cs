namespace Core.Abstractions;

/// <summary>
/// Interface for entities that track audit information
/// </summary>
public interface IAuditableEntity
{
    DateTime CreatedOn { get; }
    string CreatedBy { get; }
    DateTime LastModifiedOn { get; }
    string LastModifiedBy { get; }
}
