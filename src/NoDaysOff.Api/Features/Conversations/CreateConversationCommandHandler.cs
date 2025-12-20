using MediatR;
using NoDaysOff.Core;
using NoDaysOff.Core.Model.ConversationAggregate;

namespace NoDaysOff.Api;

public sealed class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ConversationDto>
{
    private readonly INoDaysOffContext _context;

    public CreateConversationCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ConversationDto> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        var conversation = Conversation.CreateBetween(
            request.TenantId,
            request.Participant1Id,
            request.Participant2Id,
            request.CreatedBy);

        _context.Conversations.Add(conversation);

        await _context.SaveChangesAsync(cancellationToken);

        return conversation.ToDto();
    }
}
