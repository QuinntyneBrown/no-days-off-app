import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardTile } from '../dashboard-header/dashboard-header';
import { DashboardTileComponent } from '../dashboard-tile/dashboard-tile';

@Component({
  selector: 'ndo-dashboard-grid',
  standalone: true,
  imports: [CommonModule, DashboardTileComponent],
  templateUrl: './dashboard-grid.html',
  styleUrl: './dashboard-grid.scss'
})
export class DashboardGrid {
  dashboardTiles = input<DashboardTile[]>([]);

  configureTile = output<DashboardTile>();
  removeTile = output<DashboardTile>();
  saveTile = output<DashboardTile>();

  onConfigureTile(tile: DashboardTile): void {
    this.configureTile.emit(tile);
  }

  onRemoveTile(tile: DashboardTile): void {
    this.removeTile.emit(tile);
  }

  onSaveTile(tile: DashboardTile): void {
    this.saveTile.emit(tile);
  }
}
