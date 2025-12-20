using FluentAssertions;
using NoDaysOff.Core.Aggregates.ConversationAggregate;
using NoDaysOff.Core.Exceptions;

namespace NoDaysOff.Core.Tests.Aggregates;

public class ConversationTests
{
    [Fact]
    public void Create_ShouldCreateEmptyConversation()
    {
        // Arrange & Act
        var conversation = Conversation.Create(1, "system");

        // Assert
        conversation.TenantId.Should().Be(1);
        conversation.ParticipantIds.Should().BeEmpty();
        conversation.Messages.Should().BeEmpty();
        conversation.MessageCount.Should().Be(0);
    }

    [Fact]
    public void CreateBetween_ShouldCreateConversationWithTwoParticipants()
    {
        // Arrange & Act
        var conversation = Conversation.CreateBetween(1, 1, 2, "system");

        // Assert
        conversation.ParticipantIds.Should().HaveCount(2);
        conversation.ParticipantIds.Should().Contain(new[] { 1, 2 });
    }

    [Fact]
    public void AddParticipant_ShouldAddParticipant()
    {
        // Arrange
        var conversation = Conversation.Create(1, "system");

        // Act
        conversation.AddParticipant(1, "admin");

        // Assert
        conversation.ParticipantIds.Should().HaveCount(1);
        conversation.ParticipantIds.Should().Contain(1);
    }

    [Fact]
    public void AddParticipant_DuplicateId_ShouldNotAddAgain()
    {
        // Arrange
        var conversation = Conversation.Create(1, "system");

        // Act
        conversation.AddParticipant(1, "admin");
        conversation.AddParticipant(1, "admin");

        // Assert
        conversation.ParticipantIds.Should().HaveCount(1);
    }

    [Fact]
    public void RemoveParticipant_ShouldRemoveParticipant()
    {
        // Arrange
        var conversation = Conversation.CreateBetween(1, 1, 2, "system");

        // Act
        conversation.RemoveParticipant(1, "admin");

        // Assert
        conversation.ParticipantIds.Should().HaveCount(1);
        conversation.ParticipantIds.Should().NotContain(1);
    }

    [Fact]
    public void SendMessage_WithValidBody_ShouldAddMessage()
    {
        // Arrange
        var conversation = Conversation.CreateBetween(1, 1, 2, "system");

        // Act
        conversation.SendMessage(1, 2, "Hello!", "user1");

        // Assert
        conversation.Messages.Should().HaveCount(1);
        conversation.MessageCount.Should().Be(1);
        var message = conversation.Messages.First();
        message.FromId.Should().Be(1);
        message.ToId.Should().Be(2);
        message.Body.Should().Be("Hello!");
    }

    [Fact]
    public void SendMessage_WithEmptyBody_ShouldThrowValidationException()
    {
        // Arrange
        var conversation = Conversation.CreateBetween(1, 1, 2, "system");

        // Act
        var act = () => conversation.SendMessage(1, 2, "", "user1");

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Message body is required");
    }

    [Fact]
    public void GetMessagesBetween_ShouldReturnMessagesBetweenParticipants()
    {
        // Arrange
        var conversation = Conversation.CreateBetween(1, 1, 2, "system");
        conversation.AddParticipant(3, "admin");
        conversation.SendMessage(1, 2, "Hello!", "user1");
        conversation.SendMessage(2, 1, "Hi back!", "user2");
        conversation.SendMessage(1, 3, "Hello user 3!", "user1");

        // Act
        var messages = conversation.GetMessagesBetween(1, 2).ToList();

        // Assert
        messages.Should().HaveCount(2);
    }

    [Fact]
    public void GetRecentMessages_ShouldReturnMostRecentMessages()
    {
        // Arrange
        var conversation = Conversation.CreateBetween(1, 1, 2, "system");
        for (int i = 0; i < 10; i++)
        {
            conversation.SendMessage(1, 2, $"Message {i}", "user1");
        }

        // Act
        var messages = conversation.GetRecentMessages(5).ToList();

        // Assert
        messages.Should().HaveCount(5);
    }

    [Fact]
    public void HasParticipant_WithExistingParticipant_ShouldReturnTrue()
    {
        // Arrange
        var conversation = Conversation.CreateBetween(1, 1, 2, "system");

        // Act & Assert
        conversation.HasParticipant(1).Should().BeTrue();
    }

    [Fact]
    public void HasParticipant_WithNonExistingParticipant_ShouldReturnFalse()
    {
        // Arrange
        var conversation = Conversation.Create(1, "system");

        // Act & Assert
        conversation.HasParticipant(1).Should().BeFalse();
    }
}
