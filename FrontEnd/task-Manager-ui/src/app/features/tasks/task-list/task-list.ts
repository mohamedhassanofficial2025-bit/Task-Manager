import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { TaskService } from '../../../core/services/task';
import { Task } from '../../../core/models/task';
import { TaskStatus } from '../../../core/enums/task-status';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})
export class TaskList implements OnInit {
  tasks: Task[] = [];
  filteredTasks: Task[] = [];
  isLoading = true;
  activeFilter: string = 'All';
  taskStatus = TaskStatus;

  filters = ['All', 'ToDo', 'InProgress', 'Done'];

  constructor(private readonly _taskService: TaskService,
    private readonly cdr:ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.getAllTasks();
  }

  getAllTasks() {
    this.isLoading = true;
    this._taskService.getAll().subscribe({
      next: (data) => {
        this.tasks = data;
        this.applyFilter(this.activeFilter);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load tasks:', err);
        this.isLoading = false;
      }
    });
  }

  applyFilter(filter: string) {
    this.activeFilter = filter;
    if (filter === 'All') {
      this.filteredTasks = [...this.tasks];
    } else {
      this.filteredTasks = this.tasks.filter(t => t.status === filter);
    }
  }

  deleteTask(id: number) {
    if (confirm('Are you sure you want to delete this task?')) {
      this._taskService.delete(id).subscribe({
        next: () => this.getAllTasks(),
        error: (err) => console.error('Failed to delete task:', err)
      });
    }
  }

  changeStatus(id: number, status: TaskStatus) {
    this._taskService.changeStatus(id, status).subscribe({
      next: () => this.getAllTasks(),
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

  getFilterLabel(filter: string): string {
    switch (filter) {
      case 'All': return 'All';
      case 'ToDo': return 'To Do';
      case 'InProgress': return 'In Progress';
      case 'Done': return 'Done';
      default: return filter;
    }
  }

  getFilterCount(filter: string): number {
    if (filter === 'All') return this.tasks.length;
    return this.tasks.filter(t => t.status === filter).length;
  }

  getStatusLabel(status: TaskStatus): string {
    switch (status) {
      case TaskStatus.ToDo: return 'To Do';
      case TaskStatus.InProgress: return 'In Progress';
      case TaskStatus.Done: return 'Done';
      default: return status;
    }
  }

  isOverdue(task: Task): boolean {
    if (!task.dueDate || task.status === TaskStatus.Done) return false;
    return new Date(task.dueDate) < new Date();
  }

  formatDate(date: Date | undefined): string {
    if (!date) return '—';
    return new Date(date).toLocaleDateString('en-US', {
      month: 'short', day: 'numeric', year: 'numeric'
    });
  }
}
