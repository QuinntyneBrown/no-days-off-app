# Identity & Authentication Use Case Diagrams

## Overview Diagram

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                      IDENTITY & AUTHENTICATION SYSTEM                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│    ┌─────────┐                                                              │
│    │         │                                                              │
│    │  User   │─────────┬──────────────────────────────────────────┐         │
│    │         │         │                                          │         │
│    └─────────┘         │                                          │         │
│         │              │                                          │         │
│         │              ▼                                          ▼         │
│         │    ┌───────────────────┐                    ┌───────────────────┐ │
│         │    │   UC1: Register   │                    │  UC2: Login       │ │
│         │    │     Account       │                    │                   │ │
│         │    └───────────────────┘                    └───────────────────┘ │
│         │                                                      │            │
│         │                                                      │            │
│         │                                         ┌────────────┴───────┐    │
│         │                                         ▼                    ▼    │
│         │                               ┌──────────────────┐  ┌──────────┐  │
│         │                               │UC3: Refresh Token│  │UC4: Logout│  │
│         │                               └──────────────────┘  └──────────┘  │
│         │                                                                   │
│         ▼                                                                   │
│    ┌───────────────────┐                                                    │
│    │  UC6: View Users  │                                                    │
│    └───────────────────┘                                                    │
│                                                                             │
│    ┌─────────┐                                                              │
│    │         │                                                              │
│    │  Admin  │─────────┬──────────────────────────────────────────┐         │
│    │         │         │                                          │         │
│    └─────────┘         │                                          │         │
│                        ▼                                          ▼         │
│              ┌───────────────────┐                    ┌───────────────────┐ │
│              │UC5: Manage Tenants│                    │UC7: Manage Roles  │ │
│              │ - Create          │                    │                   │ │
│              │ - View            │                    └───────────────────┘ │
│              │ - Update          │                                          │
│              └───────────────────┘                                          │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## UC1: Register Account

```
┌────────────────────────────────────────────────────────────────┐
│                    UC1: Register Account                        │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│  Actor: User (Unauthenticated)                                 │
│                                                                │
│  ┌──────┐      ┌─────────────┐      ┌─────────────┐           │
│  │ User │ ──▶  │ Registration│ ──▶  │  Validation │           │
│  └──────┘      │   Form      │      │   Service   │           │
│                └─────────────┘      └──────┬──────┘           │
│                                            │                   │
│                                            ▼                   │
│                                    ┌─────────────┐            │
│                                    │ Create User │            │
│                                    │   Record    │            │
│                                    └──────┬──────┘            │
│                                           │                   │
│                            ┌──────────────┼──────────────┐    │
│                            ▼              ▼              ▼    │
│                    ┌───────────┐  ┌───────────┐  ┌───────────┐│
│                    │  Hash     │  │  Create   │  │   Send    ││
│                    │ Password  │  │  Athlete  │  │   Email   ││
│                    └───────────┘  │  Profile  │  └───────────┘│
│                                   └───────────┘               │
└────────────────────────────────────────────────────────────────┘
```

### UC1 Details

| Attribute | Value |
|-----------|-------|
| **Name** | Register Account |
| **Actor** | User (Unauthenticated) |
| **Trigger** | User submits registration form |
| **Preconditions** | Email not already registered |
| **Postconditions** | User account created, verification email sent |

**Main Flow:**
1. User enters registration details (email, password, name)
2. System validates input data
3. System checks email uniqueness
4. System hashes password
5. System creates user record
6. System creates default athlete profile
7. System sends verification email
8. System returns success response

**Alternative Flows:**
- **A1**: Email already exists → Return 409 Conflict
- **A2**: Invalid password format → Return 400 Bad Request
- **A3**: Email service unavailable → Log error, account still created

---

## UC2: Login

