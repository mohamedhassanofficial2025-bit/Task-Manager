import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  // Auth pages (no guard)
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login').then(m => m.Login)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register').then(m => m.Register)
  },

  // User routes (auth + User role)
  {
    path: 'projects',
    loadComponent: () =>
      import('./features/projects/project-list/project-list').then(m => m.ProjectList),
    canActivate: [authGuard, roleGuard(['User'])]
  },
  {
    path: 'projects/create',
    loadComponent: () =>
      import('./features/projects/create-project/create-project').then(m => m.CreateProject),
    canActivate: [authGuard, roleGuard(['User'])]
  },
  {
    path: 'projects/:id',
    loadComponent: () =>
      import('./features/projects/project-details/project-details').then(m => m.ProjectDetails),
    canActivate: [authGuard, roleGuard(['User'])]
  },
  {
    path: 'projects/:id/edit',
    loadComponent: () =>
      import('./features/projects/update-project/update-project').then(m => m.UpdateProject),
    canActivate: [authGuard, roleGuard(['User'])]
  },
  {
    path: 'tasks',
    loadComponent: () =>
      import('./features/tasks/task-list/task-list').then(m => m.TaskList),
    canActivate: [authGuard, roleGuard(['User'])]
  },
  {
    path: 'tasks/create',
    loadComponent: () =>
      import('./features/tasks/create-task/create-task').then(m => m.CreateTask),
    canActivate: [authGuard, roleGuard(['User'])]
  },
  {
    path: 'tasks/:id/edit',
    loadComponent: () =>
      import('./features/tasks/update-task/update-task').then(m => m.UpdateTask),
    canActivate: [authGuard, roleGuard(['User'])]
  },

  // Admin routes (auth + Admin role)
  {
    path: 'admin',
    loadComponent: () =>
      import('./features/admin/admin-dashboard/admin-dashboard').then(m => m.AdminDashboard),
    canActivate: [authGuard, roleGuard(['Admin'])]
  },

  // Fallback
  { path: '**', redirectTo: 'login' }
];
