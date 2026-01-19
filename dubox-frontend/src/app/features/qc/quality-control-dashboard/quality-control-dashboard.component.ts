import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { QualityIssueDetailsModalComponent } from '../../../shared/components/quality-issue-details-modal/quality-issue-details-modal.component';
import { AssignToCrewModalComponent, AssignableIssue } from '../../../shared/components/assign-to-crew-modal/assign-to-crew-modal.component';
import { IssueCommentsComponent } from '../../../shared/components/issue-comments/issue-comments.component';
import { WIRService } from '../../../core/services/wir.service';
import { QualityIssueItem, QualityIssueDetails, QualityIssueStatus, UpdateQualityIssueStatusRequest, WIRCheckpoint, WIRCheckpointStatus } from '../../../core/models/wir.model';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { PermissionService } from '../../../core/services/permission.service';
import { AuthService } from '../../../core/services/auth.service';
import { TeamService } from '../../../core/services/team.service';
import { Team, TeamMember } from '../../../core/models/team.model';
import { BoxService } from '../../../core/services/box.service';
import { BoxStatus } from '../../../core/models/box.model';
import { ProjectService } from '../../../core/services/project.service';
import { ProjectStatus } from '../../../core/models/project.model';
import { map } from 'rxjs/operators';
import * as ExcelJS from 'exceljs';
import { environment } from '../../../../environments/environment';

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
  projectName?: string | null;
  projectId?: string;
  checkpointStatus?: WIRCheckpointStatus;
  issueStatus?: string;
  assignedTeamName?: string;
};

@Component({
  selector: 'app-quality-control-dashboard',
  standalone: true,
  imports: [HeaderComponent, CommonModule, RouterModule, SidebarComponent, ReactiveFormsModule, FormsModule, QualityIssueDetailsModalComponent, AssignToCrewModalComponent, IssueCommentsComponent],
  templateUrl: './quality-control-dashboard.component.html',
  styleUrl: './quality-control-dashboard.component.scss'
})
export class QualityControlDashboardComponent implements OnInit, OnDestroy {
  activeTab: QcTab = 'checkpoints';
  readonly statusOptions = Object.values(WIRCheckpointStatus);
  
  qualityIssueStatusMeta: Record<QualityIssueStatus, { label: string; class: string }> = {
    Open: { label: 'OPEN', class: 'status-open' },
    InProgress: { label: 'IN PROGRESS', class: 'status-inprogress' },
    Resolved: { label: 'RESOLVED', class: 'status-resolved' },
    Closed: { label: 'CLOSED', class: 'status-closed' }
  };

  // WIR Checkpoint Summary Cards
  wirCheckpointSummaryCards = [
    { label: 'All WIR Checkpoints', value: 0, tone: 'info' },
    { label: 'Pending Reviews', value: 0, tone: 'warning' },
    { label: 'Approved', value: 0, tone: 'success' },
    { label: 'Conditional Approval', value: 0, tone: 'warning' },
    { label: 'Rejected', value: 0, tone: 'danger' }
  ];

  // Quality Issue Summary Cards
  qualityIssueSummaryCards = [
    { label: 'Open Issues', value: 0, tone: 'danger' },
    { label: 'In Progress', value: 0, tone: 'warning' },
    { label: 'Resolved Issues', value: 0, tone: 'success' },
    { label: 'Closed Issues', value: 0, tone: 'info' }
  ];

  // Store original summary counts (these remain unchanged when filtering)
  private originalWirCheckpointSummary = [
    { label: 'All WIR Checkpoints', value: 0, tone: 'info' },
    { label: 'Pending Reviews', value: 0, tone: 'warning' },
    { label: 'Approved', value: 0, tone: 'success' },
    { label: 'Conditional Approval', value: 0, tone: 'warning' },
    { label: 'Rejected', value: 0, tone: 'danger' }
  ];

  private originalQualityIssueSummary = [
    { label: 'Open Issues', value: 0, tone: 'danger' },
    { label: 'In Progress', value: 0, tone: 'warning' },
    { label: 'Resolved Issues', value: 0, tone: 'success' },
    { label: 'Closed Issues', value: 0, tone: 'info' }
  ];

  // Track active KPI card for visual indication
  activeKpiCard: string | null = null;

  // Getter to return appropriate summary cards based on active tab
  get summaryCards() {
    return this.activeTab === 'checkpoints' 
      ? this.wirCheckpointSummaryCards 
      : this.qualityIssueSummaryCards;
  }

  filterForm: FormGroup;
  qualityIssuesFilterForm: FormGroup;
  checkpoints: EnrichedCheckpoint[] = [];
  qualityIssues: AggregatedQualityIssue[] = [];
  allQualityIssues: AggregatedQualityIssue[] = []; // Unfiltered list for extracting unique values
  checkpointsLoading = false;
  checkpointsError = '';
  qualityIssuesLoading = false;
  qualityIssuesError = '';
  
  // Pagination for checkpoints (backend pagination)
  checkpointsCurrentPage = 1;
  checkpointsPageSize = 25;
  checkpointsTotalCount = 0;
  checkpointsTotalPages = 0;
  
  // Pagination for quality issues (backend pagination)
  qualityIssuesCurrentPage = 1;
  qualityIssuesPageSize = 25;
  qualityIssuesTotalCount = 0;
  qualityIssuesTotalPages = 0;
  
  // Flag to prevent double API calls when resetting filters
  private isResettingFilters = false;
  private isResettingCheckpointFilters = false;

  // System Admin flag
  isSystemAdmin = false;

  // Modal state
  isDetailsModalOpen = false;
  isStatusModalOpen = false;
  selectedIssueDetails: QualityIssueDetails | null = null;
  selectedIssueForStatus: AggregatedQualityIssue | null = null;
  statusUpdateForm: {
    status: QualityIssueStatus;
    resolutionDescription: string;
  } = {
    status: 'Open',
    resolutionDescription: ''
  };
  statusUpdateLoading = false;
  statusUpdateError = '';
  qualityIssueStatuses: QualityIssueStatus[] = ['Open', 'InProgress', 'Resolved', 'Closed'];
  uniqueProjects: Array<{ id: string; code: string; name: string }> = [];
  uniqueAssignedUsers: Array<{ id: string; name: string; teamName?: string }> = [];
  
  // Multiple images state
  selectedImages: Array<{
    id: string;
    type: 'file' | 'url' | 'camera';
    file?: File;
    url?: string;
    preview: string;
    name?: string;
    size?: number;
  }> = [];
  currentImageInputMode: 'url' | 'upload' | 'camera' = 'url';
  currentUrlInput: string = '';
  isUploadingPhoto = false;
  photoUploadError = '';
  cameraStream: MediaStream | null = null;
  showCamera = false;

  // Assign modal state
  isAssignModalOpen = false;
  selectedIssueForAssign: AggregatedQualityIssue | null = null;
  assignLoading = false;

  // Comments section state
  showComments = true;

  // Cache for box statuses to avoid multiple API calls
  private boxStatusCache: Map<string, BoxStatus | null> = new Map();
  private boxStatusLoading: Set<string> = new Set();
  
  // Cache for project statuses to avoid multiple API calls
  private projectStatusCache: Map<string, ProjectStatus | null> = new Map();
  private projectStatusLoading: Set<string> = new Set();

  constructor(
    private fb: FormBuilder,
    private wirService: WIRService,
    private router: Router,
    private apiService: ApiService,
    private permissionService: PermissionService,
    private authService: AuthService,
    private teamService: TeamService,
    private boxService: BoxService,
    private projectService: ProjectService
  ) {
    this.isSystemAdmin = this.authService.isSystemAdmin();
    this.filterForm = this.fb.group({
      projectCode: [''],
      boxTag: [''],
      status: [''],
      wirNumber: [''],
      from: [''],
      to: ['']
    });

    this.qualityIssuesFilterForm = this.fb.group({
      issueNumber: [''],
      boxTag: [''],
      projectCode: [''],
      status: [''],
      issueType: [''],
      severity: [''],
      assignedUser: ['']
    });

    // Apply filters when form values change - trigger API call with backend pagination
    // Use debounce to avoid too many API calls while user is typing
    this.qualityIssuesFilterForm.valueChanges.subscribe(() => {
      // Skip if form is being reset programmatically
      if (!this.isResettingFilters) {
        this.qualityIssuesCurrentPage = 1; // Reset to first page when filters change
        this.fetchAllQualityIssues();
      }
    });
  }

