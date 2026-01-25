# Dashboard Management User Guide

## Overview

The Dashboard microservice provides customizable dashboard functionality with widgets, tiles, and statistics for visualizing fitness data and progress.

---

## Behaviors

### 1. Create Dashboard

**Purpose**: Create a new dashboard for displaying widgets and statistics.

**Command**: `CreateDashboardCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Dashboard name |
| Description | string | No | Dashboard description |
| IsDefault | bool | No | Set as default dashboard |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/dashboards
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Fitness Overview",
    "description": "Main dashboard showing workout progress and statistics",
    "isDefault": true
}
```

#### Response
```json
{
    "dashboardId": 1,
    "name": "Fitness Overview",
    "description": "Main dashboard showing workout progress and statistics",
    "isDefault": true,
    "tiles": [],
    "createdOn": "2024-01-15T10:30:00Z"
}
```

#### Business Rules
- Name is required
- Only one dashboard can be default at a time
- Setting a new default clears the previous default

---

### 2. Get Dashboards

**Purpose**: Retrieve list of all dashboards.

**Query**: `GetDashboardsQuery`

#### API Endpoint
```
GET /api/dashboards
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "dashboards": [
        {
            "dashboardId": 1,
            "name": "Fitness Overview",
            "isDefault": true,
            "tileCount": 5
        },
        {
            "dashboardId": 2,
            "name": "Weight Tracker",
            "isDefault": false,
            "tileCount": 3
        }
    ]
}
```

---

### 3. Get Dashboard by ID

**Purpose**: Retrieve detailed dashboard information including all tiles.

**Query**: `GetDashboardByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DashboardId | int | Yes | Dashboard identifier |

#### API Endpoint
```
GET /api/dashboards/{dashboardId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "dashboardId": 1,
    "name": "Fitness Overview",
    "description": "Main dashboard showing workout progress and statistics",
    "isDefault": true,
    "tiles": [
        {
            "tileId": 1,
            "name": "Current Weight",
            "widgetType": "WeightDisplay",
            "top": 0,
            "left": 0,
            "width": 2,
            "height": 1,
            "configuration": {
                "showTrend": true,
                "trendDays": 30
            }
        },
        {
            "tileId": 2,
            "name": "Weekly Progress",
            "widgetType": "ProgressChart",
            "top": 0,
            "left": 2,
            "width": 4,
            "height": 2,
            "configuration": {
                "chartType": "line",
                "metrics": ["weight", "workouts"]
            }
        }
    ]
}
```

---

### 4. Update Dashboard

**Purpose**: Modify dashboard properties.

**Command**: `UpdateDashboardCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DashboardId | int | Yes | Dashboard identifier |
| Name | string | Yes | Updated name |
| Description | string | No | Updated description |

#### API Endpoint
```
PUT /api/dashboards/{dashboardId}
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "My Fitness Dashboard",
    "description": "Personal fitness tracking dashboard"
}
```

#### Response
```json
{
    "dashboardId": 1,
    "name": "My Fitness Dashboard",
    "description": "Personal fitness tracking dashboard",
    "lastModifiedOn": "2024-01-16T14:20:00Z"
}
```

---

### 5. Delete Dashboard

**Purpose**: Remove a dashboard.

**Command**: `DeleteDashboardCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DashboardId | int | Yes | Dashboard identifier |

#### API Endpoint
```
DELETE /api/dashboards/{dashboardId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "message": "Dashboard deleted successfully"
}
```

---

### 6. Set Dashboard as Default

**Purpose**: Mark a dashboard as the default.

**Domain Operation**: `Dashboard.SetAsDefault()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DashboardId | int | Yes | Dashboard identifier |

#### API Endpoint
```
POST /api/dashboards/{dashboardId}/set-default
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "dashboardId": 1,
    "isDefault": true,
    "message": "Dashboard set as default"
}
```

#### Business Rules
- Automatically clears default flag on previous default dashboard
- Only one dashboard can be default per user

---

### 7. Clear Dashboard Default

**Purpose**: Remove the default flag from a dashboard.

**Domain Operation**: `Dashboard.ClearDefault()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DashboardId | int | Yes | Dashboard identifier |

#### API Endpoint
```
POST /api/dashboards/{dashboardId}/clear-default
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "dashboardId": 1,
    "isDefault": false,
    "message": "Default flag cleared"
}
```

---

## Tile Management

### 8. Add Tile to Dashboard

**Purpose**: Add a tile widget to a dashboard.

**Domain Operation**: `Dashboard.AddTile()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DashboardId | int | Yes | Dashboard identifier |
| TileId | int | Yes | Tile template identifier |
| Top | int | Yes | Y position on grid |
| Left | int | Yes | X position on grid |
| Width | int | Yes | Tile width in grid units |
| Height | int | Yes | Tile height in grid units |

