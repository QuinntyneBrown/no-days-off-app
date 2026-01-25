using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class GetConversationsQueryHandler : IRequestHandler<GetConversationsQuery, IEnumerable<ConversationDto>>
{
    private readonly INoDaysOffContext _context;

    public GetConversationsQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ConversationDto>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Conversations
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