  // Permission getters for template
  get canUpdateQualityIssueStatus(): boolean {
    return this.permissionService.hasPermission('quality-issues', 'edit') || 
           this.permissionService.hasPermission('quality-issues', 'resolve');
  }

  get canManageWIRCheckpoints(): boolean {
    return this.permissionService.hasPermission('wir', 'create') || 
           this.permissionService.hasPermission('wir', 'review') ||
           this.permissionService.hasPermission('wir', 'manage');
  }

  ngOnInit(): void {
    this.fetchCheckpoints();
  }

  setTab(tab: QcTab): void {
    this.activeTab = tab;
    
    // Clear active KPI card when switching tabs
    this.activeKpiCard = null;
    
    // Fetch quality issues when switching to that tab
    if (tab === 'quality-issues') {
      this.fetchAllQualityIssues();
    }
  }

  /**
   * Filter WIR checkpoints by status when clicking on KPI cards
   */
  filterCheckpointsByStatus(status: WIRCheckpointStatus | 'All'): void {
    if (status === 'All') {
      // Clear status filter
      this.filterForm.patchValue({ status: '' });
    } else {
      // Set status filter
      this.filterForm.patchValue({ status: status });
    }
    // Apply filters and fetch checkpoints
    this.applyFilters();
  }

  /**
   * Filter quality issues by status when clicking on KPI cards
   */
  filterQualityIssuesByStatus(status: QualityIssueStatus | 'All'): void {
    if (status === 'All') {
      // Clear status filter
      this.qualityIssuesFilterForm.patchValue({ status: '' });
    } else {
      // Set status filter
      this.qualityIssuesFilterForm.patchValue({ status: status });
    }
    // Fetch quality issues with the new filter
    this.qualityIssuesCurrentPage = 1;
    this.fetchAllQualityIssues();
  }

  /**
   * Handle KPI card click based on card label
   */
  onKpiCardClick(cardLabel: string): void {
    // Update active card for visual indication
    this.activeKpiCard = cardLabel;

    if (this.activeTab === 'checkpoints') {
      // Map card labels to checkpoint statuses
      switch (cardLabel) {
        case 'All WIR Checkpoints':
          this.filterCheckpointsByStatus('All');
          break;
        case 'Pending Reviews':
          this.filterCheckpointsByStatus(WIRCheckpointStatus.Pending);
          break;
        case 'Approved':
          this.filterCheckpointsByStatus(WIRCheckpointStatus.Approved);
          break;
        case 'Conditional Approval':
          this.filterCheckpointsByStatus(WIRCheckpointStatus.ConditionalApproval);
          break;
        case 'Rejected':
          this.filterCheckpointsByStatus(WIRCheckpointStatus.Rejected);
          break;
      }
    } else if (this.activeTab === 'quality-issues') {
      // Map card labels to quality issue statuses
      switch (cardLabel) {
        case 'Open Issues':
          this.filterQualityIssuesByStatus('Open');
          break;
        case 'In Progress':
          this.filterQualityIssuesByStatus('InProgress');
          break;
        case 'Resolved Issues':
          this.filterQualityIssuesByStatus('Resolved');
          break;
        case 'Closed Issues':
          this.filterQualityIssuesByStatus('Closed');
          break;
      }
    }
  }

  applyFilters(): void {
    this.checkpointsCurrentPage = 1;
    this.fetchCheckpoints();
  }

  resetFilters(): void {
    this.isResettingCheckpointFilters = true;
    this.filterForm.reset();
    this.checkpointsCurrentPage = 1;
    
    // Clear active KPI card when resetting filters
    this.activeKpiCard = null;
    
    this.fetchCheckpoints();
    // Reset flag after a short delay to allow form reset to complete
    setTimeout(() => {
      this.isResettingCheckpointFilters = false;
    }, 100);
  }
  
  onCheckpointsPageChange(page: number): void {
    if (page >= 1 && page <= this.checkpointsTotalPages) {
      this.checkpointsCurrentPage = page;
      this.fetchCheckpoints();
      // Scroll to top of table
      const tableElement = document.querySelector('.quality-table-wrapper');
      if (tableElement) {
        tableElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    }
  }
  
  onCheckpointsPageSizeChange(pageSize: number): void {
    this.checkpointsPageSize = pageSize;
    this.checkpointsCurrentPage = 1;
    this.fetchCheckpoints();
  }
  
  getCheckpointsPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 7;
    const current = this.checkpointsCurrentPage;
    const total = this.checkpointsTotalPages;
    
    if (total === 0) return [];
    
    let startPage = Math.max(1, current - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(total, startPage + maxPagesToShow - 1);
    
    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
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

  /**
   * Transform WIR number to Stage number for display purposes only
   * E.g., "WIR-1" -> "Stage-1", "WIR-2" -> "Stage-2"
   */
  getDisplayWirNumber(wirNumber: string | undefined | null): string {
    if (!wirNumber) return '—';
    return wirNumber.replace(/WIR-/gi, 'Stage-');
  }

  canNavigateToAddChecklist(checkpoint: EnrichedCheckpoint | null): boolean {
    // Check if checkpoint exists and has required data
    const hasRequiredData = !!(
      checkpoint &&
      checkpoint.wirId &&
      checkpoint.boxActivityId &&
      checkpoint.boxId &&
      (checkpoint.projectId || checkpoint.box?.projectId)
    );
    
    if (!hasRequiredData) {
      return false;
    }
    
    const projectId = checkpoint.projectId || checkpoint.box?.projectId;
    
    // Check if box is dispatched or on hold - no actions allowed
    if (this.isBoxDispatched(checkpoint.boxId) || this.isBoxOnHold(checkpoint.boxId)) {
      return false;
    }
    
    // Check if project is on hold, closed, or archived - no actions allowed
    if (projectId && this.isProjectRestricted(projectId)) {
      return false;
    }
    
    // Check if user has permission to create/manage WIR checkpoints
    return this.permissionService.hasPermission('wir', 'create') || 
           this.permissionService.hasPermission('wir', 'manage');
  }

  canNavigateToReview(checkpoint: EnrichedCheckpoint | null): boolean {
    // Check if checkpoint exists and has required data
    const hasRequiredData = !!(
      checkpoint &&
      checkpoint.boxActivityId &&
      checkpoint.boxId &&
      (checkpoint.projectId || checkpoint.box?.projectId)
    );
    
    if (!hasRequiredData) {
      return false;
    }
    
    const projectId = checkpoint.projectId || checkpoint.box?.projectId;
    
    // Check if box is dispatched or on hold - no actions allowed
    if (this.isBoxDispatched(checkpoint.boxId) || this.isBoxOnHold(checkpoint.boxId)) {
      return false;
    }
    
    // Check if project is on hold, closed, or archived - no actions allowed
    if (projectId && this.isProjectRestricted(projectId)) {
      return false;
    }
    
    // Check if user has permission to review WIR checkpoints
    return this.permissionService.hasPermission('wir', 'review') || 
           this.permissionService.hasPermission('wir', 'manage');
  }

  /**
   * Check if box is dispatched or on hold (cached)
   */
  isBoxDispatched(boxId: string | undefined): boolean {
    if (!boxId) {
      return false;
    }
    
    const cachedStatus = this.boxStatusCache.get(boxId);
    if (cachedStatus !== undefined) {
      return cachedStatus === BoxStatus.Dispatched;
    }
    
    // If not cached and not currently loading, fetch it
    if (!this.boxStatusLoading.has(boxId)) {
      this.fetchBoxStatus(boxId);
    }
    
    // Return false by default (optimistic - allow actions until we know box is dispatched)
    return false;
  }

  /**
   * Check if box is on hold (cached)
   */
  isBoxOnHold(boxId: string | undefined): boolean {
    if (!boxId) {
      return false;
    }
    
    const cachedStatus = this.boxStatusCache.get(boxId);
    if (cachedStatus !== undefined) {
      return cachedStatus === BoxStatus.OnHold;
    }
    
    // If not cached and not currently loading, fetch it
    if (!this.boxStatusLoading.has(boxId)) {
      this.fetchBoxStatus(boxId);
    }
    
    // Return false by default (optimistic - allow actions until we know box is on hold)
    return false;
  }

  /**
   * Check if project is on hold, closed, or archived (cached)
   */
  isProjectRestricted(projectId: string | undefined): boolean {
    if (!projectId) {
      return false;
    }
    
    const cachedStatus = this.projectStatusCache.get(projectId);
    if (cachedStatus !== undefined) {
      return cachedStatus === ProjectStatus.OnHold || 
             cachedStatus === ProjectStatus.Closed || 
             cachedStatus === ProjectStatus.Archived;
    }
    
    // If not cached and not currently loading, fetch it
    if (!this.projectStatusLoading.has(projectId)) {
      this.fetchProjectStatus(projectId);
    }
    
    // Return false by default (optimistic - allow actions until we know project is restricted)
    return false;
  }

  /**
   * Fetch box status and cache it
   */
  private fetchBoxStatus(boxId: string): void {
    if (this.boxStatusLoading.has(boxId)) {
      return; // Already loading
    }
    
    this.boxStatusLoading.add(boxId);
    this.boxService.getBox(boxId).subscribe({
      next: (box) => {
        const status = box.status as BoxStatus;
        this.boxStatusCache.set(boxId, status);
        this.boxStatusLoading.delete(boxId);
      },
      error: (err) => {
        console.error('Error fetching box status:', err);
        this.boxStatusCache.set(boxId, null); // Cache null to avoid repeated calls
        this.boxStatusLoading.delete(boxId);
      }
    });
  }

  /**
   * Fetch project status and cache it
   */
  private fetchProjectStatus(projectId: string): void {
    if (this.projectStatusLoading.has(projectId)) {
      return; // Already loading
    }
    
    this.projectStatusLoading.add(projectId);
    this.projectService.getProject(projectId).subscribe({
      next: (project) => {
        const status = project.status as ProjectStatus;
        this.projectStatusCache.set(projectId, status);
        this.projectStatusLoading.delete(projectId);
      },
      error: (err) => {
        console.error('Error fetching project status:', err);
        this.projectStatusCache.set(projectId, null); // Cache null to avoid repeated calls
        this.projectStatusLoading.delete(projectId);
      }
    });
  }

  /**
   * Check if quality issue actions are allowed (box not dispatched/on hold, project not restricted)
   */
  canPerformQualityIssueActions(issue: AggregatedQualityIssue): boolean {
    if (!issue.boxId) {
      return true; // Allow if no boxId (shouldn't happen, but be safe)
    }
    
    // Check if box is dispatched or on hold - no actions allowed
    if (this.isBoxDispatched(issue.boxId) || this.isBoxOnHold(issue.boxId)) {
      return false;
    }
    
    // Check if project is on hold, closed, or archived - no actions allowed
    if (issue.projectId && this.isProjectRestricted(issue.projectId)) {
      return false;
    }
    
    return true;
  }

  onAddChecklist(checkpoint: EnrichedCheckpoint): void {
    // Check if box is dispatched or on hold
    if (this.isBoxDispatched(checkpoint.boxId) || this.isBoxOnHold(checkpoint.boxId)) {
      const boxStatus = this.isBoxDispatched(checkpoint.boxId) ? 'dispatched' : 'on hold';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: {
          message: `Cannot perform actions. The box is ${boxStatus} and no actions are allowed on checkpoints.`,
          type: 'error'
        }
      }));
      return;
    }
    
