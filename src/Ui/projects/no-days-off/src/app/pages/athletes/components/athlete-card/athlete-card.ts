import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Athlete } from '../../../../models/athlete';

@Component({
  selector: 'ndo-athlete-card',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './athlete-card.html',
  styleUrl: './athlete-card.scss'
})
export class AthleteCard {
  athlete = input.required<Athlete>();

  edit = output<Athlete>();
  delete = output<Athlete>();

  onEdit(): void {
    this.edit.emit(this.athlete());
  }

  onDelete(): void {
    this.delete.emit(this.athlete());
  }
}
