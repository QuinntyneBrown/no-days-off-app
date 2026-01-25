namespace Shared.Domain.Exceptions;

/// <summary>
/// Exception thrown when a user is not authorized to perform an operation
/// </summary>
public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message = "You are not authorized to perform this action.")
        : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
