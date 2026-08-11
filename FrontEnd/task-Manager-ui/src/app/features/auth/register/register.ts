import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  firstName = '';
  lastName = '';
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
      this.router.navigate(['/projects']);
    }
  }

  get firstNameError(): string {
    if (!this.firstName.trim()) return 'First name is required.';
    if (this.firstName.trim().length < 2) return 'First name must be at least 2 characters.';
    return '';
  }

  get lastNameError(): string {
    if (!this.lastName.trim()) return 'Last name is required.';
    if (this.lastName.trim().length < 2) return 'Last name must be at least 2 characters.';
    return '';
  }

  get emailError(): string {
    if (!this.email.trim()) return 'Email is required.';
    if (!this.emailRegex.test(this.email.trim())) return 'Please enter a valid email address.';
    return '';
  }

  get passwordError(): string {
    if (!this.password) return 'Password is required.';
    if (this.password.length < 6) return 'Password must be at least 6 characters.';
    if (!/[A-Z]/.test(this.password)) return 'Password must include an uppercase letter.';
    if (!/[a-z]/.test(this.password)) return 'Password must include a lowercase letter.';
    if (!/[0-9]/.test(this.password)) return 'Password must include a number.';
    if (!/[^a-zA-Z0-9]/.test(this.password)) return 'Password must include a special character.';
    return '';
  }

  get passwordStrength(): { label: string; class: string; percent: number } {
    let score = 0;
    if (this.password.length >= 6) score++;
    if (this.password.length >= 10) score++;
    if (/[A-Z]/.test(this.password)) score++;
    if (/[a-z]/.test(this.password)) score++;
    if (/[0-9]/.test(this.password)) score++;
    if (/[^a-zA-Z0-9]/.test(this.password)) score++;

    if (score <= 2) return { label: 'Weak', class: 'strength-weak', percent: 25 };
    if (score <= 4) return { label: 'Medium', class: 'strength-medium', percent: 55 };
    if (score <= 5) return { label: 'Strong', class: 'strength-strong', percent: 80 };
    return { label: 'Excellent', class: 'strength-excellent', percent: 100 };
  }

  get isFormValid(): boolean {
    return !this.firstNameError && !this.lastNameError && !this.emailError && !this.passwordError;
  }

  onSubmit(): void {
    this.formSubmitted = true;
    if (!this.isFormValid) return;

    this.isSubmitting = true;
    this.errorMessage = '';

    this.authService.register({
      firstName: this.firstName.trim(),
      lastName: this.lastName.trim(),
      email: this.email.trim(),
      password: this.password
    }).subscribe({
      next: () => {
        this.router.navigate(['/projects']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Registration failed. Please try again.';
      }
    });
  }
}
