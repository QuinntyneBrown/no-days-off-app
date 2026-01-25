# Migration Roadmap: Event-Driven Microservices Architecture

## Executive Summary

This document outlines a comprehensive roadmap to migrate the No Days Off application from its current monolithic architecture to an event-driven microservices architecture with YARP API Gateway.

### Current State
- .NET 9.0 monolithic application with DDD/CQRS patterns
- 14 domain aggregates across a single API
- MediatR for command/query handling
- Message bus infrastructure (UDP/Redis) already in place
- Single SQL Server database with EF Core
- No authentication/authorization implemented

### Target State
- 8 independent microservices with dedicated databases
- Shared library for cross-cutting concerns
- YARP-based API Gateway for routing and aggregation
- Event-driven communication via message bus
- Identity service for authentication/authorization

---

## Target Architecture

```
                                    ┌─────────────────┐
                                    │   Angular UI    │
                                    └────────┬────────┘
                                             │
                                    ┌────────▼────────┐
                                    │  API Gateway    │
                                    │    (YARP)       │
                                    └────────┬────────┘
                                             │
        ┌────────────┬───────────┬──────────┼──────────┬───────────┬────────────┐
        │            │           │          │          │           │            │
   ┌────▼────┐ ┌────▼────┐ ┌────▼────┐ ┌───▼───┐ ┌───▼────┐ ┌────▼────┐ ┌─────▼─────┐
   │Identity │ │Athletes │ │Workouts │ │Exercises│ │Dashboard│ │  Media  │ │Communication│
   │ Service │ │ Service │ │ Service │ │ Service │ │ Service │ │ Service │ │  Service   │
   └────┬────┘ └────┬────┘ └────┬────┘ └───┬────┘ └────┬────┘ └────┬────┘ └─────┬─────┘
        │          │           │          │           │           │            │
        │          └───────────┴──────────┴───────────┴───────────┴────────────┘
        │                                     │
        │                          ┌──────────▼──────────┐
        │                          │    Message Bus      │
        │                          │  (Redis Pub/Sub)    │
        │                          └─────────────────────┘
        │
   ┌────▼────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐
   │Identity │ │Athletes │ │Workouts │ │Exercises│ │Dashboard│ │  Media  │ │Messages │
   │   DB    │ │   DB    │ │   DB    │ │   DB    │ │   DB    │ │   DB    │ │   DB    │
   └─────────┘ └─────────┘ └─────────┘ └─────────┘ └─────────┘ └─────────┘ └─────────┘
```

---

## Final Folder Structure

