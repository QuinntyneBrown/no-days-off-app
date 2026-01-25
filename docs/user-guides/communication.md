# Communication User Guide

## Overview

The Communication microservice handles notifications, messaging, and conversations between users in the No Days Off application.

---

## Behaviors

### 1. Create Notification

**Purpose**: Create and send a notification to a user.

**Command**: `CreateNotificationCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| UserId | Guid | Yes | Target user identifier |
| Type | string | Yes | Notification type |
| Title | string | Yes | Notification title |
| Message | string | Yes | Notification body |
| ActionUrl | string | No | URL to navigate on click |
| EntityType | string | No | Related entity type |
| EntityId | int | No | Related entity identifier |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/notifications
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "userId": "guid-here",
    "type": "Achievement",
    "title": "New Personal Record!",
    "message": "You lifted 100kg on Bench Press - a new PR!",
    "actionUrl": "/exercises/1/history",
    "entityType": "Exercise",
    "entityId": 1
}
```

#### Response
```json
{
    "notificationId": 1,
    "userId": "guid-here",
    "type": "Achievement",
    "title": "New Personal Record!",
    "message": "You lifted 100kg on Bench Press - a new PR!",
    "isRead": false,
    "createdOn": "2024-01-16T10:30:00Z"
}
```

#### Business Rules
- Notification is created and stored in database
- Async notification delivery service is triggered
- Push notifications sent if user has enabled them

#### Notification Types
| Type | Description | Icon |
|------|-------------|------|
| Achievement | Personal records, milestones | Trophy |
| Reminder | Workout reminders | Bell |
| System | System announcements | Info |
| Social | Friend activity, messages | User |
| Progress | Progress updates | Chart |

---

### 2. Get Notifications

**Purpose**: Retrieve notifications for the current user.

**Query**: `GetNotificationsQuery`

#### API Endpoint
```
GET /api/notifications
Authorization: Bearer {accessToken}
```

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| unreadOnly | bool | Only return unread notifications |
| type | string | Filter by notification type |
| page | int | Page number |
| pageSize | int | Items per page |

#### Response
```json
{
    "notifications": [
        {
            "notificationId": 1,
            "type": "Achievement",
            "title": "New Personal Record!",
            "message": "You lifted 100kg on Bench Press - a new PR!",
            "actionUrl": "/exercises/1/history",
            "isRead": false,
            "createdOn": "2024-01-16T10:30:00Z"
        },
        {
            "notificationId": 2,
            "type": "Reminder",
            "title": "Workout Reminder",
            "message": "Don't forget your Push Day workout!",
            "actionUrl": "/workouts/1",
            "isRead": true,
            "createdOn": "2024-01-16T08:00:00Z"
        }
    ],
    "unreadCount": 1,
    "totalCount": 15,
    "page": 1,
    "pageSize": 20
}
```

---

### 3. Mark Notification as Read

**Purpose**: Mark a single notification as read.

**Command**: `MarkAsReadCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| NotificationId | int | Yes | Notification identifier |

#### API Endpoint
```
PUT /api/notifications/{notificationId}/read
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "notificationId": 1,
    "isRead": true,
    "readOn": "2024-01-16T10:35:00Z"
}
```

---

### 4. Mark All Notifications as Read

**Purpose**: Mark all user notifications as read.

**Command**: `MarkAllAsReadCommand`

#### API Endpoint
```
PUT /api/notifications/read-all
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "markedCount": 5,
    "message": "All notifications marked as read"
}
```

---

## Conversation Management

### 5. Create Conversation

**Purpose**: Start a new conversation with another user.

**Command**: `CreateConversationCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ParticipantIds | Guid[] | Yes | Participant user IDs |
| Title | string | No | Conversation title |
| InitialMessage | string | No | First message content |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/conversations
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "participantIds": ["guid-1", "guid-2"],
    "title": "Training Discussion",
    "initialMessage": "Hey! How's your training going?"
}
```

#### Response
```json
{
    "conversationId": 1,
    "title": "Training Discussion",
    "participants": [
        {
            "userId": "guid-1",
            "name": "John Doe"
        },
        {
            "userId": "guid-2",
            "name": "Jane Smith"
        }
    ],
    "lastMessage": {
        "content": "Hey! How's your training going?",
        "sentBy": "guid-1",
        "sentOn": "2024-01-16T10:30:00Z"
    },
    "createdOn": "2024-01-16T10:30:00Z"
}
```

---

### 6. Get Conversations

**Purpose**: Retrieve user's conversations.

**Query**: `GetConversationsQuery`

#### API Endpoint
```
GET /api/conversations
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "conversations": [
        {
            "conversationId": 1,
            "title": "Training Discussion",
            "participants": ["John Doe", "Jane Smith"],
            "lastMessage": {
                "content": "Sounds good! See you tomorrow.",
                "sentBy": "Jane Smith",
                "sentOn": "2024-01-16T14:30:00Z"
            },
            "unreadCount": 2
        },
        {
            "conversationId": 2,
            "title": null,
            "participants": ["Mike Johnson"],
            "lastMessage": {
                "content": "What's your leg day routine?",
                "sentBy": "Mike Johnson",
                "sentOn": "2024-01-15T16:20:00Z"
            },
            "unreadCount": 1
        }
    ],
    "totalUnreadCount": 3
}
```

---

### 7. Get Conversation by ID

**Purpose**: Retrieve conversation details with messages.

**Query**: `GetConversationByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ConversationId | int | Yes | Conversation identifier |

