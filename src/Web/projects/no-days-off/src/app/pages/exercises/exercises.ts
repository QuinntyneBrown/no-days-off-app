import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { NdoSearchBarComponent, NdoSnackbarService } from 'components';
import { ExercisesService, BodyPartsService } from 'api';
import type { Exercise, BodyPart } from 'api';
import { ExerciseDialog } from '../../dialogs/exercise-dialog';
import { ConfirmDialog } from '../../dialogs/confirm-dialog';

@Component({
  selector: 'app-exercises',
  imports: [MatButtonModule, MatIconModule, MatChipsModule, MatCardModule, MatDialogModule, NdoSearchBarComponent],
  template: `
    <div class="page" data-testid="exercises-page">
      <div class="page-header">
        <div>
          <h2 class="page-title">Exercise Library</h2>
          <p class="page-sub">{{ exercises().length }} exercises</p>
        </div>
        <div class="header-actions">
          <ndo-search-bar placeholder="Search exercises..." (valueChange)="onSearch($event)" />
          <button mat-flat-button class="primary-btn" (click)="openAdd()" data-testid="add-exercise-btn">
            <mat-icon fontSet="material-symbols-rounded">add</mat-icon>
            Add Exercise
          </button>
        </div>
      </div>

      <mat-chip-listbox (change)="onFilter($event.value)" data-testid="exercise-filters">
        <mat-chip-option value="all" selected>All</mat-chip-option>
        @for (bp of bodyParts(); track bp.bodyPartId) {
          <mat-chip-option [value]="bp.bodyPartId">{{ bp.name }}</mat-chip-option>
        }
      </mat-chip-listbox>

      <div class="exercise-grid" data-testid="exercise-grid">
        @for (ex of filtered(); track ex.exerciseId) {
          <mat-card class="exercise-card" [attr.data-testid]="'exercise-' + ex.exerciseId">
            <div class="card-img">
              <mat-icon fontSet="material-symbols-rounded" class="card-icon">fitness_center</mat-icon>
            </div>
            <mat-card-content>
              <h3 class="card-name">{{ ex.name }}</h3>
              <div class="card-meta">
                @if (getBodyPartName(ex.bodyPartId); as bpName) {
                  <span class="tag">{{ bpName }}</span>
                }
                @if (ex.defaultSets) {
                  <span class="meta-text">{{ ex.defaultSets }} sets</span>
                }
              </div>
            </mat-card-content>
            <mat-card-actions>
              <button mat-icon-button (click)="openEdit(ex)">
                <mat-icon fontSet="material-symbols-rounded">edit</mat-icon>
              </button>
              <button mat-icon-button class="delete-btn" (click)="confirmDelete(ex)">
                <mat-icon fontSet="material-symbols-rounded">delete</mat-icon>
              </button>
            </mat-card-actions>
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
    .header-actions { display: flex; gap: 12px; align-items: center; }
    .primary-btn {
      --mdc-filled-button-container-color: var(--ndo-primary);
      --mdc-filled-button-label-text-color: var(--ndo-text-on-primary);
    }
    .exercise-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 16px; }
    .exercise-card {
      --mdc-elevated-card-container-color: var(--ndo-bg-card);
      --mdc-elevated-card-container-elevation: none;
      border: 1px solid var(--ndo-border);
    }
    .card-img {
      height: 140px; display: flex; align-items: center; justify-content: center;
      background: var(--ndo-bg-elevated);
    }
    .card-icon { font-size: 48px; width: 48px; height: 48px; color: var(--ndo-text-tertiary); }
    .card-name { font-size: 16px; font-weight: 600; margin: 12px 0 8px; }
    .card-meta { display: flex; gap: 8px; align-items: center; }
    .tag {
      background: var(--ndo-primary-dim); color: var(--ndo-primary);
      padding: 2px 8px; font-size: 12px; font-weight: 500;
    }
    .meta-text { color: var(--ndo-text-secondary); font-size: 12px; }
    .delete-btn { color: var(--ndo-error); }
  `,
})
export class ExercisesPage implements OnInit {
  private readonly exercisesService = inject(ExercisesService);
  private readonly bodyPartsService = inject(BodyPartsService);
  private readonly dialog = inject(MatDialog);
  private readonly snackbar = inject(NdoSnackbarService);

  exercises = signal<Exercise[]>([]);
  filtered = signal<Exercise[]>([]);
  bodyParts = signal<BodyPart[]>([]);
  private searchTerm = '';
  private filterBp: number | null = null;

  ngOnInit() { this.load(); this.loadBodyParts(); }

  load() {
    this.exercisesService.getExercises().subscribe({
      next: (list) => { this.exercises.set(list); this.applyFilter(); },
      error: () => {},
    });
  }

  loadBodyParts() {
    this.bodyPartsService.getBodyParts().subscribe({
      next: (list) => this.bodyParts.set(list),
      error: () => {},
    });
  }

  getBodyPartName(id?: number): string {
    if (!id) return '';
    return this.bodyParts().find(bp => bp.bodyPartId === id)?.name ?? '';
  }

  onSearch(q: string) { this.searchTerm = q; this.applyFilter(); }

  onFilter(val: string) {
    this.filterBp = val === 'all' ? null : Number(val);
    this.applyFilter();
  }

  private applyFilter() {
    let list = this.exercises();
    if (this.filterBp) list = list.filter(e => e.bodyPartId === this.filterBp);
    if (this.searchTerm) {
      const t = this.searchTerm.toLowerCase();
      list = list.filter(e => e.name.toLowerCase().includes(t));
    }
    this.filtered.set(list);
  }

  openAdd() {
    const ref = this.dialog.open(ExerciseDialog, { width: '480px', data: { bodyParts: this.bodyParts() } });
    ref.afterClosed().subscribe(r => { if (r) this.load(); });
  }

  openEdit(ex: Exercise) {
    const ref = this.dialog.open(ExerciseDialog, { width: '480px', data: { exercise: ex, bodyParts: this.bodyParts() } });
    ref.afterClosed().subscribe(r => { if (r) this.load(); });
  }

  confirmDelete(ex: Exercise) {
    const ref = this.dialog.open(ConfirmDialog, {
      width: '400px', data: { title: 'Delete Exercise', message: `Delete "${ex.name}"?` },
    });
    ref.afterClosed().subscribe(ok => {
      if (ok) {
        this.exercisesService.deleteExercise(ex.exerciseId).subscribe({
          next: () => { this.snackbar.show('Exercise deleted', 'success'); this.load(); },
          error: () => this.snackbar.show('Failed to delete', 'error'),
        });
      }
    });
  }
}
