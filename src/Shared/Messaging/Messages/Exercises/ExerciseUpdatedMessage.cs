using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Exercises;

/// <summary>
/// Message sent when an exercise is updated
/// </summary>
[MessagePackObject]
public class ExerciseUpdatedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int ExerciseId { get; set; }

    [Key(3)]
    public string Name { get; set; } = string.Empty;

    [Key(4)]
    public int? BodyPartId { get; set; }

    [Key(5)]
    public int? TenantId { get; set; }
}
