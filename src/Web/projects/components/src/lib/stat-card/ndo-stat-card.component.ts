import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ndo-stat-card',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule],
  template: `
    <mat-card class="ndo-stat-card">
      <mat-card-content>
        <div class="ndo-stat-header">
          <span class="ndo-stat-label">{{ label }}</span>
          <div class="ndo-stat-icon-wrap">
            <mat-icon [fontSet]="'material-symbols-rounded'">{{ icon }}</mat-icon>
          </div>
        </div>
        <div class="ndo-stat-value">{{ value }}</div>
        <div class="ndo-stat-change">
          <mat-icon [fontSet]="'material-symbols-rounded'" class="ndo-stat-arrow" [class.ndo-stat-down]="isNegative">
            {{ isNegative ? 'trending_down' : 'trending_up' }}
          </mat-icon>
          <span class="ndo-stat-pct" [class.ndo-stat-down]="isNegative">{{ changePercent }}</span>
          <span class="ndo-stat-period">{{ period }}</span>
        </div>
      </mat-card-content>
    </mat-card>
  `,
  styles: `
    :host { display: block; width: 100%; }

    .ndo-stat-card {
      --mdc-elevated-card-container-color: var(--ndo-bg-card, #1A1A1A);
      --mdc-elevated-card-container-elevation: none;
      border: 1px solid var(--ndo-border, #2A2A2A);
      border-radius: 0;
      font-family: 'DM Sans', sans-serif;
    }

    .ndo-stat-card mat-card-content {
      display: flex;
      flex-direction: column;
      gap: 12px;
      padding: 20px;
    }

    .ndo-stat-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .ndo-stat-label {
      font-size: 12px;
      font-weight: 500;
      color: var(--ndo-text-secondary, #9E9E9E);
    }

    .ndo-stat-icon-wrap {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 36px;
      height: 36px;
      background-color: var(--ndo-primary-dim, #00E5FF18);
      color: var(--ndo-primary, #00E5FF);
    }

    .ndo-stat-icon-wrap mat-icon {
      font-size: 20px;
      width: 20px;
      height: 20px;
    }

    .ndo-stat-value {
      font-family: 'Plus Jakarta Sans', sans-serif;
      font-size: 28px;
      font-weight: 700;
      color: var(--ndo-text-primary, #FFFFFF);
    }

    .ndo-stat-change {
      display: flex;
      align-items: center;
      gap: 4px;
    }

    .ndo-stat-arrow {
      font-size: 16px;
      width: 16px;
      height: 16px;
      color: var(--ndo-success, #4ADE80);
    }

    .ndo-stat-pct {
      font-size: 12px;
      font-weight: 500;
      color: var(--ndo-success, #4ADE80);
    }

    .ndo-stat-down { color: var(--ndo-error, #F87171); }

    .ndo-stat-period {
      font-size: 12px;
      color: var(--ndo-text-tertiary, #616161);
    }
  `
})
export class NdoStatCardComponent {
  @Input() label = 'Total Workouts';
  @Input() value = '1,284';
  @Input() icon = 'fitness_center';
  @Input() changePercent = '+12.5%';
  @Input() period = 'vs last month';

  get isNegative() { return this.changePercent.startsWith('-'); }
}
