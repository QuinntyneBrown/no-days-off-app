using Core.Abstractions;
using Core.Exceptions;

namespace Core.Model.ConversationAggregate;

/// <summary>
/// Entity representing a message in a conversation
/// </summary>
public sealed class Message : Entity, IAuditableEntity
{
    public int FromId { get; private set; }
    public int ToId { get; private set; }
    public string Body { get; private set; } = string.Empty;
    public DateTime CreatedOn { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }
    public string LastModifiedBy { get; private set; } = string.Empty;

    private Message()
    {
    }

    internal static Message Create(int fromId, int toId, string body, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ValidationException("Message body is required");
        }

        return new Message
        {
            FromId = fromId,
            ToId = toId,
            Body = body,
            CreatedOn = DateTime.UtcNow,
            LastModifiedOn = DateTime.UtcNow,
            CreatedBy = createdBy,
            LastModifiedBy = createdBy
        };
    }

    internal void UpdateBody(string body, string modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ValidationException("Message body is required");
        }

        Body = body;
        LastModifiedOn = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}
