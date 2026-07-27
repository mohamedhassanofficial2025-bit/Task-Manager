import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ProjectService } from '../../../core/services/project';
import { TaskService } from '../../../core/services/task';
import { Project } from '../../../core/models/project';
import { Task } from '../../../core/models/task';
import { TaskStatus } from '../../../core/enums/task-status';

@Component({
  selector: 'app-project-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './project-details.html',
  styleUrl: './project-details.css',
})
export class ProjectDetails implements OnInit {
  project: Project | null = null;
  tasks: Task[] = [];
  isLoading = true;
  taskStatus = TaskStatus;

  constructor(
    private readonly _projectService: ProjectService,
    private readonly _taskService: TaskService,
    private readonly _route: ActivatedRoute,
    private readonly _router: Router,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = Number(this._route.snapshot.paramMap.get('id'));
    this.loadProject(id);
  }

  loadProject(id: number) {
    this.isLoading = true;
    this._projectService.getById(id).subscribe({
      next: (project) => {
        this.project = project;
        this.cdr.detectChanges();
        this.loadTasks(id);
      },
      error: (err) => {
        console.error('Project not found:', err);
        this._router.navigate(['/projects']);
      }
    });
  }

  loadTasks(projectId: number) {
    this._taskService.getProjectTasks(projectId).subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load tasks:', err);
        this.isLoading = false;
      }
    });
  }

  deleteTask(id: number) {
    if (confirm('Are you sure you want to delete this task?')) {
      this._taskService.delete(id).subscribe({
        next: () => this.loadTasks(this.project!.id),
        error: (err) => console.error('Failed to delete task:', err)
      });
    }
  }

  changeStatus(id: number, status: TaskStatus) {
    this._taskService.changeStatus(id, status).subscribe({
      next: () => this.loadTasks(this.project!.id),
      error: (err) => console.error('Failed to update status:', err)
    });
  }

  getNextStatus(current: TaskStatus): TaskStatus {
    switch (current) {
      case TaskStatus.ToDo: return TaskStatus.InProgress;
      case TaskStatus.InProgress: return TaskStatus.Done;
      case TaskStatus.Done: return TaskStatus.ToDo;
    }
  }

  getBadgeClass(status: TaskStatus): string {
    switch (status) {
      case TaskStatus.ToDo: return 'badge-todo';
      case TaskStatus.InProgress: return 'badge-progress';
      case TaskStatus.Done: return 'badge-done';
      default: return '';
    }
  }

  getStatusLabel(status: TaskStatus): string {
    switch (status) {
      case TaskStatus.ToDo: return 'To Do';
      case TaskStatus.InProgress: return 'In Progress';
      case TaskStatus.Done: return 'Done';
      default: return status;
    }
  }

  getCountByStatus(status: TaskStatus): number {
    return this.tasks.filter(t => t.status === status).length;
  }

  formatDate(date: Date | undefined): string {
    if (!date) return '—';
    return new Date(date).toLocaleDateString('en-US', {
      month: 'short', day: 'numeric', year: 'numeric'
    });
  }
}
