import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';

export interface Dashboard {
  dashboardId: number;
  name: string;
  isDefault: boolean;
  dashboardTiles: DashboardTile[];
}

export interface DashboardTile {
  dashboardTileId: number;
  dashboardId: number;
  tileId: number;
  top: number;
  left: number;
  width: number;
  height: number;
}

@Component({
  selector: 'ndo-dashboard-header',
  standalone: true,
  imports: [CommonModule, MatToolbarModule, MatButtonModule, MatIconModule, MatMenuModule],
  templateUrl: './dashboard-header.html',
  styleUrl: './dashboard-header.scss'
})
export class DashboardHeader {
  dashboards = input<Dashboard[]>([]);
  currentDashboard = input<Dashboard | null>(null);

  addDashboard = output<void>();
  selectDashboard = output<Dashboard>();
  deleteDashboard = output<Dashboard>();

  onAddDashboard(): void {
    this.addDashboard.emit();
  }

  onSelectDashboard(dashboard: Dashboard): void {
    this.selectDashboard.emit(dashboard);
  }

  onDeleteDashboard(dashboard: Dashboard): void {
    this.deleteDashboard.emit(dashboard);
  }
}
