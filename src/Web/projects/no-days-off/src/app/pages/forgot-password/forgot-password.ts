import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-forgot-password',
  imports: [FormsModule, RouterLink, MatFormFieldModule, MatInputModule, MatIconModule, MatButtonModule],
  template: `
    <div class="auth-page">
      <div class="auth-card">
        <div class="auth-logo">
          <div class="icon-circle cyan">
            <mat-icon fontSet="material-symbols-rounded">lock</mat-icon>
          </div>
          <h1 class="card-title">Forgot Password?</h1>
          <p class="card-sub">No worries, we'll send you reset instructions.</p>
        </div>

        <form (ngSubmit)="onSubmit()" class="auth-form" data-testid="forgot-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Email Address</mat-label>
            <input matInput type="email" [(ngModel)]="email" name="email"
                   data-testid="forgot-email" required>
          </mat-form-field>

          @if (sent()) {
            <p class="success-text" data-testid="forgot-success">Reset instructions sent to your email.</p>
          }

          <button mat-flat-button class="primary-btn" type="submit" data-testid="forgot-submit">
            Send Reset Link
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
    .icon-circle.cyan { background: var(--ndo-primary-dim); color: var(--ndo-primary); }
    .icon-circle mat-icon { font-size: 32px; width: 32px; height: 32px; }
    .card-title { font-family: 'Plus Jakarta Sans', sans-serif; font-size: 20px; font-weight: 700; margin: 0 0 8px; }
    .card-sub { color: var(--ndo-text-secondary); font-size: 14px; margin: 0; }
    .auth-form { display: flex; flex-direction: column; gap: 8px; }
    .full-width { width: 100%; }
    .primary-btn {
      width: 100%; height: 48px;
      --mdc-filled-button-container-color: var(--ndo-primary);
      --mdc-filled-button-label-text-color: var(--ndo-text-on-primary);
      font-weight: 600;
    }
    .success-text { color: var(--ndo-success); font-size: 13px; margin: 0; }
    .auth-footer { text-align: center; margin-top: 24px; }
    .text-link {
      color: var(--ndo-text-secondary); text-decoration: none; font-size: 14px;
      display: inline-flex; align-items: center; gap: 4px;
    }
    .back-icon { font-size: 18px; width: 18px; height: 18px; }
  `,
})
export class ForgotPasswordPage {
  email = '';
  sent = signal(false);

  onSubmit() {
    if (this.email) {
      this.sent.set(true);
    }
  }
}
