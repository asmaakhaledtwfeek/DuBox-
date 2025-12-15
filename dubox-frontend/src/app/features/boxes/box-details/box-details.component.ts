import { Component, OnDestroy, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Box, BoxStatus, BoxLog, getBoxStatusNumber } from '../../../core/models/box.model';
import { WIRService } from '../../../core/services/wir.service';
import { ProgressUpdate, ProgressUpdatesSearchParams } from '../../../core/models/progress-update.model';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { ProgressUpdatesTableComponent } from '../../../shared/components/progress-updates-table/progress-updates-table.component';
import { QualityIssueDetails, QualityIssueStatus, UpdateQualityIssueStatusRequest, WIRCheckpoint, WIRCheckpointStatus, WIRRecord, CreateQualityIssueForBoxRequest, IssueType, SeverityType } from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ActivityTableComponent } from '../../activities/activity-table/activity-table.component';
import { BoxLogDetailsModalComponent } from '../box-log-details-modal/box-log-details-modal.component';
import { LocationService, FactoryLocation, BoxLocationHistory } from '../../../core/services/location.service';
import { ApiService } from '../../../core/services/api.service';
import { WirExportService, ProjectInfo } from '../../../core/services/wir-export.service';
import * as ExcelJS from 'exceljs';
import { Observable, Subject, takeUntil, forkJoin, of, firstValueFrom } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, skip, catchError } from 'rxjs/operators';
import { DiffUtil } from '../../../core/utils/diff.util';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-box-details',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, HttpClientModule, SidebarComponent, ActivityTableComponent, ProgressUpdatesTableComponent, HeaderComponent, BoxLogDetailsModalComponent],
  providers: [LocationService],
  animations: [
    trigger('slideDown', [
      transition(':enter', [
        style({ height: '0', opacity: '0', overflow: 'hidden' }),
        animate('300ms ease-out', style({ height: '*', opacity: '1' }))
      ]),
      transition(':leave', [
        style({ height: '*', opacity: '1', overflow: 'hidden' }),
        animate('300ms ease-in', style({ height: '0', opacity: '0' }))
      ])
    ])
  ],
  templateUrl: './box-details.component.html',
  styleUrls: ['./box-details.component.scss']
})
export class BoxDetailsComponent implements OnInit, OnDestroy {
  box: Box | null = null;
  boxId!: string;
  projectId!: string;
  loading = true;
  error = '';
  deleting = false;
  showDeleteConfirm = false;
  deleteSuccess = false;
  
  activeTab: 'overview' | 'activities' | 'wir' | 'quality-issues' | 'logs' | 'drawings' | 'progress-updates' | 'attachments' = 'overview';
  
