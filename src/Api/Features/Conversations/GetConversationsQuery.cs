using MediatR;

namespace Api;

public sealed record GetConversationsQuery : IRequest<IEnumerable<ConversationDto>>;
