import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProjectService } from '../../../core/services/project';
import { Project } from '../../../core/models/project';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './project-list.html',
  styleUrl: './project-list.css',
})
export class ProjectList implements OnInit {
  projects: Project[] = [];
  isLoading = true;

  constructor(private readonly _projectService: ProjectService,
    private readonly cdr:ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.getAllProjects();
  }

  getAllProjects() {
    this.isLoading = true;
    this._projectService.getAll().subscribe({
      next: (data) => {
        this.projects = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load projects:', err);
        this.isLoading = false;
      }
    });
  }

  deleteProject(id: number) {
    if (confirm('Are you sure you want to delete this project?')) {
      this._projectService.delete(id).subscribe({
        next: () => this.getAllProjects(),
        error: (err) => console.error('Failed to delete project:', err)
      });
    }
  }

  getTimeAgo(date: Date): string {
    const now = new Date();
    const created = new Date(date);
    const diffMs = now.getTime() - created.getTime();
    const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));

    if (diffDays === 0) return 'Today';
    if (diffDays === 1) return 'Yesterday';
    if (diffDays < 7) return `${diffDays} days ago`;
    if (diffDays < 30) return `${Math.floor(diffDays / 7)} weeks ago`;
    return `${Math.floor(diffDays / 30)} months ago`;
  }
}
