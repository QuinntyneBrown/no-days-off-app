import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AthletesService } from 'api';

@Component({
  selector: 'app-log-weight-dialog',
  imports: [FormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule],
  template: `
    <div class="dialog" data-testid="log-weight-dialog">
      <div class="dialog-header">
        <h2>Log Weight</h2>
        <button mat-icon-button (click)="close()">
          <mat-icon fontSet="material-symbols-rounded">close</mat-icon>
        </button>
      </div>
      <div class="dialog-content">
        <p class="athlete-name" data-testid="log-weight-athlete-name">{{ data.athleteName }}</p>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Weight (kg)</mat-label>
          <input matInput type="number" [(ngModel)]="weight" data-testid="log-weight-input" required>
        </mat-form-field>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Date</mat-label>
          <input matInput type="date" [(ngModel)]="date" data-testid="log-weight-date" required>
        </mat-form-field>
        @if (error()) {
          <p class="error-text">{{ error() }}</p>
        }
      </div>
      <div class="dialog-actions">
        <button mat-button (click)="close()">Cancel</button>
        <button mat-flat-button class="primary-btn" (click)="save()" [disabled]="loading()"
                data-testid="log-weight-save-btn">
          Save
        </button>
      </div>
    </div>
  `,
  styles: `
    .dialog { padding: 24px; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
    .dialog-header h2 { font-family: 'Plus Jakarta Sans', sans-serif; margin: 0; }
    .dialog-content { display: flex; flex-direction: column; gap: 4px; }
    .full-width { width: 100%; }
    .athlete-name { font-weight: 600; margin: 0 0 12px; }
    .error-text { color: var(--ndo-error); font-size: 13px; }
    .dialog-actions { display: flex; justify-content: flex-end; gap: 8px; margin-top: 20px; }
    .primary-btn { --mdc-filled-button-container-color: var(--ndo-primary); --mdc-filled-button-label-text-color: var(--ndo-text-on-primary); }
  `,
})
export class LogWeightDialog {
  private readonly dialogRef = inject(MatDialogRef<LogWeightDialog>);
  readonly data = inject<{ athleteId: number; athleteName: string }>(MAT_DIALOG_DATA);
  private readonly athletesService = inject(AthletesService);

  weight: number | null = null;
  date = new Date().toISOString().substring(0, 10);
  loading = signal(false);
  error = signal('');

  close() { this.dialogRef.close(); }

  save() {
    if (!this.weight || this.weight <= 0) { this.error.set('Weight must be greater than zero'); return; }
    if (!this.date) { this.error.set('Date is required'); return; }
    this.loading.set(true);
    this.athletesService.recordWeight(this.data.athleteId, {
      weightInKgs: this.weight,
      weighedOn: new Date(this.date).toISOString(),
    }).subscribe({
      next: () => this.dialogRef.close(true),
      error: (err) => { this.loading.set(false); this.error.set(err?.error?.message || 'Failed to log weight'); },
    });
  }
}
