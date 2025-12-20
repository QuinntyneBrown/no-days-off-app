import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AthleteCard } from '../athlete-card/athlete-card';
import { Athlete } from '../../../../models/athlete';

@Component({
  selector: 'ndo-athlete-list',
  standalone: true,
  imports: [CommonModule, AthleteCard],
  templateUrl: './athlete-list.html',
  styleUrl: './athlete-list.scss'
})
export class AthleteList {
  athletes = input<Athlete[]>([]);

  editAthlete = output<Athlete>();
  deleteAthlete = output<Athlete>();

  onEdit(athlete: Athlete): void {
    this.editAthlete.emit(athlete);
  }

  onDelete(athlete: Athlete): void {
    this.deleteAthlete.emit(athlete);
  }
}
