export interface Conversation {
  conversationId: number;
  name: string;
  participantIds: string[];
  recentMessages: Message[];
  tenantId?: number;
  createdOn: string;
}

export interface Message {
  messageId: number;
  conversationId: number;
  senderId: string;
  content: string;
  sentAt: string;
  isRead: boolean;
}

export interface CreateConversationRequest {
  name: string;
  participantIds: string[];
  tenantId?: number;
}

export interface SendMessageRequest {
  conversationId: number;
  content: string;
}

export interface Notification {
  notificationId: number;
  title: string;
  message: string;
  type: number;
  tenantId: number;
  userId: number;
  isRead: boolean;
  createdAt: string;
  readAt?: string;
  actionUrl?: string;
  entityType?: string;
  entityId?: number;
}

export interface CreateNotificationRequest {
  title: string;
  message: string;
  type: number;
  userId?: number;
  actionUrl?: string;
  entityType?: string;
  entityId?: number;
}