#### API Endpoint
```
POST /api/dashboards/{dashboardId}/tiles
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "tileId": 1,
    "top": 0,
    "left": 0,
    "width": 2,
    "height": 1
}
```

#### Response
```json
{
    "dashboardId": 1,
    "dashboardTileId": 1,
    "tileId": 1,
    "tileName": "Weight Tracker",
    "top": 0,
    "left": 0,
    "width": 2,
    "height": 1
}
```

---

### 9. Update Tile Position

**Purpose**: Reposition a tile on the dashboard.

**Domain Operation**: `Dashboard.UpdateTilePosition()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DashboardId | int | Yes | Dashboard identifier |
| DashboardTileId | int | Yes | Dashboard tile identifier |
| Top | int | Yes | New Y position |
| Left | int | Yes | New X position |

#### API Endpoint
```
PUT /api/dashboards/{dashboardId}/tiles/{dashboardTileId}/position
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "top": 2,
    "left": 4
}
```

#### Response
```json
{
    "dashboardTileId": 1,
    "top": 2,
    "left": 4,
    "message": "Tile position updated"
}
```

---

### 10. Update Tile Size

**Purpose**: Resize a tile on the dashboard.

**Domain Operation**: `Dashboard.UpdateTileSize()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DashboardId | int | Yes | Dashboard identifier |
| DashboardTileId | int | Yes | Dashboard tile identifier |
| Width | int | Yes | New width in grid units |
| Height | int | Yes | New height in grid units |

#### API Endpoint
```
PUT /api/dashboards/{dashboardId}/tiles/{dashboardTileId}/size
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "width": 4,
    "height": 2
}
```

#### Response
```json
{
    "dashboardTileId": 1,
    "width": 4,
    "height": 2,
    "message": "Tile size updated"
}
```

---

### 11. Hide Tile from Catalog

**Purpose**: Hide a tile from the tile selection catalog.

**Domain Operation**: `Dashboard.HideFromCatalog()`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| TileId | int | Yes | Tile identifier |

#### API Endpoint
```
POST /api/tiles/{tileId}/hide
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "tileId": 1,
    "hiddenFromCatalog": true,
    "message": "Tile hidden from catalog"
}
```

---

## Tile Template Management

### 12. Create Tile

**Purpose**: Create a new tile template.

**Command**: `CreateTileCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Tile name |
| WidgetType | string | Yes | Widget type identifier |
| DefaultWidth | int | Yes | Default width |
| DefaultHeight | int | Yes | Default height |
| Configuration | object | No | Widget configuration |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/tiles
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Weekly Progress Chart",
    "widgetType": "ProgressChart",
    "defaultWidth": 4,
    "defaultHeight": 2,
    "configuration": {
        "chartType": "line",
        "metrics": ["weight", "workouts"],
        "period": "week"
    }
}
```

#### Response
```json
{
    "tileId": 1,
    "name": "Weekly Progress Chart",
    "widgetType": "ProgressChart",
    "defaultWidth": 4,
    "defaultHeight": 2,
    "createdOn": "2024-01-15T10:30:00Z"
}
```

---

### 13. Get Tiles

**Purpose**: Retrieve available tile templates.

**Query**: `GetTilesQuery`

#### API Endpoint
```
GET /api/tiles
Authorization: Bearer {accessToken}
```

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| includeHidden | bool | Include hidden tiles |

#### Response
```json
{
    "tiles": [
        {
            "tileId": 1,
            "name": "Weight Tracker",
            "widgetType": "WeightDisplay",
            "defaultWidth": 2,
            "defaultHeight": 1,
            "description": "Display current weight with trend"
        },
        {
            "tileId": 2,
            "name": "Weekly Progress",
            "widgetType": "ProgressChart",
            "defaultWidth": 4,
            "defaultHeight": 2,
            "description": "Line chart showing weekly progress"
        },
        {
            "tileId": 3,
            "name": "Workout Calendar",
            "widgetType": "Calendar",
            "defaultWidth": 3,
            "defaultHeight": 3,
            "description": "Calendar view of completed workouts"
        }
    ]
}
```

---

### 14. Update Tile

**Purpose**: Modify a tile template.

**Command**: `UpdateTileCommand`

#### API Endpoint
```
PUT /api/tiles/{tileId}
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Enhanced Progress Chart",
    "configuration": {
        "chartType": "area",
        "metrics": ["weight", "workouts", "calories"],
        "period": "month"
    }
}
```

#### Response
```json
{
    "tileId": 1,
    "name": "Enhanced Progress Chart",
    "lastModifiedOn": "2024-01-16T14:20:00Z"
}
```

---

### 15. Delete Tile

**Purpose**: Remove a tile template.

**Command**: `DeleteTileCommand`

#### API Endpoint
```
DELETE /api/tiles/{tileId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "message": "Tile deleted successfully"
}
```

