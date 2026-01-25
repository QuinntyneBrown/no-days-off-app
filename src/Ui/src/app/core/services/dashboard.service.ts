import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface Widget {
  widgetId: number;
  name: string;
  type: number;
  tenantId: number;
  userId: number;
  position: number;
  width: number;
  height: number;
  configuration?: string;
  isVisible: boolean;
}

export interface DashboardStats {
  tenantId: number;
  userId: number;
  totalWorkouts: number;
  totalExercises: number;
  totalAthletes: number;
  workoutsThisWeek: number;
  workoutsThisMonth: number;
  lastUpdated: string;
}

export interface CreateWidgetRequest {
  name: string;
  type: number;
  position?: number;
  width?: number;
  height?: number;
  configuration?: string;
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private readonly widgetsPath = '/api/widgets';
  private readonly statsPath = '/api/stats';

  constructor(private api: ApiService) {}

  getWidgets(): Observable<Widget[]> {
    return this.api.get<Widget[]>(this.widgetsPath);
  }

  createWidget(request: CreateWidgetRequest): Observable<Widget> {
    return this.api.post<Widget>(this.widgetsPath, request);
  }

  getStats(): Observable<DashboardStats> {
    return this.api.get<DashboardStats>(this.statsPath);
  }
}
