import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { NdoSnackbarService } from 'components';
import { DaysService, BodyPartsService } from 'api';
import type { Day, BodyPart } from 'api';
import { WorkoutDayDialog } from '../../dialogs/workout-day-dialog';
import { ConfirmDialog } from '../../dialogs/confirm-dialog';

@Component({
  selector: 'app-workout-days',
  imports: [MatButtonModule, MatIconModule, MatCardModule, MatChipsModule, MatDialogModule],
  template: `
    <div class="page" data-testid="workout-days-page">
      <div class="page-header">
        <div>
          <h2 class="page-title">Workout Days</h2>
          <p class="page-sub">{{ days().length }} workout days configured</p>
        </div>
        <button mat-flat-button class="primary-btn" (click)="openAdd()" data-testid="add-day-btn">
          <mat-icon fontSet="material-symbols-rounded">add</mat-icon>
          Add Workout Day
        </button>
      </div>
      <div class="days-list" data-testid="days-list">
        @for (day of days(); track day.dayId) {
          <mat-card class="day-card" [attr.data-testid]="'day-' + day.dayId">
            <mat-card-content>
              <div class="day-row">
                <div class="day-info">
                  <h3 class="day-name">{{ day.name }}</h3>
                  <div class="day-tags">
                    @for (bpId of day.bodyPartIds; track bpId) {
                      <span class="bp-tag">{{ getBodyPartName(bpId) }}</span>
                    }
                  </div>
                </div>
                <div class="day-actions">
                  <button mat-icon-button (click)="openEdit(day)">
                    <mat-icon fontSet="material-symbols-rounded">edit</mat-icon>
                  </button>
                  <button mat-icon-button class="delete-btn" (click)="confirmDelete(day)">
                    <mat-icon fontSet="material-symbols-rounded">delete</mat-icon>
                  </button>
                </div>
              </div>
            </mat-card-content>
          </mat-card>
        }
      </div>
    </div>
  `,
  styles: `
    .page { display: flex; flex-direction: column; gap: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; flex-wrap: wrap; gap: 16px; }
    .page-title { font-family: 'Plus Jakarta Sans', sans-serif; font-size: 20px; font-weight: 700; margin: 0; }
    .page-sub { color: var(--ndo-text-secondary); font-size: 14px; margin: 4px 0 0; }
    .primary-btn { --mdc-filled-button-container-color: var(--ndo-primary); --mdc-filled-button-label-text-color: var(--ndo-text-on-primary); }
    .days-list { display: flex; flex-direction: column; gap: 12px; }
    .day-card { --mdc-elevated-card-container-color: var(--ndo-bg-card); --mdc-elevated-card-container-elevation: none; border: 1px solid var(--ndo-border); }
    .day-row { display: flex; justify-content: space-between; align-items: center; }
    .day-name { font-size: 16px; font-weight: 600; margin: 0 0 8px; }
    .day-tags { display: flex; gap: 6px; flex-wrap: wrap; }
    .bp-tag { background: var(--ndo-primary-dim); color: var(--ndo-primary); padding: 2px 10px; font-size: 12px; font-weight: 500; }
    .day-actions { display: flex; gap: 4px; }
    .delete-btn { color: var(--ndo-error); }
  `,
})
export class WorkoutDaysPage implements OnInit {
  private readonly daysService = inject(DaysService);
  private readonly bodyPartsService = inject(BodyPartsService);
  private readonly dialog = inject(MatDialog);
  private readonly snackbar = inject(NdoSnackbarService);

  days = signal<Day[]>([]);
  bodyParts = signal<BodyPart[]>([]);

  ngOnInit() { this.load(); this.bodyPartsService.getBodyParts().subscribe({ next: bp => this.bodyParts.set(bp), error: () => {} }); }

  load() { this.daysService.getDays().subscribe({ next: d => this.days.set(d), error: () => {} }); }

  getBodyPartName(id: number) { return this.bodyParts().find(bp => bp.bodyPartId === id)?.name ?? ''; }

  openAdd() {
    const ref = this.dialog.open(WorkoutDayDialog, { width: '480px', data: { bodyParts: this.bodyParts() } });
    ref.afterClosed().subscribe(r => { if (r) this.load(); });
  }

  openEdit(day: Day) {
    const ref = this.dialog.open(WorkoutDayDialog, { width: '480px', data: { day, bodyParts: this.bodyParts() } });
    ref.afterClosed().subscribe(r => { if (r) this.load(); });
  }

  confirmDelete(day: Day) {
    const ref = this.dialog.open(ConfirmDialog, { width: '400px', data: { title: 'Delete Workout Day', message: `Delete "${day.name}"?` } });
    ref.afterClosed().subscribe(ok => {
      if (ok) this.daysService.deleteDay(day.dayId).subscribe({
        next: () => { this.snackbar.show('Deleted', 'success'); this.load(); },
        error: () => this.snackbar.show('Failed', 'error'),
      });
    });
  }
}
