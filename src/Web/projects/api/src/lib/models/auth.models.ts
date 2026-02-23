export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  tenantId?: number;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: User;
}

export interface User {
  userId: number;
  email: string;
  firstName: string;
  lastName: string;
  tenantId?: number;
  createdOn: string;
  roles: string[];
}

export interface Tenant {
  tenantId: number;
  uniqueId: string;
  name: string;
  createdOn: string;
}

export interface CreateTenantRequest {
  name: string;
}
