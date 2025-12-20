# Athlete Management Feature

## Overview

The Athlete Management feature provides comprehensive functionality for tracking athlete profiles, weight history, and completed exercises in the NoDaysOff fitness application.

## Domain Model

### Athlete Aggregate

The `Athlete` aggregate extends `Profile` with fitness-specific tracking capabilities:

```
Athlete (extends Profile)
├── Name: string
├── Username: string
├── ImageUrl: string
├── CurrentWeight: int?
├── LastWeighedOn: DateTime?
├── Weights: ICollection<AthleteWeight>
└── CompletedExercises: ICollection<CompletedExercise>
```

### AthleteWeight Entity

Tracks weight history:

```
AthleteWeight
├── Id: int
├── WeightInKgs: int
├── WeighedOn: DateTime
├── CreatedOn: DateTime
├── CreatedBy: string
├── LastModifiedOn: DateTime
└── LastModifiedBy: string
```

### CompletedExercise Entity

Tracks completed workout exercises:

```
CompletedExercise
├── Id: int
├── ScheduledExerciseId: int
├── WeightInKgs: int
├── Reps: int
├── Sets: int
├── Distance: int
├── TimeInSeconds: int
├── CompletionDateTime: DateTime
└── [Audit fields]
```

## Features

### Weight Tracking

- Record daily weight measurements
- Automatic current weight calculation
- Weight history retrieval
- Weight change analysis over time periods

### Exercise Completion Tracking

- Record completed exercises with performance data
- Track reps, sets, weight, distance, and time
- Query exercises by date
- Performance history analysis

## Usage

### Creating an Athlete

```csharp
var athlete = Athlete.Create(
    tenantId: 1,
    name: "John Doe",
    username: "johndoe",
    createdBy: "system"
);
```

### Recording Weight

```csharp
athlete.RecordWeight(
    weightInKgs: 75,
    weighedOn: DateTime.UtcNow,
    recordedBy: "johndoe"
);

// Access current weight
int? currentWeight = athlete.CurrentWeight; // 75
DateTime? lastWeighed = athlete.LastWeighedOn;
```

### Recording Completed Exercise

```csharp
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

### Querying Weight History

```csharp
// Get last 10 weight records
var history = athlete.GetWeightHistory(count: 10);

// Calculate weight change over 30 days
int? weightChange = athlete.CalculateWeightChange(days: 30);
```

### Querying Completed Exercises

```csharp
// Get all exercises completed today
var todayExercises = athlete.GetCompletedExercisesByDate(DateTime.Today);
```

## Validation Rules

| Field | Validation |
|-------|------------|
| Name | Required, max 256 characters |
| Username | Required |
| WeightInKgs | Must be greater than 0 |
| Reps | Cannot be negative |
| Sets | Cannot be negative |

## Business Rules

1. **Weight Recording**: When a new weight is recorded with a date equal to or later than the last weighed date, `CurrentWeight` and `LastWeighedOn` are automatically updated.

2. **Weight History**: Weight history is always returned in descending order by weighed date.

3. **Exercise Tracking**: Completed exercises are linked to scheduled exercises for workout plan correlation.
