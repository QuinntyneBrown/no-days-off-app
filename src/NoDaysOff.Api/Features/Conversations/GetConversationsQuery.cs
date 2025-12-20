using MediatR;

namespace NoDaysOff.Api;

public sealed record GetConversationsQuery : IRequest<IEnumerable<ConversationDto>>;
