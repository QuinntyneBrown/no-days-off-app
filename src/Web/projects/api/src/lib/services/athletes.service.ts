import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type {
  Athlete,
  CreateAthleteRequest,
  UpdateAthleteRequest,
} from '../models/athlete.models';

@Injectable({ providedIn: 'root' })
export class AthletesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getAthletes(tenantId?: number): Observable<Athlete[]> {
    let params = new HttpParams();
    if (tenantId != null) {
      params = params.set('tenantId', tenantId);
    }
    return this.http.get<Athlete[]>(`${this.baseUrl}/athletes`, { params });
  }

  getAthlete(athleteId: number): Observable<Athlete> {
    return this.http.get<Athlete>(
      `${this.baseUrl}/athletes/${athleteId}`
    );
  }

  createAthlete(request: CreateAthleteRequest): Observable<Athlete> {
    return this.http.post<Athlete>(`${this.baseUrl}/athletes`, request);
  }

  updateAthlete(
    athleteId: number,
    request: UpdateAthleteRequest
  ): Observable<Athlete> {
    return this.http.put<Athlete>(
      `${this.baseUrl}/athletes/${athleteId}`,
      request
    );
  }

  deleteAthlete(athleteId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/athletes/${athleteId}`
    );
  }
}
