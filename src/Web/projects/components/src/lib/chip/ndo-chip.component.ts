import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ndo-chip',
  standalone: true,
  imports: [CommonModule, MatChipsModule, MatIconModule],
  template: `
    <mat-chip-option class="ndo-chip" [selected]="selected" [disabled]="disabled">
      @if (icon) {
        <mat-icon matChipAvatar [fontSet]="'material-symbols-rounded'">{{ icon }}</mat-icon>
      }
      {{ label }}
    </mat-chip-option>
  `,
  styles: `
    :host { display: inline-block; }

    .ndo-chip {
      --mdc-chip-elevated-container-color: var(--ndo-primary-dim, #00E5FF18);
      --mdc-chip-elevated-selected-container-color: var(--ndo-primary-dim, #00E5FF18);
      --mdc-chip-label-text-color: var(--ndo-primary, #00E5FF);
      --mdc-chip-selected-label-text-color: var(--ndo-primary, #00E5FF);
      --mdc-chip-with-icon-icon-color: var(--ndo-primary, #00E5FF);
      --mdc-chip-elevated-disabled-container-color: transparent;
      --mdc-chip-flat-disabled-outline-color: var(--ndo-border-light, #333333);
      --mdc-chip-disabled-label-text-color: var(--ndo-text-secondary, #9E9E9E);

      --mat-chip-label-text-font: 'DM Sans', sans-serif;
      --mat-chip-label-text-size: 12px;
      --mat-chip-label-text-weight: 500;

      border-radius: 0;
    }
  `
})
export class NdoChipComponent {
  @Input() label = 'Chip';
  @Input() icon = '';
  @Input() selected = false;
  @Input() disabled = false;
}
