# Messaging Feature

## Overview

The Messaging feature enables communication between users through conversations and messages, supporting trainer-athlete communication and social features.

## Domain Model

### Conversation Aggregate

```
Conversation
├── ParticipantIds: ICollection<int>
└── Messages: ICollection<Message>
```

### Message Entity

```
Message
├── FromId: int
├── ToId: int
├── Body: string
├── CreatedOn: DateTime
├── CreatedBy: string
├── LastModifiedOn: DateTime
└── LastModifiedBy: string
```

## Features

### Conversation Management

- Create conversations between users
- Add/remove participants
- Support for group conversations

### Messaging

- Send messages within conversations
- Track message history
- Query messages between specific participants

## Usage

### Creating a Conversation

```csharp
// Create an empty conversation
var conversation = Conversation.Create(
    tenantId: 1,
    createdBy: "system"
);

// Or create between two users
var directConversation = Conversation.CreateBetween(
    tenantId: 1,
    participant1Id: 1, // Trainer
    participant2Id: 2, // Athlete
    createdBy: "system"
);
```

### Managing Participants

```csharp
// Add a participant
conversation.AddParticipant(profileId: 3, modifiedBy: "admin");

// Remove a participant
conversation.RemoveParticipant(profileId: 3, modifiedBy: "admin");

// Check if someone is in the conversation
bool isParticipant = conversation.HasParticipant(profileId: 2);
```

### Sending Messages

```csharp
conversation.SendMessage(
    fromId: 1, // Trainer
    toId: 2,   // Athlete
    body: "Great workout today! Let's increase the weight next session.",
    sentBy: "trainer1"
);

conversation.SendMessage(
    fromId: 2,
    toId: 1,
    body: "Thanks! I'm ready for the challenge!",
    sentBy: "athlete1"
);
```

### Querying Messages

```csharp
// Get messages between two participants
var messages = conversation.GetMessagesBetween(
    participant1Id: 1,
    participant2Id: 2
);

// Get most recent messages
var recentMessages = conversation.GetRecentMessages(count: 50);

// Get total message count
int totalMessages = conversation.MessageCount;
```

## Validation Rules

| Entity | Field | Validation |
|--------|-------|------------|
| Message | Body | Required (cannot be empty or whitespace) |

## Use Cases

### Trainer-Athlete Communication

```csharp
// Trainer creates conversation with athlete
var conversation = Conversation.CreateBetween(1, trainerId, athleteId, "system");

// Trainer sends workout feedback
conversation.SendMessage(
    fromId: trainerId,
    toId: athleteId,
    body: "Your form on squats has improved significantly!",
    sentBy: "trainer"
);

// Athlete responds
conversation.SendMessage(
    fromId: athleteId,
    toId: trainerId,
    body: "Thank you! The video you shared really helped.",
    sentBy: "athlete"
);
```

### Group Conversation

```csharp
// Create group conversation
var groupChat = Conversation.Create(1, "system");
groupChat.AddParticipant(trainerId, "admin");
groupChat.AddParticipant(athlete1Id, "admin");
groupChat.AddParticipant(athlete2Id, "admin");
groupChat.AddParticipant(athlete3Id, "admin");

// Trainer sends message to group
groupChat.SendMessage(
    fromId: trainerId,
    toId: athlete1Id, // Primary recipient
    body: "Team workout tomorrow at 6 AM!",
    sentBy: "trainer"
);
```

## Relationships

```
Conversation (1) --- (*) Message
Conversation (*) --- (*) Profile (through ParticipantIds)
Profile (1) --- (*) Message (as sender/receiver)
```
