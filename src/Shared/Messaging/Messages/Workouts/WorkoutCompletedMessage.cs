using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Workouts;

/// <summary>
/// Message sent when a workout is completed
/// </summary>
[MessagePackObject]
public class WorkoutCompletedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int WorkoutId { get; set; }

    [Key(3)]
    public int AthleteId { get; set; }

    [Key(4)]
    public DateTime CompletedAt { get; set; }

    [Key(5)]
    public int? TenantId { get; set; }
}
