import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type {
  Conversation,
  CreateConversationRequest,
  SendMessageRequest,
  Message,
} from '../models/communication.models';

@Injectable({ providedIn: 'root' })
export class ConversationsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getConversations(): Observable<Conversation[]> {
    return this.http.get<Conversation[]>(`${this.baseUrl}/conversations`);
  }

  getConversation(conversationId: number): Observable<Conversation> {
    return this.http.get<Conversation>(
      `${this.baseUrl}/conversations/${conversationId}`
    );
  }

  createConversation(
    request: CreateConversationRequest
  ): Observable<Conversation> {
    return this.http.post<Conversation>(
      `${this.baseUrl}/conversations`,
      request
    );
  }

  sendMessage(request: SendMessageRequest): Observable<Message> {
    return this.http.post<Message>(
      `${this.baseUrl}/conversations/${request.conversationId}/messages`,
      request
    );
  }
}
