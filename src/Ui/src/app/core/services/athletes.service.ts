import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface Athlete {
  athleteId: number;
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
  dateOfBirth?: string;
  tenantId: number;
}

export interface CreateAthleteRequest {
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
  dateOfBirth?: string;
}

export interface UpdateAthleteRequest {
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
  dateOfBirth?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AthletesService {
  private readonly path = '/api/athletes';

  constructor(private api: ApiService) {}

  getAll(): Observable<Athlete[]> {
    return this.api.get<Athlete[]>(this.path);
  }

  getById(id: number): Observable<Athlete> {
    return this.api.get<Athlete>(`${this.path}/${id}`);
  }

  create(request: CreateAthleteRequest): Observable<Athlete> {
    return this.api.post<Athlete>(this.path, request);
  }

  update(id: number, request: UpdateAthleteRequest): Observable<Athlete> {
    return this.api.put<Athlete>(`${this.path}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`${this.path}/${id}`);
  }
}
