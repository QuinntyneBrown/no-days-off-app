using Core.Abstractions;

namespace Core.Model.ConversationAggregate;

/// <summary>
/// Aggregate root for conversation and messaging
/// </summary>
public sealed class Conversation : AggregateRoot
{
    private readonly List<int> _participantIds = new();
    private readonly List<Message> _messages = new();

    public IReadOnlyCollection<int> ParticipantIds => _participantIds.AsReadOnly();
    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

    private Conversation() : base()
    {
    }

    private Conversation(int? tenantId) : base(tenantId)
    {
    }

    public static Conversation Create(int? tenantId, string createdBy)
    {
        var conversation = new Conversation(tenantId);
        conversation.SetAuditInfo(createdBy);

        return conversation;
    }

    public static Conversation CreateBetween(int? tenantId, int participant1Id, int participant2Id, string createdBy)
    {
        var conversation = Create(tenantId, createdBy);
        conversation._participantIds.Add(participant1Id);
        conversation._participantIds.Add(participant2Id);

        return conversation;
    }

    public void AddParticipant(int profileId, string modifiedBy)
    {
        if (!_participantIds.Contains(profileId))
        {
            _participantIds.Add(profileId);
            UpdateModified(modifiedBy);
        }
    }

    public void RemoveParticipant(int profileId, string modifiedBy)
    {
        if (_participantIds.Remove(profileId))
        {
            UpdateModified(modifiedBy);
        }
    }

    public void SendMessage(int fromId, int toId, string body, string sentBy)
    {
        var message = Message.Create(fromId, toId, body, sentBy);
        _messages.Add(message);
        UpdateModified(sentBy);
    }

    public IEnumerable<Message> GetMessagesBetween(int participant1Id, int participant2Id)
    {
        return _messages.Where(m =>
            (m.FromId == participant1Id && m.ToId == participant2Id) ||
            (m.FromId == participant2Id && m.ToId == participant1Id));
    }

    public IEnumerable<Message> GetRecentMessages(int count = 50)
    {
        return _messages
            .OrderByDescending(m => m.CreatedOn)
            .Take(count);
    }

    public bool HasParticipant(int profileId) => _participantIds.Contains(profileId);

    public int MessageCount => _messages.Count;
}
