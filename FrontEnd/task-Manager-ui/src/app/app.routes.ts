import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'projects', pathMatch: 'full' },
  {
    path: 'projects',
    loadComponent: () =>
      import('./features/projects/project-list/project-list').then(m => m.ProjectList)
  },
  {
    path: 'projects/create',
    loadComponent: () =>
      import('./features/projects/create-project/create-project').then(m => m.CreateProject)
  },
  {
    path: 'projects/:id',
    loadComponent: () =>
      import('./features/projects/project-details/project-details').then(m => m.ProjectDetails)
  },
  {
    path: 'projects/:id/edit',
    loadComponent: () =>
      import('./features/projects/update-project/update-project').then(m => m.UpdateProject)
  },
  {
    path: 'tasks',
    loadComponent: () =>
      import('./features/tasks/task-list/task-list').then(m => m.TaskList)
  },
  {
    path: 'tasks/create',
    loadComponent: () =>
      import('./features/tasks/create-task/create-task').then(m => m.CreateTask)
  },
  {
    path: 'tasks/:id/edit',
    loadComponent: () =>
      import('./features/tasks/update-task/update-task').then(m => m.UpdateTask)
  }
];
