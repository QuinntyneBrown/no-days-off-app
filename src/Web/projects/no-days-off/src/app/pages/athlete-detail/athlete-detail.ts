import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { NdoAvatarComponent } from 'components';
import { AthletesService } from 'api';
import type { Athlete, AthleteWeight } from 'api';
import { DatePipe } from '@angular/common';
import { LogWeightDialog } from '../../dialogs/log-weight-dialog';

@Component({
  selector: 'app-athlete-detail',
  imports: [MatButtonModule, MatIconModule, MatCardModule, MatDialogModule, MatSnackBarModule, NdoAvatarComponent, DatePipe],
  template: `
    <div class="detail" data-testid="athlete-detail-page">
      @if (athlete(); as a) {
        <div class="detail-grid">
          <div class="profile-panel">
            <mat-card class="profile-card">
              <mat-card-content>
                <div class="profile-header">
                  <ndo-avatar [initials]="getInitials(a.name)" [size]="72" />
                  <h2 class="profile-name" data-testid="athlete-name">{{ a.name }}</h2>
                  <p class="profile-username" data-testid="athlete-username">&#64;{{ a.username }}</p>
                </div>
                <div class="profile-stats">
                  <div class="stat"><span class="stat-value">{{ a.currentWeight || '—' }}</span><span class="stat-label">lbs</span></div>
                  <div class="stat"><span class="stat-value">—</span><span class="stat-label">workouts</span></div>
                  <div class="stat"><span class="stat-value">—</span><span class="stat-label">months</span></div>
                </div>
                <div class="profile-actions">
                  <button mat-stroked-button (click)="goBack()" data-testid="back-btn">
                    <mat-icon fontSet="material-symbols-rounded">arrow_back</mat-icon>
                    Back
                  </button>
                </div>
              </mat-card-content>
            </mat-card>
          </div>
          <div class="detail-panel">
            <mat-card class="detail-card">
              <mat-card-content>
                <div class="card-header-row">
                  <div>
                    <h3>Weight Tracking</h3>
                    <p class="card-sub">Last 6 months</p>
                  </div>
                  <button mat-flat-button class="log-weight-btn" (click)="openLogWeight()" data-testid="log-weight-btn">
                    <mat-icon fontSet="material-symbols-rounded">add</mat-icon>
                    Log Weight
                  </button>
                </div>
                <div class="weight-display" data-testid="weight-tracking">
                  <p class="current-weight">{{ a.currentWeight ? a.currentWeight + ' kg' : 'No data' }}</p>
                  @if (a.lastWeighedOn) {
                    <p class="last-weighed">Last weighed: {{ a.lastWeighedOn | date:'mediumDate' }}</p>
                  }
                </div>
                @if (weightHistory().length > 0) {
                  <div class="weight-history" data-testid="weight-history">
                    <h4>History</h4>
                    @for (w of weightHistory(); track w.id) {
                      <div class="weight-history-row" data-testid="weight-history-row">
                        <span class="weight-value">{{ w.weightInKgs }} kg</span>
                        <span class="weight-date">{{ w.weighedOn | date:'mediumDate' }}</span>
                      </div>
                    }
                  </div>
                }
              </mat-card-content>
            </mat-card>
            <mat-card class="detail-card">
              <mat-card-content>
                <h3>Details</h3>
                <div class="info-row"><span class="label">Created</span><span>{{ a.createdOn | date:'mediumDate' }}</span></div>
                <div class="info-row"><span class="label">Created By</span><span>{{ a.createdBy }}</span></div>
              </mat-card-content>
            </mat-card>
          </div>
        </div>
      } @else {
        <p>Loading...</p>
      }
    </div>
  `,
  styles: `
    .detail-grid { display: grid; grid-template-columns: 320px 1fr; gap: 24px; }
    @media (max-width: 900px) { .detail-grid { grid-template-columns: 1fr; } }
    .profile-card, .detail-card {
      --mdc-elevated-card-container-color: var(--ndo-bg-card);
      --mdc-elevated-card-container-elevation: none;
      border: 1px solid var(--ndo-border);
    }
    .profile-header { text-align: center; padding: 24px 0 16px; }
    .profile-name { font-family: 'Plus Jakarta Sans', sans-serif; font-size: 20px; font-weight: 700; margin: 12px 0 4px; }
    .profile-username { color: var(--ndo-text-secondary); font-size: 14px; margin: 0; }
    .profile-stats {
      display: flex; justify-content: center; gap: 24px; padding: 16px 0;
      border-top: 1px solid var(--ndo-border); border-bottom: 1px solid var(--ndo-border);
      margin: 16px 0;
    }
    .stat { text-align: center; }
    .stat-value { display: block; font-size: 18px; font-weight: 700; }
    .stat-label { font-size: 12px; color: var(--ndo-text-tertiary); }
    .profile-actions { display: flex; gap: 8px; justify-content: center; }
    .detail-panel { display: flex; flex-direction: column; gap: 16px; }
    h3 { font-family: 'Plus Jakarta Sans', sans-serif; margin: 0 0 4px; }
    .card-sub { color: var(--ndo-text-secondary); font-size: 13px; margin: 0 0 16px; }
    .card-header-row { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 8px; }
    .log-weight-btn { --mdc-filled-button-container-color: var(--ndo-primary); --mdc-filled-button-label-text-color: var(--ndo-text-on-primary); }
    .current-weight { font-size: 28px; font-weight: 700; margin: 0; }
    .last-weighed { color: var(--ndo-text-secondary); font-size: 13px; }
    .weight-history { margin-top: 16px; border-top: 1px solid var(--ndo-border); padding-top: 12px; }
    .weight-history h4 { font-family: 'Plus Jakarta Sans', sans-serif; margin: 0 0 8px; font-size: 14px; }
    .weight-history-row { display: flex; justify-content: space-between; padding: 6px 0; border-bottom: 1px solid var(--ndo-border); font-size: 14px; }
    .weight-value { font-weight: 600; }
    .weight-date { color: var(--ndo-text-secondary); }
    .info-row { display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid var(--ndo-border); }
    .label { color: var(--ndo-text-secondary); }
  `,
})
export class AthleteDetailPage implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly athletesService = inject(AthletesService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  athlete = signal<Athlete | null>(null);
  weightHistory = signal<AthleteWeight[]>([]);

  private athleteId = 0;

  ngOnInit() {
    this.athleteId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.athleteId) {
      this.loadAthlete();
      this.loadWeightHistory();
    }
  }

  loadAthlete() {
    this.athletesService.getAthlete(this.athleteId).subscribe({
      next: (a) => this.athlete.set(a),
      error: () => this.router.navigate(['/athletes']),
    });
  }

  loadWeightHistory() {
    this.athletesService.getWeightHistory(this.athleteId).subscribe({
      next: (history) => this.weightHistory.set(history),
    });
  }

  openLogWeight() {
    const a = this.athlete();
    if (!a) return;
    const ref = this.dialog.open(LogWeightDialog, {
      width: '400px',
      data: { athleteId: a.athleteId, athleteName: a.name },
    });
    ref.afterClosed().subscribe((saved) => {
      if (saved) {
        this.loadAthlete();
        this.loadWeightHistory();
        this.snackBar.open('Weight logged successfully', 'Close', { duration: 3000 });
      }
    });
  }

  getInitials(name: string) {
    return name.split(' ').map(w => w[0]).join('').substring(0, 2).toUpperCase();
  }

  goBack() { this.router.navigate(['/athletes']); }
}