```
src/
├── ApiGateway/                          # YARP API Gateway
│   ├── ApiGateway.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   └── yarp.json                        # Route configuration
│
├── Shared/                              # Shared library
│   ├── Shared.csproj
│   ├── Messaging/
│   │   ├── IMessageBus.cs
│   │   ├── IMessage.cs
│   │   ├── MessageBusConfiguration.cs
│   │   ├── Redis/
│   │   │   └── RedisMessageBus.cs
│   │   └── Messages/
│   │       ├── Athletes/
│   │       │   ├── AthleteCreatedMessage.cs
│   │       │   ├── AthleteUpdatedMessage.cs
│   │       │   └── AthleteDeletedMessage.cs
│   │       ├── Workouts/
│   │       ├── Exercises/
│   │       ├── Dashboards/
│   │       └── Identity/
│   ├── Domain/
│   │   ├── Entity.cs
│   │   ├── AggregateRoot.cs
│   │   ├── ValueObject.cs
│   │   ├── DomainEvent.cs
│   │   └── Exceptions/
│   │       ├── DomainException.cs
│   │       └── ValidationException.cs
│   ├── Infrastructure/
│   │   ├── ServiceCollectionExtensions.cs
│   │   └── HealthChecks/
│   ├── Authentication/
│   │   ├── JwtConfiguration.cs
│   │   ├── AuthenticationExtensions.cs
│   │   └── CurrentUserService.cs
│   └── Contracts/
│       ├── Athletes/
│       │   └── AthleteDto.cs
│       ├── Workouts/
│       └── ...
│
├── Identity/                            # Identity Service
│   ├── Identity.Api/
│   │   ├── Identity.Api.csproj
│   │   ├── Program.cs
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── UsersController.cs
│   │   │   └── TenantsController.cs
│   │   └── appsettings.json
│   ├── Identity.Core/
│   │   ├── Identity.Core.csproj
│   │   ├── Aggregates/
│   │   │   ├── User/
│   │   │   ├── Tenant/
│   │   │   └── RefreshToken/
│   │   └── Features/
│   │       ├── Auth/
│   │       ├── Users/
│   │       └── Tenants/
│   └── Identity.Infrastructure/
│       ├── Identity.Infrastructure.csproj
│       ├── Data/
│       │   ├── IdentityDbContext.cs
│       │   └── Configurations/
│       └── Services/
│           └── TokenService.cs
│
├── Athletes/                            # Athletes Service
│   ├── Athletes.Api/
│   │   ├── Athletes.Api.csproj
│   │   ├── Program.cs
│   │   ├── Controllers/
│   │   │   ├── AthletesController.cs
│   │   │   └── ProfilesController.cs
│   │   └── appsettings.json
│   ├── Athletes.Core/
│   │   ├── Athletes.Core.csproj
│   │   ├── Aggregates/
│   │   │   ├── Athlete/
│   │   │   │   ├── Athlete.cs
│   │   │   │   ├── AthleteWeight.cs
│   │   │   │   └── CompletedExercise.cs
│   │   │   └── Profile/
│   │   │       └── Profile.cs
│   │   ├── ValueObjects/
│   │   │   └── Weight.cs
│   │   └── Features/
│   │       ├── Athletes/
│   │       │   ├── CreateAthlete/
│   │       │   ├── UpdateAthlete/
│   │       │   ├── DeleteAthlete/
│   │       │   ├── GetAthleteById/
│   │       │   └── GetAthletes/
│   │       └── Profiles/
│   └── Athletes.Infrastructure/
│       ├── Athletes.Infrastructure.csproj
│       ├── Data/
│       │   ├── AthletesDbContext.cs
│       │   └── Configurations/
│       └── EventHandlers/
│           └── WorkoutCompletedEventHandler.cs
│
├── Workouts/                            # Workouts Service
│   ├── Workouts.Api/
│   │   ├── Workouts.Api.csproj
│   │   ├── Program.cs
│   │   ├── Controllers/
│   │   │   ├── WorkoutsController.cs
│   │   │   ├── DaysController.cs
│   │   │   └── ScheduledExercisesController.cs
│   │   └── appsettings.json
│   ├── Workouts.Core/
│   │   ├── Workouts.Core.csproj
│   │   ├── Aggregates/
│   │   │   ├── Workout/
│   │   │   ├── Day/
│   │   │   └── ScheduledExercise/
│   │   ├── ValueObjects/
│   │   │   └── Duration.cs
│   │   └── Features/
│   └── Workouts.Infrastructure/
│       ├── Workouts.Infrastructure.csproj
│       ├── Data/
│       │   └── WorkoutsDbContext.cs
│       └── EventHandlers/
│
├── Exercises/                           # Exercises Service
│   ├── Exercises.Api/
│   │   ├── Exercises.Api.csproj
│   │   ├── Program.cs
│   │   ├── Controllers/
│   │   │   ├── ExercisesController.cs
│   │   │   └── BodyPartsController.cs
│   │   └── appsettings.json
│   ├── Exercises.Core/
│   │   ├── Exercises.Core.csproj
│   │   ├── Aggregates/
│   │   │   ├── Exercise/
│   │   │   └── BodyPart/
│   │   └── Features/
│   └── Exercises.Infrastructure/
│       ├── Exercises.Infrastructure.csproj
│       └── Data/
│           └── ExercisesDbContext.cs
│
├── Dashboard/                           # Dashboard Service
│   ├── Dashboard.Api/
│   │   ├── Dashboard.Api.csproj
│   │   ├── Program.cs
│   │   ├── Controllers/
│   │   │   ├── DashboardsController.cs
│   │   │   └── TilesController.cs
│   │   └── appsettings.json
│   ├── Dashboard.Core/
│   │   ├── Dashboard.Core.csproj
│   │   ├── Aggregates/
│   │   │   ├── Dashboard/
│   │   │   └── Tile/
│   │   └── Features/
│   └── Dashboard.Infrastructure/
│       ├── Dashboard.Infrastructure.csproj
│       └── Data/
│           └── DashboardDbContext.cs
│
├── Media/                               # Media Service
│   ├── Media.Api/
│   │   ├── Media.Api.csproj
│   │   ├── Program.cs
│   │   ├── Controllers/
│   │   │   ├── DigitalAssetsController.cs
│   │   │   └── VideosController.cs
│   │   └── appsettings.json
│   ├── Media.Core/
│   │   ├── Media.Core.csproj
│   │   ├── Aggregates/
│   │   │   ├── DigitalAsset/
│   │   │   └── Video/
│   │   └── Features/
│   └── Media.Infrastructure/
│       ├── Media.Infrastructure.csproj
│       └── Data/
│           └── MediaDbContext.cs
│
├── Communication/                       # Communication Service
│   ├── Communication.Api/
│   │   ├── Communication.Api.csproj
│   │   ├── Program.cs
│   │   ├── Controllers/
│   │   │   └── ConversationsController.cs
│   │   ├── Hubs/
│   │   │   └── ChatHub.cs
│   │   └── appsettings.json
│   ├── Communication.Core/
│   │   ├── Communication.Core.csproj
│   │   ├── Aggregates/
│   │   │   └── Conversation/
│   │   └── Features/
│   └── Communication.Infrastructure/
│       ├── Communication.Infrastructure.csproj
│       └── Data/
│           └── CommunicationDbContext.cs
│
└── Ui/                                  # Angular Frontend
    └── (existing Angular app)

tests/
├── Shared.Tests/
├── Identity.Tests/
├── Athletes.Tests/
├── Workouts.Tests/
├── Exercises.Tests/
├── Dashboard.Tests/
├── Media.Tests/
├── Communication.Tests/
└── Integration.Tests/
```

---

## Phase 1: Foundation & Shared Infrastructure

### 1.1 Create Shared Project Structure

| Item | Description | Priority |
|------|-------------|----------|
| 1.1.1 | Create `src/Shared/Shared.csproj` with .NET 9.0 target | High |
| 1.1.2 | Migrate `Entity.cs`, `AggregateRoot.cs`, `ValueObject.cs` from Core | High |
| 1.1.3 | Migrate `DomainEvent.cs` and exception classes from Core | High |
| 1.1.4 | Create `Shared/Messaging/` folder structure | High |
| 1.1.5 | Migrate `IMessageBus.cs` and `IMessage.cs` interfaces | High |
| 1.1.6 | Migrate `RedisMessageBus.cs` implementation | High |
| 1.1.7 | Remove `UdpMessageBus.cs` (dev-only, not needed in microservices) | Medium |
| 1.1.8 | Create `MessageBusConfiguration.cs` for Redis settings | High |
| 1.1.9 | Create `ServiceCollectionExtensions.cs` for DI registration | High |

### 1.2 Define Integration Messages

