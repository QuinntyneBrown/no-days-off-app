import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ndo-modal-window',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule],
  templateUrl: './modal-window.html',
  styleUrl: './modal-window.scss'
})
export class ModalWindow {
  isOpen = input<boolean>(false);
  title = input<string>('');

  closed = output<void>();

  onClose(): void {
    this.closed.emit();
  }

  onBackdropClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('modal-window__backdrop')) {
      this.onClose();
    }
  }
}
