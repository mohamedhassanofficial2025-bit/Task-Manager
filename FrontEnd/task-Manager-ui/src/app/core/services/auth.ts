import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response';
import { AuthResponse, LoginDto, RegisterDto } from '../models/auth';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly TOKEN_KEY = 'taskflow_token';
  private readonly USER_KEY = 'taskflow_user';

  constructor(private http: HttpClient, private router: Router) {}

  login(dto: LoginDto): Observable<AuthResponse> {
    return this.http.post<ApiResponse<AuthResponse>>(`${environment.apiUrl}/Account/login`, dto)
      .pipe(
        map(res => res.data),
        tap(data => this.storeAuth(data))
      );
  }

  register(dto: RegisterDto): Observable<AuthResponse> {
    return this.http.post<ApiResponse<AuthResponse>>(`${environment.apiUrl}/Account/register`, dto)
      .pipe(
        map(res => res.data),
        tap(data => this.storeAuth(data))
      );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;

    // Check if token is expired
    try {
      const payload = this.decodeToken(token);
      const expiration = payload.exp * 1000; // convert to ms
      return Date.now() < expiration;
    } catch {
      return false;
    }
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getRole(): string {
    const user = this.getStoredUser();
    return user?.role ?? '';
  }

  getUserName(): string {
    const token = this.getToken();
    if (!token) return '';
    try {
      const payload = this.decodeToken(token);
      return payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ?? '';
    } catch {
      return '';
    }
  }

  getUserEmail(): string {
    const user = this.getStoredUser();
    return user?.email ?? '';
  }

  getUserInitial(): string {
    const name = this.getUserName();
    return name ? name.charAt(0).toUpperCase() : '?';
  }

  private storeAuth(data: AuthResponse): void {
    localStorage.setItem(this.TOKEN_KEY, data.token);
    localStorage.setItem(this.USER_KEY, JSON.stringify({
      email: data.email,
      role: data.role,
      expiration: data.expiration
    }));
  }

  private getStoredUser(): { email: string; role: string; expiration: string } | null {
    const raw = localStorage.getItem(this.USER_KEY);
    if (!raw) return null;
    try {
      return JSON.parse(raw);
    } catch {
      return null;
    }
  }

  private decodeToken(token: string): any {
    const parts = token.split('.');
    if (parts.length !== 3) throw new Error('Invalid token');
    const payload = parts[1];
    const decoded = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
    return JSON.parse(decoded);
  }
}
