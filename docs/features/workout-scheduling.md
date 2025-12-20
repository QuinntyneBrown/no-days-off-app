# Workout Scheduling Feature

## Overview

The Workout Scheduling feature allows users to organize their training week by defining workout days, assigning body parts to days, and creating structured workout plans.

## Domain Model

### Day Aggregate

```
Day
├── Name: string (max 256 chars)
└── BodyPartIds: ICollection<int>
```

### BodyPart Aggregate

```
BodyPart
└── Name: string (max 256 chars)
```

### Workout Aggregate

```
Workout
└── BodyPartIds: ICollection<int>
```

## Features

### Day Management

- Create workout days (e.g., "Push Day", "Pull Day", "Leg Day")
- Assign body parts to days
- Flexible day naming for different training splits

### Body Part Management

- Define body parts (e.g., Chest, Back, Legs, Shoulders)
- Associate exercises with body parts
- Track which body parts are worked on which days

### Workout Sessions

- Create workout instances
- Associate body parts with workouts
- Track workout composition

## Usage

### Creating a Body Part

```csharp
var chest = BodyPart.Create(
    tenantId: 1,
    name: "Chest",
    createdBy: "admin"
);

var back = BodyPart.Create(
    tenantId: 1,
    name: "Back",
    createdBy: "admin"
);
```

### Creating a Workout Day

```csharp
var pushDay = Day.Create(
    tenantId: 1,
    name: "Push Day",
    createdBy: "admin"
);

// Assign body parts to the day
pushDay.AssignBodyPart(chestId, modifiedBy: "admin");
pushDay.AssignBodyPart(shouldersId, modifiedBy: "admin");
pushDay.AssignBodyPart(tricepsId, modifiedBy: "admin");
```

### Creating a Workout

```csharp
var workout = Workout.Create(
    tenantId: 1,
    createdBy: "system"
);

// Add body parts to the workout
workout.AddBodyPart(chestId, modifiedBy: "admin");
workout.AddBodyPart(shouldersId, modifiedBy: "admin");

// Check if workout includes a body part
bool hasChest = workout.HasBodyPart(chestId); // true
```

## Example: Push/Pull/Legs Split

```csharp
// Create body parts
var chest = BodyPart.Create(1, "Chest", "admin");
var shoulders = BodyPart.Create(1, "Shoulders", "admin");
var triceps = BodyPart.Create(1, "Triceps", "admin");
var back = BodyPart.Create(1, "Back", "admin");
var biceps = BodyPart.Create(1, "Biceps", "admin");
var quads = BodyPart.Create(1, "Quadriceps", "admin");
var hamstrings = BodyPart.Create(1, "Hamstrings", "admin");
var calves = BodyPart.Create(1, "Calves", "admin");

// Create days
var pushDay = Day.Create(1, "Push Day", "admin");
pushDay.AssignBodyPart(chestId, "admin");
pushDay.AssignBodyPart(shouldersId, "admin");
pushDay.AssignBodyPart(tricepsId, "admin");

var pullDay = Day.Create(1, "Pull Day", "admin");
pullDay.AssignBodyPart(backId, "admin");
pullDay.AssignBodyPart(bicepsId, "admin");

var legDay = Day.Create(1, "Leg Day", "admin");
legDay.AssignBodyPart(quadsId, "admin");
legDay.AssignBodyPart(hamstringsId, "admin");
legDay.AssignBodyPart(calvesId, "admin");
```

## Validation Rules

| Entity | Field | Validation |
|--------|-------|------------|
| Day | Name | Required, max 256 characters |
| BodyPart | Name | Required, max 256 characters |

## Relationships

```
Day (1) --- (*) BodyPart (through BodyPartDay)
Workout (1) --- (*) BodyPart (through WorkoutBodyPart)
Day (1) --- (*) ScheduledExercise
```

## Best Practices

1. **Consistent Naming**: Use descriptive day names that reflect the training focus
2. **Body Part Groups**: Group related body parts logically (push muscles, pull muscles)
3. **Rest Distribution**: Plan days to allow adequate recovery between training same muscles
4. **Progressive Overload**: Use scheduled exercises to track and progress weights over time
