import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface MediaFile {
  mediaFileId: number;
  fileName: string;
  originalFileName: string;
  contentType: string;
  size: number;
  type: number;
  tenantId: number;
  entityId?: number;
  entityType?: string;
  uploadedAt: string;
  uploadedBy: string;
  url?: string;
}

@Injectable({
  providedIn: 'root'
})
export class MediaService {
  private readonly path = '/media';

  constructor(private api: ApiService) {}

  getAll(entityId?: number, entityType?: string): Observable<MediaFile[]> {
    let path = this.path;
    const params: string[] = [];
    if (entityId) params.push(`entityId=${entityId}`);
    if (entityType) params.push(`entityType=${entityType}`);
    if (params.length) path += `?${params.join('&')}`;
    return this.api.get<MediaFile[]>(path);
  }

  upload(file: File, entityId?: number, entityType?: string): Observable<MediaFile> {
    return this.api.upload<MediaFile>(this.path, file, entityId, entityType);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`${this.path}/${id}`);
  }
}