#### API Endpoint
```
GET /api/conversations/{conversationId}
Authorization: Bearer {accessToken}
```

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| page | int | Message page number |
| pageSize | int | Messages per page |

#### Response
```json
{
    "conversationId": 1,
    "title": "Training Discussion",
    "participants": [
        {
            "userId": "guid-1",
            "name": "John Doe",
            "imageUrl": "https://..."
        },
        {
            "userId": "guid-2",
            "name": "Jane Smith",
            "imageUrl": "https://..."
        }
    ],
    "messages": [
        {
            "messageId": 1,
            "content": "Hey! How's your training going?",
            "sentBy": {
                "userId": "guid-1",
                "name": "John Doe"
            },
            "sentOn": "2024-01-16T10:30:00Z",
            "isRead": true
        },
        {
            "messageId": 2,
            "content": "Great! Hit a new PR on squats!",
            "sentBy": {
                "userId": "guid-2",
                "name": "Jane Smith"
            },
            "sentOn": "2024-01-16T10:32:00Z",
            "isRead": true
        }
    ],
    "totalMessages": 15,
    "page": 1,
    "pageSize": 50
}
```

---

### 8. Send Message

**Purpose**: Send a message in an existing conversation.

**Command**: `SendMessageCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ConversationId | int | Yes | Conversation identifier |
| Content | string | Yes | Message content |

#### API Endpoint
```
POST /api/conversations/{conversationId}/messages
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "content": "That's awesome! What weight did you hit?"
}
```

#### Response
```json
{
    "messageId": 3,
    "conversationId": 1,
    "content": "That's awesome! What weight did you hit?",
    "sentBy": "guid-1",
    "sentOn": "2024-01-16T10:35:00Z"
}
```

#### Business Rules
- Message is saved to conversation history
- All participants receive notification
- Real-time delivery via SignalR (if connected)

---

### 9. Delete Conversation

**Purpose**: Remove a conversation (for current user).

**Command**: `DeleteConversationCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ConversationId | int | Yes | Conversation identifier |

#### API Endpoint
```
DELETE /api/conversations/{conversationId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "message": "Conversation deleted successfully"
}
```

#### Business Rules
- Only removes for current user
- Other participants retain their copy
- Messages are preserved for other participants

---

## Notification Flow

```
+-------------+     +------------------+     +------------------+
|   Trigger   |---->| Create           |---->| Store in         |
|   Event     |     | Notification     |     | Database         |
+-------------+     +------------------+     +------------------+
                                                     |
                                                     v
                    +------------------+     +------------------+
                    | Update Badge     |<----| Notification     |
                    | Count (SignalR)  |     | Service          |
                    +------------------+     +------------------+
                                                     |
                                                     v
                    +------------------+     +------------------+
                    | Email (optional) |     | Push (optional)  |
                    +------------------+     +------------------+
```

---

## Messaging Flow

```
+-------------+     +------------------+     +------------------+
|   User A    |---->| Send Message     |---->| Store Message    |
|   Sends     |     | Command          |     | in Database      |
+-------------+     +------------------+     +------------------+
                                                     |
                                                     v
                    +------------------+     +------------------+
                    | Notify User B    |<----| Create           |
                    | (Push/SignalR)   |     | Notification     |
                    +------------------+     +------------------+
                                                     |
                                                     v
                    +------------------+
                    | User B Receives  |
                    | Real-time Update |
                    +------------------+
```

---

## Notification Entity

```
Notification
├── NotificationId: int
├── UserId: Guid
├── Type: NotificationType (enum)
├── Title: string
├── Message: string
├── ActionUrl: string?
├── EntityType: string?
├── EntityId: int?
├── IsRead: bool
├── ReadOn: DateTime?
├── CreatedOn: DateTime
├── CreatedBy: string
└── TenantId: int
```

---

## Conversation Entity

```
Conversation
├── ConversationId: int
├── Title: string?
├── Participants: ICollection<ConversationParticipant>
├── Messages: ICollection<Message>
├── CreatedOn: DateTime
├── CreatedBy: string
└── TenantId: int

Message
├── MessageId: int
├── ConversationId: int
├── Content: string
├── SentBy: Guid
├── SentOn: DateTime
├── IsRead: bool
├── ReadOn: DateTime?
└── [Audit fields]
```

---

## Real-time Communication

The application uses SignalR for real-time features:

### SignalR Hubs
- **NotificationHub**: Real-time notification delivery
- **MessagingHub**: Real-time message delivery

### Events
| Event | Description |
|-------|-------------|
| `NotificationReceived` | New notification for user |
| `NotificationCountUpdated` | Badge count changed |
| `MessageReceived` | New message in conversation |
| `UserTyping` | User is typing in conversation |
| `ConversationUpdated` | Conversation metadata changed |

### Client Connection
```javascript
// Connect to notification hub
const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/notifications")
    .build();

notificationConnection.on("NotificationReceived", (notification) => {
    // Handle new notification
});

notificationConnection.start();
```

---

## Validation Rules Summary

| Entity | Field | Validation |
|--------|-------|------------|
| Notification | Title | Required |
| Notification | Message | Required |
| Notification | UserId | Required, valid user |
| Conversation | ParticipantIds | At least 2 participants |
| Message | Content | Required, max 5000 chars |

---

## Error Codes

| Code | Description |
|------|-------------|
| 400 | Invalid input data |
| 401 | Unauthorized access |
| 403 | Not a conversation participant |
| 404 | Notification or Conversation not found |
