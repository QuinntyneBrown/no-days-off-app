import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type { MediaFile } from '../models/media.models';

@Injectable({ providedIn: 'root' })
export class MediaService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getMedia(entityId?: number, entityType?: string): Observable<MediaFile[]> {
    let params = new HttpParams();
    if (entityId != null) {
      params = params.set('entityId', entityId);
    }
    if (entityType != null) {
      params = params.set('entityType', entityType);
    }
    return this.http.get<MediaFile[]>(`${this.baseUrl}/media`, { params });
  }

  uploadMedia(
    file: File,
    entityId?: number,
    entityType?: string
  ): Observable<MediaFile> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    let params = new HttpParams();
    if (entityId != null) {
      params = params.set('entityId', entityId);
    }
    if (entityType != null) {
      params = params.set('entityType', entityType);
    }

    return this.http.post<MediaFile>(`${this.baseUrl}/media`, formData, {
      params,
    });
  }

  deleteMedia(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/media/${id}`);
  }
}
