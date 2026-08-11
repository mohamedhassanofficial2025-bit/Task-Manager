import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  email = '';
  password = '';
  isSubmitting = false;
  errorMessage = '';
  formSubmitted = false;

  private readonly emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {
    if (this.authService.isLoggedIn()) {
      const role = this.authService.getRole();
      this.router.navigate([role === 'Admin' ? '/admin' : '/projects']);
    }
  }

  get emailError(): string {
    if (!this.email.trim()) return 'Email is required.';
    if (!this.emailRegex.test(this.email.trim())) return 'Please enter a valid email address.';
    return '';
  }

  get passwordError(): string {
    if (!this.password) return 'Password is required.';
    if (this.password.length < 6) return 'Password must be at least 6 characters.';
    return '';
  }

  get isFormValid(): boolean {
    return !this.emailError && !this.passwordError;
  }

  onSubmit(): void {
    this.formSubmitted = true;
    if (!this.isFormValid) return;

    this.isSubmitting = true;
    this.errorMessage = '';

    this.authService.login({ email: this.email.trim(), password: this.password }).subscribe({
      next: (data) => {
        const role = data.role;
        this.router.navigate([role === 'Admin' ? '/admin' : '/projects']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Invalid email or password.';
      }
    });
  }
}
