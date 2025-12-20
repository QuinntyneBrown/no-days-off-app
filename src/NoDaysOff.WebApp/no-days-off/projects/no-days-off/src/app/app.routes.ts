import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/home-page').then(m => m.HomePage)
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login-page').then(m => m.LoginPage)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./pages/dashboard/dashboard-page').then(m => m.DashboardPage)
  },
  {
    path: 'dashboard/:id',
    loadComponent: () => import('./pages/dashboard/dashboard-page').then(m => m.DashboardPage)
  },
  {
    path: 'athletes',
    loadComponent: () => import('./pages/athletes/athletes-page').then(m => m.AthletesPage)
  },
  {
    path: 'athletes/:id',
    loadComponent: () => import('./pages/athletes/athletes-page').then(m => m.AthletesPage)
  },
  {
    path: 'weekly-planner',
    loadComponent: () => import('./pages/weekly-planner/weekly-planner-page').then(m => m.WeeklyPlannerPage)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
