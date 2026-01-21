import { Component, OnDestroy, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { UserService, UserDto } from '../../../core/services/user.service';
import { TeamService } from '../../../core/services/team.service';
import { ProjectService } from '../../../core/services/project.service';
import { Team, TeamMember } from '../../../core/models/team.model';
import { Box, BoxStatus, BoxLog, BoxPanel, PanelStatus, getBoxStatusNumber, getAvailableBoxStatuses, canPerformBoxActions, canPerformActivityActions } from '../../../core/models/box.model';
import { WIRService } from '../../../core/services/wir.service';
import { ProgressUpdate, ProgressUpdatesSearchParams } from '../../../core/models/progress-update.model';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { ProgressUpdatesTableComponent } from '../../../shared/components/progress-updates-table/progress-updates-table.component';
import { QualityIssueDetails, QualityIssueStatus, UpdateQualityIssueStatusRequest, WIRCheckpoint, WIRCheckpointStatus, WIRRecord, CreateQualityIssueForBoxRequest, IssueType, SeverityType } from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ActivityTableComponent } from '../../activities/activity-table/activity-table.component';
import { BoxLogDetailsModalComponent } from '../box-log-details-modal/box-log-details-modal.component';
import { UploadDrawingModalComponent } from '../upload-drawing-modal/upload-drawing-modal.component';
import { BoxPanelsComponent } from '../box-panels/box-panels.component';
import { QualityIssueDetailsModalComponent } from '../../../shared/components/quality-issue-details-modal/quality-issue-details-modal.component';
import { AssignToCrewModalComponent, AssignableIssue } from '../../../shared/components/assign-to-crew-modal/assign-to-crew-modal.component';
import { IssueCommentsComponent } from '../../../shared/components/issue-comments/issue-comments.component';
import { LocationService, FactoryLocation, BoxLocationHistory } from '../../../core/services/location.service';
import { ApiService } from '../../../core/services/api.service';
import { WirExportService, ProjectInfo } from '../../../core/services/wir-export.service';
import * as ExcelJS from 'exceljs';
import { Observable, Subject, takeUntil, forkJoin, of, firstValueFrom } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, skip, catchError } from 'rxjs/operators';
import { DiffUtil } from '../../../core/utils/diff.util';
import { environment } from '../../../../environments/environment';

type BoxDrawing = {
  imageUrl: string; 
  displayUrl: string; 
  updateDate?: Date; 
  activityName?: string; 
  progressPercentage?: number; 
  imageType: 'file' | 'url'; 
  originalUrl?: string;
  fileName?: string;
  originalFileName?: string; // Original filename
  fileExtension?: string;
  fileSize?: number;
  fileData?: string;
  boxDrawingId?: string;
  createdBy?: string;
  createdByName?: string;
  version?: number; // Version number for files with same name
  createdDate?: Date; // Creation date
  drawingUrl?: string; // URL for drawing
};

@Component({
  selector: 'app-box-details',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, SidebarComponent, ActivityTableComponent, ProgressUpdatesTableComponent, HeaderComponent, BoxLogDetailsModalComponent, UploadDrawingModalComponent, QualityIssueDetailsModalComponent, AssignToCrewModalComponent, IssueCommentsComponent, BoxPanelsComponent],
  providers: [LocationService],
  animations: [
    trigger('slideDown', [
      transition(':enter', [
        style({ height: '0', opacity: '0', overflow: 'hidden' }),
        animate('50ms ease-out', style({ height: '*', opacity: '1' }))
      ]),
      transition(':leave', [
        style({ height: '*', opacity: '1', overflow: 'hidden' }),
        animate('50ms ease-in', style({ height: '0', opacity: '0' }))
      ])
    ]),
    trigger('slideIn', [
      transition(':enter', [
        style({ transform: 'translateX(100%)', opacity: '0' }),
        animate('100ms ease-out', style({ transform: 'translateX(0)', opacity: '1' }))
      ]),
      transition(':leave', [
        style({ transform: 'translateX(0)', opacity: '1' }),
        animate('80ms ease-in', style({ transform: 'translateX(100%)', opacity: '0' }))
      ])
    ])
  ],
  templateUrl: './box-details.component.html',
  styleUrls: ['./box-details.component.scss', './box-details-attachments.scss']
})
export class BoxDetailsComponent implements OnInit, OnDestroy {
  box: Box | null = null;
  boxId!: string;
  projectId!: string;
  project: any = null; // Store project details to check if archived
  loading = true;
  error = '';
  deleting = false;
  showDeleteConfirm = false;
  deleteSuccess = false;
  
  activeTab: 'overview' | 'activities' | 'wir' | 'quality-issues' | 'logs' | 'drawings' | 'progress-updates' | 'attachments' | 'panels' = 'overview';
  
  canEdit = false;
  canDelete = false;
  canUpdateBoxStatus = false;
  canUpdateQualityIssueStatus = false;
  canUploadDrawing = false;
  isProjectArchived = false; // Track if project is archived
  isProjectOnHold = false; // Track if project is on hold
  isProjectClosed = false; // Track if project is closed
  BoxStatus = BoxStatus;
  Math = Math;
  
