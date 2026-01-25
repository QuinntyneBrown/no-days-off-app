# Workout Management User Guide

## Overview

The Workout microservice manages workout sessions, workout days, and the organization of exercises into structured training plans.

---

## Behaviors

### 1. Create Workout

**Purpose**: Create a new workout session or template.

**Command**: `CreateWorkoutCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Workout name |
| Description | string | No | Workout description |
| BodyPartIds | int[] | No | Associated body parts |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/workouts
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Push Day",
    "description": "Chest, shoulders, and triceps workout",
    "bodyPartIds": [1, 4, 5]
}
```

#### Response
```json
{
    "workoutId": 1,
    "name": "Push Day",
    "description": "Chest, shoulders, and triceps workout",
    "bodyParts": [
        { "bodyPartId": 1, "name": "Chest" },
        { "bodyPartId": 4, "name": "Shoulders" },
        { "bodyPartId": 5, "name": "Triceps" }
    ],
    "days": [],
    "createdOn": "2024-01-15T10:30:00Z"
}
```

#### Business Rules
- Name is required and must be unique within tenant
- Body parts help categorize the workout for filtering

---

### 2. Get Workouts

**Purpose**: Retrieve a list of all workouts.

**Query**: `GetWorkoutsQuery`

#### API Endpoint
```
GET /api/workouts
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
    "workouts": [
        {
            "workoutId": 1,
            "name": "Push Day",
            "description": "Chest, shoulders, and triceps",
            "bodyParts": ["Chest", "Shoulders", "Triceps"],
            "exerciseCount": 6
        },
        {
            "workoutId": 2,
            "name": "Pull Day",
            "description": "Back and biceps workout",
            "bodyParts": ["Back", "Biceps"],
            "exerciseCount": 7
        },
        {
            "workoutId": 3,
            "name": "Leg Day",
            "description": "Quadriceps, hamstrings, and calves",
            "bodyParts": ["Legs"],
            "exerciseCount": 8
        }
    ],
    "totalCount": 3,
    "page": 1,
    "pageSize": 20
}
```

---

### 3. Get Workout by ID

**Purpose**: Retrieve detailed information for a specific workout.

**Query**: `GetWorkoutByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| WorkoutId | int | Yes | Workout identifier |

#### API Endpoint
```
GET /api/workouts/{workoutId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "workoutId": 1,
    "name": "Push Day",
    "description": "Chest, shoulders, and triceps workout",
    "bodyParts": [
        { "bodyPartId": 1, "name": "Chest" },
        { "bodyPartId": 4, "name": "Shoulders" },
        { "bodyPartId": 5, "name": "Triceps" }
    ],
    "days": [
        {
            "dayId": 1,
            "name": "Monday Push",
            "exercises": [
                {
                    "scheduledExerciseId": 1,
                    "name": "Bench Press",
                    "sets": 4,
                    "reps": 8,
                    "weightInKgs": 60
                },
                {
                    "scheduledExerciseId": 2,
                    "name": "Overhead Press",
                    "sets": 3,
                    "reps": 10,
                    "weightInKgs": 40
                }
            ]
        }
    ],
    "totalExercises": 6,
    "estimatedDuration": 45
}
```

---

### 4. Delete Workout

**Purpose**: Remove a workout from the system.

**Command**: `DeleteWorkoutCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| WorkoutId | int | Yes | Workout identifier |

#### API Endpoint
```
DELETE /api/workouts/{workoutId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "message": "Workout deleted successfully"
}
```

#### Business Rules
- Performs soft delete
- Associated days and scheduled exercises are preserved
- Completed exercise history remains intact

---

### 5. Add Body Part to Workout

**Purpose**: Associate a body part with a workout.

**Domain Operation**: `Workout.AddBodyPart()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| WorkoutId | int | Yes | Workout identifier |
| BodyPartId | int | Yes | Body part to add |

#### API Endpoint
```
POST /api/workouts/{workoutId}/body-parts
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "bodyPartId": 6
}
```

#### Response
```json
{
    "workoutId": 1,
    "bodyParts": [
        { "bodyPartId": 1, "name": "Chest" },
        { "bodyPartId": 4, "name": "Shoulders" },
        { "bodyPartId": 5, "name": "Triceps" },
        { "bodyPartId": 6, "name": "Core" }
    ]
}
```

---

### 6. Check Body Part Association

**Purpose**: Verify if a workout includes a specific body part.

**Domain Operation**: `Workout.HasBodyPart()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| WorkoutId | int | Yes | Workout identifier |
| BodyPartId | int | Yes | Body part to check |

#### API Endpoint
```
GET /api/workouts/{workoutId}/has-body-part/{bodyPartId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "workoutId": 1,
    "bodyPartId": 1,
    "hasBodyPart": true
}
```

---

## Day Management

### 7. Create Day

**Purpose**: Create a workout day to organize exercises.

**Command**: `CreateDayCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Day name |
| WorkoutId | int | No | Associated workout |
| Sort | int | Yes | Order within workout |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/days
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Monday Push",
    "workoutId": 1,
    "sort": 0
}
```

#### Response
```json
{
    "dayId": 1,
    "name": "Monday Push",
    "workoutId": 1,
    "sort": 0,
    "scheduledExercises": [],
    "createdOn": "2024-01-15T10:30:00Z"
}
```

---

### 8. Get Days

**Purpose**: Retrieve all workout days.

