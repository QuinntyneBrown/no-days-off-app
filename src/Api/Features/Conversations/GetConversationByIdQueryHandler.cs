using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class GetConversationByIdQueryHandler : IRequestHandler<GetConversationByIdQuery, ConversationDto?>
{
    private readonly INoDaysOffContext _context;

    public GetConversationByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ConversationDto?> Handle(GetConversationByIdQuery request, CancellationToken cancellationToken)
    {
        var conversation = await _context.Conversations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ConversationId && !x.IsDeleted, cancellationToken);

        return conversation?.ToDto();
    }
}
