import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type {
  ScheduledExercise,
  CreateScheduledExerciseRequest,
  UpdateScheduledExerciseRequest,
} from '../models/workout.models';

@Injectable({ providedIn: 'root' })
export class ScheduledExercisesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getScheduledExercises(
    tenantId?: number,
    dayId?: number
  ): Observable<ScheduledExercise[]> {
    let params = new HttpParams();
    if (tenantId != null) {
      params = params.set('tenantId', tenantId);
    }
    if (dayId != null) {
      params = params.set('dayId', dayId);
    }
    return this.http.get<ScheduledExercise[]>(
      `${this.baseUrl}/scheduled-exercises`,
      { params }
    );
  }

  getScheduledExercise(id: number): Observable<ScheduledExercise> {
    return this.http.get<ScheduledExercise>(
      `${this.baseUrl}/scheduled-exercises/${id}`
    );
  }

  createScheduledExercise(
    request: CreateScheduledExerciseRequest
  ): Observable<ScheduledExercise> {
    return this.http.post<ScheduledExercise>(
      `${this.baseUrl}/scheduled-exercises`,
      request
    );
  }

  updateScheduledExercise(
    id: number,
    request: UpdateScheduledExerciseRequest
  ): Observable<ScheduledExercise> {
    return this.http.put<ScheduledExercise>(
      `${this.baseUrl}/scheduled-exercises/${id}`,
      request
    );
  }

  deleteScheduledExercise(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/scheduled-exercises/${id}`
    );
  }
}
