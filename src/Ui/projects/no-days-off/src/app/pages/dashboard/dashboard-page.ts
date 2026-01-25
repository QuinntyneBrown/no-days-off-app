import { Component, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { DashboardHeader, Dashboard, DashboardTile } from './components/dashboard-header/dashboard-header';
import { DashboardGrid } from './components/dashboard-grid/dashboard-grid';
import { PlusButton } from '../../components/plus-button';
import { ModalWindow } from '../../components/modal-window';
import { DashboardService, Widget, DashboardStats, CreateWidgetRequest } from '../../../../../../src/app/core/services/dashboard.service';

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
export class DashboardPage implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private dashboardService = inject(DashboardService);

  dashboards = signal<Dashboard[]>([]);
  currentDashboard = signal<Dashboard | null>(null);
  stats = signal<DashboardStats | null>(null);
  widgets = signal<Widget[]>([]);
  isAddTileModalOpen = signal(false);
  isLoading = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    this.isLoading.set(true);
    this.error.set(null);

    // Load stats
    this.dashboardService.getStats().subscribe({
      next: (stats) => {
        this.stats.set(stats);
      },
      error: (err) => {
        console.error('Failed to load stats:', err);
      }
    });

    // Load widgets
    this.dashboardService.getWidgets().subscribe({
      next: (widgets) => {
        this.widgets.set(widgets);
        // Convert widgets to dashboard tiles for the current UI
        const dashboard = this.createDashboardFromWidgets(widgets);
        this.dashboards.set([dashboard]);
        this.currentDashboard.set(dashboard);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load widgets:', err);
        this.error.set('Failed to load dashboard. Please try again.');
        this.isLoading.set(false);
        // Show empty dashboard on error
        const emptyDashboard: Dashboard = {
          dashboardId: 1,
          name: 'My Dashboard',
          isDefault: true,
          dashboardTiles: []
        };
        this.dashboards.set([emptyDashboard]);
        this.currentDashboard.set(emptyDashboard);
      }
    });
  }

  private createDashboardFromWidgets(widgets: Widget[]): Dashboard {
    const tiles: DashboardTile[] = widgets.map((w, index) => ({
      dashboardTileId: w.widgetId,
      dashboardId: 1,
      tileId: w.widgetId,
      top: Math.floor(index / 3),
      left: index % 3,
      width: w.width,
      height: w.height
    }));

    return {
      dashboardId: 1,
      name: 'My Dashboard',
      isDefault: true,
      dashboardTiles: tiles
    };
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

  onAddWidget(request: CreateWidgetRequest): void {
    this.dashboardService.createWidget(request).subscribe({
      next: (widget) => {
        this.widgets.update(w => [...w, widget]);
        const dashboard = this.createDashboardFromWidgets(this.widgets());
        this.currentDashboard.set(dashboard);
        this.isAddTileModalOpen.set(false);
      },
      error: (err) => {
        console.error('Failed to create widget:', err);
        this.error.set('Failed to add widget. Please try again.');
      }
    });
  }

  onConfigureTile(tile: DashboardTile): void {
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
    console.log('Save tile:', tile);
  }

  get statsDisplay() {
    const s = this.stats();
    if (!s) return null;
    return {
      totalWorkouts: s.totalWorkouts,
      totalExercises: s.totalExercises,
      totalAthletes: s.totalAthletes,
      workoutsThisWeek: s.workoutsThisWeek,
      workoutsThisMonth: s.workoutsThisMonth
    };
  }
}