```
┌────────────────────────────────────────────────────────────────┐
│                         UC2: Login                              │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│  ┌──────┐      ┌─────────────┐      ┌─────────────┐           │
│  │ User │ ──▶  │   Login     │ ──▶  │  Validate   │           │
│  └──────┘      │   Request   │      │ Credentials │           │
│                └─────────────┘      └──────┬──────┘           │
│                                            │                   │
│                              ┌─────────────┴─────────────┐    │
│                              │                           │    │
│                        [Valid]                      [Invalid]  │
│                              │                           │    │
│                              ▼                           ▼    │
│                    ┌───────────────┐           ┌─────────────┐│
│                    │   Generate    │           │   Return    ││
│                    │    Tokens     │           │    401      ││
│                    │ - Access      │           └─────────────┘│
│                    │ - Refresh     │                          │
│                    └───────┬───────┘                          │
│                            │                                  │
│                            ▼                                  │
│                    ┌───────────────┐                          │
│                    │  Return Token │                          │
│                    │   Response    │                          │
│                    └───────────────┘                          │
└────────────────────────────────────────────────────────────────┘
```

### UC2 Details

| Attribute | Value |
|-----------|-------|
| **Name** | Login |
| **Actor** | User |
| **Trigger** | User submits login credentials |
| **Preconditions** | User account exists, account verified |
| **Postconditions** | Access and refresh tokens issued |

**Main Flow:**
1. User submits email and password
2. System retrieves user by email
3. System validates password hash
4. System checks account status (verified, not locked)
5. System generates JWT access token
6. System generates refresh token
7. System stores refresh token
8. System returns token pair

---

## UC3: Refresh Token

```
┌────────────────────────────────────────────────────────────────┐
│                      UC3: Refresh Token                         │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│  ┌──────┐      ┌─────────────┐      ┌─────────────┐           │
│  │ User │ ──▶  │   Refresh   │ ──▶  │  Validate   │           │
│  └──────┘      │   Request   │      │   Token     │           │
│                └─────────────┘      └──────┬──────┘           │
│                                            │                   │
│                                            ▼                   │
│                                    ┌─────────────┐            │
│                                    │  Invalidate │            │
│                                    │  Old Token  │            │
│                                    └──────┬──────┘            │
│                                           │                   │
│                                           ▼                   │
│                                   ┌─────────────┐             │
│                                   │  Generate   │             │
│                                   │  New Pair   │             │
│                                   └──────┬──────┘             │
│                                          │                    │
│                                          ▼                    │
│                                   ┌─────────────┐             │
│                                   │   Return    │             │
│                                   │   Tokens    │             │
│                                   └─────────────┘             │
└────────────────────────────────────────────────────────────────┘
```

---

## UC4: Revoke Token (Logout)

```
┌────────────────────────────────────────────────────────────────┐
│                    UC4: Revoke Token                            │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│  ┌──────┐      ┌─────────────┐      ┌─────────────┐           │
│  │ User │ ──▶  │   Revoke    │ ──▶  │  Find Token │           │
│  └──────┘      │   Request   │      │  in Store   │           │
│                └─────────────┘      └──────┬──────┘           │
│                                            │                   │
│                                            ▼                   │
│                                    ┌─────────────┐            │
│                                    │  Mark Token │            │
│                                    │  as Revoked │            │
│                                    └──────┬──────┘            │
│                                           │                   │
│                                           ▼                   │
│                                   ┌─────────────┐             │
│                                   │   Return    │             │
│                                   │   Success   │             │
│                                   └─────────────┘             │
└────────────────────────────────────────────────────────────────┘
```

---

## UC5: Manage Tenants

```
┌────────────────────────────────────────────────────────────────┐
│                    UC5: Manage Tenants                          │
├────────────────────────────────────────────────────────────────┤
│                                                                │
│  ┌───────┐                                                     │
│  │ Admin │                                                     │
│  └───┬───┘                                                     │
│      │                                                         │
│      ├───────────────────┬───────────────────┐                │
│      ▼                   ▼                   ▼                │
│ ┌──────────┐       ┌──────────┐       ┌──────────┐           │
│ │  Create  │       │   View   │       │  Update  │           │
│ │  Tenant  │       │  Tenants │       │  Tenant  │           │
│ └────┬─────┘       └────┬─────┘       └────┬─────┘           │
│      │                  │                  │                  │
│      ▼                  ▼                  ▼                  │
│ ┌──────────┐       ┌──────────┐       ┌──────────┐           │
│ │ Validate │       │  Return  │       │ Validate │           │
│ │   Name   │       │   List   │       │  Update  │           │
│ └────┬─────┘       └──────────┘       └────┬─────┘           │
│      │                                     │                  │
│      ▼                                     ▼                  │
│ ┌──────────┐                         ┌──────────┐            │
│ │  Create  │                         │   Save   │            │
│ │  Record  │                         │  Changes │            │
│ └──────────┘                         └──────────┘            │
│                                                              │
└──────────────────────────────────────────────────────────────┘
```

