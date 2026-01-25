import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  get<T>(path: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}${path}`);
  }

  post<T>(path: string, body: any): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}${path}`, body);
  }

  put<T>(path: string, body: any): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}${path}`, body);
  }

  delete<T>(path: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}${path}`);
  }

  upload<T>(path: string, file: File, entityId?: number, entityType?: string): Observable<T> {
    const formData = new FormData();
    formData.append('file', file);
    let url = `${this.baseUrl}${path}`;
    const params: string[] = [];
    if (entityId) params.push(`entityId=${entityId}`);
    if (entityType) params.push(`entityType=${entityType}`);
    if (params.length) url += `?${params.join('&')}`;
    return this.http.post<T>(url, formData);
  }
}
