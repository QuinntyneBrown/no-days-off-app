# Athlete Management User Guide

## Overview

The Athlete microservice manages athlete profiles, weight tracking, and exercise completion history. Athletes are the core users of the fitness tracking system.

---

## Behaviors

### 1. Create Athlete

**Purpose**: Create a new athlete profile in the system.

**Command**: `CreateAthleteCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Athlete's full name (max 256 chars) |
| Username | string | Yes | Unique username |
| ImageUrl | string | No | Profile image URL |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/athletes
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "John Doe",
    "username": "johndoe",
    "imageUrl": "https://example.com/avatar.jpg"
}
```

#### Response
```json
{
    "athleteId": 1,
    "name": "John Doe",
    "username": "johndoe",
    "imageUrl": "https://example.com/avatar.jpg",
    "currentWeight": null,
    "lastWeighedOn": null,
    "createdOn": "2024-01-15T10:30:00Z"
}
```

#### Business Rules
- Username must be unique within the tenant
- Name cannot exceed 256 characters
- Upon creation, an `AthleteCreatedMessage` is published to the message bus

#### Message Bus Event
```json
{
    "topic": "athlete.created",
    "payload": {
        "athleteId": 1,
        "name": "John Doe",
        "tenantId": 1
    }
}
```

---

### 2. Update Athlete

**Purpose**: Update an existing athlete's profile information.

**Command**: `UpdateAthleteCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| AthleteId | int | Yes | Athlete identifier |
| Name | string | Yes | Updated name |
| Username | string | Yes | Updated username |
| ImageUrl | string | No | Updated profile image URL |

#### API Endpoint
```
PUT /api/athletes/{athleteId}
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "John D. Doe",
    "username": "johndoe_updated",
    "imageUrl": "https://example.com/new-avatar.jpg"
}
```

#### Response
```json
{
    "athleteId": 1,
    "name": "John D. Doe",
    "username": "johndoe_updated",
    "imageUrl": "https://example.com/new-avatar.jpg",
    "lastModifiedOn": "2024-01-16T14:20:00Z"
}
```

#### Business Rules
- Athlete must exist
- Username must remain unique
- Audit trail is updated

---

### 3. Delete Athlete

**Purpose**: Soft delete an athlete from the system.

**Command**: `DeleteAthleteCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| AthleteId | int | Yes | Athlete identifier |

#### API Endpoint
```
DELETE /api/athletes/{athleteId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "message": "Athlete deleted successfully"
}
```

#### Business Rules
- Performs soft delete (sets `IsDeleted = true`)
- Related data (weights, exercises) is preserved
- Athlete can be restored by admin if needed

---

### 4. Get Athletes

**Purpose**: Retrieve a list of all athletes in the current tenant.

**Query**: `GetAthletesQuery`

#### API Endpoint
```
GET /api/athletes
Authorization: Bearer {accessToken}
```

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| page | int | Page number (default: 1) |
| pageSize | int | Items per page (default: 20) |
| search | string | Search by name or username |

#### Response
```json
{
    "athletes": [
        {
            "athleteId": 1,
            "name": "John Doe",
            "username": "johndoe",
            "currentWeight": 75,
            "lastWeighedOn": "2024-01-15T08:00:00Z"
        },
        {
            "athleteId": 2,
            "name": "Jane Smith",
            "username": "janesmith",
            "currentWeight": 65,
            "lastWeighedOn": "2024-01-14T07:30:00Z"
        }
    ],
    "totalCount": 2,
    "page": 1,
    "pageSize": 20
}
```

---

### 5. Get Athlete by ID

**Purpose**: Retrieve detailed information for a specific athlete.

**Query**: `GetAthleteByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| AthleteId | int | Yes | Athlete identifier |

#### API Endpoint
```
GET /api/athletes/{athleteId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "athleteId": 1,
    "name": "John Doe",
    "username": "johndoe",
    "imageUrl": "https://example.com/avatar.jpg",
    "currentWeight": 75,
    "lastWeighedOn": "2024-01-15T08:00:00Z",
    "createdOn": "2024-01-01T10:00:00Z",
    "weights": [
        {
            "weightInKgs": 75,
            "weighedOn": "2024-01-15T08:00:00Z"
        },
        {
            "weightInKgs": 76,
            "weighedOn": "2024-01-08T08:00:00Z"
        }
    ],
    "recentExercises": [
        {
            "exerciseName": "Bench Press",
            "completedOn": "2024-01-15T10:30:00Z",
            "sets": 3,
            "reps": 10,
            "weightInKgs": 50
        }
    ]
}
```

---

### 6. Record Weight

**Purpose**: Log a weight measurement for an athlete.

**Domain Operation**: `Athlete.RecordWeight()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| AthleteId | int | Yes | Athlete identifier |
| WeightInKgs | int | Yes | Weight in kilograms |
| WeighedOn | DateTime | Yes | Date/time of measurement |

#### API Endpoint
```
POST /api/athletes/{athleteId}/weights
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "weightInKgs": 74,
    "weighedOn": "2024-01-16T07:30:00Z"
}
```

#### Response
```json
{
    "athleteId": 1,
    "weightId": 15,
    "weightInKgs": 74,
    "weighedOn": "2024-01-16T07:30:00Z",
    "previousWeight": 75,
    "weightChange": -1
}
```

