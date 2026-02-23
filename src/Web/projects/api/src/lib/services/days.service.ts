import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type {
  Day,
  CreateDayRequest,
  UpdateDayRequest,
} from '../models/workout.models';

@Injectable({ providedIn: 'root' })
export class DaysService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getDays(tenantId?: number): Observable<Day[]> {
    let params = new HttpParams();
    if (tenantId != null) {
      params = params.set('tenantId', tenantId);
    }
    return this.http.get<Day[]>(`${this.baseUrl}/days`, { params });
  }

  getDay(dayId: number): Observable<Day> {
    return this.http.get<Day>(`${this.baseUrl}/days/${dayId}`);
  }

  createDay(request: CreateDayRequest): Observable<Day> {
    return this.http.post<Day>(`${this.baseUrl}/days`, request);
  }

  updateDay(dayId: number, request: UpdateDayRequest): Observable<Day> {
    return this.http.put<Day>(`${this.baseUrl}/days/${dayId}`, request);
  }

  deleteDay(dayId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/days/${dayId}`);
  }
}
