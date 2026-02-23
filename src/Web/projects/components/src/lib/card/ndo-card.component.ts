import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'ndo-card',
  standalone: true,
  imports: [MatCardModule],
  template: `
    <mat-card class="ndo-card">
      <mat-card-header class="ndo-card-header">
        <ng-content select="[cardHeader]" />
      </mat-card-header>
      <mat-card-content class="ndo-card-content">
        <ng-content />
      </mat-card-content>
      <mat-card-actions class="ndo-card-actions" align="end">
        <ng-content select="[cardActions]" />
      </mat-card-actions>
    </mat-card>
  `,
  styles: `
    .ndo-card {
      --mdc-elevated-card-container-color: var(--ndo-bg-card, #1A1A1A);
      --mdc-elevated-card-container-elevation: none;
      border: 1px solid var(--ndo-border, #2A2A2A);
      border-radius: 0;
      width: 320px;
      font-family: 'DM Sans', sans-serif;
    }

    .ndo-card-header {
      padding: 16px 20px;
    }

    .ndo-card-content {
      padding: 0 20px 16px;
    }

    .ndo-card-actions {
      padding: 12px 20px;
      border-top: 1px solid var(--ndo-border, #2A2A2A);
    }
  `
})
export class NdoCardComponent {}