| Item | Description | Priority |
|------|-------------|----------|
| 1.2.1 | Create `Shared/Messaging/Messages/Athletes/AthleteCreatedMessage.cs` | High |
| 1.2.2 | Create `Shared/Messaging/Messages/Athletes/AthleteUpdatedMessage.cs` | High |
| 1.2.3 | Create `Shared/Messaging/Messages/Athletes/AthleteDeletedMessage.cs` | High |
| 1.2.4 | Create `Shared/Messaging/Messages/Workouts/WorkoutCreatedMessage.cs` | High |
| 1.2.5 | Create `Shared/Messaging/Messages/Workouts/WorkoutCompletedMessage.cs` | High |
| 1.2.6 | Create `Shared/Messaging/Messages/Exercises/ExerciseCreatedMessage.cs` | High |
| 1.2.7 | Create `Shared/Messaging/Messages/Exercises/ExerciseUpdatedMessage.cs` | High |
| 1.2.8 | Create `Shared/Messaging/Messages/Dashboards/DashboardCreatedMessage.cs` | Medium |
| 1.2.9 | Create `Shared/Messaging/Messages/Identity/UserCreatedMessage.cs` | High |
| 1.2.10 | Create `Shared/Messaging/Messages/Identity/UserAuthenticatedMessage.cs` | High |
| 1.2.11 | Create `Shared/Messaging/Messages/Identity/TenantCreatedMessage.cs` | Medium |

### 1.3 Shared Authentication Infrastructure

| Item | Description | Priority |
|------|-------------|----------|
| 1.3.1 | Create `Shared/Authentication/JwtConfiguration.cs` | High |
| 1.3.2 | Create `Shared/Authentication/AuthenticationExtensions.cs` | High |
| 1.3.3 | Create `Shared/Authentication/CurrentUserService.cs` | High |
| 1.3.4 | Create `Shared/Authentication/ICurrentUserService.cs` interface | High |
| 1.3.5 | Add JWT validation middleware configuration | High |

### 1.4 Shared Contracts (DTOs)

| Item | Description | Priority |
|------|-------------|----------|
| 1.4.1 | Create `Shared/Contracts/Athletes/AthleteDto.cs` | High |
| 1.4.2 | Create `Shared/Contracts/Athletes/ProfileDto.cs` | High |
| 1.4.3 | Create `Shared/Contracts/Workouts/WorkoutDto.cs` | High |
| 1.4.4 | Create `Shared/Contracts/Workouts/DayDto.cs` | High |
| 1.4.5 | Create `Shared/Contracts/Workouts/ScheduledExerciseDto.cs` | High |
| 1.4.6 | Create `Shared/Contracts/Exercises/ExerciseDto.cs` | High |
| 1.4.7 | Create `Shared/Contracts/Exercises/BodyPartDto.cs` | High |
| 1.4.8 | Create `Shared/Contracts/Dashboards/DashboardDto.cs` | Medium |
| 1.4.9 | Create `Shared/Contracts/Dashboards/TileDto.cs` | Medium |
| 1.4.10 | Create `Shared/Contracts/Media/DigitalAssetDto.cs` | Medium |
| 1.4.11 | Create `Shared/Contracts/Media/VideoDto.cs` | Medium |
| 1.4.12 | Create `Shared/Contracts/Communication/ConversationDto.cs` | Medium |
| 1.4.13 | Create `Shared/Contracts/Identity/UserDto.cs` | High |
| 1.4.14 | Create `Shared/Contracts/Identity/TenantDto.cs` | High |
| 1.4.15 | Create `Shared/Contracts/Identity/AuthResponseDto.cs` | High |

### 1.5 Health Checks & Observability

| Item | Description | Priority |
|------|-------------|----------|
| 1.5.1 | Create `Shared/Infrastructure/HealthChecks/RedisHealthCheck.cs` | Medium |
| 1.5.2 | Create `Shared/Infrastructure/HealthChecks/SqlServerHealthCheck.cs` | Medium |
| 1.5.3 | Add OpenTelemetry packages to Shared | Medium |
| 1.5.4 | Create `Shared/Infrastructure/Telemetry/TelemetryExtensions.cs` | Medium |
| 1.5.5 | Add Serilog structured logging configuration | Medium |

---

## Phase 2: API Gateway with YARP

### 2.1 Create API Gateway Project

| Item | Description | Priority |
|------|-------------|----------|
| 2.1.1 | Create `src/ApiGateway/ApiGateway.csproj` | High |
| 2.1.2 | Add `Yarp.ReverseProxy` NuGet package (latest version) | High |
| 2.1.3 | Add reference to `Shared` project | High |
| 2.1.4 | Create `Program.cs` with YARP configuration | High |
| 2.1.5 | Create `appsettings.json` with base configuration | High |
| 2.1.6 | Create `appsettings.Development.json` | High |
| 2.1.7 | Create `appsettings.Production.json` | Medium |

### 2.2 YARP Route Configuration

| Item | Description | Priority |
|------|-------------|----------|
| 2.2.1 | Create `yarp.json` route configuration file | High |
| 2.2.2 | Configure route for Identity service (`/api/auth/**`, `/api/users/**`) | High |
| 2.2.3 | Configure route for Athletes service (`/api/athletes/**`, `/api/profiles/**`) | High |
| 2.2.4 | Configure route for Workouts service (`/api/workouts/**`, `/api/days/**`, `/api/scheduled-exercises/**`) | High |
| 2.2.5 | Configure route for Exercises service (`/api/exercises/**`, `/api/body-parts/**`) | High |
| 2.2.6 | Configure route for Dashboard service (`/api/dashboards/**`, `/api/tiles/**`) | High |
| 2.2.7 | Configure route for Media service (`/api/digital-assets/**`, `/api/videos/**`) | High |
| 2.2.8 | Configure route for Communication service (`/api/conversations/**`) | High |
| 2.2.9 | Configure health check aggregation route (`/health`) | Medium |
| 2.2.10 | Configure Swagger aggregation (if needed) | Low |

### 2.3 Gateway Features

| Item | Description | Priority |
|------|-------------|----------|
| 2.3.1 | Implement JWT token validation at gateway level | High |
| 2.3.2 | Add rate limiting middleware | Medium |
| 2.3.3 | Add request/response logging middleware | Medium |
| 2.3.4 | Configure CORS for Angular frontend | High |
| 2.3.5 | Add request timeout policies per route | Medium |
| 2.3.6 | Configure load balancing for multiple service instances | Low |
| 2.3.7 | Add circuit breaker pattern for service failures | Medium |
| 2.3.8 | Implement API versioning support in routes | Low |

