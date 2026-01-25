import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ndo-left-nav',
  standalone: true,
  imports: [CommonModule, MatSidenavModule, MatListModule, MatIconModule],
  templateUrl: './left-nav.html',
  styleUrl: './left-nav.scss'
})
export class LeftNav {
  isOpen = input<boolean>(false);
  closed = output<void>();

  onClose(): void {
    this.closed.emit();
  }
}
