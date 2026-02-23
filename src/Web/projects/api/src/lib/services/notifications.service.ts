import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type {
  Notification,
  CreateNotificationRequest,
} from '../models/communication.models';

@Injectable({ providedIn: 'root' })
export class NotificationsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getNotifications(unreadOnly?: boolean): Observable<Notification[]> {
    let params = new HttpParams();
    if (unreadOnly != null) {
      params = params.set('unreadOnly', unreadOnly);
    }
    return this.http.get<Notification[]>(`${this.baseUrl}/notifications`, {
      params,
    });
  }

  createNotification(
    request: CreateNotificationRequest
  ): Observable<Notification> {
    return this.http.post<Notification>(
      `${this.baseUrl}/notifications`,
      request
    );
  }

  markAsRead(id: number): Observable<void> {
    return this.http.post<void>(
      `${this.baseUrl}/notifications/${id}/read`,
      null
    );
  }

  markAllAsRead(): Observable<{ markedAsRead: number }> {
    return this.http.post<{ markedAsRead: number }>(
      `${this.baseUrl}/notifications/read-all`,
      null
    );
  }
}
