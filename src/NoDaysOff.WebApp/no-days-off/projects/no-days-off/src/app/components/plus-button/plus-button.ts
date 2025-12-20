import { Component, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ndo-plus-button',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './plus-button.html',
  styleUrl: './plus-button.scss'
})
export class PlusButton {
  clicked = output<void>();

  onClick(): void {
    this.clicked.emit();
  }
}
