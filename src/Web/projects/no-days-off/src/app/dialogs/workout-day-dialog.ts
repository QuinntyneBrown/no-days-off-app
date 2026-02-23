import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { DaysService } from 'api';
import type { Day, BodyPart } from 'api';

interface DialogData { day?: Day; bodyParts: BodyPart[]; }

@Component({
  selector: 'app-workout-day-dialog',
  imports: [FormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatCheckboxModule],
  template: `
    <div class="dialog" data-testid="workout-day-dialog">
      <div class="dialog-header">
        <h2>{{ isEdit ? 'Edit Workout Day' : 'Add Workout Day' }}</h2>
        <button mat-icon-button (click)="close()">
          <mat-icon fontSet="material-symbols-rounded">close</mat-icon>
        </button>
      </div>
      <div class="dialog-content">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Day Name</mat-label>
          <input matInput [(ngModel)]="name" data-testid="day-name-input" required>
        </mat-form-field>
        <p class="label">Body Parts</p>
        @for (bp of data.bodyParts; track bp.bodyPartId) {
          <mat-checkbox [checked]="selectedBps.has(bp.bodyPartId)"
                        (change)="toggleBp(bp.bodyPartId, $event.checked)">
            {{ bp.name }}
          </mat-checkbox>
        }
        @if (error()) { <p class="error-text">{{ error() }}</p> }
      </div>
      <div class="dialog-actions">
        <button mat-button (click)="close()">Cancel</button>
        <button mat-flat-button class="primary-btn" (click)="save()" [disabled]="loading()"
                data-testid="day-save-btn">
          {{ isEdit ? 'Update' : 'Create' }}
        </button>
      </div>
    </div>
  `,
  styles: `
    .dialog { padding: 24px; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
    .dialog-header h2 { font-family: 'Plus Jakarta Sans', sans-serif; margin: 0; }
    .dialog-content { display: flex; flex-direction: column; gap: 8px; }
    .full-width { width: 100%; }
    .label { font-size: 14px; font-weight: 500; margin: 8px 0 4px; color: var(--ndo-text-secondary); }
    .error-text { color: var(--ndo-error); font-size: 13px; }
    .dialog-actions { display: flex; justify-content: flex-end; gap: 8px; margin-top: 20px; }
    .primary-btn { --mdc-filled-button-container-color: var(--ndo-primary); --mdc-filled-button-label-text-color: var(--ndo-text-on-primary); }
  `,
})
export class WorkoutDayDialog {
  private readonly dialogRef = inject(MatDialogRef<WorkoutDayDialog>);
  readonly data = inject<DialogData>(MAT_DIALOG_DATA);
  private readonly daysService = inject(DaysService);

  isEdit = !!this.data.day;
  name = this.data.day?.name ?? '';
  selectedBps = new Set<number>(this.data.day?.bodyPartIds ?? []);
  loading = signal(false);
  error = signal('');

  close() { this.dialogRef.close(); }

  toggleBp(id: number, checked: boolean | null) {
    if (checked) this.selectedBps.add(id);
    else this.selectedBps.delete(id);
  }

  save() {
    if (!this.name) { this.error.set('Name is required'); return; }
    this.loading.set(true);
    const bodyPartIds = [...this.selectedBps];
    const obs = this.isEdit
      ? this.daysService.updateDay(this.data.day!.dayId, { dayId: this.data.day!.dayId, name: this.name, bodyPartIds })
      : this.daysService.createDay({ name: this.name, bodyPartIds });
    obs.subscribe({
      next: () => this.dialogRef.close(true),
      error: (err) => { this.loading.set(false); this.error.set(err?.error?.message || 'Failed'); },
    });
  }
}
