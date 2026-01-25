import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface Exercise {
  exerciseId: number;
  name: string;
  description?: string;
  tenantId: number;
  bodyPartId?: number;
  videoUrl?: string;
  imageUrl?: string;
  instructions?: string;
  type: number;
}

export interface BodyPart {
  bodyPartId: number;
  name: string;
  description?: string;
  tenantId: number;
}

export interface CreateExerciseRequest {
  name: string;
  description?: string;
  bodyPartId?: number;
  videoUrl?: string;
  imageUrl?: string;
  instructions?: string;
  type?: number;
}

export interface CreateBodyPartRequest {
  name: string;
  description?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ExercisesService {
  private readonly exercisesPath = '/exercises';
  private readonly bodyPartsPath = '/bodyparts';

  constructor(private api: ApiService) {}

  // Exercises
  getAllExercises(bodyPartId?: number): Observable<Exercise[]> {
    let path = this.exercisesPath;
    if (bodyPartId) path += `?bodyPartId=${bodyPartId}`;
    return this.api.get<Exercise[]>(path);
  }

  getExerciseById(id: number): Observable<Exercise> {
    return this.api.get<Exercise>(`${this.exercisesPath}/${id}`);
  }

  createExercise(request: CreateExerciseRequest): Observable<Exercise> {
    return this.api.post<Exercise>(this.exercisesPath, request);
  }

  updateExercise(id: number, request: CreateExerciseRequest): Observable<Exercise> {
    return this.api.put<Exercise>(`${this.exercisesPath}/${id}`, request);
  }

  deleteExercise(id: number): Observable<void> {
    return this.api.delete<void>(`${this.exercisesPath}/${id}`);
  }

  // Body Parts
  getAllBodyParts(): Observable<BodyPart[]> {
    return this.api.get<BodyPart[]>(this.bodyPartsPath);
  }

  createBodyPart(request: CreateBodyPartRequest): Observable<BodyPart> {
    return this.api.post<BodyPart>(this.bodyPartsPath, request);
  }
}
