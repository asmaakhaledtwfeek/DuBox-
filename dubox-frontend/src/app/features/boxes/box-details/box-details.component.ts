import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ActivityTableComponent } from '../../activities/activity-table/activity-table.component';

@Component({
  selector: 'app-box-details',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent, ActivityTableComponent],
  templateUrl: './box-details.component.html',
  styleUrls: ['./box-details.component.scss']
})
export class BoxDetailsComponent implements OnInit {
  box: Box | null = null;
  boxId!: string;
  projectId!: string;
  loading = true;
  error = '';
  deleting = false;
  showDeleteConfirm = false;
  
  activeTab: 'overview' | 'activities' | 'logs' | 'attachments' = 'overview';
  
  canEdit = false;
  canDelete = false;
  BoxStatus = BoxStatus;
  
  actualActivityCount: number = 0; // Actual count from activity table (excluding WIR rows)

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxService: BoxService,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.boxId = this.route.snapshot.params['boxId'];
    this.projectId = this.route.snapshot.params['projectId'];
    
    this.canEdit = this.permissionService.canEdit('boxes');
    this.canDelete = this.permissionService.canDelete('boxes');
    
    this.loadBox();
  }

  loadBox(): void {
    this.loading = true;
    this.error = '';
    
    this.boxService.getBox(this.boxId).subscribe({
      next: (box) => {
        this.box = box;
        
        // Generate QR code if it doesn't exist
        if (!box.qrCode) {
          console.log('QR code not found, generating...');
          this.generateQRCode();
        }
        
        // Load activities separately
        this.loadActivities();
        
        this.loading = false;
      },
      error: (err) => {
        this.error = err.message || 'Failed to load box details';
        this.loading = false;
        console.error('Error loading box:', err);
      }
    });
  }

  loadActivities(): void {
    console.log('📡 Loading activities for box:', this.boxId);
    this.boxService.getBoxActivities(this.boxId).subscribe({
      next: (activities) => {
        console.log('📦 Raw activities received:', activities);
        if (this.box) {
          this.box.activities = activities;
          console.log('✅ Loaded activities:', activities.length);
          
          // Log first activity details for debugging
          if (activities.length > 0) {
            const firstActivity = activities[0];
            console.log('🔍 First activity details:', {
              id: firstActivity.id,
              name: firstActivity.name,
              status: firstActivity.status,
              allKeys: Object.keys(firstActivity)
            });
          } else {
            console.warn('⚠️ No activities returned from API');
          }
        }
      },
      error: (err) => {
        console.error('❌ Error loading activities:', err);
        console.error('❌ Full error:', JSON.stringify(err, null, 2));
        // Don't show error to user, just log it
      }
    });
  }

  generateQRCode(): void {
    this.boxService.generateQRCode(this.boxId).subscribe({
      next: (base64String) => {
        if (this.box && base64String) {
          // Convert base64 string to data URL for image display
          this.box.qrCode = `data:image/png;base64,${base64String}`;
          console.log('✅ QR code generated successfully');
        }
      },
      error: (err) => {
        console.error('❌ Failed to generate QR code:', err);
        // Don't show error to user, just log it
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes']);
  }

  editBox(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId, 'edit']);
  }

  openDeleteConfirm(): void {
    this.showDeleteConfirm = true;
  }

  cancelDelete(): void {
    this.showDeleteConfirm = false;
  }

  deleteBox(): void {
    if (this.deleting) {
      return;
    }

    this.deleting = true;
    this.error = '';
    this.boxService.deleteBox(this.boxId).subscribe({
      next: () => {
        this.showDeleteConfirm = false;
        this.deleting = false;
        this.router.navigate(['/projects', this.projectId, 'boxes']);
      },
      error: (err) => {
        this.deleting = false;
        this.error = err.message || 'Failed to delete box';
        this.showDeleteConfirm = false;
        console.error('Error deleting box:', err);
      }
    });
  }

  downloadQRCode(): void {
    if (this.box?.qrCode) {
      const link = document.createElement('a');
      link.href = this.box.qrCode;
      link.download = `QR-${this.box.code}.png`;
      link.click();
    }
  }

  setActiveTab(tab: 'overview' | 'activities' | 'logs' | 'attachments'): void {
    this.activeTab = tab;
  }

  getStatusClass(status: BoxStatus): string {
    const statusMap: Record<BoxStatus, string> = {
      [BoxStatus.NotStarted]: 'badge-secondary',
      [BoxStatus.InProgress]: 'badge-warning',
      [BoxStatus.QAReview]: 'badge-info',
      [BoxStatus.Completed]: 'badge-success',
      [BoxStatus.ReadyForDelivery]: 'badge-primary',
      [BoxStatus.Delivered]: 'badge-success',
      [BoxStatus.OnHold]: 'badge-danger'
    };
    return statusMap[status] || 'badge-secondary';
  }

  getStatusLabel(status: BoxStatus): string {
    const labels: Record<BoxStatus, string> = {
      [BoxStatus.NotStarted]: 'Not Started',
      [BoxStatus.InProgress]: 'In Progress',
      [BoxStatus.QAReview]: 'QA Review',
      [BoxStatus.Completed]: 'Completed',
      [BoxStatus.ReadyForDelivery]: 'Ready for Delivery',
      [BoxStatus.Delivered]: 'Delivered',
      [BoxStatus.OnHold]: 'On Hold'
    };
    return labels[status] || status;
  }

  onActivityCountChanged(count: number): void {
    this.actualActivityCount = count;
    console.log(`📊 Activity count updated: ${count}`);
  }
}
