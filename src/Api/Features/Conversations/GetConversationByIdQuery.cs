using MediatR;

namespace Api;

public sealed record GetConversationByIdQuery(int ConversationId) : IRequest<ConversationDto?>;
