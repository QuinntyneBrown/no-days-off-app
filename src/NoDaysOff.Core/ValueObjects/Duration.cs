using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.ValueObjects;

/// <summary>
/// Value object representing a duration in seconds
/// </summary>
public sealed class Duration : ValueObject
{
    public int Seconds { get; }

    private Duration(int seconds)
    {
        Seconds = seconds;
    }

    public static Duration FromSeconds(int seconds)
    {
        if (seconds < 0)
        {
            throw new ValidationException("Duration cannot be negative");
        }

        return new Duration(seconds);
    }

    public static Duration FromMinutes(int minutes) => FromSeconds(minutes * 60);

    public static Duration Zero => new(0);

    public int ToMinutes() => Seconds / 60;
    public TimeSpan ToTimeSpan() => TimeSpan.FromSeconds(Seconds);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Seconds;
    }

    public override string ToString()
    {
        var timeSpan = ToTimeSpan();
        return timeSpan.Hours > 0
            ? $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}"
            : $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }
}
