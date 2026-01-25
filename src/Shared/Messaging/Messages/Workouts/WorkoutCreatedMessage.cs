using MessagePack;
using Shared.Messages;

namespace Shared.Messaging.Messages.Workouts;

/// <summary>
/// Message sent when a workout is created
/// </summary>
[MessagePackObject]
public class WorkoutCreatedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();

    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Key(2)]
    public int WorkoutId { get; set; }

    [Key(3)]
    public string Name { get; set; } = string.Empty;

    [Key(4)]
    public int? TenantId { get; set; }
}
