import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type {
  DashboardStats,
  Widget,
  CreateWidgetRequest,
} from '../models/dashboard.models';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.baseUrl}/stats`);
  }

  getWidgets(): Observable<Widget[]> {
    return this.http.get<Widget[]>(`${this.baseUrl}/widgets`);
  }

  createWidget(request: CreateWidgetRequest): Observable<Widget> {
    return this.http.post<Widget>(`${this.baseUrl}/widgets`, request);
  }
}
