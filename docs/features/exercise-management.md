# Exercise Management Feature

## Overview

The Exercise Management feature provides functionality for defining exercises, creating exercise templates with default configurations, and scheduling exercises within workout plans.

## Domain Model

### Exercise Aggregate

```
Exercise
├── Name: string (max 256 chars)
├── BodyPartId: int?
├── DefaultSets: int
├── DefaultRepetitions: int
├── DefaultSetCollection: ICollection<ExerciseSet>
└── DigitalAssetIds: ICollection<int>
```

### ExerciseSet Entity

Defines default set configuration:

```
ExerciseSet
├── Id: int
├── WeightInKgs: int
└── Repetitions: int
```

### ScheduledExercise Aggregate

```
ScheduledExercise
├── Name: string (max 256 chars)
├── DayId: int?
├── ExerciseId: int?
├── Sort: int
├── Repetitions: int
├── WeightInKgs: int
├── Sets: int
├── Distance: int (for cardio)
├── TimeInSeconds: int (for cardio)
└── SetCollection: ICollection<ScheduledExerciseSet>
```

## Features

### Exercise Definition

- Create exercises with default configurations
- Associate exercises with body parts
- Attach digital assets (images, videos)
- Define default set templates

### Scheduled Exercises

- Schedule exercises for specific days
- Define performance targets (reps, sets, weight)
- Support cardio exercises (distance, time)
- Order exercises within a day

## Usage

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

// Add default set templates
exercise.AddDefaultSet(weightInKgs: 50, repetitions: 10, modifiedBy: "admin");
exercise.AddDefaultSet(weightInKgs: 55, repetitions: 8, modifiedBy: "admin");
exercise.AddDefaultSet(weightInKgs: 60, repetitions: 6, modifiedBy: "admin");
```

### Attaching Digital Assets

```csharp
// Attach tutorial images or videos
exercise.AddDigitalAsset(digitalAssetId: 1, modifiedBy: "admin");
exercise.AddDigitalAsset(digitalAssetId: 2, modifiedBy: "admin");

// Remove asset
exercise.RemoveDigitalAsset(digitalAssetId: 1, modifiedBy: "admin");
```

### Scheduling an Exercise

```csharp
var scheduledExercise = ScheduledExercise.Create(
    tenantId: 1,
    name: "Morning Bench Press",
    dayId: 1, // Push Day
    exerciseId: 1, // Bench Press
    sort: 0,
    createdBy: "admin"
);

// Set performance targets for strength training
scheduledExercise.UpdatePerformanceTargets(
    repetitions: 10,
    sets: 3,
    weightInKgs: 50,
    modifiedBy: "admin"
);

// Or set targets for cardio
scheduledExercise.UpdateCardioTargets(
    distance: 5000, // meters
    timeInSeconds: 1800, // 30 minutes
    modifiedBy: "admin"
);
```

### Adding Sets to Scheduled Exercise

```csharp
scheduledExercise.AddSet(weightInKgs: 50, repetitions: 10, modifiedBy: "admin");
scheduledExercise.AddSet(weightInKgs: 55, repetitions: 8, modifiedBy: "admin");
scheduledExercise.AddSet(weightInKgs: 60, repetitions: 6, modifiedBy: "admin");
```

## Validation Rules

| Entity | Field | Validation |
|--------|-------|------------|
| Exercise | Name | Required, max 256 characters |
| Exercise | DefaultSets | Cannot be negative |
| Exercise | DefaultRepetitions | Cannot be negative |
| ExerciseSet | WeightInKgs | Cannot be negative |
| ExerciseSet | Repetitions | Cannot be negative |
| ScheduledExercise | Name | Required, max 256 characters |
| ScheduledExercise | Repetitions | Cannot be negative |
| ScheduledExercise | Sets | Cannot be negative |
| ScheduledExercise | WeightInKgs | Cannot be negative |
| ScheduledExercise | Distance | Cannot be negative |
| ScheduledExercise | TimeInSeconds | Cannot be negative |

## Relationships

```
BodyPart (1) --- (*) Exercise
Exercise (1) --- (*) ExerciseSet
Exercise (1) --- (*) ScheduledExercise
Day (1) --- (*) ScheduledExercise
ScheduledExercise (1) --- (*) ScheduledExerciseSet
Exercise (*) --- (*) DigitalAsset
```
