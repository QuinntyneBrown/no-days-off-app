namespace NoDaysOff.Api;

public sealed record ConversationDto(
    int ConversationId,
    IEnumerable<int> ParticipantIds,
    int MessageCount,
    DateTime CreatedOn,
    string CreatedBy);
