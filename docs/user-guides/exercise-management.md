# Exercise Management User Guide

## Overview

The Exercise microservice manages exercise definitions, body part classifications, default configurations, and scheduled exercises within workout plans.

---

## Behaviors

### 1. Create Exercise

**Purpose**: Define a new exercise with default configurations.

**Command**: `CreateExerciseCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Exercise name (max 256 chars) |
| BodyPartId | int | No | Associated body part |
| DefaultSets | int | No | Default number of sets |
| DefaultRepetitions | int | No | Default reps per set |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/exercises
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Bench Press",
    "bodyPartId": 1,
    "defaultSets": 3,
    "defaultRepetitions": 10
}
```

#### Response
```json
{
    "exerciseId": 1,
    "name": "Bench Press",
    "bodyPartId": 1,
    "bodyPartName": "Chest",
    "defaultSets": 3,
    "defaultRepetitions": 10,
    "defaultSetCollection": [],
    "digitalAssets": [],
    "createdOn": "2024-01-15T10:30:00Z"
}
```

#### Business Rules
- Name must be unique within tenant
- DefaultSets and DefaultRepetitions cannot be negative
- BodyPartId must reference a valid body part if provided

---

### 2. Update Exercise

**Purpose**: Modify an existing exercise definition.

**Command**: `UpdateExerciseCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ExerciseId | int | Yes | Exercise identifier |
| Name | string | Yes | Updated name |
| BodyPartId | int | No | Updated body part association |
| DefaultSets | int | No | Updated default sets |
| DefaultRepetitions | int | No | Updated default reps |

#### API Endpoint
```
PUT /api/exercises/{exerciseId}
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Barbell Bench Press",
    "bodyPartId": 1,
    "defaultSets": 4,
    "defaultRepetitions": 8
}
```

#### Response
```json
{
    "exerciseId": 1,
    "name": "Barbell Bench Press",
    "bodyPartId": 1,
    "defaultSets": 4,
    "defaultRepetitions": 8,
    "lastModifiedOn": "2024-01-16T14:20:00Z"
}
```

---

### 3. Delete Exercise

**Purpose**: Remove an exercise definition from the system.

**Command**: `DeleteExerciseCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ExerciseId | int | Yes | Exercise identifier |

#### API Endpoint
```
DELETE /api/exercises/{exerciseId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "message": "Exercise deleted successfully"
}
```

#### Business Rules
- Performs soft delete
- Scheduled exercises referencing this exercise are preserved
- Historical completed exercises remain intact

---

### 4. Get Exercises

**Purpose**: Retrieve a list of all exercises.

**Query**: `GetExercisesQuery`

#### API Endpoint
```
GET /api/exercises
Authorization: Bearer {accessToken}
```

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| bodyPartId | int | Filter by body part |
| search | string | Search by name |
| page | int | Page number |
| pageSize | int | Items per page |

#### Response
```json
{
    "exercises": [
        {
            "exerciseId": 1,
            "name": "Bench Press",
            "bodyPartId": 1,
            "bodyPartName": "Chest",
            "defaultSets": 3,
            "defaultRepetitions": 10
        },
        {
            "exerciseId": 2,
            "name": "Squat",
            "bodyPartId": 2,
            "bodyPartName": "Legs",
            "defaultSets": 4,
            "defaultRepetitions": 8
        }
    ],
    "totalCount": 25,
    "page": 1,
    "pageSize": 20
}
```

---

### 5. Get Exercise by ID

**Purpose**: Retrieve detailed information for a specific exercise.

**Query**: `GetExerciseByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ExerciseId | int | Yes | Exercise identifier |

#### API Endpoint
```
GET /api/exercises/{exerciseId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "exerciseId": 1,
    "name": "Bench Press",
    "bodyPartId": 1,
    "bodyPartName": "Chest",
    "defaultSets": 3,
    "defaultRepetitions": 10,
    "defaultSetCollection": [
        {
            "setId": 1,
            "weightInKgs": 50,
            "repetitions": 10
        },
        {
            "setId": 2,
            "weightInKgs": 55,
            "repetitions": 8
        },
        {
            "setId": 3,
            "weightInKgs": 60,
            "repetitions": 6
        }
    ],
    "digitalAssets": [
        {
            "digitalAssetId": 1,
            "url": "https://cdn.example.com/exercises/bench-press.jpg",
            "type": "Image"
        }
    ]
}
```

---

### 6. Add Default Set

**Purpose**: Add a default set configuration to an exercise template.

**Domain Operation**: `Exercise.AddDefaultSet()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ExerciseId | int | Yes | Exercise identifier |
| WeightInKgs | int | Yes | Weight for this set |
| Repetitions | int | Yes | Reps for this set |

