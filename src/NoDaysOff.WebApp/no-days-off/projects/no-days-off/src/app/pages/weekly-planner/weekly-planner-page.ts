import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WeeklyPlannerGrid } from './components/weekly-planner-grid/weekly-planner-grid';
import { SecondaryHeader } from '../../components/secondary-header';
import { ModalWindow } from '../../components/modal-window';
import { Day, ScheduledExercise } from '../../models/exercise';

@Component({
  selector: 'ndo-weekly-planner-page',
  standalone: true,
  imports: [CommonModule, WeeklyPlannerGrid, SecondaryHeader, ModalWindow],
  templateUrl: './weekly-planner-page.html',
  styleUrl: './weekly-planner-page.scss'
})
export class WeeklyPlannerPage {
  days = signal<Day[]>([]);
  selectedDay = signal<Day | null>(null);
  isAddExerciseModalOpen = signal(false);

  constructor() {
    this.loadWeek();
  }

  private loadWeek(): void {
    // Generate current week days
    const today = new Date();
    const startOfWeek = new Date(today);
    startOfWeek.setDate(today.getDate() - today.getDay());

    const dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    const weekDays: Day[] = [];

    for (let i = 0; i < 7; i++) {
      const date = new Date(startOfWeek);
      date.setDate(startOfWeek.getDate() + i);
      weekDays.push({
        dayId: i + 1,
        name: dayNames[i],
        date: date,
        scheduledExercises: []
      });
    }

    this.days.set(weekDays);
  }

  onExerciseToggled(exercise: ScheduledExercise): void {
    this.days.update(days =>
      days.map(day => ({
        ...day,
        scheduledExercises: day.scheduledExercises.map(ex =>
          ex.scheduledExerciseId === exercise.scheduledExerciseId ? exercise : ex
        )
      }))
    );
  }

  onAddExercise(day: Day): void {
    this.selectedDay.set(day);
    this.isAddExerciseModalOpen.set(true);
  }

  onCloseModal(): void {
    this.isAddExerciseModalOpen.set(false);
    this.selectedDay.set(null);
  }
}