### 2.4 Gateway Security

| Item | Description | Priority |
|------|-------------|----------|
| 2.4.1 | Configure HTTPS/TLS termination | High |
| 2.4.2 | Add request validation middleware | Medium |
| 2.4.3 | Implement anti-forgery token handling | Medium |
| 2.4.4 | Add IP whitelisting configuration (optional) | Low |
| 2.4.5 | Configure security headers (HSTS, CSP, etc.) | Medium |

---

## Phase 3: Identity Service

### 3.1 Create Identity Service Projects

| Item | Description | Priority |
|------|-------------|----------|
| 3.1.1 | Create `src/Identity/Identity.Api/Identity.Api.csproj` | High |
| 3.1.2 | Create `src/Identity/Identity.Core/Identity.Core.csproj` | High |
| 3.1.3 | Create `src/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj` | High |
| 3.1.4 | Add project references (Api → Core, Api → Infrastructure, Infrastructure → Core) | High |
| 3.1.5 | Add reference to `Shared` project in all Identity projects | High |

### 3.2 Identity Domain Model

| Item | Description | Priority |
|------|-------------|----------|
| 3.2.1 | Create `User` aggregate root with properties (Id, Email, PasswordHash, FirstName, LastName, TenantId) | High |
| 3.2.2 | Create `RefreshToken` entity (Id, Token, UserId, ExpiresAt, CreatedAt, RevokedAt) | High |
| 3.2.3 | Migrate `Tenant` aggregate from existing Core project | High |
| 3.2.4 | Create `Role` entity (Id, Name, Permissions) | Medium |
| 3.2.5 | Create `UserRole` join entity | Medium |
| 3.2.6 | Create password hashing value object or service | High |

### 3.3 Identity Features (MediatR Handlers)

| Item | Description | Priority |
|------|-------------|----------|
| 3.3.1 | Create `RegisterUserCommand` and handler | High |
| 3.3.2 | Create `LoginCommand` and handler (returns JWT + refresh token) | High |
| 3.3.3 | Create `RefreshTokenCommand` and handler | High |
| 3.3.4 | Create `RevokeTokenCommand` and handler | Medium |
| 3.3.5 | Create `GetUserByIdQuery` and handler | High |
| 3.3.6 | Create `GetUsersQuery` and handler | Medium |
| 3.3.7 | Create `UpdateUserCommand` and handler | Medium |
| 3.3.8 | Create `ChangePasswordCommand` and handler | Medium |
| 3.3.9 | Create `ForgotPasswordCommand` and handler | Low |
| 3.3.10 | Create `ResetPasswordCommand` and handler | Low |
| 3.3.11 | Migrate `CreateTenantCommand` from existing Core | High |
| 3.3.12 | Migrate `UpdateTenantCommand` from existing Core | High |
| 3.3.13 | Migrate `GetTenantByIdQuery` from existing Core | High |
| 3.3.14 | Migrate `GetTenantsQuery` from existing Core | High |

### 3.4 Identity API Controllers

| Item | Description | Priority |
|------|-------------|----------|
| 3.4.1 | Create `AuthController` (Login, Register, RefreshToken, Logout) | High |
| 3.4.2 | Create `UsersController` (CRUD operations) | High |
| 3.4.3 | Migrate `TenantsController` from existing API | High |
| 3.4.4 | Add Swagger/OpenAPI documentation | Medium |

### 3.5 Identity Infrastructure

| Item | Description | Priority |
|------|-------------|----------|
| 3.5.1 | Create `IdentityDbContext` with User, Tenant, RefreshToken DbSets | High |
| 3.5.2 | Create EF Core configurations for User, Tenant, RefreshToken | High |
| 3.5.3 | Create initial migration | High |
| 3.5.4 | Create `TokenService` for JWT generation | High |
| 3.5.5 | Create `PasswordHasher` service | High |
| 3.5.6 | Register services in DI container | High |

### 3.6 Identity Event Publishing

| Item | Description | Priority |
|------|-------------|----------|
| 3.6.1 | Publish `UserCreatedMessage` on registration | High |
| 3.6.2 | Publish `UserAuthenticatedMessage` on login | Medium |
| 3.6.3 | Publish `TenantCreatedMessage` on tenant creation | Medium |
| 3.6.4 | Publish `UserUpdatedMessage` on profile update | Low |

---

## Phase 4: Athletes Service

### 4.1 Create Athletes Service Projects

| Item | Description | Priority |
|------|-------------|----------|
| 4.1.1 | Create `src/Athletes/Athletes.Api/Athletes.Api.csproj` | High |
| 4.1.2 | Create `src/Athletes/Athletes.Core/Athletes.Core.csproj` | High |
| 4.1.3 | Create `src/Athletes/Athletes.Infrastructure/Athletes.Infrastructure.csproj` | High |
| 4.1.4 | Add project references and Shared reference | High |

### 4.2 Migrate Athletes Domain

| Item | Description | Priority |
|------|-------------|----------|
| 4.2.1 | Move `Athlete` aggregate to Athletes.Core | High |
| 4.2.2 | Move `AthleteWeight` entity to Athletes.Core | High |
| 4.2.3 | Move `CompletedExercise` entity to Athletes.Core | High |
| 4.2.4 | Move `Profile` aggregate to Athletes.Core | High |
| 4.2.5 | Move `Weight` value object to Athletes.Core | High |
| 4.2.6 | Update namespace references | High |

### 4.3 Migrate Athletes Features

| Item | Description | Priority |
|------|-------------|----------|
| 4.3.1 | Move `CreateAthleteCommand` and handler | High |
| 4.3.2 | Move `UpdateAthleteCommand` and handler | High |
| 4.3.3 | Move `DeleteAthleteCommand` and handler | High |
| 4.3.4 | Move `GetAthleteByIdQuery` and handler | High |
| 4.3.5 | Move `GetAthletesQuery` and handler | High |
| 4.3.6 | Move Profile-related commands and queries | High |
| 4.3.7 | Add `RecordWeightCommand` and handler | Medium |
| 4.3.8 | Add `RecordCompletedExerciseCommand` and handler | Medium |
| 4.3.9 | Update handlers to use Athletes-specific DbContext | High |