#### API Endpoint
```
POST /api/exercises/{exerciseId}/default-sets
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "weightInKgs": 60,
    "repetitions": 6
}
```

#### Response
```json
{
    "exerciseId": 1,
    "setId": 4,
    "weightInKgs": 60,
    "repetitions": 6
}
```

#### Business Rules
- WeightInKgs cannot be negative
- Repetitions cannot be negative
- Sets are ordered by creation

---

### 7. Add Digital Asset

**Purpose**: Attach instructional media (images, videos) to an exercise.

**Domain Operation**: `Exercise.AddDigitalAsset()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ExerciseId | int | Yes | Exercise identifier |
| DigitalAssetId | int | Yes | Digital asset identifier |

#### API Endpoint
```
POST /api/exercises/{exerciseId}/digital-assets
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "digitalAssetId": 5
}
```

#### Response
```json
{
    "exerciseId": 1,
    "digitalAssetId": 5,
    "url": "https://cdn.example.com/videos/bench-press-tutorial.mp4",
    "type": "Video"
}
```

---

### 8. Remove Digital Asset

**Purpose**: Detach a digital asset from an exercise.

**Domain Operation**: `Exercise.RemoveDigitalAsset()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ExerciseId | int | Yes | Exercise identifier |
| DigitalAssetId | int | Yes | Digital asset to remove |

#### API Endpoint
```
DELETE /api/exercises/{exerciseId}/digital-assets/{digitalAssetId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "message": "Digital asset removed from exercise"
}
```

---

## Body Part Management

### 9. Create Body Part

**Purpose**: Define a new body part category for exercises.

**Command**: `CreateBodyPartCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Body part name |
| Description | string | No | Description |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/body-parts
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Chest",
    "description": "Pectoral muscles"
}
```

#### Response
```json
{
    "bodyPartId": 1,
    "name": "Chest",
    "description": "Pectoral muscles",
    "exerciseCount": 0
}
```

---

### 10. Get Body Parts

**Purpose**: Retrieve all body part categories.

**Query**: `GetBodyPartsQuery`

#### API Endpoint
```
GET /api/body-parts
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "bodyParts": [
        {
            "bodyPartId": 1,
            "name": "Chest",
            "exerciseCount": 5
        },
        {
            "bodyPartId": 2,
            "name": "Back",
            "exerciseCount": 8
        },
        {
            "bodyPartId": 3,
            "name": "Legs",
            "exerciseCount": 10
        },
        {
            "bodyPartId": 4,
            "name": "Shoulders",
            "exerciseCount": 6
        },
        {
            "bodyPartId": 5,
            "name": "Arms",
            "exerciseCount": 7
        },
        {
            "bodyPartId": 6,
            "name": "Core",
            "exerciseCount": 4
        }
    ]
}
```

---

## Scheduled Exercise Management

### 11. Create Scheduled Exercise

**Purpose**: Schedule an exercise for a specific workout day.

**Domain Operation**: `ScheduledExercise.Create()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Scheduled exercise name |
| DayId | int | No | Associated workout day |
| ExerciseId | int | No | Base exercise template |
| Sort | int | Yes | Order in workout |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/scheduled-exercises
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Morning Bench Press",
    "dayId": 1,
    "exerciseId": 1,
    "sort": 0
}
```

#### Response
```json
{
    "scheduledExerciseId": 1,
    "name": "Morning Bench Press",
    "dayId": 1,
    "exerciseId": 1,
    "sort": 0,
    "repetitions": 0,
    "sets": 0,
    "weightInKgs": 0
}
```

---

### 12. Update Performance Targets

**Purpose**: Set strength training targets for a scheduled exercise.

