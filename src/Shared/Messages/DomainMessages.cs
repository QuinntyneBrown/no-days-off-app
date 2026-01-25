using MessagePack;

namespace Shared.Messages;

/// <summary>
/// Message sent when an athlete is created
/// </summary>
[MessagePackObject]
public class AthleteCreatedMessage : IMessage
{
    [Key(0)]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();
    
    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    [Key(2)]
    public int AthleteId { get; set; }
    
    [Key(3)]
    public string Name { get; set; } = string.Empty;
    
    [Key(4)]
    public string Username { get; set; } = string.Empty;
    
    [Key(5)]
    public int? TenantId { get; set; }
}

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
}

/// <summary>
/// Message sent when an exercise is created
/// </summary>
[MessagePackObject]
public class ExerciseCreatedMessage : IMessage
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
    public string Description { get; set; } = string.Empty;
}
