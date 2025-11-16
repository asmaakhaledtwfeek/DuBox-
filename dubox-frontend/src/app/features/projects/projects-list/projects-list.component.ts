import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ProjectService } from '../../../core/services/project.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Project, ProjectStatus } from '../../../core/models/project.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-projects-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './projects-list.component.html',
  styleUrl: './projects-list.component.scss'
})
export class ProjectsListComponent implements OnInit {
  projects: Project[] = [];
  filteredProjects: Project[] = [];
  loading = true;
  error = '';
  
  searchControl = new FormControl('');
  selectedStatus: ProjectStatus | 'All' = 'All';
  
  ProjectStatus = ProjectStatus;
  canCreateProject = false;

  constructor(
    private projectService: ProjectService,
    private permissionService: PermissionService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.canCreateProject = this.permissionService.canCreate('projects');
    this.loadProjects();
    this.setupSearch();
  }

  loadProjects(): void {
    this.loading = true;
    this.error = '';

    this.projectService.getProjects().subscribe({
      next: (projects) => {
        this.projects = projects;
        this.filteredProjects = projects;
        this.applyFilters();
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load projects';
        this.loading = false;
        console.error('Error loading projects:', error);
      }
    });
  }

  setupSearch(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.applyFilters();
      });
  }

  applyFilters(): void {
    let filtered = [...this.projects];

    // Apply search filter
    const searchTerm = this.searchControl.value?.toLowerCase() || '';
    if (searchTerm) {
      filtered = filtered.filter(project =>
        project.name.toLowerCase().includes(searchTerm) ||
        project.code.toLowerCase().includes(searchTerm) ||
        project.location?.toLowerCase().includes(searchTerm)
      );
    }

    // Apply status filter
    if (this.selectedStatus !== 'All') {
      filtered = filtered.filter(project => project.status === this.selectedStatus);
    }

    this.filteredProjects = filtered;
  }

  filterByStatus(status: ProjectStatus | 'All'): void {
    this.selectedStatus = status;
    this.applyFilters();
  }

  viewProject(projectId: string): void {
    this.router.navigate(['/projects', projectId, 'dashboard']);
  }

  viewBoxes(projectId: string, event: Event): void {
    event.stopPropagation();
    this.router.navigate(['/projects', projectId, 'boxes']);
  }

  createProject(): void {
    // Navigate to create project page (to be implemented)
    console.log('Create project');
  }

  getStatusClass(status: ProjectStatus): string {
    const statusMap: Record<ProjectStatus, string> = {
      [ProjectStatus.Planning]: 'badge-info',
      [ProjectStatus.Active]: 'badge-success',
      [ProjectStatus.OnHold]: 'badge-warning',
      [ProjectStatus.Completed]: 'badge-success',
      [ProjectStatus.Cancelled]: 'badge-error'
    };
    return statusMap[status] || 'badge-info';
  }

  getProgressColor(progress: number): string {
    if (progress >= 75) return 'var(--success-color)';
    if (progress >= 50) return 'var(--info-color)';
    if (progress >= 25) return 'var(--warning-color)';
    return 'var(--error-color)';
  }
}
