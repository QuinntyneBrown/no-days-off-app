# No Days Off - User Guides

## Overview

No Days Off is a comprehensive fitness tracking application built on a microservices architecture. This documentation provides complete user guides for all system behaviors organized by module.

## Table of Contents

1. [Identity & Authentication](./identity-authentication.md)
   - User Registration
   - Login & Authentication
   - Token Management
   - Multi-tenant Support

2. [Athlete Management](./athlete-management.md)
   - Profile Management
   - Weight Tracking
   - Exercise Completion Tracking
   - Progress Analytics

3. [Exercise Management](./exercise-management.md)
   - Exercise Definition
   - Body Part Classification
   - Digital Asset Attachment
   - Default Set Configuration

4. [Workout Management](./workout-management.md)
   - Workout Creation
   - Day Scheduling
   - Exercise Scheduling
   - Workout Templates

5. [Dashboard Management](./dashboard-management.md)
   - Dashboard Creation
   - Widget Configuration
   - Tile Management
   - Statistics Display

6. [Communication](./communication.md)
   - Notifications
   - Messaging
   - Conversations

7. [Media Management](./media-management.md)
   - File Upload
   - Digital Asset Management
   - Video Content

## Architecture Overview

```
+------------------+     +------------------+     +------------------+
|   Identity       |     |   Athletes       |     |   Exercises      |
|   Microservice   |     |   Microservice   |     |   Microservice   |
+------------------+     +------------------+     +------------------+
         |                       |                       |
         +-----------------------------------------------+
                                 |
                    +---------------------------+
                    |      Message Bus          |
                    +---------------------------+
                                 |
         +-----------------------------------------------+
         |                       |                       |
+------------------+     +------------------+     +------------------+
|   Workouts       |     |   Dashboard      |     |   Communication  |
|   Microservice   |     |   Microservice   |     |   Microservice   |
+------------------+     +------------------+     +------------------+
         |                       |                       |
         +-----------------------------------------------+
                                 |
                    +---------------------------+
                    |      Media Microservice   |
                    +---------------------------+
```

## Common Patterns

### CQRS Pattern
All modules follow the Command Query Responsibility Segregation (CQRS) pattern:
- **Commands**: Write operations (Create, Update, Delete)
- **Queries**: Read operations (Get, List, Search)

### Multi-tenancy
All entities support multi-tenancy through `TenantId` field.

### Audit Trail
All entities include audit fields:
- `CreatedOn` / `CreatedBy`
- `LastModifiedOn` / `LastModifiedBy`
- `IsDeleted` (soft delete)

### API Response Format
All API endpoints return consistent response structures with proper HTTP status codes.
