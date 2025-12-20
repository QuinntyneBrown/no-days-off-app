using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateConversationCommand(
    int TenantId,
    int Participant1Id,
    int Participant2Id,
    string CreatedBy) : IRequest<ConversationDto>;
