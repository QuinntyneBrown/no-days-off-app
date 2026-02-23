import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from 'api';

@Component({
  selector: 'app-login',
  imports: [
    FormsModule, RouterLink, MatFormFieldModule, MatInputModule,
    MatIconModule, MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule,
  ],
  template: `
    <div class="auth-page">
      <div class="auth-card">
        <div class="auth-logo">
          <div class="logo-box">N</div>
          <h1 class="logo-title">NO DAYS OFF</h1>
          <p class="logo-tagline">Train harder. Get stronger.</p>
        </div>

        <form (ngSubmit)="onLogin()" class="auth-form" data-testid="login-form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-icon matPrefix fontSet="material-symbols-rounded">mail</mat-icon>
            <mat-label>Email</mat-label>
            <input matInput type="email" [(ngModel)]="email" name="email"
                   data-testid="login-email" required>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-icon matPrefix fontSet="material-symbols-rounded">lock</mat-icon>
            <mat-label>Password</mat-label>
            <input matInput [type]="showPassword() ? 'text' : 'password'"
                   [(ngModel)]="password" name="password"
                   data-testid="login-password" required>
            <button mat-icon-button matSuffix type="button"
                    (click)="showPassword.set(!showPassword())">
              <mat-icon fontSet="material-symbols-rounded">
                {{ showPassword() ? 'visibility_off' : 'visibility' }}
              </mat-icon>
            </button>
          </mat-form-field>

          <div class="form-options">
            <mat-checkbox [(ngModel)]="rememberMe" name="remember"
                          data-testid="login-remember">Remember me</mat-checkbox>
            <a routerLink="/forgot-password" class="text-link" data-testid="forgot-password-link">Forgot password?</a>
          </div>

          @if (error()) {
            <p class="error-text" data-testid="login-error">{{ error() }}</p>
          }

          <button mat-flat-button class="primary-btn" type="submit"
                  [disabled]="loading()" data-testid="login-submit">
            @if (loading()) {
              <mat-spinner diameter="20" />
            } @else {
              Sign In
            }
          </button>
        </form>

        <p class="auth-footer">
          Don't have an account?
          <a routerLink="/register" class="text-link" data-testid="register-link">Sign Up</a>
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
    .logo-box {
      width: 56px; height: 56px; background: var(--ndo-primary);
      color: var(--ndo-text-on-primary); display: inline-flex; align-items: center;
      justify-content: center; font-family: 'Plus Jakarta Sans', sans-serif;
      font-weight: 800; font-size: 24px; margin-bottom: 16px;
    }
    .logo-title {
      font-family: 'Plus Jakarta Sans', sans-serif; font-size: 20px;
      font-weight: 700; letter-spacing: 3px; margin: 0 0 8px;
    }
    .logo-tagline { color: var(--ndo-text-secondary); font-size: 14px; margin: 0; }
    .auth-form { display: flex; flex-direction: column; gap: 4px; }
    .full-width { width: 100%; }
    .full-width mat-icon { color: var(--ndo-text-tertiary); }
    .form-options {
      display: flex; justify-content: space-between; align-items: center;
      margin-bottom: 16px;
    }
    .text-link {
      color: var(--ndo-primary); text-decoration: none; font-size: 14px;
      &:hover { text-decoration: underline; }
    }
    .error-text { color: var(--ndo-error); font-size: 13px; margin: 0 0 8px; }
    .primary-btn {
      width: 100%; height: 48px;
      --mdc-filled-button-container-color: var(--ndo-primary);
      --mdc-filled-button-label-text-color: var(--ndo-text-on-primary);
      font-weight: 600;
    }
    .auth-footer {
      text-align: center; margin-top: 24px; font-size: 14px;
      color: var(--ndo-text-secondary);
    }
  `,
})
export class LoginPage {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  password = '';
  rememberMe = false;
  showPassword = signal(false);
  loading = signal(false);
  error = signal('');

  onLogin() {
    if (!this.email || !this.password) {
      this.error.set('Please enter email and password');
      return;
    }
    this.loading.set(true);
    this.error.set('');
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.loading.set(false);
        this.error.set(err?.error?.message || 'Invalid email or password');
      },
    });
  }
}
