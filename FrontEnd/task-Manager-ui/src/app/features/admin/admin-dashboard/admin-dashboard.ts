import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../../core/services/admin';
import { UserResponse } from '../../../core/models/auth';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
})
export class AdminDashboard implements OnInit {
  users: UserResponse[] = [];
  isLoading = true;

  // Create user modal
  showCreateModal = false;
  newFirstName = '';
  newLastName = '';
  newEmail = '';
  newPassword = '';
  newRole = 'User';
  isCreating = false;
  createError = '';
  modalFormSubmitted = false;

  private readonly emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  constructor(
    private readonly adminService: AdminService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.adminService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load users:', err);
        this.isLoading = false;
      }
    });
  }

  // --- Validation Getters ---
  get newFirstNameError(): string {
    if (!this.newFirstName.trim()) return 'First name is required.';
    if (this.newFirstName.trim().length < 2) return 'First name must be at least 2 characters.';
    return '';
  }

  get newLastNameError(): string {
    if (!this.newLastName.trim()) return 'Last name is required.';
    return '';
  }

  get newEmailError(): string {
    if (!this.newEmail.trim()) return 'Email is required.';
    if (!this.emailRegex.test(this.newEmail.trim())) return 'Please enter a valid email address.';
    return '';
  }

  get newPasswordError(): string {
    if (!this.newPassword) return 'Password is required.';
    if (this.newPassword.length < 6) return 'Password must be at least 6 characters.';
    if (!/[A-Z]/.test(this.newPassword)) return 'Must include an uppercase letter.';
    if (!/[a-z]/.test(this.newPassword)) return 'Must include a lowercase letter.';
    if (!/[0-9]/.test(this.newPassword)) return 'Must include a number.';
    if (!/[^a-zA-Z0-9]/.test(this.newPassword)) return 'Must include a special character.';
    return '';
  }

  get isModalFormValid(): boolean {
    return !this.newFirstNameError && !this.newLastNameError && !this.newEmailError && !this.newPasswordError;
  }

  openCreateModal(): void {
    this.showCreateModal = true;
    this.newFirstName = '';
    this.newLastName = '';
    this.newEmail = '';
    this.newPassword = '';
    this.newRole = 'User';
    this.createError = '';
    this.modalFormSubmitted = false;
  }

  closeCreateModal(): void {
    this.showCreateModal = false;
  }

  createUser(): void {
    this.modalFormSubmitted = true;
    if (!this.isModalFormValid) return;

    this.isCreating = true;
    this.createError = '';

    this.adminService.createUser({
      firstName: this.newFirstName.trim(),
      lastName: this.newLastName.trim(),
      email: this.newEmail.trim(),
      password: this.newPassword,
      role: this.newRole
    }).subscribe({
      next: () => {
        this.showCreateModal = false;
        this.isCreating = false;
        this.loadUsers();
      },
      error: (err) => {
        this.isCreating = false;
        this.createError = err.error?.message || 'Failed to create user.';
      }
    });
  }

  changeRole(user: UserResponse): void {
    const newRole = user.role === 'Admin' ? 'User' : 'Admin';
    if (!confirm(`Change ${user.firstName}'s role to ${newRole}?`)) return;

    this.adminService.changeRole(user.id, { role: newRole }).subscribe({
      next: () => this.loadUsers(),
      error: (err) => console.error('Failed to change role:', err)
    });
  }

  toggleStatus(user: UserResponse): void {
    const action = user.isActive ? 'disable' : 'enable';
    if (!confirm(`Are you sure you want to ${action} ${user.firstName}?`)) return;

    this.adminService.toggleStatus(user.id).subscribe({
      next: () => this.loadUsers(),
      error: (err) => console.error('Failed to toggle status:', err)
    });
  }

  deleteUser(user: UserResponse): void {
    if (!confirm(`Are you sure you want to permanently delete ${user.firstName} ${user.lastName}?`)) return;

    this.adminService.deleteUser(user.id).subscribe({
      next: () => this.loadUsers(),
      error: (err) => console.error('Failed to delete user:', err)
    });
  }

  getActiveCount(): number {
    return this.users.filter(u => u.isActive).length;
  }

  getAdminCount(): number {
    return this.users.filter(u => u.role === 'Admin').length;
  }

  getUserInitial(user: UserResponse): string {
    return (user.firstName.charAt(0) + user.lastName.charAt(0)).toUpperCase();
  }
}
