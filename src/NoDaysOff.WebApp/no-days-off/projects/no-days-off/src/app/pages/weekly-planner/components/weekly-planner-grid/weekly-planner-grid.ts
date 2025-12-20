import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WeeklyPlannerDay } from '../weekly-planner-day/weekly-planner-day';
import { Day, ScheduledExercise } from '../../../../models/exercise';

@Component({
  selector: 'ndo-weekly-planner-grid',
  standalone: true,
  imports: [CommonModule, WeeklyPlannerDay],
  templateUrl: './weekly-planner-grid.html',
  styleUrl: './weekly-planner-grid.scss'
})
export class WeeklyPlannerGrid {
  days = input<Day[]>([]);

  exerciseToggled = output<ScheduledExercise>();
  addExercise = output<Day>();

  onExerciseToggled(exercise: ScheduledExercise): void {
    this.exerciseToggled.emit(exercise);
  }

  onAddExercise(day: Day): void {
    this.addExercise.emit(day);
  }
}
