import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response';
import { UserResponse, CreateUserDto, ChangeRoleDto } from '../models/auth';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  constructor(private http: HttpClient) {}

  getUsers(): Observable<UserResponse[]> {
    return this.http.get<ApiResponse<UserResponse[]>>(`${environment.apiUrl}/Admin/users`)
      .pipe(map(res => res.data));
  }

  createUser(dto: CreateUserDto): Observable<UserResponse> {
    return this.http.post<ApiResponse<UserResponse>>(`${environment.apiUrl}/Admin/users`, dto)
      .pipe(map(res => res.data));
  }

  changeRole(userId: string, dto: ChangeRoleDto): Observable<UserResponse> {
    return this.http.put<ApiResponse<UserResponse>>(`${environment.apiUrl}/Admin/users/${userId}/role`, dto)
      .pipe(map(res => res.data));
  }

  toggleStatus(userId: string): Observable<any> {
    return this.http.put<ApiResponse<any>>(`${environment.apiUrl}/Admin/users/${userId}/toggle-status`, {});
  }

  deleteUser(userId: string): Observable<any> {
    return this.http.delete<ApiResponse<any>>(`${environment.apiUrl}/Admin/users/${userId}`);
  }
}