    const projectId = checkpoint.projectId || checkpoint.box?.projectId;
    if (projectId && this.isProjectRestricted(projectId)) {
      const cachedStatus = this.projectStatusCache.get(projectId);
      const statusText = cachedStatus === ProjectStatus.OnHold ? 'on hold' : 
                         cachedStatus === ProjectStatus.Closed ? 'closed' : 'archived';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: {
          message: `Cannot perform actions. The project is ${statusText} and no actions are allowed on checkpoints.`,
          type: 'error'
        }
      }));
      return;
    }
    
    // Navigate to add-items step (Step 2) since checkpoint already exists
    this.navigateToCheckpoint(checkpoint, { step: 'add-items', from: 'quality-control' });
  }

  onReviewCheckpoint(checkpoint: EnrichedCheckpoint): void {
    // Check if box is dispatched or on hold
    if (this.isBoxDispatched(checkpoint.boxId) || this.isBoxOnHold(checkpoint.boxId)) {
      const boxStatus = this.isBoxDispatched(checkpoint.boxId) ? 'dispatched' : 'on hold';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: {
          message: `Cannot perform actions. The box is ${boxStatus} and no actions are allowed on checkpoints.`,
          type: 'error'
        }
      }));
      return;
    }
    
    const projectId = checkpoint.projectId || checkpoint.box?.projectId;
    if (projectId && this.isProjectRestricted(projectId)) {
      const cachedStatus = this.projectStatusCache.get(projectId);
      const statusText = cachedStatus === ProjectStatus.OnHold ? 'on hold' : 
                         cachedStatus === ProjectStatus.Closed ? 'closed' : 'archived';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: {
          message: `Cannot perform actions. The project is ${statusText} and no actions are allowed on checkpoints.`,
          type: 'error'
        }
      }));
      return;
    }
    
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

  fetchCheckpoints(): void {
    this.checkpointsLoading = true;
    this.qualityIssuesLoading = true;
    this.checkpointsError = '';
    this.qualityIssuesError = '';

    const filters = this.filterForm.value;
    const params: any = {
      projectCode: filters.projectCode || undefined,
      boxTag: filters.boxTag || undefined,
      status: filters.status || undefined,
      wirNumber: filters.wirNumber || undefined,
      from: filters.from || undefined,
      to: filters.to || undefined,
      page: this.checkpointsCurrentPage,
      pageSize: this.checkpointsPageSize
    };

    this.wirService.getWIRCheckpoints(params).subscribe({
      next: (response) => {
        // Handle paginated response
        if (response.items) {
          this.checkpoints = response.items as EnrichedCheckpoint[];
          this.checkpointsTotalCount = response.totalCount || 0;
          this.checkpointsCurrentPage = response.page || 1;
          this.checkpointsPageSize = response.pageSize || 25;
          this.checkpointsTotalPages = response.totalPages || 0;
        } else {
          // Fallback for non-paginated response (backward compatibility)
          const checkpoints = (response as any) || [];
          this.checkpoints = Array.isArray(checkpoints) ? checkpoints as EnrichedCheckpoint[] : [];
          this.checkpointsTotalCount = this.checkpoints.length;
          this.checkpointsTotalPages = 1;
        }
        
        // Pre-fetch box statuses and project statuses for all checkpoints
        this.checkpoints.forEach(checkpoint => {
          if (checkpoint.boxId && !this.boxStatusCache.has(checkpoint.boxId) && !this.boxStatusLoading.has(checkpoint.boxId)) {
            this.fetchBoxStatus(checkpoint.boxId);
          }
          const projectId = checkpoint.projectId || checkpoint.box?.projectId;
          if (projectId && !this.projectStatusCache.has(projectId) && !this.projectStatusLoading.has(projectId)) {
            this.fetchProjectStatus(projectId);
          }
        });
        
        // Update summary using backend summary data
        this.updateSummaryFromBackend(response);
        
       
        this.checkpointsLoading = false;
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
  
  /**
   * Update summary cards using backend summary data
   * Only updates on initial load or when filters are cleared to preserve total counts
   */
  private updateSummaryFromBackend(response: any): void {
    // Check if any filters are active
    const hasActiveFilters = this.filterForm.value.status || 
                             this.filterForm.value.wirNumber || 
                             this.filterForm.value.boxTag || 
                             this.filterForm.value.projectCode ||
                             this.filterForm.value.from ||
                             this.filterForm.value.to;
    
    // Only update summary if no filters are active (initial load or after reset)
    // This ensures counts remain unchanged when filtering
    if (hasActiveFilters) {
      console.log('⚠️ Filters active - preserving original summary counts');
      // Keep existing original counts, don't update
      return;
    }

    // Use backend summary if available, otherwise fallback to calculating from current page
    if (response.summary) {
      console.log('✅ Using backend summary (all checkpoints):', response.summary);
      const summary = response.summary;
      
      // Store original summary (total counts - never changes)
      this.originalWirCheckpointSummary = [
        { label: 'All WIR Checkpoints', value: summary.totalCheckpoints || 0, tone: 'info' },
        { label: 'Pending Reviews', value: summary.pendingReviews || 0, tone: 'warning' },
        { label: 'Approved', value: summary.approved || 0, tone: 'success' },
        { label: 'Conditional Approval', value: summary.conditionalApproval || 0, tone: 'warning' },
        { label: 'Rejected', value: summary.rejected || 0, tone: 'danger' }
      ];
      
      // Display cards always show original counts
      this.wirCheckpointSummaryCards = [...this.originalWirCheckpointSummary];
    } else {
      console.warn('⚠️ Backend summary not available, using fallback (current page only)');

      // Fallback: Calculate from current page (legacy behavior)
      const totalCheckpoints = response.totalCount || this.checkpoints.length;
      const pendingReviews = this.checkpoints.filter(cp => 
        cp.status === WIRCheckpointStatus.Pending
      ).length;
      const approved = this.checkpoints.filter(cp => 
        cp.status === WIRCheckpointStatus.Approved
      ).length;
      const conditionalApproval = this.checkpoints.filter(cp => 
        cp.status === WIRCheckpointStatus.ConditionalApproval
      ).length;
      const rejected = this.checkpoints.filter(cp => 
        cp.status === WIRCheckpointStatus.Rejected
      ).length;

      this.originalWirCheckpointSummary = [
        { label: 'All WIR Checkpoints', value: totalCheckpoints, tone: 'info' },
        { label: 'Pending Reviews', value: pendingReviews, tone: 'warning' },
        { label: 'Approved', value: approved, tone: 'success' },
        { label: 'Conditional Approval', value: conditionalApproval, tone: 'warning' },
        { label: 'Rejected', value: rejected, tone: 'danger' }
      ];
      
      this.wirCheckpointSummaryCards = [...this.originalWirCheckpointSummary];
    }
  }

  /**
   * Update quality issue summary cards using backend summary data
   * Only updates on initial load or when filters are cleared to preserve total counts
   */
  private updateQualityIssuesSummaryFromBackend(response: any): void {
    // Check if any filters are active
    const hasActiveFilters = this.qualityIssuesFilterForm.value.status || 
                             this.qualityIssuesFilterForm.value.issueNumber || 
                             this.qualityIssuesFilterForm.value.boxTag || 
                             this.qualityIssuesFilterForm.value.projectCode ||
                             this.qualityIssuesFilterForm.value.issueType ||
                             this.qualityIssuesFilterForm.value.severity ||
                             this.qualityIssuesFilterForm.value.assignedUser;
    
    // Only update summary if no filters are active (initial load or after reset)
    // This ensures counts remain unchanged when filtering
    if (hasActiveFilters) {
      console.log('⚠️ Filters active - preserving original summary counts');
      // Keep existing original counts, don't update
      return;
    }

    // Use backend summary if available, otherwise fallback to calculating from current page
    if (response.summary) {
      console.log('✅ Using backend summary for quality issues (all issues):', response.summary);
      const summary = response.summary;
      
      // Store original summary (total counts - never changes)
      this.originalQualityIssueSummary = [
        { label: 'Open Issues', value: summary.openIssues || 0, tone: 'danger' },
        { label: 'In Progress', value: summary.inProgressIssues || 0, tone: 'warning' },
        { label: 'Resolved Issues', value: summary.resolvedIssues || 0, tone: 'success' },
        { label: 'Closed Issues', value: summary.closedIssues || 0, tone: 'info' }
      ];
      
      // Display cards always show original counts
      this.qualityIssueSummaryCards = [...this.originalQualityIssueSummary];
    } else {
      console.warn('⚠️ Backend summary not available for quality issues, using fallback (current page only)');
      // Fallback: Calculate from current page (legacy behavior)
      const openIssues = this.qualityIssues.filter(issue => 
        issue.status === 'Open'
      ).length;
      const inProgressIssues = this.qualityIssues.filter(issue => 
        issue.status === 'InProgress'
      ).length;
      const resolvedIssues = this.qualityIssues.filter(issue => 
        issue.status === 'Resolved'
      ).length;
      const closedIssues = this.qualityIssues.filter(issue => 
        issue.status === 'Closed'
      ).length;

      this.originalQualityIssueSummary = [
        { label: 'Open Issues', value: openIssues, tone: 'danger' },
        { label: 'In Progress', value: inProgressIssues, tone: 'warning' },
        { label: 'Resolved Issues', value: resolvedIssues, tone: 'success' },
        { label: 'Closed Issues', value: closedIssues, tone: 'info' }
      ];
      
      this.qualityIssueSummaryCards = [...this.originalQualityIssueSummary];
    }
  }

  /**
   * Update summary cards using quality issues data (legacy/fallback method)
   */
  private updateQualityIssuesSummary(): void {
    // Count issues by status from current page (approximation)
    const openIssues = this.qualityIssues.filter(issue => 
      issue.status === 'Open'
    ).length;
    const inProgressIssues = this.qualityIssues.filter(issue => 
      issue.status === 'InProgress'
    ).length;
    const resolvedIssues = this.qualityIssues.filter(issue => 
      issue.status === 'Resolved'
    ).length;
    const closedIssues = this.qualityIssues.filter(issue => 
      issue.status === 'Closed'
    ).length;

    // Update Quality Issue Summary Cards
    this.qualityIssueSummaryCards = [
      { label: 'Open Issues', value: openIssues, tone: 'danger' },
      { label: 'In Progress', value: inProgressIssues, tone: 'warning' },
      { label: 'Resolved Issues', value: resolvedIssues, tone: 'success' },
      { label: 'Closed Issues', value: closedIssues, tone: 'info' }
    ];
  }

  /**
   * Fetch all quality issues with backend pagination and filters
   */
  fetchAllQualityIssues(): void {
    this.qualityIssuesLoading = true;
    this.qualityIssuesError = '';

    const filters = this.qualityIssuesFilterForm.value;
    
    // Map filter form values to API parameters
    const params: any = {
      page: this.qualityIssuesCurrentPage,
      pageSize: this.qualityIssuesPageSize
    };

    // Add filters if they have values
    if (filters.status && filters.status.trim()) {
      params.status = filters.status;
    }
    if (filters.severity && filters.severity.trim()) {
      params.severity = filters.severity;
    }
    if (filters.issueType && filters.issueType.trim()) {
      params.issueType = filters.issueType;
    }


    this.wirService.getAllQualityIssues(params).subscribe({
      next: (response) => {
        console.log('✅ Fetched paginated quality issues:', response.items.length, 'of', response.totalCount);
        console.log(response.items);
        this.buildQualityIssuesListFromPaginatedResponse(response);
        this.qualityIssuesLoading = false;
      },
      error: (err) => {
        console.error('❌ Failed to load quality issues:', err);
        this.qualityIssuesError = err?.error?.message || err?.message || 'Failed to load quality issues';
        this.qualityIssuesLoading = false;
      }
    });
  }
  private updateSummary(checkpoints: EnrichedCheckpoint[]): void {
    const total = checkpoints.length;
    // Pending reviews - only Pending status (not ConditionalApproval)
    const pendingReviews = checkpoints.filter(cp => 
      cp.status === WIRCheckpointStatus.Pending
    ).length;
    const approved = checkpoints.filter(cp => 
      cp.status === WIRCheckpointStatus.Approved
    ).length;
    const conditionalApproval = checkpoints.filter(cp => 
      cp.status === WIRCheckpointStatus.ConditionalApproval
    ).length;
    const rejected = checkpoints.filter(cp => 
      cp.status === WIRCheckpointStatus.Rejected
    ).length;

    // Update WIR Checkpoint Summary Cards
    this.wirCheckpointSummaryCards = [
      { label: 'All WIR Checkpoints', value: total, tone: 'info' },
      { label: 'Pending Reviews', value: pendingReviews, tone: 'warning' },
      { label: 'Approved', value: approved, tone: 'success' },
      { label: 'Conditional Approval', value: conditionalApproval, tone: 'warning' },
      { label: 'Rejected', value: rejected, tone: 'danger' }
    ];
  }

    /**
     * Build quality issues list from paginated response
     */
    private buildQualityIssuesListFromPaginatedResponse(response: any): void {
      const issues = response.items || [];
      
      console.log('📋 Raw quality issues from API:', issues);
      
      // Store the full unfiltered list
      this.allQualityIssues = issues.map((issue: any) => {
        const mapped = {
          issueId: issue.issueId || issue.IssueId || issue.qualityIssueId || issue.QualityIssueId,
          issueNumber: issue.issueNumber || undefined,
          issueType: issue.issueType,
          severity: issue.severity,
          issueDescription: issue.issueDescription,
          assignedTo: issue.assignedTo,
          assignedTeamName: issue.assignedTeamName || issue.assignedTeam || undefined,
          assignedToUserId: issue.assignedToUserId || undefined,
          assignedToUserName: issue.assignedToUserName || undefined,
          ccUserId: issue.ccUserId || undefined,
          ccUserName: issue.ccUserName || undefined,
          dueDate: issue.dueDate,
          photoPath: issue.photoPath,
          reportedBy: issue.reportedBy,
          issueDate: issue.issueDate,
          status: issue.status,
          boxId: issue.boxId,
          wirNumber: issue.wirNumber || undefined,
          wirName: issue.wirName || undefined,
          boxTag: issue.boxTag,
          boxName: issue.boxName,
          projectCode: issue.projectCode || undefined,
          projectName: issue.projectName || undefined,
          projectId: issue.projectId || undefined,
          checkpointStatus: issue.wirStatus as WIRCheckpointStatus | undefined,
          issueStatus: issue.status,
        };
        console.log('📋 Mapped issue projectId:', mapped.projectId);
        console.log('📋 Mapped issue projectCode:', mapped.projectCode);
        console.log('📋 Mapped issue projectName:', mapped.projectName);
        console.log('📋 Mapped issue issueNumber:', mapped.issueNumber);
        console.log('📋 Mapped issue ccUserName:', mapped.ccUserName);
        console.log('📋 Mapped issue assignedToUserName:', mapped.assignedToUserName);
        
        // Pre-fetch box status and project status for this issue
        if (mapped.boxId && !this.boxStatusCache.has(mapped.boxId) && !this.boxStatusLoading.has(mapped.boxId)) {
          this.fetchBoxStatus(mapped.boxId);
        }
        if (mapped.projectId && !this.projectStatusCache.has(mapped.projectId) && !this.projectStatusLoading.has(mapped.projectId)) {
          this.fetchProjectStatus(mapped.projectId);
        }
        
        return mapped;
      });
    
    // Update pagination info from response
    this.qualityIssuesTotalCount = response.totalCount || 0;
    this.qualityIssuesCurrentPage = response.page || 1;
    this.qualityIssuesPageSize = response.pageSize || 25;
    this.qualityIssuesTotalPages = response.totalPages || 0;

    // Update summary cards using backend summary data
    this.updateQualityIssuesSummaryFromBackend(response);
    
    // Extract unique projects and assigned users for filter dropdowns
    this.extractUniqueFilterOptions();
    
    // Apply client-side filters for WIR Number, Box Tag, and Project Code
    // (these are not supported by backend API)
    this.applyClientSideFilters();
  }
  
  /**
   * Extract unique projects and assigned users from quality issues for filter dropdowns
   */
  private extractUniqueFilterOptions(): void {
    console.log('📋 All quality issues for extraction:', this.allQualityIssues.length);
    
    // Extract unique projects from the unfiltered list
    const projectsMap = new Map<string, { id: string; code: string; name: string }>();
    this.allQualityIssues.forEach((issue, index) => {
      console.log(`📋 Issue ${index}:`, {
        projectId: issue.projectId,
        projectCode: issue.projectCode,
        projectName: issue.projectName
      });
      
      if (issue.projectCode) {
        // Use projectId if available, otherwise use projectCode as fallback
        const key = issue.projectId || issue.projectCode;
        projectsMap.set(key, {
          id: issue.projectId || issue.projectCode, // Use projectId if available, fallback to projectCode
          code: issue.projectCode,
          name: issue.projectName || issue.projectCode
        });
      }
    });
    
    this.uniqueProjects = Array.from(projectsMap.values()).sort((a, b) => 
      a.code.localeCompare(b.code)
    );
    console.log('📋 Extracted unique projects:', this.uniqueProjects);

    // Extract unique assigned users from the unfiltered list
    const usersMap = new Map<string, { id: string; name: string; teamName?: string }>();
    this.allQualityIssues.forEach(issue => {
      if (issue.assignedToUserId && issue.assignedToUserName) {
        usersMap.set(issue.assignedToUserId, {
          id: issue.assignedToUserId,
          name: issue.assignedToUserName,
          teamName: issue.assignedTeamName
        });
      }
    });
    this.uniqueAssignedUsers = Array.from(usersMap.values()).sort((a, b) => 
      a.name.localeCompare(b.name)
    );
    console.log('📋 Extracted unique assigned users:', this.uniqueAssignedUsers);
  }
  
  /**
   * Apply client-side filters (WIR Number, Box Tag, Project Code, Assigned User)
   * These filters are applied after receiving data from backend
   */
  private applyClientSideFilters(): void {
    const filters = this.qualityIssuesFilterForm.value;
    // Always start from the full unfiltered list
    let filtered = [...this.allQualityIssues];
    
    // Filter by Issue Number (client-side only)
    if (filters.issueNumber && filters.issueNumber.trim()) {
      filtered = filtered.filter(issue => {
        const issueNumberStr = issue.issueNumber ? issue.issueNumber.toString() : '';
        return issueNumberStr.toLowerCase().includes(filters.issueNumber.toLowerCase().trim());
      });
    }

    // Filter by Box Tag (client-side only)
    if (filters.boxTag && filters.boxTag.trim()) {
      filtered = filtered.filter(issue => {
        const boxTagMatch = (issue.boxTag || '').toLowerCase().includes(filters.boxTag.toLowerCase().trim());
        return boxTagMatch;
      });
    }

    // Filter by Project ID (client-side only) - exact match since using dropdown
    // Note: formControlName is still "projectCode" but it stores projectId (or projectCode as fallback)
    if (filters.projectCode && filters.projectCode.trim()) {
      const selectedValue = filters.projectCode.trim();
      filtered = filtered.filter(issue => {
        // Match by projectId if available, otherwise fall back to projectCode
        return (issue.projectId && issue.projectId === selectedValue) || 
               (issue.projectCode && issue.projectCode === selectedValue);
      });
    }

    // Filter by Assigned User (client-side only)
    if (filters.assignedUser && filters.assignedUser.trim()) {
      filtered = filtered.filter(issue => {
        return issue.assignedToUserId === filters.assignedUser.trim();
      });
    }
    
    // Update the displayed list (note: pagination counts are from backend)
    this.qualityIssues = filtered;
    console.log('📋 Filtered quality issues count:', filtered.length);
  }

  /**
   * Update summary cards to include all quality issues (not just from checkpoints)
   */
  private updateSummaryWithAllIssues(): void {
    // Count issues by status
    const openIssues = this.qualityIssues.filter(issue => 
      issue.status === 'Open'
    ).length;
    const inProgressIssues = this.qualityIssues.filter(issue => 
      issue.status === 'InProgress'
    ).length;
    const resolvedIssues = this.qualityIssues.filter(issue => 
      issue.status === 'Resolved'
    ).length;
    const closedIssues = this.qualityIssues.filter(issue => 
      issue.status === 'Closed'
    ).length;

    // Update Quality Issue Summary Cards
    this.qualityIssueSummaryCards = [
      { label: 'Open Issues', value: openIssues, tone: 'danger' },
      { label: 'In Progress', value: inProgressIssues, tone: 'warning' },
      { label: 'Resolved Issues', value: resolvedIssues, tone: 'success' },
      { label: 'Closed Issues', value: closedIssues, tone: 'info' }
    ];
  }

  onQualityIssuesPageChange(page: number): void {
    if (page >= 1 && page <= this.qualityIssuesTotalPages) {
      this.qualityIssuesCurrentPage = page;
      this.fetchAllQualityIssues();
      // Scroll to top of table
      const tableElement = document.querySelector('.quality-table-wrapper');
      if (tableElement) {
        tableElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    }
  }
  
  onQualityIssuesPageSizeChange(pageSize: number): void {
    this.qualityIssuesPageSize = pageSize;
    this.qualityIssuesCurrentPage = 1;
    this.fetchAllQualityIssues();
  }
  
  getQualityIssuesPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 7;
    const current = this.qualityIssuesCurrentPage;
    const total = this.qualityIssuesTotalPages;
    
    if (total === 0) return [];
    
    let startPage = Math.max(1, current - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(total, startPage + maxPagesToShow - 1);
    
    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  resetQualityIssuesFilters(): void {
    this.isResettingFilters = true;
    this.qualityIssuesFilterForm.reset();
    this.qualityIssuesCurrentPage = 1;
    
    // Clear active KPI card when resetting filters
    this.activeKpiCard = null;
    
    this.fetchAllQualityIssues();
    // Reset flag after a short delay to allow form reset to complete
    setTimeout(() => {
      this.isResettingFilters = false;
    }, 100);
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
    // If we have issueId, fetch full details using GetQualityIssueById to get images
    if (issue.issueId) {
      // Show modal immediately with existing data
      this.selectedIssueDetails = this.convertToQualityIssueDetails(issue, boxId);
      
      this.isDetailsModalOpen = true;

      // Fetch fresh details from backend (including images)
      this.wirService.getQualityIssueById(issue.issueId).subscribe({
        next: (fullIssue) => {
          console.log('✅ Loaded quality issue details with images:', fullIssue);
          this.selectedIssueDetails = fullIssue;
        },
        error: (err) => {
          console.error('❌ Failed to fetch quality issue details:', err);
          // Keep showing the initial data if fetch fails
        }
      });
    } else {
      // Use aggregated data directly (no issueId available)
      this.selectedIssueDetails = this.convertToQualityIssueDetails(issue, boxId);
     
      this.isDetailsModalOpen = true;
    }
  }

  closeIssueDetails(): void {
    this.isDetailsModalOpen = false;
    this.selectedIssueDetails = null;
  }


  openAssignModal(issue: AggregatedQualityIssue): void {
    // Check if box is dispatched/on hold or project is restricted
    if (!this.canPerformQualityIssueActions(issue)) {
      let message = 'Cannot perform actions. ';
      if (issue.boxId) {
        if (this.isBoxDispatched(issue.boxId)) {
          message += 'The box is dispatched and no actions are allowed on quality issues.';
        } else if (this.isBoxOnHold(issue.boxId)) {
          message += 'The box is on hold and no actions are allowed on quality issues.';
        } else if (issue.projectId && this.isProjectRestricted(issue.projectId)) {
          const cachedStatus = this.projectStatusCache.get(issue.projectId);
          const statusText = cachedStatus === ProjectStatus.OnHold ? 'on hold' : 
                             cachedStatus === ProjectStatus.Closed ? 'closed' : 'archived';
          message += `The project is ${statusText} and no actions are allowed on quality issues.`;
        }
      }
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: {
          message: message,
          type: 'error'
        }
      }));
      return;
    }
    
    // Close other modals first
    if (this.isDetailsModalOpen) {
      this.closeIssueDetails();
    }
    if (this.isStatusModalOpen) {
      this.closeStatusModal();
    }

    // Check for issueId in multiple possible property names
    const issueId = issue.issueId || (issue as any).IssueId || (issue as any).qualityIssueId || (issue as any).QualityIssueId;
    
    if (!issueId) {
      alert('Cannot assign: Issue ID is missing. This issue may not be saved yet.');
      return;
    }

    // Ensure issue has issueId for the assignment
    if (!issue.issueId) {
      (issue as any).issueId = issueId;
    }

    this.selectedIssueForAssign = issue;
    this.isAssignModalOpen = true;
  }

  closeAssignModal(): void {
    this.isAssignModalOpen = false;
    this.selectedIssueForAssign = null;
  }

  onAssignToCrew(event: { teamId: string | null; memberId: string | null; ccUserId: string | null }): void {
    if (!this.selectedIssueForAssign || !this.selectedIssueForAssign.issueId) {
      return;
    }

    this.assignLoading = true;

    this.wirService.assignQualityIssueToTeam(this.selectedIssueForAssign.issueId, event.teamId, event.memberId, event.ccUserId).subscribe({
      next: (updatedIssue) => {
        this.assignLoading = false;
        
        // Update the issue in the list
        const index = this.qualityIssues.findIndex(issue => issue.issueId === updatedIssue.issueId);
        if (index !== -1) {
          this.qualityIssues[index] = {
            ...this.qualityIssues[index],
            assignedTeamName: updatedIssue.assignedTeamName || undefined
          };
        }

        // If details modal is open for this issue, update it
        if (this.selectedIssueDetails && this.selectedIssueDetails.issueId === updatedIssue.issueId) {
          this.selectedIssueDetails = updatedIssue;
        }

        this.closeAssignModal();
        
        // Show success message
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: {
            message: event.teamId ? `Issue assigned to team successfully.` : `Issue unassigned successfully.`,
            type: 'success'
          }
        }));
      },
      error: (err) => {
        this.assignLoading = false;
        console.error('Error assigning issue to team:', err);
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: {
            message: err.error?.message || err.message || 'Failed to assign issue to team',
            type: 'error'
          }
        }));
      }
    });
  }

  openStatusModal(issue: AggregatedQualityIssue): void {
    // Check if box is dispatched/on hold or project is restricted
    if (!this.canPerformQualityIssueActions(issue)) {
      let message = 'Cannot perform actions. ';
      if (issue.boxId) {
        if (this.isBoxDispatched(issue.boxId)) {
          message += 'The box is dispatched and no actions are allowed on quality issues.';
        } else if (this.isBoxOnHold(issue.boxId)) {
          message += 'The box is on hold and no actions are allowed on quality issues.';
        } else if (issue.projectId && this.isProjectRestricted(issue.projectId)) {
          const cachedStatus = this.projectStatusCache.get(issue.projectId);
          const statusText = cachedStatus === ProjectStatus.OnHold ? 'on hold' : 
                             cachedStatus === ProjectStatus.Closed ? 'closed' : 'archived';
          message += `The project is ${statusText} and no actions are allowed on quality issues.`;
        }
      }
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: {
          message: message,
          type: 'error'
        }
      }));
      return;
    }
    
    // Close other modals first
    if (this.isDetailsModalOpen) {
      this.closeIssueDetails();
    }
    if (this.isAssignModalOpen) {
      this.closeAssignModal();
    }

    if (!issue.issueId) {
      console.error('Cannot update status: issueId is missing');
      return;
    }

    this.selectedIssueForStatus = issue;
    this.statusUpdateForm = {
      status: (issue.issueStatus || issue.status || 'Open') as QualityIssueStatus,
      resolutionDescription: ''
    };
    this.statusUpdateError = '';
    this.selectedImages = [];
    this.currentImageInputMode = 'url';
    this.currentUrlInput = '';
    this.showCamera = false;
    this.stopCamera();
    this.isStatusModalOpen = true;
  }

  closeStatusModal(): void {
    this.isStatusModalOpen = false;
    this.selectedIssueForStatus = null;
    this.statusUpdateLoading = false;
    this.statusUpdateError = '';
    this.selectedImages = [];
    this.currentImageInputMode = 'url';
    this.currentUrlInput = '';
    this.showCamera = false;
    this.showComments = true;
    this.stopCamera();
  }

  toggleComments(): void {
    this.showComments = !this.showComments;
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

    this.statusUpdateLoading = true;
    this.statusUpdateError = '';
    this.photoUploadError = '';

    // Prepare files and URLs
    const files = this.selectedImages
      .filter(img => img.type === 'file' || img.type === 'camera')
      .map(img => img.file!)
      .filter((file): file is File => file !== undefined);

    const imageUrls = this.selectedImages
      .filter(img => img.type === 'url' && img.url)
      .map(img => img.url!)
      .filter((url): url is string => url !== undefined && url.trim() !== '');

    const payload: UpdateQualityIssueStatusRequest = {
      issueId: this.selectedIssueForStatus.issueId,
      status: this.statusUpdateForm.status,
      resolutionDescription: this.requiresResolutionDescription(this.statusUpdateForm.status)
        ? this.statusUpdateForm.resolutionDescription?.trim()
        : null
    };

    this.wirService.updateQualityIssueStatus(
      payload.issueId, 
      payload,
      files.length > 0 ? files : undefined,
      imageUrls.length > 0 ? imageUrls : undefined
    ).subscribe({
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

  // Multiple images methods
  setImageInputMode(mode: 'url' | 'upload' | 'camera'): void {
    this.currentImageInputMode = mode;
    if (mode !== 'camera') {
      this.showCamera = false;
      this.stopCamera();
    }
  }

  openFileInput(): void {
    this.showCamera = false;
    const fileInput = document.getElementById('issue-photo-file-input') as HTMLInputElement;
    if (fileInput) {
      fileInput.click();
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      Array.from(input.files).forEach(file => {
        if (file.type.startsWith('image/')) {
          this.addImageFromFile(file);
        } else {
          this.photoUploadError = 'Please select image files only';
        }
      });
      // Reset input to allow selecting the same file again
      input.value = '';
    }
  }

  addImageFromFile(file: File): void {
    const reader = new FileReader();
    reader.onload = (e) => {
      const preview = e.target?.result as string;
      this.selectedImages.push({
        id: `file-${Date.now()}-${Math.random()}`,
        type: 'file',
        file: file,
        preview: preview,
        name: file.name,
        size: file.size
      });
      this.photoUploadError = '';
    };
    reader.readAsDataURL(file);
  }

  addImageFromDataUrl(imageData: string): void {
    // Convert data URL to File for consistency with existing structure
    fetch(imageData)
      .then(res => res.blob())
      .then(blob => {
        const file = new File([blob], `photo-${Date.now()}.jpg`, { type: 'image/jpeg' });
        this.selectedImages.push({
          id: `camera-${Date.now()}-${Math.random()}`,
          type: 'camera',
          file: file,
          preview: imageData,
          name: file.name,
          size: file.size
        });
        this.photoUploadError = '';
      })
      .catch(err => {
        console.error('Error converting data URL to file:', err);
        this.photoUploadError = 'Failed to process captured image.';
      });
  }

  addImageFromUrl(): void {
    const url = this.currentUrlInput?.trim();
    if (url && url.trim()) {
      // Validate URL format
      try {
        new URL(url);
        this.selectedImages.push({
          id: `url-${Date.now()}-${Math.random()}`,
          type: 'url',
          url: url.trim(),
          preview: url.trim() // Use URL as preview
        });
        this.currentUrlInput = '';
        this.currentImageInputMode = 'url';
        this.photoUploadError = '';
      } catch {
        this.photoUploadError = 'Please enter a valid URL';
      }
    }
  }

  removeImage(imageId: string): void {
    this.selectedImages = this.selectedImages.filter(img => img.id !== imageId);
  }

  clearAllImages(): void {
    this.selectedImages = [];
    this.currentUrlInput = '';
    this.photoUploadError = '';
  }

  trackByImageId(index: number, image: { id: string }): string {
    return image.id;
  }

  // Camera methods
  async openCamera(): Promise<void> {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ 
        video: { 
          facingMode: 'environment' // Use back camera on mobile
        } 
      });
      this.cameraStream = stream;
      this.showCamera = true;
      this.currentImageInputMode = 'camera';
      
      // Wait for video element to be rendered
      setTimeout(() => {
        const video = document.getElementById('issue-camera-preview') as HTMLVideoElement;
        const cameraContainer = document.getElementById('camera-preview-container') as HTMLElement;
        
        if (!video) {
          console.error('Video element not found');
          return;
        }
        
        // Set video source and play
        video.srcObject = stream;
        video.muted = true;
        video.playsInline = true;
        video.autoplay = true;
        video.play();
        
        // Wait for video to start playing before going fullscreen
        const handlePlaying = () => {
          console.log('Video is playing, requesting fullscreen');
          
          // Request fullscreen for camera container
          if (cameraContainer) {
            const requestFullscreen = () => {
              if (cameraContainer.requestFullscreen) {
                return cameraContainer.requestFullscreen();
              } else if ((cameraContainer as any).webkitRequestFullscreen) {
                return (cameraContainer as any).webkitRequestFullscreen();
              } else if ((cameraContainer as any).mozRequestFullScreen) {
                return (cameraContainer as any).mozRequestFullScreen();
              } else if ((cameraContainer as any).msRequestFullscreen) {
                return (cameraContainer as any).msRequestFullscreen();
              }
              return Promise.reject('Fullscreen not supported');
            };
            
            requestFullscreen().catch((err: unknown) => {
              console.warn('Error attempting to enable fullscreen:', err);
            });
          }
        };
        
        // Ensure video plays
        video.play().then(() => {
          console.log('Video play() resolved');
          // Wait a bit more to ensure video is actually rendering
          setTimeout(() => {
            if (video.readyState >= 2 && video.videoWidth > 0) {
              handlePlaying();
            } else {
              // Fallback: wait for playing event
              video.addEventListener('playing', handlePlaying, { once: true });
            }
          }, 300);
        }).catch(err => {
          console.error('Error playing video:', err);
          this.photoUploadError = 'Error starting camera preview.';
        });
        
        // Also listen for playing event as backup
        video.addEventListener('playing', () => {
          console.log('Video playing event fired');
        }, { once: true });
        
      }, 100);
    } catch (err) {
      console.error('Error accessing camera:', err);
      this.photoUploadError = 'Unable to access camera. Please check permissions.';
      this.showCamera = false;
    }
  }

  stopCamera(): void {
    // Exit fullscreen if active
    this.exitFullscreen();
    
    // Stop video stream
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach(track => track.stop());
      this.cameraStream = null;
    }
    
    // Clear video element
    const video = document.getElementById('issue-camera-preview') as HTMLVideoElement;
    if (video) {
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach(track => track.stop());
      }
      video.srcObject = null;
      video.pause();
    }
    
    this.showCamera = false;
  }

  private exitFullscreen(): void {
    if (document.fullscreenElement || (document as any).webkitFullscreenElement || 
        (document as any).mozFullScreenElement || (document as any).msFullscreenElement) {
      if (document.exitFullscreen) {
        document.exitFullscreen().catch(err => console.warn('Error exiting fullscreen:', err));
      } else if ((document as any).webkitExitFullscreen) {
        (document as any).webkitExitFullscreen();
      } else if ((document as any).mozCancelFullScreen) {
        (document as any).mozCancelFullScreen();
      } else if ((document as any).msExitFullscreen) {
        (document as any).msExitFullscreen();
      }
    }
  }

  capturePhoto(): void {
    const video = document.getElementById('issue-camera-preview') as HTMLVideoElement;
    
    if (!video) {
      this.photoUploadError = 'Camera element not found.';
      return;
    }
    
    // Check if video has valid dimensions
    if (!video.videoWidth || !video.videoHeight || video.videoWidth === 0 || video.videoHeight === 0) {
      console.warn('Video dimensions not ready:', { 
        width: video.videoWidth, 
        height: video.videoHeight,
        readyState: video.readyState 
      });
      this.photoUploadError = 'Camera not ready. Please wait a moment and try again.';
      return;
    }

    // Check if video is actually playing
    if (video.readyState < 2) {
      console.warn('Video not ready:', { readyState: video.readyState });
      this.photoUploadError = 'Camera stream not ready. Please wait a moment.';
      return;
    }
    
    // Check if video is paused
    if (video.paused) {
      console.warn('Video is paused, attempting to play');
      video.play().catch(err => {
        console.error('Error playing video for capture:', err);
        this.photoUploadError = 'Camera is paused. Please try again.';
        return;
      });
    }

    try {
      // Create canvas and draw video frame
      const canvas = document.createElement('canvas');
      canvas.width = video.videoWidth;
      canvas.height = video.videoHeight;
      
      const ctx = canvas.getContext('2d');
      
      if (!ctx) {
        this.photoUploadError = 'Unable to create canvas context.';
        return;
      }
      
      // Draw the current video frame to canvas
      ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
      
      // Convert to Base64 data URL
      const imageData = canvas.toDataURL('image/jpeg', 0.9);
      
      // Stop camera stream immediately before any async operations
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach((track) => track.stop());
      }
      
      // Clear video element srcObject
      video.srcObject = null;
      
      // Clear camera stream reference
      if (this.cameraStream) {
        this.cameraStream.getTracks().forEach((track) => track.stop());
        this.cameraStream = null;
      }
      
      // Exit fullscreen
      this.exitFullscreen();
      
      // Close camera UI immediately
      this.showCamera = false;
      
      // Add the captured image to the list
      this.addImageFromDataUrl(imageData);
      
    } catch (err) {
      console.error('Error capturing photo:', err);
      this.photoUploadError = 'Error capturing image. Please try again.';
      // Ensure camera is stopped even on error
      this.stopCamera();
    }
  }

  ngOnDestroy(): void {
    // Cleanup: stop camera stream on component unmount
    const video = document.getElementById('issue-camera-preview') as HTMLVideoElement;
    if (video) {
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach((track) => track.stop());
      }
      video.srcObject = null;
    }
    
    // Also stop any stored stream reference
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach((track) => track.stop());
      this.cameraStream = null;
    }
    
    this.stopCamera();
  }

  private applyUpdatedQualityIssue(updated: QualityIssueDetails): void {
    // Update qualityIssues array
    this.qualityIssues = this.qualityIssues.map(issue =>
      issue.issueId === updated.issueId ? { ...issue, ...updated, issueStatus: updated.status } : issue
    );
    
    // Update the issue in checkpoints array so summary counts update correctly
    this.checkpoints = this.checkpoints.map(checkpoint => {
      if (checkpoint.qualityIssues && checkpoint.qualityIssues.some(issue => (issue as any).issueId === updated.issueId)) {
        return {
          ...checkpoint,
          qualityIssues: checkpoint.qualityIssues.map(issue => 
            (issue as any).issueId === updated.issueId 
              ? { ...issue, status: updated.status, ...updated } 
              : issue
          )
        };
      }
      return checkpoint;
    });
    
    // Refresh the list to reflect changes
    this.fetchAllQualityIssues();
    // Note: Summary will be updated when checkpoints are refreshed
  }

  private convertToQualityIssueDetails(issue: AggregatedQualityIssue, boxId?: string): QualityIssueDetails {
    return {
      issueId: issue.issueId || '',
      issueNumber: issue.issueNumber,
      issueType: issue.issueType,
      severity: issue.severity,
      issueDescription: issue.issueDescription,
      assignedTo: issue.assignedTo,
      assignedTeamName: issue.assignedTeamName || issue.assignedTeam || undefined,
      assignedToUserId: issue.assignedToUserId,
      assignedToUserName: issue.assignedToUserName,
      ccUserId: issue.ccUserId,
      ccUserName: issue.ccUserName,
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

  async exportQualityIssuesToExcel(): Promise<void> {
    if (this.qualityIssues.length === 0) {
      alert('No quality issues to export. Please adjust your filters or ensure there are quality issues available.');
      return;
    }

    // Format dates properly
    const formatDateForExcel = (date?: string | Date): string => {
      if (!date) return '—';
      const d = date instanceof Date ? date : new Date(date);
      if (isNaN(d.getTime())) return '—';
      return d.toLocaleDateString('en-US', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
      });
    };

    // Create a new workbook and worksheet
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Quality Issues');

    // Define column headers
    const headers = [
      'No.',
      'Issue Number',
      'Stage Number',
      'Box Tag',
      'Box Name',
      'Project Code',
      'Status',
      'Issue Type',
      'Severity',
      'Project Name',
      'Assigned Team',
      'Assigned User',
      'CC User',
      'Reported By',
      'Issue Date',
      'Due Date',
      'Resolution Description',
      'Resolution Date',
      'Photo Path'
    ];

    // Set column widths
    worksheet.columns = [
      { width: 5 },   // No.
      { width: 15 },  // Issue Number
      { width: 15 },  // WIR Number
      { width: 15 },  // Box Tag
      { width: 20 },  // Box Name
      { width: 15 },  // Project Code
      { width: 12 },  // Status
      { width: 15 },  // Issue Type
      { width: 10 },  // Severity
      { width: 30 },  // Project Name
      { width: 20 },  // Assigned Team
      { width: 20 },  // Assigned User
      { width: 15 },  // CC User
      { width: 15 },  // Reported By
      { width: 12 },  // Issue Date
      { width: 12 },  // Due Date
      { width: 40 },  // Resolution Description
      { width: 12 },  // Resolution Date
      { width: 30 }   // Photo Path
    ];

    // Add header row with styling
    const headerRow = worksheet.addRow(headers);
    headerRow.eachCell((cell) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FF4472C4' } // Blue background
      };
      cell.font = {
        bold: true,
        color: { argb: 'FFFFFFFF' }, // White text
        size: 11
      };
      cell.alignment = {
        horizontal: 'center',
        vertical: 'middle',
        wrapText: true
      };
      cell.border = {
        top: { style: 'thin', color: { argb: 'FF000000' } },
        bottom: { style: 'thin', color: { argb: 'FF000000' } },
        left: { style: 'thin', color: { argb: 'FF000000' } },
        right: { style: 'thin', color: { argb: 'FF000000' } }
      };
    });
    headerRow.height = 25; // Set header row height

    // Add data rows
    this.qualityIssues.forEach((issue, index) => {
      const row = worksheet.addRow([
        index + 1,
        issue.issueNumber ? `#${issue.issueNumber}` : '—',
        issue.wirNumber || issue.wirName || '—',
        issue.boxTag || '—',
        issue.boxName || '—',
        issue.projectCode || '—',
        this.getQualityIssueStatusLabel(issue.issueStatus || issue.status),
        issue.issueType || '—',
        issue.severity || '—',
        issue.projectName || '—',
        issue.assignedTeamName || issue.assignedTo || '—',
        issue.assignedToUserName || '—',
        issue.ccUserName || '—',
        issue.reportedBy || '—',
        formatDateForExcel(issue.issueDate),
        formatDateForExcel(issue.dueDate),
        (issue as any).resolutionDescription || '—',
        formatDateForExcel((issue as any).resolutionDate),
        issue.photoPath || '—'
      ]);

      // Style data rows (optional - light gray alternating rows)
      row.eachCell((cell, colNumber) => {
        cell.border = {
          top: { style: 'thin', color: { argb: 'FFE0E0E0' } },
          bottom: { style: 'thin', color: { argb: 'FFE0E0E0' } },
          left: { style: 'thin', color: { argb: 'FFE0E0E0' } },
          right: { style: 'thin', color: { argb: 'FFE0E0E0' } }
        };
        cell.alignment = {
          vertical: 'middle',
          wrapText: true
        };
        // Alternate row colors for better readability
        if (index % 2 === 0) {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFF9F9F9' } // Very light gray
          };
        }
      });
    });

    // Freeze header row
    worksheet.views = [
      {
        state: 'frozen',
        ySplit: 1 // Freeze first row
      }
    ];

    // Generate filename with current date
    const currentDate = new Date().toISOString().split('T')[0];
    const filename = `Quality_Issues_${currentDate}.xlsx`;

    // Export to Excel
    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    link.click();
    window.URL.revokeObjectURL(url);
  }
  
  // Expose Math for template
  readonly Math = Math;
}

