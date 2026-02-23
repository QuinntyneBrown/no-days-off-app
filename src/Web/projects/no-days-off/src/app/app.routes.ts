import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login').then(m => m.LoginPage),
  },
  {
    path: 'register',
    loadComponent: () => import('./pages/register/register').then(m => m.RegisterPage),
  },
  {
    path: 'forgot-password',
    loadComponent: () => import('./pages/forgot-password/forgot-password').then(m => m.ForgotPasswordPage),
  },
  {
    path: 'reset-password',
    loadComponent: () => import('./pages/reset-password/reset-password').then(m => m.ResetPasswordPage),
  },
  {
    path: '',
    loadComponent: () => import('./layouts/main-layout/main-layout').then(m => m.MainLayout),
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadComponent: () => import('./pages/dashboard/dashboard').then(m => m.DashboardPage),
      },
      {
        path: 'athletes',
        loadComponent: () => import('./pages/athletes/athletes').then(m => m.AthletesPage),
      },
      {
        path: 'athletes/:id',
        loadComponent: () => import('./pages/athlete-detail/athlete-detail').then(m => m.AthleteDetailPage),
      },
      {
        path: 'exercises',
        loadComponent: () => import('./pages/exercises/exercises').then(m => m.ExercisesPage),
      },
      {
        path: 'weekly-planner',
        loadComponent: () => import('./pages/weekly-planner/weekly-planner').then(m => m.WeeklyPlannerPage),
      },
      {
        path: 'workout-days',
        loadComponent: () => import('./pages/workout-days/workout-days').then(m => m.WorkoutDaysPage),
      },
      {
        path: 'messages',
        loadComponent: () => import('./pages/messages/messages').then(m => m.MessagesPage),
      },
      {
        path: 'notifications',
        loadComponent: () => import('./pages/notifications/notifications').then(m => m.NotificationsPage),
      },
      {
        path: 'videos',
        loadComponent: () => import('./pages/videos/videos').then(m => m.VideosPage),
      },
      {
        path: 'settings',
        loadComponent: () => import('./pages/settings/settings').then(m => m.SettingsPage),
      },
      {
        path: 'body-parts',
        loadComponent: () => import('./pages/body-parts/body-parts').then(m => m.BodyPartsPage),
      },
    ],
  },
  { path: '**', redirectTo: 'login' },
];
