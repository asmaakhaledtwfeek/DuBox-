import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { BoxService } from '../../../core/services/box.service';
import { BoxActivity } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { UpdateProgressModalComponent } from '../update-progress-modal/update-progress-modal.component';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { BoxActivityDetail, ProgressUpdate } from '../../../core/models/progress-update.model';

@Component({
  selector: 'app-activity-details',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent, UpdateProgressModalComponent],
  templateUrl: './activity-details.component.html',
  styleUrls: ['./activity-details.component.scss']
})
export class ActivityDetailsComponent implements OnInit {
  activity: BoxActivity | null = null;
  activityDetail: BoxActivityDetail | null = null;
  progressHistory: ProgressUpdate[] = [];
  activityId!: string;
  projectId!: string;
  boxId!: string;
  loading = true;
  error = '';
  isModalOpen = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxService: BoxService,
    private progressUpdateService: ProgressUpdateService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['projectId'];
    this.boxId = this.route.snapshot.params['boxId'];
    this.activityId = this.route.snapshot.params['activityId'];
    
    if (!this.projectId || !this.boxId || !this.activityId) {
      this.error = 'Missing required parameters';
      this.loading = false;
      return;
    }
    
    this.loadActivity();
  }

  loadActivity(): void {
    this.loading = true;
    this.error = '';
    
    // Load activity details
    this.progressUpdateService.getActivityDetails(this.activityId).subscribe({
      next: (activityDetail) => {
        this.activityDetail = activityDetail;
        this.activity = this.convertToBoxActivity(activityDetail);
        this.loading = false;
        console.log('✅ Activity loaded:', activityDetail);
        
        // Load progress history
        this.loadProgressHistory();
      },
      error: (err) => {
        // Fallback to old service if new endpoint not available
        this.boxService.getActivityDetails(this.activityId).subscribe({
          next: (activity) => {
            this.activity = activity;
            this.loading = false;
            console.log('✅ Activity loaded (fallback):', activity);
          },
          error: (err) => {
            this.error = err.message || 'Failed to load activity details';
            this.loading = false;
            console.error('❌ Error loading activity:', err);
          }
        });
      }
    });
  }

  loadProgressHistory(): void {
    this.progressUpdateService.getProgressUpdatesByActivity(this.activityId).subscribe({
      next: (history) => {
        this.progressHistory = history.sort((a, b) => 
          new Date(b.updateDate || b.createdDate || '').getTime() - 
          new Date(a.updateDate || a.createdDate || '').getTime()
        );
        console.log('✅ Progress history loaded:', this.progressHistory.length, 'updates');
      },
      error: (err) => {
        console.warn('⚠️ Could not load progress history:', err);
        // Not critical, just log the warning
      }
    });
  }

  convertToBoxActivity(detail: BoxActivityDetail): BoxActivity {
    return {
      id: detail.boxActivityId,
      boxId: detail.boxId,
      name: detail.activityName,
      description: detail.workDescription,
      status: detail.status as any,
      sequence: detail.sequence,
      assignedTo: detail.teamName || detail.assignedMemberName,
      plannedDuration: detail.duration || 0,
      weightPercentage: detail.progressPercentage,
      plannedStartDate: detail.plannedStartDate,
      actualStartDate: detail.actualStartDate,
      plannedEndDate: detail.plannedEndDate,
      actualEndDate: detail.actualEndDate
    };
  }

  openProgressModal(): void {
    if (this.activityDetail) {
      this.isModalOpen = true;
    }
  }

  closeProgressModal(): void {
    this.isModalOpen = false;
  }

  onProgressUpdated(response: any): void {
    console.log('Progress updated:', response);
    
    // Reload activity details
    this.loadActivity();
    
    // Show WIR notification if created
    if (response.wirCreated) {
      alert(`✅ Progress updated! WIR ${response.wirCode} has been created for QC inspection.`);
    }
  }

  goBack(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId]);
  }

  getStatusClass(status: string): string {
    const statusMap: Record<string, string> = {
      'NotStarted': 'badge-secondary',
      'InProgress': 'badge-warning',
      'Completed': 'badge-success',
      'Approved': 'badge-primary',
      'Rejected': 'badge-danger',
      'OnHold': 'badge-warning'
    };
    return statusMap[status] || 'badge-secondary';
  }

  formatDate(date?: Date): string {
    if (!date) return 'Not set';
    return new Date(date).toLocaleDateString('en-US', { 
      year: 'numeric', 
      month: 'short', 
      day: 'numeric' 
    });
  }

  getProgressColor(): string {
    if (!this.activity) return 'var(--secondary-color)';
    const progress = this.activity.weightPercentage;
    if (progress < 25) return 'var(--danger-color)';
    if (progress < 75) return 'var(--warning-color)';
    return 'var(--success-color)';
  }

  formatDateTime(date?: Date | string): string {
    if (!date) return 'Not set';
    return new Date(date).toLocaleString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  getStatusLabel(status: string): string {
    const statusMap: Record<string, string> = {
      'NotStarted': 'Not Started',
      'InProgress': 'In Progress',
      'Completed': 'Completed',
      'Approved': 'Approved',
      'Rejected': 'Rejected',
      'OnHold': 'On Hold',
      'Delayed': 'Delayed'
    };
    return statusMap[status] || status;
  }

  isWIRCheckpoint(): boolean {
    return this.activityDetail?.isWIRCheckpoint || false;
  }
}