#### Business Rules
- Weight must be greater than 0
- If `weighedOn` is equal to or later than `lastWeighedOn`, the `currentWeight` and `lastWeighedOn` are automatically updated
- Weight history is always maintained in descending order by date

---

### 7. Get Weight History

**Purpose**: Retrieve an athlete's weight measurement history.

**Domain Operation**: `Athlete.GetWeightHistory()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| AthleteId | int | Yes | Athlete identifier |
| Count | int | No | Number of records (default: 10) |

#### API Endpoint
```
GET /api/athletes/{athleteId}/weights?count=30
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "athleteId": 1,
    "weights": [
        {
            "weightId": 15,
            "weightInKgs": 74,
            "weighedOn": "2024-01-16T07:30:00Z"
        },
        {
            "weightId": 14,
            "weightInKgs": 75,
            "weighedOn": "2024-01-15T08:00:00Z"
        },
        {
            "weightId": 13,
            "weightInKgs": 76,
            "weighedOn": "2024-01-08T08:00:00Z"
        }
    ]
}
```

---

### 8. Calculate Weight Change

**Purpose**: Calculate weight change over a specified time period.

**Domain Operation**: `Athlete.CalculateWeightChange()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| AthleteId | int | Yes | Athlete identifier |
| Days | int | Yes | Number of days to analyze |

#### API Endpoint
```
GET /api/athletes/{athleteId}/weight-change?days=30
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "athleteId": 1,
    "periodDays": 30,
    "startWeight": 78,
    "endWeight": 74,
    "weightChange": -4,
    "percentageChange": -5.13
}
```

---

### 9. Record Completed Exercise

**Purpose**: Log a completed exercise with performance metrics.

**Domain Operation**: `Athlete.RecordCompletedExercise()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| AthleteId | int | Yes | Athlete identifier |
| ScheduledExerciseId | int | Yes | Reference to scheduled exercise |
| WeightInKgs | int | No | Weight used (for strength training) |
| Reps | int | No | Repetitions completed |
| Sets | int | No | Sets completed |
| Distance | int | No | Distance in meters (for cardio) |
| TimeInSeconds | int | No | Duration (for cardio) |
| CompletionDateTime | DateTime | Yes | When exercise was completed |

#### API Endpoint
```
POST /api/athletes/{athleteId}/completed-exercises
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "scheduledExerciseId": 5,
    "weightInKgs": 55,
    "reps": 10,
    "sets": 3,
    "completionDateTime": "2024-01-16T10:45:00Z"
}
```

#### Response
```json
{
    "completedExerciseId": 42,
    "athleteId": 1,
    "scheduledExerciseId": 5,
    "exerciseName": "Bench Press",
    "weightInKgs": 55,
    "reps": 10,
    "sets": 3,
    "completionDateTime": "2024-01-16T10:45:00Z"
}
```

#### Business Rules
- Reps, sets, weight cannot be negative
- At least one metric must be provided
- CompletionDateTime is required

---

### 10. Get Completed Exercises by Date

**Purpose**: Retrieve all exercises completed by an athlete on a specific date.

**Domain Operation**: `Athlete.GetCompletedExercisesByDate()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| AthleteId | int | Yes | Athlete identifier |
| Date | DateTime | Yes | Target date |

#### API Endpoint
```
GET /api/athletes/{athleteId}/completed-exercises?date=2024-01-16
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "athleteId": 1,
    "date": "2024-01-16",
    "completedExercises": [
        {
            "completedExerciseId": 42,
            "exerciseName": "Bench Press",
            "weightInKgs": 55,
            "reps": 10,
            "sets": 3,
            "completionDateTime": "2024-01-16T10:45:00Z"
        },
        {
            "completedExerciseId": 43,
            "exerciseName": "Incline Dumbbell Press",
            "weightInKgs": 25,
            "reps": 12,
            "sets": 3,
            "completionDateTime": "2024-01-16T11:00:00Z"
        }
    ],
    "totalExercises": 2
}
```

---

## Athlete Lifecycle

```
                    +-------------------+
                    |   Registration    |
                    +-------------------+
                            |
                            v
                    +-------------------+
                    |  Create Athlete   |
                    +-------------------+
                            |
                            v
            +-------------------------------+
            |       Active State            |
            |  - Record Weights             |
            |  - Complete Exercises         |
            |  - Update Profile             |
            +-------------------------------+
                            |
                            v
                    +-------------------+
                    |  Delete Athlete   |
                    |  (Soft Delete)    |
                    +-------------------+
                            |
                            v
                    +-------------------+
                    |   Archived        |
                    |   (Recoverable)   |
                    +-------------------+
```

---

## Validation Rules Summary

| Field | Validation Rule |
|-------|-----------------|
| Name | Required, max 256 characters |
| Username | Required, unique within tenant |
| WeightInKgs | Must be greater than 0 |
| Reps | Cannot be negative |
| Sets | Cannot be negative |
| Distance | Cannot be negative |
| TimeInSeconds | Cannot be negative |

---

## Error Codes

| Code | Description |
|------|-------------|
| 400 | Invalid input data |
| 401 | Unauthorized access |
| 403 | Forbidden (wrong tenant) |
| 404 | Athlete not found |
| 409 | Username already exists |
