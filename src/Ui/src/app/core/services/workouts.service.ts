import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface Workout {
  workoutId: number;
  name: string;
  description?: string;
  tenantId: number;
}

export interface CreateWorkoutRequest {
  name: string;
  description?: string;
}

@Injectable({
  providedIn: 'root'
})
export class WorkoutsService {
  private readonly path = '/api/workouts';

  constructor(private api: ApiService) {}

  getAll(): Observable<Workout[]> {
    return this.api.get<Workout[]>(this.path);
  }

  getById(id: number): Observable<Workout> {
    return this.api.get<Workout>(`${this.path}/${id}`);
  }

  create(request: CreateWorkoutRequest): Observable<Workout> {
    return this.api.post<Workout>(this.path, request);
  }

  update(id: number, request: CreateWorkoutRequest): Observable<Workout> {
    return this.api.put<Workout>(`${this.path}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`${this.path}/${id}`);
  }
}
