import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { TaskService } from '../../../core/services/task';
import { ProjectService } from '../../../core/services/project';
import { Project } from '../../../core/models/project';
import { TaskStatus } from '../../../core/enums/task-status';

@Component({
  selector: 'app-create-task',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './create-task.html',
  styleUrl: './create-task.css',
})
export class CreateTask implements OnInit {
  @ViewChild('taskForm') taskForm!: NgForm;

  title = '';
  description = '';
  status: TaskStatus = TaskStatus.ToDo;
  dueDate = '';
  projectId: number | null = null;
  isSubmitting = false;
  formSubmitted = false;
  projects: Project[] = [];
  taskStatuses = [TaskStatus.ToDo, TaskStatus.InProgress, TaskStatus.Done];
  minDate = '';

  constructor(
    private readonly _taskService: TaskService,
    private readonly _projectService: ProjectService,
    private readonly _router: Router,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
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

    if (!this.title.trim() || !this.projectId || this.isDueDateInvalid()) return;

    this.isSubmitting = true;
    this._taskService.create({
      title: this.title.trim(),
      description: this.description.trim() || undefined,
      status: this.status,
      dueDate: this.dueDate ? new Date(this.dueDate) : undefined,
      projectId: this.projectId
    }).subscribe({
      next: () => {
        this._router.navigate(['/tasks']);
      },
      error: (err) => {
        console.error('Failed to create task:', err);
        this.isSubmitting = false;
      }
    });
  }
}
