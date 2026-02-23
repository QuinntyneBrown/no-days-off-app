import { Component, inject, OnInit, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { NdoAvatarComponent } from 'components';
import { ConversationsService } from 'api';
import type { Conversation, Message } from 'api';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-messages',
  imports: [MatIconModule, MatButtonModule, MatInputModule, MatFormFieldModule, FormsModule, NdoAvatarComponent, DatePipe],
  template: `
    <div class="messages-layout" data-testid="messages-page">
      <div class="chat-list">
        <div class="chat-list-header">
          <h3>Messages</h3>
        </div>
        @for (c of conversations(); track c.conversationId) {
          <div class="chat-item" [class.active]="selected()?.conversationId === c.conversationId"
               (click)="select(c)" [attr.data-testid]="'conversation-' + c.conversationId">
            <ndo-avatar [initials]="c.name.substring(0,2).toUpperCase()" [size]="40" />
            <div class="chat-info">
              <span class="chat-name">{{ c.name }}</span>
              @if (c.recentMessages.length) {
                <span class="chat-preview">{{ c.recentMessages[c.recentMessages.length - 1].content }}</span>
              }
            </div>
          </div>
        }
      </div>

      <div class="chat-view">
        @if (selected(); as conv) {
          <div class="chat-view-header">
            <ndo-avatar [initials]="conv.name.substring(0,2).toUpperCase()" [size]="40" />
            <div>
              <span class="chat-name">{{ conv.name }}</span>
              <span class="chat-status">Online</span>
            </div>
          </div>
          <div class="message-area" data-testid="message-area">
            @for (msg of conv.recentMessages; track msg.messageId) {
              <div class="msg" [class.sent]="msg.senderId === 'me'">
                <div class="msg-bubble">{{ msg.content }}</div>
                <span class="msg-time">{{ msg.sentAt | date:'shortTime' }}</span>
              </div>
            }
          </div>
          <div class="message-input">
            <mat-form-field appearance="outline" class="msg-field">
              <input matInput placeholder="Type a message..." [(ngModel)]="newMessage"
                     (keyup.enter)="send()" data-testid="message-input">
            </mat-form-field>
            <button mat-flat-button class="send-btn" (click)="send()" data-testid="send-btn">
              <mat-icon fontSet="material-symbols-rounded">send</mat-icon>
            </button>
          </div>
        } @else {
          <div class="empty-state">Select a conversation</div>
        }
      </div>
    </div>
  `,
  styles: `
    :host { display: block; height: calc(100vh - 128px); }
    .messages-layout { display: flex; height: 100%; border: 1px solid var(--ndo-border); }
    .chat-list { width: 340px; min-width: 340px; border-right: 1px solid var(--ndo-border); overflow-y: auto; background: var(--ndo-bg-card); }
    .chat-list-header { padding: 16px 20px; border-bottom: 1px solid var(--ndo-border); }
    .chat-list-header h3 { margin: 0; font-family: 'Plus Jakarta Sans', sans-serif; }
    .chat-item { display: flex; gap: 12px; padding: 12px 20px; cursor: pointer; border-bottom: 1px solid var(--ndo-border); }
    .chat-item:hover { background: var(--ndo-bg-elevated); }
    .chat-item.active { background: var(--ndo-primary-dim); border-left: 3px solid var(--ndo-primary); }
    .chat-info { flex: 1; display: flex; flex-direction: column; overflow: hidden; }
    .chat-name { font-weight: 600; font-size: 14px; }
    .chat-preview { font-size: 13px; color: var(--ndo-text-secondary); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
    .chat-status { font-size: 12px; color: var(--ndo-success); }
    .chat-view { flex: 1; display: flex; flex-direction: column; background: var(--ndo-bg-surface); }
    .chat-view-header { display: flex; gap: 12px; align-items: center; padding: 12px 20px; border-bottom: 1px solid var(--ndo-border); }
    .message-area { flex: 1; overflow-y: auto; padding: 20px; display: flex; flex-direction: column; gap: 12px; }
    .msg { display: flex; flex-direction: column; max-width: 70%; }
    .msg.sent { align-self: flex-end; }
    .msg-bubble { background: var(--ndo-bg-card); border: 1px solid var(--ndo-border); padding: 10px 14px; font-size: 14px; }
    .msg.sent .msg-bubble { background: var(--ndo-primary-dim); border-color: var(--ndo-primary-muted); }
    .msg-time { font-size: 11px; color: var(--ndo-text-tertiary); margin-top: 4px; }
    .message-input { display: flex; gap: 8px; padding: 12px 20px; border-top: 1px solid var(--ndo-border); }
    .msg-field { flex: 1; }
    .send-btn { --mdc-filled-button-container-color: var(--ndo-primary); --mdc-filled-button-label-text-color: var(--ndo-text-on-primary); }
    .empty-state { flex: 1; display: flex; align-items: center; justify-content: center; color: var(--ndo-text-tertiary); }
  `,
})
export class MessagesPage implements OnInit {
  private readonly conversationsService = inject(ConversationsService);
  conversations = signal<Conversation[]>([]);
  selected = signal<Conversation | null>(null);
  newMessage = '';

  ngOnInit() {
    this.conversationsService.getConversations().subscribe({
      next: (list) => { this.conversations.set(list); if (list.length) this.select(list[0]); },
      error: () => {},
    });
  }

  select(c: Conversation) { this.selected.set(c); }

  send() {
    const conv = this.selected();
    if (!conv || !this.newMessage.trim()) return;
    this.conversationsService.sendMessage({ conversationId: conv.conversationId, content: this.newMessage }).subscribe({
      next: (msg) => {
        conv.recentMessages = [...conv.recentMessages, msg];
        this.selected.set({ ...conv });
        this.newMessage = '';
      },
      error: () => {},
    });
  }
}