### 4.4 Athletes API & Infrastructure

| Item | Description | Priority |
|------|-------------|----------|
| 4.4.1 | Create `AthletesController` | High |
| 4.4.2 | Create `ProfilesController` | High |
| 4.4.3 | Create `AthletesDbContext` | High |
| 4.4.4 | Create EF Core configurations | High |
| 4.4.5 | Create initial migration | High |
| 4.4.6 | Configure Program.cs with MediatR, EF Core, MessageBus | High |
| 4.4.7 | Add JWT authentication middleware | High |

### 4.5 Athletes Event Handling

| Item | Description | Priority |
|------|-------------|----------|
| 4.5.1 | Publish `AthleteCreatedMessage` on creation | High |
| 4.5.2 | Publish `AthleteUpdatedMessage` on update | High |
| 4.5.3 | Publish `AthleteDeletedMessage` on deletion | High |
| 4.5.4 | Subscribe to `WorkoutCompletedMessage` to update athlete stats | Medium |
| 4.5.5 | Subscribe to `UserCreatedMessage` to auto-create athlete profile | Medium |

---

## Phase 5: Workouts Service

### 5.1 Create Workouts Service Projects

| Item | Description | Priority |
|------|-------------|----------|
| 5.1.1 | Create `src/Workouts/Workouts.Api/Workouts.Api.csproj` | High |
| 5.1.2 | Create `src/Workouts/Workouts.Core/Workouts.Core.csproj` | High |
| 5.1.3 | Create `src/Workouts/Workouts.Infrastructure/Workouts.Infrastructure.csproj` | High |
| 5.1.4 | Add project references and Shared reference | High |

### 5.2 Migrate Workouts Domain

| Item | Description | Priority |
|------|-------------|----------|
| 5.2.1 | Move `Workout` aggregate to Workouts.Core | High |
| 5.2.2 | Move `Day` aggregate to Workouts.Core | High |
| 5.2.3 | Move `ScheduledExercise` aggregate to Workouts.Core | High |
| 5.2.4 | Move `ScheduledExerciseSet` entity to Workouts.Core | High |
| 5.2.5 | Move `Duration` value object to Workouts.Core | High |
| 5.2.6 | Update namespace references | High |

### 5.3 Migrate Workouts Features

| Item | Description | Priority |
|------|-------------|----------|
| 5.3.1 | Move all Workout commands and queries | High |
| 5.3.2 | Move all Day commands and queries | High |
| 5.3.3 | Move all ScheduledExercise commands and queries | High |
| 5.3.4 | Add `CompleteWorkoutCommand` and handler | High |
| 5.3.5 | Update handlers to use Workouts-specific DbContext | High |

### 5.4 Workouts API & Infrastructure

| Item | Description | Priority |
|------|-------------|----------|
| 5.4.1 | Create `WorkoutsController` | High |
| 5.4.2 | Create `DaysController` | High |
| 5.4.3 | Create `ScheduledExercisesController` | High |
| 5.4.4 | Create `WorkoutsDbContext` | High |
| 5.4.5 | Create EF Core configurations | High |
| 5.4.6 | Create initial migration | High |
| 5.4.7 | Configure Program.cs | High |

### 5.5 Workouts Event Handling

| Item | Description | Priority |
|------|-------------|----------|
| 5.5.1 | Publish `WorkoutCreatedMessage` on creation | High |
| 5.5.2 | Publish `WorkoutCompletedMessage` on completion | High |
| 5.5.3 | Subscribe to `ExerciseCreatedMessage` for exercise catalog sync | Medium |
| 5.5.4 | Store local copy of exercise names (eventual consistency) | Medium |

---

## Phase 6: Exercises Service

### 6.1 Create Exercises Service Projects

| Item | Description | Priority |
|------|-------------|----------|
| 6.1.1 | Create `src/Exercises/Exercises.Api/Exercises.Api.csproj` | High |
| 6.1.2 | Create `src/Exercises/Exercises.Core/Exercises.Core.csproj` | High |
| 6.1.3 | Create `src/Exercises/Exercises.Infrastructure/Exercises.Infrastructure.csproj` | High |
| 6.1.4 | Add project references and Shared reference | High |

### 6.2 Migrate Exercises Domain

| Item | Description | Priority |
|------|-------------|----------|
| 6.2.1 | Move `Exercise` aggregate to Exercises.Core | High |
| 6.2.2 | Move `ExerciseSet` entity to Exercises.Core | High |
| 6.2.3 | Move `BodyPart` aggregate to Exercises.Core | High |
| 6.2.4 | Update namespace references | High |

### 6.3 Migrate Exercises Features

| Item | Description | Priority |
|------|-------------|----------|
| 6.3.1 | Move all Exercise commands and queries | High |
| 6.3.2 | Move all BodyPart commands and queries | High |
| 6.3.3 | Update handlers to use Exercises-specific DbContext | High |

### 6.4 Exercises API & Infrastructure

| Item | Description | Priority |
|------|-------------|----------|
| 6.4.1 | Create `ExercisesController` | High |
| 6.4.2 | Create `BodyPartsController` | High |
| 6.4.3 | Create `ExercisesDbContext` | High |
| 6.4.4 | Create EF Core configurations | High |
| 6.4.5 | Create initial migration | High |
| 6.4.6 | Configure Program.cs | High |

### 6.5 Exercises Event Handling

| Item | Description | Priority |
|------|-------------|----------|
| 6.5.1 | Publish `ExerciseCreatedMessage` on creation | High |
| 6.5.2 | Publish `ExerciseUpdatedMessage` on update | High |
| 6.5.3 | Publish `ExerciseDeletedMessage` on deletion | High |
| 6.5.4 | Publish `BodyPartCreatedMessage` on creation | Medium |

---

