import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ProjectService } from '../../../core/services/project';

@Component({
  selector: 'app-update-project',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './update-project.html',
  styleUrl: './update-project.css',
})
export class UpdateProject implements OnInit {
  @ViewChild('projectForm') projectForm!: NgForm;

  projectId!: number;
  name = '';
  description = '';
  isLoading = true;
  isSubmitting = false;
  formSubmitted = false;

  constructor(
    private readonly _projectService: ProjectService,
    private readonly _router: Router,
    private readonly _route: ActivatedRoute,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.projectId = Number(this._route.snapshot.paramMap.get('id'));
    this._projectService.getById(this.projectId).subscribe({
      next: (project) => {
        this.name = project.name;
        this.description = project.description || '';
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load project:', err);
        this._router.navigate(['/projects']);
      }
    });
  }

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
    this._projectService.update(this.projectId, {
      name: this.name.trim(),
      description: this.description.trim() || undefined
    }).subscribe({
      next: () => {
        this._router.navigate(['/projects']);
      },
      error: (err) => {
        console.error('Failed to update project:', err);
        this.isSubmitting = false;
      }
    });
  }
}
