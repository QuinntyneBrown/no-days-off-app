import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';
import type { Tenant, CreateTenantRequest } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class TenantsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getTenants(): Observable<Tenant[]> {
    return this.http.get<Tenant[]>(`${this.baseUrl}/tenants`);
  }

  getTenant(tenantId: number): Observable<Tenant> {
    return this.http.get<Tenant>(`${this.baseUrl}/tenants/${tenantId}`);
  }

  createTenant(request: CreateTenantRequest): Observable<Tenant> {
    return this.http.post<Tenant>(`${this.baseUrl}/tenants`, request);
  }
}