  canEdit = false;
  canDelete = false;
  canUpdateBoxStatus = false;
  canUpdateQualityIssueStatus = false;
  BoxStatus = BoxStatus;
  Math = Math;
  
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
  } = {
    status: 'Open',
    resolutionDescription: ''
  };
  isDetailsModalOpen = false;
  selectedIssueDetails: QualityIssueDetails | null = null;
  
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
    assignedTo: string;
    dueDate: string;
  } = {
    issueType: 'Defect',
    severity: 'Major',
    issueDescription: '',
    assignedTo: '',
    dueDate: ''
  };
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
  boxDrawings: Array<{ imageUrl: string; displayUrl: string; updateDate?: Date; activityName?: string; progressPercentage?: number; imageType: 'file' | 'url'; originalUrl?: string }> = [];
  loadingBoxDrawings = false;
  boxDrawingsError = '';

  // All box attachments (WIR, Progress Update, Quality Issue images)
  boxAttachments: any = null;
  loadingBoxAttachments = false;
  boxAttachmentsError = '';
  
  // Collapsible sections state
  wirImagesExpanded = true;
  progressImagesExpanded = true;
  qualityImagesExpanded = true;
  
  // Sub-tab for Drawings section
  activeDrawingTab: 'file' | 'url' = 'file';
  
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
  availableBoxStatuses: BoxStatus[] = [BoxStatus.Dispatched, BoxStatus.InProgress, BoxStatus.OnHold];

  // Location Management
  isMoveLocationModalOpen = false;
  locations: FactoryLocation[] = [];
  locationsLoading = false;
  moveLocationLoading = false;
  moveLocationError = '';
  selectedLocationId = new FormControl<string>('', [Validators.required]);
  moveReason = new FormControl<string>('');
  locationHistory: BoxLocationHistory[] = [];
  filteredLocationHistory: BoxLocationHistory[] = [];
  locationHistoryLoading = false;
  showLocationHistory = false;
  locationHistorySearchControl = new FormControl('');
  
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
  showBoxLogsSearch = false;
  boxLogsSearchControl = new FormControl('');
  availableBoxLogActions: string[] = [];
  private boxLogActionSet = new Set<string>();
  
  // Box Log Details Modal
  selectedBoxLog: BoxLog | null = null;
  isBoxLogDetailsModalOpen = false;
  readonly DiffUtil = DiffUtil;
  
  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxService: BoxService,
    private permissionService: PermissionService,
    private wirService: WIRService,
    private progressUpdateService: ProgressUpdateService,
    @Inject(LocationService) private locationService: LocationService,
    private apiService: ApiService,
    private http: HttpClient,
    private wirExportService: WirExportService
  ) {}

  ngOnInit(): void {
    this.boxId = this.route.snapshot.params['boxId'];
    this.projectId = this.route.snapshot.params['projectId'];
    
    // Check for tab query parameter to set active tab
    const tabParam = this.route.snapshot.queryParams['tab'];
    if (tabParam && ['overview', 'activities', 'wir', 'quality-issues', 'logs', 'drawings', 'progress-updates'].includes(tabParam)) {
      this.activeTab = tabParam as any;
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
    
    this.setupLocationHistorySearch();
    this.setupBoxLogsSearch();
    this.loadBox();
  }
  
  private checkPermissions(): void {
    this.canEdit = this.permissionService.canEdit('boxes');
    this.canDelete = this.permissionService.canDelete('boxes');
    this.canUpdateBoxStatus = this.permissionService.canEdit('boxes') || 
                              this.permissionService.hasPermission('boxes', 'update-status');
    this.canUpdateQualityIssueStatus = this.permissionService.hasPermission('quality-issues', 'edit') || 
                                       this.permissionService.hasPermission('quality-issues', 'resolve');
    this.canCreateQualityIssue = this.permissionService.canCreate('quality-issues');
    console.log('✅ Box permissions checked:', {
      canEdit: this.canEdit,
      canDelete: this.canDelete,
      canUpdateBoxStatus: this.canUpdateBoxStatus,
      canCreateQualityIssue: this.canCreateQualityIssue
    });
  }

  private setupLocationHistorySearch(): void {
    this.locationHistorySearchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.applyLocationHistoryFilters();
      });
  }

  applyLocationHistoryFilters(): void {
    const searchTerm = this.locationHistorySearchControl.value?.toLowerCase() || '';
    
    if (!searchTerm) {
      this.filteredLocationHistory = [...this.locationHistory];
      return;
    }

    this.filteredLocationHistory = this.locationHistory.filter(history =>
      history.locationCode?.toLowerCase().includes(searchTerm) ||
      history.locationName?.toLowerCase().includes(searchTerm) ||
      history.movedFromLocationCode?.toLowerCase().includes(searchTerm) ||
      history.movedFromLocationName?.toLowerCase().includes(searchTerm) ||
      history.reason?.toLowerCase().includes(searchTerm) ||
      history.movedByUsername?.toLowerCase().includes(searchTerm) ||
      history.movedByFullName?.toLowerCase().includes(searchTerm) ||
      history.boxTag?.toLowerCase().includes(searchTerm) ||
      history.serialNumber?.toLowerCase().includes(searchTerm)
    );
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
          this.router.navigate(['/projects', this.projectId, 'boxes']);
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

  setActiveTab(tab: 'overview' | 'activities' | 'wir' | 'quality-issues' | 'logs' | 'drawings' | 'progress-updates' | 'attachments'): void {
    this.activeTab = tab;
    
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

  canNavigateToAddChecklist(checkpoint: WIRCheckpoint | null): boolean {
    // Check if checkpoint exists and has required data
    if (!checkpoint?.wirId || !checkpoint?.boxActivityId) {
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

  navigateToAddChecklist(checkpoint: WIRCheckpoint): void {
    if (!this.canNavigateToAddChecklist(checkpoint)) {
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

  // Create Quality Issue Methods
  openCreateQualityIssueModal(): void {
    this.isCreateQualityIssueModalOpen = true;
    this.createQualityIssueError = '';
    this.newQualityIssueForm = {
      issueType: 'Defect',
      severity: 'Major',
      issueDescription: '',
      assignedTo: '',
      dueDate: ''
    };
    this.qualityIssueImages = [];
    this.currentImageInputMode = 'url';
    this.currentUrlInput = '';
    this.showCamera = false;
  }

  closeCreateQualityIssueModal(): void {
    this.isCreateQualityIssueModalOpen = false;
    this.newQualityIssueForm = {
      issueType: 'Defect',
      severity: 'Major',
      issueDescription: '',
      assignedTo: '',
      dueDate: ''
    };
    this.qualityIssueImages = [];
    this.createQualityIssueError = '';
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach(track => track.stop());
      this.cameraStream = null;
    }
    this.showCamera = false;
  }

  createQualityIssue(): void {
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

    this.qualityIssueImages.forEach(img => {
      if (img.type === 'url' && img.url) {
        imageUrls.push(img.url.trim());
      } else if ((img.type === 'file' || img.type === 'camera') && img.file) {
        files.push(img.file);
      } else if (img.preview && img.preview.startsWith('data:image/')) {
        imageUrls.push(img.preview);
      }
    });

    const request: CreateQualityIssueForBoxRequest = {
      boxId: this.boxId,
      issueType: this.newQualityIssueForm.issueType,
      severity: this.newQualityIssueForm.severity,
      issueDescription: this.newQualityIssueForm.issueDescription.trim(),
      assignedTo: this.newQualityIssueForm.assignedTo?.trim() || undefined,
      dueDate: this.newQualityIssueForm.dueDate || undefined,
      imageUrls: imageUrls.length > 0 ? imageUrls : undefined,
      files: files.length > 0 ? files : undefined
    };

    console.log('📤 Creating quality issue with request:', {
      boxId: request.boxId,
      issueType: request.issueType,
      severity: request.severity,
      descriptionLength: request.issueDescription.length,
      assignedTo: request.assignedTo,
      dueDate: request.dueDate,
      imageUrlsCount: imageUrls.length,
      filesCount: files.length
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
        log.performedBy || 'System',
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
      'WIR Number',
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
        issue.assignedTo || '—',
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

  openStatusModal(issue: QualityIssueDetails): void {
    if (this.isDetailsModalOpen) {
      this.closeIssueDetails();
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
      fetch(photoUrl)
        .then(response => response.blob())
        .then(blob => {
          const blobUrl = URL.createObjectURL(blob);
          const newWindow = window.open(blobUrl, '_blank', 'noopener,noreferrer');
          if (!newWindow) {
            console.error('Failed to open image in new tab. Popup may be blocked.');
          }
        })
        .catch(error => {
          console.error('Error converting data URL to blob:', error);
          // Fallback: try to open data URL directly (may not work in all browsers)
          window.open(photoUrl, '_blank', 'noopener,noreferrer');
        });
      return;
    }
    // If it's already an absolute URL (http/https), use as is
    else if (!photoUrl.startsWith('http://') && !photoUrl.startsWith('https://')) {
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

  // Get file-type images (uploaded images)
  getFileTypeImages(): Array<{ imageUrl: string; displayUrl: string; updateDate?: Date; activityName?: string; progressPercentage?: number; imageType: 'file' | 'url'; originalUrl?: string }> {
    return this.boxDrawings.filter(img => img.imageType === 'file');
  }

  // Get URL-type images
  getUrlTypeImages(): Array<{ imageUrl: string; displayUrl: string; updateDate?: Date; activityName?: string; progressPercentage?: number; imageType: 'file' | 'url'; originalUrl?: string }> {
    return this.boxDrawings.filter(img => img.imageType === 'url');
  }

  // Open image - for URL-type images, open original URL, otherwise open the display URL
  openImage(image: { imageUrl: string; displayUrl: string; imageType: 'file' | 'url'; originalUrl?: string }): void {
    if (image.imageType === 'url' && image.originalUrl) {
      // For URL-type images, open the original URL stored in originalName
      console.log('🔗 Opening original URL:', image.originalUrl);
      window.open(image.originalUrl, '_blank');
    } else {
      // For file-type images, open the display URL
      console.log('🖼️ Opening image URL:', image.displayUrl || image.imageUrl);
      this.openPhotoInNewTab(image.displayUrl || image.imageUrl);
    }
  }

  // Load box drawings from dedicated endpoint
  loadBoxDrawings(): void {
    if (!this.boxId) {
      return;
    }

    this.loadingBoxDrawings = true;
    this.boxDrawingsError = '';

    console.log('📦 Loading box drawings for box:', this.boxId);

    this.boxService.getBoxDrawingImages(this.boxId).subscribe({
      next: (response) => {
        console.log('✅ Box drawings loaded in component:', response);
        console.log('✅ Response.images:', response.images);
        console.log('✅ Response.images length:', response.images?.length || 0);
        console.log('✅ Response.totalCount:', response.totalCount);
        
        // Process images and resolve URLs
        type ResolvedImage = { 
          imageUrl: string; 
          displayUrl: string; 
          updateDate?: Date; 
          activityName?: string; 
          progressPercentage?: number; 
          imageType: 'file' | 'url';
          originalUrl?: string; // Store original URL for URL-type images
        };

        const imageRequests = response.images.map((img, index) => {
          console.log(`🖼️ Processing image ${index + 1}:`, img);
          const imageUrl = img.imageUrl || img.imageData || '';
          
          // Convert relative URLs to absolute
          let absoluteUrl = imageUrl;
          if (imageUrl && (imageUrl.startsWith('/api/') || (imageUrl.startsWith('/') && !imageUrl.startsWith('http')))) {
            const baseUrl = `${window.location.protocol}//${window.location.host}`;
            absoluteUrl = baseUrl + imageUrl;
          }
          
          // For URL-type images, originalName contains the actual original URL
          const originalUrl = img.imageType === 'url' ? img.originalName : undefined;
          
          return this.fetchImageAsObjectUrl(absoluteUrl, img.imageData).pipe(
            map(displayUrl => ({
              imageUrl: absoluteUrl,
              displayUrl: displayUrl,
              updateDate: img.updateDate,
              activityName: img.activityName,
              progressPercentage: img.progressPercentage,
              imageType: img.imageType,
              originalUrl: originalUrl // Store the original URL
            } as ResolvedImage)),
            catchError(err => {
              console.error('❌ Failed to fetch image:', absoluteUrl, err);
              return of({
                imageUrl: absoluteUrl,
                displayUrl: absoluteUrl,
                updateDate: img.updateDate,
                activityName: img.activityName,
                progressPercentage: img.progressPercentage,
                imageType: img.imageType,
                originalUrl: originalUrl
              } as ResolvedImage);
            })
          );
        });

        if (imageRequests.length > 0) {
          forkJoin(imageRequests).subscribe({
            next: (resolvedImages: ResolvedImage[]) => {
              this.boxDrawings = resolvedImages;
              this.loadingBoxDrawings = false;
              console.log('🎨 Resolved box drawings:', {
                total: resolvedImages.length,
                fileType: resolvedImages.filter((img: ResolvedImage) => img.imageType === 'file').length,
                urlType: resolvedImages.filter((img: ResolvedImage) => img.imageType === 'url').length
              });
            },
            error: (err) => {
              console.error('❌ Error resolving images:', err);
              this.boxDrawingsError = 'Failed to load some images';
              this.loadingBoxDrawings = false;
            }
          });
        } else {
          this.boxDrawings = [];
          this.loadingBoxDrawings = false;
          console.warn('⚠️ No drawings found for this box. Response had no images.');
          console.log('⚠️ Empty response details:', {
            responseImages: response.images,
            responseImagesLength: response.images?.length,
            totalCount: response.totalCount
          });
        }
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
        this.boxAttachments = response;
        this.loadingBoxAttachments = false;
      },
      error: (err) => {
        console.error('❌ Error loading box attachments:', err);
        this.boxAttachmentsError = err.error?.message || err.message || 'Failed to load attachments';
        this.loadingBoxAttachments = false;
      }
    });
  }

  openImagePreview(imageUrl: string): void {
    if (imageUrl) {
      window.open(imageUrl, '_blank');
    }
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
        // Optionally show success message
        console.log('✅ Box status updated successfully');
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

  // Location Management Methods
  openMoveLocationModal(): void {
    this.isMoveLocationModalOpen = true;
    this.moveLocationError = '';
    this.selectedLocationId.setValue('');
    this.moveReason.setValue('');
    this.loadLocations();
  }

  closeMoveLocationModal(): void {
    this.isMoveLocationModalOpen = false;
    this.selectedLocationId.setValue('');
    this.moveReason.setValue('');
    this.moveLocationError = '';
  }

  loadLocations(): void {
    this.locationsLoading = true;
    this.locationService.getLocations().subscribe({
      next: (locations) => {
        this.locations = locations.filter(l => l.isActive);
        this.locationsLoading = false;
      },
      error: (err) => {
        console.error('Error loading locations:', err);
        this.locationsLoading = false;
        this.moveLocationError = 'Failed to load locations';
      }
    });
  }

  moveBoxToLocation(): void {
    if (!this.selectedLocationId.value || !this.box) {
      return;
    }

    this.moveLocationLoading = true;
    this.moveLocationError = '';

    this.locationService.moveBoxToLocation({
      boxId: this.boxId,
      toLocationId: this.selectedLocationId.value,
      reason: this.moveReason.value || undefined
    }).subscribe({
      next: (history) => {
        this.moveLocationLoading = false;
        this.closeMoveLocationModal();
        this.loadBox(); // Reload box to get updated location
        console.log('✅ Box moved to location successfully');
      },
      error: (err) => {
        console.error('❌ Failed to move box to location:', err);
        this.moveLocationLoading = false;
        this.moveLocationError = err?.error?.message || err?.message || 'Failed to move box to location';
      }
    });
  }

  loadLocationHistory(): void {
    this.locationHistoryLoading = true;
    this.locationService.getBoxLocationHistory(this.boxId).pipe(takeUntil(this.destroy$)).subscribe({
      next: (history) => {
        this.locationHistory = history;
        this.applyLocationHistoryFilters();
        this.locationHistoryLoading = false;
      },
      error: (err) => {
        console.error('Error loading location history:', err);
        this.locationHistoryLoading = false;
      }
    });
  }

  toggleLocationHistory(): void {
    this.showLocationHistory = !this.showLocationHistory;
    if (this.showLocationHistory && this.locationHistory.length === 0) {
      this.loadLocationHistory();
    }
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

    this.boxService.getBoxLogs(this.boxId, page, pageSize, searchTerm, action, fromDate, toDate).subscribe({
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
    this.boxLogsSearchControl.setValue('');
    this.boxLogsCurrentPage = 1;
    this.loadBoxLogs(1, this.boxLogsPageSize);
  }

  clearBoxLogsSearch(): void {
    this.resetBoxLogsFilters();
  }

  toggleBoxLogsSearch(): void {
    this.showBoxLogsSearch = !this.showBoxLogsSearch;
    if (!this.showBoxLogsSearch) {
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
}
