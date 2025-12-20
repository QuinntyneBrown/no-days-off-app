using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteConversationCommandHandler : IRequestHandler<DeleteConversationCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteConversationCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(x => x.Id == request.ConversationId, cancellationToken)
            ?? throw new InvalidOperationException($"Conversation with id {request.ConversationId} not found.");

        conversation.Delete();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
