import { Component, inject, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive, Router } from '@angular/router';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatBadgeModule } from '@angular/material/badge';
import { NdoAvatarComponent, NdoBottomNavComponent } from 'components';
import { AuthService } from 'api';
import { map } from 'rxjs';
import { AsyncPipe } from '@angular/common';

interface NavItem { icon: string; label: string; route: string; }

@Component({
  selector: 'app-main-layout',
  imports: [
    RouterOutlet, RouterLink, RouterLinkActive, AsyncPipe,
    MatSidenavModule, MatListModule, MatIconModule, MatButtonModule, MatBadgeModule,
    NdoAvatarComponent, NdoBottomNavComponent,
  ],
  template: `
    @if (isMobile$ | async) {
      <!-- Mobile layout -->
      <div class="mobile-layout">
        <header class="mobile-toolbar">
          <div class="toolbar-left">
            <div class="logo-box">N</div>
            <span class="logo-text">NO DAYS OFF</span>
          </div>
          <div class="toolbar-right">
            <button mat-icon-button routerLink="/notifications">
              <mat-icon fontSet="material-symbols-rounded">notifications</mat-icon>
            </button>
            <ndo-avatar [initials]="'JD'" [size]="32" />
          </div>
        </header>
        <main class="mobile-content">
          <router-outlet />
        </main>
        <ndo-bottom-nav
          [items]="mobileNavItems"
          [activeIndex]="activeMobileIndex()"
          (activeIndexChange)="onMobileNav($event)" />
      </div>
    } @else {
      <!-- Desktop layout -->
      <div class="desktop-layout">
        <aside class="sidebar" data-testid="sidebar">
          <div class="sidebar-top">
            <div class="sidebar-logo">
              <div class="logo-box">N</div>
              <span class="logo-text">NO DAYS OFF</span>
            </div>
            <nav>
              <mat-nav-list>
                @for (item of navItems; track item.route) {
                  <a mat-list-item [routerLink]="item.route" routerLinkActive="active-nav"
                     [attr.data-testid]="'nav-' + item.route">
                    <mat-icon matListItemIcon fontSet="material-symbols-rounded">{{ item.icon }}</mat-icon>
                    <span matListItemTitle>{{ item.label }}</span>
                  </a>
                }
              </mat-nav-list>
            </nav>
          </div>
          <div class="sidebar-bottom">
            <mat-nav-list>
              <a mat-list-item routerLink="/settings" routerLinkActive="active-nav" data-testid="nav-settings">
                <mat-icon matListItemIcon fontSet="material-symbols-rounded">settings</mat-icon>
                <span matListItemTitle>Settings</span>
              </a>
            </mat-nav-list>
            <div class="user-panel" data-testid="user-panel">
              <ndo-avatar [initials]="'JD'" [size]="40" />
              <div class="user-info">
                <span class="user-name">John Doe</span>
                <span class="user-email">john&#64;example.com</span>
              </div>
              <button mat-icon-button (click)="logout()" data-testid="logout-btn">
                <mat-icon fontSet="material-symbols-rounded">logout</mat-icon>
              </button>
            </div>
          </div>
        </aside>
        <div class="main-area">
          <header class="desktop-toolbar" data-testid="toolbar">
            <h1 class="page-title">{{ pageTitle() }}</h1>
            <div class="toolbar-actions">
              <button mat-icon-button routerLink="/notifications" data-testid="toolbar-notifications">
                <mat-icon fontSet="material-symbols-rounded" [matBadge]="3" matBadgeColor="warn" matBadgeSize="small">notifications</mat-icon>
              </button>
              <ndo-avatar [initials]="'JD'" [size]="40" />
            </div>
          </header>
          <main class="desktop-content">
            <router-outlet />
          </main>
        </div>
      </div>
    }
  `,
  styles: `
    :host { display: block; height: 100vh; }

    .desktop-layout {
      display: flex; height: 100vh;
    }
    .sidebar {
      width: 260px; min-width: 260px; height: 100vh;
      background: var(--ndo-bg-surface); border-right: 1px solid var(--ndo-border);
      display: flex; flex-direction: column; justify-content: space-between;
      overflow-y: auto;
    }
    .sidebar-logo {
      display: flex; align-items: center; gap: 12px;
      padding: 24px 20px;
    }
    .logo-box {
      width: 36px; height: 36px; background: var(--ndo-primary);
      color: var(--ndo-text-on-primary); display: flex; align-items: center;
      justify-content: center; font-family: 'Plus Jakarta Sans', sans-serif;
      font-weight: 800; font-size: 18px;
    }
    .logo-text {
      font-family: 'Plus Jakarta Sans', sans-serif; font-weight: 700;
      font-size: 14px; letter-spacing: 2px; color: var(--ndo-text-primary);
    }
    .sidebar mat-nav-list a { color: var(--ndo-text-secondary); }
    .sidebar mat-nav-list a.active-nav {
      color: var(--ndo-primary);
      background: var(--ndo-primary-dim);
      border-left: 3px solid var(--ndo-primary);
    }
    .sidebar mat-nav-list mat-icon { color: inherit; }
    .sidebar-bottom { padding: 16px; border-top: 1px solid var(--ndo-border); }
    .user-panel {
      display: flex; align-items: center; gap: 12px; padding: 12px 8px;
    }
    .user-info { flex: 1; display: flex; flex-direction: column; }
    .user-name { font-size: 14px; font-weight: 600; color: var(--ndo-text-primary); }
    .user-email { font-size: 12px; color: var(--ndo-text-tertiary); }
    .main-area { flex: 1; display: flex; flex-direction: column; overflow: hidden; }
    .desktop-toolbar {
      height: 64px; min-height: 64px; display: flex; align-items: center;
      justify-content: space-between; padding: 0 32px;
      border-bottom: 1px solid var(--ndo-border);
      background: var(--ndo-bg-surface);
    }
    .page-title {
      font-family: 'Plus Jakarta Sans', sans-serif; font-size: 20px;
      font-weight: 700; margin: 0;
    }
    .toolbar-actions { display: flex; align-items: center; gap: 12px; }
    .desktop-content { flex: 1; overflow-y: auto; padding: 32px; }

    /* Mobile */
    .mobile-layout { display: flex; flex-direction: column; height: 100vh; }
    .mobile-toolbar {
      height: 56px; display: flex; align-items: center;
      justify-content: space-between; padding: 0 16px;
      border-bottom: 1px solid var(--ndo-border);
      background: var(--ndo-bg-surface);
    }
    .toolbar-left, .toolbar-right { display: flex; align-items: center; gap: 8px; }
    .mobile-content { flex: 1; overflow-y: auto; padding: 16px; }
  `,
})
export class MainLayout {
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);
  private readonly breakpoint = inject(BreakpointObserver);

  isMobile$ = this.breakpoint.observe([Breakpoints.Handset]).pipe(map(r => r.matches));

  navItems: NavItem[] = [
    { icon: 'dashboard', label: 'Dashboard', route: '/dashboard' },
    { icon: 'groups', label: 'Athletes', route: '/athletes' },
    { icon: 'fitness_center', label: 'Exercises', route: '/exercises' },
    { icon: 'calendar_month', label: 'Weekly Planner', route: '/weekly-planner' },
    { icon: 'assignment', label: 'Workout Days', route: '/workout-days' },
    { icon: 'play_circle', label: 'Videos', route: '/videos' },
    { icon: 'chat', label: 'Messages', route: '/messages' },
    { icon: 'notifications', label: 'Notifications', route: '/notifications' },
    { icon: 'accessibility_new', label: 'Body Parts', route: '/body-parts' },
  ];

  mobileNavItems = [
    { icon: 'dashboard', label: 'Home' },
    { icon: 'fitness_center', label: 'Workouts' },
    { icon: 'groups', label: 'Athletes' },
    { icon: 'person', label: 'Profile' },
  ];

  pageTitle = signal('Dashboard');
  activeMobileIndex = signal(0);

  onMobileNav(index: number) {
    this.activeMobileIndex.set(index);
    const routes = ['/dashboard', '/exercises', '/athletes', '/settings'];
    this.router.navigate([routes[index]]);
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