  actualActivityCount: number = 0; // Actual count from activity table (excluding WIR rows)
  wirCheckpoints: WIRCheckpoint[] = [];
  wirLoading = false;
  wirError = '';
  expandedCheckpointVersions: Set<string> = new Set(); // Track which checkpoints have versions expanded
  allCheckpointVersionsMap: Map<string, WIRCheckpoint[]> = new Map(); // Store all versions for each checkpoint
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
  } = {
    status: 'Open',
    resolutionDescription: ''
  };
  isDetailsModalOpen = false;

  // Assign modal state
  isAssignModalOpen = false;
  selectedIssueForAssign: QualityIssueDetails | null = null;
  assignLoading = false;
  selectedIssueDetails: QualityIssueDetails | null = null;
  selectedCommentId?: string; // For scrolling to specific comment from notifications
  
  // Comments modal state
  isCommentsModalOpen = false;
  selectedIssueForComments: QualityIssueDetails | null = null;
  
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

  // Create Quality Issue
  isCreateQualityIssueModalOpen = false;
  createQualityIssueLoading = false;
  createQualityIssueError = '';
  issueTypes: IssueType[] = ['Defect', 'NonConformance', 'Observation'];
  severityLevels: SeverityType[] = ['Critical', 'Major', 'Minor'];
  newQualityIssueForm: {
    issueType: IssueType;
    severity: SeverityType;
    issueDescription: string;
    assignedTo: string; // Team ID
    assignedToUserId: string; // User ID within team
    ccUserId: string; // CC User ID
    dueDate: string;
  } = {
    issueType: 'Defect',
    severity: 'Major',
    issueDescription: '',
    assignedTo: '',
    assignedToUserId: '',
    ccUserId: '',
    dueDate: ''
  };
  availableTeams: Team[] = [];
  loadingTeams = false;
  availableTeamUsers: {userId: string, userName: string, userEmail: string}[] = [];
  availableTeamMembers: TeamMember[] = [];
  loadingTeamUsers = false;
  availableCCUsers: {userId: string, userName: string, userEmail: string}[] = [];
  loadingCCUsers = false;
  qualityIssueImages: Array<{
    id: string;
    type: 'file' | 'url' | 'camera';
    file?: File;
    url?: string;
    preview: string;
    name?: string;
    size?: number;
  }> = [];
  canCreateQualityIssue = false;

  progressUpdates: ProgressUpdate[] = [];
  progressUpdatesLoading = false;
  progressUpdatesError = '';
  selectedProgressUpdate: ProgressUpdate | null = null;
  isProgressModalOpen = false;
  
  // All progress updates for drawings (images)
  allProgressUpdatesForImages: ProgressUpdate[] = [];
  loadingProgressUpdateImages = false;
  resolvedProgressImages: Array<{ imageUrl: string; displayUrl: string; updateDate?: Date; activityName?: string; progressPercentage?: number; imageType?: 'file' | 'url' }> = [];
  resolvingProgressImages = false;
  
  // Box drawings from dedicated endpoint
  boxDrawings: BoxDrawing[] = [];
  loadingBoxDrawings = false;
  boxDrawingsError = '';
  
  // User name cache for drawings
  private userNamesCache = new Map<string, string>();

  // All box attachments (WIR, Progress Update, Quality Issue images)
  boxAttachments: any = null;
  loadingBoxAttachments = false;
  boxAttachmentsError = '';
  
  // Collapsible sections state
  wirImagesExpanded = false;
  progressImagesExpanded = false;
  qualityImagesExpanded = false;
  
  // Track expanded file groups (by fileName)
  expandedFileGroups = new Set<string>();
  expandedUrlGroups = new Set<string>();
  expandedWirImageGroups = new Set<string>();
  expandedProgressImageGroups = new Set<string>();
  expandedQualityImageGroups = new Set<string>();
  
  // Version History Sidebar
  versionHistorySidebarOpen = false;
  currentFileVersions: BoxDrawing[] = [];
  currentFileName = '';
  selectedVersionsForCompare: BoxDrawing[] = [];
  compareMode = false;
  
  // Sub-tab for Drawings section
  activeDrawingTab: 'file' | 'url' = 'file';
  
  // Upload drawing modal
  isUploadDrawingModalOpen = false;
  
  // Pagination for progress updates
  progressUpdatesCurrentPage = 1;
  progressUpdatesPageSize = 10;
  progressUpdatesTotalCount = 0;
  progressUpdatesTotalPages = 0;
  
  // Search filters for progress updates
  progressUpdatesSearchTerm = '';
  progressUpdatesActivityName = '';
  progressUpdatesStatus = '';
  progressUpdatesFromDate = '';
  progressUpdatesToDate = '';
  showProgressUpdatesSearch = false;

  // Box Status Update
  isBoxStatusModalOpen = false;
  boxStatusUpdateLoading = false;
  boxStatusUpdateError = '';
  selectedBoxStatus: BoxStatus | null = null;
  availableBoxStatuses: BoxStatus[] = [];
  
  // Box Logs
  boxLogs: BoxLog[] = [];
  boxLogsLoading = false;
  boxLogsError = '';
  boxLogsCurrentPage = 1;
  boxLogsPageSize = 25;
  boxLogsTotalCount = 0;
  boxLogsTotalPages = 0;
  boxLogsSearchTerm = '';
  boxLogsAction = '';
  boxLogsFromDate = '';
  boxLogsToDate = '';
  boxLogsChangedBy = '';
  showBoxLogsSearch = false;
  boxLogsSearchControl = new FormControl('');
  availableBoxLogActions: string[] = [];
  availableBoxLogUsers: UserDto[] = [];
  loadingBoxLogUsers = false;
  private boxLogActionSet = new Set<string>();
  
  // Box Log Details Modal
  selectedBoxLog: BoxLog | null = null;
  isBoxLogDetailsModalOpen = false;
  readonly DiffUtil = DiffUtil;
  
  // Concrete Panel Delivery and Pod Delivery
  boxPanels: BoxPanel[] = [];
  podDeliverChecked = false;
  readonly PanelStatus = PanelStatus;
  podName: string = '';
  podType: string = '';
  updatingDeliveryInfo = false;
  showLegend = false;
  
  // Track original pod delivery values to detect changes
  private originalPodDeliver = false;
  private originalPodName = '';
  private originalPodType = '';
  hasPodDeliveryChanges = false;
  
  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxService: BoxService,
    private permissionService: PermissionService,
    private userService: UserService,
    private wirService: WIRService,
    private progressUpdateService: ProgressUpdateService,
    @Inject(LocationService) private locationService: LocationService,
    private apiService: ApiService,
    private http: HttpClient,
    private wirExportService: WirExportService,
    private teamService: TeamService,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.boxId = this.route.snapshot.params['boxId'];
    this.projectId = this.route.snapshot.params['projectId'];
    
    // Check for tab query parameter to set active tab
    const tabParam = this.route.snapshot.queryParams['tab'];
    if (tabParam && ['overview', 'activities', 'wir', 'quality-issues', 'logs', 'drawings', 'progress-updates', 'attachments'].includes(tabParam)) {
      this.activeTab = tabParam as any;
    }
    
    // Check for issueId query parameter (from notifications)
    const issueId = this.route.snapshot.queryParams['issueId'];
    const commentId = this.route.snapshot.queryParams['commentId'];
    
    if (issueId) {
      // Set active tab to quality-issues
      this.activeTab = 'quality-issues';
      
      // Load quality issues and then open the specific issue modal
      setTimeout(() => {
        this.loadQualityIssuesAndOpenIssue(issueId, commentId);
      }, 500);
    }
    
    // Check permissions immediately
    this.checkPermissions();
    
    // Subscribe to permission changes to update UI when permissions are loaded
    this.permissionService.permissions$
      .pipe(
        skip(1), // Skip initial empty value
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        console.log('🔄 Permissions updated, re-checking box permissions');
        this.checkPermissions();
      });
    
    this.setupBoxLogsSearch();
    this.loadBox();
    this.loadProjectDetails();
  }
  
  private loadProjectDetails(): void {
    if (!this.projectId) return;
    
    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.project = project;
        this.isProjectArchived = project.status === 'Archived';
        this.isProjectOnHold = project.status === 'OnHold';
        this.isProjectClosed = project.status === 'Closed';
        // Re-check permissions after loading project status
        this.checkPermissions();
        console.log('📁 Project loaded. Status:', project.status, 'Is Archived:', this.isProjectArchived, 'Is OnHold:', this.isProjectOnHold, 'Is Closed:', this.isProjectClosed);
      },
      error: (err) => {
        console.error('Error loading project details:', err);
      }
    });
  }
  
  private checkPermissions(): void {
    const baseCanEdit = this.permissionService.canEdit('boxes');
    const baseCanDelete = this.permissionService.canDelete('boxes');
    const baseCanUpdateStatus = this.permissionService.canEdit('boxes') || 
                              this.permissionService.hasPermission('boxes', 'update-status');
    const baseCanUpdateQualityIssueStatus = this.permissionService.hasPermission('quality-issues', 'edit') || 
                                       this.permissionService.hasPermission('quality-issues', 'resolve');
    const baseCanCreateQualityIssue = this.permissionService.canCreate('quality-issues');
    const baseCanUploadDrawing = this.permissionService.canEdit('boxes');
    
    // Check if box actions are allowed based on box status
    const canPerformBoxActionsBasedOnStatus = this.box ? canPerformBoxActions(this.box.status) : true;
    // For status updates, check if there are available status transitions
    const canChangeStatusBasedOnBoxStatus = this.box ? getAvailableBoxStatuses(this.box.status, this.box.progress).length > 0 : true;
    
    // Disable all actions if project is archived, on hold, or closed
    // Also disable box actions if box status doesn't allow them (OnHold or Dispatched)
    this.canEdit = baseCanEdit && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canPerformBoxActionsBasedOnStatus;
    this.canDelete = baseCanDelete && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canPerformBoxActionsBasedOnStatus;
    this.canUpdateBoxStatus = baseCanUpdateStatus && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canChangeStatusBasedOnBoxStatus;
    this.canUpdateQualityIssueStatus = baseCanUpdateQualityIssueStatus && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canPerformBoxActionsBasedOnStatus;
    this.canCreateQualityIssue = baseCanCreateQualityIssue && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canPerformBoxActionsBasedOnStatus;
    this.canUploadDrawing = baseCanUploadDrawing && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canPerformBoxActionsBasedOnStatus;
    
    console.log('✅ Box permissions checked:', {
      canEdit: this.canEdit,
      canDelete: this.canDelete,
      canUpdateBoxStatus: this.canUpdateBoxStatus,
      canCreateQualityIssue: this.canCreateQualityIssue,
      isProjectArchived: this.isProjectArchived,
      isProjectOnHold: this.isProjectOnHold,
      isProjectClosed: this.isProjectClosed,
      boxStatus: this.box?.status,
      canPerformBoxActionsBasedOnStatus,
      canChangeStatusBasedOnBoxStatus
    });
  }

  loadBox(): void {
    this.loading = true;
    this.error = '';
    
    this.boxService.getBox(this.boxId).subscribe({
      next: (box) => {
        this.box = box;
        
        // Initialize concrete panel and pod delivery data
        this.initializeDeliveryInfo(box);
        
        // Re-check permissions after box is loaded to account for box status
        this.checkPermissions();
        
        // Generate QR code if it doesn't exist
        if (!box.qrCode) {
          console.log('QR code not found, generating...');
          this.generateQRCode();
        }
        
        // Load activities if not already loaded (needed for Activity Progress Overview chart)
        if (!box.activities || box.activities.length === 0) {
          this.loadActivities();
        }
        
        this.loading = false;
        
        // After box is loaded, if we have a tab query parameter, trigger data loading for that tab
        const tabParam = this.route.snapshot.queryParams['tab'];
        if (tabParam && ['activities', 'wir', 'quality-issues', 'logs', 'drawings', 'progress-updates', 'attachments'].includes(tabParam)) {
          // Use setTimeout to ensure the component is fully initialized
          setTimeout(() => {
            this.setActiveTab(tabParam as any);
          }, 0);
        }
      },
      error: (err) => {
        this.error = err.message || 'Failed to load box details';
        this.loading = false;
        console.error('Error loading box:', err);
      }
    });
  }

  private initializeDeliveryInfo(box: Box): void {
    // Initialize panels from box.boxPanels
    this.boxPanels = box.boxPanels ? [...box.boxPanels] : [];
    
    // Initialize pod delivery
    this.podDeliverChecked = box.podDeliver ?? false;
    this.podName = box.podName ?? '';
    this.podType = box.podType ?? '';
    
    // Store original values for change detection
    this.originalPodDeliver = this.podDeliverChecked;
    this.originalPodName = this.podName;
    this.originalPodType = this.podType;
    this.hasPodDeliveryChanges = false;
  }

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
   * Toggle legend visibility
   */
  toggleLegend(): void {
    this.showLegend = !this.showLegend;
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
   * Get rejection reason for a panel from either first or second approval notes
   */
  getPanelRejectionReason(panel: BoxPanel): string | null {
    // Check if second approval was rejected
    if (panel.secondApprovalStatus === 'Rejected' && panel.secondApprovalNotes) {
      return panel.secondApprovalNotes;
    }
    
    // Check if first approval was rejected
    if (panel.firstApprovalStatus === 'Rejected' && panel.firstApprovalNotes) {
      return panel.firstApprovalNotes;
    }
    
    return null;
  }

  /**
   * Check if a panel has been rejected (either first or second approval)
   */
  isPanelRejected(panel: BoxPanel): boolean {
    return (panel.firstApprovalStatus === 'Rejected' && !!panel.firstApprovalNotes) ||
           (panel.secondApprovalStatus === 'Rejected' && !!panel.secondApprovalNotes);
  }

  togglePanelStatus(panel: BoxPanel, newStatus: PanelStatus): void {
    if (!this.box) return;
    
    // Update backend
    this.updatingDeliveryInfo = true;
    this.boxService.updateBoxPanelStatus(panel.boxPanelId, newStatus).subscribe({
      next: (response) => {
        // Reload box to get updated data
        this.loadBox();
        this.updatingDeliveryInfo = false;
        console.log('✅ Panel status updated successfully');
      },
      error: (err) => {
        console.error('❌ Error updating panel status:', err);
        this.updatingDeliveryInfo = false;
        alert('Failed to update panel status. Please try again.');
      }
    });
  }


  /**
   * Check if pod delivery data has changed
   */
  private checkPodDeliveryChanges(): void {
    this.hasPodDeliveryChanges = 
      this.podDeliverChecked !== this.originalPodDeliver ||
      this.podName !== this.originalPodName ||
      this.podType !== this.originalPodType;
  }

  togglePodDeliver(event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    this.podDeliverChecked = checkbox.checked;
    
    // If unchecking, clear pod info
    if (!this.podDeliverChecked) {
      this.podName = '';
      this.podType = '';
    }
    
    // Check for changes
    this.checkPodDeliveryChanges();
  }

  onPodDataChange(): void {
    // Check for changes when pod data is modified
    this.checkPodDeliveryChanges();
  }

  /**
   * Save pod delivery information to backend
   */
  savePodDeliveryInfo(): void {
    if (!this.box || !this.hasPodDeliveryChanges) {
      return;
    }
    
    this.updateDeliveryInfo({
      podDeliver: this.podDeliverChecked,
      podName: this.podName || null,
      podType: this.podType || null
    });
  }

  updatePodInfo(): void {
    // This method is kept for backward compatibility
    this.savePodDeliveryInfo();
  }

  private updateDeliveryInfo(deliveryInfo: any): void {
    if (!this.box) return;
    
    this.updatingDeliveryInfo = true;
    
    this.boxService.updateBoxDeliveryInfo(this.boxId, deliveryInfo).subscribe({
      next: (updatedBox) => {
        // Reload box to get all updated data including panels
        this.loadBox();
        this.updatingDeliveryInfo = false;
        console.log('✅ Delivery info updated successfully');
        
        // Show success message for pod delivery updates
        if (deliveryInfo.podDeliver !== undefined || deliveryInfo.podName !== undefined || deliveryInfo.podType !== undefined) {
          // You can add a toast notification here if you have one
          console.log('✅ Pod delivery information saved successfully');
        }
      },
      error: (err) => {
        console.error('❌ Error updating delivery info:', err);
        this.updatingDeliveryInfo = false;
        alert('Failed to update delivery information. Please try again.');
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
        const allCheckpoints = checkpoints || [];
        
        if (!allCheckpoints.length) {
          this.wirCheckpoints = [];
          this.wirLoading = false;
          return;
        }

        // Group checkpoints by WIR number AND BoxActivityId, then keep only the latest version
        this.wirCheckpoints = this.getLatestCheckpointsOnly(allCheckpoints);
        
        console.log(`📋 Loaded ${allCheckpoints.length} total checkpoint(s), showing ${this.wirCheckpoints.length} latest version(s)`);
        
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

  /**
   * Group checkpoints by WIR number and BoxActivityId, returning only the latest version for each group
   * This ensures that when a checkpoint has multiple versions (after rejection/resubmission),
   * only the latest version is displayed in the list.
   */
  private getLatestCheckpointsOnly(checkpoints: WIRCheckpoint[]): WIRCheckpoint[] {
    if (!checkpoints || checkpoints.length === 0) {
      return [];
    }

    // Clear the versions map
    this.allCheckpointVersionsMap.clear();

    // Group checkpoints by a composite key: wirNumber + boxActivityId
    const groupedMap = new Map<string, WIRCheckpoint[]>();
    
    checkpoints.forEach(checkpoint => {
      const wirNumber = (checkpoint.wirNumber || '').toUpperCase();
      const boxActivityId = checkpoint.boxActivityId || 'NO_ACTIVITY';
      
      // Create a composite key to group by both WIR number and activity
      const groupKey = `${wirNumber}_${boxActivityId}`;
      
      if (!groupedMap.has(groupKey)) {
        groupedMap.set(groupKey, []);
      }
      groupedMap.get(groupKey)!.push(checkpoint);
    });

    // For each group, sort by version descending and take the latest (first one)
    const latestCheckpoints: WIRCheckpoint[] = [];
    
    groupedMap.forEach((versions, groupKey) => {
      // Sort by version descending (highest version first)
      versions.sort((a, b) => (b.version || 1) - (a.version || 1));
      
      // Take the first one (latest version)
      const latest = versions[0];
      
      // Store the total version count in a custom property for display
      (latest as any)._totalVersions = versions.length;
      (latest as any)._groupKey = groupKey; // Store group key for version lookup
      
      // Store all versions in the map using wirId of the latest as key
      if (latest.wirId) {
        this.allCheckpointVersionsMap.set(latest.wirId, versions);
      }
      
      if (versions.length > 1) {
        console.log(`📋 Group "${groupKey}": Found ${versions.length} version(s), showing latest v${latest.version || 1}`);
      }
      
      latestCheckpoints.push(latest);
    });

    // Sort by WIR number for consistent display order
    latestCheckpoints.sort((a, b) => {
      const numA = parseInt(a.wirNumber?.replace(/\D/g, '') || '0');
      const numB = parseInt(b.wirNumber?.replace(/\D/g, '') || '0');
      return numA - numB;
    });

    return latestCheckpoints;
  }

  /**
   * Get the version label for a checkpoint
   * Returns "v{number}" (e.g., "v2", "v3") or empty string if version 1
   */
  getCheckpointVersionLabel(checkpoint: WIRCheckpoint): string {
    const version = checkpoint.version || 1;
    return version > 1 ? `v${version}` : '';
  }

  /**
   * Check if a checkpoint has multiple versions
   */
  hasMultipleVersions(checkpoint: WIRCheckpoint): boolean {
    const totalVersions = (checkpoint as any)._totalVersions || 1;
    return totalVersions > 1;
  }

  /**
   * Get total version count for a checkpoint
   */
  getTotalVersionCount(checkpoint: WIRCheckpoint): number {
    return (checkpoint as any)._totalVersions || 1;
  }

  /**
   * Toggle expansion of checkpoint versions
   */
  toggleCheckpointVersions(checkpoint: WIRCheckpoint): void {
    if (!checkpoint.wirId) return;
    
    if (this.expandedCheckpointVersions.has(checkpoint.wirId)) {
      this.expandedCheckpointVersions.delete(checkpoint.wirId);
    } else {
      this.expandedCheckpointVersions.add(checkpoint.wirId);
    }
  }

  /**
   * Check if checkpoint versions are expanded
   */
  isCheckpointVersionsExpanded(checkpoint: WIRCheckpoint): boolean {
    return checkpoint.wirId ? this.expandedCheckpointVersions.has(checkpoint.wirId) : false;
  }

  /**
   * Get older versions of a checkpoint (excluding the latest)
   */
  getOlderVersions(checkpoint: WIRCheckpoint): WIRCheckpoint[] {
    if (!checkpoint.wirId) return [];
    
    const allVersions = this.allCheckpointVersionsMap.get(checkpoint.wirId) || [];
    // Return all except the first one (which is the latest)
    return allVersions.slice(1);
  }

  /**
   * Check if a checkpoint is an older version (not the latest)
   */
  isOlderVersion(checkpoint: WIRCheckpoint, latestCheckpoint: WIRCheckpoint): boolean {
    return checkpoint.wirId !== latestCheckpoint.wirId;
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
    // Navigate back to boxes filtered by current box type
    const queryParams: any = {};
    
    if (this.box?.boxTypeName) {
      queryParams.boxType = this.box.boxTypeName;
      
      if (this.box.boxSubTypeName) {
        queryParams.boxSubType = this.box.boxSubTypeName;
      }
    }
    
    this.router.navigate(['/projects', this.projectId, 'boxes'], { queryParams });
  }

  // Activity chart methods
  getActivityCountByStatus(status: string): number {
    if (!this.box || !this.box.activities) {
      console.log('⚠️ Chart Debug: No box or activities', { box: !!this.box, activities: !!this.box?.activities });
      return 0;
    }
    
    // Normalize status for comparison (case-insensitive, handle enum values)
    const normalizedStatus = status.toString().toLowerCase();
    const count = this.box.activities.filter(activity => {
      if (!activity.status) return false;
      const activityStatus = activity.status.toString().toLowerCase();
      
      // For "Completed" status, also include "Delayed" activities with 100% progress
      if (normalizedStatus === 'completed') {
        const isCompleted = activityStatus === 'completed';
        const isDelayedWith100Progress = activityStatus === 'delayed' && 
                                         (activity.weightPercentage >= 100 || 
                                          (activity as any).progressPercentage >= 100);
        return isCompleted || isDelayedWith100Progress;
      }
      
      return activityStatus === normalizedStatus;
    }).length;
    
    console.log(`📊 Chart Debug: ${status} count = ${count} out of ${this.box.activities.length} total activities`, {
      status,
      activities: this.box.activities.map(a => ({ name: a.name, status: a.status, progress: a.weightPercentage || (a as any).progressPercentage }))
    });
    return count;
  }

  getActivityPercentage(status: string): number {
    if (!this.box || !this.box.activities || this.box.activities.length === 0) return 0;
    const count = this.getActivityCountByStatus(status);
    const percentage = Math.round((count / this.box.activities.length) * 100);
    console.log(`📊 Chart Debug: ${status} percentage = ${percentage}%`);
    return percentage;
  }

  // Donut chart calculations
  getCircleSegment(status: string): string {
    const percentage = this.getActivityPercentage(status);
    const circumference = 2 * Math.PI * 80; // radius = 80
    const segmentLength = (percentage / 100) * circumference;
    return `${segmentLength} ${circumference}`;
  }

  getCircleOffset(status: string): number {
    const circumference = 2 * Math.PI * 80;
    let offset = 0;
    
    // Calculate cumulative offset based on previous segments
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

  editBox(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId, 'edit']);
  }

  openDeleteConfirm(): void {
    this.showDeleteConfirm = true;
    this.error = '';
    this.deleteSuccess = false;
  }

  cancelDelete(): void {
    this.showDeleteConfirm = false;
    this.error = '';
    this.deleteSuccess = false;
  }

  deleteBox(): void {
    if (this.deleting) {
      return;
    }

    this.deleting = true;
    this.error = '';
    this.deleteSuccess = false;
    this.boxService.deleteBox(this.boxId).subscribe({
      next: () => {
        this.deleting = false;
        this.deleteSuccess = true;
        // Show success message and navigate after a delay
        setTimeout(() => {
          this.showDeleteConfirm = false;
          // Navigate back to boxes filtered by the deleted box's type
          const queryParams: any = {};
          if (this.box?.boxTypeName) {
            queryParams.boxType = this.box.boxTypeName;
            if (this.box.boxSubTypeName) {
              queryParams.boxSubType = this.box.boxSubTypeName;
            }
          }
          this.router.navigate(['/projects', this.projectId, 'boxes'], { queryParams });
        }, 1500);
      },
      error: (err) => {
        this.deleting = false;
        this.deleteSuccess = false;
        
        // Extract error message from backend response
        let errorMessage = 'Failed to delete box';
        if (err?.error) {
          if (err.error.errors && typeof err.error.errors === 'object') {
            const errors = Object.values(err.error.errors).flat() as string[];
            errorMessage = errors.length > 0 ? errors[0] : errorMessage;
          } else if (err.error.message) {
            const message = err.error.message;
            // Remove property name prefix if present (e.g., "BoxTag: Error message" -> "Error message")
            errorMessage = message.includes(': ') && /^[A-Za-z]+: /.test(message)
              ? message.substring(message.indexOf(': ') + 2)
              : message;
          } else if (typeof err.error === 'string') {
            errorMessage = err.error;
          }
        } else if (err?.message) {
          errorMessage = err.message;
        }
        
        this.error = errorMessage;
        console.error('Error deleting box:', err);
      }
    });
  }

  copiedSerialNumber = false;
  private copyTimeout?: ReturnType<typeof setTimeout>;

  formatSerialNumber(serialNumber: string | undefined): string {
    if (!serialNumber) return '';
    // Ensure consistent format: SN-YYYY-######
    // If already in correct format, return as is
    if (/^SN-\d{4}-\d{6}$/.test(serialNumber)) {
      return serialNumber;
    }
    // If format is slightly different, try to normalize
    return serialNumber;
  }

  copyToClipboard(text: string): void {
    if (!text) return;

    navigator.clipboard.writeText(text).then(() => {
      this.copiedSerialNumber = true;
      
      // Clear any existing timeout
      if (this.copyTimeout) {
        clearTimeout(this.copyTimeout);
      }
      
      // Reset the copied state after 2 seconds
      this.copyTimeout = setTimeout(() => {
        this.copiedSerialNumber = false;
      }, 2000);
    }).catch(err => {
      console.error('Failed to copy to clipboard:', err);
      // Fallback for older browsers
      const textArea = document.createElement('textarea');
      textArea.value = text;
      textArea.style.position = 'fixed';
      textArea.style.opacity = '0';
      document.body.appendChild(textArea);
      textArea.select();
      try {
        document.execCommand('copy');
        this.copiedSerialNumber = true;
        if (this.copyTimeout) {
          clearTimeout(this.copyTimeout);
        }
        this.copyTimeout = setTimeout(() => {
          this.copiedSerialNumber = false;
        }, 2000);
      } catch (err) {
        console.error('Fallback copy failed:', err);
      }
      document.body.removeChild(textArea);
    });
  }

  ngOnDestroy(): void {
    if (this.copyTimeout) {
      clearTimeout(this.copyTimeout);
    }
  }

  downloadQRCode(): void {
    if (this.box?.qrCode) {
      const link = document.createElement('a');
      link.href = this.box.qrCode;
      link.download = `QR-${this.box.code}${this.box.serialNumber ? '-' + this.box.serialNumber : ''}.png`;
      link.click();
    }
  }

  setActiveTab(tab: 'overview' | 'activities' | 'wir' | 'quality-issues' | 'logs' | 'drawings' | 'progress-updates' | 'attachments' | 'panels'): void {
    this.activeTab = tab;
    
    // Scroll to top of page for better UX when switching tabs
    window.scrollTo({ top: 0, behavior: 'smooth' });
    
    // Lazy load data when tab is clicked
    if (tab === 'activities') {
      // Load activities when Activities tab is clicked
      if (!this.box?.activities || this.box.activities.length === 0) {
        this.loadActivities();
      }
    }
    if (tab === 'wir') {
      // Load WIR checkpoints when WIR tab is clicked
      if (this.wirCheckpoints.length === 0 && !this.wirLoading) {
        this.loadWIRCheckpoints();
      }
    }
    if (tab === 'quality-issues') {
      // Load quality issues when Quality Issues tab is clicked
      if (this.qualityIssues.length === 0 && !this.qualityIssuesLoading) {
        this.loadQualityIssues();
      }
    }
    if (tab === 'logs') {
      // Load logs when clicking the tab if logs haven't been loaded yet
      if (this.boxLogs.length === 0 && !this.boxLogsLoading) {
        this.loadBoxLogs(this.boxLogsCurrentPage, this.boxLogsPageSize);
      }
    }
    if (tab === 'drawings') {
      // Load box drawings when drawings tab is opened
      if (this.boxDrawings.length === 0 && !this.loadingBoxDrawings) {
        this.loadBoxDrawings();
      }
    }
    if (tab === 'progress-updates') {
      // Load progress updates when Progress History tab is clicked
      // Service-level cache will handle whether to fetch from API or use cached data
      if (this.progressUpdates.length === 0 && !this.progressUpdatesLoading) {
        this.loadProgressUpdates(1, this.progressUpdatesPageSize);
      }
    }
    if (tab === 'attachments') {
      // Load all box attachments when Attachments tab is clicked
      if (!this.boxAttachments && !this.loadingBoxAttachments) {
        this.loadAllBoxAttachments();
      }
    }
  }

  getStatusClass(status: BoxStatus): string {
    const statusMap: Record<BoxStatus, string> = {
      [BoxStatus.NotStarted]: 'badge-secondary',
      [BoxStatus.InProgress]: 'badge-warning',
      [BoxStatus.QAReview]: 'badge-info',
      [BoxStatus.Completed]: 'badge-success',
      [BoxStatus.ReadyForDelivery]: 'badge-primary',
      [BoxStatus.Delivered]: 'badge-success',
      [BoxStatus.OnHold]: 'badge-danger',
      [BoxStatus.Dispatched]: 'badge-primary'
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
      [BoxStatus.OnHold]: 'On Hold',
      [BoxStatus.Dispatched]: 'Dispatched'
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

  /**
   * Transform WIR number to Stage number for display purposes only
   * E.g., "WIR-1" -> "Stage-1", "WIR-2" -> "Stage-2"
   */
  getDisplayWirNumber(wirNumber: string | undefined | null): string {
    if (!wirNumber) return 'Stage';
    return wirNumber.replace(/WIR-/gi, 'Stage-');
  }

  /**
   * Transform WIR text to Stage text for display purposes only
   * Replaces "WIR-X" with "Stage-X" and "WIR " with "Stage " in any text
   */
  getDisplayWirText(text: string | undefined | null): string {
    if (!text) return '';
    return text.replace(/WIR-/gi, 'Stage-').replace(/WIR /gi, 'Stage ');
  }

  canNavigateToAddChecklist(checkpoint: WIRCheckpoint | null): boolean {
    // Check if checkpoint exists and has required data
    if (!checkpoint?.wirId || !checkpoint?.boxActivityId) {
      return false;
    }
    // Check if box status allows actions
    if (this.box && !canPerformBoxActions(this.box.status)) {
      return false;
    }
    // Check if project is archived or on hold
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed) {
      return false;
    }
    // Check if user has permission to create/manage WIR checkpoints
    return this.permissionService.hasPermission('wir', 'create') || 
           this.permissionService.hasPermission('wir', 'manage');
  }

  canNavigateToReview(checkpoint: WIRCheckpoint | null): boolean {
    // Check if checkpoint exists and has required data
    if (!checkpoint?.boxActivityId) {
      return false;
    }
    // Check if box status allows actions
    if (this.box && !canPerformBoxActions(this.box.status)) {
      return false;
    }
    // Check if project is archived or on hold
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed) {
      return false;
    }
    // Check if user has permission to review WIR checkpoints
    return this.permissionService.hasPermission('wir', 'review') || 
           this.permissionService.hasPermission('wir', 'manage');
  }

  getApprovedCheckpointsCount(): number {
    return this.wirCheckpoints.filter(cp => 
      cp.status?.toLowerCase() === WIRCheckpointStatus.Approved.toLowerCase()
    ).length;
  }

  getPendingCheckpointsCount(): number {
    return this.wirCheckpoints.filter(cp => 
      cp.status?.toLowerCase() === WIRCheckpointStatus.Pending.toLowerCase()
    ).length;
  }

  getRejectedCheckpointsCount(): number {
    return this.wirCheckpoints.filter(cp => 
      cp.status?.toLowerCase() === WIRCheckpointStatus.Rejected.toLowerCase()
    ).length;
  }

  getConditionalApprovedCheckpointsCount(): number {
    return this.wirCheckpoints.filter(cp => 
      cp.status?.toLowerCase() === WIRCheckpointStatus.ConditionalApproval.toLowerCase()
    ).length;
  }

  navigateToAddChecklist(checkpoint: WIRCheckpoint): void {
    if (!this.canNavigateToAddChecklist(checkpoint)) {
      // Check if blocked by box status
      if (this.box && !canPerformBoxActions(this.box.status)) {
        const statusMessage = this.box.status === 'Dispatched' 
          ? 'Cannot add checklist items. The box is dispatched and no actions are allowed.'
          : 'Cannot add checklist items. The box is on hold and no actions are allowed. Only box status changes are allowed.';
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: statusMessage, type: 'error' }
        }));
      }
      return;
    }

    // Navigate to add-items step (Step 2) since checkpoint already exists
    this.router.navigate([
      '/projects',
      this.projectId,
      'boxes',
      this.boxId,
      'activities',
      checkpoint.boxActivityId,
      'qa-qc'
    ], {
      queryParams: { step: 'add-items' }
    });
  }

  navigateToReview(checkpoint: WIRCheckpoint): void {
    if (!this.canNavigateToReview(checkpoint)) {
      // Check if blocked by box status
      if (this.box && !canPerformBoxActions(this.box.status)) {
        const statusMessage = this.box.status === 'Dispatched' 
          ? 'Cannot review checkpoint. The box is dispatched and no actions are allowed.'
          : 'Cannot review checkpoint. The box is on hold and no actions are allowed. Only box status changes are allowed.';
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: statusMessage, type: 'error' }
        }));
      }
      return;
    }

    // Determine the appropriate step based on checkpoint status
    // If checklist items exist, go to review step, otherwise go to add-items
    let targetStep: string = 'review';
    if (!checkpoint.checklistItems || checkpoint.checklistItems.length === 0) {
      targetStep = 'add-items'; // Need to add checklist items first
    }

    this.router.navigate([
      '/projects',
      this.projectId,
      'boxes',
      this.boxId,
      'activities',
      checkpoint.boxActivityId,
      'qa-qc'
    ], {
      queryParams: { step: targetStep }
    });
  }

  exportWIRToPDF(checkpoint: WIRCheckpoint): void {
    // Export as PDF via print dialog
    this.wirExportService.exportWIRToPrintDialog(checkpoint, this.box, null);
  }

  async downloadWIRAsPDF(checkpoint: WIRCheckpoint): Promise<void> {
    // Direct PDF download with DuBox logo watermark (no print dialog)
    await this.wirExportService.downloadWIRAsPDF(checkpoint, this.box, null);
  }

  downloadWIRAsHTML(checkpoint: WIRCheckpoint): void {
    // Download as HTML file (legacy support)
    this.wirExportService.downloadWIRAsHTML(checkpoint, this.box, null);
  }

  loadQualityIssues(): void {
    if (!this.boxId) {
      return;
    }

    this.qualityIssuesLoading = true;
    this.qualityIssuesError = '';

    this.wirService.getQualityIssuesByBox(this.boxId).subscribe({
      next: (issues) => {
        // Filter out incomplete/invalid quality issues
        // A valid quality issue should have at least an issueId and issueDate
        const validIssues = (issues || []).filter(issue => {
          console.log('🔍 VERSION DEBUG - Quality issue:', issue);
          // Must have a valid issueId
          if (!issue.issueId) {
            console.warn('⚠️ Quality issue missing issueId:', issue);
            return false;
          }
          // Must have a valid issueDate
          if (!issue.issueDate) {
            console.warn('⚠️ Quality issue missing issueDate:', issue);
            return false;
          }
          return true;
        });
        
        this.qualityIssues = validIssues;
        this.qualityIssueCount = this.qualityIssues.length;
        this.qualityIssuesLoading = false;
        
        if (validIssues.length < (issues || []).length) {
          console.warn(`⚠️ Filtered out ${(issues || []).length - validIssues.length} incomplete quality issue(s)`);
        }
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

  loadQualityIssuesAndOpenIssue(issueId: string, commentId?: string): void {
    if (!this.boxId) {
      return;
    }

    this.qualityIssuesLoading = true;
    this.qualityIssuesError = '';

    this.wirService.getQualityIssuesByBox(this.boxId).subscribe({
      next: (issues) => {
        console.log('📋 Quality issues loaded for opening specific issue:', issues);
        
        // Filter valid issues
        const validIssues = (issues || []).filter((issue: any) => {
          const isValid = issue && typeof issue === 'object' && issue.issueId;
          if (!isValid) {
            console.warn('⚠️ Invalid quality issue detected:', issue);
          }
          return isValid;
        });
        
        this.qualityIssues = validIssues;
        this.qualityIssueCount = this.qualityIssues.length;
        this.qualityIssuesLoading = false;
        
        // Find and open the specific issue
        const issueToOpen = this.qualityIssues.find(issue => issue.issueId === issueId);
        
        if (issueToOpen) {
          console.log('🎯 Opening issue from notification:', issueToOpen);
          
          // Store commentId if provided
          if (commentId) {
            this.selectedCommentId = commentId;
            console.log('💬 Comment to focus:', commentId);
          }
          
          // Open the issue details modal
          setTimeout(() => {
            this.openIssueDetails(issueToOpen);
          }, 300);
        } else {
          console.warn('⚠️ Issue not found:', issueId);
          alert('Issue not found');
        }
      },
      error: (err) => {
        console.error('❌ Error loading quality issues:', err);
        this.qualityIssuesError = err?.error?.message || err?.message || 'Failed to load quality issues';
        this.qualityIssuesLoading = false;
        alert('Failed to load issue. Please try again.');
      }
    });
  }

  // Create Quality Issue Methods
  openCreateQualityIssueModal(): void {
    this.isCreateQualityIssueModalOpen = true;
    this.createQualityIssueError = '';
    this.loadAvailableTeams();
    this.loadAllUsersForCC();
    this.newQualityIssueForm = {
      issueType: 'Defect',
      severity: 'Major',
      issueDescription: '',
      assignedTo: '',
      assignedToUserId: '',
      ccUserId: '',
      dueDate: ''
    };
    this.availableTeamUsers = [];
    this.qualityIssueImages = [];
    this.currentImageInputMode = 'url';
    this.currentUrlInput = '';
    this.showCamera = false;
  }

  closeCreateQualityIssueModal(): void {
    this.isCreateQualityIssueModalOpen = false;
    this.createQualityIssueError = '';
    this.newQualityIssueForm = {
      issueType: 'Defect',
      severity: 'Major',
      issueDescription: '',
      assignedTo: '',
      assignedToUserId: '',
      ccUserId: '',
      dueDate: ''
    };
    this.availableTeamUsers = [];
    this.qualityIssueImages = [];
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach(track => track.stop());
      this.cameraStream = null;
    }
    this.showCamera = false;
  }

  createQualityIssue(): void {
    // Check if box status allows actions
    if (this.box && !canPerformBoxActions(this.box.status)) {
      const statusMessage = this.box.status === 'Dispatched' 
        ? 'Cannot create quality issue. The box is dispatched and no actions are allowed.'
        : 'Cannot create quality issue. The box is on hold and no actions are allowed. Only box status changes are allowed.';
      this.createQualityIssueError = statusMessage;
      return;
    }
    
    // Validate form
    if (!this.newQualityIssueForm.issueDescription?.trim()) {
      this.createQualityIssueError = 'Issue description is required.';
      return;
    }

    this.createQualityIssueLoading = true;
    this.createQualityIssueError = '';

    // Separate files from URLs
    const files: File[] = [];
    const imageUrls: string[] = [];
    const fileNames: string[] = [];

    this.qualityIssueImages.forEach(img => {
      if (img.type === 'url' && img.url) {
        imageUrls.push(img.url.trim());
      } else if ((img.type === 'file' || img.type === 'camera') && img.file) {
        files.push(img.file);
        fileNames.push(img.name || img.file.name);
      } else if (img.preview && img.preview.startsWith('data:image/')) {
        imageUrls.push(img.preview);
      }
    });

    console.log('📎 VERSION DEBUG - Quality Issue uploading files with names:', fileNames);

    // Debug CC User ID
    console.log('📋 CC User ID from form:', this.newQualityIssueForm.ccUserId);
    console.log('📋 CC User ID type:', typeof this.newQualityIssueForm.ccUserId);
    
    // Handle CC User ID properly - it might be undefined, null, or empty string from the select
    const ccUserIdValue = this.newQualityIssueForm.ccUserId;
    let processedCCUserId: string | undefined = undefined;
    
    if (ccUserIdValue !== null && ccUserIdValue !== undefined && ccUserIdValue !== '') {
      const ccUserIdString = String(ccUserIdValue).trim();
      if (ccUserIdString !== '' && ccUserIdString !== 'undefined' && ccUserIdString !== 'null') {
        processedCCUserId = ccUserIdString;
      }
    }
    
    console.log('📤 Processed CC User ID:', processedCCUserId);

    const request: CreateQualityIssueForBoxRequest = {
      boxId: this.boxId,
      issueType: this.newQualityIssueForm.issueType,
      severity: this.newQualityIssueForm.severity,
      issueDescription: this.newQualityIssueForm.issueDescription.trim(),
      assignedTo: this.newQualityIssueForm.assignedTo?.trim() || undefined, // Team ID as string
      assignedToUserId: this.newQualityIssueForm.assignedToUserId?.trim() || undefined, // User ID within team
      ccUserId: processedCCUserId, // CC User ID - properly processed to avoid "undefined" string
      dueDate: this.newQualityIssueForm.dueDate || undefined,
      imageUrls: imageUrls.length > 0 ? imageUrls : undefined,
      files: files.length > 0 ? files : undefined,
      fileNames: fileNames.length > 0 ? fileNames : undefined
    };

    console.log('📤 Creating quality issue with request:', {
      boxId: request.boxId,
      issueType: request.issueType,
      severity: request.severity,
      descriptionLength: request.issueDescription.length,
      assignedTo: request.assignedTo,
      assignedToUserId: request.assignedToUserId,
      ccUserId: request.ccUserId,
      dueDate: request.dueDate,
      imageUrlsCount: imageUrls.length,
      filesCount: files.length
    });
    console.log('📋 Form values before sending:', {
      assignedTo: this.newQualityIssueForm.assignedTo,
      assignedToUserId: this.newQualityIssueForm.assignedToUserId
    });

    this.wirService.createQualityIssueForBox(request).subscribe({
      next: (createdIssue) => {
        console.log('✅ Quality issue created successfully:', createdIssue);
        this.createQualityIssueLoading = false;
        this.closeCreateQualityIssueModal();
        
        // Show success toast
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Quality issue created successfully.', type: 'success' }
        }));
        
        // Reload quality issues
        this.loadQualityIssues();
      },
      error: (err) => {
        console.error('❌ Error creating quality issue:', err);
        console.error('Full error details:', {
          status: err?.status,
          statusText: err?.statusText,
          error: err?.error,
          message: err?.message
        });
        
        // Extract the most useful error message
        let errorMessage = 'Failed to create quality issue.';
        
        if (err?.error?.message) {
          errorMessage = err.error.message;
        } else if (err?.error?.title) {
          errorMessage = err.error.title;
        } else if (err?.error?.errors) {
          // Handle validation errors
          const errors = err.error.errors;
          const errorList = Object.keys(errors).map(key => `${key}: ${errors[key].join(', ')}`);
          errorMessage = errorList.join('; ');
        } else if (err?.message) {
          errorMessage = err.message;
        }
        
        this.createQualityIssueError = errorMessage;
        this.createQualityIssueLoading = false;
      }
    });
  }

  loadAvailableTeams(onComplete?: () => void): void {
    this.loadingTeams = true;
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        // Filter to only active teams that user has access to
        // The backend already filters based on user permissions
        this.availableTeams = teams.filter(team => team.isActive);
        this.loadingTeams = false;
        if (onComplete) {
          onComplete();
        }
      },
      error: (err) => {
        console.error('❌ Error loading teams:', err);
        if (onComplete) {
          onComplete();
        }
        this.availableTeams = [];
        this.loadingTeams = false;
      }
    });
  }

  onQualityIssueTeamChange(): void {
    // Reset user selection when team changes
    this.newQualityIssueForm.assignedToUserId = '';
    this.availableTeamUsers = [];
    this.availableTeamMembers = [];

    // Load users if a team is selected
    if (this.newQualityIssueForm.assignedTo) {
      this.loadTeamMembersForAssignment(this.newQualityIssueForm.assignedTo);
    }
  }

  loadTeamUsers(teamId: string): void {
    this.loadingTeamUsers = true;
    this.teamService.getTeamUsers(teamId).subscribe({
      next: (users) => {
        console.log('📥 Raw team users received:', users);
        // Filter to only include users that have a userId
        this.availableTeamUsers = users.filter(user => user.userId && user.userId.trim() !== '');
        console.log('✅ Filtered team users (with userId):', this.availableTeamUsers);
        this.loadingTeamUsers = false;
      },
      error: (err) => {
        console.error('❌ Error loading team users:', err);
        this.availableTeamUsers = [];
        this.loadingTeamUsers = false;
      }
    });
  }

  loadTeamMembersForAssignment(teamId: string): void {
    this.loadingTeamUsers = true;
    this.teamService.getTeamMembers(teamId).subscribe({
      next: (response) => {
        console.log('📥 Raw team members received:', response);
        // Filter to only include members that have a userId (actual user account)
        this.availableTeamMembers = (response.members || []).filter(member => member.userId && member.userId.trim() !== '');
        
        // Map to the format expected by the dropdown
        this.availableTeamUsers = this.availableTeamMembers.map(member => ({
          userId: member.teamMemberId, // Use teamMemberId for assignment
          userName: member.employeeName || member.fullName || 'Unknown',
          userEmail: member.email || ''
        }));
        
        console.log('✅ Filtered team members (with userId):', this.availableTeamMembers);
        console.log('✅ Mapped to dropdown format:', this.availableTeamUsers);
        this.loadingTeamUsers = false;
      },
      error: (err) => {
        console.error('❌ Error loading team members:', err);
        this.availableTeamUsers = [];
        this.availableTeamMembers = [];
        this.loadingTeamUsers = false;
      }
    });
  }

  loadAllUsersForCC(): void {
    this.loadingCCUsers = true;
    this.userService.getUsers(1, 1000).subscribe({
      next: (response) => {
        this.availableCCUsers = response.items
          .filter((user: any) => user.userId) // Only include users with valid IDs
          .map((user: any) => ({
            userId: user.userId, // UserDto has userId field, not id
            userName: user.fullName || user.email || 'Unknown',
            userEmail: user.email || ''
          }));
        console.log('✅ Loaded CC Users:', this.availableCCUsers);
        console.log('✅ Total CC Users loaded:', this.availableCCUsers.length);
        this.loadingCCUsers = false;
      },
      error: (err) => {
        console.error('❌ Error loading users for CC:', err);
        this.availableCCUsers = [];
        this.loadingCCUsers = false;
      }
    });
  }

  // Image management for create quality issue
  addQualityIssueImageUrl(): void {
    const url = this.currentUrlInput.trim();
    if (!url) {
      return;
    }

    const imageId = `url-${Date.now()}`;
    
    // Determine preview URL based on input format
    let preview = url;
    if (!url.startsWith('http://') && !url.startsWith('https://') && !url.startsWith('data:image/')) {
      // Assume it's base64 without the data URL prefix
      preview = `data:image/jpeg;base64,${url}`;
    }
    
    this.qualityIssueImages.push({
      id: imageId,
      type: 'url',
      url: url,
      preview: preview
    });

    this.currentUrlInput = '';
  }

  onQualityIssueFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) {
      return;
    }

    Array.from(input.files).forEach((file) => {
      if (!file.type.startsWith('image/')) {
        alert(`${file.name} is not an image file.`);
        return;
      }

      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        const imageId = `file-${Date.now()}-${Math.random()}`;
        this.qualityIssueImages.push({
          id: imageId,
          type: 'file',
          file: file,
          preview: e.target?.result as string,
          name: file.name,
          size: file.size
        });
      };
      reader.readAsDataURL(file);
    });

    // Reset input
    input.value = '';
  }

  openQualityIssueFileInput(): void {
    const fileInput = document.getElementById('qualityIssueFileInput') as HTMLInputElement;
    if (fileInput) {
      fileInput.click();
    }
    this.currentImageInputMode = 'upload';
    this.showCamera = false;
  }

  async openQualityIssueCamera(): Promise<void> {
    this.currentImageInputMode = 'camera';
    this.showCamera = true;

    try {
      const stream = await navigator.mediaDevices.getUserMedia({ 
        video: { facingMode: 'environment' } 
      });
      this.cameraStream = stream;

      setTimeout(() => {
        const videoElement = document.getElementById('qualityIssueCameraVideo') as HTMLVideoElement;
        if (videoElement) {
          videoElement.srcObject = stream;
        }
      }, 100);
    } catch (error) {
      console.error('Error accessing camera:', error);
      alert('Unable to access camera. Please check permissions.');
      this.showCamera = false;
    }
  }

  captureQualityIssuePhoto(): void {
    const videoElement = document.getElementById('qualityIssueCameraVideo') as HTMLVideoElement;
    const canvas = document.createElement('canvas');
    canvas.width = videoElement.videoWidth;
    canvas.height = videoElement.videoHeight;
    const context = canvas.getContext('2d');
    if (context) {
      context.drawImage(videoElement, 0, 0);
      canvas.toBlob((blob) => {
        if (blob) {
          const file = new File([blob], `camera-capture-${Date.now()}.jpg`, { type: 'image/jpeg' });
          const imageId = `camera-${Date.now()}`;
          this.qualityIssueImages.push({
            id: imageId,
            type: 'camera',
            file: file,
            preview: canvas.toDataURL('image/jpeg'),
            name: file.name,
            size: file.size
          });
        }
      }, 'image/jpeg', 0.9);
    }
  }

  closeQualityIssueCamera(): void {
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach(track => track.stop());
      this.cameraStream = null;
    }
    this.showCamera = false;
    this.currentImageInputMode = 'url';
  }

  removeQualityIssueImage(imageId: string): void {
    this.qualityIssueImages = this.qualityIssueImages.filter(img => img.id !== imageId);
  }

  setQualityIssueImageInputMode(mode: 'url' | 'upload' | 'camera'): void {
    this.currentImageInputMode = mode;
    if (mode !== 'camera') {
      this.closeQualityIssueCamera();
    }
  }

  async exportBoxLogsToExcel(): Promise<void> {
    // Use the same pattern as audit logs export - export filtered logs
    if (!this.boxLogs || this.boxLogs.length === 0) {
      alert('No data to export');
      return;
    }

    // Helper function to parse raw values (pipe-separated, JSON, or object)
    const parseRawValues = (raw: any): Record<string, any> => {
      if (!raw) {
        return {};
      }

      if (typeof raw === 'object' && raw !== null && !Array.isArray(raw)) {
        return raw;
      }

      if (typeof raw === 'string') {
        const trimmed = raw.trim();
        if (!trimmed) {
          return {};
        }

        try {
          // Try JSON first
          if (trimmed.startsWith('{') || trimmed.startsWith('[')) {
            return JSON.parse(trimmed);
          }
        } catch {
          // Not JSON, continue to parse as delimited format
        }

        // Parse pipe-separated, comma-separated, or newline-separated format
        // Format: "Key: Value | Key: Value | Key: Value"
        const fallback: Record<string, string> = {};
        
        let segments: string[] = [];
        if (trimmed.includes('|')) {
          segments = trimmed.split(/\s*\|\s*/);
        } else if (trimmed.includes('\n')) {
          segments = trimmed.split(/\r?\n/);
        } else {
          segments = trimmed.split(/,(?=\s*[A-Za-z][A-Za-z0-9]*\s*:)/);
        }

        segments.forEach((segment) => {
          const cleanSegment = segment.trim();
          if (!cleanSegment) {
            return;
          }

          const separatorIndex = cleanSegment.indexOf(':');
          if (separatorIndex === -1) {
            return;
          }

          const key = cleanSegment.slice(0, separatorIndex).trim();
          const value = cleanSegment.slice(separatorIndex + 1).trim();

          if (key) {
            fallback[key] = value || '—';
          }
        });

        return fallback;
      }

      return {};
    };

    // Helper function to get all changes from log (similar to audit logs)
    const getChangesFromLog = (log: BoxLog): Array<{ field: string; oldValue: string | null; newValue: string | null }> => {
      const changes: Array<{ field: string; oldValue: string | null; newValue: string | null }> = [];

      // First, try to parse oldValues and newValues (preferred method)
      if (log.oldValues || log.newValues) {
        const oldVals = parseRawValues(log.oldValues);
        const newVals = parseRawValues(log.newValues);

        const allKeys = new Set<string>([
          ...Object.keys(oldVals),
          ...Object.keys(newVals)
        ]);

        allKeys.forEach(key => {
          // Skip internal/system fields
          const skipFields = ['Id', 'CreatedDate', 'ModifiedDate', 'CreatedBy', 'ModifiedBy', 'AuditId', 'ChangedBy', 'ChangedDate'];
          if (skipFields.some(f => key.toLowerCase().includes(f.toLowerCase()))) {
            return;
          }

          const oldVal = oldVals[key];
          const newVal = newVals[key];
          
          // Convert to strings for comparison
          const oldStr = oldVal !== null && oldVal !== undefined ? String(oldVal) : null;
          const newStr = newVal !== null && newVal !== undefined ? String(newVal) : null;
          
          // Only include if values are different
          if (oldStr !== newStr) {
            changes.push({
              field: key,
              oldValue: oldStr,
              newValue: newStr
            });
          }
        });
      }

      // Fallback: use single field/oldValue/newValue if no oldValues/newValues
      if (changes.length === 0 && (log.field || log.oldValue || log.newValue)) {
        const oldVal = log.oldValue !== null && log.oldValue !== undefined 
          ? String(log.oldValue) 
          : null;
        const newVal = log.newValue !== null && log.newValue !== undefined 
          ? String(log.newValue) 
          : null;
        
        if (oldVal !== newVal) {
          changes.push({
            field: log.field || 'Field',
            oldValue: oldVal,
            newValue: newVal
          });
        }
      }

      return changes;
    };

    // Helper function to format changes text (similar to audit logs)
    const formatChangesText = (log: BoxLog): string => {
      const changes = getChangesFromLog(log);
      
      if (changes.length === 0) {
        return 'No changes';
      }

      try {
        const formatted = changes
          .map(change => {
            const fieldName = change.field || 'Unknown Field';
            const oldVal = change.oldValue !== null && change.oldValue !== undefined 
              ? DiffUtil.formatValue(change.oldValue) 
              : '—';
            const newVal = change.newValue !== null && change.newValue !== undefined 
              ? DiffUtil.formatValue(change.newValue) 
              : '—';
            return `${fieldName}: ${oldVal} → ${newVal}`;
          })
          .join('; ');
        
        return formatted || 'No changes';
      } catch (e) {
        console.error('Error formatting changes:', e, changes);
        return 'Error formatting changes';
      }
    };

    // Format dates using the same format as audit logs
    const formatDateForExcel = (date?: string | Date): string => {
      if (!date) return '—';
      const d = typeof date === 'string' ? new Date(date) : date;
      if (isNaN(d.getTime())) return '—';
      return d.toLocaleString('en-US', { 
        year: 'numeric', 
        month: 'short', 
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
      });
    };

    // Create a new workbook and worksheet
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Box Logs');

    // Define column headers (similar structure to audit logs)
    const headers = [
      'Timestamp',
      'Action',
      'Description',
      'Changed By',
      'Changes'
    ];

    // Set column widths (matching audit logs pattern)
    worksheet.columns = [
      { width: 20 }, // Timestamp
      { width: 12 }, // Action
      { width: 50 }, // Description
      { width: 20 }, // Changed By
      { width: 60 }  // Changes
    ];

    // Add header row with styling (same as audit logs)
    const headerRow = worksheet.addRow(headers);
    headerRow.eachCell((cell) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FF4472C4' } // Blue background (same as audit logs)
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

    // Add data rows - use the same logs that are displayed in the UI (filtered logs)
    this.boxLogs.forEach((log, index) => {
      const changesText = formatChangesText(log);
      const actionLabel = DiffUtil.getActionLabel(log.action);

      const row = worksheet.addRow([
        formatDateForExcel(log.performedAt),
        actionLabel || log.action || '',
        log.description || '',
        log.performedByName || 'System',
        changesText
      ]);

      // Style data rows (same as audit logs)
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
        // Alternate row colors for better readability (same as audit logs)
        if (index % 2 === 0) {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFF9F9F9' } // Very light gray
          };
        }
      });
    });

    // Freeze header row (same as audit logs)
    worksheet.views = [
      {
        state: 'frozen',
        ySplit: 1 // Freeze first row
      }
    ];

    // Generate filename with current date (same pattern as audit logs)
    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    const boxCode = this.box?.code || 'Box';
    const fileName = `Box_Logs_${boxCode}_${dateStr}.xlsx`;

    // Export to Excel (same as audit logs)
    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    link.click();
    window.URL.revokeObjectURL(url);
  }

  async exportQualityIssuesToExcel(): Promise<void> {
    if (this.qualityIssues.length === 0) {
      alert('No quality issues to export. Please ensure there are quality issues available.');
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
      'Stage Number',
      'Box Tag',
      'Box Name',
      'Status',
      'Issue Type',
      'Severity',
      'Issue Description',
      'Assigned To',
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
      { width: 15 },  // WIR Number
      { width: 15 },  // Box Tag
      { width: 20 },  // Box Name
      { width: 12 },  // Status
      { width: 15 },  // Issue Type
      { width: 10 },  // Severity
      { width: 40 },  // Issue Description
      { width: 15 },  // Assigned To
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
        this.getQualityIssueWir(issue),
        issue.boxTag || '—',
        issue.boxName || '—',
        this.getQualityIssueStatusLabel(issue.status),
        issue.issueType || '—',
        issue.severity || '—',
        issue.issueDescription || '—',
        issue.assignedTeamName || '—',
        issue.reportedBy || '—',
        formatDateForExcel(issue.issueDate),
        formatDateForExcel(issue.dueDate),
        issue.resolutionDescription || '—',
        formatDateForExcel(issue.resolutionDate),
        issue.photoPath || '—'
      ]);

      // Style data rows
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
        // Alternate row colors
        if (index % 2 === 0) {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFF9F9F9' }
          };
        }
      });
    });

    // Freeze header row
    worksheet.views = [{ state: 'frozen', ySplit: 1 }];

    // Generate filename with current date
    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    const fileName = `Quality_Issues_${dateStr}.xlsx`;

    // Generate Excel file and download
    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    link.click();
    window.URL.revokeObjectURL(url);
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
      return this.getDisplayWirNumber(issue.wirNumber);
    }

    if (issue.wirId) {
      const matchedCheckpoint = this.wirCheckpoints.find(cp => cp.wirId === issue.wirId);
      if (matchedCheckpoint?.wirNumber) {
        return this.getDisplayWirNumber(matchedCheckpoint.wirNumber);
      }
    }

    return this.getDisplayWirText(issue.wirName) || '—';
  }

  getQualityIssueStatusLabel(status?: QualityIssueStatus | string): string {
    const normalized = (status || 'Open') as QualityIssueStatus;
    return this.qualityIssueStatusMeta[normalized]?.label || 'Open';
  }

  getQualityIssueStatusClass(status?: QualityIssueStatus | string): string {
    const normalized = (status || 'Open') as QualityIssueStatus;
    return this.qualityIssueStatusMeta[normalized]?.class || 'status-open';
  }

  formatDate(date?: string | Date): string {
    if (!date) {
      return '—';
    }
    const parsed = date instanceof Date ? date : new Date(date);
    if (isNaN(parsed.getTime())) {
      return '—';
    }
    return parsed.toISOString().split('T')[0];
  }

  openIssueDetails(issue: QualityIssueDetails): void {
    // Show existing data immediately
    this.selectedIssueDetails = issue;
    this.isDetailsModalOpen = true;

    // Refresh from backend to get latest details (including images)
    if (issue.issueId) {
      this.wirService.getQualityIssueById(issue.issueId).subscribe({
        next: (freshIssue) => {
          this.selectedIssueDetails = freshIssue;
        },
        error: (err) => {
          console.error('Failed to load quality issue details', err);
        }
      });
    }
  }

  closeIssueDetails(): void {
    this.isDetailsModalOpen = false;
    this.selectedIssueDetails = null;
    this.selectedCommentId = undefined; // Clear comment ID
  }

  /**
   * Open quality issue details modal from image (fetches issue by ID)
   */
  openQualityIssueFromImage(issueId: string): void {
    if (!issueId) {
      console.error('No issue ID provided');
      return;
    }

    // Fetch the full quality issue details
    this.wirService.getQualityIssueById(issueId).subscribe({
      next: (issue) => {
        this.openIssueDetails(issue);
      },
      error: (err) => {
        console.error('Failed to load quality issue details', err);
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: {
            message: 'Failed to load quality issue details',
            type: 'error'
          }
        }));
      }
    });
  }

  getQualityIssueImageUrls(issue: QualityIssueDetails | null): string[] {
    if (!issue) return [];
    
    // First, try to use the new Images array
    if (issue.images && Array.isArray(issue.images) && issue.images.length > 0) {
      return issue.images
        .sort((a, b) => (a.sequence || 0) - (b.sequence || 0))
        .map((img) => {
          // Use imageUrl if available (new API returns URL for on-demand loading)
          if (img.imageUrl) {
            // Convert relative URL to full API URL
            const baseUrl = this.getApiBaseUrl();
            return img.imageUrl.startsWith('/') ? `${baseUrl}${img.imageUrl}` : img.imageUrl;
          }
          
          // Fallback to imageData for backward compatibility
          const imageData = img.imageData || '';
          // If it's already a data URL, return as is
          if (imageData.startsWith('data:image/')) {
            return imageData;
          }
          // If it's a URL, return as is
          if (imageData.startsWith('http://') || imageData.startsWith('https://')) {
            return imageData;
          }
          // Otherwise, assume it's base64 and add data URI prefix
          if (imageData) {
            return `data:image/jpeg;base64,${imageData}`;
          }
          return '';
        })
        .filter((url: string) => url && url.trim().length > 0);
    }
    
    // Fallback to old PhotoPath field (backward compatibility)
    if (issue.photoPath) {
      return [issue.photoPath];
    }
    
    return [];
  }
  
  /**
   * Get the API base URL (without /api suffix, since imageUrl already includes /api)
   */
  private getApiBaseUrl(): string {
    // Use environment.apiUrl and remove /api suffix if present
    return environment.apiUrl.replace(/\/api\/?$/, '');
  }

  openImageInNewTab(imageUrl: string): void {
    if (!imageUrl) {
      console.error('No image URL provided');
      return;
    }

    // Ensure URL is absolute
    let absoluteUrl = imageUrl;
    
    // If it's a relative URL, make it absolute
    if (imageUrl.startsWith('/')) {
      const baseUrl = this.getApiBaseUrl();
      absoluteUrl = `${baseUrl}${imageUrl}`;
    }
    // If it's a data URL, we can't open it in a new tab - create a blob URL instead
    else if (imageUrl.startsWith('data:image/')) {
      // For data URLs, convert to blob URL
      fetch(imageUrl)
        .then(response => response.blob())
        .then(blob => {
          const blobUrl = URL.createObjectURL(blob);
          const newWindow = window.open(blobUrl, '_blank', 'noopener,noreferrer');
          if (!newWindow) {
            console.error('Failed to open image in new tab. Popup may be blocked.');
            this.downloadImage(imageUrl);
          }
        })
        .catch(error => {
          console.error('Error converting data URL to blob:', error);
          // Fallback: try to open data URL directly (may not work in all browsers)
          window.open(imageUrl, '_blank', 'noopener,noreferrer');
        });
      return;
    }
    // If it's already an absolute URL (http/https), use as is
    else if (!imageUrl.startsWith('http://') && !imageUrl.startsWith('https://')) {
      // Relative URL without leading slash - prepend base URL
      const baseUrl = this.getApiBaseUrl();
      absoluteUrl = `${baseUrl}/${imageUrl}`;
    }

    // Open in new tab
    console.log('Opening image URL:', absoluteUrl);
    const newWindow = window.open(absoluteUrl, '_blank', 'noopener,noreferrer');
    
    if (!newWindow) {
      console.error('Failed to open image in new tab. Popup may be blocked.');
      // Fallback: try to download instead
      this.downloadImage(imageUrl);
    }
  }

  downloadImage(imageUrl: string): void {
    try {
      const link = document.createElement('a');
      link.href = imageUrl;
      link.download = `quality-issue-image-${Date.now()}.jpg`;
      link.target = '_blank';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    } catch (error) {
      console.error('Error downloading image:', error);
      // Fallback: open in new tab
      this.openImageInNewTab(imageUrl);
    }
  }

  openAssignModal(issue: QualityIssueDetails): void {
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

  openCommentsModal(issue: QualityIssueDetails): void {
    // Close other modals first
    if (this.isDetailsModalOpen) {
      this.closeIssueDetails();
    }
    if (this.isStatusModalOpen) {
      this.closeStatusModal();
    }
    if (this.isAssignModalOpen) {
      this.closeAssignModal();
    }

    this.selectedIssueForComments = issue;
    this.isCommentsModalOpen = true;
  }

  closeCommentsModal(): void {
    this.isCommentsModalOpen = false;
    this.selectedIssueForComments = null;
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

  openStatusModal(issue: QualityIssueDetails): void {
    // Close other modals first
    if (this.isDetailsModalOpen) {
      this.closeIssueDetails();
    }
    if (this.isAssignModalOpen) {
      this.closeAssignModal();
    }

    this.selectedIssueForStatus = issue;
    this.statusUpdateForm = {
      status: issue.status || 'Open',
      resolutionDescription: issue.resolutionDescription || ''
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
    this.selectedImages = [];
    this.currentImageInputMode = 'url';
    this.currentUrlInput = '';
    this.showCamera = false;
    this.stopCamera();
    this.selectedIssueForStatus = null;
    this.statusUpdateLoading = false;
    this.statusUpdateError = '';
  }

  openProgressDetails(update: ProgressUpdate): void {
    // Navigate to activity details page instead of opening modal
    if (update.boxActivityId) {
      // Navigate to activity details with returnTo query param to indicate coming from progress history
      this.router.navigate(
        ['/projects', this.projectId, 'boxes', this.boxId, 'activities', update.boxActivityId],
        { queryParams: { returnTo: 'progress-history' } }
      );
    } else {
      console.error('Cannot navigate: boxActivityId is missing from progress update');
    }
  }

  closeProgressDetails(): void {
    this.isProgressModalOpen = false;
    this.selectedProgressUpdate = null;
  }

  getProgressStatusClass(status: string | any): string {
    if (!status) return 'status-unknown';
    
    const statusStr = typeof status === 'string' ? status : String(status);
    const normalized = statusStr.toLowerCase();
    
    switch (normalized) {
      case 'notstarted':
        return 'status-not-started';
      case 'inprogress':
        return 'status-in-progress';
      case 'completed':
        return 'status-completed';
      case 'onhold':
        return 'status-on-hold';
      case 'delayed':
        return 'status-delayed';
      default:
        return 'status-unknown';
    }
  }

  getProgressStatusLabel(status: string | any): string {
    if (!status) return '—';
    
    const statusStr = typeof status === 'string' ? status : String(status);
    const normalized = statusStr.toLowerCase();
    
    switch (normalized) {
      case 'notstarted':
        return 'Not Started';
      case 'inprogress':
        return 'In Progress';
      case 'completed':
        return 'Completed';
      case 'onhold':
        return 'On Hold';
      case 'delayed':
        return 'Delayed';
      default:
        return statusStr;
    }
  }

  getPhotoUrls(progressUpdate: any): string[] {
    // First, try to use the new Images array
    if (progressUpdate.images && Array.isArray(progressUpdate.images) && progressUpdate.images.length > 0) {
      return progressUpdate.images
        .sort((a: any, b: any) => (a.sequence || 0) - (b.sequence || 0))
        .map((img: any) => {
          // Try imageData first (full base64), then imageUrl (for on-demand loading)
          let url = img.imageData || img.ImageData || img.imageUrl || img.ImageUrl || '';
          
          console.log('📸 Progress Update Image:', {
            hasImageData: !!(img.imageData || img.ImageData),
            hasImageUrl: !!(img.imageUrl || img.ImageUrl),
            rawUrl: url,
            imageObject: img
          });
          
          // If it's a relative URL (starts with /api/ or just /), prepend the base URL
          if (url && (url.startsWith('/api/') || (url.startsWith('/') && !url.startsWith('http')))) {
            // Get base URL from current location
            const baseUrl = `${window.location.protocol}//${window.location.host}`;
            url = baseUrl + url;
            console.log('🔗 Converted relative URL to absolute:', url);
          }
          
          return url;
        })
        .filter((url: string) => url && url.trim().length > 0);
    }
    
    // Fallback to old Photo field (backward compatibility)
    const photoUrls = progressUpdate.photo || progressUpdate.photoUrls;
    if (!photoUrls) return [];
    
    // Check if it's a base64 string (starts with data:image or is a long base64 string)
    if (typeof photoUrls === 'string' && photoUrls.startsWith('data:image/')) {
      return [photoUrls];
    }
    
    // Check if it's a base64 string without data URI prefix
    const base64Pattern = /^[A-Za-z0-9+/=]+$/;
    if (typeof photoUrls === 'string' && photoUrls.length > 100 && base64Pattern.test(photoUrls)) {
      return [`data:image/jpeg;base64,${photoUrls}`];
    }
    
    try {
      const parsed = JSON.parse(photoUrls);
      if (Array.isArray(parsed)) {
        return parsed.map((url: any) => {
          if (typeof url === 'string') {
            if (url.startsWith('data:image/')) {
              return url;
            }
            const base64Pattern = /^[A-Za-z0-9+/=]+$/;
            if (url.length > 100 && base64Pattern.test(url)) {
              return `data:image/jpeg;base64,${url}`;
            }
            return url;
          }
          return '';
        }).filter((url: string) => url && url.trim().length > 0);
      }
    } catch {
      // If not JSON, try splitting by comma
      if (typeof photoUrls === 'string') {
        return photoUrls.split(',').map((url: string) => {
          const trimmed = url.trim();
          if (!trimmed) return '';
          if (trimmed.startsWith('data:image/')) {
            return trimmed;
          }
          const base64Pattern = /^[A-Za-z0-9+/=]+$/;
          if (trimmed.length > 100 && base64Pattern.test(trimmed)) {
            return `data:image/jpeg;base64,${trimmed}`;
          }
          return trimmed;
        }).filter((url: string) => url.length > 0);
      }
    }
    
    return [];
  }

  private buildAbsoluteImageUrl(url: string): string {
    if (!url) return '';
    if (url.startsWith('http://') || url.startsWith('https://')) {
      return url;
    }
    if (url.startsWith('/')) {
      const baseUrl = `${window.location.protocol}//${window.location.host}`;
      return baseUrl + url;
    }
    return url;
  }

  private isDataUri(url: string): boolean {
    return /^data:image\//i.test(url);
  }

  private isBareBase64Image(url: string): boolean {
    // Heuristic: long base64 string without scheme, may contain +/=
    return !!url && !url.startsWith('http') && !url.startsWith('/') && url.length > 100 && /^[A-Za-z0-9+/=]+$/i.test(url);
  }

  private createObjectUrlFromBase64(data: string): string {
    if (!data) return '';

    let base64Part = data;
    let contentType = 'image/jpeg';

    const dataUriMatch = data.match(/^data:(image\/[a-zA-Z0-9.+-]+);base64,(.+)$/i);
    if (dataUriMatch) {
      contentType = dataUriMatch[1];
      base64Part = dataUriMatch[2];
    }

    try {
      const byteCharacters = atob(base64Part);
      const byteNumbers = new Array(byteCharacters.length);
      for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      const blob = new Blob([byteArray], { type: contentType });
      return URL.createObjectURL(blob);
    } catch (err) {
      console.error('❌ Failed to convert base64 to blob URL', err);
      // Fallback to data URI
      if (this.isDataUri(data)) {
        return data;
      }
      return `data:${contentType};base64,${base64Part}`;
    }
  }

  private fetchImageAsObjectUrl(url: string, imageData?: string): Observable<string> {
    // Prefer base64 when available
    const base64Source = imageData || url;
    if (base64Source && (this.isDataUri(base64Source) || this.isBareBase64Image(base64Source))) {
      const display = this.createObjectUrlFromBase64(base64Source);
      return of(display);
    }

    const absoluteUrl = this.buildAbsoluteImageUrl(url);
    return this.http.get(absoluteUrl, { responseType: 'blob' }).pipe(
      map(blob => URL.createObjectURL(blob))
    );
  }

  private resolveProgressUpdateImages(): void {
    const rawImages = this.getAllProgressUpdateImages();
    if (rawImages.length === 0) {
      this.resolvedProgressImages = [];
      return;
    }

    this.resolvingProgressImages = true;

    const requests = rawImages.map(img =>
      this.fetchImageAsObjectUrl(img.imageUrl, (img as any).imageData).pipe(
        map(displayUrl => ({
          ...img,
          displayUrl
        })),
        catchError(err => {
          console.error('❌ Failed to fetch image with auth header, falling back to raw URL:', img.imageUrl, err);
          return of({
            ...img,
            displayUrl: this.isBareBase64Image((img as any).imageData) || this.isDataUri((img as any).imageData)
              ? this.createObjectUrlFromBase64((img as any).imageData)
              : this.buildAbsoluteImageUrl(img.imageUrl)
          });
        })
      )
    );

    forkJoin(requests).subscribe({
      next: (resolved) => {
        this.resolvedProgressImages = resolved;
        this.resolvingProgressImages = false;
      },
      error: (err) => {
        console.error('❌ Failed to resolve progress update images:', err);
        this.resolvedProgressImages = [];
        this.resolvingProgressImages = false;
      }
    });
  }

  openPhotoInNewTab(photoUrl: string): void {
    if (!photoUrl) {
      console.error('No photo URL provided');
      return;
    }

    // Ensure URL is absolute
    let absoluteUrl = photoUrl;
    
    // If it's a relative URL, make it absolute
    if (photoUrl.startsWith('/')) {
      const baseUrl = this.getApiBaseUrl();
      absoluteUrl = `${baseUrl}${photoUrl}`;
    }
    // For base64/data images, convert to blob URL
    else if (photoUrl.startsWith('data:image/')) {
      const newWindow = window.open();
      if (newWindow) {
        newWindow.document.write(`<img src="${photoUrl}" style="max-width: 100%; height: auto;" />`);
      }
    } else {
      window.open(photoUrl, '_blank', 'noopener,noreferrer');
    }
    // If it's already an absolute URL (http/https), use as is
     if (!photoUrl.startsWith('http://') && !photoUrl.startsWith('https://')) {
      // Relative URL without leading slash - prepend base URL
      const baseUrl = this.getApiBaseUrl();
      absoluteUrl = `${baseUrl}/${photoUrl}`;
    }

    console.log('🖼️ Opening photo URL:', absoluteUrl);
    const newWindow = window.open(absoluteUrl, '_blank', 'noopener,noreferrer');
    if (!newWindow) {
      console.error('Failed to open photo in new tab. Popup may be blocked.');
    }
  }

  downloadPhoto(photoUrl: string): void {
    const link = document.createElement('a');
    
    if (photoUrl.startsWith('data:image/')) {
      // Handle base64 image
      link.href = photoUrl;
      // Extract format from data URI or default to jpg
      const formatMatch = photoUrl.match(/data:image\/([^;]+)/);
      const format = formatMatch ? formatMatch[1] : 'jpg';
      link.download = `progress-photo-${Date.now()}.${format}`;
    } else {
      // Handle regular URL
      link.href = photoUrl;
      link.download = photoUrl.split('/').pop() || 'photo.jpg';
    }
    
    link.target = '_blank';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  onImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.src = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjAwIiBoZWlnaHQ9IjIwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iMjAwIiBoZWlnaHQ9IjIwMCIgZmlsbD0iI2YxZjVmOSIvPjx0ZXh0IHg9IjUwJSIgeT0iNTAlIiBmb250LWZhbWlseT0iQXJpYWwiIGZvbnQtc2l6ZT0iMTQiIGZpbGw9IiM5NDk4YjgiIHRleHQtYW5jaG9yPSJtaWRkbGUiIGR5PSIuM2VtIj5JbWFnZSBub3QgYXZhaWxhYmxlPC90ZXh0Pjwvc3ZnPg==';
    img.style.objectFit = 'contain';
    img.style.padding = '20px';
  }

  // Load all progress updates to extract images for drawings section
  loadAllProgressUpdatesForImages(): void {
    if (!this.boxId || this.loadingProgressUpdateImages) {
      return;
    }

    console.log('📋 Loading progress updates for drawings tab...');
    this.loadingProgressUpdateImages = true;
    // Load with a large page size to get all progress updates
    this.progressUpdateService.getProgressUpdatesByBox(this.boxId, 1, 1000, {}).subscribe({
      next: (response) => {
        console.log('✅ Progress updates loaded:', response.items?.length || 0, 'updates');
        this.allProgressUpdatesForImages = (response.items || [])
          .map(update => ({
            ...update,
            updateDate: update.updateDate ? new Date(update.updateDate) : undefined
          }));
        
        // Log image counts
        const totalImages = this.getAllProgressUpdateImages().length;
        console.log('🖼️ Total images found:', totalImages);
        // Resolve images with authenticated requests to avoid 401/Mixed Content
        this.resolveProgressUpdateImages();
        
        this.loadingProgressUpdateImages = false;
      },
      error: (err) => {
        console.error('❌ Error loading progress updates for images:', err);
        this.loadingProgressUpdateImages = false;
      }
    });
  }

  // Get all images from all progress updates
  getAllProgressUpdateImages(): Array<{ imageUrl: string; updateDate?: Date; activityName?: string; progressPercentage?: number; imageType?: 'file' | 'url' }> {
    const allImages: Array<{ imageUrl: string; updateDate?: Date; activityName?: string; progressPercentage?: number; imageData?: string; imageType?: 'file' | 'url' }> = [];
    
    console.log('🔍 Processing progress updates for images:', this.allProgressUpdatesForImages.length);
    
    this.allProgressUpdatesForImages.forEach((update, index) => {
      console.log(`Update #${index + 1} (${update.activityName}):`, {
        hasImages: !!update.images,
        imagesCount: update.images?.length || 0,
      });
      
      // Use the images array which contains imageType info
      if (update.images && Array.isArray(update.images) && update.images.length > 0) {
        update.images.forEach((img: any) => {
          let imageUrl = img.imageData || img.ImageData || img.imageUrl || img.ImageUrl || '';
          
          // Convert relative URLs to absolute
          if (imageUrl && (imageUrl.startsWith('/api/') || (imageUrl.startsWith('/') && !imageUrl.startsWith('http')))) {
            const baseUrl = `${window.location.protocol}//${window.location.host}`;
            imageUrl = baseUrl + imageUrl;
          }
          
          if (imageUrl && imageUrl.trim()) {
            allImages.push({
              imageUrl,
              updateDate: update.updateDate,
              activityName: update.activityName,
              progressPercentage: update.progressPercentage,
              imageData: img.imageData || img.ImageData,
              imageType: img.imageType || 'file' // Default to 'file' if not specified
            });
          }
        });
      } else {
        // Fallback for old format without images array
        const photoUrls = this.getPhotoUrls(update);
        photoUrls.forEach(imageUrl => {
          if (imageUrl && imageUrl.trim()) {
            allImages.push({
              imageUrl,
              updateDate: update.updateDate,
              activityName: update.activityName,
              progressPercentage: update.progressPercentage,
              imageData: undefined,
              imageType: 'file' // Default to 'file' for legacy images
            });
          }
        });
      }
    });
    
    console.log('✅ Total images to display:', allImages.length);
    console.log('📸 Image types distribution:', {
      file: allImages.filter(img => img.imageType === 'file').length,
      url: allImages.filter(img => img.imageType === 'url').length
    });
    
    return allImages;
  }

  // Check if there are any images from progress updates
  hasProgressUpdateImages(): boolean {
    return this.resolvedProgressImages.length > 0;
  }

  // Set active drawing sub-tab
  setActiveDrawingTab(tab: 'file' | 'url'): void {
    this.activeDrawingTab = tab;
  }

  // Get file-type drawings (PDF/DWG files)
  getFileTypeImages(): BoxDrawing[] {
    return this.boxDrawings.filter(drawing => drawing.imageType === 'file');
  }

  // Get URL-type drawings (only actual image URLs, not PDF/DWG files)
  getUrlTypeImages(): BoxDrawing[] {
    return this.boxDrawings.filter(drawing => {
      // Only show URLs that are not PDF/DWG files
      if (drawing.imageType === 'url') {
        const url = drawing.displayUrl || drawing.imageUrl || '';
        const isFileUrl = /\.(pdf|dwg)(\?|$|#)/i.test(url);
        return !isFileUrl; // Only return if it's NOT a file URL
      }
      return false;
    });
  }

  // Group file drawings by filename with version history
  getGroupedFileTypeImages(): { fileName: string; versions: BoxDrawing[] }[] {
    const fileDrawings = this.getFileTypeImages();
    const grouped = new Map<string, BoxDrawing[]>();

    // Group by filename
    fileDrawings.forEach(drawing => {
      const fileName = drawing.fileName || drawing.originalFileName || 'Untitled';
      if (!grouped.has(fileName)) {
        grouped.set(fileName, []);
      }
      grouped.get(fileName)!.push(drawing);
    });

    // Convert to array and sort versions (newest first: V5, V4, V3, V2, V1)
    const result = Array.from(grouped.entries())
      .map(([fileName, versions]) => {
        const sorted = versions.sort((a, b) => (b.version || 1) - (a.version || 1));
        console.log(`📊 VERSION DEBUG - File "${fileName}":`, sorted.map(v => ({ version: v.version, date: v.createdDate })));
        return {
          fileName,
          versions: sorted
        };
      })
      .sort((a, b) => {
        // Sort groups by the most recent upload date
        const aLatestDate = Math.max(...a.versions.map(v => new Date(v.updateDate || v.createdDate || 0).getTime()));
        const bLatestDate = Math.max(...b.versions.map(v => new Date(v.updateDate || v.createdDate || 0).getTime()));
        return bLatestDate - aLatestDate; // Newest first
      });
    
    return result;
  }

  // Group URL drawings by URL with version history
  getGroupedUrlTypeImages(): { fileName: string; versions: BoxDrawing[] }[] {
    const urlDrawings = this.getUrlTypeImages();
    const grouped = new Map<string, BoxDrawing[]>();

    // Group by URL
    urlDrawings.forEach(drawing => {
      const url = drawing.drawingUrl || drawing.displayUrl || drawing.imageUrl || 'Untitled';
      if (!grouped.has(url)) {
        grouped.set(url, []);
      }
      grouped.get(url)!.push(drawing);
    });

    // Convert to array and sort versions (newest first)
    return Array.from(grouped.entries())
      .map(([fileName, versions]) => ({
        fileName,
        versions: versions.sort((a, b) => (b.version || 1) - (a.version || 1))
      }))
      .sort((a, b) => {
        const aLatestDate = Math.max(...a.versions.map(v => new Date(v.updateDate || v.createdDate || 0).getTime()));
        const bLatestDate = Math.max(...b.versions.map(v => new Date(v.updateDate || v.createdDate || 0).getTime()));
        return bLatestDate - aLatestDate;
      });
  }

  // Group WIR checkpoint images by filename with version history
  getGroupedWirImages(): { fileName: string; versions: any[] }[] {
    if (!this.boxAttachments.wirCheckpointImages || this.boxAttachments.wirCheckpointImages.length === 0) {
      return [];
    }

    const grouped = new Map<string, any[]>();

    // Group by filename
    this.boxAttachments.wirCheckpointImages.forEach((image: any) => {
      const fileName = image.originalName || 'Stage_Image';
      if (!grouped.has(fileName)) {
        grouped.set(fileName, []);
      }
      grouped.get(fileName)!.push(image);
    });

    // Convert to array and sort versions (newest first)
    return Array.from(grouped.entries())
      .map(([fileName, versions]) => ({
        fileName,
        versions: versions.sort((a, b) => (b.version || 1) - (a.version || 1))
      }))
      .sort((a, b) => {
        const aLatestDate = Math.max(...a.versions.map(v => new Date(v.createdDate || 0).getTime()));
        const bLatestDate = Math.max(...b.versions.map(v => new Date(v.createdDate || 0).getTime()));
        return bLatestDate - aLatestDate;
      });
  }

  // Group progress update images by filename with version history
  getGroupedProgressImages(): { fileName: string; versions: any[] }[] {
    if (!this.boxAttachments.progressUpdateImages || this.boxAttachments.progressUpdateImages.length === 0) {
      return [];
    }

    const grouped = new Map<string, any[]>();

    // Group by filename
    this.boxAttachments.progressUpdateImages.forEach((image: any) => {
      const fileName = image.originalName || 'Progress_Image';
      if (!grouped.has(fileName)) {
        grouped.set(fileName, []);
      }
      grouped.get(fileName)!.push(image);
    });

    // Convert to array and sort versions (newest first)
    return Array.from(grouped.entries())
      .map(([fileName, versions]) => ({
        fileName,
        versions: versions.sort((a, b) => (b.version || 1) - (a.version || 1))
      }))
      .sort((a, b) => {
        const aLatestDate = Math.max(...a.versions.map(v => new Date(v.createdDate || 0).getTime()));
        const bLatestDate = Math.max(...b.versions.map(v => new Date(v.createdDate || 0).getTime()));
        return bLatestDate - aLatestDate;
      });
  }

  // Group quality issue images by filename with version history
  getGroupedQualityImages(): { fileName: string; versions: any[] }[] {
    if (!this.boxAttachments.qualityIssueImages || this.boxAttachments.qualityIssueImages.length === 0) {
      return [];
    }

    const grouped = new Map<string, any[]>();

    // Group by filename
    this.boxAttachments.qualityIssueImages.forEach((image: any) => {
      const fileName = image.originalName || 'Quality_Image';
      if (!grouped.has(fileName)) {
        grouped.set(fileName, []);
      }
      grouped.get(fileName)!.push(image);
    });

    // Convert to array and sort versions (newest first)
    return Array.from(grouped.entries())
      .map(([fileName, versions]) => ({
        fileName,
        versions: versions.sort((a, b) => (b.version || 1) - (a.version || 1))
      }))
      .sort((a, b) => {
        const aLatestDate = Math.max(...a.versions.map(v => new Date(v.createdDate || 0).getTime()));
        const bLatestDate = Math.max(...b.versions.map(v => new Date(v.createdDate || 0).getTime()));
        return bLatestDate - aLatestDate;
      });
  }

downloadFileDrawing(drawing: any): void {
  if (!drawing.displayUrl || !drawing.fileName) {
    console.error('Cannot download: missing file data or filename');
    return;
  }

  // If the URL is a data URL (base64)
  if (drawing.displayUrl.startsWith('data:')) {
    const link = document.createElement('a');
    link.href = drawing.displayUrl;
    link.download = drawing.fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  } else {
    // For SAS URLs (Azure Blob Storage), use native fetch to avoid auth headers
    // SAS URLs have their own authentication and don't need additional headers
    const isSasUrl = drawing.displayUrl.includes('?sv=') || drawing.displayUrl.includes('blob.core.windows.net');
    
    if (isSasUrl) {
      // Use native fetch for SAS URLs (no auth headers)
      fetch(drawing.displayUrl)
        .then(response => {
          if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
          }
          return response.blob();
        })
        .then(blob => {
          const blobUrl = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = blobUrl;
          link.download = drawing.fileName;
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
          
          // Revoke blob URL after download
          setTimeout(() => window.URL.revokeObjectURL(blobUrl), 1000);
        })
        .catch(err => {
          console.error('Error downloading file from SAS URL:', err);
          document.dispatchEvent(
            new CustomEvent('app-toast', {
              detail: { message: 'Failed to download file', type: 'error' }
            })
          );
        });
    } else {
      // For API URLs, use HttpClient (with auth headers)
      this.http.get(drawing.displayUrl, { responseType: 'blob' }).subscribe({
        next: (blob) => {
          if (!blob) {
            console.error('No file data received');
            return;
          }

          // Create blob URL for download
          const blobUrl = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = blobUrl;
          link.download = drawing.fileName;
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);

          // Revoke blob URL after a short delay
          setTimeout(() => window.URL.revokeObjectURL(blobUrl), 1000);
        },
        error: (err) => {
          console.error('Error downloading file from API:', err);
          document.dispatchEvent(
            new CustomEvent('app-toast', {
              detail: { message: 'Failed to download file', type: 'error' }
            })
          );
        }
      });
    }
  }
}

  // Open file drawing (for PDFs, open in new tab; for DWG, download)
  openFileDrawing(drawing: any): void {
    if (!drawing.displayUrl) {
      console.error('Cannot open: missing file data');
      return;
    }

    const extension = drawing.fileExtension?.toLowerCase() || '';
    if (extension === '.pdf') {
      // For PDFs, download and open in new tab using blob URL
      if (drawing.displayUrl.startsWith('data:')) {
        // For data URLs, open directly
        window.open(drawing.displayUrl, '_blank');
      } else {
        // Check if it's a SAS URL (Azure Blob Storage)
        const isSasUrl = drawing.displayUrl.includes('?sv=') || drawing.displayUrl.includes('blob.core.windows.net');
        
        if (isSasUrl) {
          // For SAS URLs, use native fetch (no auth headers)
          fetch(drawing.displayUrl)
            .then(response => {
              if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
              }
              return response.blob();
            })
            .then(blob => {
              const blobUrl = window.URL.createObjectURL(blob);
              window.open(blobUrl, '_blank');
              // Clean up after a delay
              setTimeout(() => window.URL.revokeObjectURL(blobUrl), 60000);
            })
            .catch(err => {
              console.error('Error opening PDF from SAS URL:', err);
              document.dispatchEvent(new CustomEvent('app-toast', {
                detail: { message: 'Failed to open PDF', type: 'error' }
              }));
            });
        } else {
          // For API URLs, use HttpClient (with auth headers)
          this.http.get(drawing.displayUrl, { responseType: 'blob' }).subscribe({
            next: (blob) => {
              const blobUrl = window.URL.createObjectURL(blob);
              window.open(blobUrl, '_blank');
              // Clean up after a delay
              setTimeout(() => window.URL.revokeObjectURL(blobUrl), 60000);
            },
            error: (err) => {
              console.error('Error opening PDF from API:', err);
              document.dispatchEvent(new CustomEvent('app-toast', {
                detail: { message: 'Failed to open PDF', type: 'error' }
              }));
            }
          });
        }
      }
    } else {
      // For DWG and other files, download
      this.downloadFileDrawing(drawing);
    }
  }

  // Format file size
  formatFileSize(bytes?: number): string {
    if (!bytes) return 'Unknown size';
    if (bytes < 1024) return bytes + ' B';
    if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB';
    return (bytes / (1024 * 1024)).toFixed(1) + ' MB';
  }

  // Load user names for drawings
  private loadUserNamesForDrawings(drawings: Array<{ createdBy?: string }>): void {
    // Get unique user IDs
    const userIds = new Set<string>();
    drawings.forEach(drawing => {
      if (drawing.createdBy && !this.userNamesCache.has(drawing.createdBy)) {
        userIds.add(drawing.createdBy);
      }
    });

    if (userIds.size === 0) {
      // All user names are already cached or no users to load
      this.loadingBoxDrawings = false;
      return;
    }

    // Fetch user names for all unique user IDs
    const userObservables = Array.from(userIds).map(userId => 
      this.userService.getUserById(userId).pipe(
        map((user: any) => ({ userId, userName: user.fullName || user.email || 'Unknown User' })),
        catchError(() => of({ userId, userName: 'Unknown User' }))
      )
    );

    forkJoin(userObservables).subscribe({
      next: (results) => {
        // Cache user names
        results.forEach(result => {
          this.userNamesCache.set(result.userId, result.userName);
        });

        // Update drawings with user names
        this.boxDrawings = this.boxDrawings.map(drawing => ({
          ...drawing,
          createdByName: drawing.createdBy ? this.userNamesCache.get(drawing.createdBy) : undefined
        }));

        this.loadingBoxDrawings = false;
        console.log('✅ User names loaded for drawings');
      },
      error: (err) => {
        console.error('❌ Error loading user names:', err);
        // Continue even if user names fail to load
        this.loadingBoxDrawings = false;
      }
    });
  }

  // Load user names for attachments
  private loadUserNamesForAttachments(attachments: any): void {
    // Collect all unique user IDs from all attachment types
    const userIds = new Set<string>();
    
    // WIR Checkpoint Images
    if (attachments.wirCheckpointImages) {
      attachments.wirCheckpointImages.forEach((img: any) => {
        if (img.createdBy && !this.userNamesCache.has(img.createdBy)) {
          userIds.add(img.createdBy);
        }
      });
    }
    
    // Progress Update Images
    if (attachments.progressUpdateImages) {
      attachments.progressUpdateImages.forEach((img: any) => {
        if (img.createdBy && !this.userNamesCache.has(img.createdBy)) {
          userIds.add(img.createdBy);
        }
      });
    }
    
    // Quality Issue Images
    if (attachments.qualityIssueImages) {
      attachments.qualityIssueImages.forEach((img: any) => {
        if (img.createdBy && !this.userNamesCache.has(img.createdBy)) {
          userIds.add(img.createdBy);
        }
      });
    }

    if (userIds.size === 0) {
      // All user names are already cached or no users to load
      this.updateAttachmentsWithUserNames(attachments);
      this.loadingBoxAttachments = false;
      return;
    }

    // Fetch user names for all unique user IDs in parallel
    const userObservables = Array.from(userIds).map(userId => 
      this.userService.getUserById(userId).pipe(
        map((user: any) => ({ userId, userName: user.fullName || user.email || 'Unknown User' })),
        catchError(() => of({ userId, userName: 'Unknown User' }))
      )
    );

    forkJoin(userObservables).subscribe({
      next: (results) => {
        // Cache user names
        results.forEach(result => {
          this.userNamesCache.set(result.userId, result.userName);
        });

        // Update attachments with user names
        this.updateAttachmentsWithUserNames(attachments);
        this.loadingBoxAttachments = false;
        console.log('✅ User names loaded for attachments');
      },
      error: (err) => {
        console.error('❌ Error loading user names for attachments:', err);
        // Continue even if user names fail to load
        this.updateAttachmentsWithUserNames(attachments);
        this.loadingBoxAttachments = false;
      }
    });
  }

  // Update attachments with user names
  private updateAttachmentsWithUserNames(attachments: any): void {
    // Update WIR Checkpoint Images
    if (attachments.wirCheckpointImages) {
      attachments.wirCheckpointImages = attachments.wirCheckpointImages.map((img: any) => ({
        ...img,
        createdByName: img.createdBy ? this.userNamesCache.get(img.createdBy) : undefined
      }));
    }
    
    // Update Progress Update Images
    if (attachments.progressUpdateImages) {
      attachments.progressUpdateImages = attachments.progressUpdateImages.map((img: any) => ({
        ...img,
        createdByName: img.createdBy ? this.userNamesCache.get(img.createdBy) : undefined
      }));
    }
    
    // Update Quality Issue Images
    if (attachments.qualityIssueImages) {
      attachments.qualityIssueImages = attachments.qualityIssueImages.map((img: any) => ({
        ...img,
        createdByName: img.createdBy ? this.userNamesCache.get(img.createdBy) : undefined
      }));
    }
    
    this.boxAttachments = attachments;
  }

  // Open image - for URL-type images, open original URL, otherwise open the display URL
  openImage(image: { imageUrl: string; displayUrl: string; imageType: 'file' | 'url'; originalUrl?: string }): void {
    if (image.imageType === 'url') {
      // For URL-type, open the URL (originalUrl or displayUrl)
      const urlToOpen = image.originalUrl || image.displayUrl || image.imageUrl;
      console.log('🔗 Opening URL:', urlToOpen);
      if (urlToOpen) {
        window.open(urlToOpen, '_blank');
      }
    } else {
      // For file-type images, open the display URL
      console.log('🖼️ Opening image URL:', image.displayUrl || image.imageUrl);
      this.openPhotoInNewTab(image.displayUrl || image.imageUrl);
    }
  }

  // Open URL directly (for URL links)
  openUrl(url: string): void {
    if (url) {
      window.open(url, '_blank');
    }
  }

  // Check if URL is an image URL
  isImageUrl(url: string): boolean {
    if (!url) return false;
    const imageExtensions = /\.(jpg|jpeg|png|gif|bmp|webp|svg)(\?|$|#)/i;
    return imageExtensions.test(url);
  }

  // Load box drawings from dedicated BoxDrawings endpoint
  loadBoxDrawings(): void {
    if (!this.boxId) {
      return;
    }

    this.loadingBoxDrawings = true;
    this.boxDrawingsError = '';

    console.log('📦 Loading box drawings for box:', this.boxId);

    this.boxService.getBoxDrawingsFromEndpoint(this.boxId).subscribe({
      next: (drawings) => {
        console.log('✅ Box drawings loaded from endpoint:', drawings);
        console.log('📊 VERSION DEBUG - Drawings with versions:', drawings.map(d => ({
          fileName: d.originalFileName || d.drawingUrl,
          version: d.version,
          createdDate: d.createdDate
        })));
        
        // Process drawings and resolve URLs/data
        type ResolvedDrawing = { 
          imageUrl: string; 
          displayUrl: string; 
          updateDate?: Date; 
          activityName?: string; 
          progressPercentage?: number; 
          imageType: 'file' | 'url';
          originalUrl?: string;
          fileName?: string;
          originalFileName?: string; // Original filename
          fileExtension?: string;
          fileSize?: number;
          fileData?: string;
          boxDrawingId?: string;
          createdBy?: string;
          createdByName?: string;
          version?: number; // Version number for files with same name
          createdDate?: Date; // Creation date
          drawingUrl?: string; // URL for drawing
          downloadUrl?: string; // Blob storage download URL
        };

        const processedDrawings = drawings.map((drawing, index) => {
          console.log(`📄 Processing drawing ${index + 1}:`, drawing);
          
          let fileType = (drawing.fileType || 'file').toLowerCase() as 'file' | 'url';
          let imageUrl = '';
          let displayUrl = '';
          let fileName = drawing.originalFileName;
          let fileExtension = drawing.fileExtension;
          
          // Priority 1: Use downloadUrl from blob storage if available
          if (drawing.downloadUrl) {
            fileType = 'file';
            fileName = drawing.originalFileName || drawing.drawingFileName || 'Drawing';
            fileExtension = drawing.fileExtension;
            imageUrl = drawing.downloadUrl;
            displayUrl = drawing.downloadUrl;
            console.log('✅ Using blob storage download URL:', displayUrl);
          }
          // Priority 2: Check if URL points to a PDF/DWG file (not an image)
          else if (fileType === 'url' && drawing.drawingUrl) {
            // Check if backend provided fileExtension (for URLs pointing to files)
            const isFileExtension = drawing.fileExtension?.toLowerCase() === '.pdf' || 
                                   drawing.fileExtension?.toLowerCase() === '.dwg';
            
            // Or extract file extension from URL
            const urlExtension = drawing.drawingUrl.toLowerCase().match(/\.(pdf|dwg)(\?|$|#)/)?.[1];
            
            if (isFileExtension || urlExtension) {
              // URL points to a PDF/DWG file, treat it as a file type
              fileType = 'file';
              fileExtension = drawing.fileExtension || `.${urlExtension}`;
              // Extract filename from URL if not provided
              if (!fileName) {
                const urlParts = drawing.drawingUrl.split('/');
                fileName = urlParts[urlParts.length - 1].split('?')[0].split('#')[0];
              }
            }
            
            imageUrl = drawing.drawingUrl;
            displayUrl = drawing.drawingUrl;
          } else if (fileType === 'file') {
            // Legacy: For file-type drawings without downloadUrl, use boxDrawingId to construct download URL
            displayUrl = this.boxService.getBoxDrawingDownloadUrl(drawing.boxDrawingId);
            imageUrl = displayUrl; // For PDF/DWG, this will be used for download
            console.log('⚠️ Using legacy download URL (should use blob storage):', displayUrl);
          }
          
          return {
            imageUrl: imageUrl,
            displayUrl: displayUrl,
            updateDate: drawing.createdDate,
            activityName: undefined, // Box drawings don't have activity names
            progressPercentage: undefined, // Box drawings don't have progress
            imageType: fileType, // This determines which section to show in
            originalUrl: drawing.drawingUrl || undefined,
            fileName: fileName,
            originalFileName: drawing.originalFileName,
            fileExtension: fileExtension,
            fileSize: drawing.fileSize,
            fileData: drawing.fileData,
            boxDrawingId: drawing.boxDrawingId,
            createdBy: drawing.createdBy,
            createdByName: drawing.createdByName, // Include user name from backend
            version: drawing.version, // Include version number from backend
            createdDate: drawing.createdDate,
            drawingUrl: drawing.drawingUrl,
            downloadUrl: drawing.downloadUrl // Store the blob storage download URL
          } as ResolvedDrawing;
        });

        this.boxDrawings = processedDrawings;
        
        // Load user names for all drawings
        this.loadUserNamesForDrawings(processedDrawings);
      },
      error: (err) => {
        console.error('❌ Error loading box drawings:', err);
        console.error('❌ Error details:', {
          status: err.status,
          statusText: err.statusText,
          error: err.error,
          message: err.message,
          url: err.url
        });
        this.boxDrawingsError = err.error?.message || err.message || 'Failed to load drawings';
        this.loadingBoxDrawings = false;
      }
    });
  }

  // Upload Drawing Modal Methods
  openUploadDrawingModal(): void {
    // Check if box status allows actions
    if (this.box && !canPerformBoxActions(this.box.status)) {
      const statusMessage = this.box.status === 'Dispatched' 
        ? 'Cannot upload drawings. The box is dispatched and no actions are allowed.'
        : 'Cannot upload drawings. The box is on hold and no actions are allowed. Only box status changes are allowed.';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: statusMessage,
          type: 'error' 
        }
      }));
      return;
    }
    if (this.isProjectOnHold || this.isProjectArchived || this.isProjectClosed) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: this.isProjectClosed 
            ? 'Cannot upload drawings. This project is closed. Only project status changes are allowed.'
            : this.isProjectOnHold 
            ? 'Cannot upload drawings. This project is on hold. Only project status changes are allowed.' 
            : 'Cannot upload drawings. This project is archived and read-only.',
          type: 'error' 
        }
      }));
      return;
    }
    this.isUploadDrawingModalOpen = true;
  }

  closeUploadDrawingModal(): void {
    this.isUploadDrawingModalOpen = false;
  }

  onDrawingUploaded(): void {
    // Reload drawings after successful upload
    this.loadBoxDrawings();
    this.closeUploadDrawingModal();
    
    // Show success toast
    document.dispatchEvent(new CustomEvent('app-toast', {
      detail: { message: 'Drawing uploaded successfully!', type: 'success' }
    }));
  }


  loadAllBoxAttachments(): void {
    if (!this.boxId) {
      return;
    }

    this.loadingBoxAttachments = true;
    this.boxAttachmentsError = '';

    console.log('📎 Loading all box attachments for box:', this.boxId);

    this.boxService.getAllBoxAttachments(this.boxId).subscribe({
      next: (response) => {
        console.log('✅ Box attachments loaded:', response);
        
        // Debug: Log versions for each attachment type
        console.log('📊 VERSION DEBUG - WIR Images:', response.wirCheckpointImages?.map((img: any) => ({
          name: img.originalName,
          version: img.version
        })));
        console.log('📊 VERSION DEBUG - Progress Images:', response.progressUpdateImages?.map((img: any) => ({
          name: img.originalName,
          version: img.version
        })));
        console.log('📊 VERSION DEBUG - Quality Images:', response.qualityIssueImages?.map((img: any) => ({
          name: img.originalName,
          version: img.version,
          hasImageData: !!img.imageData,
          hasImageUrl: !!img.imageUrl,
          imageUrl: img.imageUrl,
          imageType: img.imageType
        })));
        
        this.boxAttachments = response;
        
        // Load user names for all attachments
        this.loadUserNamesForAttachments(response);
      },
      error: (err) => {
        console.error('❌ Error loading box attachments:', err);
        this.boxAttachmentsError = err.error?.message || err.message || 'Failed to load attachments';
        this.loadingBoxAttachments = false;
      }
    });
  }

  /**
   * Format image data to ensure it has proper data URL format
   * Now handles both ImageUrl (blob storage) and ImageData (base64) for backward compatibility
   */
  handleImageError(event: Event, image: any): void {
    const imgElement = event.target as HTMLImageElement;
    console.error('❌ Image failed to load:', {
      originalName: image.originalName,
      imageUrl: image.imageUrl,
      imageFileName: image.imageFileName,
      imageType: image.imageType,
      hasImageData: !!image.imageData,
      attemptedSrc: imgElement.src
    });
    
    // Set a placeholder image or error state
    imgElement.style.display = 'none';
    const wrapper = imgElement.parentElement;
    if (wrapper) {
      wrapper.classList.add('image-load-error');
      const errorMsg = wrapper.querySelector('.error-message');
      if (!errorMsg) {
        const errorDiv = document.createElement('div');
        errorDiv.className = 'error-message';
        errorDiv.textContent = 'Image failed to load';
        wrapper.appendChild(errorDiv);
      }
    }
  }

  formatImageData(imageData: string, imageType?: string, imageUrl?: string): string {
    // Priority 1: Use imageUrl from blob storage if available
    if (imageUrl && imageUrl.trim()) {
      // If it's a relative URL, convert to absolute URL
      if (imageUrl.startsWith('/')) {
        const baseUrl = environment.apiUrl || window.location.origin;
        const fullUrl = `${baseUrl}${imageUrl}`;
        console.log('🔗 Formatting relative URL:', imageUrl, '→', fullUrl);
        return fullUrl;
      }
      // If it's already an absolute URL (with or without protocol), return as is
      if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) {
        console.log('🔗 Using absolute URL:', imageUrl);
        return imageUrl;
      }
      // For any other URL format, return it as is (might be a blob URL or other valid URL)
      console.log('🔗 Using URL as-is:', imageUrl);
      return imageUrl;
    }

    // Priority 2: Fall back to imageData for backward compatibility
    if (!imageData || !imageData.trim()) {
      console.warn('⚠️ No imageUrl or imageData provided, returning empty string');
      return '';
    }

    // Check if imageData is already a valid data URL or regular URL
    if (imageData.startsWith('data:') || imageData.startsWith('http://') || imageData.startsWith('https://')) {
      console.log('✅ Using imageData URL:', imageData.substring(0, 50) + '...');
      return imageData;
    }

    // It's a base64 string without the data URL prefix
    // Determine MIME type from imageType field or default to PNG
    let mimeType = 'image/png'; // Default

    if (imageType) {
      const type = imageType.toLowerCase();
      if (type.includes('jpg') || type.includes('jpeg')) {
        mimeType = 'image/jpeg';
      } else if (type.includes('png')) {
        mimeType = 'image/png';
      } else if (type.includes('gif')) {
        mimeType = 'image/gif';
      } else if (type.includes('webp')) {
        mimeType = 'image/webp';
      } else if (type.includes('bmp')) {
        mimeType = 'image/bmp';
      }
    }

    console.log('📦 Converting base64 to data URL with type:', mimeType);
    return `data:${mimeType};base64,${imageData}`;
  }

  openImagePreview(imageData: string, imageType?: string, imageUrl?: string): void {
    if (!imageData && !imageUrl) {
      console.warn('⚠️ No image data or URL provided');
      return;
    }

    console.log('🖼️ Opening image preview. Type:', imageType, 'URL:', imageUrl, 'Data:', imageData?.substring(0, 50) + '...');

    const formattedUrl = this.formatImageData(imageData, imageType, imageUrl);
    if (formattedUrl.startsWith('data:image/')) {
      const newWindow = window.open();
      if (newWindow) {
        newWindow.document.write(`<img src="${formattedUrl}" style="max-width: 100%; height: auto;" />`);
      }
    } else {
      window.open(formattedUrl, '_blank', 'noopener,noreferrer');
    }
    console.log('✅ Opening formatted image URL', formattedUrl);

  }

  toggleWirImages(): void {
    this.wirImagesExpanded = !this.wirImagesExpanded;
  }

  toggleProgressImages(): void {
    this.progressImagesExpanded = !this.progressImagesExpanded;
  }

  toggleQualityImages(): void {
    this.qualityImagesExpanded = !this.qualityImagesExpanded;
  }

  collapseAllAttachments(): void {
    this.wirImagesExpanded = false;
    this.progressImagesExpanded = false;
    this.qualityImagesExpanded = false;
  }

  expandAllAttachments(): void {
    this.wirImagesExpanded = true;
    this.progressImagesExpanded = true;
    this.qualityImagesExpanded = true;
  }

  /**
   * Toggle expansion of a file group
   */
  toggleFileGroupExpansion(fileName: string): void {
    if (this.expandedFileGroups.has(fileName)) {
      this.expandedFileGroups.delete(fileName);
    } else {
      this.expandedFileGroups.add(fileName);
    }
  }

  /**
   * Check if a file group is expanded
   */
  isFileGroupExpanded(fileName: string): boolean {
    return this.expandedFileGroups.has(fileName);
  }

  /**
   * Toggle expansion of a URL group
   */
  toggleUrlGroupExpansion(fileName: string): void {
    if (this.expandedUrlGroups.has(fileName)) {
      this.expandedUrlGroups.delete(fileName);
    } else {
      this.expandedUrlGroups.add(fileName);
    }
  }

  /**
   * Check if a URL group is expanded
   */
  isUrlGroupExpanded(fileName: string): boolean {
    return this.expandedUrlGroups.has(fileName);
  }

  /**
   * Get visible versions for a file group (only latest if collapsed, all if expanded)
   */
  getVisibleVersions(group: { fileName: string; versions: BoxDrawing[] }): BoxDrawing[] {
    if (group.versions.length <= 1) {
      return group.versions; // Always show if only one version
    }
    const isExpanded = this.isFileGroupExpanded(group.fileName);
    const visibleVersions = isExpanded ? group.versions : [group.versions[0]];
    
    console.log(`getVisibleVersions for ${group.fileName}:`, {
      totalVersions: group.versions.length,
      isExpanded: isExpanded,
      visibleCount: visibleVersions.length,
      versions: visibleVersions
    });
    
    return visibleVersions;
  }

  /**
   * Get visible versions for a URL group (only latest if collapsed, all if expanded)
   */
  getVisibleUrlVersions(group: { fileName: string; versions: BoxDrawing[] }): BoxDrawing[] {
    if (group.versions.length <= 1) {
      return group.versions; // Always show if only one version
    }
    return this.isUrlGroupExpanded(group.fileName) ? group.versions : [group.versions[0]];
  }

  // WIR Image Group Methods
  toggleWirImageGroupExpansion(fileName: string): void {
    if (this.expandedWirImageGroups.has(fileName)) {
      this.expandedWirImageGroups.delete(fileName);
    } else {
      this.expandedWirImageGroups.add(fileName);
    }
  }

  isWirImageGroupExpanded(fileName: string): boolean {
    return this.expandedWirImageGroups.has(fileName);
  }

  getVisibleWirImageVersions(group: { fileName: string; versions: any[] }): any[] {
    if (group.versions.length <= 1) {
      return group.versions;
    }
    return this.isWirImageGroupExpanded(group.fileName) ? group.versions : [group.versions[0]];
  }

  // Progress Image Group Methods
  toggleProgressImageGroupExpansion(fileName: string): void {
    if (this.expandedProgressImageGroups.has(fileName)) {
      this.expandedProgressImageGroups.delete(fileName);
    } else {
      this.expandedProgressImageGroups.add(fileName);
    }
  }

  isProgressImageGroupExpanded(fileName: string): boolean {
    return this.expandedProgressImageGroups.has(fileName);
  }

  getVisibleProgressImageVersions(group: { fileName: string; versions: any[] }): any[] {
    if (group.versions.length <= 1) {
      return group.versions;
    }
    return this.isProgressImageGroupExpanded(group.fileName) ? group.versions : [group.versions[0]];
  }

  // Quality Image Group Methods
  toggleQualityImageGroupExpansion(fileName: string): void {
    if (this.expandedQualityImageGroups.has(fileName)) {
      this.expandedQualityImageGroups.delete(fileName);
    } else {
      this.expandedQualityImageGroups.add(fileName);
    }
  }

  isQualityImageGroupExpanded(fileName: string): boolean {
    return this.expandedQualityImageGroups.has(fileName);
  }

  getVisibleQualityImageVersions(group: { fileName: string; versions: any[] }): any[] {
    if (group.versions.length <= 1) {
      return group.versions;
    }
    return this.isQualityImageGroupExpanded(group.fileName) ? group.versions : [group.versions[0]];
  }

  /**
   * Open version history sidebar
   */
  openVersionHistory(group: { fileName: string; versions: BoxDrawing[] }): void {
    console.log('Opening version history for:', group.fileName);
    console.log('Versions count:', group.versions.length);
    console.log('Versions:', group.versions);
    
    this.currentFileName = group.fileName;
    this.currentFileVersions = [...group.versions].sort((a, b) => (b.version || 1) - (a.version || 1));
    this.versionHistorySidebarOpen = true;
    this.selectedVersionsForCompare = [];
    this.compareMode = false;
    
    console.log('Current file versions after assignment:', this.currentFileVersions);
    console.log('Sidebar open:', this.versionHistorySidebarOpen);
  }

  /**
   * Close version history sidebar
   */
  closeVersionHistory(): void {
    this.versionHistorySidebarOpen = false;
    this.currentFileVersions = [];
    this.currentFileName = '';
    this.selectedVersionsForCompare = [];
    this.compareMode = false;
  }

  /**
   * Toggle compare mode
   */
  toggleCompareMode(): void {
    this.compareMode = !this.compareMode;
    if (!this.compareMode) {
      this.selectedVersionsForCompare = [];
    }
  }

  /**
   * Toggle version selection for comparison
   */
  toggleVersionSelection(version: BoxDrawing): void {
    const index = this.selectedVersionsForCompare.findIndex(v => v.version === version.version);
    if (index > -1) {
      this.selectedVersionsForCompare.splice(index, 1);
    } else {
      if (this.selectedVersionsForCompare.length < 2) {
        this.selectedVersionsForCompare.push(version);
      } else {
        // Replace the oldest selection
        this.selectedVersionsForCompare.shift();
        this.selectedVersionsForCompare.push(version);
      }
    }
  }

  /**
   * Check if a version is selected for comparison
   */
  isVersionSelected(version: BoxDrawing): boolean {
    return this.selectedVersionsForCompare.some(v => v.version === version.version);
  }

  /**
   * Compare selected versions
   */
  compareSelectedVersions(): void {
    if (this.selectedVersionsForCompare.length === 2) {
      // Sort by version number (older first, newer second)
      const sorted = [...this.selectedVersionsForCompare].sort((a, b) => (a.version || 1) - (b.version || 1));
      const older = sorted[0];
      const newer = sorted[1];
      
      // Open both files in new tabs/windows for comparison
      window.open(older.displayUrl || older.imageUrl || older.drawingUrl, '_blank');
      setTimeout(() => {
        window.open(newer.displayUrl || newer.imageUrl || newer.drawingUrl, '_blank');
      }, 500);
    }
  }

  /**
   * Get formatted date and time
   */
  getFormattedDateTime(date: Date | undefined): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toLocaleString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    });
  }

  /**
   * Download attachment image
   * Now handles both ImageUrl (blob storage) and ImageData (base64)
   */
  downloadAttachmentImage(imageData: string, imageType?: string, originalName?: string, imageUrl?: string): void {
    if (!imageData && !imageUrl) {
      console.error('No image data or URL provided');
      return;
    }

    const formattedUrl = this.formatImageData(imageData, imageType, imageUrl);
    const fileName = originalName || `attachment-image-${Date.now()}.jpg`;

    // For data URLs, convert to blob and download
    if (formattedUrl.startsWith('data:image/')) {
      fetch(formattedUrl)
        .then(response => response.blob())
        .then(blob => {
          const blobUrl = URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = blobUrl;
          link.download = fileName;
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
          URL.revokeObjectURL(blobUrl);
        })
        .catch(error => {
          console.error('Error downloading image:', error);
        });
      return;
    }

    // For regular URLs (including API endpoints), fetch and download
    fetch(formattedUrl)
      .then(response => {
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.blob();
      })
      .then(blob => {
        const blobUrl = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = blobUrl;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(blobUrl);
      })
      .catch(error => {
        console.error('Error downloading image:', error);
        // Fallback: try direct download link
        const link = document.createElement('a');
        link.href = formattedUrl;
        link.download = fileName;
        link.target = '_blank';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      });
  }

  loadProgressUpdates(page: number = 1, pageSize: number = 10, forceRefresh: boolean = false): void {
    if (!this.boxId) {
      return;
    }

    this.progressUpdatesLoading = true;
    this.progressUpdatesError = '';
    this.progressUpdatesCurrentPage = page;
    this.progressUpdatesPageSize = pageSize;

    // Build search parameters
    const searchParams: ProgressUpdatesSearchParams = {};
    if (this.progressUpdatesSearchTerm?.trim()) {
      searchParams.searchTerm = this.progressUpdatesSearchTerm.trim();
    }
    if (this.progressUpdatesActivityName?.trim()) {
      searchParams.activityName = this.progressUpdatesActivityName.trim();
    }
    if (this.progressUpdatesStatus?.trim()) {
      searchParams.status = this.progressUpdatesStatus.trim();
    }
    if (this.progressUpdatesFromDate) {
      searchParams.fromDate = this.progressUpdatesFromDate;
    }
    if (this.progressUpdatesToDate) {
      searchParams.toDate = this.progressUpdatesToDate;
    }

    this.progressUpdateService.getProgressUpdatesByBox(this.boxId, page, pageSize, searchParams, forceRefresh).subscribe({
      next: (response) => {
        this.progressUpdates = (response.items || [])
          .map(update => ({
            ...update,
            updateDate: update.updateDate ? new Date(update.updateDate) : undefined
          }));
        this.progressUpdatesTotalCount = response.totalCount;
        this.progressUpdatesTotalPages = response.totalPages;
        this.progressUpdatesCurrentPage = response.pageNumber;
        this.progressUpdatesPageSize = response.pageSize;
        this.progressUpdatesLoading = false;
      },
      error: (err) => {
        console.error('❌ Error loading progress updates:', err);
        this.progressUpdatesError = err?.error?.message || err?.message || 'Failed to load progress updates';
        this.progressUpdatesLoading = false;
      }
    });
  }

  applyProgressUpdatesSearch(): void {
    this.progressUpdatesCurrentPage = 1; // Reset to first page when searching
    this.loadProgressUpdates(1, this.progressUpdatesPageSize, true); // Force refresh with new search params
  }

  clearProgressUpdatesSearch(): void {
    this.progressUpdatesSearchTerm = '';
    this.progressUpdatesActivityName = '';
    this.progressUpdatesStatus = '';
    this.progressUpdatesFromDate = '';
    this.progressUpdatesToDate = '';
    this.progressUpdatesCurrentPage = 1;
    this.loadProgressUpdates(1, this.progressUpdatesPageSize, true); // Force refresh after clearing search
  }

  toggleProgressUpdatesSearch(): void {
    this.showProgressUpdatesSearch = !this.showProgressUpdatesSearch;
    if (!this.showProgressUpdatesSearch) {
      this.clearProgressUpdatesSearch();
    }
  }

  onProgressUpdatesSearchChange(searchParams: {
    searchTerm: string;
    activityName: string;
    status: string;
    fromDate: string;
    toDate: string;
  }): void {
    this.progressUpdatesSearchTerm = searchParams.searchTerm;
    this.progressUpdatesActivityName = searchParams.activityName;
    this.progressUpdatesStatus = searchParams.status;
    this.progressUpdatesFromDate = searchParams.fromDate;
    this.progressUpdatesToDate = searchParams.toDate;
    this.applyProgressUpdatesSearch();
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
    this.currentImageInputMode = 'upload';
    this.showCamera = false;
    this.stopCamera();
    const fileInput = document.getElementById('box-details-photo-file-input') as HTMLInputElement;
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

  addImageFromUrl(): void {
    const url = this.currentUrlInput?.trim();
    if (!url) {
      this.photoUploadError = 'Please enter a valid URL';
      return;
    }

    // Validate URL format
    try {
      new URL(url);
    } catch {
      this.photoUploadError = 'Please enter a valid URL';
      return;
    }

    // Check if it's a data URL (base64)
    if (url.startsWith('data:image/')) {
      this.selectedImages.push({
        id: `url-${Date.now()}-${Math.random()}`,
        type: 'url',
        url: url,
        preview: url,
        name: 'Base64 Image'
      });
    } else {
      // For external URLs, we'll use the URL as preview (backend will download it)
      this.selectedImages.push({
        id: `url-${Date.now()}-${Math.random()}`,
        type: 'url',
        url: url,
        preview: url, // Will be replaced by actual image if URL is valid
        name: url.length > 50 ? url.substring(0, 50) + '...' : url
      });
    }

    this.currentUrlInput = '';
    this.photoUploadError = '';
  }

  removeImage(imageId: string): void {
    this.selectedImages = this.selectedImages.filter(img => img.id !== imageId);
  }

  trackByImageId(index: number, image: { id: string }): string {
    return image.id;
  }

  // Camera methods
  async openCamera(): Promise<void> {
    try {
      this.currentImageInputMode = 'camera';
      const stream = await navigator.mediaDevices.getUserMedia({ 
        video: { facingMode: 'environment' } // Use back camera on mobile
      });
      this.cameraStream = stream;
      this.showCamera = true;
      
      // Wait for video element to be rendered
      setTimeout(() => {
        const video = document.getElementById('box-details-camera-preview') as HTMLVideoElement;
        if (video) {
          video.srcObject = stream;
          video.play();
        }
      }, 100);
    } catch (err) {
      console.error('Error accessing camera:', err);
      this.photoUploadError = 'Unable to access camera. Please check permissions.';
      this.showCamera = false;
      this.currentImageInputMode = 'url';
    }
  }

  stopCamera(): void {
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach(track => track.stop());
      this.cameraStream = null;
    }
    this.showCamera = false;
    if (this.currentImageInputMode === 'camera') {
      this.currentImageInputMode = 'url';
    }
  }

  capturePhoto(): void {
    const video = document.getElementById('box-details-camera-preview') as HTMLVideoElement;
    if (!video) return;

    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const ctx = canvas.getContext('2d');
    if (ctx) {
      ctx.drawImage(video, 0, 0);
      canvas.toBlob((blob) => {
        if (blob) {
          const file = new File([blob], `photo-${Date.now()}.jpg`, { type: 'image/jpeg' });
          this.addImageFromFile(file);
          this.stopCamera();
        }
      }, 'image/jpeg', 0.9);
    }
  }

  // Box Status Update Methods
  openBoxStatusModal(): void {
    this.isBoxStatusModalOpen = true;
    this.boxStatusUpdateError = '';
    this.selectedBoxStatus = null;
    
    // Get available statuses based on current status and progress
    if (this.box) {
      this.availableBoxStatuses = getAvailableBoxStatuses(this.box.status, this.box.progress);
    } else {
      this.availableBoxStatuses = [];
    }
    
    document.body.style.overflow = 'hidden';
  }

  closeBoxStatusModal(): void {
    this.isBoxStatusModalOpen = false;
    this.selectedBoxStatus = null;
    this.boxStatusUpdateError = '';
    document.body.style.overflow = '';
  }

  updateBoxStatus(): void {
    if (!this.box || !this.selectedBoxStatus) {
      return;
    }

    this.boxStatusUpdateLoading = true;
    this.boxStatusUpdateError = '';

    // Convert status to number for backend
    const statusNumber = getBoxStatusNumber(this.selectedBoxStatus);

    this.boxService.updateBoxStatus(this.boxId, statusNumber).subscribe({
      next: (updatedBox) => {
        this.box = updatedBox;
        this.boxStatusUpdateLoading = false;
        this.closeBoxStatusModal();
        // Show success message
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Box status updated successfully', type: 'success' }
        }));
        console.log('✅ Box status updated successfully');
        // Refresh page to show updated status across all sections
        setTimeout(() => {
          window.location.reload();
        }, 1000);
      },
      error: (err) => {
        console.error('❌ Failed to update box status:', err);
        this.boxStatusUpdateLoading = false;
        this.boxStatusUpdateError = err?.error?.message || err?.message || 'Failed to update box status';
      }
    });
  }

  getStatusLabelForModal(status: BoxStatus): string {
    return this.getStatusLabel(status);
  }


  private setupBoxLogsSearch(): void {
    this.boxLogsSearchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.boxLogsSearchTerm = this.boxLogsSearchControl.value || '';
        this.boxLogsCurrentPage = 1;
        this.loadBoxLogs(this.boxLogsCurrentPage, this.boxLogsPageSize);
      });
  }

  loadBoxLogsCount(): void {
    if (!this.boxId) {
      return;
    }

    // Load just the count with minimal page size
    this.boxService.getBoxLogs(this.boxId, 1, 1).subscribe({
      next: (response) => {
        this.boxLogsTotalCount = response.totalCount || 0;
        this.boxLogsTotalPages = response.totalPages || 0;
      },
      error: (err) => {
        // Silently fail - count will be loaded when tab is clicked
        console.error('Error loading box logs count:', err);
      }
    });
  }

  loadBoxLogs(page: number = 1, pageSize: number = 25): void {
    if (!this.boxId) {
      return;
    }

    this.boxLogsLoading = true;
    this.boxLogsError = '';
    this.boxLogsCurrentPage = page;
    this.boxLogsPageSize = pageSize;

    const searchTerm = this.boxLogsSearchTerm?.trim() || undefined;
    const action = this.boxLogsAction?.trim() || undefined;
    const fromDate = this.boxLogsFromDate || undefined;
    const toDate = this.boxLogsToDate || undefined;
    const changedBy = this.boxLogsChangedBy?.trim() || undefined;

    this.boxService.getBoxLogs(this.boxId, page, pageSize, searchTerm, action, fromDate, toDate, changedBy).subscribe({
      next: (response) => {
        this.boxLogs = (response.items || []).map(log => ({
          ...log,
          performedAt: log.performedAt ? new Date(log.performedAt) : new Date()
        }));
        this.boxLogsTotalCount = response.totalCount || 0;
        this.boxLogsTotalPages = response.totalPages || 0;
        this.boxLogsCurrentPage = response.pageNumber || page;
        this.boxLogsPageSize = response.pageSize || pageSize;
        
        // Update available actions from current page
        this.boxLogActionSet.clear();
        this.boxLogs.forEach(log => {
          if (log.action) {
            this.boxLogActionSet.add(log.action);
          }
        });
        this.availableBoxLogActions = Array.from(this.boxLogActionSet).sort((a, b) => a.localeCompare(b));
        
        this.boxLogsLoading = false;
      },
      error: (err) => {
        console.error('Error loading box logs:', err);
        this.boxLogsError = err?.error?.message || err?.message || 'Failed to load box logs';
        this.boxLogsLoading = false;
      }
    });
  }

  loadBoxLogUsers(): void {
    if (this.availableBoxLogUsers.length > 0) {
      return; // Already loaded
    }

    this.loadingBoxLogUsers = true;
    this.userService.getUsers(1, 1000).subscribe({
      next: (response) => {
        this.availableBoxLogUsers = response.items.sort((a, b) => {
          const nameA = a.fullName || a.email || '';
          const nameB = b.fullName || b.email || '';
          return nameA.localeCompare(nameB);
        });
        this.loadingBoxLogUsers = false;
      },
      error: (err) => {
        console.error('Failed to load users:', err);
        this.loadingBoxLogUsers = false;
      }
    });
  }

  applyBoxLogsSearch(): void {
    this.boxLogsCurrentPage = 1;
    this.loadBoxLogs(1, this.boxLogsPageSize);
  }

  applyBoxLogsFilters(): void {
    this.boxLogsCurrentPage = 1;
    this.loadBoxLogs(1, this.boxLogsPageSize);
  }

  resetBoxLogsFilters(): void {
    this.boxLogsSearchTerm = '';
    this.boxLogsAction = '';
    this.boxLogsFromDate = '';
    this.boxLogsToDate = '';
    this.boxLogsChangedBy = '';
    this.boxLogsSearchControl.setValue('');
    this.boxLogsCurrentPage = 1;
    this.loadBoxLogs(1, this.boxLogsPageSize);
  }

  clearBoxLogsSearch(): void {
    this.resetBoxLogsFilters();
  }

  toggleBoxLogsSearch(): void {
    this.showBoxLogsSearch = !this.showBoxLogsSearch;
    if (this.showBoxLogsSearch) {
      // Load users when opening search for the first time
      this.loadBoxLogUsers();
    } else {
      this.clearBoxLogsSearch();
    }
  }

  onBoxLogsPageChange(page: number): void {
    this.loadBoxLogs(page, this.boxLogsPageSize);
  }

  onBoxLogsPageSizeChange(pageSize: number): void {
    this.loadBoxLogs(1, pageSize);
  }

  getBoxLogsPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPages = Math.min(this.boxLogsTotalPages, 7);
    let startPage = Math.max(1, this.boxLogsCurrentPage - 3);
    let endPage = Math.min(this.boxLogsTotalPages, startPage + maxPages - 1);
    
    if (endPage - startPage < maxPages - 1) {
      startPage = Math.max(1, endPage - maxPages + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  private applyUpdatedQualityIssue(updated: QualityIssueDetails): void {
    this.qualityIssues = this.qualityIssues.map(issue =>
      issue.issueId === updated.issueId ? { ...issue, ...updated } : issue
    );
    const count = this.qualityIssues.length;
    this.qualityIssueCount = count;
  }

  trackByLogId(index: number, log: BoxLog): string {
    return log.id;
  }

  getActionIcon(action: string): string {
    return DiffUtil.getActionIcon(action);
  }

  getActionType(action: string): 'create' | 'update' | 'delete' | 'assignment' | 'default' {
    const upperAction = action.toUpperCase();
    if (upperAction.includes('CREATE') || upperAction.includes('INSERT')) {
      return 'create';
    } else if (upperAction.includes('UPDATE') || upperAction.includes('MODIFY')) {
      return 'update';
    } else if (upperAction.includes('DELETE') || upperAction.includes('REMOVE')) {
      return 'delete';
    } else if (upperAction.includes('ASSIGN')) {
      return 'assignment';
    }
    return 'default';
  }

  isQualityIssueLog(log: any): boolean {
    return log.tableName?.toLowerCase().includes('qualityissue');
  }

  formatBoxLogDate(date?: string | Date): string {
    if (!date) return '—';
    const d = typeof date === 'string' ? new Date(date) : date;
    if (isNaN(d.getTime())) return '—';
    return d.toLocaleString('en-US', { 
      year: 'numeric', 
      month: 'short', 
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    });
  }

  formatBoxLogDescription(description?: string | null): string {
    if (!description) {
      return '';
    }
    // Format numeric values similar to audit logs
    const numericValueRegex = /([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)(%?)/g;
    return description.replace(numericValueRegex, (_match, valuePart: string, percentPart: string) => {
      const parsedValue = Number(valuePart);
      if (!isFinite(parsedValue)) {
        return _match;
      }

      const absValue = Math.abs(parsedValue);
      const decimals = absValue >= 1 ? 2 : 4;
      let formatted: string;

      if (parsedValue !== 0 && absValue < Math.pow(10, -decimals)) {
        formatted = parsedValue.toExponential(2);
      } else {
        formatted = parsedValue.toFixed(decimals);
        formatted = parseFloat(formatted).toString();
      }

      return percentPart ? `${formatted}${percentPart}` : formatted;
    });
  }

  getBoxLogNoChangeSummary(log: BoxLog): string {
    if (log.description) {
      return this.formatBoxLogDescription(log.description);
    }

    const actionLabel = DiffUtil.getActionLabel(log.action);
    const boxName = this.box?.name || this.box?.code || 'Box';

    return `${actionLabel} Box "${boxName}" with no tracked field changes.`;
  }

  trackByAction(index: number, action: string): string {
    return action;
  }

  openBoxLogDetails(log: BoxLog): void {
    this.selectedBoxLog = log;
    this.isBoxLogDetailsModalOpen = true;
    document.body.style.overflow = 'hidden';
  }

  closeBoxLogDetails(): void {
    this.isBoxLogDetailsModalOpen = false;
    this.selectedBoxLog = null;
    document.body.style.overflow = '';
  }

  /**
   * Extract box type abbreviation from BoxTag
   * BoxTag format: ProjectNumber-Building-Floor-Type-SubType
   */
  getBoxTypeFromTag(): string {
    if (!this.box) return '';
    const parts = (this.box.code || '').split('-');
    // Type is at position 3 (index 3)
    return parts.length >= 4 ? parts[3] : '';
  }

  /**
   * Extract box subtype abbreviation from BoxTag
   * BoxTag format: ProjectNumber-Building-Floor-Type-SubType
   */
  getBoxSubTypeFromTag(): string {
    if (!this.box) return '';
    const parts = (this.box.code || '').split('-');
    // SubType is at position 4 (index 4)
    return parts.length >= 5 ? parts[4] : '';
  }
}
