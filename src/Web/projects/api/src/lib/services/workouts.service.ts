import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type { Workout, CreateWorkoutRequest } from '../models/workout.models';

@Injectable({ providedIn: 'root' })
export class WorkoutsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getWorkouts(tenantId?: number): Observable<Workout[]> {
    let params = new HttpParams();
    if (tenantId != null) {
      params = params.set('tenantId', tenantId);
    }
    return this.http.get<Workout[]>(`${this.baseUrl}/workouts`, { params });
  }

  createWorkout(request: CreateWorkoutRequest): Observable<Workout> {
    return this.http.post<Workout>(`${this.baseUrl}/workouts`, request);
  }
}
