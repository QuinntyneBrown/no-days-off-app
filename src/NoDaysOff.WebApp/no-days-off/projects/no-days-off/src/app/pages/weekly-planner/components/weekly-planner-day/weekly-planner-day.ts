import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Day, ScheduledExercise } from '../../../../models/exercise';

@Component({
  selector: 'ndo-weekly-planner-day',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatListModule, MatCheckboxModule, MatButtonModule, MatIconModule],
  templateUrl: './weekly-planner-day.html',
  styleUrl: './weekly-planner-day.scss'
})
export class WeeklyPlannerDay {
  day = input.required<Day>();

  exerciseToggled = output<ScheduledExercise>();
  addExercise = output<Day>();

  onToggleExercise(exercise: ScheduledExercise): void {
    this.exerciseToggled.emit({ ...exercise, isCompleted: !exercise.isCompleted });
  }

  onAddExercise(): void {
    this.addExercise.emit(this.day());
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric' });
  }
}
