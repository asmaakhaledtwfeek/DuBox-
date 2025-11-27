import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { WIRService } from '../../../core/services/wir.service';
import { QualityIssueItem, QualityIssueDetails, QualityIssueStatus, UpdateQualityIssueStatusRequest, WIRCheckpoint, WIRCheckpointStatus } from '../../../core/models/wir.model';
import { FormsModule } from '@angular/forms';

type QcTab = 'checkpoints' | 'quality-issues';
type ChecklistNavigationQuery = { step?: string; from?: string };
type EnrichedCheckpoint = WIRCheckpoint & { projectCode?: string | null };
type AggregatedQualityIssue = QualityIssueItem & {
  issueId?: string;
  boxId?: string;
  wirNumber?: string;
  wirName?: string;
  boxTag?: string;
  boxName?: string;
  projectCode?: string | null;
  projectId?: string;
  checkpointStatus?: WIRCheckpointStatus;
  issueStatus?: string;
};

@Component({
  selector: 'app-quality-control-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent, ReactiveFormsModule, FormsModule],
  templateUrl: './quality-control-dashboard.component.html',
  styleUrl: './quality-control-dashboard.component.scss'
})
export class QualityControlDashboardComponent implements OnInit {
  activeTab: QcTab = 'checkpoints';
  readonly statusOptions = Object.values(WIRCheckpointStatus);
  
  qualityIssueStatusMeta: Record<QualityIssueStatus, { label: string; class: string }> = {
    Open: { label: 'OPEN', class: 'status-open' },
    InProgress: { label: 'IN PROGRESS', class: 'status-inprogress' },
    Resolved: { label: 'RESOLVED', class: 'status-resolved' },
    Closed: { label: 'CLOSED', class: 'status-closed' }
  };

  summaryCards = [
    { label: 'Open WIR Checkpoints', value: 0, tone: 'info' },
    { label: 'Pending Reviews', value: 0, tone: 'warning' },
    { label: 'Logged Issues', value: 0, tone: 'danger' },
    { label: 'Resolved Issues', value: 0, tone: 'success' }
  ];

  filterForm: FormGroup;
  qualityIssuesFilterForm: FormGroup;
  checkpoints: EnrichedCheckpoint[] = [];
  qualityIssues: AggregatedQualityIssue[] = [];
  filteredQualityIssues: AggregatedQualityIssue[] = [];
  checkpointsLoading = false;
  checkpointsError = '';
  qualityIssuesLoading = false;
  qualityIssuesError = '';

  // Modal state
  isDetailsModalOpen = false;
  isStatusModalOpen = false;
  selectedIssueDetails: QualityIssueDetails | null = null;
  selectedIssueForStatus: AggregatedQualityIssue | null = null;
  statusUpdateForm: {
    status: QualityIssueStatus;
    resolutionDescription: string;
    photoPath: string;
  } = {
    status: 'Open',
    resolutionDescription: '',
    photoPath: ''
  };
  statusUpdateLoading = false;
  statusUpdateError = '';
  qualityIssueStatuses: QualityIssueStatus[] = ['Open', 'InProgress', 'Resolved', 'Closed'];

  constructor(
    private fb: FormBuilder,
    private wirService: WIRService,
    private router: Router
  ) {
    this.filterForm = this.fb.group({
      projectCode: [''],
      boxTag: [''],
      status: [''],
      wirNumber: [''],
      from: [''],
      to: ['']
    });

    this.qualityIssuesFilterForm = this.fb.group({
      wirNumber: [''],
      boxTag: [''],
      projectCode: [''],
      status: [''],
      issueType: [''],
      severity: ['']
    });

    // Apply filters when form values change
    this.qualityIssuesFilterForm.valueChanges.subscribe(() => {
      this.applyQualityIssuesFilters();
    });
  }

  ngOnInit(): void {
    this.fetchCheckpoints();
  }

  setTab(tab: QcTab): void {
    this.activeTab = tab;
  }

  applyFilters(): void {
    this.fetchCheckpoints();
  }

  resetFilters(): void {
    this.filterForm.reset();
    this.fetchCheckpoints();
  }

