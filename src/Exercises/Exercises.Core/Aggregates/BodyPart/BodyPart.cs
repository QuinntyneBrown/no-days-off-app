using Shared.Domain;

namespace Exercises.Core.Aggregates.BodyPart;

public class BodyPart : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int TenantId { get; private set; }

    private BodyPart() { }

    public BodyPart(string name, int tenantId, string? description = null)
    {
        Name = name;
        TenantId = tenantId;
        Description = description;
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
