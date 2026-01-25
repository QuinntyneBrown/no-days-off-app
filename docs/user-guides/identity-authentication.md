# Identity & Authentication User Guide

## Overview

The Identity microservice handles all authentication, authorization, user management, and multi-tenant operations in the No Days Off application.

---

## Behaviors

### 1. User Registration

**Purpose**: Create a new user account in the system.

**Command**: `RegisterUserCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Email | string | Yes | User's email address (unique) |
| Password | string | Yes | User's password (min 8 chars) |
| FirstName | string | Yes | User's first name |
| LastName | string | Yes | User's last name |
| TenantId | int | No | Tenant identifier (defaults to system tenant) |

#### API Endpoint
```
POST /api/auth/register
Content-Type: application/json

{
    "email": "john.doe@example.com",
    "password": "SecurePass123!",
    "firstName": "John",
    "lastName": "Doe"
}
```

#### Response
```json
{
    "userId": "guid-here",
    "email": "john.doe@example.com",
    "message": "Registration successful"
}
```

#### Business Rules
- Email must be unique across the tenant
- Password must meet complexity requirements
- A verification email is sent upon registration

#### Error Scenarios
| Error Code | Description |
|------------|-------------|
| 400 | Invalid input data |
| 409 | Email already exists |

---

### 2. User Login

**Purpose**: Authenticate a user and obtain access tokens.

**Command**: `LoginCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Email | string | Yes | User's registered email |
| Password | string | Yes | User's password |

#### API Endpoint
```
POST /api/auth/login
Content-Type: application/json

{
    "email": "john.doe@example.com",
    "password": "SecurePass123!"
}
```

#### Response
```json
{
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "refresh-token-guid",
    "expiresIn": 3600,
    "tokenType": "Bearer"
}
```

#### Business Rules
- Account must be verified
- Account must not be locked
- Failed attempts are tracked for security

#### Error Scenarios
| Error Code | Description |
|------------|-------------|
| 401 | Invalid credentials |
| 403 | Account locked or unverified |

---

### 3. Token Refresh

**Purpose**: Obtain a new access token using a refresh token.

**Command**: `RefreshTokenCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| RefreshToken | string | Yes | Valid refresh token |

#### API Endpoint
```
POST /api/auth/refresh-token
Content-Type: application/json

{
    "refreshToken": "refresh-token-guid"
}
```

#### Response
```json
{
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "new-refresh-token-guid",
    "expiresIn": 3600
}
```

#### Business Rules
- Refresh token must be valid and not expired
- Old refresh token is invalidated after use
- Each refresh generates a new token pair

---

### 4. Token Revocation

**Purpose**: Invalidate a refresh token (logout).

**Command**: `RevokeTokenCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| RefreshToken | string | Yes | Token to revoke |

#### API Endpoint
```
POST /api/auth/revoke-token
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "refreshToken": "refresh-token-guid"
}
```

#### Response
```json
{
    "message": "Token revoked successfully"
}
```

---

### 5. Create Tenant

**Purpose**: Create a new tenant for multi-tenant isolation.

**Command**: `CreateTenantCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Tenant name |
| Identifier | string | Yes | Unique tenant identifier |

#### API Endpoint
```
POST /api/tenants
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Fitness Studio ABC",
    "identifier": "fitness-studio-abc"
}
```

#### Response
```json
{
    "tenantId": 1,
    "name": "Fitness Studio ABC",
    "identifier": "fitness-studio-abc",
    "createdOn": "2024-01-15T10:30:00Z"
}
```

#### Business Rules
- Identifier must be unique across system
- Only system administrators can create tenants

---

### 6. Get Tenants

**Purpose**: Retrieve list of all tenants.

**Query**: `GetTenantsQuery`

#### API Endpoint
```
GET /api/tenants
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "tenants": [
        {
            "tenantId": 1,
            "name": "Fitness Studio ABC",
            "identifier": "fitness-studio-abc"
        },
        {
            "tenantId": 2,
            "name": "Gym XYZ",
            "identifier": "gym-xyz"
        }
    ]
}
```

---

### 7. Get Tenant by ID

**Purpose**: Retrieve a specific tenant by its ID.

**Query**: `GetTenantByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
GET /api/tenants/{tenantId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "tenantId": 1,
    "name": "Fitness Studio ABC",
    "identifier": "fitness-studio-abc",
    "createdOn": "2024-01-15T10:30:00Z",
    "users": []
}
```

---

### 8. Get Users

**Purpose**: Retrieve list of users within the current tenant.

**Query**: `GetUsersQuery`

#### API Endpoint
```
GET /api/users
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "users": [
        {
            "userId": "guid-1",
            "email": "john.doe@example.com",
            "firstName": "John",
            "lastName": "Doe",
            "roles": ["Athlete"]
        }
    ]
}
```

---

### 9. Get User by ID

**Purpose**: Retrieve a specific user's details.

**Query**: `GetUserByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| UserId | Guid | Yes | User identifier |

#### API Endpoint
```
GET /api/users/{userId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "userId": "guid-here",
    "email": "john.doe@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "roles": ["Athlete"],
    "tenantId": 1,
    "createdOn": "2024-01-15T10:30:00Z"
}
```

---

## Authentication Flow

```
+--------+                                +--------+
| Client |                                | Server |
+--------+                                +--------+
    |                                          |
    |  1. POST /api/auth/login                 |
    |  (email, password)                       |
    |----------------------------------------->|
    |                                          |
    |  2. Validate credentials                 |
    |  3. Generate tokens                      |
    |                                          |
    |  4. Return access + refresh tokens       |
    |<-----------------------------------------|
    |                                          |
    |  5. GET /api/athletes                    |
    |  Authorization: Bearer {accessToken}     |
    |----------------------------------------->|
    |                                          |
    |  6. Validate token                       |
    |  7. Process request                      |
    |                                          |
    |  8. Return data                          |
    |<-----------------------------------------|
    |                                          |
    |  [When token expires]                    |
    |                                          |
    |  9. POST /api/auth/refresh-token         |
    |  (refreshToken)                          |
    |----------------------------------------->|
    |                                          |
    |  10. Return new token pair               |
    |<-----------------------------------------|
```

---

## Security Considerations

1. **Password Storage**: Passwords are hashed using industry-standard algorithms
2. **Token Expiration**: Access tokens expire after 1 hour
3. **Refresh Token Rotation**: Refresh tokens are rotated on each use
4. **Rate Limiting**: Authentication endpoints are rate-limited
5. **Account Lockout**: Accounts are locked after multiple failed attempts

---

## Role-Based Access Control

| Role | Permissions |
|------|-------------|
| SystemAdmin | Full system access |
| TenantAdmin | Full tenant access |
| Coach | Manage athletes, workouts |
| Athlete | View/edit own profile |
