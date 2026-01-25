using MediatR;

namespace Api;

public sealed record DeleteConversationCommand(int ConversationId, string DeletedBy) : IRequest;
