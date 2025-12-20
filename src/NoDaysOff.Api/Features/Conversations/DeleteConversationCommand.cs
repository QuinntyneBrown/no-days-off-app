using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteConversationCommand(int ConversationId, string DeletedBy) : IRequest;
