import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Project } from '../../../core/models/project.model';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-project-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './project-dashboard.component.html',
  styleUrl: './project-dashboard.component.scss'
})
export class ProjectDashboardComponent implements OnInit {
  project: Project | null = null;
  projectId: string = '';
  loading = true;
  error = '';
  showDeleteConfirm = false;
  deleting = false;
  canEdit = false;
  canDelete = false;


  dashboardData = {
    totalBoxes: 0,
    completedBoxes: 0,
    inProgressBoxes: 0,
    readyForDelivery: 0,
    notStarted: 0,
    onHold: 0
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private boxService: BoxService,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
    console.log('🏠 Project Dashboard - Project ID from route:', this.projectId);
    if (!this.projectId) {
      console.error('❌ No project ID in route!');
      this.error = 'Project ID is missing';
      this.loading = false;
      return;
    }
    this.canEdit = this.permissionService.canEdit('projects');
    this.canDelete = this.permissionService.canDelete('projects');
    this.loadProject();
  }

  loadProject(): void {
    this.loading = true;
    console.log('📡 Loading project data for ID:', this.projectId);
    
    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        console.log('✅ Project loaded:', project);
        console.log('🆔 Project ID:', project.id);
        
        this.project = project;
        
        // Load boxes to calculate accurate counts
        this.loadBoxesAndCalculateCounts();
      },
      error: (error) => {
        this.error = 'Failed to load project';
        this.loading = false;
        console.error('❌ Error loading project:', error);
      }
    });
  }

  loadBoxesAndCalculateCounts(): void {
    this.boxService.getBoxesByProject(this.projectId).subscribe({
      next: (boxes) => {
        console.log('✅ Boxes loaded:', boxes.length);
        
        // Calculate counts based on actual box statuses
        const counts = {
          totalBoxes: boxes.length,
          completedBoxes: 0,
          inProgressBoxes: 0,
          readyForDelivery: 0,
          notStarted: 0,
          onHold: 0
        };

        boxes.forEach(box => {
          const status = box.status as BoxStatus;
          switch (status) {
            case BoxStatus.NotStarted:
              counts.notStarted++;
              break;
            case BoxStatus.InProgress:
              counts.inProgressBoxes++;
              break;
            case BoxStatus.QAReview:
              // QA Review is also considered in progress
              counts.inProgressBoxes++;
              break;
            case BoxStatus.Completed:
              counts.completedBoxes++;
              break;
            case BoxStatus.ReadyForDelivery:
              counts.readyForDelivery++;
              break;
            case BoxStatus.Delivered:
              // Delivered is also considered completed
              counts.completedBoxes++;
              break;
            case BoxStatus.OnHold:
              counts.onHold++;
              break;
          }
        });

        // Calculate project progress from boxes (average of box progress) - same way box progress is calculated from activities
        if (boxes.length > 0 && this.project) {
          const averageProgress = boxes.reduce((sum, box) => sum + (box.progress || 0), 0) / boxes.length;
          this.project.progress = averageProgress;
          console.log(`📊 Calculated project progress from boxes: ${averageProgress}%`);
          console.log(`📊 Box progress values:`, boxes.map(b => b.progress));
          console.log(`📊 Display value: ${this.formatProgress(averageProgress)}%`);
          console.log(`📊 Bar width: ${this.getProgressForBar(averageProgress)}%`);
        }

        this.dashboardData = counts;
        console.log('📊 Calculated box counts:', this.dashboardData);
        console.log('📊 Project progress (calculated from boxes):', this.project?.progress + '%');
        this.loading = false;
      },
      error: (err) => {
        console.error('❌ Error loading boxes:', err);
        // Fallback to project data if boxes can't be loaded
        if (this.project) {
          this.dashboardData = {
            totalBoxes: this.project.totalBoxes || 0,
            completedBoxes: this.project.completedBoxes || 0,
            inProgressBoxes: this.project.inProgressBoxes || 0,
            readyForDelivery: this.project.readyForDeliveryBoxes || 0,
            notStarted: 0,
            onHold: 0
          };
        }
        this.loading = false;
      }
    });
  }


  viewBoxes(): void {
    console.log('🔍 Navigate to boxes for project:', this.projectId);
    if (!this.projectId) {
      console.error('❌ Cannot navigate: projectId is undefined');
      alert('Error: Project ID is missing. Cannot view boxes.');
      return;
    }
    this.router.navigate(['/projects', this.projectId, 'boxes']);
  }

  goBack(): void {
    this.router.navigate(['/projects']);
  }

  editProject(): void {
    if (!this.projectId) {
      return;
    }

    this.router.navigate(['/projects/create'], {
      queryParams: { mode: 'edit', projectId: this.projectId }
    });
  }

  openDeleteConfirm(): void {
    this.showDeleteConfirm = true;
  }

  cancelDelete(): void {
    this.showDeleteConfirm = false;
  }

  deleteProject(): void {
    if (this.deleting || !this.projectId) {
      return;
    }

    this.deleting = true;
    this.error = '';

    this.projectService.deleteProject(this.projectId).subscribe({
      next: () => {
        this.deleting = false;
        this.showDeleteConfirm = false;
        this.router.navigate(['/projects']);
      },
      error: (err) => {
        this.deleting = false;
        this.showDeleteConfirm = false;
        this.error = err.message || 'Failed to delete project';
        console.error('❌ Error deleting project:', err);
      }
    });
  }

  formatProgress(progress: number | undefined): string {
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

  getProgressForBar(progress: number | undefined): number {
    if (progress === undefined || progress === null) return 0;
    
    // Progress is calculated from boxes (average of box.progress)
    // Box progress is in percentage format (0-100), so project progress is also in percentage format
    // Use the value directly for the bar width (no conversion needed)
    // Ensure it doesn't exceed 100%
    return Math.min(progress, 100);
  }

  getProgressColor(progress: number | undefined): string {
    if (progress === undefined || progress === null) return 'var(--error-color)';
    // Normalize progress to 0-100 scale for color calculation
    const normalizedProgress = progress >= 1 && progress <= 100 ? progress : progress * 100;
    if (normalizedProgress >= 75) return 'var(--success-color)';
    if (normalizedProgress >= 50) return 'var(--info-color)';
    if (normalizedProgress >= 25) return 'var(--warning-color)';
    return 'var(--error-color)';
  }
}