  getStatusLabel(status?: WIRCheckpointStatus | string): string {
    if (!status) {
      return 'Pending';
    }

    const labels: Record<string, string> = {
      [WIRCheckpointStatus.Pending]: 'Pending',
      [WIRCheckpointStatus.Approved]: 'Approved',
      [WIRCheckpointStatus.Rejected]: 'Rejected',
      [WIRCheckpointStatus.ConditionalApproval]: 'Conditional Approval'
    };

    return labels[status] || status;
  }

  formatDate(date?: Date | string): string {
    if (!date) {
      return '—';
    }

    const parsed = date instanceof Date ? date : new Date(date);
    if (isNaN(parsed.getTime())) {
      return '—';
    }

    return parsed.toLocaleDateString('en-US', {
      month: 'short',
      day: '2-digit',
      year: 'numeric'
    });
  }

  canNavigateToAddChecklist(checkpoint: EnrichedCheckpoint | null): boolean {
    return !!(
      checkpoint &&
      checkpoint.wirId &&
      checkpoint.boxActivityId &&
      checkpoint.boxId &&
      (checkpoint.projectId || checkpoint.box?.projectId)
    );
  }

  canNavigateToReview(checkpoint: EnrichedCheckpoint | null): boolean {
    return !!(
      checkpoint &&
      checkpoint.boxActivityId &&
      checkpoint.boxId &&
      (checkpoint.projectId || checkpoint.box?.projectId)
    );
  }

  onAddChecklist(checkpoint: EnrichedCheckpoint): void {
    // Navigate to add-items step (Step 2) since checkpoint already exists
    this.navigateToCheckpoint(checkpoint, { step: 'add-items', from: 'quality-control' });
  }

  onReviewCheckpoint(checkpoint: EnrichedCheckpoint): void {
    // Determine the appropriate step based on checkpoint status
    // If checklist items exist, go to review step, otherwise go to add-items
    let targetStep: string = 'review';
    if (!checkpoint.checklistItems || checkpoint.checklistItems.length === 0) {
      targetStep = 'add-items'; // Need to add checklist items first
    }
    this.navigateToCheckpoint(checkpoint, { step: targetStep, from: 'quality-control' });
  }

  private navigateToCheckpoint(checkpoint: EnrichedCheckpoint, query?: ChecklistNavigationQuery): void {
    if (!this.canNavigateToAddChecklist(checkpoint)) {
      return;
    }

    const projectId = checkpoint.projectId || checkpoint.box?.projectId;
    if (!projectId) {
      return;
    }

    const route = [
      '/quality',
      'projects',
      projectId,
      'boxes',
      checkpoint.boxId,
      'activities',
      checkpoint.boxActivityId,
      'qa-qc'
    ];

    const queryParams = {
      ...(query?.step ? { step: query.step } : {}),
      ...(query?.from ? { from: query.from } : {})
    };

    this.router.navigate(route, { queryParams: Object.keys(queryParams).length ? queryParams : undefined });
  }

  getCheckpointStatusClass(status?: WIRCheckpointStatus | string): string {
    const classes: Record<string, string> = {
      [WIRCheckpointStatus.Pending]: 'pending',
      [WIRCheckpointStatus.Approved]: 'approved',
      [WIRCheckpointStatus.Rejected]: 'rejected',
      [WIRCheckpointStatus.ConditionalApproval]: 'conditional'
    };

    const key = (status || WIRCheckpointStatus.Pending).toString();
    return classes[key] || 'pending';
  }

