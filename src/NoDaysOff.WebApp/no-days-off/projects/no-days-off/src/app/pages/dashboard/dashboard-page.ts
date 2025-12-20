import { Component, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { DashboardHeader, Dashboard, DashboardTile } from './components/dashboard-header/dashboard-header';
import { DashboardGrid } from './components/dashboard-grid/dashboard-grid';
import { PlusButton } from '../../components/plus-button';
import { ModalWindow } from '../../components/modal-window';

@Component({
  selector: 'ndo-dashboard-page',
  standalone: true,
  imports: [
    CommonModule,
    DashboardHeader,
    DashboardGrid,
    PlusButton,
    ModalWindow
  ],
  templateUrl: './dashboard-page.html',
  styleUrl: './dashboard-page.scss'
})
export class DashboardPage {
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  dashboards = signal<Dashboard[]>([]);
  currentDashboard = signal<Dashboard | null>(null);
  isAddTileModalOpen = signal(false);

  constructor() {
    // Load initial data
    this.loadDashboards();
  }

  private loadDashboards(): void {
    // TODO: Replace with actual service call
    const mockDashboards: Dashboard[] = [
      {
        dashboardId: 1,
        name: 'My Dashboard',
        isDefault: true,
        dashboardTiles: [
          { dashboardTileId: 1, dashboardId: 1, tileId: 1, top: 0, left: 0, width: 1, height: 1 },
          { dashboardTileId: 2, dashboardId: 1, tileId: 2, top: 0, left: 1, width: 1, height: 1 }
        ]
      }
    ];
    this.dashboards.set(mockDashboards);
    this.currentDashboard.set(mockDashboards[0]);
  }

  onAddDashboard(): void {
    const newDashboard: Dashboard = {
      dashboardId: Date.now(),
      name: 'New Dashboard',
      isDefault: false,
      dashboardTiles: []
    };
    this.dashboards.update(d => [...d, newDashboard]);
    this.currentDashboard.set(newDashboard);
  }

  onSelectDashboard(dashboard: Dashboard): void {
    this.currentDashboard.set(dashboard);
  }

  onDeleteDashboard(dashboard: Dashboard): void {
    this.dashboards.update(d => d.filter(db => db.dashboardId !== dashboard.dashboardId));
    if (this.currentDashboard()?.dashboardId === dashboard.dashboardId) {
      this.currentDashboard.set(this.dashboards()[0] ?? null);
    }
    this.router.navigate(['/dashboard']);
  }

  onOpenTileCatalog(): void {
    this.isAddTileModalOpen.set(true);
  }

  onCloseTileModal(): void {
    this.isAddTileModalOpen.set(false);
  }

  onConfigureTile(tile: DashboardTile): void {
    // TODO: Open configure tile modal
    console.log('Configure tile:', tile);
  }

  onRemoveTile(tile: DashboardTile): void {
    const dashboard = this.currentDashboard();
    if (dashboard) {
      const updatedTiles = dashboard.dashboardTiles.filter(
        t => t.dashboardTileId !== tile.dashboardTileId
      );
      this.currentDashboard.set({ ...dashboard, dashboardTiles: updatedTiles });
    }
  }

  onSaveTile(tile: DashboardTile): void {
    // TODO: Save tile position/size
    console.log('Save tile:', tile);
  }
}
