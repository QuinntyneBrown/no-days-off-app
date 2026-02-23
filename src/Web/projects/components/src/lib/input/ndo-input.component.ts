import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ndo-input',
  standalone: true,
  imports: [CommonModule, FormsModule, MatFormFieldModule, MatInputModule, MatIconModule],
  template: `
    <mat-form-field appearance="outline" class="ndo-input" [class.ndo-input-full]="fullWidth">
      <mat-label>{{ label }}</mat-label>
      @if (prefixIcon) {
        <mat-icon matPrefix [fontSet]="'material-symbols-rounded'">{{ prefixIcon }}</mat-icon>
      }
      @if (type === 'textarea') {
        <textarea matInput
          [placeholder]="placeholder"
          [value]="value"
          [disabled]="disabled"
          [rows]="rows"
          (input)="onInput($event)">
        </textarea>
      } @else {
        <input matInput
          [type]="type"
          [placeholder]="placeholder"
          [value]="value"
          [disabled]="disabled"
          (input)="onInput($event)">
      }
      @if (hint) {
        <mat-hint>{{ hint }}</mat-hint>
      }
      @if (suffixIcon) {
        <mat-icon matSuffix [fontSet]="'material-symbols-rounded'">{{ suffixIcon }}</mat-icon>
      }
    </mat-form-field>
  `,
  styles: `
    :host { display: block; }

    .ndo-input {
      width: 280px;
      font-family: 'DM Sans', sans-serif;

      --mdc-outlined-text-field-outline-color: var(--ndo-border, #2A2A2A);
      --mdc-outlined-text-field-focus-outline-color: var(--ndo-primary, #00E5FF);
      --mdc-outlined-text-field-hover-outline-color: var(--ndo-border-light, #333333);
      --mdc-outlined-text-field-label-text-color: var(--ndo-text-secondary, #9E9E9E);
      --mdc-outlined-text-field-focus-label-text-color: var(--ndo-primary, #00E5FF);
      --mdc-outlined-text-field-input-text-color: var(--ndo-text-primary, #FFFFFF);
      --mdc-outlined-text-field-input-text-placeholder-color: var(--ndo-text-tertiary, #616161);
      --mdc-outlined-text-field-caret-color: var(--ndo-primary, #00E5FF);
      --mdc-outlined-text-field-container-shape: 0px;

      --mat-form-field-container-text-font: 'DM Sans', sans-serif;
      --mat-form-field-container-text-size: 14px;
      --mat-form-field-subscript-text-font: 'DM Sans', sans-serif;
      --mat-form-field-subscript-text-size: 11px;
    }

    .ndo-input-full { width: 100%; }
  `
})
export class NdoInputComponent {
  @Input() label = 'Label';
  @Input() placeholder = 'Enter value...';
  @Input() value = '';
  @Input() type = 'text';
  @Input() prefixIcon = '';
  @Input() suffixIcon = '';
  @Input() hint = '';
  @Input() disabled = false;
  @Input() fullWidth = false;
  @Input() rows = 3;
  @Output() valueChange = new EventEmitter<string>();

  onInput(event: Event) {
    const target = event.target as HTMLInputElement;
    this.value = target.value;
    this.valueChange.emit(this.value);
  }
}