  private fetchCheckpoints(): void {
    this.checkpointsLoading = true;
    this.qualityIssuesLoading = true;
    this.checkpointsError = '';
    this.qualityIssuesError = '';

    const filters = this.filterForm.value;
    const params = {
      projectCode: filters.projectCode || undefined,
      boxTag: filters.boxTag || undefined,
      status: filters.status || undefined,
      wirNumber: filters.wirNumber || undefined,
      from: filters.from || undefined,
      to: filters.to || undefined
    };

    this.wirService.getWIRCheckpoints(params).subscribe({
      next: (data) => {
        const checkpoints = (data || []) as EnrichedCheckpoint[];
        this.checkpoints = checkpoints;
        this.updateSummary(checkpoints);
        this.buildQualityIssuesList(checkpoints);
        this.checkpointsLoading = false;
        this.qualityIssuesLoading = false;
      },
      error: (err) => {
        console.error('❌ Failed to load WIR checkpoints:', err);
        this.checkpointsLoading = false;
        this.checkpointsError = err?.error?.message || err?.message || 'Failed to load WIR checkpoints';
        this.qualityIssuesLoading = false;
        this.qualityIssuesError = this.checkpointsError;
      }
    });
  }
  private updateSummary(checkpoints: EnrichedCheckpoint[]): void {
    const total = checkpoints.length;
    // Pending reviews should include both Pending and ConditionalApproval statuses
    const pendingReviews = checkpoints.filter(cp => 
      cp.status === WIRCheckpointStatus.Pending || 
      cp.status === WIRCheckpointStatus.ConditionalApproval
    ).length;
    const loggedIssues = checkpoints.reduce((total, cp) => total + (cp.qualityIssues?.length || 0), 0);
    const resolvedIssues = checkpoints.reduce((total, cp) => {
      const resolved = (cp.qualityIssues || []).filter(issue => (issue as any)?.status === 'Resolved').length;
      return total + resolved;
    }, 0);

    this.summaryCards = [
      { label: 'All WIR Checkpoints', value: total, tone: 'info' },
      { label: 'Pending Reviews', value: pendingReviews, tone: 'warning' },
      { label: 'Logged Issues', value: loggedIssues, tone: 'danger' },
      { label: 'Resolved Issues', value: resolvedIssues, tone: 'success' }
    ];
  }

  private buildQualityIssuesList(checkpoints: EnrichedCheckpoint[]): void {
    this.qualityIssues = checkpoints.flatMap(checkpoint =>
      (checkpoint.qualityIssues || []).map(issue => ({
        ...issue,
        boxId: checkpoint.boxId,
        wirNumber: checkpoint.wirNumber,
        wirName: checkpoint.wirName,
        boxTag: checkpoint.box?.boxTag,
        boxName: checkpoint.box?.boxName,
        projectCode: checkpoint.projectCode || checkpoint.box?.projectCode,
        projectId: checkpoint.projectId || checkpoint.box?.projectId,
        checkpointStatus: checkpoint.status,
        issueStatus: issue.status
      }))
    );
    // Apply filters after building the list
    this.applyQualityIssuesFilters();
  }

  applyQualityIssuesFilters(): void {
    const filters = this.qualityIssuesFilterForm.value;
    
    this.filteredQualityIssues = this.qualityIssues.filter(issue => {
      // Filter by WIR Number
      if (filters.wirNumber && filters.wirNumber.trim()) {
        const wirMatch = (issue.wirNumber || '').toLowerCase().includes(filters.wirNumber.toLowerCase().trim()) ||
                        (issue.wirName || '').toLowerCase().includes(filters.wirNumber.toLowerCase().trim());
        if (!wirMatch) return false;
      }

      // Filter by Box Tag
      if (filters.boxTag && filters.boxTag.trim()) {
        const boxTagMatch = (issue.boxTag || '').toLowerCase().includes(filters.boxTag.toLowerCase().trim());
        if (!boxTagMatch) return false;
      }

      // Filter by Project Code
      if (filters.projectCode && filters.projectCode.trim()) {
        const projectMatch = (issue.projectCode || '').toLowerCase().includes(filters.projectCode.toLowerCase().trim());
        if (!projectMatch) return false;
      }

      // Filter by Status
      if (filters.status && filters.status.trim()) {
        const issueStatus = (issue.issueStatus || issue.status || '').toString().toLowerCase();
        const filterStatus = filters.status.toLowerCase();
        if (issueStatus !== filterStatus) return false;
      }

      // Filter by Issue Type
      if (filters.issueType && filters.issueType.trim()) {
        const typeMatch = (issue.issueType || '').toLowerCase().includes(filters.issueType.toLowerCase().trim());
        if (!typeMatch) return false;
      }

      // Filter by Severity
      if (filters.severity && filters.severity.trim()) {
        const severityMatch = (issue.severity || '').toLowerCase() === filters.severity.toLowerCase().trim();
        if (!severityMatch) return false;
      }

      return true;
    });
  }

