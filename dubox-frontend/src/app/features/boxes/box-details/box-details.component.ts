import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { WIRService } from '../../../core/services/wir.service';
import { QualityIssueDetails, QualityIssueStatus, UpdateQualityIssueStatusRequest, WIRCheckpoint, WIRCheckpointStatus, WIRRecord } from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ActivityTableComponent } from '../../activities/activity-table/activity-table.component';

@Component({
  selector: 'app-box-details',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent, ActivityTableComponent],
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
  
  activeTab: 'overview' | 'activities' | 'wir' | 'quality-issues' | 'logs' | 'attachments' = 'overview';
  
  canEdit = false;
  canDelete = false;
  BoxStatus = BoxStatus;
  
  actualActivityCount: number = 0; // Actual count from activity table (excluding WIR rows)
  wirCheckpoints: WIRCheckpoint[] = [];
  wirLoading = false;
  wirError = '';
  qualityIssueCount = 0;
  qualityIssues: QualityIssueDetails[] = [];
  qualityIssuesLoading = false;
  qualityIssuesError = '';
  qualityIssueStatuses: QualityIssueStatus[] = ['Open', 'InProgress', 'Resolved', 'Closed'];
  qualityIssueStatusMeta: Record<QualityIssueStatus, { label: string; class: string }> = {
    Open: { label: 'Open', class: 'status-open' },
    InProgress: { label: 'In Progress', class: 'status-inprogress' },
    Resolved: { label: 'Resolved', class: 'status-resolved' },
    Closed: { label: 'Closed', class: 'status-closed' }
  };
  isStatusModalOpen = false;
  selectedIssueForStatus: QualityIssueDetails | null = null;
  statusUpdateLoading = false;
  statusUpdateError = '';
  statusUpdateForm: {
    status: QualityIssueStatus;
    resolutionDescription: string;
    photoPath: string;
  } = {
    status: 'Open',
    resolutionDescription: '',
    photoPath: ''
  };
  isDetailsModalOpen = false;
  selectedIssueDetails: QualityIssueDetails | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxService: BoxService,
    private permissionService: PermissionService,
    private wirService: WIRService
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
        this.loadWIRCheckpoints();
        this.loadQualityIssues();
        
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

  loadWIRCheckpoints(): void {
    if (!this.boxId) {
      return;
    }

    this.wirLoading = true;
    this.wirError = '';

        this.wirService.getWIRCheckpointsByBox(this.boxId).subscribe({
      next: (checkpoints) => {
        this.wirCheckpoints = checkpoints || [];
        if (!this.wirCheckpoints.length) {
          this.wirLoading = false;
          return;
        }
        this.attachActivitiesToWIRCheckpoints();
      },
      error: (err) => {
        console.error('❌ Error loading WIR checkpoints:', err);
        this.wirError = err?.error?.message || err?.message || 'Failed to load WIR checkpoints';
        this.wirLoading = false;
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

  setActiveTab(tab: 'overview' | 'activities' | 'wir' | 'quality-issues' | 'logs' | 'attachments'): void {
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

  refreshWIRCheckpoints(): void {
    this.loadWIRCheckpoints();
  }

  private attachActivitiesToWIRCheckpoints(): void {
    this.wirService.getWIRRecordsByBox(this.boxId).subscribe({
      next: (wirRecords) => {
        const wirMap = new Map<string, WIRRecord>();
        (wirRecords || []).forEach(record => {
          const key = (record?.wirCode || '').toLowerCase();
          if (key) {
            wirMap.set(key, record);
          }
        });

        this.wirCheckpoints = this.wirCheckpoints.map(checkpoint => {
          if (checkpoint.boxActivityId) {
            return checkpoint;
          }

          const mapKey = (checkpoint.wirNumber || '').toLowerCase();
          const matchedRecord = mapKey ? wirMap.get(mapKey) : undefined;
          if (matchedRecord?.boxActivityId) {
            return {
              ...checkpoint,
              boxActivityId: matchedRecord.boxActivityId
            };
          }

          return checkpoint;
        });

        this.wirLoading = false;
      },
      error: (err) => {
        console.error('❌ Error loading WIR records for checkpoint mapping:', err);
        this.wirLoading = false;
      }
    });
  }

  getWIRCheckpointStatusClass(status?: WIRCheckpointStatus | string): string {
    const normalized = (status || WIRCheckpointStatus.Pending).toString();
    const statusMap: Record<string, string> = {
      [WIRCheckpointStatus.Pending]: 'wir-status-pending',
      [WIRCheckpointStatus.Approved]: 'wir-status-approved',
      [WIRCheckpointStatus.Rejected]: 'wir-status-rejected',
      [WIRCheckpointStatus.ConditionalApproval]: 'wir-status-conditional'
    };

    return `wir-status-badge ${statusMap[normalized] || 'wir-status-pending'}`;
  }

  getWIRCheckpointStatusLabel(status?: WIRCheckpointStatus | string): string {
    const normalized = (status || WIRCheckpointStatus.Pending).toString();
    const labelMap: Record<string, string> = {
      [WIRCheckpointStatus.Pending]: 'Pending Review',
      [WIRCheckpointStatus.Approved]: 'Approved',
      [WIRCheckpointStatus.Rejected]: 'Rejected',
      [WIRCheckpointStatus.ConditionalApproval]: 'Conditional'
    };

    return labelMap[normalized] || 'Pending Review';
  }

  formatWIRDate(date?: Date): string {
    if (!date) {
      return '—';
    }

    const parsed = date instanceof Date ? date : new Date(date);
    if (isNaN(parsed.getTime())) {
      return '—';
    }

    return parsed.toLocaleDateString('en-US', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  canNavigateToAddChecklist(checkpoint: WIRCheckpoint | null): boolean {
    return !!checkpoint?.wirId && !!checkpoint.boxActivityId;
  }

  canNavigateToReview(checkpoint: WIRCheckpoint | null): boolean {
    return !!checkpoint?.boxActivityId;
  }

  navigateToAddChecklist(checkpoint: WIRCheckpoint): void {
    if (!this.canNavigateToAddChecklist(checkpoint)) {
      return;
    }

    this.router.navigate([
      '/projects',
      this.projectId,
      'boxes',
      this.boxId,
      'activities',
      checkpoint.boxActivityId,
      'qa-qc'
    ]);
  }

  navigateToReview(checkpoint: WIRCheckpoint): void {
    if (!this.canNavigateToReview(checkpoint)) {
      return;
    }

    this.router.navigate([
      '/projects',
      this.projectId,
      'boxes',
      this.boxId,
      'activities',
      checkpoint.boxActivityId,
      'qa-qc'
    ]);
  }

  loadQualityIssues(): void {
    if (!this.boxId) {
      return;
    }

    this.qualityIssuesLoading = true;
    this.qualityIssuesError = '';

    this.wirService.getQualityIssuesByBox(this.boxId).subscribe({
      next: (issues) => {
        this.qualityIssues = issues || [];
        this.qualityIssueCount = this.qualityIssues.length;
        this.qualityIssuesLoading = false;
      },
      error: (err) => {
        console.error('❌ Error loading quality issues for box:', err);
        this.qualityIssuesError = err?.error?.message || err?.message || 'Failed to load quality issues';
        this.qualityIssuesLoading = false;
      }
    });
  }

  refreshQualityIssues(): void {
    this.loadQualityIssues();
  }

  formatIssueDate(date?: string | Date): string {
    if (!date) {
      return '—';
    }

    const parsed = date instanceof Date ? date : new Date(date);
    if (isNaN(parsed.getTime())) {
      return '—';
    }

    return parsed.toISOString().split('T')[0];
  }

  getQualityIssueWir(issue: QualityIssueDetails): string {
    if (issue.wirNumber) {
      return issue.wirNumber;
    }

    if (issue.wirId) {
      const matchedCheckpoint = this.wirCheckpoints.find(cp => cp.wirId === issue.wirId);
      if (matchedCheckpoint?.wirNumber) {
        return matchedCheckpoint.wirNumber;
      }
    }

    return issue.wirName || '—';
  }

  getQualityIssueStatusLabel(status?: QualityIssueStatus | string): string {
    const normalized = (status || 'Open') as QualityIssueStatus;
    return this.qualityIssueStatusMeta[normalized]?.label || 'Open';
  }

  getQualityIssueStatusClass(status?: QualityIssueStatus | string): string {
    const normalized = (status || 'Open') as QualityIssueStatus;
    return this.qualityIssueStatusMeta[normalized]?.class || 'status-open';
  }

  openIssueDetails(issue: QualityIssueDetails): void {
    this.selectedIssueDetails = issue;
    this.isDetailsModalOpen = true;
  }

  closeIssueDetails(): void {
    this.isDetailsModalOpen = false;
    this.selectedIssueDetails = null;
  }

  openStatusModal(issue: QualityIssueDetails): void {
    if (this.isDetailsModalOpen) {
      this.closeIssueDetails();
    }

    this.selectedIssueForStatus = issue;
    this.statusUpdateForm = {
      status: issue.status || 'Open',
      resolutionDescription: issue.resolutionDescription || '',
      photoPath: issue.photoPath || ''
    };
    this.statusUpdateError = '';
    this.isStatusModalOpen = true;
  }

  closeStatusModal(): void {
    this.isStatusModalOpen = false;
    this.selectedIssueForStatus = null;
    this.statusUpdateLoading = false;
    this.statusUpdateError = '';
  }

  requiresResolutionDescription(status: QualityIssueStatus | string | undefined): boolean {
    return status === 'Resolved' || status === 'Closed';
  }

  canSubmitStatusUpdate(): boolean {
    if (!this.selectedIssueForStatus) {
      return false;
    }

    if (!this.statusUpdateForm.status) {
      return false;
    }

    if (this.requiresResolutionDescription(this.statusUpdateForm.status)) {
      return !!this.statusUpdateForm.resolutionDescription?.trim();
    }

    return true;
  }

  submitStatusUpdate(): void {
    if (!this.selectedIssueForStatus || !this.canSubmitStatusUpdate() || this.statusUpdateLoading) {
      return;
    }

    const payload: UpdateQualityIssueStatusRequest = {
      issueId: this.selectedIssueForStatus.issueId,
      status: this.statusUpdateForm.status,
      resolutionDescription: this.requiresResolutionDescription(this.statusUpdateForm.status)
        ? this.statusUpdateForm.resolutionDescription?.trim()
        : null,
      photoPath: this.statusUpdateForm.photoPath?.trim() || null
    };

    this.statusUpdateLoading = true;
    this.statusUpdateError = '';

    this.wirService.updateQualityIssueStatus(payload.issueId, payload).subscribe({
      next: (updatedIssue) => {
        this.statusUpdateLoading = false;
        this.applyUpdatedQualityIssue(updatedIssue);
        this.closeStatusModal();
      },
      error: (err) => {
        console.error('❌ Failed to update quality issue status:', err);
        this.statusUpdateLoading = false;
        this.statusUpdateError = err?.error?.message || err?.message || 'Failed to update issue status';
      }
    });
  }

  private applyUpdatedQualityIssue(updated: QualityIssueDetails): void {
    this.qualityIssues = this.qualityIssues.map(issue =>
      issue.issueId === updated.issueId ? { ...issue, ...updated } : issue
    );
    const count = this.qualityIssues.length;
    this.qualityIssueCount = count;
  }
}
