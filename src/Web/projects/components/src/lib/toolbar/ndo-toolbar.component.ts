import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatBadgeModule } from '@angular/material/badge';
import { NdoAvatarComponent } from '../avatar/ndo-avatar.component';

@Component({
  selector: 'ndo-toolbar',
  standalone: true,
  imports: [CommonModule, MatToolbarModule, MatIconModule, MatButtonModule, MatBadgeModule, NdoAvatarComponent],
  template: `
    <mat-toolbar class="ndo-toolbar">
      <div class="ndo-toolbar-left">
        <button mat-icon-button (click)="menuToggle.emit()">
          <mat-icon [fontSet]="'material-symbols-rounded'">menu</mat-icon>
        </button>
        <span class="ndo-toolbar-title">{{ title }}</span>
      </div>
      <div class="ndo-toolbar-right">
        <button mat-icon-button [matBadge]="notificationCount > 0 ? notificationCount : null" matBadgeSize="small" matBadgeColor="warn">
          <mat-icon [fontSet]="'material-symbols-rounded'">notifications</mat-icon>
        </button>
        <ndo-avatar [initials]="avatarInitials" [src]="avatarSrc" />
      </div>
    </mat-toolbar>
  `,
  styles: `
    .ndo-toolbar {
      --mat-toolbar-container-background-color: var(--ndo-bg-surface, #141414);
      --mat-toolbar-container-text-color: var(--ndo-text-primary, #FFFFFF);

      display: flex;
      justify-content: space-between;
      align-items: center;
      height: 64px;
      padding: 0 20px;
      border-bottom: 1px solid var(--ndo-border, #2A2A2A);
    }

    .ndo-toolbar-left {
      display: flex;
      align-items: center;
      gap: 12px;
    }

    .ndo-toolbar-title {
      font-family: 'Plus Jakarta Sans', sans-serif;
      font-size: 18px;
      font-weight: 800;
      letter-spacing: 2px;
      color: var(--ndo-primary, #00E5FF);
    }

    .ndo-toolbar-right {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .ndo-toolbar-left button,
    .ndo-toolbar-right button {
      --mdc-icon-button-icon-color: var(--ndo-text-secondary, #9E9E9E);
    }
  `
})
export class NdoToolbarComponent {
  @Input() title = 'NO DAYS OFF';
  @Input() avatarInitials = 'JD';
  @Input() avatarSrc = '';
  @Input() notificationCount = 0;
  @Output() menuToggle = new EventEmitter<void>();
}