## Phase 7: Dashboard Service

### 7.1 Create Dashboard Service Projects

| Item | Description | Priority |
|------|-------------|----------|
| 7.1.1 | Create `src/Dashboard/Dashboard.Api/Dashboard.Api.csproj` | High |
| 7.1.2 | Create `src/Dashboard/Dashboard.Core/Dashboard.Core.csproj` | High |
| 7.1.3 | Create `src/Dashboard/Dashboard.Infrastructure/Dashboard.Infrastructure.csproj` | High |
| 7.1.4 | Add project references and Shared reference | High |

### 7.2 Migrate Dashboard Domain

| Item | Description | Priority |
|------|-------------|----------|
| 7.2.1 | Move `Dashboard` aggregate to Dashboard.Core | High |
| 7.2.2 | Move `DashboardTile` entity to Dashboard.Core | High |
| 7.2.3 | Move `Tile` aggregate to Dashboard.Core | High |
| 7.2.4 | Update namespace references | High |

### 7.3 Migrate Dashboard Features

| Item | Description | Priority |
|------|-------------|----------|
| 7.3.1 | Move all Dashboard commands and queries | High |
| 7.3.2 | Move all Tile commands and queries | High |
| 7.3.3 | Update handlers to use Dashboard-specific DbContext | High |

### 7.4 Dashboard API & Infrastructure

| Item | Description | Priority |
|------|-------------|----------|
| 7.4.1 | Create `DashboardsController` | High |
| 7.4.2 | Create `TilesController` | High |
| 7.4.3 | Create `DashboardDbContext` | High |
| 7.4.4 | Create EF Core configurations | High |
| 7.4.5 | Create initial migration | High |
| 7.4.6 | Configure Program.cs | High |

### 7.5 Dashboard Event Handling

| Item | Description | Priority |
|------|-------------|----------|
| 7.5.1 | Subscribe to `UserCreatedMessage` to create default dashboard | Medium |
| 7.5.2 | Subscribe to `WorkoutCompletedMessage` to update workout tiles | Medium |
| 7.5.3 | Publish `DashboardCreatedMessage` on creation | Low |

---

## Phase 8: Media Service

### 8.1 Create Media Service Projects

| Item | Description | Priority |
|------|-------------|----------|
| 8.1.1 | Create `src/Media/Media.Api/Media.Api.csproj` | High |
| 8.1.2 | Create `src/Media/Media.Core/Media.Core.csproj` | High |
| 8.1.3 | Create `src/Media/Media.Infrastructure/Media.Infrastructure.csproj` | High |
| 8.1.4 | Add project references and Shared reference | High |

### 8.2 Migrate Media Domain

| Item | Description | Priority |
|------|-------------|----------|
| 8.2.1 | Move `DigitalAsset` aggregate to Media.Core | High |
| 8.2.2 | Move `Video` aggregate to Media.Core | High |
| 8.2.3 | Update namespace references | High |

### 8.3 Migrate Media Features

| Item | Description | Priority |
|------|-------------|----------|
| 8.3.1 | Move all DigitalAsset commands and queries | High |
| 8.3.2 | Move all Video commands and queries | High |
| 8.3.3 | Add file upload/download functionality | High |
| 8.3.4 | Update handlers to use Media-specific DbContext | High |

### 8.4 Media API & Infrastructure

| Item | Description | Priority |
|------|-------------|----------|
| 8.4.1 | Create `DigitalAssetsController` with file upload support | High |
| 8.4.2 | Create `VideosController` | High |
| 8.4.3 | Create `MediaDbContext` | High |
| 8.4.4 | Create EF Core configurations | High |
| 8.4.5 | Create initial migration | High |
| 8.4.6 | Configure Program.cs | High |
| 8.4.7 | Add blob storage integration (Azure Blob/AWS S3/local) | Medium |

### 8.5 Media Event Handling

| Item | Description | Priority |
|------|-------------|----------|
| 8.5.1 | Publish `DigitalAssetUploadedMessage` on upload | Medium |
| 8.5.2 | Publish `VideoUploadedMessage` on upload | Medium |
| 8.5.3 | Subscribe to `ExerciseCreatedMessage` to associate videos | Low |

---

## Phase 9: Communication Service

### 9.1 Create Communication Service Projects

| Item | Description | Priority |
|------|-------------|----------|
| 9.1.1 | Create `src/Communication/Communication.Api/Communication.Api.csproj` | High |
| 9.1.2 | Create `src/Communication/Communication.Core/Communication.Core.csproj` | High |
| 9.1.3 | Create `src/Communication/Communication.Infrastructure/Communication.Infrastructure.csproj` | High |
| 9.1.4 | Add project references and Shared reference | High |

### 9.2 Migrate Communication Domain

| Item | Description | Priority |
|------|-------------|----------|
| 9.2.1 | Move `Conversation` aggregate to Communication.Core | High |
| 9.2.2 | Move `Message` entity to Communication.Core | High |
| 9.2.3 | Update namespace references | High |

### 9.3 Migrate Communication Features

| Item | Description | Priority |
|------|-------------|----------|
| 9.3.1 | Move all Conversation commands and queries | High |
| 9.3.2 | Add `SendMessageCommand` and handler | High |
| 9.3.3 | Add `GetConversationMessagesQuery` and handler | High |
| 9.3.4 | Update handlers to use Communication-specific DbContext | High |

### 9.4 Communication API & Infrastructure

| Item | Description | Priority |
|------|-------------|----------|
| 9.4.1 | Create `ConversationsController` | High |
| 9.4.2 | Create `CommunicationDbContext` | High |
| 9.4.3 | Create EF Core configurations | High |
| 9.4.4 | Create initial migration | High |
| 9.4.5 | Configure Program.cs | High |

### 9.5 Real-time Communication (SignalR)

