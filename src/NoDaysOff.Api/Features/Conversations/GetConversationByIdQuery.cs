using MediatR;

namespace NoDaysOff.Api;

public sealed record GetConversationByIdQuery(int ConversationId) : IRequest<ConversationDto?>;
