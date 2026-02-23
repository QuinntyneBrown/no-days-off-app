import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ndo-nav-item',
  standalone: true,
  imports: [CommonModule, MatListModule, MatIconModule],
  template: `
    <a mat-list-item class="ndo-nav-item" [class.ndo-nav-item-active]="active">
      <mat-icon matListItemIcon [fontSet]="'material-symbols-rounded'">{{ icon }}</mat-icon>
      <span matListItemTitle>{{ label }}</span>
    </a>
  `,
  styles: `
    :host { display: block; width: 100%; }

    .ndo-nav-item {
      --mdc-list-list-item-label-text-font: 'DM Sans', sans-serif;
      --mdc-list-list-item-label-text-size: 14px;
      --mdc-list-list-item-label-text-color: var(--ndo-text-secondary, #9E9E9E);
      --mdc-list-list-item-leading-icon-color: var(--ndo-text-tertiary, #616161);
      --mdc-list-list-item-hover-label-text-color: var(--ndo-text-primary, #FFFFFF);
      --mdc-list-list-item-hover-leading-icon-color: var(--ndo-primary, #00E5FF);

      padding: 12px 20px;
      gap: 14px;
      border-radius: 0;
    }

    .ndo-nav-item-active {
      --mdc-list-list-item-label-text-color: var(--ndo-primary, #00E5FF);
      --mdc-list-list-item-label-text-weight: 600;
      --mdc-list-list-item-leading-icon-color: var(--ndo-primary, #00E5FF);

      background-color: var(--ndo-primary-dim, #00E5FF18);
      border-left: 3px solid var(--ndo-primary, #00E5FF);
    }
  `
})
export class NdoNavItemComponent {
  @Input() icon = 'dashboard';
  @Input() label = 'Menu Item';
  @Input() active = false;
}
