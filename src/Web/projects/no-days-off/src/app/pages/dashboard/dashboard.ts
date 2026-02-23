import { Component, inject, OnInit, signal } from '@angular/core';
import { NdoStatCardComponent } from 'components';
import { DashboardService } from 'api';
import type { DashboardStats } from 'api';

@Component({
  selector: 'app-dashboard',
  imports: [NdoStatCardComponent],
  template: `
    <div class="dashboard" data-testid="dashboard-page">
      <div class="stats-row" data-testid="stats-row">
        <ndo-stat-card label="Total Workouts" [value]="stats()?.totalWorkouts?.toString() ?? '0'"
                       icon="fitness_center" changePercent="+12.5%" period="vs last month" />
        <ndo-stat-card label="Total Exercises" [value]="stats()?.totalExercises?.toString() ?? '0'"
                       icon="exercise" changePercent="+4" period="this week" />
        <ndo-stat-card label="Active Athletes" [value]="stats()?.totalAthletes?.toString() ?? '0'"
                       icon="groups" changePercent="+3" period="new this month" />
        <ndo-stat-card label="This Week" [value]="stats()?.workoutsThisWeek?.toString() ?? '0'"
                       icon="calendar_month" changePercent="-1" period="vs last week" />
      </div>
    </div>
  `,
  styles: `
    .dashboard { display: flex; flex-direction: column; gap: 24px; }
    .stats-row { display: grid; grid-template-columns: repeat(auto-fit, minmax(220px, 1fr)); gap: 16px; }
  `,
})
export class DashboardPage implements OnInit {
  private readonly dashboardService = inject(DashboardService);
  stats = signal<DashboardStats | null>(null);

  ngOnInit() {
    this.dashboardService.getStats().subscribe({
      next: (s) => this.stats.set(s),
      error: () => {},
    });
  }
}
