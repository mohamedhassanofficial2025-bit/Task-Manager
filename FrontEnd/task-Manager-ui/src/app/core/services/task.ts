import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Task } from '../models/task';
import { TaskStatus } from '../enums/task-status';
import { ApiResponse } from '../models/api-response';
import { UpdateTaskDto } from '../models/update-task-dto';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  constructor(private http: HttpClient) {}

  /**
   * Get all tasks
   */
  getAll(): Observable<Task[]> {
    return this.http.get<ApiResponse<Task[]>>(`${environment.apiUrl}/Tasks`)
      .pipe(map(res => res.data));
  }

  /**
   * Get a specific task
   */
  getOne(id: number): Observable<Task> {
    return this.http.get<ApiResponse<Task>>(`${environment.apiUrl}/Tasks/${id}`)
      .pipe(map(res => res.data));
  }

  /**
   * Create a new task
   */
  create(task: Partial<Task>): Observable<Task> {
    return this.http.post<ApiResponse<Task>>(`${environment.apiUrl}/Tasks`, task)
      .pipe(map(res => res.data));
  }

  /**
   * Update an existing task (matches backend UpdateTaskDto: title, description, dueDate only)
   */
  update(id: number, task: UpdateTaskDto): Observable<Task> {
    return this.http.put<ApiResponse<Task>>(`${environment.apiUrl}/Tasks/${id}`, task)
      .pipe(map(res => res.data));
  }

  /**
   * Delete a task
   */
  delete(id: number): Observable<Task> {
    return this.http.delete<ApiResponse<Task>>(`${environment.apiUrl}/Tasks/${id}`)
      .pipe(map(res => res.data));
  }

  /**
   * Change the Status of a task
   */
  changeStatus(id: number, status: TaskStatus): Observable<Task> {
    return this.http.patch<ApiResponse<Task>>(`${environment.apiUrl}/Tasks/${id}/status`, { status })
      .pipe(map(res => res.data));
  }

  /**
   * Get all tasks for a project
   */
  getProjectTasks(projectId: number): Observable<Task[]> {
    return this.http.get<ApiResponse<Task[]>>(`${environment.apiUrl}/Tasks/project/${projectId}/tasks`)
      .pipe(map(res => res.data));
  }

  /**
   * Get all tasks by status
   */
  getTaskStatus(status: TaskStatus): Observable<Task[]> {
    return this.http.get<ApiResponse<Task[]>>(`${environment.apiUrl}/Tasks/status/${status}`)
      .pipe(map(res => res.data));
  }
}
