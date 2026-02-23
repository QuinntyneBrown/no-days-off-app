import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatBadgeModule } from '@angular/material/badge';
import { NotificationsService } from 'api';
import type { Notification } from 'api';
import { DatePipe } from '@angular/common';

const TYPE_CONFIG: Record<number, { icon: string; color: string; bg: string }> = {
  0: { icon: 'info', color: 'var(--ndo-info)', bg: 'var(--ndo-info-dim)' },
  1: { icon: 'check_circle', color: 'var(--ndo-success)', bg: 'var(--ndo-success-dim)' },
  2: { icon: 'warning', color: 'var(--ndo-warning)', bg: 'var(--ndo-warning-dim)' },
  3: { icon: 'error', color: 'var(--ndo-error)', bg: 'var(--ndo-error-dim)' },
};

@Component({
  selector: 'app-notifications',
  imports: [MatButtonModule, MatIconModule, MatChipsModule, MatBadgeModule, DatePipe],
  template: `
    <div class="page" data-testid="notifications-page">
      <div class="page-header">
        <div class="title-row">
          <h2 class="page-title">All Notifications</h2>
          @if (unreadCount() > 0) {
            <span class="badge" data-testid="unread-badge">{{ unreadCount() }} new</span>
          }
        </div>
        <button mat-button (click)="markAllRead()" data-testid="mark-all-read">Mark all as read</button>
      </div>

      <mat-chip-listbox (change)="onFilter($event.value)" data-testid="notification-filters">
        <mat-chip-option value="all" selected>All</mat-chip-option>
        <mat-chip-option value="unread">Unread</mat-chip-option>
      </mat-chip-listbox>

      <div class="notification-list" data-testid="notification-list">
        @for (n of filtered(); track n.notificationId) {
          <div class="notification-item" [class.unread]="!n.isRead"
               (click)="markRead(n)" [attr.data-testid]="'notification-' + n.notificationId">
            <div class="notif-icon" [style.background]="getConfig(n.type).bg" [style.color]="getConfig(n.type).color">
              <mat-icon fontSet="material-symbols-rounded">{{ getConfig(n.type).icon }}</mat-icon>
            </div>
            <div class="notif-content">
              <span class="notif-title">{{ n.title }}</span>
              <span class="notif-message">{{ n.message }}</span>
            </div>
            <div class="notif-meta">
              <span class="notif-time">{{ n.createdAt | date:'shortTime' }}</span>
              @if (!n.isRead) { <span class="dot"></span> }
            </div>
          </div>
        }
      </div>
    </div>
  `,
  styles: `
    .page { display: flex; flex-direction: column; gap: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: center; }
    .title-row { display: flex; align-items: center; gap: 12px; }
    .page-title { font-family: 'Plus Jakarta Sans', sans-serif; font-size: 20px; font-weight: 700; margin: 0; }
    .badge { background: var(--ndo-accent); color: #fff; padding: 2px 10px; border-radius: 12px; font-size: 12px; font-weight: 600; }
    .notification-list { display: flex; flex-direction: column; }
    .notification-item {
      display: flex; gap: 16px; padding: 16px 20px; align-items: flex-start;
      border-bottom: 1px solid var(--ndo-border); cursor: pointer;
    }
    .notification-item:hover { background: var(--ndo-bg-elevated); }
    .notification-item.unread { background: var(--ndo-bg-card); }
    .notif-icon {
      width: 40px; height: 40px; min-width: 40px; border-radius: 50%;
      display: flex; align-items: center; justify-content: center;
    }
    .notif-content { flex: 1; display: flex; flex-direction: column; gap: 2px; }
    .notif-title { font-weight: 600; font-size: 14px; }
    .notif-message { font-size: 13px; color: var(--ndo-text-secondary); }
    .notif-meta { display: flex; flex-direction: column; align-items: flex-end; gap: 6px; }
    .notif-time { font-size: 12px; color: var(--ndo-text-tertiary); white-space: nowrap; }
    .dot { width: 8px; height: 8px; border-radius: 50%; background: var(--ndo-primary); }
  `,
})
export class NotificationsPage implements OnInit {
  private readonly notificationsService = inject(NotificationsService);
  notifications = signal<Notification[]>([]);
  filtered = signal<Notification[]>([]);
  private showUnreadOnly = false;

  get unreadCount() { return signal(this.notifications().filter(n => !n.isRead).length); }

  ngOnInit() { this.load(); }

  load() {
    this.notificationsService.getNotifications().subscribe({
      next: (list) => { this.notifications.set(list); this.applyFilter(); },
      error: () => {},
    });
  }

  getConfig(type: number) { return TYPE_CONFIG[type] ?? TYPE_CONFIG[0]; }

  onFilter(val: string) {
    this.showUnreadOnly = val === 'unread';
    this.applyFilter();
  }

  private applyFilter() {
    this.filtered.set(this.showUnreadOnly ? this.notifications().filter(n => !n.isRead) : this.notifications());
  }

  markRead(n: Notification) {
    if (!n.isRead) {
      this.notificationsService.markAsRead(n.notificationId).subscribe({
        next: () => { n.isRead = true; this.notifications.set([...this.notifications()]); this.applyFilter(); },
        error: () => {},
      });
    }
  }

  markAllRead() {
    this.notificationsService.markAllAsRead().subscribe({
      next: () => {
        this.notifications().forEach(n => n.isRead = true);
        this.notifications.set([...this.notifications()]);
        this.applyFilter();
      },
      error: () => {},
    });
  }
}
