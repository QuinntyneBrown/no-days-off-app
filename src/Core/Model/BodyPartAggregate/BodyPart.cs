using Core.Abstractions;
using Core.Exceptions;

namespace Core.Model.BodyPartAggregate;

/// <summary>
/// Aggregate root for body part definitions (e.g., chest, legs, back)
/// </summary>
public sealed class BodyPart : AggregateRoot
{
    public const int MaxNameLength = 256;

    public string Name { get; private set; } = string.Empty;

    private BodyPart() : base()
    {
    }

    private BodyPart(int? tenantId) : base(tenantId)
    {
    }

    public static BodyPart Create(int? tenantId, string name, string createdBy)
    {
        ValidateName(name);

        var bodyPart = new BodyPart(tenantId)
        {
            Name = name
        };
        bodyPart.SetAuditInfo(createdBy);

        return bodyPart;
    }

    public void UpdateName(string name, string modifiedBy)
    {
        ValidateName(name);
        Name = name;
        UpdateModified(modifiedBy);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Body part name is required");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ValidationException($"Body part name cannot exceed {MaxNameLength} characters");
        }
    }
}
