import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ndo-hamburger-button',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './hamburger-button.html',
  styleUrl: './hamburger-button.scss'
})
export class HamburgerButton {
  isOpen = input<boolean>(false);
  clicked = output<void>();

  onClick(): void {
    this.clicked.emit();
  }
}
