import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, skip } from 'rxjs/operators';
import { forkJoin, Subscription } from 'rxjs';
import { ProjectService } from '../../../core/services/project.service';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Project, ProjectStatus } from '../../../core/models/project.model';
import { BoxStatus } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-projects-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './projects-list.component.html',
  styleUrl: './projects-list.component.scss'
})
export class ProjectsListComponent implements OnInit, OnDestroy {
  projects: Project[] = [];
  filteredProjects: Project[] = [];
  loading = true;
  error = '';
  
  searchControl = new FormControl('');
  selectedStatus: ProjectStatus | 'All' = 'All';
  
  ProjectStatus = ProjectStatus;
  canCreateProject = false;
  
  private subscriptions: Subscription[] = [];

  constructor(
    private projectService: ProjectService,
    private boxService: BoxService,
    private permissionService: PermissionService,
    private router: Router
  ) {}

  ngOnInit(): void {
    console.log('🚀 Projects List Component Initialized');
    
    // Check permissions immediately
    this.checkPermissions();
    
    // Subscribe to permission changes to update UI when permissions are loaded
    this.subscriptions.push(
      this.permissionService.permissions$
        .pipe(skip(1)) // Skip initial empty value
        .subscribe(() => {
          console.log('🔄 Permissions updated, re-checking create project permission');
          this.checkPermissions();
        })
    );
    
    this.loadProjects();
    this.setupSearch();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
  
  private checkPermissions(): void {
    this.canCreateProject = this.permissionService.canCreate('projects');
    console.log('✅ Can create project:', this.canCreateProject);
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
        
        // Log progress values to debug
        projects.forEach(p => {
          console.log(`📊 Project ${p.name || p.code} progress value:`, p.progress, typeof p.progress);
        });
        
        this.projects = projects;
        this.filteredProjects = projects;
        
        // Load boxes for each project to calculate accurate counts
        this.loadBoxesForProjects(projects);
      },
      error: (error) => {
        this.error = 'Failed to load projects';
        this.loading = false;
        console.error('❌ Error loading projects:', error);
      }
    });
  }

  loadBoxesForProjects(projects: Project[]): void {
    const projectsWithId = projects.filter(p => p.id);
    
    if (projectsWithId.length === 0) {
      this.applyFilters();
      this.loading = false;
      return;
    }

    // Create observables to load boxes for each project
    const boxObservables = projectsWithId.map(project => 
      this.boxService.getBoxesByProject(project.id).pipe(
        // Map to include project ID for reference
        map(boxes => ({ projectId: project.id, boxes }))
      )
    );

    // Load all boxes in parallel
    forkJoin(boxObservables).subscribe({
      next: (results) => {
        console.log('✅ Loaded boxes for all projects');
        
        // Calculate counts and progress for each project from boxes (like box progress is calculated from activities)
        results.forEach(({ projectId, boxes }) => {
          const project = this.projects.find(p => p.id === projectId);
          if (!project) {
            return;
          }

          if (boxes.length > 0) {
            const result = this.calculateBoxCounts(boxes);
            project.totalBoxes = result.totalBoxes;
            project.completedBoxes = result.completedBoxes;
            project.inProgressBoxes = result.inProgressBoxes;
            project.readyForDeliveryBoxes = result.readyForDelivery;
            
            // Keep ProgressPercentage from database response - don't override with calculated value
            // project.progress is already set from ProgressPercentage in transformProject
            
            const earliestActualStart = boxes
              .map(box => box.actualStartDate)
              .filter((date): date is Date => !!date)
              .sort((a, b) => a.getTime() - b.getTime())[0];
            if (!project.actualStartDate && earliestActualStart) {
              project.actualStartDate = earliestActualStart;
            }
            console.log(`📊 Using ProgressPercentage from database: ${project.progress}% for project ${project.name}`);
          }
        });

        // Update filtered projects
        this.filteredProjects = [...this.projects];
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        console.error('❌ Error loading boxes for projects:', err);
        // Continue with project data even if boxes fail to load
        this.applyFilters();
        this.loading = false;
      }
    });
  }

  calculateBoxCounts(boxes: any[]): { 
    totalBoxes: number; 
    completedBoxes: number; 
    inProgressBoxes: number; 
    readyForDelivery: number;
  } {
    const counts = {
      totalBoxes: boxes.length,
      completedBoxes: 0,
      inProgressBoxes: 0,
      readyForDelivery: 0
    };

    boxes.forEach(box => {
      const status = box.status as BoxStatus;
      switch (status) {
        case BoxStatus.InProgress:
        case BoxStatus.QAReview:
          counts.inProgressBoxes++;
          break;
        case BoxStatus.Completed:
        case BoxStatus.Delivered:
        case BoxStatus.Dispatched:
          counts.completedBoxes++;
          break;
        case BoxStatus.ReadyForDelivery:
          counts.readyForDelivery++;
          break;
      }
    });

    return counts;
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
      [ProjectStatus.Active]: 'badge-success',
      [ProjectStatus.OnHold]: 'badge-warning',
      [ProjectStatus.Completed]: 'badge-success',
      [ProjectStatus.Archived]: 'badge-neutral',
      [ProjectStatus.Closed]: 'badge-error'
    };
    return statusMap[status] || 'badge-info';
  }

  getProgressColor(progress: number): string {
    // Normalize progress to 0-100 scale for color calculation
    const normalizedProgress = progress > 1 ? progress : progress * 100;
    if (normalizedProgress >= 75) return 'var(--success-color)';
    if (normalizedProgress >= 50) return 'var(--info-color)';
    if (normalizedProgress >= 25) return 'var(--warning-color)';
    return 'var(--error-color)';
  }

  formatProgress(progress: number): string {
    // Handle null, undefined, or falsy values (but allow 0)
    if (progress === null || progress === undefined) return '0.00';
    
    // Convert to number if it's a string
    const numProgress = typeof progress === 'string' ? parseFloat(progress) : Number(progress);
    
    // Handle NaN or invalid numbers
    if (isNaN(numProgress) || !isFinite(numProgress)) return '0.00';
    
    // Progress is calculated from boxes (average of box.progress)
    // Box progress is in percentage format (0-100), so project progress is also in percentage format
    // Display directly with 2 decimal places
    return numProgress.toFixed(2);
  }

  getProgressForBar(progress: number): number {
    // Progress is calculated from boxes (average of box.progress)
    // Box progress is in percentage format (0-100), so project progress is also in percentage format
    // Use the value directly for the bar width (no conversion needed)
    // Ensure it doesn't exceed 100%
    return Math.min(progress, 100);
  }

  /**
   * Get the priority start date for a project based on: ActualStartDate > CompressionStartDate > PlannedStartDate
   */
  getProjectStartDate(project: Project): Date | undefined {
    if (!project) return undefined;
    
    // Priority: ActualStartDate > CompressionStartDate > PlannedStartDate
    if (project.actualStartDate) {
      const date = project.actualStartDate instanceof Date ? project.actualStartDate : new Date(project.actualStartDate);
      if (!isNaN(date.getTime())) return date;
    }
    
    if (project.compressionStartDate) {
      const date = project.compressionStartDate instanceof Date ? project.compressionStartDate : new Date(project.compressionStartDate);
      if (!isNaN(date.getTime())) return date;
    }
    
    if (project.plannedStartDate) {
      const date = project.plannedStartDate instanceof Date ? project.plannedStartDate : new Date(project.plannedStartDate);
      if (!isNaN(date.getTime())) return date;
    }
    
    return undefined;
  }

  /**
   * Get the label for the priority start date
   */
  getProjectStartDateLabel(project: Project): string {
    if (!project) return 'Not Scheduled';
    
    if (project.actualStartDate) {
      const date = project.actualStartDate instanceof Date ? project.actualStartDate : new Date(project.actualStartDate);
      if (!isNaN(date.getTime())) return 'Started:';
    }
    
    if (project.compressionStartDate) {
      const date = project.compressionStartDate instanceof Date ? project.compressionStartDate : new Date(project.compressionStartDate);
      if (!isNaN(date.getTime())) return 'Compression Start:';
    }
    
    if (project.plannedStartDate) {
      const date = project.plannedStartDate instanceof Date ? project.plannedStartDate : new Date(project.plannedStartDate);
      if (!isNaN(date.getTime())) return 'Planned Start:';
    }
    
    return 'Not Scheduled';
  }
}
