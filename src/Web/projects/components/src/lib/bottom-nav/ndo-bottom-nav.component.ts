import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';

export interface NdoBottomNavItem {
  icon: string;
  label: string;
  route?: string;
}

@Component({
  selector: 'ndo-bottom-nav',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatRippleModule],
  template: `
    <nav class="ndo-bottom-nav">
      @for (item of items; track item.label; let i = $index) {
        <button class="ndo-bottom-nav-item" [class.ndo-bottom-nav-item-active]="activeIndex === i" matRipple (click)="onSelect(i)">
          <mat-icon [fontSet]="'material-symbols-rounded'">{{ item.icon }}</mat-icon>
          <span class="ndo-bottom-nav-label">{{ item.label }}</span>
        </button>
      }
    </nav>
  `,
  styles: `
    :host { display: block; width: 100%; }

    .ndo-bottom-nav {
      display: flex;
      align-items: center;
      justify-content: space-around;
      height: 64px;
      background-color: var(--ndo-bg-surface, #141414);
      border-top: 1px solid var(--ndo-border, #2A2A2A);
    }

    .ndo-bottom-nav-item {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 2px;
      padding: 6px 0;
      background: none;
      border: none;
      cursor: pointer;
      color: var(--ndo-text-tertiary, #616161);
      font-family: 'DM Sans', sans-serif;
      font-size: 10px;
      min-width: 64px;
    }

    .ndo-bottom-nav-item mat-icon {
      font-size: 24px;
      width: 24px;
      height: 24px;
    }

    .ndo-bottom-nav-item-active {
      color: var(--ndo-primary, #00E5FF);
    }

    .ndo-bottom-nav-item-active .ndo-bottom-nav-label {
      font-weight: 600;
    }
  `
})
export class NdoBottomNavComponent {
  @Input() items: NdoBottomNavItem[] = [
    { icon: 'dashboard', label: 'Home' },
    { icon: 'fitness_center', label: 'Workouts' },
    { icon: 'groups', label: 'Athletes' },
    { icon: 'person', label: 'Profile' }
  ];
  @Input() activeIndex = 0;
  @Output() activeIndexChange = new EventEmitter<number>();

  onSelect(index: number) {
    this.activeIndex = index;
    this.activeIndexChange.emit(index);
  }
}
