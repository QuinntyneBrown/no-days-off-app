import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from './components/header';
import { LeftNav } from './components/left-nav';
import { HamburgerButton } from './components/hamburger-button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterModule,
    Header,
    LeftNav,
    HamburgerButton,
    MatSidenavModule,
    MatListModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('No Days Off');
  protected isNavOpen = signal(false);
  protected isAuthenticated = signal(true);

  toggleNav(): void {
    this.isNavOpen.update(v => !v);
  }

  closeNav(): void {
    this.isNavOpen.set(false);
  }
}
