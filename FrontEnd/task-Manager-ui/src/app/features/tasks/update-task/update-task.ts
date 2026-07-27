import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TaskService } from '../../../core/services/task';
import { ProjectService } from '../../../core/services/project';
import { Project } from '../../../core/models/project';
import { TaskStatus } from '../../../core/enums/task-status';
import { switchMap, of } from 'rxjs';
import { UpdateTaskDto } from '../../../core/models/update-task-dto';

@Component({
  selector: 'app-update-task',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './update-task.html',
  styleUrl: './update-task.css',
})
export class UpdateTask implements OnInit {
  @ViewChild('taskForm') taskForm!: NgForm;

  taskId!: number;
  title = '';
  description = '';
  status: TaskStatus = TaskStatus.ToDo;
  originalStatus: TaskStatus = TaskStatus.ToDo;
  dueDate = '';
  projectId: number | null = null;
  isLoading = true;
  isSubmitting = false;
  formSubmitted = false;
  projects: Project[] = [];
  taskStatuses = [TaskStatus.ToDo, TaskStatus.InProgress, TaskStatus.Done];
  minDate = '';

  constructor(
    private readonly _taskService: TaskService,
    private readonly _projectService: ProjectService,
    private readonly _router: Router,
    private readonly _route: ActivatedRoute,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.taskId = Number(this._route.snapshot.paramMap.get('id'));

    this._projectService.getAll().subscribe({
      next: (data) => {
        this.projects = data;
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Failed to load projects:', err)
    });

    // Set minimum date to today
    const today = new Date();
    this.minDate = today.toISOString().split('T')[0];

    this._taskService.getOne(this.taskId).subscribe({
      next: (task) => {
        this.title = task.title;
        this.description = task.description || '';
        this.status = task.status;
        this.originalStatus = task.status;
        this.projectId = task.projectId;
        if (task.dueDate) {
          this.dueDate = task.dueDate.toString().split('T')[0];
        }
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load task:', err);
        this._router.navigate(['/tasks']);
      }
    });
  }

  getStatusLabel(status: TaskStatus): string {
    switch (status) {
      case TaskStatus.ToDo: return 'To Do';
      case TaskStatus.InProgress: return 'In Progress';
      case TaskStatus.Done: return 'Done';
      default: return status;
    }
  }

  isDueDateInvalid(): boolean {
    if (!this.dueDate) return false; // optional field
    return this.dueDate < this.minDate;
  }

  onSubmit() {
    this.formSubmitted = true;

    // Mark all fields as touched to trigger validation display
    if (this.taskForm) {
      Object.values(this.taskForm.controls).forEach(control => {
        control.markAsTouched();
      });
    }

    if (!this.title.trim() || this.isDueDateInvalid()) return;

    this.isSubmitting = true;

    // Build the update payload matching backend UpdateTaskDto exactly
    const updatePayload: UpdateTaskDto = {
      title: this.title.trim(),
      description: this.description.trim() ? this.description.trim() : null,
      dueDate: this.dueDate ? new Date(this.dueDate) : null,
    };

    const statusChanged = this.status !== this.originalStatus;

    // First PUT the task data, then PATCH status if it changed (sequential)
    this._taskService.update(this.taskId, updatePayload).pipe(
      switchMap(() => {
        if (statusChanged) {
          return this._taskService.changeStatus(this.taskId, this.status);
        }
        return of(null);
      })
    ).subscribe({
      next: () => {
        this._router.navigate(['/tasks']);
      },
      error: (err) => {
        console.error('Failed to update task:', err);
        this.isSubmitting = false;
      }
    });
  }
}