---

## Authentication Sequence Diagram

```
┌──────┐          ┌─────────┐          ┌─────────┐          ┌─────────┐
│Client│          │   API   │          │Identity │          │Database │
└──┬───┘          └────┬────┘          └────┬────┘          └────┬────┘
   │                   │                    │                    │
   │ POST /auth/login  │                    │                    │
   │──────────────────▶│                    │                    │
   │                   │                    │                    │
   │                   │  LoginCommand      │                    │
   │                   │───────────────────▶│                    │
   │                   │                    │                    │
   │                   │                    │  Query User        │
   │                   │                    │───────────────────▶│
   │                   │                    │                    │
   │                   │                    │◀───────────────────│
   │                   │                    │   User Record      │
   │                   │                    │                    │
   │                   │                    │  Validate Password │
   │                   │                    │────────┐           │
   │                   │                    │        │           │
   │                   │                    │◀───────┘           │
   │                   │                    │                    │
   │                   │                    │  Generate Tokens   │
   │                   │                    │────────┐           │
   │                   │                    │        │           │
   │                   │                    │◀───────┘           │
   │                   │                    │                    │
   │                   │                    │  Store Refresh     │
   │                   │                    │───────────────────▶│
   │                   │                    │                    │
   │                   │◀───────────────────│                    │
   │                   │   Token Response   │                    │
   │                   │                    │                    │
   │◀──────────────────│                    │                    │
   │  200 OK + Tokens  │                    │                    │
   │                   │                    │                    │
```

---

## Token Refresh Sequence

```
┌──────┐          ┌─────────┐          ┌─────────┐          ┌─────────┐
│Client│          │   API   │          │Identity │          │Database │
└──┬───┘          └────┬────┘          └────┬────┘          └────┬────┘
   │                   │                    │                    │
   │ POST /auth/refresh│                    │                    │
   │ RefreshToken      │                    │                    │
   │──────────────────▶│                    │                    │
   │                   │                    │                    │
   │                   │RefreshTokenCommand │                    │
   │                   │───────────────────▶│                    │
   │                   │                    │                    │
   │                   │                    │  Find Token        │
   │                   │                    │───────────────────▶│
   │                   │                    │                    │
   │                   │                    │◀───────────────────│
   │                   │                    │                    │
   │                   │                    │  Validate Token    │
   │                   │                    │────────┐           │
   │                   │                    │        │           │
   │                   │                    │◀───────┘           │
   │                   │                    │                    │
   │                   │                    │  Invalidate Old    │
   │                   │                    │───────────────────▶│
   │                   │                    │                    │
   │                   │                    │  Generate New Pair │
   │                   │                    │────────┐           │
   │                   │                    │        │           │
   │                   │                    │◀───────┘           │
   │                   │                    │                    │
   │                   │                    │  Store New Refresh │
   │                   │                    │───────────────────▶│
   │                   │                    │                    │
   │                   │◀───────────────────│                    │
   │                   │   New Token Pair   │                    │
   │                   │                    │                    │
   │◀──────────────────│                    │                    │
   │  200 OK + Tokens  │                    │                    │
```

---

## Roles and Permissions Matrix

| Permission | User | Athlete | Coach | Admin |
|------------|:----:|:-------:|:-----:|:-----:|
| Register Account | ✓ | ✓ | ✓ | ✓ |
| Login | ✓ | ✓ | ✓ | ✓ |
| Refresh Token | ✓ | ✓ | ✓ | ✓ |
| Revoke Token | ✓ | ✓ | ✓ | ✓ |
| View Own Profile | ✓ | ✓ | ✓ | ✓ |
| View All Users | - | - | ✓ | ✓ |
| Manage Tenants | - | - | - | ✓ |
| Manage Roles | - | - | - | ✓ |
