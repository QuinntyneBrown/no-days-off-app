import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type {
  Exercise,
  CreateExerciseRequest,
  UpdateExerciseRequest,
} from '../models/exercise.models';

@Injectable({ providedIn: 'root' })
export class ExercisesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getExercises(tenantId?: number, bodyPartId?: number): Observable<Exercise[]> {
    let params = new HttpParams();
    if (tenantId != null) {
      params = params.set('tenantId', tenantId);
    }
    if (bodyPartId != null) {
      params = params.set('bodyPartId', bodyPartId);
    }
    return this.http.get<Exercise[]>(`${this.baseUrl}/exercises`, { params });
  }

  getExercise(id: number, tenantId?: number): Observable<Exercise> {
    let params = new HttpParams();
    if (tenantId != null) {
      params = params.set('tenantId', tenantId);
    }
    return this.http.get<Exercise>(`${this.baseUrl}/exercises/${id}`, {
      params,
    });
  }

  createExercise(request: CreateExerciseRequest): Observable<Exercise> {
    return this.http.post<Exercise>(`${this.baseUrl}/exercises`, request);
  }

  updateExercise(
    id: number,
    request: UpdateExerciseRequest
  ): Observable<Exercise> {
    return this.http.put<Exercise>(
      `${this.baseUrl}/exercises/${id}`,
      request
    );
  }

  deleteExercise(id: number, tenantId?: number): Observable<void> {
    let params = new HttpParams();
    if (tenantId != null) {
      params = params.set('tenantId', tenantId);
    }
    return this.http.delete<void>(`${this.baseUrl}/exercises/${id}`, {
      params,
    });
  }
}
