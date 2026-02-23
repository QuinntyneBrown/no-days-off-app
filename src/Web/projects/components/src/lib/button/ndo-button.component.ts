import { Component, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

export type NdoButtonVariant = 'primary' | 'accent' | 'outlined' | 'text' | 'icon' | 'fab';

@Component({
  selector: 'ndo-button',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  template: `
    @switch (variant) {
      @case ('primary') {
        <button mat-flat-button class="ndo-btn ndo-btn-primary" [disabled]="disabled">
          @if (icon) {
            <mat-icon [fontSet]="'material-symbols-rounded'">{{ icon }}</mat-icon>
          }
          @if (label) {
            <span>{{ label }}</span>
          }
        </button>
      }
      @case ('accent') {
        <button mat-flat-button class="ndo-btn ndo-btn-accent" [disabled]="disabled">
          @if (icon) {
            <mat-icon [fontSet]="'material-symbols-rounded'">{{ icon }}</mat-icon>
          }
          @if (label) {
            <span>{{ label }}</span>
          }
        </button>
      }
      @case ('outlined') {
        <button mat-stroked-button class="ndo-btn ndo-btn-outlined" [disabled]="disabled">
          @if (icon) {
            <mat-icon [fontSet]="'material-symbols-rounded'">{{ icon }}</mat-icon>
          }
          @if (label) {
            <span>{{ label }}</span>
          }
        </button>
      }
      @case ('text') {
        <button mat-button class="ndo-btn ndo-btn-text" [disabled]="disabled">
          @if (icon) {
            <mat-icon [fontSet]="'material-symbols-rounded'">{{ icon }}</mat-icon>
          }
          @if (label) {
            <span>{{ label }}</span>
          }
        </button>
      }
      @case ('icon') {
        <button mat-icon-button class="ndo-btn ndo-btn-icon" [disabled]="disabled">
          <mat-icon [fontSet]="'material-symbols-rounded'">{{ icon || 'more_vert' }}</mat-icon>
        </button>
      }
      @case ('fab') {
        <button mat-fab class="ndo-btn ndo-btn-fab" [disabled]="disabled">
          <mat-icon [fontSet]="'material-symbols-rounded'">{{ icon || 'add' }}</mat-icon>
        </button>
      }
    }
  `,
  styles: `
    :host { display: inline-block; }

    .ndo-btn-primary {
      --mdc-filled-button-container-color: var(--ndo-primary, #00E5FF);
      --mdc-filled-button-label-text-color: var(--ndo-text-on-primary, #0A0A0A);
      --mat-filled-button-icon-color: var(--ndo-text-on-primary, #0A0A0A);
      font-family: 'DM Sans', sans-serif;
      font-weight: 500;
      letter-spacing: 0.5px;
    }

    .ndo-btn-accent {
      --mdc-filled-button-container-color: var(--ndo-accent, #FF6B35);
      --mdc-filled-button-label-text-color: #FFFFFF;
      --mat-filled-button-icon-color: #FFFFFF;
      font-family: 'DM Sans', sans-serif;
      font-weight: 500;
      letter-spacing: 0.5px;
    }

    .ndo-btn-outlined {
      --mdc-outlined-button-outline-color: var(--ndo-border-light, #333333);
      --mdc-outlined-button-label-text-color: var(--ndo-text-primary, #FFFFFF);
      --mat-outlined-button-icon-color: var(--ndo-text-secondary, #9E9E9E);
      font-family: 'DM Sans', sans-serif;
      font-weight: 500;
      letter-spacing: 0.5px;
    }

    .ndo-btn-text {
      --mdc-text-button-label-text-color: var(--ndo-primary, #00E5FF);
      --mat-text-button-icon-color: var(--ndo-primary, #00E5FF);
      font-family: 'DM Sans', sans-serif;
      font-weight: 500;
      letter-spacing: 0.5px;
    }

    .ndo-btn-icon {
      --mdc-icon-button-icon-color: var(--ndo-text-secondary, #9E9E9E);
      width: 44px;
      height: 44px;
    }

    .ndo-btn-fab {
      --mdc-fab-container-color: var(--ndo-primary, #00E5FF);
      --mat-fab-foreground-color: var(--ndo-text-on-primary, #0A0A0A);
    }
  `
})
export class NdoButtonComponent {
  @Input() variant: NdoButtonVariant = 'primary';
  @Input() label = 'Button';
  @Input() icon = '';
  @Input() disabled = false;
}
