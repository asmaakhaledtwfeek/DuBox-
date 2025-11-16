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
    console.log('🚀 Projects List Component Initialized');
    this.canCreateProject = this.permissionService.canCreate('projects');
    console.log('✅ Can create project:', this.canCreateProject);
    this.loadProjects();
    this.setupSearch();
  }

  loadProjects(): void {
    this.loading = true;
    this.error = '';
    console.log('📡 Loading projects from API...');

    this.projectService.getProjects().subscribe({
      next: (projects) => {
        console.log('📦 Loaded projects:', projects);
        console.log('📊 Total projects count:', projects.length);
        
        // Check if any projects have missing IDs
        const projectsWithoutId = projects.filter(p => !p.id);
        if (projectsWithoutId.length > 0) {
          console.error('⚠️ Found', projectsWithoutId.length, 'projects without IDs:', projectsWithoutId);
          console.error('⚠️ Projects without IDs:', projectsWithoutId);
          
          // Show which projects are missing IDs
          projectsWithoutId.forEach((p, index) => {
            console.error(`Project ${index + 1}:`, {
              name: p.name,
              code: p.code,
              id: p.id,
              allKeys: Object.keys(p)
            });
          });
        }
        
        // Also log projects WITH IDs
        const projectsWithId = projects.filter(p => p.id);
        console.log('✅ Projects WITH IDs:', projectsWithId.length);
        if (projectsWithId.length > 0) {
          console.log('✅ First project with ID:', projectsWithId[0]);
        }
        
        this.projects = projects;
        this.filteredProjects = projects;
        this.applyFilters();
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load projects';
        this.loading = false;
        console.error('❌ Error loading projects:', error);
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
    console.log('🔍 View project dashboard for ID:', projectId);
    console.log('🔍 Type of projectId:', typeof projectId);
    console.log('🔍 All projects:', this.projects);
    
    if (!projectId) {
      console.error('❌ Project ID is undefined!');
      
      // Show what we have in projects
      if (this.projects.length > 0) {
        console.error('First project object:', this.projects[0]);
        console.error('First project keys:', Object.keys(this.projects[0]));
        console.error('First project.id:', this.projects[0].id);
        console.error('First project.projectId:', (this.projects[0] as any).projectId);
        console.error('First project.ProjectId:', (this.projects[0] as any).ProjectId);
        
        alert(`Error: Project ID is missing!\n\nFirst project has these keys: ${Object.keys(this.projects[0]).join(', ')}\n\nCheck console for details.`);
      } else {
        alert('Error: No projects loaded!');
      }
      return;
    }
    this.router.navigate(['/projects', projectId, 'dashboard']);
  }

  viewBoxes(projectId: string, event: Event): void {
    event.stopPropagation();
    console.log('🔍 View boxes for project ID:', projectId);
    if (!projectId) {
      console.error('❌ Project ID is undefined!');
      return;
    }
    this.router.navigate(['/projects', projectId, 'boxes']);
  }

  createProject(): void {
    this.router.navigate(['/projects/create']);
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