**Domain Operation**: `ScheduledExercise.UpdatePerformanceTargets()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ScheduledExerciseId | int | Yes | Scheduled exercise ID |
| Repetitions | int | Yes | Target reps per set |
| Sets | int | Yes | Target number of sets |
| WeightInKgs | int | Yes | Target weight |

#### API Endpoint
```
PUT /api/scheduled-exercises/{scheduledExerciseId}/performance-targets
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "repetitions": 10,
    "sets": 3,
    "weightInKgs": 50
}
```

#### Response
```json
{
    "scheduledExerciseId": 1,
    "repetitions": 10,
    "sets": 3,
    "weightInKgs": 50
}
```

---

### 13. Update Cardio Targets

**Purpose**: Set cardio-specific targets for a scheduled exercise.

**Domain Operation**: `ScheduledExercise.UpdateCardioTargets()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ScheduledExerciseId | int | Yes | Scheduled exercise ID |
| Distance | int | Yes | Target distance in meters |
| TimeInSeconds | int | Yes | Target duration |

#### API Endpoint
```
PUT /api/scheduled-exercises/{scheduledExerciseId}/cardio-targets
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "distance": 5000,
    "timeInSeconds": 1800
}
```

#### Response
```json
{
    "scheduledExerciseId": 1,
    "distance": 5000,
    "timeInSeconds": 1800,
    "targetPace": "6:00/km"
}
```

---

### 14. Add Set to Scheduled Exercise

**Purpose**: Add a specific set configuration to a scheduled exercise.

**Domain Operation**: `ScheduledExercise.AddSet()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| ScheduledExerciseId | int | Yes | Scheduled exercise ID |
| WeightInKgs | int | Yes | Weight for this set |
| Repetitions | int | Yes | Reps for this set |

#### API Endpoint
```
POST /api/scheduled-exercises/{scheduledExerciseId}/sets
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "weightInKgs": 55,
    "repetitions": 8
}
```

#### Response
```json
{
    "scheduledExerciseId": 1,
    "setId": 1,
    "weightInKgs": 55,
    "repetitions": 8,
    "setNumber": 2
}
```

---

## Entity Relationships

```
+-------------+         +----------------+
| BodyPart    |<------->| Exercise       |
+-------------+    1:*  +----------------+
                              |
                              | 1:*
                              v
                        +----------------+
                        | ExerciseSet    |
                        +----------------+

                        +----------------+
                        | Exercise       |
                        +----------------+
                              |
                              | 1:*
                              v
                        +--------------------+
                        | ScheduledExercise  |
                        +--------------------+
                              |
                              | 1:*
                              v
                        +------------------------+
                        | ScheduledExerciseSet   |
                        +------------------------+

+----------------+    *:*   +----------------+
| Exercise       |<-------->| DigitalAsset   |
+----------------+          +----------------+
```

---

## Exercise Types

| Type | Description | Key Metrics |
|------|-------------|-------------|
| Strength | Weight-based exercises | Sets, Reps, Weight |
| Cardio | Endurance exercises | Distance, Time |
| Flexibility | Stretching exercises | Duration |
| Compound | Multi-joint movements | Sets, Reps, Weight |
| Isolation | Single-joint movements | Sets, Reps, Weight |

---

## Validation Rules Summary

| Entity | Field | Validation |
|--------|-------|------------|
| Exercise | Name | Required, max 256 chars |
| Exercise | DefaultSets | Cannot be negative |
| Exercise | DefaultRepetitions | Cannot be negative |
| ExerciseSet | WeightInKgs | Cannot be negative |
| ExerciseSet | Repetitions | Cannot be negative |
| ScheduledExercise | Name | Required, max 256 chars |
| ScheduledExercise | Repetitions | Cannot be negative |
| ScheduledExercise | Sets | Cannot be negative |
| ScheduledExercise | WeightInKgs | Cannot be negative |
| ScheduledExercise | Distance | Cannot be negative |
| ScheduledExercise | TimeInSeconds | Cannot be negative |

---

## Error Codes

| Code | Description |
|------|-------------|
| 400 | Invalid input data |
| 401 | Unauthorized access |
| 404 | Exercise or BodyPart not found |
| 409 | Exercise name already exists |