**Query**: `GetDaysQuery`

#### API Endpoint
```
GET /api/days
Authorization: Bearer {accessToken}
```

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| workoutId | int | Filter by workout |

#### Response
```json
{
    "days": [
        {
            "dayId": 1,
            "name": "Monday Push",
            "workoutId": 1,
            "workoutName": "Push Day",
            "exerciseCount": 6
        },
        {
            "dayId": 2,
            "name": "Tuesday Pull",
            "workoutId": 2,
            "workoutName": "Pull Day",
            "exerciseCount": 7
        }
    ]
}
```

---

### 9. Get Day by ID

**Purpose**: Retrieve detailed information for a specific day.

**Query**: `GetDayByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DayId | int | Yes | Day identifier |

#### API Endpoint
```
GET /api/days/{dayId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "dayId": 1,
    "name": "Monday Push",
    "workoutId": 1,
    "workoutName": "Push Day",
    "sort": 0,
    "scheduledExercises": [
        {
            "scheduledExerciseId": 1,
            "name": "Bench Press",
            "sort": 0,
            "sets": 4,
            "repetitions": 8,
            "weightInKgs": 60,
            "setCollection": [
                { "setId": 1, "weightInKgs": 50, "repetitions": 10 },
                { "setId": 2, "weightInKgs": 55, "repetitions": 8 },
                { "setId": 3, "weightInKgs": 60, "repetitions": 6 },
                { "setId": 4, "weightInKgs": 60, "repetitions": 6 }
            ]
        },
        {
            "scheduledExerciseId": 2,
            "name": "Overhead Press",
            "sort": 1,
            "sets": 3,
            "repetitions": 10,
            "weightInKgs": 40
        }
    ]
}
```

---

### 10. Update Day

**Purpose**: Update a workout day's information.

**Command**: `UpdateDayCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DayId | int | Yes | Day identifier |
| Name | string | Yes | Updated name |
| Sort | int | No | Updated order |

#### API Endpoint
```
PUT /api/days/{dayId}
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Monday - Heavy Push",
    "sort": 0
}
```

#### Response
```json
{
    "dayId": 1,
    "name": "Monday - Heavy Push",
    "sort": 0,
    "lastModifiedOn": "2024-01-16T14:20:00Z"
}
```

---

### 11. Delete Day

**Purpose**: Remove a workout day.

**Command**: `DeleteDayCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DayId | int | Yes | Day identifier |

#### API Endpoint
```
DELETE /api/days/{dayId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "message": "Day deleted successfully"
}
```

---

## Workout Structure

```
+------------------+
|     Workout      |
|  (Push Day)      |
+------------------+
        |
        | 1:*
        v
+------------------+     +------------------+     +------------------+
|    Day 1         |     |    Day 2         |     |    Day 3         |
| (Monday Push)    |     | (Wednesday Push) |     | (Friday Push)    |
+------------------+     +------------------+     +------------------+
        |                        |                        |
        | 1:*                    | 1:*                    | 1:*
        v                        v                        v
+------------------+     +------------------+     +------------------+
| Scheduled        |     | Scheduled        |     | Scheduled        |
| Exercises        |     | Exercises        |     | Exercises        |
| - Bench Press    |     | - Incline Press  |     | - Decline Press  |
| - Overhead Press |     | - Arnold Press   |     | - Lateral Raises |
| - Tricep Dips    |     | - Face Pulls     |     | - Skull Crushers |
+------------------+     +------------------+     +------------------+
```

---

## Workout Templates

### Standard PPL Split

```json
{
    "template": "PPL Split",
    "workouts": [
        {
            "name": "Push Day",
            "days": ["Monday", "Thursday"],
            "bodyParts": ["Chest", "Shoulders", "Triceps"]
        },
        {
            "name": "Pull Day",
            "days": ["Tuesday", "Friday"],
            "bodyParts": ["Back", "Biceps"]
        },
        {
            "name": "Leg Day",
            "days": ["Wednesday", "Saturday"],
            "bodyParts": ["Quadriceps", "Hamstrings", "Calves"]
        }
    ]
}
```

### Upper/Lower Split

```json
{
    "template": "Upper/Lower",
    "workouts": [
        {
            "name": "Upper Body",
            "days": ["Monday", "Thursday"],
            "bodyParts": ["Chest", "Back", "Shoulders", "Arms"]
        },
        {
            "name": "Lower Body",
            "days": ["Tuesday", "Friday"],
            "bodyParts": ["Quadriceps", "Hamstrings", "Glutes", "Calves"]
        }
    ]
}
```

---

## Entity Relationships

```
+------------------+
|     Workout      |
+------------------+
        |
        | 1:*
        v
+------------------+    *:*   +------------------+
|      Day         |<-------->| ScheduledExercise|
+------------------+          +------------------+
        ^                            |
        |                            | 1:*
        | *:*                        v
        |                     +----------------------+
+------------------+          | ScheduledExerciseSet |
|    BodyPart      |          +----------------------+
+------------------+
```

---

## Validation Rules Summary

| Entity | Field | Validation |
|--------|-------|------------|
| Workout | Name | Required, max 256 chars |
| Day | Name | Required, max 256 chars |
| Day | Sort | Cannot be negative |

---

## Error Codes

| Code | Description |
|------|-------------|
| 400 | Invalid input data |
| 401 | Unauthorized access |
| 404 | Workout or Day not found |
| 409 | Name already exists |
