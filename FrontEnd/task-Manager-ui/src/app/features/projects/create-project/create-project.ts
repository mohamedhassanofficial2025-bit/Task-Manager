import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ProjectService } from '../../../core/services/project';

@Component({
  selector: 'app-create-project',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './create-project.html',
  styleUrl: './create-project.css',
})
export class CreateProject {
  @ViewChild('projectForm') projectForm!: NgForm;

  name = '';
  description = '';
  isSubmitting = false;
  formSubmitted = false;

  constructor(
    private readonly _projectService: ProjectService,
    private readonly _router: Router
  ) {}

  onSubmit() {
    this.formSubmitted = true;

    // Mark all fields as touched to trigger validation display
    if (this.projectForm) {
      Object.values(this.projectForm.controls).forEach(control => {
        control.markAsTouched();
      });
    }

    if (!this.name.trim()) return;

    this.isSubmitting = true;
    this._projectService.create({
      name: this.name.trim(),
      description: this.description.trim() || undefined
    }).subscribe({
      next: () => {
        this._router.navigate(['/projects']);
      },
      error: (err) => {
        console.error('Failed to create project:', err);
        this.isSubmitting = false;
      }
    });
  }
}