---

## Widget Statistics

### 16. Get Dashboard Stats

**Purpose**: Retrieve aggregated statistics for dashboard display.

**Query**: `GetDashboardStatsQuery`

#### API Endpoint
```
GET /api/dashboard-stats
Authorization: Bearer {accessToken}
```

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| athleteId | int | Filter by athlete |
| period | string | Time period (week, month, year) |

#### Response
```json
{
    "athleteId": 1,
    "period": "month",
    "stats": {
        "currentWeight": 74,
        "weightChange": -2,
        "totalWorkouts": 16,
        "workoutsThisWeek": 4,
        "totalExercisesCompleted": 128,
        "averageWorkoutDuration": 45,
        "streakDays": 12,
        "personalRecords": [
            {
                "exerciseName": "Bench Press",
                "record": "70kg x 8 reps",
                "date": "2024-01-14"
            }
        ]
    }
}
```

---

### 17. Update Dashboard Stats

**Purpose**: Manually trigger stats recalculation.

**Command**: `UpdateDashboardStatsCommand`

#### API Endpoint
```
POST /api/dashboard-stats/refresh
Authorization: Bearer {accessToken}

{
    "athleteId": 1
}
```

#### Response
```json
{
    "message": "Dashboard statistics updated",
    "lastUpdated": "2024-01-16T14:30:00Z"
}
```

---

## Widget Management

### 18. Create Widget

**Purpose**: Create a new widget definition.

**Command**: `CreateWidgetCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Widget name |
| Type | string | Yes | Widget type |
| Component | string | Yes | UI component identifier |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/widgets
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Calorie Tracker",
    "type": "MetricDisplay",
    "component": "calorie-tracker-widget"
}
```

#### Response
```json
{
    "widgetId": 1,
    "name": "Calorie Tracker",
    "type": "MetricDisplay",
    "component": "calorie-tracker-widget"
}
```

---

### 19. Get Widgets

**Purpose**: Retrieve available widget definitions.

**Query**: `GetWidgetsQuery`

#### API Endpoint
```
GET /api/widgets
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "widgets": [
        {
            "widgetId": 1,
            "name": "Weight Display",
            "type": "MetricDisplay",
            "description": "Shows current weight with trend indicator"
        },
        {
            "widgetId": 2,
            "name": "Progress Chart",
            "type": "Chart",
            "description": "Line/area chart for progress visualization"
        },
        {
            "widgetId": 3,
            "name": "Workout Calendar",
            "type": "Calendar",
            "description": "Calendar view of workout history"
        },
        {
            "widgetId": 4,
            "name": "Recent Exercises",
            "type": "List",
            "description": "List of recently completed exercises"
        }
    ]
}
```

---

## Dashboard Layout Grid

```
+------------------------------------------+
|  Dashboard: Fitness Overview             |
+------------------------------------------+
|  [0,0]    [0,2]              [0,6]       |
|  +-----+  +----------------+  +-----+    |
|  |Weight|  | Progress Chart |  |Streak|   |
|  | 74kg |  |  ___/\___     |  | 12  |    |
|  +-----+  |       \__/     |  |days |    |
|           +----------------+  +-----+    |
|                                          |
|  [2,0]                        [2,4]      |
|  +----------------------+  +----------+  |
|  | Recent Exercises     |  | Calendar |  |
|  | - Bench Press 60kg   |  | Jan 2024 |  |
|  | - Squats 80kg        |  | M T W T F|  |
|  | - Deadlift 100kg     |  | x x x . x|  |
|  +----------------------+  +----------+  |
+------------------------------------------+

Grid Layout:
- Unit size: configurable (default 100px x 100px)
- Positions: top (row), left (column)
- Sizes: width (columns), height (rows)
```

---

## Widget Types

| Type | Description | Configuration Options |
|------|-------------|----------------------|
| WeightDisplay | Current weight with trend | showTrend, trendDays |
| ProgressChart | Line/area/bar charts | chartType, metrics, period |
| Calendar | Workout calendar | monthsToShow, highlightStyle |
| List | Recent items list | itemCount, itemType |
| Streak | Streak counter | streakType |
| Summary | Statistics summary | metrics[] |

---

## Validation Rules Summary

| Entity | Field | Validation |
|--------|-------|------------|
| Dashboard | Name | Required |
| Tile | Name | Required |
| Tile | DefaultWidth | Must be positive |
| Tile | DefaultHeight | Must be positive |
| DashboardTile | Top | Cannot be negative |
| DashboardTile | Left | Cannot be negative |
| DashboardTile | Width | Must be positive |
| DashboardTile | Height | Must be positive |

---

## Error Codes

| Code | Description |
|------|-------------|
| 400 | Invalid input data |
| 401 | Unauthorized access |
| 404 | Dashboard or Tile not found |
| 409 | Name already exists |
