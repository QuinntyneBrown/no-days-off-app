import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface Athlete {
  athleteId: number;
  name: string;
  username: string;
  imageUrl?: string;
  currentWeight?: number;
  lastWeighedOn?: string;
  tenantId?: number;
  createdOn: string;
  createdBy: string;
}

export interface CreateAthleteRequest {
  name: string;
  username: string;
  tenantId?: number;
}

export interface UpdateAthleteRequest {
  athleteId: number;
  name: string;
  username: string;
}

@Injectable({
  providedIn: 'root'
})
export class AthletesService {
  private readonly path = '/athletes';

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
