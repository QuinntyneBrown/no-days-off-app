import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type {
  BodyPart,
  CreateBodyPartRequest,
} from '../models/exercise.models';

@Injectable({ providedIn: 'root' })
export class BodyPartsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getBodyParts(tenantId?: number): Observable<BodyPart[]> {
    let params = new HttpParams();
    if (tenantId != null) {
      params = params.set('tenantId', tenantId);
    }
    return this.http.get<BodyPart[]>(`${this.baseUrl}/bodyparts`, { params });
  }

  createBodyPart(request: CreateBodyPartRequest): Observable<BodyPart> {
    return this.http.post<BodyPart>(`${this.baseUrl}/bodyparts`, request);
  }
}
