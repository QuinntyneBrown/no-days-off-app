using Shared.Domain;

namespace Exercises.Core.Aggregates.Exercise;

public class Exercise : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int TenantId { get; private set; }
    public int? BodyPartId { get; private set; }
    public string? VideoUrl { get; private set; }
    public string? ImageUrl { get; private set; }
    public string? Instructions { get; private set; }
    public ExerciseType Type { get; private set; }

    private Exercise() { }

    public Exercise(
        string name,
        int tenantId,
        ExerciseType type = ExerciseType.Strength,
        int? bodyPartId = null,
        string? description = null,
        string? videoUrl = null,
        string? imageUrl = null,
        string? instructions = null)
    {
        Name = name;
        TenantId = tenantId;
        Type = type;
        BodyPartId = bodyPartId;
        Description = description;
        VideoUrl = videoUrl;
        ImageUrl = imageUrl;
        Instructions = instructions;
    }

    public void Update(
        string name,
        ExerciseType type,
        int? bodyPartId,
        string? description,
        string? videoUrl,
        string? imageUrl,
        string? instructions)
    {
        Name = name;
        Type = type;
        BodyPartId = bodyPartId;
        Description = description;
        VideoUrl = videoUrl;
        ImageUrl = imageUrl;
        Instructions = instructions;
    }

    public void SetBodyPart(int? bodyPartId)
    {
        BodyPartId = bodyPartId;
    }
}

public enum ExerciseType
{
    Strength = 0,
    Cardio = 1,
    Flexibility = 2,
    Balance = 3,
    Plyometric = 4
}
