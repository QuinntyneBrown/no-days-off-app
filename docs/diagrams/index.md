# No Days Off - Use Case Diagrams Index

## Overview

This directory contains comprehensive use case diagrams for all behaviors in the No Days Off fitness tracking application. The diagrams are organized by microservice/module.

## Diagram Files

| Module | File | Use Cases |
|--------|------|-----------|
| **All Modules** | [use-cases.md](./use-cases.md) | Complete overview with Mermaid diagrams |
| **Identity** | [identity-use-cases.md](./identity-use-cases.md) | 9 use cases |
| **Athletes** | [athlete-use-cases.md](./athlete-use-cases.md) | 10 use cases |
| **Exercises** | [exercise-use-cases.md](./exercise-use-cases.md) | 14 use cases |
| **Workouts** | [workout-use-cases.md](./workout-use-cases.md) | 11 use cases |
| **Dashboard** | [dashboard-use-cases.md](./dashboard-use-cases.md) | 18 use cases |
| **Communication** | [communication-use-cases.md](./communication-use-cases.md) | 10 use cases |
| **Media** | [media-use-cases.md](./media-use-cases.md) | 14 use cases |

## Total Use Cases: 95+

## Diagram Types Included

Each module file contains:

1. **Overview Diagrams** - High-level view of all use cases in the module
2. **Individual Use Case Diagrams** - Detailed flow for each behavior
3. **Sequence Diagrams** - Interaction flows between components
4. **Activity Diagrams** - Process flows and decision points
5. **Entity Relationship Diagrams** - Data model relationships
6. **Permissions Matrices** - Role-based access control

## Actor Definitions

| Actor | Description |
|-------|-------------|
| **User** | Any authenticated system user |
| **Athlete** | User with athlete profile (can track workouts) |
| **Coach** | User with coaching privileges (can manage athletes) |
| **Admin** | System administrator (full access) |
| **System** | Internal processes (background jobs, events) |

## Quick Navigation

### Authentication & Users
- [Register Account](./identity-use-cases.md#uc1-register-account)
- [Login](./identity-use-cases.md#uc2-login)
- [Token Management](./identity-use-cases.md#uc3-refresh-token)

### Athlete Features
- [Create Profile](./athlete-use-cases.md#uc1-create-athlete-profile)
- [Weight Tracking](./athlete-use-cases.md#uc4-record-weight)
- [Exercise Completion](./athlete-use-cases.md#uc7-record-completed-exercise)

### Exercise Management
- [Create Exercise](./exercise-use-cases.md#uc1-create-exercise)
- [Schedule Exercise](./exercise-use-cases.md#uc11-14-scheduled-exercise-flow)
- [Digital Assets](./exercise-use-cases.md#uc5--uc6-digital-asset-management)

### Workout Planning
- [Create Workout](./workout-use-cases.md#uc1-create-workout)
- [Workout Days](./workout-use-cases.md#uc7-create-workout-day)
- [Workout Templates](./workout-use-cases.md#workout-template-examples)

### Dashboard & Analytics
- [Dashboard Management](./dashboard-use-cases.md#uc7-uc10-tile-management-flow)
- [Widget Types](./dashboard-use-cases.md#widget-types-and-statistics)
- [Statistics](./dashboard-use-cases.md#dashboard-statistics-flow)

### Communication
- [Notifications](./communication-use-cases.md#uc1-create-notification-system)
- [Messaging](./communication-use-cases.md#uc5-uc9-conversation--messaging-flow)
- [Real-time Updates](./communication-use-cases.md#real-time-communication-signalr)

### Media Management
- [File Upload](./media-use-cases.md#uc1-upload-media-file)
- [Digital Assets](./media-use-cases.md#uc5-create-digital-asset)
- [Video Content](./media-use-cases.md#uc9-create-video-youtube-integration)

## Viewing the Diagrams

### Mermaid Diagrams
The `use-cases.md` file contains Mermaid diagrams that can be rendered in:
- GitHub (native support)
- VS Code with Mermaid extension
- Online Mermaid editors (mermaid.live)

### ASCII Diagrams
Individual module files use ASCII art diagrams that render correctly in any text viewer or markdown renderer.

## Related Documentation

- **User Guides**: [../user-guides/](../user-guides/)
- **Feature Specs**: [../features/](../features/)
- **System Specs**: [../specs/](../specs/)
