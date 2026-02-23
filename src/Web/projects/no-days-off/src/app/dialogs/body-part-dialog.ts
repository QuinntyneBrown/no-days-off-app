import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { BodyPartsService } from 'api';

@Component({
  selector: 'app-body-part-dialog',
  imports: [FormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule],
  template: `
    <div class="dialog" data-testid="body-part-dialog">
      <div class="dialog-header">
        <h2>Add Body Part</h2>
        <button mat-icon-button (click)="close()">
          <mat-icon fontSet="material-symbols-rounded">close</mat-icon>
        </button>
      </div>
      <div class="dialog-content">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Body Part Name</mat-label>
          <input matInput [(ngModel)]="name" data-testid="body-part-name-input" required>
        </mat-form-field>
        @if (error()) { <p class="error-text">{{ error() }}</p> }
      </div>
      <div class="dialog-actions">
        <button mat-button (click)="close()">Cancel</button>
        <button mat-flat-button class="primary-btn" (click)="save()" [disabled]="loading()"
                data-testid="body-part-save-btn">
          Create
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
    .error-text { color: var(--ndo-error); font-size: 13px; }
    .dialog-actions { display: flex; justify-content: flex-end; gap: 8px; margin-top: 20px; }
    .primary-btn { --mdc-filled-button-container-color: var(--ndo-primary); --mdc-filled-button-label-text-color: var(--ndo-text-on-primary); }
  `,
})
export class BodyPartDialog {
  private readonly dialogRef = inject(MatDialogRef<BodyPartDialog>);
  private readonly bodyPartsService = inject(BodyPartsService);
  name = '';
  loading = signal(false);
  error = signal('');

  close() { this.dialogRef.close(); }

  save() {
    if (!this.name) { this.error.set('Name is required'); return; }
    this.loading.set(true);
    this.bodyPartsService.createBodyPart({ name: this.name }).subscribe({
      next: () => this.dialogRef.close(true),
      error: (err) => { this.loading.set(false); this.error.set(err?.error?.message || 'Failed'); },
    });
  }
}
