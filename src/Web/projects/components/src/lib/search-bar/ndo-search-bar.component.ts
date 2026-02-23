import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ndo-search-bar',
  standalone: true,
  imports: [CommonModule, FormsModule, MatFormFieldModule, MatInputModule, MatIconModule],
  template: `
    <mat-form-field appearance="outline" class="ndo-search-bar" [class.ndo-search-full]="fullWidth">
      <mat-icon matPrefix [fontSet]="'material-symbols-rounded'">search</mat-icon>
      <input matInput [placeholder]="placeholder" [value]="value" (input)="onInput($event)">
    </mat-form-field>
  `,
  styles: `
    :host { display: block; }

    .ndo-search-bar {
      font-family: 'DM Sans', sans-serif;

      --mdc-outlined-text-field-outline-color: var(--ndo-border, #2A2A2A);
      --mdc-outlined-text-field-focus-outline-color: var(--ndo-primary, #00E5FF);
      --mdc-outlined-text-field-hover-outline-color: var(--ndo-border-light, #333333);
      --mdc-outlined-text-field-input-text-color: var(--ndo-text-primary, #FFFFFF);
      --mdc-outlined-text-field-input-text-placeholder-color: var(--ndo-text-tertiary, #616161);
      --mdc-outlined-text-field-caret-color: var(--ndo-primary, #00E5FF);
      --mdc-outlined-text-field-container-shape: 0px;

      --mat-form-field-container-text-font: 'DM Sans', sans-serif;
      --mat-form-field-container-text-size: 14px;

      width: 100%;
    }

    .ndo-search-bar mat-icon {
      color: var(--ndo-text-tertiary, #616161);
    }

    .ndo-search-full { width: 100%; }
  `
})
export class NdoSearchBarComponent {
  @Input() placeholder = 'Search...';
  @Input() value = '';
  @Input() fullWidth = false;
  @Output() valueChange = new EventEmitter<string>();

  onInput(event: Event) {
    const target = event.target as HTMLInputElement;
    this.value = target.value;
    this.valueChange.emit(this.value);
  }
}
