# Dashboard Management Feature

## Overview

The Dashboard Management feature provides customizable dashboards with configurable tiles for displaying workout statistics, progress tracking, and other fitness-related information.

## Domain Model

### Dashboard Aggregate

```
Dashboard
├── Name: string (max 256 chars)
├── Username: string
├── IsDefault: bool
└── Tiles: ICollection<DashboardTile>
```

### DashboardTile Entity

```
DashboardTile
├── Name: string (max 256 chars)
├── TileId: int
├── Top: int
├── Left: int
├── Width: int
└── Height: int
```

### Tile Aggregate

```
Tile
├── Name: string (max 256 chars)
├── DefaultWidth: int
├── DefaultHeight: int
└── IsVisibleInCatalog: bool
```

## Features

### Dashboard Creation

- Create personalized dashboards
- Set default dashboard for users
- Multiple dashboards per user

### Tile Management

- Add tiles with custom positions and sizes
- Reposition and resize tiles
- Remove tiles from dashboard

### Tile Catalog

- Define reusable tile templates
- Control tile visibility in catalog
- Default dimensions for tiles

## Usage

### Creating a Tile Template

```csharp
var weightProgressTile = Tile.Create(
    tenantId: 1,
    name: "Weight Progress Chart",
    defaultWidth: 300,
    defaultHeight: 200,
    createdBy: "admin"
);

// Hide from catalog if needed
weightProgressTile.HideFromCatalog(modifiedBy: "admin");
```

### Creating a Dashboard

```csharp
var dashboard = Dashboard.Create(
    tenantId: 1,
    name: "My Fitness Dashboard",
    username: "johndoe",
    isDefault: true,
    createdBy: "johndoe"
);
```

### Adding Tiles to Dashboard

```csharp
dashboard.AddTile(
    tileId: 1, // Weight Progress Chart
    name: "My Weight Progress",
    top: 0,
    left: 0,
    width: 300,
    height: 200,
    modifiedBy: "johndoe"
);

dashboard.AddTile(
    tileId: 2, // Workout Calendar
    name: "Workout Calendar",
    top: 0,
    left: 310,
    width: 400,
    height: 300,
    modifiedBy: "johndoe"
);
```

### Repositioning Tiles

```csharp
// Move first tile to new position
dashboard.UpdateTilePosition(
    index: 0,
    top: 220,
    left: 0,
    modifiedBy: "johndoe"
);
```

### Resizing Tiles

```csharp
dashboard.UpdateTileSize(
    index: 0,
    width: 400,
    height: 250,
    modifiedBy: "johndoe"
);
```

### Managing Default Dashboard

```csharp
// Set as default
dashboard.SetAsDefault(modifiedBy: "johndoe");

// Clear default flag
dashboard.ClearDefault(modifiedBy: "johndoe");
```

## Validation Rules

| Entity | Field | Validation |
|--------|-------|------------|
| Dashboard | Name | Required, max 256 characters |
| DashboardTile | Name | Required, max 256 characters |
| DashboardTile | Width | Must be greater than 0 |
| DashboardTile | Height | Must be greater than 0 |
| Tile | Name | Required, max 256 characters |
| Tile | DefaultWidth | Must be greater than 0 |
| Tile | DefaultHeight | Must be greater than 0 |

## Example Dashboard Layout

```
+------------------+-------------------------+
|  Weight Progress |     Workout Calendar    |
|     (300x200)    |        (400x300)        |
+------------------+                         |
|  Today's Goals   |                         |
|     (300x100)    +-------------------------+
+------------------+-------------------------+
|        Weekly Workout Summary (710x200)    |
+--------------------------------------------+
```

## Relationships

```
Dashboard (1) --- (*) DashboardTile
Tile (1) --- (*) DashboardTile
User (1) --- (*) Dashboard
```
