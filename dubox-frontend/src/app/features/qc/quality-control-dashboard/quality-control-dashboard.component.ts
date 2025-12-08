import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { WIRService } from '../../../core/services/wir.service';
import { QualityIssueItem, QualityIssueDetails, QualityIssueStatus, UpdateQualityIssueStatusRequest, WIRCheckpoint, WIRCheckpointStatus } from '../../../core/models/wir.model';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { PermissionService } from '../../../core/services/permission.service';
import { map } from 'rxjs/operators';
import * as ExcelJS from 'exceljs';

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
export class QualityControlDashboardComponent implements OnInit, OnDestroy {
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
  } = {
    status: 'Open',
    resolutionDescription: ''
  };
  statusUpdateLoading = false;
  statusUpdateError = '';
  qualityIssueStatuses: QualityIssueStatus[] = ['Open', 'InProgress', 'Resolved', 'Closed'];
  
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

  constructor(
    private fb: FormBuilder,
    private wirService: WIRService,
    private router: Router,
    private apiService: ApiService,
    private permissionService: PermissionService
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

  fetchCheckpoints(): void {
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
   * Get the API base URL
   */
  private getApiBaseUrl(): string {
    // Get base URL from environment or current location
    return (window as any).__env?.apiUrl || 'https://localhost:7098';
  }

  openImageInNewTab(imageUrl: string): void {
    window.open(imageUrl, '_blank', 'noopener,noreferrer');
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

  onImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.src = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjAwIiBoZWlnaHQ9IjIwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iMjAwIiBoZWlnaHQ9IjIwMCIgZmlsbD0iI2VlZSIvPjx0ZXh0IHg9IjUwJSIgeT0iNTAlIiBmb250LWZhbWlseT0iQXJpYWwiIGZvbnQtc2l6ZT0iMTQiIGZpbGw9IiM5OTkiIHRleHQtYW5jaG9yPSJtaWRkbGUiIGR5PSIuM2VtIj5JbWFnZSBub3QgYXZhaWxhYmxlPC90ZXh0Pjwvc3ZnPg==';
  }

  // Lightbox functionality
  lightboxOpen = false;
  lightboxImageIndex = 0;
  lightboxImages: string[] = [];

  openLightbox(imageUrl: string, allImages: string[]): void {
    this.lightboxImages = allImages;
    this.lightboxImageIndex = allImages.indexOf(imageUrl);
    if (this.lightboxImageIndex === -1) this.lightboxImageIndex = 0;
    this.lightboxOpen = true;
    document.body.style.overflow = 'hidden';
  }

  closeLightbox(): void {
    this.lightboxOpen = false;
    document.body.style.overflow = '';
  }

  nextImage(): void {
    if (this.lightboxImageIndex < this.lightboxImages.length - 1) {
      this.lightboxImageIndex++;
    } else {
      this.lightboxImageIndex = 0;
    }
  }

  previousImage(): void {
    if (this.lightboxImageIndex > 0) {
      this.lightboxImageIndex--;
    } else {
      this.lightboxImageIndex = this.lightboxImages.length - 1;
    }
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
    this.stopCamera();
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
    this.closeLightbox();
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
    
    // Update filtered list to reflect changes in the UI
    this.applyQualityIssuesFilters();
    // Update summary with updated checkpoints
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

  async exportQualityIssuesToExcel(): Promise<void> {
    if (this.filteredQualityIssues.length === 0) {
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
      'WIR Number',
      'Box Tag',
      'Box Name',
      'Project Code',
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
      { width: 15 },  // Project Code
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
    this.filteredQualityIssues.forEach((issue, index) => {
      const row = worksheet.addRow([
        index + 1,
        issue.wirNumber || issue.wirName || '—',
        issue.boxTag || '—',
        issue.boxName || '—',
        issue.projectCode || '—',
        this.getQualityIssueStatusLabel(issue.issueStatus || issue.status),
        issue.issueType || '—',
        issue.severity || '—',
        issue.issueDescription || '—',
        issue.assignedTo || '—',
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
}

