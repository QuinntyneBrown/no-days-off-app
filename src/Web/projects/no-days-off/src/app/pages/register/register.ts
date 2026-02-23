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
  selector: 'app-register',
  imports: [
    FormsModule, RouterLink, MatFormFieldModule, MatInputModule,
    MatIconModule, MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule,
  ],
  template: `
    <div class="auth-page">
      <div class="auth-card">
        <div class="auth-logo">
          <div class="logo-box">N</div>
          <h1 class="logo-title">Create Account</h1>
          <p class="logo-tagline">Start your fitness journey today</p>
        </div>

        <form (ngSubmit)="onRegister()" class="auth-form" data-testid="register-form">
          <div class="name-row">
            <mat-form-field appearance="outline">
              <mat-label>First Name</mat-label>
              <input matInput [(ngModel)]="firstName" name="firstName"
                     data-testid="register-firstname" required>
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Last Name</mat-label>
              <input matInput [(ngModel)]="lastName" name="lastName"
                     data-testid="register-lastname" required>
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-icon matPrefix fontSet="material-symbols-rounded">mail</mat-icon>
            <mat-label>Email</mat-label>
            <input matInput type="email" [(ngModel)]="email" name="email"
                   data-testid="register-email" required>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-icon matPrefix fontSet="material-symbols-rounded">lock</mat-icon>
            <mat-label>Password</mat-label>
            <input matInput type="password" [(ngModel)]="password" name="password"
                   data-testid="register-password" required>
            <mat-hint>Min. 8 characters</mat-hint>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-icon matPrefix fontSet="material-symbols-rounded">lock</mat-icon>
            <mat-label>Confirm Password</mat-label>
            <input matInput type="password" [(ngModel)]="confirmPassword" name="confirmPassword"
                   data-testid="register-confirm-password" required>
          </mat-form-field>

          <mat-checkbox [(ngModel)]="acceptTerms" name="terms"
                        data-testid="register-terms">
            I agree to the Terms of Service
          </mat-checkbox>

          @if (error()) {
            <p class="error-text" data-testid="register-error">{{ error() }}</p>
          }

          <button mat-flat-button class="accent-btn" type="submit"
                  [disabled]="loading()" data-testid="register-submit">
            @if (loading()) {
              <mat-spinner diameter="20" />
            } @else {
              Create Account
            }
          </button>
        </form>

        <p class="auth-footer">
          Already have an account?
          <a routerLink="/login" class="text-link" data-testid="login-link">Sign In</a>
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
      width: 100%; max-width: 440px; padding: 40px 32px;
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
      font-weight: 700; margin: 0 0 8px;
    }
    .logo-tagline { color: var(--ndo-text-secondary); font-size: 14px; margin: 0; }
    .auth-form { display: flex; flex-direction: column; gap: 4px; }
    .name-row { display: flex; gap: 12px; }
    .name-row mat-form-field { flex: 1; }
    .full-width { width: 100%; }
    .full-width mat-icon { color: var(--ndo-text-tertiary); }
    .text-link { color: var(--ndo-primary); text-decoration: none; font-size: 14px; }
    .error-text { color: var(--ndo-error); font-size: 13px; margin: 4px 0; }
    .accent-btn {
      width: 100%; height: 48px; margin-top: 12px;
      --mdc-filled-button-container-color: var(--ndo-accent);
      --mdc-filled-button-label-text-color: #FFFFFF;
      font-weight: 600;
    }
    .auth-footer {
      text-align: center; margin-top: 24px; font-size: 14px;
      color: var(--ndo-text-secondary);
    }
  `,
})
export class RegisterPage {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  firstName = '';
  lastName = '';
  email = '';
  password = '';
  confirmPassword = '';
  acceptTerms = false;
  loading = signal(false);
  error = signal('');

  onRegister() {
    if (!this.firstName || !this.lastName || !this.email || !this.password) {
      this.error.set('Please fill in all fields');
      return;
    }
    if (this.password !== this.confirmPassword) {
      this.error.set('Passwords do not match');
      return;
    }
    if (this.password.length < 8) {
      this.error.set('Password must be at least 8 characters');
      return;
    }
    if (!this.acceptTerms) {
      this.error.set('Please accept the Terms of Service');
      return;
    }
    this.loading.set(true);
    this.error.set('');
    this.auth.register({
      email: this.email,
      password: this.password,
      firstName: this.firstName,
      lastName: this.lastName,
    }).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.loading.set(false);
        this.error.set(err?.error?.message || 'Registration failed');
      },
    });
  }
}
