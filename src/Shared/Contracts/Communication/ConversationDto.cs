namespace Shared.Contracts.Communication;

public sealed record ConversationDto(
    int ConversationId,
    string Name,
    IEnumerable<string> ParticipantIds,
    IEnumerable<MessageDto> RecentMessages,
    int? TenantId,
    DateTime CreatedOn);

public sealed record MessageDto(
    int MessageId,
    int ConversationId,
    string SenderId,
    string Content,
    DateTime SentAt,
    bool IsRead);

public sealed record CreateConversationDto(
    string Name,
    IEnumerable<string> ParticipantIds,
    int? TenantId = null);

public sealed record SendMessageDto(
    int ConversationId,
    string Content);
