using Core.Model.ConversationAggregate;

namespace Api;

public static class ConversationExtensions
{
    public static ConversationDto ToDto(this Conversation conversation)
    {
        return new ConversationDto(
            conversation.Id,
            conversation.ParticipantIds,
            conversation.MessageCount,
            conversation.CreatedOn,
            conversation.CreatedBy);
    }
}
