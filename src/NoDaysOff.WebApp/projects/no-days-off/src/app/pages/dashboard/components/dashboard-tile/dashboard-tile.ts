import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { DashboardTile } from '../dashboard-header/dashboard-header';

@Component({
  selector: 'ndo-dashboard-tile',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, MatMenuModule],
  templateUrl: './dashboard-tile.html',
  styleUrl: './dashboard-tile.scss'
})
export class DashboardTileComponent {
  tile = input.required<DashboardTile>();

  configure = output<DashboardTile>();
  remove = output<DashboardTile>();
  save = output<DashboardTile>();

  onConfigure(): void {
    this.configure.emit(this.tile());
  }

  onRemove(): void {
    this.remove.emit(this.tile());
  }

  onSave(): void {
    this.save.emit(this.tile());
  }
}