| Item | Description | Priority |
|------|-------------|----------|
| 9.5.1 | Add SignalR package | High |
| 9.5.2 | Create `ChatHub` for real-time messaging | High |
| 9.5.3 | Configure SignalR with Redis backplane for scaling | Medium |
| 9.5.4 | Update API Gateway to support WebSocket proxying | High |
| 9.5.5 | Add presence tracking (online/offline status) | Low |

### 9.6 Communication Event Handling

| Item | Description | Priority |
|------|-------------|----------|
| 9.6.1 | Publish `MessageSentMessage` for notifications | Medium |
| 9.6.2 | Subscribe to `UserCreatedMessage` to initialize user presence | Low |

---

## Phase 10: Data Migration & Cleanup

### 10.1 Database Separation

| Item | Description | Priority |
|------|-------------|----------|
| 10.1.1 | Create database migration scripts for splitting monolithic DB | High |
| 10.1.2 | Create `NoDaysOff.Identity` database | High |
| 10.1.3 | Create `NoDaysOff.Athletes` database | High |
| 10.1.4 | Create `NoDaysOff.Workouts` database | High |
| 10.1.5 | Create `NoDaysOff.Exercises` database | High |
| 10.1.6 | Create `NoDaysOff.Dashboard` database | High |
| 10.1.7 | Create `NoDaysOff.Media` database | High |
| 10.1.8 | Create `NoDaysOff.Communication` database | High |
| 10.1.9 | Create data migration tool/scripts for existing data | High |
| 10.1.10 | Verify data integrity post-migration | High |

### 10.2 Legacy Code Cleanup

| Item | Description | Priority |
|------|-------------|----------|
| 10.2.1 | Remove old `src/Api` project (or archive) | Medium |
| 10.2.2 | Remove old `src/Core` project (or archive) | Medium |
| 10.2.3 | Remove old `src/Infrastructure` project (or archive) | Medium |
| 10.2.4 | Update solution file with new project structure | High |
| 10.2.5 | Update any CI/CD pipelines | High |
| 10.2.6 | Update documentation | Medium |

### 10.3 Cross-Service Data Consistency

| Item | Description | Priority |
|------|-------------|----------|
| 10.3.1 | Implement Saga pattern for distributed transactions (if needed) | Low |
| 10.3.2 | Create compensating transactions for failure scenarios | Low |
| 10.3.3 | Implement outbox pattern for reliable event publishing | Medium |
| 10.3.4 | Add idempotency checks in event handlers | Medium |

---

## Phase 11: Testing Infrastructure

### 11.1 Unit Tests

| Item | Description | Priority |
|------|-------------|----------|
| 11.1.1 | Create `tests/Shared.Tests/` project | High |
| 11.1.2 | Create `tests/Identity.Tests/` project | High |
| 11.1.3 | Create `tests/Athletes.Tests/` project | High |
| 11.1.4 | Create `tests/Workouts.Tests/` project | High |
| 11.1.5 | Create `tests/Exercises.Tests/` project | High |
| 11.1.6 | Create `tests/Dashboard.Tests/` project | Medium |
| 11.1.7 | Create `tests/Media.Tests/` project | Medium |
| 11.1.8 | Create `tests/Communication.Tests/` project | Medium |
| 11.1.9 | Migrate existing tests to appropriate service test projects | High |

### 11.2 Integration Tests

| Item | Description | Priority |
|------|-------------|----------|
| 11.2.1 | Create `tests/Integration.Tests/` project | High |
| 11.2.2 | Add Testcontainers for SQL Server | High |
| 11.2.3 | Add Testcontainers for Redis | High |
| 11.2.4 | Create API Gateway integration tests | High |
| 11.2.5 | Create cross-service integration tests | High |
| 11.2.6 | Create message bus integration tests | Medium |

### 11.3 End-to-End Tests

| Item | Description | Priority |
|------|-------------|----------|
| 11.3.1 | Update existing Playwright tests for new architecture | High |
| 11.3.2 | Create Docker Compose for E2E test environment | High |
| 11.3.3 | Add API contract tests (Pact or similar) | Medium |

---

## Phase 12: DevOps & Deployment

### 12.1 Containerization

| Item | Description | Priority |
|------|-------------|----------|
| 12.1.1 | Create `Dockerfile` for API Gateway | High |
| 12.1.2 | Create `Dockerfile` for Identity service | High |
| 12.1.3 | Create `Dockerfile` for Athletes service | High |
| 12.1.4 | Create `Dockerfile` for Workouts service | High |
| 12.1.5 | Create `Dockerfile` for Exercises service | High |
| 12.1.6 | Create `Dockerfile` for Dashboard service | High |
| 12.1.7 | Create `Dockerfile` for Media service | High |
| 12.1.8 | Create `Dockerfile` for Communication service | High |
| 12.1.9 | Create `docker-compose.yml` for local development | High |
| 12.1.10 | Create `docker-compose.override.yml` for development settings | Medium |

### 12.2 Service Configuration

| Item | Description | Priority |
|------|-------------|----------|
| 12.2.1 | Externalize configuration to environment variables | High |
| 12.2.2 | Add support for configuration providers (Azure Key Vault, AWS Secrets Manager) | Medium |
| 12.2.3 | Create service discovery configuration | Medium |
| 12.2.4 | Configure health check endpoints for all services | High |

### 12.3 CI/CD Pipeline

| Item | Description | Priority |
|------|-------------|----------|
| 12.3.1 | Update GitHub Actions/Azure DevOps pipeline for multi-service build | High |
| 12.3.2 | Add parallel build jobs for services | Medium |
| 12.3.3 | Add container image publishing | High |
| 12.3.4 | Add automated testing in pipeline | High |
| 12.3.5 | Add deployment stages (dev, staging, production) | Medium |

### 12.4 Orchestration (Optional - Future)

| Item | Description | Priority |
|------|-------------|----------|
| 12.4.1 | Create Kubernetes manifests for each service | Low |
| 12.4.2 | Create Helm charts for deployment | Low |
| 12.4.3 | Configure Kubernetes Ingress for API Gateway | Low |
| 12.4.4 | Set up service mesh (Istio/Linkerd) for observability | Low |

