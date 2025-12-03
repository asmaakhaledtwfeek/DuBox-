import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Box, BoxStatus, getBoxStatusNumber } from '../../../core/models/box.model';
import { WIRService } from '../../../core/services/wir.service';
import { ProgressUpdate } from '../../../core/models/progress-update.model';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { ProgressUpdatesTableComponent } from '../../../shared/components/progress-updates-table/progress-updates-table.component';
import { QualityIssueDetails, QualityIssueStatus, UpdateQualityIssueStatusRequest, WIRCheckpoint, WIRCheckpointStatus, WIRRecord } from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ActivityTableComponent } from '../../activities/activity-table/activity-table.component';
import { LocationService, FactoryLocation, BoxLocationHistory } from '../../../core/services/location.service';
import { ApiService } from '../../../core/services/api.service';
import * as ExcelJS from 'exceljs';
import { Subject, takeUntil } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';

@Component({
  selector: 'app-box-details',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, HeaderComponent, SidebarComponent, ActivityTableComponent, ProgressUpdatesTableComponent],
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
  
  activeTab: 'overview' | 'activities' | 'wir' | 'quality-issues' | 'logs' | 'attachments' | 'location-history' = 'overview';
  
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
  
  // Photo upload state
  selectedFile: File | null = null;
  photoPreview: string | null = null;
  isUploadingPhoto = false;
  photoUploadError = '';
  cameraStream: MediaStream | null = null;
  showCamera = false;

  progressUpdates: ProgressUpdate[] = [];
  progressUpdatesLoading = false;
  progressUpdatesError = '';
  selectedProgressUpdate: ProgressUpdate | null = null;
  isProgressModalOpen = false;

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
  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxService: BoxService,
    private permissionService: PermissionService,
    private wirService: WIRService,
    private progressUpdateService: ProgressUpdateService,
    private locationService: LocationService,
    private apiService: ApiService
  ) {}

  ngOnInit(): void {
    this.boxId = this.route.snapshot.params['boxId'];
    this.projectId = this.route.snapshot.params['projectId'];
    
    this.canEdit = this.permissionService.canEdit('boxes');
    this.canDelete = this.permissionService.canDelete('boxes');
    
    this.setupLocationHistorySearch();
    this.loadBox();
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
        
        // Load activities separately
        this.loadActivities();
        this.loadWIRCheckpoints();
        this.loadQualityIssues();
        this.loadProgressUpdates();
        this.loadLocationHistory();
        
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

  setActiveTab(tab: 'overview' | 'activities' | 'wir' | 'quality-issues' | 'logs' | 'attachments' | 'location-history'): void {
    this.activeTab = tab;
    if (tab === 'location-history' && this.locationHistory.length === 0) {
      this.loadLocationHistory();
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
    return !!checkpoint?.wirId && !!checkpoint.boxActivityId;
  }

  canNavigateToReview(checkpoint: WIRCheckpoint | null): boolean {
    return !!checkpoint?.boxActivityId;
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
    this.selectedFile = null;
    this.photoPreview = null;
    this.showCamera = false;
    this.stopCamera();
    this.isStatusModalOpen = true;
  }

  closeStatusModal(): void {
    this.isStatusModalOpen = false;
    this.selectedFile = null;
    this.photoPreview = null;
    this.showCamera = false;
    this.stopCamera();
    this.selectedIssueForStatus = null;
    this.statusUpdateLoading = false;
    this.statusUpdateError = '';
  }

  openProgressDetails(update: ProgressUpdate): void {
    this.selectedProgressUpdate = update;
    this.isProgressModalOpen = true;
  }

  closeProgressDetails(): void {
    this.isProgressModalOpen = false;
    this.selectedProgressUpdate = null;
  }

  loadProgressUpdates(): void {
    if (!this.boxId) {
      return;
    }

    this.progressUpdatesLoading = true;
    this.progressUpdatesError = '';

    this.progressUpdateService.getProgressUpdatesByBox(this.boxId).subscribe({
      next: (updates) => {
        this.progressUpdates = (updates || [])
          .map(update => ({
            ...update,
            updateDate: update.updateDate ? new Date(update.updateDate) : undefined
          }))
          .sort((a, b) => {
            const aTime = a.updateDate ? new Date(a.updateDate).getTime() : 0;
            const bTime = b.updateDate ? new Date(b.updateDate).getTime() : 0;
            return bTime - aTime;
          });
        this.progressUpdatesLoading = false;
      },
      error: (err) => {
        console.error('❌ Error loading progress updates:', err);
        this.progressUpdatesError = err?.error?.message || err?.message || 'Failed to load progress updates';
        this.progressUpdatesLoading = false;
      }
    });
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

    // Upload photo if file is selected, otherwise proceed with URL
    if (this.selectedFile) {
      this.isUploadingPhoto = true;
      this.uploadPhoto(this.selectedFile).subscribe({
        next: (uploadResult) => {
          this.isUploadingPhoto = false;
          const photoPath = uploadResult || this.statusUpdateForm.photoPath?.trim() || null;
          this.submitStatusUpdateWithPhoto(photoPath);
        },
        error: (err: any) => {
          console.error('❌ Failed to upload photo:', err);
          this.isUploadingPhoto = false;
          this.photoUploadError = err?.error?.message || err?.message || 'Failed to upload photo';
          this.statusUpdateLoading = false;
        }
      });
    } else {
      const photoPath = this.statusUpdateForm.photoPath?.trim() || null;
      this.submitStatusUpdateWithPhoto(photoPath);
    }
  }

  private submitStatusUpdateWithPhoto(photoPath: string | null): void {
    if (!this.selectedIssueForStatus) {
      return;
    }

    const payload: UpdateQualityIssueStatusRequest = {
      issueId: this.selectedIssueForStatus.issueId,
      status: this.statusUpdateForm.status,
      resolutionDescription: this.requiresResolutionDescription(this.statusUpdateForm.status)
        ? this.statusUpdateForm.resolutionDescription?.trim()
        : null,
      photoPath: photoPath
    };

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

  // Photo upload methods
  openFileInput(): void {
    this.showCamera = false;
    const fileInput = document.getElementById('box-details-photo-file-input') as HTMLInputElement;
    if (fileInput) {
      fileInput.click();
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      if (file.type.startsWith('image/')) {
        this.selectedFile = file;
        this.statusUpdateForm.photoPath = ''; // Clear URL when file is selected
        this.previewImage(file);
      } else {
        this.photoUploadError = 'Please select an image file';
      }
    }
  }

  previewImage(file: File): void {
    const reader = new FileReader();
    reader.onload = (e) => {
      this.photoPreview = e.target?.result as string;
    };
    reader.readAsDataURL(file);
  }

  removeSelectedFile(): void {
    this.selectedFile = null;
    this.photoPreview = null;
  }

  uploadPhoto(file: File) {
    return this.apiService.upload<{ url: string }>('upload/quality-issue-photo', file).pipe(
      map((response: any) => {
        if (typeof response === 'string') return response;
        return response?.url || response?.photoPath || response?.data?.url || response?.data?.photoPath || '';
      })
    );
  }

  // Camera methods
  async openCamera(): Promise<void> {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ 
        video: { facingMode: 'environment' } // Use back camera on mobile
      });
      this.cameraStream = stream;
      this.showCamera = true;
      this.selectedFile = null;
      this.photoPreview = null;
      this.statusUpdateForm.photoPath = '';
      
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
    }
  }

  stopCamera(): void {
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach(track => track.stop());
      this.cameraStream = null;
    }
    this.showCamera = false;
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
          this.selectedFile = file;
          this.previewImage(file);
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
        this.loadLocationHistory(); // Refresh history
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

  private applyUpdatedQualityIssue(updated: QualityIssueDetails): void {
    this.qualityIssues = this.qualityIssues.map(issue =>
      issue.issueId === updated.issueId ? { ...issue, ...updated } : issue
    );
    const count = this.qualityIssues.length;
    this.qualityIssueCount = count;
  }
}
