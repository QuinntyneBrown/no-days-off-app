<p align="center">
  <img src="assets/logo.svg" alt="No Days Off App Logo" width="200"/>
</p>

# No Days Off App

A comprehensive fitness management application built with modern .NET technologies and Domain-Driven Design principles.

## Technology Stack

### Backend
- **Framework**: ASP.NET Core (.NET 9.0)
- **Database**: SQL Server with Entity Framework Core 9.0
- **Architecture**: Domain-Driven Design (DDD) with CQRS pattern using MediatR
- **Messaging**: MessagePack serialization with UDP (dev) and Redis Pub/Sub (production)
- **API Documentation**: Swagger/OpenAPI
- **Testing**: xUnit with FluentAssertions and code coverage

### Frontend
- **Framework**: Angular 21.0
- **UI Components**: Angular Material 21.0
- **State Management**: RxJS 7.8
- **Testing**: Jest 30.2 with jest-preset-angular 16.0
- **E2E Testing**: Playwright 1.57
- **Component Development**: Storybook 8.6
- **Documentation**: Compodoc 1.1

## Project Structure

The solution follows a clean, layered architecture with microservices-ready infrastructure:

### Backend Projects

- **[NoDaysOff.Core](src/NoDaysOff.Core/)** - Domain layer containing business logic, aggregates, value objects, and domain events
- **[NoDaysOff.Infrastructure](src/NoDaysOff.Infrastructure/)** - Data access layer with Entity Framework Core DbContext and configurations
- **[NoDaysOff.Api](src/NoDaysOff.Api/)** - REST API layer with controllers, MediatR commands/queries, and DTOs
- **[NoDaysOff.Shared](src/NoDaysOff.Shared/)** - Shared messaging infrastructure for microservices communication

### Frontend Project

- **[NoDaysOff.WebApp](src/NoDaysOff.WebApp/)** - Angular 21 single-page application with Material Design components, Storybook for component development, and comprehensive test coverage

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

- **Microservices-Ready** - Event-driven messaging infrastructure with MessagePack serialization
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
- Node.js 18+ and npm 10+
- SQL Server (LocalDB, Express, or full version)
- Visual Studio 2022 or VS Code

### Running the Backend API

1. Clone the repository
2. Update the connection string in [src/NoDaysOff.Api/appsettings.Development.json](src/NoDaysOff.Api/appsettings.Development.json)
3. Run database migrations (if configured)
4. Start the API project:
   ```bash
   dotnet run --project src/NoDaysOff.Api
   ```
5. Navigate to `https://localhost:<port>/swagger` to explore the API

### Running the Frontend Application

1. Navigate to the WebApp directory:
   ```bash
   cd src/NoDaysOff.WebApp
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   npm start
   ```
4. Navigate to `http://localhost:4200/` to view the application

### Running Tests

**Backend Tests:**
```bash
dotnet test
```

**Frontend Tests:**
```bash
cd src/NoDaysOff.WebApp
npm test                # Run Jest unit tests
npm run test:coverage   # Run tests with coverage
npm run e2e            # Run Playwright E2E tests
```

### Component Development with Storybook

View and develop UI components in isolation:
```bash
cd src/NoDaysOff.WebApp
npm run storybook
```

Navigate to `http://localhost:6006/` to explore the component library.

## Documentation

Comprehensive documentation is available in the [docs](docs/) directory:

### Architecture & Core
- **[system.md](docs/specs/system.md)** - Technical specifications and coding standards
- **[microservices-architecture.md](docs/microservices-architecture.md)** - Microservices patterns and messaging infrastructure
- **[core-domain.md](docs/features/core-domain.md)** - DDD architecture and domain model
- **[IMPLEMENTATION_SUMMARY.md](docs/IMPLEMENTATION_SUMMARY.md)** - Microservices refactoring summary

### Feature Documentation
- **[athlete-management.md](docs/features/athlete-management.md)** - Athlete features
- **[exercise-management.md](docs/features/exercise-management.md)** - Exercise definitions
- **[workout-scheduling.md](docs/features/workout-scheduling.md)** - Workout planning
- **[dashboard-management.md](docs/features/dashboard-management.md)** - Dashboard configuration
- **[messaging.md](docs/features/messaging.md)** - User communication
- **[video-content.md](docs/features/video-content.md)** - Video management

### Frontend Documentation
For detailed frontend documentation, see the [WebApp README](src/NoDaysOff.WebApp/README.md).

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!