---

## Phase 13: Frontend Updates

### 13.1 Angular Application Updates

| Item | Description | Priority |
|------|-------------|----------|
| 13.1.1 | Update API base URL to point to API Gateway | High |
| 13.1.2 | Implement JWT token handling and refresh logic | High |
| 13.1.3 | Add authentication service with login/logout | High |
| 13.1.4 | Add HTTP interceptor for token attachment | High |
| 13.1.5 | Add HTTP interceptor for token refresh on 401 | High |
| 13.1.6 | Update environment configuration | High |
| 13.1.7 | Add SignalR client for real-time messaging | Medium |

---

## Service Port Allocation

| Service | Development Port | Description |
|---------|-----------------|-------------|
| API Gateway | 5000 | Main entry point |
| Identity Service | 5001 | Authentication & users |
| Athletes Service | 5002 | Athletes & profiles |
| Workouts Service | 5003 | Workouts, days, scheduled exercises |
| Exercises Service | 5004 | Exercises & body parts |
| Dashboard Service | 5005 | Dashboards & tiles |
| Media Service | 5006 | Digital assets & videos |
| Communication Service | 5007 | Conversations & real-time chat |
| Angular UI | 4200 | Frontend application |

---

## Message Topics

| Topic | Publisher | Subscribers | Description |
|-------|-----------|-------------|-------------|
| `identity.user.created` | Identity | Athletes, Dashboard | New user registered |
| `identity.user.authenticated` | Identity | - | User logged in (audit) |
| `identity.tenant.created` | Identity | All services | New tenant created |
| `athletes.athlete.created` | Athletes | Dashboard | New athlete profile created |
| `athletes.athlete.updated` | Athletes | Dashboard | Athlete profile updated |
| `athletes.athlete.deleted` | Athletes | Dashboard | Athlete profile deleted |
| `workouts.workout.created` | Workouts | Dashboard | New workout created |
| `workouts.workout.completed` | Workouts | Athletes, Dashboard | Workout completed |
| `exercises.exercise.created` | Exercises | Workouts | New exercise added |
| `exercises.exercise.updated` | Exercises | Workouts | Exercise updated |
| `exercises.exercise.deleted` | Exercises | Workouts | Exercise deleted |
| `media.asset.uploaded` | Media | Exercises | New asset uploaded |
| `communication.message.sent` | Communication | - | Message sent (notifications) |

---

## Migration Phases Summary

| Phase | Description | Estimated Items |
|-------|-------------|-----------------|
| Phase 1 | Foundation & Shared Infrastructure | 46 items |
| Phase 2 | API Gateway with YARP | 22 items |
| Phase 3 | Identity Service | 35 items |
| Phase 4 | Athletes Service | 24 items |
| Phase 5 | Workouts Service | 22 items |
| Phase 6 | Exercises Service | 18 items |
| Phase 7 | Dashboard Service | 17 items |
| Phase 8 | Media Service | 19 items |
| Phase 9 | Communication Service | 21 items |
| Phase 10 | Data Migration & Cleanup | 14 items |
| Phase 11 | Testing Infrastructure | 14 items |
| Phase 12 | DevOps & Deployment | 19 items |
| Phase 13 | Frontend Updates | 7 items |
| **Total** | | **258 items** |

---

## Dependencies Between Phases

```
Phase 1 (Shared) ──────┬──────────────────────────────────────────────────┐
                       │                                                  │
                       ▼                                                  │
Phase 2 (Gateway) ─────┤                                                  │
                       │                                                  │
                       ▼                                                  │
Phase 3 (Identity) ────┼─────────────────────────────────────────────────┤
                       │                                                  │
        ┌──────────────┼──────────────┬──────────────┬──────────────┐    │
        ▼              ▼              ▼              ▼              ▼    │
   Phase 4        Phase 5        Phase 6        Phase 7        Phase 8   │
   (Athletes)     (Workouts)     (Exercises)    (Dashboard)    (Media)   │
        │              │              │              │              │    │
        └──────────────┴──────────────┴──────────────┴──────────────┘    │
                                      │                                  │
                                      ▼                                  │
                               Phase 9 (Communication)                   │
                                      │                                  │
                                      ▼                                  │
                               Phase 10 (Data Migration) ◄───────────────┘
                                      │
                                      ▼
                               Phase 11 (Testing)
                                      │
                                      ▼
                               Phase 12 (DevOps)
                                      │
                                      ▼
                               Phase 13 (Frontend)
```

---

## Risk Mitigation

| Risk | Mitigation Strategy |
|------|---------------------|
| Data consistency across services | Implement outbox pattern, eventual consistency, idempotent handlers |
| Service communication failures | Circuit breaker pattern, retry policies, dead letter queues |
| Authentication complexity | Centralized Identity service, JWT validation at gateway |
| Database migration data loss | Comprehensive backup strategy, staged migration, rollback plan |
| Increased operational complexity | Centralized logging, distributed tracing, health dashboards |
| Performance degradation | Service-level caching, connection pooling, async messaging |

---

## Success Criteria

- [ ] All 8 microservices deployed and operational
- [ ] API Gateway routing all requests correctly
- [ ] Event-driven communication functioning between services
- [ ] Authentication working end-to-end
- [ ] All existing functionality preserved
- [ ] Integration tests passing
- [ ] E2E tests passing
- [ ] Performance metrics within acceptable thresholds
- [ ] Documentation updated
- [ ] CI/CD pipeline operational

---

## Notes

1. **Parallel Development**: Phases 4-8 can be developed in parallel after Phases 1-3 are complete
2. **Feature Flags**: Consider using feature flags to gradually roll out microservices
3. **Strangler Fig Pattern**: Legacy API can remain operational while services are migrated
4. **Database Per Service**: Each service has its own database for true isolation
5. **Eventual Consistency**: Accept that data may be eventually consistent across services
6. **Versioning**: Plan for API versioning from the start

---

*Document Version: 1.0*
*Last Updated: January 2026*
