import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { DaysService, ScheduledExercisesService } from 'api';
import type { Day, ScheduledExercise } from 'api';

const WEEKDAYS = ['MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT', 'SUN'];

@Component({
  selector: 'app-weekly-planner',
  imports: [MatButtonModule, MatIconModule, MatCardModule],
  template: `
    <div class="page" data-testid="weekly-planner-page">
      <div class="page-header">
        <div class="week-nav">
          <button mat-icon-button (click)="prevWeek()">
            <mat-icon fontSet="material-symbols-rounded">chevron_left</mat-icon>
          </button>
          <span class="week-label" data-testid="week-label">{{ weekLabel() }}</span>
          <button mat-icon-button (click)="nextWeek()">
            <mat-icon fontSet="material-symbols-rounded">chevron_right</mat-icon>
          </button>
        </div>
      </div>
      <div class="planner-grid" data-testid="planner-grid">
        @for (day of WEEKDAYS; track day; let i = $index) {
          <div class="day-column">
            <div class="day-header">
              <span class="day-name">{{ day }}</span>
            </div>
            <div class="day-exercises">
              @for (se of getExercisesForDay(i); track se.scheduledExerciseId) {
                <mat-card class="exercise-item">
                  <span class="ex-name">{{ se.name }}</span>
                  @if (se.sets && se.repetitions) {
                    <span class="ex-meta">{{ se.sets }}x{{ se.repetitions }}
                      @if (se.weightInKgs) { · {{ se.weightInKgs }}kg }
                    </span>
                  }
                </mat-card>
              }
            </div>
          </div>
        }
      </div>
    </div>
  `,
  styles: `
    .page { display: flex; flex-direction: column; gap: 20px; }
    .page-header { display: flex; justify-content: center; }
    .week-nav { display: flex; align-items: center; gap: 12px; }
    .week-label { font-family: 'Plus Jakarta Sans', sans-serif; font-size: 16px; font-weight: 600; }
    .planner-grid { display: grid; grid-template-columns: repeat(7, 1fr); gap: 8px; min-height: 400px; }
    .day-column {
      background: var(--ndo-bg-card); border: 1px solid var(--ndo-border);
      display: flex; flex-direction: column;
    }
    .day-header {
      padding: 12px 8px; text-align: center;
      border-bottom: 1px solid var(--ndo-border);
    }
    .day-name { font-size: 12px; font-weight: 600; color: var(--ndo-text-secondary); }
    .day-exercises { padding: 8px; display: flex; flex-direction: column; gap: 4px; }
    .exercise-item {
      --mdc-elevated-card-container-color: var(--ndo-bg-elevated);
      --mdc-elevated-card-container-elevation: none;
      padding: 8px; border-left: 3px solid var(--ndo-primary);
    }
    .ex-name { font-size: 13px; font-weight: 500; display: block; }
    .ex-meta { font-size: 11px; color: var(--ndo-text-secondary); }
  `,
})
export class WeeklyPlannerPage implements OnInit {
  private readonly daysService = inject(DaysService);
  private readonly seService = inject(ScheduledExercisesService);
  WEEKDAYS = WEEKDAYS;

  days = signal<Day[]>([]);
  scheduledExercises = signal<ScheduledExercise[]>([]);
  weekOffset = signal(0);

  weekLabel = signal(this.getWeekLabel(0));

  ngOnInit() {
    this.daysService.getDays().subscribe({ next: (d) => this.days.set(d), error: () => {} });
    this.seService.getScheduledExercises().subscribe({ next: (s) => this.scheduledExercises.set(s), error: () => {} });
  }

  getExercisesForDay(dayIndex: number): ScheduledExercise[] {
    const dayList = this.days();
    if (dayIndex < dayList.length) {
      return this.scheduledExercises().filter(se => se.dayId === dayList[dayIndex].dayId);
    }
    return [];
  }

  prevWeek() { this.weekOffset.update(v => v - 1); this.weekLabel.set(this.getWeekLabel(this.weekOffset())); }
  nextWeek() { this.weekOffset.update(v => v + 1); this.weekLabel.set(this.getWeekLabel(this.weekOffset())); }

  private getWeekLabel(offset: number): string {
    const now = new Date();
    const start = new Date(now);
    start.setDate(now.getDate() - now.getDay() + 1 + offset * 7);
    const end = new Date(start);
    end.setDate(start.getDate() + 6);
    return `${start.toLocaleDateString('en-US', { month: 'short', day: 'numeric' })} - ${end.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' })}`;
  }
}
