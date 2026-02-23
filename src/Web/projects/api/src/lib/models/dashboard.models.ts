export interface DashboardStats {
  tenantId: number;
  userId: number;
  totalWorkouts: number;
  totalExercises: number;
  totalAthletes: number;
  workoutsThisWeek: number;
  workoutsThisMonth: number;
  lastUpdated: string;
}

export interface Widget {
  widgetId: number;
  name: string;
  type: number;
  tenantId: number;
  userId: number;
  position: number;
  width: number;
  height: number;
  configuration?: string;
  isVisible: boolean;
}

export interface CreateWidgetRequest {
  name: string;
  type: number;
  position: number;
  width?: number;
  height?: number;
  configuration?: string;
}

export interface Dashboard {
  dashboardId: number;
  name: string;
  username: string;
  isDefault: boolean;
  tiles: DashboardTile[];
  tenantId?: number;
  createdOn: string;
}

export interface DashboardTile {
  dashboardTileId: number;
  tileId: number;
  row: number;
  column: number;
  width: number;
  height: number;
}

export interface CreateDashboardRequest {
  name: string;
  username: string;
  isDefault?: boolean;
  tenantId?: number;
}

export interface UpdateDashboardRequest {
  dashboardId: number;
  name: string;
  isDefault: boolean;
}

export interface Tile {
  tileId: number;
  name: string;
  type: string;
  configuration?: string;
  tenantId?: number;
  createdOn: string;
}

export interface CreateTileRequest {
  name: string;
  type: string;
  configuration?: string;
  tenantId?: number;
}

export interface UpdateTileRequest {
  tileId: number;
  name: string;
  type: string;
  configuration?: string;
}
