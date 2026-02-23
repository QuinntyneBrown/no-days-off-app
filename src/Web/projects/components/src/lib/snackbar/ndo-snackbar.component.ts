import { Component, Injectable, Input, Output, EventEmitter, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

export type NdoSnackbarType = 'success' | 'error' | 'warning' | 'info';

const SNACKBAR_CONFIG: Record<NdoSnackbarType, { icon: string; colorVar: string; dimVar: string }> = {
  success: { icon: 'check_circle', colorVar: '--ndo-success', dimVar: '--ndo-success-dim' },
  error: { icon: 'error', colorVar: '--ndo-error', dimVar: '--ndo-error-dim' },
  warning: { icon: 'warning', colorVar: '--ndo-warning', dimVar: '--ndo-warning-dim' },
  info: { icon: 'info', colorVar: '--ndo-info', dimVar: '--ndo-info-dim' }
};

const SNACKBAR_COLORS: Record<NdoSnackbarType, { color: string; dim: string }> = {
  success: { color: '#4ADE80', dim: '#4ADE8020' },
  error: { color: '#F87171', dim: '#F8717120' },
  warning: { color: '#FBBF24', dim: '#FBBF2420' },
  info: { color: '#60A5FA', dim: '#60A5FA20' }
};

@Component({
  selector: 'ndo-snackbar',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule],
  template: `
    <div class="ndo-snackbar" [style.background-color]="bgColor" [style.border-left-color]="accentColor">
      <mat-icon [fontSet]="'material-symbols-rounded'" [style.color]="accentColor">{{ config.icon }}</mat-icon>
      <span class="ndo-snackbar-message">{{ message }}</span>
      <button mat-icon-button class="ndo-snackbar-close" (click)="dismissed.emit()">
        <mat-icon [fontSet]="'material-symbols-rounded'">close</mat-icon>
      </button>
    </div>
  `,
  styles: `
    :host { display: block; }

    .ndo-snackbar {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 14px 20px;
      border-left: 4px solid;
      width: 360px;
      font-family: 'DM Sans', sans-serif;
    }

    .ndo-snackbar-message {
      flex: 1;
      font-size: 14px;
      font-weight: 500;
      color: var(--ndo-text-primary, #FFFFFF);
    }

    .ndo-snackbar-close {
      --mdc-icon-button-icon-color: var(--ndo-text-secondary, #9E9E9E);
      width: 32px;
      height: 32px;
    }

    .ndo-snackbar-close mat-icon {
      font-size: 20px;
      width: 20px;
      height: 20px;
    }
  `
})
export class NdoSnackbarComponent {
  @Input() type: NdoSnackbarType = 'success';
  @Input() message = 'Successfully saved!';
  @Output() dismissed = new EventEmitter<void>();

  get config() { return SNACKBAR_CONFIG[this.type]; }
  get accentColor() { return SNACKBAR_COLORS[this.type].color; }
  get bgColor() { return SNACKBAR_COLORS[this.type].dim; }
}

/**
 * Service to show snackbar notifications using Angular Material's MatSnackBar.
 * Usage: inject(NdoSnackbarService).show('Message', 'success');
 */
@Injectable({ providedIn: 'root' })
export class NdoSnackbarService {
  private snackBar = inject(MatSnackBar);

  show(message: string, type: NdoSnackbarType = 'info', duration = 4000) {
    this.snackBar.openFromComponent(NdoSnackbarComponent, {
      duration,
      data: { message, type },
      panelClass: [`ndo-snackbar-${type}`],
      horizontalPosition: 'end',
      verticalPosition: 'bottom'
    });
  }
}
