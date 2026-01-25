import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ApiService } from './api.service';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

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

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  private readonly path = '/api/notifications';
  private hubConnection?: signalR.HubConnection;
  private notificationReceived = new Subject<{ title: string; message: string; type: number }>();

  notificationReceived$ = this.notificationReceived.asObservable();

  constructor(
    private api: ApiService,
    private authService: AuthService
  ) {}

  getAll(unreadOnly: boolean = false): Observable<Notification[]> {
    const path = unreadOnly ? `${this.path}?unreadOnly=true` : this.path;
    return this.api.get<Notification[]>(path);
  }

  create(request: CreateNotificationRequest): Observable<Notification> {
    return this.api.post<Notification>(this.path, request);
  }

  markAsRead(id: number): Observable<void> {
    return this.api.post<void>(`${this.path}/${id}/read`, {});
  }

  markAllAsRead(): Observable<{ markedAsRead: number }> {
    return this.api.post<{ markedAsRead: number }>(`${this.path}/read-all`, {});
  }

  // SignalR Connection
  startConnection(): void {
    const token = this.authService.getToken();
    if (!token) return;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/notifications`, {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveNotification', (notification: { title: string; message: string; type: number }) => {
      this.notificationReceived.next(notification);
    });

    this.hubConnection.start().catch(err => console.error('SignalR connection error:', err));
  }

  stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }
}
