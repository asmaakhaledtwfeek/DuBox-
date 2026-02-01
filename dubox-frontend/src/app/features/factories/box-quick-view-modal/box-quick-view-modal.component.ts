import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxActivity, BoxPanel, PanelStatus } from '../../../core/models/box.model';

@Component({
  selector: 'app-box-quick-view-modal',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './box-quick-view-modal.component.html',
  styleUrls: ['./box-quick-view-modal.component.scss']
})
export class BoxQuickViewModalComponent implements OnInit, OnDestroy {
  @Input() boxId: string = '';
  @Input() projectId: string = '';
  @Output() close = new EventEmitter<void>();
  
  box: Box | null = null;
  loading = true;
  error = '';
  
  // Panel status handling
  readonly PanelStatus = PanelStatus;
  boxPanels: BoxPanel[] = [];
  showLegend = false;
  
  // Pod Delivery
  podDeliverChecked = false;
  podName: string = '';
  podType: string = '';
  
  private subscriptions: Subscription[] = [];

  constructor(
    private boxService: BoxService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (this.boxId) {
      this.loadBox();
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadBox(): void {
    this.loading = true;
    this.error = '';
    
    const boxSub = this.boxService.getBox(this.boxId).subscribe({
      next: (box) => {
        this.box = box;
        this.initializeDeliveryInfo(box);
        
        // Load activities if not already loaded
        if (!box.activities || box.activities.length === 0) {
          this.loadActivities();
        } else {
          this.loading = false;
        }
      },
      error: (err) => {
        this.error = err.message || 'Failed to load box details';
        this.loading = false;
        console.error('Error loading box:', err);
      }
    });

    this.subscriptions.push(boxSub);
  }

  loadActivities(): void {
    if (!this.boxId) return;
    
    const activitiesSub = this.boxService.getBoxActivities(this.boxId).subscribe({
      next: (activities) => {
        if (this.box) {
          this.box.activities = activities;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading activities:', err);
        this.loading = false;
      }
    });

    this.subscriptions.push(activitiesSub);
  }

  private initializeDeliveryInfo(box: Box): void {
    // Initialize box panels (for walls/panels with statuses)
    this.boxPanels = box.boxPanels || [];
    
    // Initialize slab and soffit (these remain as simple checkboxes)
  
    // Initialize pod delivery
    this.podDeliverChecked = box.podDeliver ?? false;
    this.podName = box.podName ?? '';
    this.podType = box.podType ?? '';
  }

  /**
   * Normalize panel status to handle both number and enum comparisons
   */
  private normalizePanelStatus(status: unknown): PanelStatus | null {
    if (status === null || status === undefined) {
      return null;
    }

    // Already a number (or numeric string)
    if (typeof status === 'number') {
      return status as PanelStatus;
    }
    if (typeof status === 'string') {
      const trimmed = status.trim();
      const asNumber = Number(trimmed);
      if (!Number.isNaN(asNumber)) {
        return asNumber as PanelStatus;
      }

      // Backend may send enum name strings
      const key = trimmed.replace(/\s+/g, '').toLowerCase();
      const map: Record<string, PanelStatus> = {
        notstarted: PanelStatus.NotStarted,
        firstapprovalapproved: PanelStatus.FirstApprovalApproved,
        secondapprovalapproved: PanelStatus.SecondApprovalApproved,
        secondapprovalrejected: PanelStatus.SecondApprovalRejected
      };

      return map[key] ?? null;
    }

    return null;
  }

  /**
   * Get panel status label
   */
  getPanelStatusLabel(status: unknown): string {
    const normalized = this.normalizePanelStatus(status);
    if (normalized === null) {
      return 'Unknown';
    }

    switch (normalized) {
      case PanelStatus.NotStarted:
        return 'Not Started';
      case PanelStatus.FirstApprovalApproved:
        return 'First Approval Approved';
      case PanelStatus.SecondApprovalApproved:
        return 'Second Approval Approved';
      case PanelStatus.SecondApprovalRejected:
        return 'Second Approval Rejected';
      default:
        return 'Unknown';
    }
  }

  /**
   * Check if panel status matches (handles both number and enum comparisons)
   */
  isPanelStatus(panelStatus: unknown, status: PanelStatus): boolean {
    const normalized = this.normalizePanelStatus(panelStatus);
    return normalized !== null && normalized === status;
  }

  /**
   * Get CSS class for panel status text based on status
   */
  getPanelStatusTextClass(status: unknown): string {
    const statusNum = this.normalizePanelStatus(status);
    if (statusNum === null) {
      return 'status-text-default';
    }
    
    if (statusNum === PanelStatus.NotStarted) return 'status-text-not-started'; // Gray
    if (statusNum === PanelStatus.FirstApprovalApproved) return 'status-text-first-approval-approved'; // Yellow
    if (statusNum === PanelStatus.SecondApprovalApproved) return 'status-text-second-approval-approved'; // Green
    if (statusNum === PanelStatus.SecondApprovalRejected) return 'status-text-second-approval-rejected'; // Red
    
    return 'status-text-default';
  }

  /**
   * Toggle legend visibility
   */
  toggleLegend(): void {
    this.showLegend = !this.showLegend;
  }

  // Activity chart methods
  getActivityCountByStatus(status: string): number {
    if (!this.box || !this.box.activities) {
      return 0;
    }
    
    const normalizedStatus = status.toString().toLowerCase();
    return this.box.activities.filter(activity => {
      if (!activity.status) return false;
      const activityStatus = activity.status.toString().toLowerCase();
      
      if (normalizedStatus === 'completed') {
        const isCompleted = activityStatus === 'completed';
        const isDelayedWith100Progress = activityStatus === 'delayed' && 
                                         (activity.weightPercentage >= 100 || 
                                          (activity as any).progressPercentage >= 100);
        return isCompleted || isDelayedWith100Progress;
      }
      
      return activityStatus === normalizedStatus;
    }).length;
  }

  getActivityPercentage(status: string): number {
    if (!this.box || !this.box.activities || this.box.activities.length === 0) return 0;
    const count = this.getActivityCountByStatus(status);
    return Math.round((count / this.box.activities.length) * 100);
  }

  getCircleSegment(status: string): string {
    const percentage = this.getActivityPercentage(status);
    const circumference = 2 * Math.PI * 80;
    const segmentLength = (percentage / 100) * circumference;
    return `${segmentLength} ${circumference}`;
  }

  getCircleOffset(status: string): number {
    const circumference = 2 * Math.PI * 80;
    let offset = 0;
    
    if (status === 'InProgress') {
      const completedPercentage = this.getActivityPercentage('Completed');
      offset = -((completedPercentage / 100) * circumference);
    } else if (status === 'NotStarted') {
      const completedPercentage = this.getActivityPercentage('Completed');
      const inProgressPercentage = this.getActivityPercentage('InProgress');
      offset = -(((completedPercentage + inProgressPercentage) / 100) * circumference);
    }
    
    return offset;
  }

  viewFullDetails(): void {
    if (this.box && this.box.projectId) {
      this.router.navigate(['/projects', this.box.projectId, 'boxes', this.box.id]);
    }
  }

  closeModal(): void {
    this.close.emit();
  }
}
