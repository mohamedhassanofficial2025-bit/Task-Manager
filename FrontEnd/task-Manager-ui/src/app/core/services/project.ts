import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Project } from '../models/project';
import { ApiResponse } from '../models/api-response';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  constructor(private http: HttpClient) {}

  /**
   * Get all projects from the database
   */
  getAll(): Observable<Project[]> {
    return this.http.get<ApiResponse<Project[]>>(`${environment.apiUrl}/Projects`)
      .pipe(map(res => res.data));
  }

  /**
   * Get a specific project by id
   */
  getById(id: number): Observable<Project> {
    return this.http.get<ApiResponse<Project>>(`${environment.apiUrl}/Projects/${id}`)
      .pipe(map(res => res.data));
  }

  /**
   * Create a new project in the database
   */
  create(project: Partial<Project>): Observable<Project> {
    return this.http.post<ApiResponse<Project>>(`${environment.apiUrl}/Projects`, project)
      .pipe(map(res => res.data));
  }

  /**
   * Update a specific project by id
   */
  update(id: number, project: Partial<Project>): Observable<Project> {
    return this.http.put<ApiResponse<Project>>(`${environment.apiUrl}/Projects/${id}`, project)
      .pipe(map(res => res.data));
  }

  /**
   * Delete a specific project by id
   */
  delete(id: number): Observable<Project> {
    return this.http.delete<ApiResponse<Project>>(`${environment.apiUrl}/Projects/${id}`)
      .pipe(map(res => res.data));
  }
}
