import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { BoxService } from '../../../core/services/box.service';
import { BoxActivity } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-activity-details',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './activity-details.component.html',
  styleUrls: ['./activity-details.component.scss']
})
export class ActivityDetailsComponent implements OnInit {
  activity: BoxActivity | null = null;
  activityId!: string;
  projectId!: string;
  boxId!: string;
  loading = true;
  error = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxService: BoxService
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
    
    this.boxService.getActivityDetails(this.activityId).subscribe({
      next: (activity) => {
        this.activity = activity;
        this.loading = false;
        console.log('✅ Activity loaded:', activity);
      },
      error: (err) => {
        this.error = err.message || 'Failed to load activity details';
        this.loading = false;
        console.error('❌ Error loading activity:', err);
      }
    });
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
}
