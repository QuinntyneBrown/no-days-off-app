import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { NdoAvatarComponent } from 'components';
import { AuthService } from 'api';

@Component({
  selector: 'app-settings',
  imports: [MatCardModule, MatButtonModule, MatIconModule, NdoAvatarComponent],
  template: `
    <div class="page" data-testid="settings-page">
      <h2 class="page-title">Settings</h2>
      <div class="settings-grid">
        <mat-card class="profile-card">
          <mat-card-content>
            <div class="profile-section">
              <ndo-avatar [initials]="'JD'" [size]="72" />
              <h3>John Doe</h3>
              <p class="email">john&#64;example.com</p>
            </div>
            <button mat-stroked-button class="full-btn">Edit Profile</button>
          </mat-card-content>
        </mat-card>
        <div class="settings-panel">
          <mat-card class="setting-card">
            <mat-card-content>
              <h3>Account</h3>
              <div class="setting-item">
                <mat-icon fontSet="material-symbols-rounded">person</mat-icon>
                <span>Profile Information</span>
                <mat-icon fontSet="material-symbols-rounded" class="chevron">chevron_right</mat-icon>
              </div>
              <div class="setting-item">
                <mat-icon fontSet="material-symbols-rounded">lock</mat-icon>
                <span>Change Password</span>
                <mat-icon fontSet="material-symbols-rounded" class="chevron">chevron_right</mat-icon>
              </div>
              <div class="setting-item">
                <mat-icon fontSet="material-symbols-rounded">notifications</mat-icon>
                <span>Notification Preferences</span>
                <mat-icon fontSet="material-symbols-rounded" class="chevron">chevron_right</mat-icon>
              </div>
            </mat-card-content>
          </mat-card>
          <button mat-stroked-button class="logout-btn" (click)="logout()" data-testid="settings-logout">
            <mat-icon fontSet="material-symbols-rounded">logout</mat-icon>
            Sign Out
          </button>
        </div>
      </div>
    </div>
  `,
  styles: `
    .page { display: flex; flex-direction: column; gap: 24px; }
    .page-title { font-family: 'Plus Jakarta Sans', sans-serif; font-size: 20px; font-weight: 700; margin: 0; }
    .settings-grid { display: grid; grid-template-columns: 300px 1fr; gap: 24px; }
    @media (max-width: 900px) { .settings-grid { grid-template-columns: 1fr; } }
    .profile-card, .setting-card { --mdc-elevated-card-container-color: var(--ndo-bg-card); --mdc-elevated-card-container-elevation: none; border: 1px solid var(--ndo-border); }
    .profile-section { text-align: center; padding: 24px 0 16px; }
    .profile-section h3 { margin: 12px 0 4px; font-size: 18px; }
    .email { color: var(--ndo-text-secondary); font-size: 14px; margin: 0 0 16px; }
    .full-btn { width: 100%; }
    .settings-panel { display: flex; flex-direction: column; gap: 16px; }
    h3 { font-family: 'Plus Jakarta Sans', sans-serif; margin: 0 0 16px; }
    .setting-item {
      display: flex; align-items: center; gap: 12px; padding: 12px 0;
      border-bottom: 1px solid var(--ndo-border); cursor: pointer;
    }
    .setting-item span { flex: 1; }
    .chevron { color: var(--ndo-text-tertiary); }
    .logout-btn { color: var(--ndo-error); border-color: var(--ndo-error); }
  `,
})
export class SettingsPage {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
