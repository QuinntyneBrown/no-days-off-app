import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { NdoAvatarComponent, NdoSearchBarComponent, NdoSnackbarService } from 'components';
import { AthletesService } from 'api';
import type { Athlete } from 'api';
import { AthleteDialog } from '../../dialogs/athlete-dialog';
import { ConfirmDialog } from '../../dialogs/confirm-dialog';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-athletes',
  imports: [
    MatTableModule, MatButtonModule, MatIconModule, MatDialogModule,
    NdoAvatarComponent, NdoSearchBarComponent, DatePipe,
  ],
  template: `
    <div class="page" data-testid="athletes-page">
      <div class="page-header">
        <div>
          <h2 class="page-title">All Athletes</h2>
          <p class="page-sub">Manage and track your athletes</p>
        </div>
        <div class="header-actions">
          <ndo-search-bar placeholder="Search athletes..." (valueChange)="onSearch($event)" />
          <button mat-flat-button class="primary-btn" (click)="openAdd()" data-testid="add-athlete-btn">
            <mat-icon fontSet="material-symbols-rounded">add</mat-icon>
            Add Athlete
          </button>
        </div>
      </div>

      <div class="table-wrap" data-testid="athletes-table">
        <table mat-table [dataSource]="filtered()">
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef>ATHLETE</th>
            <td mat-cell *matCellDef="let row">
              <div class="athlete-cell">
                <ndo-avatar [initials]="getInitials(row.name)" [size]="36" />
                <span>{{ row.name }}</span>
              </div>
            </td>
          </ng-container>
          <ng-container matColumnDef="username">
            <th mat-header-cell *matHeaderCellDef>USERNAME</th>
            <td mat-cell *matCellDef="let row">&#64;{{ row.username }}</td>
          </ng-container>
          <ng-container matColumnDef="weight">
            <th mat-header-cell *matHeaderCellDef>WEIGHT</th>
            <td mat-cell *matCellDef="let row">{{ row.currentWeight ? row.currentWeight + ' kg' : '—' }}</td>
          </ng-container>
          <ng-container matColumnDef="lastWeighed">
            <th mat-header-cell *matHeaderCellDef>LAST WEIGHED</th>
            <td mat-cell *matCellDef="let row">{{ row.lastWeighedOn ? (row.lastWeighedOn | date:'mediumDate') : '—' }}</td>
          </ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>ACTIONS</th>
            <td mat-cell *matCellDef="let row">
              <button mat-icon-button (click)="viewDetail(row)" [attr.data-testid]="'view-athlete-' + row.athleteId">
                <mat-icon fontSet="material-symbols-rounded">visibility</mat-icon>
              </button>
              <button mat-icon-button (click)="openEdit(row)" [attr.data-testid]="'edit-athlete-' + row.athleteId">
                <mat-icon fontSet="material-symbols-rounded">edit</mat-icon>
              </button>
              <button mat-icon-button class="delete-btn" (click)="confirmDelete(row)"
                      [attr.data-testid]="'delete-athlete-' + row.athleteId">
                <mat-icon fontSet="material-symbols-rounded">delete</mat-icon>
              </button>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="columns"></tr>
          <tr mat-row *matRowDef="let row; columns: columns;"></tr>
        </table>
      </div>
    </div>
  `,
  styles: `
    .page { display: flex; flex-direction: column; gap: 24px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; flex-wrap: wrap; gap: 16px; }
    .page-title { font-family: 'Plus Jakarta Sans', sans-serif; font-size: 20px; font-weight: 700; margin: 0; }
    .page-sub { color: var(--ndo-text-secondary); font-size: 14px; margin: 4px 0 0; }
    .header-actions { display: flex; gap: 12px; align-items: center; }
    .primary-btn {
      --mdc-filled-button-container-color: var(--ndo-primary);
      --mdc-filled-button-label-text-color: var(--ndo-text-on-primary);
    }
    .table-wrap {
      background: var(--ndo-bg-card); border: 1px solid var(--ndo-border); overflow: auto;
    }
    table { width: 100%; }
    th { color: var(--ndo-text-secondary) !important; font-size: 12px; font-weight: 600; }
    .athlete-cell { display: flex; align-items: center; gap: 12px; }
    .delete-btn { color: var(--ndo-error); }
  `,
})
export class AthletesPage implements OnInit {
  private readonly athletesService = inject(AthletesService);
  private readonly dialog = inject(MatDialog);
  private readonly router = inject(Router);
  private readonly snackbar = inject(NdoSnackbarService);

  athletes = signal<Athlete[]>([]);
  filtered = signal<Athlete[]>([]);
  columns = ['name', 'username', 'weight', 'lastWeighed', 'actions'];

  ngOnInit() { this.load(); }

  load() {
    this.athletesService.getAthletes().subscribe({
      next: (list) => { this.athletes.set(list); this.filtered.set(list); },
      error: () => {},
    });
  }

  onSearch(q: string) {
    const term = q.toLowerCase();
    this.filtered.set(this.athletes().filter(a =>
      a.name.toLowerCase().includes(term) || a.username.toLowerCase().includes(term)
    ));
  }

  getInitials(name: string) {
    return name.split(' ').map(w => w[0]).join('').substring(0, 2).toUpperCase();
  }

  viewDetail(a: Athlete) { this.router.navigate(['/athletes', a.athleteId]); }

  openAdd() {
    const ref = this.dialog.open(AthleteDialog, { width: '480px' });
    ref.afterClosed().subscribe(result => { if (result) this.load(); });
  }

  openEdit(a: Athlete) {
    const ref = this.dialog.open(AthleteDialog, { width: '480px', data: a });
    ref.afterClosed().subscribe(result => { if (result) this.load(); });
  }

  confirmDelete(a: Athlete) {
    const ref = this.dialog.open(ConfirmDialog, {
      width: '400px',
      data: { title: 'Delete Athlete', message: `Are you sure you want to delete ${a.name}?` },
    });
    ref.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.athletesService.deleteAthlete(a.athleteId).subscribe({
          next: () => { this.snackbar.show('Athlete deleted', 'success'); this.load(); },
          error: () => this.snackbar.show('Failed to delete athlete', 'error'),
        });
      }
    });
  }
}
