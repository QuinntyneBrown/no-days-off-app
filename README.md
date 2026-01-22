<p align="center">
  <img src="assets/logo.svg" alt="No Days Off App Logo" width="200"/>
</p>

# No Days Off App

A comprehensive fitness management application built with modern .NET technologies and Domain-Driven Design principles.

## Technology Stack

- **Backend**: ASP.NET Core (.NET 9.0)
- **Database**: SQL Server with Entity Framework Core 9.0
- **Architecture**: Domain-Driven Design (DDD) with CQRS pattern using MediatR
- **Messaging**: MessagePack serialization with UDP (dev) and Redis Pub/Sub (production)
- **API Documentation**: Swagger/OpenAPI
- **Testing**: xUnit with FluentAssertions and code coverage

## Project Structure

The solution follows a microservices architecture with domain-driven design principles:

### Microservices

- **[NoDaysOff.Services.Athletes](src/NoDaysOff.Services.Athletes/)** - Athlete management and profiles microservice
- **[NoDaysOff.Services.Workouts](src/NoDaysOff.Services.Workouts/)** - Workout planning and scheduling microservice
- **[NoDaysOff.Services.Exercises](src/NoDaysOff.Services.Exercises/)** - Exercise definitions and body parts microservice
- **[NoDaysOff.Services.Dashboards](src/NoDaysOff.Services.Dashboards/)** - User dashboards and widgets microservice
- **[NoDaysOff.Services.Media](src/NoDaysOff.Services.Media/)** - Video content and digital assets microservice
- **[NoDaysOff.Services.Communication](src/NoDaysOff.Services.Communication/)** - User conversations and messaging microservice

### Core Projects

- **[NoDaysOff.Core](src/NoDaysOff.Core/)** - Domain layer containing business logic, aggregates, value objects, and domain events
- **[NoDaysOff.Infrastructure](src/NoDaysOff.Infrastructure/)** - Data access layer with Entity Framework Core DbContext and configurations
- **[NoDaysOff.Shared](src/NoDaysOff.Shared/)** - Shared messaging infrastructure for microservices communication
- **[NoDaysOff.Api](src/NoDaysOff.Api/)** - Legacy monolithic API (being migrated to microservices)

### Test Projects

- **[NoDaysOff.Core.Tests](tests/NoDaysOff.Core.Tests/)** - Unit tests for domain models
- **[NoDaysOff.Api.Tests](tests/NoDaysOff.Api.Tests/)** - API integration tests
- **[NoDaysOff.Infrastructure.Tests](tests/NoDaysOff.Infrastructure.Tests/)** - Data access tests

## Features

The application implements 14 bounded contexts for comprehensive fitness management:

1. **Athlete Management** - User profiles with fitness tracking and progress monitoring
2. **Exercise Management** - Exercise definitions with sets, reps, and weight configurations
3. **Workout Management** - Complete workout sessions and tracking
4. **Scheduled Exercises** - Exercise planning within workout routines
5. **Day Management** - Workout day organization (Push Day, Pull Day, Leg Day, etc.)
6. **Body Part Management** - Body part categorization (Chest, Back, Legs, Arms, etc.)
7. **Dashboard Management** - Customizable user dashboards
8. **Tile Management** - Dashboard widget configuration
9. **Video Content** - Exercise tutorial and instructional videos
10. **Conversations** - User messaging and communication
11. **Digital Assets** - File and media management
12. **Profile Management** - User profile administration
13. **Tenant Management** - Multi-tenancy support
14. **Authentication & Authorization** (planned)

## Key Architectural Features

- **Microservices Architecture** - 6 independent services with separate databases and independent deployment
- **Event-Driven Communication** - MessagePack-based messaging with UDP (dev) and Redis Pub/Sub (production)
- **Database Per Service** - Each microservice has its own isolated database for true independence
- **Docker Support** - Docker Compose orchestration for running all services together
- **Multi-tenancy Support** - Built-in tenant isolation for SaaS deployment
- **Soft Delete** - All entities support soft deletion with IsDeleted flag
- **Audit Tracking** - Automatic tracking of creation and modification timestamps with user attribution
- **Domain Events** - Event-driven architecture for cross-aggregate communication
- **Value Objects** - Rich domain modeling with custom value types (Weight, Duration, etc.)
- **CQRS Pattern** - Command and Query separation using MediatR
- **No Repository Pattern** - Direct DbContext interface for simplified data access
- **Message Bus** - UDP for local development, Redis Pub/Sub for production

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB, Express, or full version)
- Visual Studio 2022 or VS Code
- (Optional) Docker Desktop for containerized deployment

### Running the Microservices

#### Option 1: Docker Compose (Recommended)

```bash
# Build and start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down
```

Access services at:
- Athletes: http://localhost:5100/swagger
- Workouts: http://localhost:5101/swagger
- Exercises: http://localhost:5102/swagger
- Dashboards: http://localhost:5103/swagger
- Media: http://localhost:5104/swagger
- Communication: http://localhost:5105/swagger

#### Option 2: Run Individual Services Locally

```bash
# Athletes Service
dotnet run --project src/NoDaysOff.Services.Athletes

# Workouts Service
dotnet run --project src/NoDaysOff.Services.Workouts

# And so on...
```

#### Option 3: Legacy Monolithic API

```bash
dotnet run --project src/NoDaysOff.Api
```

Navigate to `https://localhost:<port>/swagger` to explore the API

### Running Tests

```bash
dotnet test
```

## Documentation

Comprehensive documentation is available in the [docs](docs/) directory:

- **[deployment.md](docs/deployment.md)** - Microservices deployment guide with Docker and local setup
- **[microservices-architecture.md](docs/microservices-architecture.md)** - Microservices patterns and messaging infrastructure
- **[system.md](docs/system.md)** - Technical specifications and coding standards
- **[core-domain.md](docs/core-domain.md)** - DDD architecture and domain model
- **[athlete-management.md](docs/athlete-management.md)** - Athlete features
- **[exercise-management.md](docs/exercise-management.md)** - Exercise definitions
- **[workout-scheduling.md](docs/workout-scheduling.md)** - Workout planning
- **[dashboard-management.md](docs/dashboard-management.md)** - Dashboard configuration
- **[messaging.md](docs/messaging.md)** - User communication
- **[video-content.md](docs/video-content.md)** - Video management

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!
