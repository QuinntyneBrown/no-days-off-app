namespace NoDaysOff.Core.Exceptions;

/// <summary>
/// Exception thrown when domain validation fails
/// </summary>
public class ValidationException : DomainException
{
    public IReadOnlyCollection<string> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    public ValidationException(IEnumerable<string> errors) : base(string.Join("; ", errors))
    {
        Errors = errors.ToList();
    }
}
