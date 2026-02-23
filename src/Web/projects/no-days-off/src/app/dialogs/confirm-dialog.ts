import { Component, inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

interface ConfirmData { title: string; message: string; }

@Component({
  selector: 'app-confirm-dialog',
  imports: [MatDialogModule, MatButtonModule, MatIconModule],
  template: `
    <div class="dialog" data-testid="confirm-dialog">
      <div class="dialog-header">
        <div class="icon-circle">
          <mat-icon fontSet="material-symbols-rounded">warning</mat-icon>
        </div>
        <h2>{{ data.title }}</h2>
      </div>
      <p class="dialog-message" data-testid="confirm-message">{{ data.message }}</p>
      <div class="dialog-actions">
        <button mat-button (click)="dialogRef.close(false)" data-testid="confirm-cancel">Cancel</button>
        <button mat-flat-button class="danger-btn" (click)="dialogRef.close(true)" data-testid="confirm-ok">
          Delete
        </button>
      </div>
    </div>
  `,
  styles: `
    .dialog { padding: 24px; text-align: center; }
    .dialog-header { margin-bottom: 12px; }
    .dialog-header h2 { font-family: 'Plus Jakarta Sans', sans-serif; margin: 12px 0 0; }
    .icon-circle {
      width: 56px; height: 56px; border-radius: 50%; display: inline-flex;
      align-items: center; justify-content: center;
      background: var(--ndo-error-dim); color: var(--ndo-error); margin: 0 auto;
    }
    .icon-circle mat-icon { font-size: 28px; width: 28px; height: 28px; }
    .dialog-message { color: var(--ndo-text-secondary); font-size: 14px; margin: 8px 0 24px; }
    .dialog-actions { display: flex; justify-content: center; gap: 12px; }
    .danger-btn { --mdc-filled-button-container-color: var(--ndo-error); --mdc-filled-button-label-text-color: #FFFFFF; }
  `,
})
export class ConfirmDialog {
  readonly dialogRef = inject(MatDialogRef<ConfirmDialog>);
  readonly data = inject<ConfirmData>(MAT_DIALOG_DATA);
}
