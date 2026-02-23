import { Component, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { inject } from '@angular/core';

@Component({
  selector: 'app-reset-password',
  imports: [FormsModule, RouterLink, MatFormFieldModule, MatInputModule, MatIconModule, MatButtonModule],
  template: `
    <div class="auth-page">
      <div class="auth-card">
        <div class="auth-logo">
          <div class="icon-circle accent">
            <mat-icon fontSet="material-symbols-rounded">key</mat-icon>
          </div>
          <h1 class="card-title">Reset Password</h1>
          <p class="card-sub">Create a new strong password for your account.</p>
        </div>

        <form (ngSubmit)="onSubmit()" class="auth-form" data-testid="reset-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-icon matPrefix fontSet="material-symbols-rounded">lock</mat-icon>
            <mat-label>New Password</mat-label>
            <input matInput type="password" [(ngModel)]="password" name="password"
                   data-testid="reset-password" required>
            <mat-hint>Min. 8 characters</mat-hint>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-icon matPrefix fontSet="material-symbols-rounded">lock</mat-icon>
            <mat-label>Confirm New Password</mat-label>
            <input matInput type="password" [(ngModel)]="confirmPassword" name="confirmPassword"
                   data-testid="reset-confirm" required>
          </mat-form-field>

          @if (error()) {
            <p class="error-text" data-testid="reset-error">{{ error() }}</p>
          }

          <button mat-flat-button class="accent-btn" type="submit" data-testid="reset-submit">
            Reset Password
          </button>
        </form>

        <p class="auth-footer">
          <a routerLink="/login" class="text-link" data-testid="back-to-login">
            <mat-icon fontSet="material-symbols-rounded" class="back-icon">arrow_back</mat-icon>
            Back to Sign In
          </a>
        </p>
      </div>
    </div>
  `,
  styles: `
    .auth-page {
      min-height: 100vh; display: flex; align-items: center; justify-content: center;
      background: var(--ndo-bg-page); padding: 16px;
    }
    .auth-card {
      width: 100%; max-width: 400px; padding: 40px 32px;
      background: var(--ndo-bg-surface); border: 1px solid var(--ndo-border);
    }
    .auth-logo { text-align: center; margin-bottom: 32px; }
    .icon-circle {
      width: 72px; height: 72px; border-radius: 50%; display: inline-flex;
      align-items: center; justify-content: center; margin-bottom: 16px;
    }
    .icon-circle.accent { background: var(--ndo-accent-dim); color: var(--ndo-accent); }
    .icon-circle mat-icon { font-size: 32px; width: 32px; height: 32px; }
    .card-title { font-family: 'Plus Jakarta Sans', sans-serif; font-size: 20px; font-weight: 700; margin: 0 0 8px; }
    .card-sub { color: var(--ndo-text-secondary); font-size: 14px; margin: 0; }
    .auth-form { display: flex; flex-direction: column; gap: 4px; }
    .full-width { width: 100%; }
    .full-width mat-icon { color: var(--ndo-text-tertiary); }
    .error-text { color: var(--ndo-error); font-size: 13px; margin: 4px 0; }
    .accent-btn {
      width: 100%; height: 48px; margin-top: 12px;
      --mdc-filled-button-container-color: var(--ndo-accent);
      --mdc-filled-button-label-text-color: #FFFFFF;
      font-weight: 600;
    }
    .auth-footer { text-align: center; margin-top: 24px; }
    .text-link {
      color: var(--ndo-text-secondary); text-decoration: none; font-size: 14px;
      display: inline-flex; align-items: center; gap: 4px;
    }
    .back-icon { font-size: 18px; width: 18px; height: 18px; }
  `,
})
export class ResetPasswordPage {
  private readonly router = inject(Router);
  password = '';
  confirmPassword = '';
  error = signal('');

  onSubmit() {
    if (this.password.length < 8) {
      this.error.set('Password must be at least 8 characters');
      return;
    }
    if (this.password !== this.confirmPassword) {
      this.error.set('Passwords do not match');
      return;
    }
    this.router.navigate(['/login']);
  }
}