  resetQualityIssuesFilters(): void {
    this.qualityIssuesFilterForm.reset();
    this.applyQualityIssuesFilters();
  }

  getQualityIssueStatusLabel(status?: QualityIssueStatus | string): string {
    const normalized = (status || 'Open') as QualityIssueStatus;
    return this.qualityIssueStatusMeta[normalized]?.label || 'OPEN';
  }

  getQualityIssueStatusClass(status?: QualityIssueStatus | string): string {
    const normalized = (status || 'Open') as QualityIssueStatus;
    return this.qualityIssueStatusMeta[normalized]?.class || 'status-open';
  }

  openIssueDetails(issue: AggregatedQualityIssue): void {
    const boxId = issue.boxId;

    // If we have issueId and boxId, fetch full details; otherwise use aggregated data
    if (issue.issueId && boxId) {
      // Fetch full details for better information
      this.wirService.getQualityIssuesByBox(boxId).subscribe({
        next: (issues) => {
          const fullIssue = issues.find(i => i.issueId === issue.issueId);
          if (fullIssue) {
            this.selectedIssueDetails = fullIssue;
          } else {
            // Fallback to aggregated data converted to QualityIssueDetails
            this.selectedIssueDetails = this.convertToQualityIssueDetails(issue, boxId);
          }
          this.isDetailsModalOpen = true;
        },
        error: (err) => {
          console.error('Failed to fetch issue details:', err);
          // Fallback to aggregated data
          this.selectedIssueDetails = this.convertToQualityIssueDetails(issue, boxId);
          this.isDetailsModalOpen = true;
        }
      });
    } else {
      // Use aggregated data directly
      this.selectedIssueDetails = this.convertToQualityIssueDetails(issue, boxId);
      this.isDetailsModalOpen = true;
    }
  }

  closeIssueDetails(): void {
    this.isDetailsModalOpen = false;
    this.selectedIssueDetails = null;
  }

  openStatusModal(issue: AggregatedQualityIssue): void {
    if (this.isDetailsModalOpen) {
      this.closeIssueDetails();
    }

    if (!issue.issueId) {
      console.error('Cannot update status: issueId is missing');
      return;
    }

    this.selectedIssueForStatus = issue;
    this.statusUpdateForm = {
      status: (issue.issueStatus || issue.status || 'Open') as QualityIssueStatus,
      resolutionDescription: '',
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
    if (!this.selectedIssueForStatus || !this.selectedIssueForStatus.issueId) {
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
    if (!this.selectedIssueForStatus || !this.selectedIssueForStatus.issueId || !this.canSubmitStatusUpdate() || this.statusUpdateLoading) {
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
      issue.issueId === updated.issueId ? { ...issue, ...updated, issueStatus: updated.status } : issue
    );
    // Update summary
    this.updateSummary(this.checkpoints);
  }

  private convertToQualityIssueDetails(issue: AggregatedQualityIssue, boxId?: string): QualityIssueDetails {
    return {
      issueId: issue.issueId || '',
      issueType: issue.issueType,
      severity: issue.severity,
      issueDescription: issue.issueDescription,
      assignedTo: issue.assignedTo,
      dueDate: issue.dueDate,
      photoPath: issue.photoPath,
      reportedBy: issue.reportedBy,
      issueDate: issue.issueDate,
      status: (issue.issueStatus || issue.status || 'Open') as QualityIssueStatus,
      boxId: boxId,
      boxName: issue.boxName,
      boxTag: issue.boxTag,
      wirId: undefined,
      wirNumber: issue.wirNumber,
      wirName: issue.wirName
    };
  }

  formatIssueDate(date?: string | Date): string {
    if (!date) return '—';
    const d = date instanceof Date ? date : new Date(date);
    if (isNaN(d.getTime())) return '—';
    return d.toISOString().split('T')[0];
  }
}

