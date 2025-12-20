# NoDaysOff Core Domain

## Overview

The `NoDaysOff.Core` project contains the core domain model for the NoDaysOff fitness application. It follows Domain-Driven Design (DDD) principles with aggregate roots, entities, value objects, and domain events.

## Architecture

### Project Structure

```
src/NoDaysOff.Core/
├── Abstractions/           # Base interfaces and abstract classes
│   ├── Entity.cs
│   ├── AggregateRoot.cs
│   ├── IEntity.cs
│   ├── IAggregateRoot.cs
│   ├── IAuditableEntity.cs
│   ├── ISoftDeletable.cs
│   └── ITenantEntity.cs
├── Aggregates/             # Domain aggregates
│   ├── AthleteAggregate/
│   ├── BodyPartAggregate/
│   ├── ConversationAggregate/
│   ├── DashboardAggregate/
│   ├── DayAggregate/
│   ├── DigitalAssetAggregate/
│   ├── ExerciseAggregate/
│   ├── ProfileAggregate/
│   ├── ScheduledExerciseAggregate/
│   ├── TenantAggregate/
│   ├── TileAggregate/
│   ├── VideoAggregate/
│   └── WorkoutAggregate/
├── Events/                 # Domain events
│   ├── IDomainEvent.cs
│   └── DomainEvent.cs
├── Exceptions/             # Domain exceptions
│   ├── DomainException.cs
│   └── ValidationException.cs
└── ValueObjects/           # Value objects
    ├── ValueObject.cs
    ├── Weight.cs
    └── Duration.cs
```

## Core Concepts

### Aggregate Roots

All aggregate roots inherit from `AggregateRoot` which provides:
- Identity (Id)
- Audit trail (CreatedOn, CreatedBy, LastModifiedOn, LastModifiedBy)
- Soft delete support (IsDeleted)
- Multi-tenancy support (TenantId)
- Domain events collection

### Key Aggregates

| Aggregate | Description |
|-----------|-------------|
| **Athlete** | Extends Profile with fitness tracking (weight history, completed exercises) |
| **Exercise** | Exercise definitions with default sets and repetitions |
| **ScheduledExercise** | Scheduled exercises within a workout plan |
| **Workout** | Workout session with body parts |
| **Day** | Workout day configuration (e.g., "Push Day", "Leg Day") |
| **BodyPart** | Body part definitions (e.g., Chest, Legs, Back) |
| **Dashboard** | User dashboard with configurable tiles |
| **Video** | Video content for exercise tutorials |
| **Conversation** | Messaging between users |
| **DigitalAsset** | File and media asset management |
| **Tenant** | Multi-tenancy support |

## Usage Examples

### Creating an Athlete

```csharp
var athlete = Athlete.Create(
    tenantId: 1,
    name: "John Doe",
    username: "johndoe",
    createdBy: "system"
);

// Record weight
athlete.RecordWeight(75, DateTime.UtcNow, "johndoe");

// Record completed exercise
athlete.RecordCompletedExercise(
    scheduledExerciseId: 1,
    weightInKgs: 50,
    reps: 10,
    sets: 3,
    distance: 0,
    timeInSeconds: 0,
    completionDateTime: DateTime.UtcNow,
    recordedBy: "johndoe"
);
```

### Creating an Exercise

```csharp
var exercise = Exercise.Create(
    tenantId: 1,
    name: "Bench Press",
    bodyPartId: 1, // Chest
    defaultSets: 3,
    defaultRepetitions: 10,
    createdBy: "admin"
);

// Add default sets
exercise.AddDefaultSet(weightInKgs: 50, repetitions: 10, modifiedBy: "admin");
exercise.AddDefaultSet(weightInKgs: 55, repetitions: 8, modifiedBy: "admin");
```

### Creating a Scheduled Exercise

```csharp
var scheduledExercise = ScheduledExercise.Create(
    tenantId: 1,
    name: "Morning Bench Press",
    dayId: 1, // Monday
    exerciseId: 1, // Bench Press
    sort: 0,
    createdBy: "admin"
);

scheduledExercise.UpdatePerformanceTargets(
    repetitions: 10,
    sets: 3,
    weightInKgs: 50,
    modifiedBy: "admin"
);
```

## Value Objects

### Weight

Represents weight in kilograms with conversion support:

```csharp
var weight = Weight.FromKilograms(75);
double pounds = weight.ToPounds(); // 165.35
```

### Duration

Represents time duration in seconds:

```csharp
var duration = Duration.FromMinutes(30);
int seconds = duration.Seconds; // 1800
TimeSpan timeSpan = duration.ToTimeSpan();
```

## Validation

All aggregates include validation to ensure domain invariants:

- Required field validation
- String length validation
- Numeric range validation
- Business rule validation

Validation failures throw `ValidationException` with descriptive error messages.

## Multi-Tenancy

All aggregates support multi-tenancy through the `TenantId` property. This allows data isolation between different tenants in a shared database.

## Soft Delete

All aggregates support soft delete through the `ISoftDeletable` interface:

```csharp
entity.Delete();    // Marks as deleted
entity.Restore();   // Restores deleted entity
```

## Testing

The `NoDaysOff.Core.Tests` project contains comprehensive unit tests for all aggregates and value objects using xUnit and FluentAssertions.
