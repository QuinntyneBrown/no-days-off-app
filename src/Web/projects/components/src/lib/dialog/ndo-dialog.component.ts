import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'ndo-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatIconModule, MatButtonModule],
  template: `
    <div class="ndo-dialog">
      <div class="ndo-dialog-header">
        <h2 class="ndo-dialog-title">{{ title }}</h2>
        <button mat-icon-button (click)="closed.emit()">
          <mat-icon [fontSet]="'material-symbols-rounded'">close</mat-icon>
        </button>
      </div>
      <div class="ndo-dialog-content">
        <ng-content />
      </div>
      <div class="ndo-dialog-actions">
        <ng-content select="[dialogActions]" />
      </div>
    </div>
  `,
  styles: `
    .ndo-dialog {
      background-color: var(--ndo-bg-card, #1A1A1A);
      border: 1px solid var(--ndo-border, #2A2A2A);
      width: 480px;
      font-family: 'DM Sans', sans-serif;
    }

    .ndo-dialog-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 20px 24px;
      border-bottom: 1px solid var(--ndo-border, #2A2A2A);
    }

    .ndo-dialog-title {
      font-family: 'Plus Jakarta Sans', sans-serif;
      font-size: 18px;
      font-weight: 700;
      color: var(--ndo-text-primary, #FFFFFF);
      margin: 0;
    }

    .ndo-dialog-header button {
      --mdc-icon-button-icon-color: var(--ndo-text-secondary, #9E9E9E);
    }

    .ndo-dialog-content {
      padding: 24px;
    }

    .ndo-dialog-actions {
      display: flex;
      justify-content: flex-end;
      gap: 12px;
      padding: 16px 24px;
      border-top: 1px solid var(--ndo-border, #2A2A2A);
    }
  `
})
export class NdoDialogComponent {
  @Input() title = 'Dialog Title';
  @Output() closed = new EventEmitter<void>();
}
