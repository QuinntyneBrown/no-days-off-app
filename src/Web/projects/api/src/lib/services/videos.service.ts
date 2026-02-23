import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type {
  Video,
  CreateVideoRequest,
  UpdateVideoRequest,
} from '../models/media.models';

@Injectable({ providedIn: 'root' })
export class VideosService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getVideos(tenantId?: number): Observable<Video[]> {
    let params = new HttpParams();
    if (tenantId != null) {
      params = params.set('tenantId', tenantId);
    }
    return this.http.get<Video[]>(`${this.baseUrl}/videos`, { params });
  }

  getVideo(videoId: number): Observable<Video> {
    return this.http.get<Video>(`${this.baseUrl}/videos/${videoId}`);
  }

  createVideo(request: CreateVideoRequest): Observable<Video> {
    return this.http.post<Video>(`${this.baseUrl}/videos`, request);
  }

  updateVideo(videoId: number, request: UpdateVideoRequest): Observable<Video> {
    return this.http.put<Video>(
      `${this.baseUrl}/videos/${videoId}`,
      request
    );
  }

  deleteVideo(videoId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/videos/${videoId}`);
  }
}
