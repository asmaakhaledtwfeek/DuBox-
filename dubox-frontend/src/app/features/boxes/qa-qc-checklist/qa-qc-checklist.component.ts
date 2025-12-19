import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule, FormsModule, FormControl } from '@angular/forms';
import { WIRService } from '../../../core/services/wir.service';
import { BoxService } from '../../../core/services/box.service';
import { AuthService } from '../../../core/services/auth.service';
import { PermissionService } from '../../../core/services/permission.service';
import { ApiService } from '../../../core/services/api.service';
import { TeamService } from '../../../core/services/team.service';
import { Team } from '../../../core/models/team.model';
import { Observable, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { 
  WIRRecord, 
  CheckpointStatus, 
  WIRCheckpoint,
  CreateWIRCheckpointRequest,
  AddChecklistItemsRequest,
  UpdateChecklistItemRequest,
  PredefinedChecklistItem,
  ReviewWIRCheckpointRequest,
  WIRCheckpointStatus,
  CheckListItemStatus,
  AddQualityIssueRequest,
  IssueType,
  SeverityType,
  QualityIssueItem,
  QualityIssueDetails,
  QualityIssueImage,
  WIRCheckpointChecklistItem,
  QualityIssueStatus
} from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { QualityIssueDetailsModalComponent } from '../../../shared/components/quality-issue-details-modal/quality-issue-details-modal.component';
import * as ExcelJS from 'exceljs';

type ReviewStep = 'create-checkpoint' | 'add-items' | 'review' | 'quality-issues';

type QualityIssueFormValue = {
  issueType: IssueType;
  severity: SeverityType;
  issueDescription: string;
  assignedTo?: string; // Team ID
  assignedTeam?: string; // Team name for display
  dueDate?: string;
  photoPath?: string;
  reportedBy?: string;
  issueDate?: string;
  imageDataUrls?: string[];
  files?: File[]; // Store File objects separately
};

@Component({
  selector: 'app-qa-qc-checklist',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent, QualityIssueDetailsModalComponent],
  templateUrl: './qa-qc-checklist.component.html',
  styleUrls: ['./qa-qc-checklist.component.scss']
})
export class QaQcChecklistComponent implements OnInit, OnDestroy {
  wirRecord: WIRRecord | null = null;
  wirCheckpoint: WIRCheckpoint | null = null;
  projectId!: string;
  boxId!: string;
  activityId!: string;
  fromContext: 'quality-control' | 'box-details' = 'box-details';
  readonly highlightSection = 'quality-control';
  
  // Forms
  checklistForm!: FormGroup;
  createCheckpointForm!: FormGroup;
  addChecklistItemsForm!: FormGroup;
  qualityIssuesForm!: FormGroup;
  finalStatusControl!: FormControl<WIRCheckpointStatus>;
  newChecklistForm!: FormGroup;
  newQualityIssueForm!: FormGroup;
  isChecklistModalOpen = false;
  isDeleteModalOpen = false;
  isQualityIssueModalOpen = false;
  isQualityIssueDetailsModalOpen = false;
  selectedQualityIssueDetails: {
    issue?: QualityIssueItem;
    formIssue?: any;
    images?: string[];
  } | null = null;

  // Attachment/Image upload state (for Attachment Path field in Step 1)
  attachmentImages: Array<{ type: 'file' | 'url' | 'camera'; file?: File; url?: string; preview?: string; name?: string; size?: number }> = [];
  attachmentPhotoUrl: string = '';
  attachmentUploadError = '';
  attachmentCameraStream: MediaStream | null = null;
  showAttachmentCamera = false;
  attachmentInputMethod: 'url' | 'upload' | 'camera' = 'url';

  // Attachment image handling methods
  onAttachmentFileSelected(event: any): void {
    const files = event.target.files;
    if (files && files.length > 0) {
      Array.from(files as FileList).forEach((file: File) => {
        if (file.type.startsWith('image/')) {
          this.addAttachmentImageFile(file);
        } else {
          this.attachmentUploadError = 'Please select image files only';
        }
      });
      const fileInput = event.target as HTMLInputElement;
      if (fileInput) {
        fileInput.value = '';
      }
    }
  }

  addAttachmentImageFile(file: File): void {
    const reader = new FileReader();
    reader.onload = (e) => {
      this.attachmentImages.push({
        type: 'file',
        file: file,
        preview: e.target?.result as string,
        name: file.name,
        size: file.size
      });
      this.attachmentInputMethod = 'upload';
      this.attachmentUploadError = '';
    };
    reader.readAsDataURL(file);
  }

  addAttachmentImageUrl(url: string): void {
    if (url && url.trim()) {
      try {
        new URL(url);
        this.attachmentImages.push({
          type: 'url',
          url: url.trim(),
          preview: url.trim()
        });
        this.attachmentPhotoUrl = '';
        this.attachmentInputMethod = 'url';
        this.attachmentUploadError = '';
      } catch {
        this.attachmentUploadError = 'Please enter a valid URL';
      }
    }
  }

  openAttachmentFileInput(): void {
    this.showAttachmentCamera = false;
    const fileInput = document.getElementById('checkpoint-attachment-file-input') as HTMLInputElement;
    if (fileInput) {
      fileInput.click();
    }
  }

  removeAttachmentImage(index: number): void {
    this.attachmentImages.splice(index, 1);
    this.attachmentUploadError = '';
  }

  clearAllAttachmentImages(): void {
    this.attachmentImages = [];
    this.attachmentPhotoUrl = '';
    this.attachmentUploadError = '';
  }

  async openAttachmentCamera(): Promise<void> {
    try {
      this.stopAttachmentCamera();
      this.showAttachmentCamera = true;
      this.attachmentInputMethod = 'camera';
      this.attachmentUploadError = '';
      
      const stream = await navigator.mediaDevices.getUserMedia({ 
        video: { facingMode: 'environment' }
      });
      this.attachmentCameraStream = stream;
      
      setTimeout(() => {
        const video = document.getElementById('checkpoint-attachment-camera-preview') as HTMLVideoElement;
        if (video) {
          video.srcObject = stream;
          video.play().catch(err => {
            console.error('Error playing video:', err);
            this.attachmentUploadError = 'Unable to start camera preview.';
            this.stopAttachmentCamera();
          });
        } else {
          this.stopAttachmentCamera();
        }
      }, 100);
    } catch (err) {
      console.error('Error accessing camera:', err);
      this.attachmentUploadError = 'Unable to access camera. Please check permissions.';
      this.showAttachmentCamera = false;
    }
  }

  stopAttachmentCamera(): void {
    if (this.attachmentCameraStream) {
      this.attachmentCameraStream.getTracks().forEach(track => track.stop());
      this.attachmentCameraStream = null;
    }
    
    const video = document.getElementById('checkpoint-attachment-camera-preview') as HTMLVideoElement;
    if (video) {
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach(track => track.stop());
      }
      video.srcObject = null;
      video.pause();
    }
    
    this.showAttachmentCamera = false;
  }

  captureAttachmentPhoto(): void {
    const video = document.getElementById('checkpoint-attachment-camera-preview') as HTMLVideoElement;
    if (!video || !video.srcObject) return;

    if (!video.videoWidth || !video.videoHeight) {
      this.attachmentUploadError = 'Camera not ready. Please wait a moment and try again.';
      return;
    }

    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    ctx.drawImage(video, 0, 0);
    const imageData = canvas.toDataURL('image/jpeg', 0.9);

    this.stopAttachmentCamera();

    fetch(imageData)
      .then(res => res.blob())
      .then(blob => {
        const file = new File([blob], `checkpoint-attachment-${Date.now()}.jpg`, { type: 'image/jpeg' });
        this.attachmentImages.push({
          type: 'camera',
          file: file,
          preview: imageData,
          name: file.name,
          size: file.size
        });
        this.attachmentUploadError = '';
      })
      .catch(err => {
        console.error('Error converting data URL to file:', err);
        this.attachmentUploadError = 'Failed to process captured image.';
      });
  }
  
  // Convert to QualityIssueDetails for shared modal
  getQualityIssueDetailsForModal(): QualityIssueDetails | null {
    if (!this.selectedQualityIssueDetails) return null;
    
    const { issue, formIssue, images } = this.selectedQualityIssueDetails;
    
    // Build QualityIssueDetails from available data
    const details: QualityIssueDetails = {
      issueId: (issue as any)?.issueId || '',
      issueType: this.getQualityIssueDetail('issueType') || issue?.issueType || 'Defect',
      severity: this.getQualityIssueDetail('severity') || issue?.severity || 'Minor',
      issueDescription: this.getQualityIssueDetail('issueDescription') || issue?.issueDescription || '',
      assignedTeamName: this.getQualityIssueDetail('assignedTeam') || issue?.assignedTeam || issue?.assignedTo || undefined,
      dueDate: this.getQualityIssueDetail('dueDate') || issue?.dueDate,
      reportedBy: this.getQualityIssueDetail('reportedBy') || issue?.reportedBy,
      issueDate: this.getQualityIssueDetail('issueDate') || issue?.issueDate,
      status: (this.getQualityIssueDetail('status') || issue?.status || 'Open') as QualityIssueStatus,
      resolutionDescription: this.getQualityIssueDetail('resolutionDescription'),
      resolutionDate: this.getQualityIssueDetail('resolutionDate'),
      boxName: this.getQualityIssueDetail('boxName'),
      boxTag: this.getQualityIssueDetail('boxTag'),
      wirNumber: this.getQualityIssueDetail('wirNumber') || this.wirCheckpoint?.wirNumber,
      wirName: this.getQualityIssueDetail('wirName') || this.wirCheckpoint?.wirName,
      wirRequestedDate: this.getQualityIssueDetail('wirRequestedDate') || this.wirCheckpoint?.requestedDate,
      inspectorName: this.getQualityIssueDetail('inspectorName'),
      photoPath: issue?.photoPath,
      // Convert images array to QualityIssueImage format
      images: images?.map((imgUrl, index) => ({
        qualityIssueImageId: `temp-${index}`,
        issueId: (issue as any)?.issueId || '',
        imageData: imgUrl.startsWith('data:') ? imgUrl.split(',')[1] : (imgUrl.startsWith('http') ? undefined : imgUrl),
        imageUrl: imgUrl.startsWith('http') ? imgUrl : undefined,
        imageType: imgUrl.startsWith('http') ? 'url' : 'file' as 'file' | 'url',
        sequence: index,
        createdDate: new Date()
      })) || []
    };
    
    return details;
  }
  
  // Get WIR checkpoints array for modal (convert single checkpoint to array)
  getWirCheckpointsForModal(): WIRCheckpoint[] {
    return this.wirCheckpoint ? [this.wirCheckpoint] : [];
  }
  
  // Lightbox state
  lightboxOpen = false;
  lightboxImages: string[] = [];
  lightboxImageIndex = 0;
  
  // Quality issue status metadata
  qualityIssueStatusMeta: Record<QualityIssueStatus, { label: string; class: string }> = {
    Open: { label: 'OPEN', class: 'status-open' },
    InProgress: { label: 'IN PROGRESS', class: 'status-inprogress' },
    Resolved: { label: 'RESOLVED', class: 'status-resolved' },
    Closed: { label: 'CLOSED', class: 'status-closed' }
  };
  
  // Quality Issue Images
  qualityIssueImages: Array<{
    id: string;
    type: 'file' | 'url' | 'camera';
    file?: File;
    url?: string;
    preview: string;
    name?: string;
    size?: number;
  }> = [];
  showQualityIssueCamera = false;
  qualityIssuePhotoInputMethod: 'url' | 'upload' | 'camera' = 'upload';
  qualityIssueCurrentUrlInput = '';
  qualityIssueVideoRef?: HTMLVideoElement;
  qualityIssueCameraStream: MediaStream | null = null;
  availableTeams: Team[] = [];
  loadingTeams = false;
  isAddPredefinedItemsModalOpen = false;
  isItemReviewModalOpen = false;
  pendingDeleteIndex: number | null = null;
  pendingDeleteChecklistItemId: string | null = null;
  selectedReviewItemIndex: number | null = null;
  itemReviewForm!: FormGroup;
  savingItemReview = false;
  
  // Predefined checklist items
  predefinedChecklistItems: PredefinedChecklistItem[] = [];
  availablePredefinedItems: PredefinedChecklistItem[] = []; // Items not yet added to checkpoint
  loadingPredefinedItems = false;
  selectedPredefinedItemIds: string[] = [];
  shouldRefreshCheckpoint = false;
  
  loading = true;
  error = '';
  submitting = false;
  creatingCheckpoint = false;
  addingChecklistItems = false;
  savingQualityIssues = false;
  
  uploadedPhotos: File[] = [];
  photoPreviewUrls: string[] = [];
  boxQualityIssues: QualityIssueDetails[] = [];
  boxIssuesLoading = false;
  boxIssuesError = '';
  
  // Photo upload state (enhanced - multiple images support)
  selectedImages: Array<{
    id: string;
    type: 'file' | 'url' | 'camera';
    file?: File;
    url?: string;
    preview: string;
    name?: string;
    size?: number;
  }> = [];
  currentUrlInput: string = '';
  isUploadingPhoto = false;
  photoUploadError = '';
  cameraStream: MediaStream | null = null;
  showCamera = false;
  photoInputMethod: 'url' | 'upload' | 'camera' = 'upload';
  
  // Legacy properties for backward compatibility (deprecated)
  photoUrl: string = '';
  selectedFile: File | null = null;
  photoPreview: string | null = null;
  
  CheckpointStatus = CheckpointStatus;
  WIRCheckpointStatus = WIRCheckpointStatus;
  CheckListItemStatus = CheckListItemStatus;
  
  // Step tracking
  currentStep: ReviewStep | null = null;
  pendingAction: ReviewStep | null = null;
  private initialStepFromQuery: ReviewStep | null = null;
  initialStepApplied = false;
  qualityIssuesOnlyView = false;
  readonly stepFlow: ReviewStep[] = ['create-checkpoint', 'add-items', 'review', 'quality-issues'];
  readonly stepMeta: Array<{ id: ReviewStep; title: string; subtitle: string }> = [
    { id: 'create-checkpoint', title: 'Create Checkpoint', subtitle: 'Capture WIR context' },
    { id: 'add-items', title: 'Checklist Items', subtitle: 'Define inspection scope' },
    { id: 'review', title: 'Review & Sign-off', subtitle: 'Validate and approve' },
    { id: 'quality-issues', title: 'Quality Issues', subtitle: 'Log follow-up items (optional)' }
  ];
  readonly finalStatusOptions = [
    { label: 'Approved', value: WIRCheckpointStatus.Approved },
    { label: 'Conditional Approval', value: WIRCheckpointStatus.ConditionalApproval },
    { label: 'Rejected', value: WIRCheckpointStatus.Rejected }
  ];
  readonly issueTypes: IssueType[] = ['Defect', 'NonConformance', 'Observation'];
  readonly severityLevels: SeverityType[] = ['Critical', 'Major', 'Minor'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private wirService: WIRService,
    private boxService: BoxService,
    private authService: AuthService,
    private permissionService: PermissionService,
    private apiService: ApiService,
    private teamService: TeamService
  ) {}

  // Permission getter for template
  get canReviewWIR(): boolean {
    return this.permissionService.hasPermission('wir', 'review');
  }

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['projectId'];
    this.boxId = this.route.snapshot.params['boxId'];
    this.activityId = this.route.snapshot.params['activityId'];
    const fromParam = this.route.snapshot.queryParamMap.get('from');
    if (fromParam === 'quality-control') {
      this.fromContext = 'quality-control';
    }
    const checklistAdded = this.route.snapshot.queryParamMap.get('checklistAdded');
    if (checklistAdded === 'true') {
      setTimeout(() => {
        document.dispatchEvent(new CustomEvent('app-toast', { detail: { message: 'Checklist items added successfully!', type: 'success' } }));
      }, 0);
      this.router.navigate([], { queryParams: { checklistAdded: null }, queryParamsHandling: 'merge' });
    }
    
    // Check if returning from add-checklist-items page (refresh checkpoint data)
    const refreshParam = this.route.snapshot.queryParamMap.get('refresh');
    if (refreshParam) {
      this.shouldRefreshCheckpoint = true;
      // Clear the query param
      this.router.navigate([], { 
        queryParams: { refresh: null }, 
        queryParamsHandling: 'merge',
        replaceUrl: true 
      });
    }
    
    const stepParam = this.route.snapshot.queryParamMap.get('step');
    const viewParam = this.route.snapshot.queryParamMap.get('view');
    if (viewParam === 'quality-list') {
      this.qualityIssuesOnlyView = true;
    }
    // Capture step parameter for validation (will be validated after checkpoint loads)
    if (stepParam && ['create-checkpoint', 'add-items', 'review', 'quality-issues'].includes(stepParam)) {
      this.initialStepFromQuery = stepParam as ReviewStep;
    }
    
    this.initForm();
    this.loadWIRRecord();
    this.loadBoxQualityIssues();
    // Don't load predefined items here - will load when modal opens with correct WIR filter
  }

  private loadPredefinedChecklistItems(): void {
    this.loadingPredefinedItems = true;
    
    // Pass WIR number to API for server-side filtering (more efficient)
    const currentWirNumber = this.wirCheckpoint?.wirNumber;
    
    this.wirService.getPredefinedChecklistItems(currentWirNumber).subscribe({
      next: (items) => {
        this.predefinedChecklistItems = items;
        this.updateAvailablePredefinedItems();
        this.loadingPredefinedItems = false;
      },
      error: (err) => {
        console.error('Error loading predefined checklist items:', err);
        this.loadingPredefinedItems = false;
      }
    });
  }


  private updateAvailablePredefinedItems(): void {
    if (!this.wirCheckpoint || !this.predefinedChecklistItems.length) {
      this.availablePredefinedItems = [...this.predefinedChecklistItems];
      return;
    }

    // Get IDs of items already added to this checkpoint
    const addedPredefinedIds = new Set(
      (this.wirCheckpoint.checklistItems || [])
        .map(item => item.predefinedItemId)
        .filter(id => id != null) as string[]
    );

    // Filter items by current WIR number AND exclude already added items
    const currentWirNumber = this.wirCheckpoint.wirNumber;
    this.availablePredefinedItems = this.predefinedChecklistItems.filter(
      item => item.wirNumber === currentWirNumber && !addedPredefinedIds.has(item.predefinedItemId)
    );
  }

  getGroupedPredefinedItems(): { category: string; items: PredefinedChecklistItem[] }[] {
    const grouped = new Map<string, PredefinedChecklistItem[]>();
    
    // Group items by category (use categoryName first, fallback to category, then 'OTHER')
    this.availablePredefinedItems.forEach(item => {
      const category = item.categoryName || item.category || 'OTHER';
      if (!grouped.has(category)) {
        grouped.set(category, []);
      }
      grouped.get(category)!.push(item);
    });

    // Convert to array and sort by WIR number, then sequence
    const result: { category: string; items: PredefinedChecklistItem[] }[] = [];
    
    // Sort categories alphabetically, but put 'OTHER' at the end
    const categories = Array.from(grouped.keys()).sort((a, b) => {
      if (a === 'OTHER') return 1;
      if (b === 'OTHER') return -1;
      return a.localeCompare(b);
    });

    // Add all categories with their items sorted by sequence
    categories.forEach(category => {
        result.push({
          category,
        items: grouped.get(category)!.sort((a, b) => a.sequence - b.sequence)
        });
    });

    return result;
  }

  selectAllItems(): void {
    this.selectedPredefinedItemIds = this.availablePredefinedItems.map(item => item.predefinedItemId);
  }

  deselectAllItems(): void {
    this.selectedPredefinedItemIds = [];
  }

  selectAllInGroup(category: string): void {
    const group = this.getGroupedPredefinedItems().find(g => g.category === category);
    if (!group) return;

    const groupItemIds = group.items.map(item => item.predefinedItemId);
    groupItemIds.forEach(itemId => {
      if (!this.selectedPredefinedItemIds.includes(itemId)) {
        this.selectedPredefinedItemIds.push(itemId);
      }
    });
  }

  deselectAllInGroup(category: string): void {
    const group = this.getGroupedPredefinedItems().find(g => g.category === category);
    if (!group) return;

    const groupItemIds = group.items.map(item => item.predefinedItemId);
    this.selectedPredefinedItemIds = this.selectedPredefinedItemIds.filter(
      id => !groupItemIds.includes(id)
    );
  }

  areAllItemsSelected(): boolean {
    return this.availablePredefinedItems.length > 0 && 
           this.selectedPredefinedItemIds.length === this.availablePredefinedItems.length;
  }

  areAllInGroupSelected(category: string): boolean {
    const group = this.getGroupedPredefinedItems().find(g => g.category === category);
    if (!group || group.items.length === 0) return false;

    return group.items.every(item => this.selectedPredefinedItemIds.includes(item.predefinedItemId));
  }

  private initForm(): void {
    // Review form (for final review)
    this.checklistForm = this.fb.group({
      checklistItems: this.fb.array([]),
      inspectionNotes: ['', [Validators.maxLength(1000)]],
      inspectorRole: ['', [Validators.maxLength(100)]]
    });

    // Create checkpoint form (only user-filled fields)
    this.createCheckpointForm = this.fb.group({
      wirName: ['', [Validators.maxLength(200)]],
      wirDescription: ['', [Validators.maxLength(500)]],
      attachmentPath: ['', [Validators.maxLength(500)]],
      comments: ['', [Validators.maxLength(1000)]]
    });

    // Add checklist items form
    this.addChecklistItemsForm = this.fb.group({
      checklistItems: this.fb.array([])
    });

    this.qualityIssuesForm = this.fb.group({
      issues: this.fb.array([])
    });
    this.newQualityIssueForm = this.fb.group({
      issueDescription: ['', [Validators.required, Validators.maxLength(500)]],
      severity: [this.severityLevels[0], Validators.required],
      issueType: [this.issueTypes[0], Validators.required],
      assignedTo: ['', [Validators.maxLength(200)]],
      dueDate: [''],
      photoPath: ['', [Validators.maxLength(500)]]
    });

    this.finalStatusControl = this.fb.control<WIRCheckpointStatus>(WIRCheckpointStatus.Approved, { nonNullable: true });
    this.finalStatusControl.valueChanges.subscribe(status => this.handleFinalStatusChange(status));
    this.newChecklistForm = this.fb.group({
      checkpointDescription: ['', [Validators.required, Validators.maxLength(500)]],
      referenceDocument: ['', [Validators.maxLength(250)]]
    });

    // Item review form
    this.itemReviewForm = this.fb.group({
      status: [CheckpointStatus.Pending, Validators.required],
      referenceDocument: ['', [Validators.maxLength(250)]],
      remarks: ['', [Validators.maxLength(1000)]]
    });

    // Add conditional validation for remarks when status is Fail
    this.itemReviewForm.get('status')?.valueChanges.subscribe(status => {
      const remarksControl = this.itemReviewForm.get('remarks');
      if (status === CheckpointStatus.Fail) {
        remarksControl?.setValidators([Validators.required, Validators.maxLength(1000)]);
      } else {
        remarksControl?.setValidators([Validators.maxLength(1000)]);
      }
      remarksControl?.updateValueAndValidity();
    });
  }

  get checklistItems(): FormArray {
    return this.checklistForm.get('checklistItems') as FormArray;
  }

  getGroupedReviewChecklistItems(): { checklistName: string; sections: { sectionName: string; items: any[] }[] }[] {
    const itemsArray = this.checklistItems;
    if (!itemsArray || itemsArray.length === 0) {
      return [];
    }

    // Group by checklist -> section -> items
    const grouped = new Map<string, Map<string, any[]>>();

    itemsArray.controls.forEach((control, index) => {
      const formGroup = control as FormGroup;
      const checklistName = formGroup.get('checklistName')?.value || 'General Checklist';
      const sectionName = formGroup.get('sectionName')?.value || 'General Items';
      const checkpointDescription = formGroup.get('checkpointDescription')?.value;
      const status = formGroup.get('status')?.value;
      
      if (!grouped.has(checklistName)) {
        grouped.set(checklistName, new Map());
      }
      
      const checklistMap = grouped.get(checklistName)!;
      if (!checklistMap.has(sectionName)) {
        checklistMap.set(sectionName, []);
      }
      
      checklistMap.get(sectionName)!.push({
        control: formGroup,
        index: index,
        checkpointDescription: checkpointDescription,
        status: status,
        checklistName: checklistName,
        sectionName: sectionName
      });
    });

    // Sort checklists alphabetically
    const checklists = Array.from(grouped.keys()).sort((a, b) => {
      if (a === 'General Checklist') return -1;
      if (b === 'General Checklist') return 1;
      return a.localeCompare(b);
    });

    return checklists.map(checklist => {
      const checklistMap = grouped.get(checklist)!;
      const sections = Array.from(checklistMap.keys()).sort((a, b) => {
        if (a === 'General Items') return -1;
        if (b === 'General Items') return 1;
        return a.localeCompare(b);
      });

      return {
        checklistName: checklist,
        sections: sections.map(sectionName => ({
          sectionName: sectionName,
          items: checklistMap.get(sectionName)!
        }))
      };
    });
  }

  get qualityIssuesArray(): FormArray {
    return this.qualityIssuesForm.get('issues') as FormArray;
  }

  loadWIRRecord(): void {
    this.loading = true;
    this.error = '';
    
    // Get WIR records for this activity
    this.wirService.getWIRRecordsByActivity(this.activityId).subscribe({
      next: (wirs) => {
        if (wirs && wirs.length > 0) {
          // Get the rejected WIR record (since we're coming from rejection)
          this.wirRecord = wirs.find(w => w.status === 'Rejected') || wirs.find(w => w.status === 'Pending') || wirs[0];
          
          // Pre-fill create checkpoint form with suggested values
          if (this.wirRecord && this.createCheckpointForm) {
            this.createCheckpointForm.patchValue({
              wirName: `${this.wirRecord.wirCode} - ${this.wirRecord.activityName}`,
              wirDescription: `WIR checkpoint for ${this.wirRecord.activityName}`
            });
          }
          
          // Check if WIR checkpoint exists for this activity
          this.checkWIRCheckpoint();
        } else {
          this.error = 'No WIR record found for this activity.';
          this.loading = false;
        }
      },
      error: (err) => {
        this.error = 'Failed to load WIR record';
        this.loading = false;
        console.error('Error loading WIR:', err);
      }
    });
  }

  checkWIRCheckpoint(): void {
    if (!this.wirRecord || !this.wirRecord.wirCode) {
      this.loading = false;
      return;
    }

    const expectedWirCode = this.wirRecord.wirCode;
console.log(expectedWirCode);
    // Check if WIR checkpoint exists for this box/activity by WIR code
    console.log('checkWIRCheckpoint - Searching for checkpoint with boxId:', this.boxId, 'wirCode:', expectedWirCode);
    this.wirService.getWIRCheckpointByActivity(this.boxId, expectedWirCode).subscribe({
      next: (checkpoint) => {
        if (checkpoint) {
          console.log('Found checkpoint by activity search:', checkpoint);
          console.log('Checkpoint WIR Number:', checkpoint.wirNumber, 'Expected:', expectedWirCode);
          console.log('Checkpoint WIR ID:', checkpoint.wirId);
          
          // Verify the checkpoint matches the expected WIR code before reloading
          const wirCodeMatch = checkpoint.wirNumber === expectedWirCode || 
                               checkpoint.wirNumber?.toLowerCase() === expectedWirCode?.toLowerCase();
          
          if (!wirCodeMatch) {
            console.warn('Checkpoint WIR code mismatch! Expected:', expectedWirCode, 'Got:', checkpoint.wirNumber);
            // Don't use this checkpoint, treat as if none exists
            this.currentStep = 'create-checkpoint';
            this.pendingAction = null;
            this.applyInitialStepFromQuery();
            this.loading = false;
            return;
          }
          
          // Reload checkpoint by ID to ensure all data including checklist items is included
          this.wirService.getWIRCheckpointById(checkpoint.wirId).subscribe({
            next: (fullCheckpoint) => {
              console.log('Reloaded checkpoint by ID:', fullCheckpoint);
              console.log('Reloaded checkpoint WIR Number:', fullCheckpoint.wirNumber);
              console.log('Checkpoint checklist items:', fullCheckpoint.checklistItems?.length || 0, fullCheckpoint.checklistItems);
              
              // Verify the reloaded checkpoint still matches
              const reloadedMatch = fullCheckpoint.wirNumber === expectedWirCode || 
                                    fullCheckpoint.wirNumber?.toLowerCase() === expectedWirCode?.toLowerCase();
              
              if (!reloadedMatch) {
                console.error('Reloaded checkpoint WIR code mismatch! Expected:', expectedWirCode, 'Got:', fullCheckpoint.wirNumber);
                this.error = 'Checkpoint data mismatch. Please refresh the page.';
                this.loading = false;
                return;
              }
              
              this.wirCheckpoint = fullCheckpoint;
              this.syncReviewFormFromCheckpoint();
              this.processCheckpointLoaded();
            },
            error: (err) => {
              console.error('Error loading checkpoint by ID, using checkpoint from search:', err);
              // Fallback to checkpoint from search, but verify it matches
              if (wirCodeMatch) {
                this.wirCheckpoint = checkpoint;
                console.log('Using checkpoint from search:', checkpoint);
                console.log('Checkpoint checklist items:', checkpoint.checklistItems?.length || 0, checkpoint.checklistItems);
                this.syncReviewFormFromCheckpoint();
                this.processCheckpointLoaded();
              } else {
                this.error = 'Failed to load correct checkpoint data.';
                this.loading = false;
              }
            }
          });
        } else {
          // No checkpoint exists, start at Step 1
          console.log('No checkpoint found for WIR code:', expectedWirCode);
          this.currentStep = 'create-checkpoint';
          this.pendingAction = null;
          this.applyInitialStepFromQuery();
          this.loading = false;
        }
      },
      error: (err) => {
        console.error('Error checking WIR checkpoint:', err);
        this.currentStep = 'create-checkpoint';
        this.pendingAction = null;
        this.loading = false;
      }
    });
  }

  private processCheckpointLoaded(): void {
    if (!this.wirCheckpoint) {
      this.loading = false;
      return;
    }
    
    console.log('processCheckpointLoaded - wirCheckpoint exists:', this.wirCheckpoint.wirId);
    console.log('processCheckpointLoaded - initialStepFromQuery:', this.initialStepFromQuery);
    
    // If there's a step query parameter, apply it first
    if (this.initialStepFromQuery) {
      // Validate query parameter step before applying
      if (this.initialStepFromQuery === 'create-checkpoint') {
        // Allow create-checkpoint if status is Pending, otherwise redirect to add-items
        if (this.wirCheckpoint.status === 'Pending') {
          console.log('Checkpoint exists with Pending status, allowing edit in Step 1');
          this.currentStep = 'create-checkpoint';
          this.pendingAction = null;
          this.createCheckpointForm.patchValue({
            wirName: this.wirCheckpoint.wirName || '',
            wirDescription: this.wirCheckpoint.wirDescription || '',
            inspectorName: this.wirCheckpoint.inspectorName || '',
            inspectorRole: this.wirCheckpoint.inspectorRole || ''
          });
          
          // Load existing attachment path into images array if it exists
          if (this.wirCheckpoint.attachmentPath) {
            // Check if it's a URL or a file path
            if (this.wirCheckpoint.attachmentPath.startsWith('http://') || this.wirCheckpoint.attachmentPath.startsWith('https://')) {
              this.attachmentImages = [{
                type: 'url',
                url: this.wirCheckpoint.attachmentPath,
                preview: this.wirCheckpoint.attachmentPath
              }];
            } else {
              // For file paths, we'll just show it as a URL for now
              this.attachmentImages = [{
                type: 'url',
                url: this.wirCheckpoint.attachmentPath,
                preview: this.wirCheckpoint.attachmentPath
              }];
            }
          }
          
          this.initialStepApplied = true;
        } else {
          console.log('Checkpoint already reviewed, redirecting to add-items');
          this.currentStep = 'add-items'; // Set immediately
          this.router.navigate([], {
            relativeTo: this.route,
            queryParams: { step: 'add-items' },
            queryParamsHandling: 'merge',
            replaceUrl: true
          });
          this.startAddChecklistFlow();
          this.initialStepApplied = true;
        }
      } else {
        // Apply initial step from query (will validate accessibility)
        // If navigating to add-items, ensure form is built with existing items
        if (this.initialStepFromQuery === 'add-items' && this.wirCheckpoint.checklistItems && this.wirCheckpoint.checklistItems.length > 0) {
          console.log('Pre-populating checklist form with', this.wirCheckpoint.checklistItems.length, 'items');
          this.buildAddChecklistItemsForm(this.wirCheckpoint.checklistItems);
        }
        this.applyInitialStepFromQuery();
      }
    } else {
      // No step query parameter - checkpoint exists, navigate directly to add-items
      console.log('Checkpoint already exists, navigating directly to add-items');
      console.log('Setting currentStep to add-items immediately');
      this.currentStep = 'add-items'; // Set immediately
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { step: 'add-items' },
        queryParamsHandling: 'merge',
        replaceUrl: true
      });
      this.startAddChecklistFlow();
    }
    
    this.updateAvailablePredefinedItems();
    this.loading = false;
  }

  onCreateCheckpoint(): void {
    if (this.createCheckpointForm.invalid || !this.wirRecord) {
      this.markFormGroupTouched(this.createCheckpointForm);
      return;
    }

    this.creatingCheckpoint = true;
    this.error = '';
    
    const formValue = this.createCheckpointForm.value;
    
    // Separate files and URLs from attachment images
    const files: File[] = this.attachmentImages
      .filter(img => (img.type === 'file' || img.type === 'camera') && img.file)
      .map(img => img.file!);
    
    const imageUrls: string[] = this.attachmentImages
      .filter(img => img.type === 'url' && img.url)
      .map(img => img.url!);
    
    // For backward compatibility, set attachmentPath to first URL if no files
    let attachmentPath: string | undefined = undefined;
    if (imageUrls.length > 0 && files.length === 0) {
      attachmentPath = imageUrls[0];
    }
    
    const request: CreateWIRCheckpointRequest = {
      boxActivityId: this.activityId, // Get from route param
      wirNumber: this.wirRecord.wirCode || '', // Get from WIRRecord
      wirName: formValue.wirName?.trim() || undefined,
      wirDescription: formValue.wirDescription?.trim() || undefined,
      attachmentPath: attachmentPath || formValue.attachmentPath?.trim() || undefined,
      comments: formValue.comments?.trim() || undefined,
      files: files.length > 0 ? files : undefined,
      imageUrls: imageUrls.length > 0 ? imageUrls : undefined
    };

    this.wirService.createWIRCheckpoint(request).subscribe({
      next: (checkpoint) => {
        // After creating checkpoint, fetch it again by ID to ensure all data including checklist items is loaded
        this.wirService.getWIRCheckpointById(checkpoint.wirId).subscribe({
          next: (fullCheckpoint) => {
            this.wirCheckpoint = fullCheckpoint;
            console.log('Reloaded checkpoint after creation:', fullCheckpoint);
            console.log('Checklist items in reloaded checkpoint:', fullCheckpoint.checklistItems?.length || 0);
            this.syncReviewFormFromCheckpoint();
            this.creatingCheckpoint = false;
            // Navigate directly to Step 2 (Add Checklist Items)
            this.startAddChecklistFlow();
            // Clear the step query parameter to allow normal flow
            this.router.navigate([], {
              relativeTo: this.route,
              queryParams: { step: null },
              queryParamsHandling: 'merge'
            });
          },
          error: (err) => {
            console.error('Error reloading checkpoint:', err);
            // Fallback to using the checkpoint from creation response
            this.wirCheckpoint = checkpoint;
            this.syncReviewFormFromCheckpoint();
            this.creatingCheckpoint = false;
            this.startAddChecklistFlow();
            this.router.navigate([], {
              relativeTo: this.route,
              queryParams: { step: null },
              queryParamsHandling: 'merge'
            });
          }
        });
      },
      error: (err) => {
        this.creatingCheckpoint = false;
        this.error = err.error?.message || err.message || 'Failed to create WIR checkpoint';
        console.error('Error creating WIR checkpoint:', err);
      }
    });
  }

  private buildAddChecklistItemsForm(sourceItems?: WIRCheckpointChecklistItem[]): void {
    const itemsArray = this.addChecklistItemsArray;
    itemsArray.clear();
    
    console.log('buildAddChecklistItemsForm called with items:', sourceItems?.length || 0);
    console.log('Source items data:', sourceItems);
    
    if (sourceItems && sourceItems.length > 0) {
      sourceItems
        .sort((a, b) => (a.sequence || 0) - (b.sequence || 0))
        .forEach((item, index) => {
          console.log(`Adding checklist item ${index + 1}:`, item);
          itemsArray.push(this.createChecklistItemGroup({
            checklistItemId: item.checklistItemId,
            checkpointDescription: item.checkpointDescription,
            referenceDocument: item.referenceDocument,
            sequence: item.sequence,
            predefinedItemId: item.predefinedItemId,
            categoryName: item.categoryName || 'General',
            sectionName: item.sectionName || undefined,
            sectionId: item.sectionId || undefined,
            sectionOrder: item.sectionOrder || undefined,
            checklistName: item.checklistName || undefined,
            checklistId: item.checklistId || undefined,
            checklistCode: item.checklistCode || undefined
          }));
        });
      console.log('Form array after building:', itemsArray.length, 'items');
    } else {
      console.log('No source items provided or empty array');
    }

    this.updateAddChecklistSequences();
  }

  onAddChecklistItems(): void {
    if (!this.wirCheckpoint || this.selectedPredefinedItemIds.length === 0) {
      this.error = 'Please select at least one predefined checklist item to add.';
      return;
    }

    if (this.isChecklistLocked) {
      this.notifyChecklistLocked();
      return;
    }

    this.addingChecklistItems = true;
    this.error = '';

    const request: AddChecklistItemsRequest = {
      wirId: this.wirCheckpoint.wirId,
      predefinedItemIds: this.selectedPredefinedItemIds
    };

    this.wirService.addChecklistItems(request).subscribe({
      next: () => {
        this.selectedPredefinedItemIds = [];
        this.closeAddPredefinedItemsModal();
        this.refreshCheckpointDetails(true);
      },
      error: (err) => {
        this.addingChecklistItems = false;
        this.error = err.error?.message || err.message || 'Failed to add checklist items';
        console.error('Error adding checklist items:', err);
      },
      complete: () => {
        this.addingChecklistItems = false;
      }
    });
  }

  openAddPredefinedItemsModal(): void {
    // Navigate to the add predefined items page
    if (!this.wirCheckpoint) {
      console.error('No WIR checkpoint available');
      return;
    }

    // Construct the route to the add-checklist-items page
    const route = [
      '/projects',
      this.projectId,
      'boxes',
      this.boxId,
      'activities',
      this.activityId,
      'wir-checkpoints',
      this.wirCheckpoint.wirId,
      'add-checklist-items'
    ];

    this.router.navigate(route);
  }

  closeAddPredefinedItemsModal(): void {
    this.isAddPredefinedItemsModalOpen = false;
    this.selectedPredefinedItemIds = [];
    document.body.style.overflow = '';
  }

  togglePredefinedItemSelection(itemId: string): void {
    const index = this.selectedPredefinedItemIds.indexOf(itemId);
    if (index > -1) {
      this.selectedPredefinedItemIds.splice(index, 1);
    } else {
      this.selectedPredefinedItemIds.push(itemId);
    }
  }

  isPredefinedItemSelected(itemId: string): boolean {
    return this.selectedPredefinedItemIds.includes(itemId);
  }

  private loadChecklistItems(): void {
    if (!this.wirCheckpoint) {
      console.warn('loadChecklistItems: wirCheckpoint is null');
      return;
    }

    // Clear existing items
    this.checklistItems.clear();
    
    console.log('loadChecklistItems: checkpoint items', this.wirCheckpoint.checklistItems);
    
    if (this.wirCheckpoint.checklistItems && this.wirCheckpoint.checklistItems.length > 0) {
      // Sort items by sequence before loading
      const sortedItems = [...this.wirCheckpoint.checklistItems].sort((a, b) => (a.sequence || 0) - (b.sequence || 0));
      
      // Load existing checklist items from checkpoint
      sortedItems.forEach(item => {
        const itemGroup = this.fb.group({
          checklistItemId: [item.checklistItemId, Validators.required],
          sequence: [item.sequence],
          checkpointDescription: [item.checkpointDescription || ''],
          referenceDocument: [item.referenceDocument || ''],
          status: [this.mapCheckListItemStatus(item.status), Validators.required],
          remarks: [item.remarks || ''],
          checklistName: [item.checklistName || ''],
          sectionName: [item.sectionName || '']
        });
        this.checklistItems.push(itemGroup);
      });
      
      console.log('loadChecklistItems: loaded', this.checklistItems.length, 'items');
    } else {
      console.warn('loadChecklistItems: No checklist items found in checkpoint');
    }
  }

  private reloadCheckpointData(): void {
    if (!this.wirCheckpoint) return;
    
    console.log('Reloading checkpoint data...');
    this.loading = true;
    
    this.wirService.getWIRCheckpointById(this.wirCheckpoint.wirId).subscribe({
      next: (checkpoint: WIRCheckpoint) => {
        console.log('Checkpoint reloaded successfully:', checkpoint);
        this.wirCheckpoint = checkpoint;
        this.syncReviewFormFromCheckpoint();
        this.loading = false;
        this.shouldRefreshCheckpoint = false;
        
        // Build the form with the updated items
        const existingItems = checkpoint.checklistItems && checkpoint.checklistItems.length > 0
          ? checkpoint.checklistItems
          : undefined;
        
        console.log('Building form with reloaded items:', existingItems?.length || 0);
        this.buildAddChecklistItemsForm(existingItems);
        
        this.currentStep = 'add-items';
        this.pendingAction = null;
      },
      error: (err: any) => {
        console.error('Error reloading checkpoint:', err);
        this.loading = false;
        this.shouldRefreshCheckpoint = false;
        this.error = 'Failed to reload checkpoint data';
      }
    });
  }

  startAddChecklistFlow(): void {
    if (!this.wirCheckpoint) {
      return;
    }

    if (this.isChecklistLocked) {
      this.notifyChecklistLocked();
      return;
    }

    // Debug: Log checkpoint state
    console.log('startAddChecklistFlow - checkpoint:', this.wirCheckpoint);
    console.log('startAddChecklistFlow - checklistItems:', this.wirCheckpoint.checklistItems);
    
    // If we need to refresh, reload the checkpoint data
    if (this.shouldRefreshCheckpoint) {
      console.log('Refreshing checkpoint data after adding items');
      this.reloadCheckpointData();
      return;
    }
    
    // Build the form with existing items from checkpoint
    const existingItems = this.wirCheckpoint.checklistItems && this.wirCheckpoint.checklistItems.length > 0
      ? this.wirCheckpoint.checklistItems
      : undefined;
    
    console.log('Building checklist form with items:', existingItems?.length || 0, existingItems);
    this.buildAddChecklistItemsForm(existingItems);
    
    this.currentStep = 'add-items';
    this.pendingAction = null;
  }

  startReviewFlow(): void {
    if (!this.wirCheckpoint || !this.hasChecklistItems) {
      return;
    }
    // Check if user has permission to review WIR checkpoints
    if (!this.permissionService.hasPermission('wir', 'review')) {
      this.error = 'You do not have permission to review WIR checkpoints.';
      return;
    }
    this.currentStep = 'review';
    this.pendingAction = null;
    this.loadChecklistItems();
    this.setInitialFinalStatus();
  }

  startQualityIssuesFlow(): void {
    if (!this.wirCheckpoint) {
      return;
    }
    this.currentStep = 'quality-issues';
    this.pendingAction = null;
    this.resetQualityIssuesForm();
  }

  private createChecklistItemGroup(item?: Partial<{ 
    checklistItemId?: string; 
    checkpointDescription: string; 
    referenceDocument?: string; 
    sequence?: number; 
    predefinedItemId?: string; 
    categoryName?: string; 
    sectionName?: string;
    sectionId?: string;
    sectionOrder?: number;
    checklistName?: string;
    checklistId?: string;
    checklistCode?: string;
  }>): FormGroup {
    return this.fb.group({
      checklistItemId: [item?.checklistItemId || null],
      checkpointDescription: [item?.checkpointDescription || '', Validators.required],
      referenceDocument: [item?.referenceDocument || ''],
      sequence: [item?.sequence || 1, [Validators.required, Validators.min(1)]],
      predefinedItemId: [item?.predefinedItemId || null],
      categoryName: [item?.categoryName || 'General'],
      sectionName: [item?.sectionName || undefined],
      sectionId: [item?.sectionId || undefined],
      sectionOrder: [item?.sectionOrder || undefined],
      checklistName: [item?.checklistName || undefined],
      checklistId: [item?.checklistId || undefined],
      checklistCode: [item?.checklistCode || undefined]
    });
  }

  addChecklistItemFromModal(): void {
    if (this.isChecklistLocked) {
      this.notifyChecklistLocked();
      return;
    }

    if (this.newChecklistForm.invalid) {
      this.newChecklistForm.markAllAsTouched();
      return;
    }

    const formValue = this.newChecklistForm.value;
    this.addChecklistItemsArray.push(
      this.createChecklistItemGroup({
        checkpointDescription: formValue.checkpointDescription,
        referenceDocument: formValue.referenceDocument,
        sequence: this.addChecklistItemsArray.length + 1
      })
    );

    this.updateAddChecklistSequences();
    this.newChecklistForm.reset();
    this.closeChecklistModal();
  }

  updateChecklistItem(index: number): void {
    if (!this.wirCheckpoint) return;
    
    const itemControl = this.checklistItems.at(index);
    if (!itemControl || itemControl.invalid) {
      this.markFormGroupTouched(itemControl as FormGroup);
      return;
    }

    const itemValue = itemControl.value;
    const checklistItemId = itemValue.checklistItemId;
    
    if (!checklistItemId) {
      this.error = 'Checklist item ID is missing';
      return;
    }

    const request: UpdateChecklistItemRequest = {
      checklistItemId: checklistItemId,
      checkpointDescription: itemValue.checkpointDescription?.trim(),
      referenceDocument: itemValue.referenceDocument?.trim() || undefined,
      status: this.mapToCheckListItemStatus(itemValue.status),
      remarks: itemValue.remarks?.trim() || undefined,
      sequence: itemValue.sequence
    };

    this.wirService.updateChecklistItem(request).subscribe({
      next: () => {
        this.refreshCheckpointDetails(true);
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Checklist item updated successfully', type: 'success' }
        }));
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to update checklist item';
        console.error('Error updating checklist item:', err);
      }
    });
  }

  deleteChecklistItem(checklistItemId: string, stayOnCurrentStep = false): void {
    if (!checklistItemId) return;

    this.wirService.deleteChecklistItem(checklistItemId).subscribe({
      next: () => {
        // Refresh checkpoint but stay on current step if requested
        this.refreshCheckpointDetails(false, stayOnCurrentStep);
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Checklist item deleted successfully', type: 'success' }
        }));
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to delete checklist item';
        console.error('Error deleting checklist item:', err);
      }
    });
  }

  removeChecklistItemRow(index: number): void {
    this.addChecklistItemsArray.removeAt(index);
    this.updateAddChecklistSequences();
  }

  confirmRemoveChecklistItem(index: number): void {
    if (this.isChecklistLocked) {
      this.notifyChecklistLocked();
      return;
    }
    const itemControl = this.addChecklistItemsArray.at(index);
    const checklistItemId = itemControl?.value?.checklistItemId || null;
    this.pendingDeleteIndex = index;
    this.pendingDeleteChecklistItemId = checklistItemId;
    this.isDeleteModalOpen = true;
  }

  private updateAddChecklistSequences(): void {
    this.addChecklistItemsArray.controls.forEach((control, idx) => {
      control.get('sequence')?.setValue(idx + 1, { emitEvent: false });
    });
  }

  cancelDeleteChecklistItem(): void {
    this.pendingDeleteIndex = null;
    this.pendingDeleteChecklistItemId = null;
    this.isDeleteModalOpen = false;
  }

  confirmDeleteChecklistItem(): void {
    if (this.pendingDeleteIndex !== null) {
      const checklistItemId = this.pendingDeleteChecklistItemId;
      if (checklistItemId) {
        // Item exists in backend, delete via API
        // Pass stayOnCurrentStep=true to keep user on add-items step
        this.deleteChecklistItem(checklistItemId, true);
      } else {
        // New item not yet saved, just remove from form
        this.removeChecklistItemRow(this.pendingDeleteIndex);
        const currentCheckpoint = this.wirCheckpoint;
        if (currentCheckpoint && currentCheckpoint.checklistItems) {
          this.wirCheckpoint = {
            ...currentCheckpoint,
            checklistItems: currentCheckpoint.checklistItems.filter((_, idx) => idx !== this.pendingDeleteIndex)
          };
        }
      }
    }
    this.cancelDeleteChecklistItem();
  }

  private refreshCheckpointDetails(resetToOverview = false, stayOnCurrentStep = false): void {
    if (!this.wirCheckpoint?.wirId) {
      return;
    }

    this.wirService.getWIRCheckpointById(this.wirCheckpoint.wirId).subscribe({
      next: (checkpoint) => {
        this.wirCheckpoint = checkpoint;
        this.syncReviewFormFromCheckpoint();
        this.buildAddChecklistItemsForm(checkpoint.checklistItems || []);
        
        // Reload quality issues into the form array to reflect any changes
        this.resetQualityIssuesForm();
        
        // If we're on the review step, reload checklist items
        if (this.currentStep === 'review') {
          this.loadChecklistItems();
        }
        
        if (stayOnCurrentStep) {
          // Stay on current step (e.g., add-items) after refresh
          // Don't change currentStep or pendingAction
        } else if (resetToOverview) {
          this.pendingAction = 'review';
          this.currentStep = null;
          this.applyInitialStepFromQuery();
        } else {
          this.applyInitialStepFromQuery();
        }
      },
      error: (err) => {
        console.error('Error refreshing WIR checkpoint:', err);
      }
    });
  }

  openChecklistModal(): void {
    if (this.isChecklistLocked) {
      this.notifyChecklistLocked();
      return;
    }
    this.isChecklistModalOpen = true;
    this.newChecklistForm.reset();
  }

  closeChecklistModal(): void {
    this.isChecklistModalOpen = false;
  }

  openQualityIssueModal(): void {
    this.newQualityIssueForm.reset({
      issueDescription: '',
      severity: this.severityLevels[0],
      issueType: this.issueTypes[0],
      assignedTo: '',
      dueDate: ''
    });
    this.qualityIssueImages = [];
    this.qualityIssueCurrentUrlInput = '';
    this.showQualityIssueCamera = false;
    this.qualityIssuePhotoInputMethod = 'upload';
    this.stopQualityIssueCamera();
    this.isQualityIssueModalOpen = true;
    this.loadAvailableTeams();
  }

  closeQualityIssueModal(): void {
    this.isQualityIssueModalOpen = false;
  }

  openItemReviewModal(itemIndex: number): void {
    if (this.isChecklistLocked || itemIndex < 0 || itemIndex >= this.checklistItems.length) {
      return;
    }
    this.selectedReviewItemIndex = itemIndex;
    const item = this.checklistItems.at(itemIndex);
    const itemValue = item?.value;
    
    // Load current values into the review form
    this.itemReviewForm.patchValue({
      status: itemValue?.status || CheckpointStatus.Pending,
      referenceDocument: itemValue?.referenceDocument || '',
      remarks: itemValue?.remarks || ''
    });
    
    this.isItemReviewModalOpen = true;
    document.body.style.overflow = 'hidden';
  }

  closeItemReviewModal(): void {
    this.isItemReviewModalOpen = false;
    this.selectedReviewItemIndex = null;
    this.itemReviewForm.reset();
    this.error = '';
    document.body.style.overflow = '';
  }

  isItemReviewFormInvalid(): boolean {
    // Check if status is set and is not Pending
    const status = this.itemReviewForm.get('status')?.value;
    
    // Status must be Pass or Fail (not Pending or empty)
    if (!status || status === CheckpointStatus.Pending) {
      return true;
    }

    // If status is Fail, remarks is required
    if (status === CheckpointStatus.Fail) {
      const remarks = this.itemReviewForm.get('remarks')?.value?.trim();
      if (!remarks) {
        return true;
      }
    }

    // Check other form validations
    if (this.itemReviewForm.invalid) {
      return true;
    }

    return false;
  }

  saveItemReview(): void {
    if (this.itemReviewForm.invalid || this.selectedReviewItemIndex === null) {
      this.itemReviewForm.markAllAsTouched();
      return;
    }

    const formValue = this.itemReviewForm.value;
    const itemControl = this.checklistItems.at(this.selectedReviewItemIndex);
    
    if (!itemControl) {
      this.error = 'Checklist item not found';
      return;
    }

    const itemValue = itemControl.value;
    const checklistItemId = itemValue.checklistItemId;
    
    if (!checklistItemId) {
      this.error = 'Checklist item ID is missing';
      return;
    }

    // Validate remarks if status is Fail
    if (formValue.status === CheckpointStatus.Fail && !formValue.remarks?.trim()) {
      this.error = 'Remarks are required when status is Fail';
      this.itemReviewForm.get('remarks')?.markAsTouched();
      return;
    }

    this.savingItemReview = true;
    this.error = '';

    const request: UpdateChecklistItemRequest = {
      checklistItemId: checklistItemId,
      checkpointDescription: itemValue.checkpointDescription?.trim(),
      referenceDocument: formValue.referenceDocument?.trim() || undefined,
      status: this.mapToCheckListItemStatus(formValue.status),
      remarks: formValue.remarks?.trim() || undefined,
      sequence: itemValue.sequence
    };

    this.wirService.updateChecklistItem(request).subscribe({
      next: () => {
        this.savingItemReview = false;
        this.closeItemReviewModal();
        // Refresh the checkpoint data to update the UI
        this.refreshCheckpointDetails(false, true);
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Review saved successfully', type: 'success' }
        }));
      },
      error: (err) => {
        this.savingItemReview = false;
        this.error = err.error?.message || err.message || 'Failed to save review';
        console.error('Error saving review:', err);
      }
    });
  }

  savingQualityIssue: boolean = false; // Loading state for single issue submission

  addQualityIssueFromModal(): void {
    if (this.newQualityIssueForm.invalid) {
      this.newQualityIssueForm.markAllAsTouched();
      return;
    }

    if (!this.wirCheckpoint) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { message: 'WIR checkpoint not found.', type: 'error' }
      }));
      return;
    }

    const value = this.newQualityIssueForm.value;
    
    // Separate files from URLs
    const files: File[] = [];
    const imageUrls: string[] = [];
    
    this.qualityIssueImages.forEach(img => {
      if (img.type === 'url' && img.url) {
        // URL type - add to imageUrls
        imageUrls.push(img.url.trim());
      } else if ((img.type === 'file' || img.type === 'camera') && img.file) {
        // File/Camera type - add to files array
        files.push(img.file);
      } else if (img.preview && img.preview.startsWith('data:image/')) {
        // Base64 data URL from camera/file - add to imageUrls
        imageUrls.push(img.preview);
      }
    });
    
    // Build request for single issue
    const request: AddQualityIssueRequest = {
      wirId: this.wirCheckpoint.wirId,
      issueType: value.issueType,
      severity: value.severity,
      issueDescription: value.issueDescription?.trim() || '',
      assignedTo: value.assignedTo?.trim() || undefined,
      dueDate: value.dueDate || undefined,
      imageUrls: imageUrls.length > 0 ? imageUrls : undefined,
      files: files.length > 0 ? files : undefined
    };

    this.savingQualityIssue = true;

    this.wirService.addQualityIssue(request).subscribe({
      next: (updatedCheckpoint) => {
        this.wirCheckpoint = updatedCheckpoint;
        
        // Reload quality issues into the form array to show the newly added issue
        this.resetQualityIssuesForm();
        
        // Refresh checkpoint details to ensure all data is up to date
        this.refreshCheckpointDetails();
        
        // Clear the form and close modal
        this.newQualityIssueForm.reset();
        this.qualityIssueImages = [];
        this.qualityIssueCurrentUrlInput = '';
        this.stopQualityIssueCamera();
        this.closeQualityIssueModal();
        
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Quality issue added successfully.', type: 'success' }
        }));
        
        this.savingQualityIssue = false;
      },
      error: (err) => {
        console.error('Failed to add quality issue:', err);
        this.savingQualityIssue = false;
        
        const errorMessage = err.error?.message || err.message || 'Failed to add quality issue. Please try again.';
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: errorMessage, type: 'error' }
        }));
      }
    });
  }

  loadAvailableTeams(): void {
    this.loadingTeams = true;
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        // Filter to only active teams that user has access to
        // The backend already filters based on user permissions
        this.availableTeams = teams.filter(team => team.isActive);
        this.loadingTeams = false;
      },
      error: (err) => {
        console.error('❌ Error loading teams:', err);
        this.availableTeams = [];
        this.loadingTeams = false;
      }
    });
  }


  closeWorkflowStep(): void {
    this.currentStep = null;
  }

  get hasChecklistItems(): boolean {
    return !!this.wirCheckpoint?.checklistItems && this.wirCheckpoint.checklistItems.length > 0;
  }

  get isChecklistLocked(): boolean {
    const status = this.wirCheckpoint?.status as WIRCheckpointStatus | undefined;
    return !!status && status !== WIRCheckpointStatus.Pending;
  }

  /**
   * Check if Step 1 (Create Checkpoint) is completed
   */
  isStep1Completed(): boolean {
    return !!this.wirCheckpoint;
  }

  /**
   * Check if Step 2 (Add Checklist Items) is completed
   */
  isStep2Completed(): boolean {
    return this.hasChecklistItems;
  }

  /**
   * Check if Step 3 (Review & Sign-off) is completed
   */
  isStep3Completed(): boolean {
    if (!this.wirCheckpoint) return false;
    const status = this.wirCheckpoint.status as WIRCheckpointStatus | undefined;
    return !!status && status !== WIRCheckpointStatus.Pending;
  }

  /**
   * Check if a specific step is completed based on backend status
   */
  isStepCompletedByStatus(step: ReviewStep): boolean {
    switch (step) {
      case 'create-checkpoint':
        return this.isStep1Completed();
      case 'add-items':
        return this.isStep2Completed();
      case 'review':
        return this.isStep3Completed();
      case 'quality-issues':
        return this.isStep3Completed(); // Quality issues can only be accessed after review
      default:
        return false;
    }
  }

  /**
   * Get the next accessible step based on completion status
   */
  getNextAccessibleStep(): ReviewStep | null {
    if (!this.isStep1Completed()) {
      return 'create-checkpoint';
    }
    if (!this.isStep2Completed()) {
      return 'add-items';
    }
    if (!this.isStep3Completed()) {
      return 'review';
    }
    return 'quality-issues';
  }

  private setInitialFinalStatus(): void {
    const currentStatus = (this.wirCheckpoint?.status as WIRCheckpointStatus) || WIRCheckpointStatus.Pending;
    const defaultStatus = currentStatus === WIRCheckpointStatus.Approved ||
      currentStatus === WIRCheckpointStatus.ConditionalApproval ||
      currentStatus === WIRCheckpointStatus.Rejected
      ? currentStatus
      : WIRCheckpointStatus.Approved;

    this.finalStatusControl.setValue(defaultStatus, { emitEvent: false });
    this.handleFinalStatusChange(defaultStatus);
  }

  private resetQualityIssuesForm(): void {
    this.qualityIssuesArray.clear();
    this.qualityIssueFilesMap.clear();
    const existing = this.existingQualityIssues;
    if (existing && existing.length > 0) {
      existing.forEach(issue => {
        this.addQualityIssueRow({
          issueDescription: issue.issueDescription,
          severity: (issue.severity as SeverityType) || this.severityLevels[0],
          issueType: (issue.issueType as IssueType) || this.issueTypes[0],
          assignedTo: issue.assignedTo, // Team ID for form
          assignedTeam: issue.assignedTeam, // Team name for display
          dueDate: issue.dueDate ? this.toDateInputValue(issue.dueDate) : '',
          photoPath: issue.photoPath
        });
      });
    }
    // Don't add empty row - only show existing quality issues
  }

  // Store files separately for each issue (indexed by form array index)
  private qualityIssueFilesMap: Map<number, File[]> = new Map();

  private addQualityIssueRow(issue?: Partial<QualityIssueFormValue>): void {
    const formGroup = this.fb.group({
      issueType: [issue?.issueType || this.issueTypes[0], Validators.required],
      severity: [issue?.severity || this.severityLevels[0], Validators.required],
      issueDescription: [issue?.issueDescription || '', [Validators.required, Validators.maxLength(500)]],
      assignedTo: [issue?.assignedTo || '', [Validators.maxLength(200)]],
      assignedTeam: [issue?.assignedTeam || ''], // Team name for display
      dueDate: [issue?.dueDate || ''],
      photoPath: [issue?.photoPath || '', [Validators.maxLength(500)]],
      reportedBy: [issue?.reportedBy || null],
      issueDate: [issue?.issueDate || null],
      imageDataUrls: [issue?.imageDataUrls || []]
    });
    
    const index = this.qualityIssuesArray.length;
    this.qualityIssuesArray.push(formGroup);
    
    // Store files for this issue if they exist
    if (issue?.files && issue.files.length > 0) {
      this.qualityIssueFilesMap.set(index, issue.files);
    }
  }

  viewQualityIssueDetails(index: number): void {
    const issue = this.getExistingQualityIssue(index);
    const formIssue = this.qualityIssuesArray.at(index);
    
    if (!issue && !formIssue) {
      return;
    }

    // Debug logging
    console.log('[QA/QC] Viewing quality issue details:', issue);
    console.log('[QA/QC] Issue images property:', (issue as any)?.images);

    // Get images from the issue
    let images: string[] = [];
    
    // Get images from existing issue (if it has images property - QualityIssueDetails or QualityIssueItem with images)
    if (issue) {
      const issueDetails = issue as QualityIssueDetails & { images?: QualityIssueImage[] };
      
      // First, check for images array (QualityIssueImage[])
      if (issueDetails.images && Array.isArray(issueDetails.images) && issueDetails.images.length > 0) {
        console.log('[QA/QC] Found images array with', issueDetails.images.length, 'images');
        images = issueDetails.images
          .sort((a, b) => (a.sequence || 0) - (b.sequence || 0))
          .map((img: QualityIssueImage) => {
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
            return `data:image/jpeg;base64,${imageData}`;
          })
          .filter((url: string) => url && url.trim().length > 0);
      }
      
      // Also check imageDataUrls if available (for backward compatibility)
      if (issue.imageDataUrls && Array.isArray(issue.imageDataUrls) && issue.imageDataUrls.length > 0) {
        const urlImages = issue.imageDataUrls
          .filter((url: string) => url && url.trim().length > 0)
          .map((url: string) => {
            // If it's already a data URL, return as is
            if (url.startsWith('data:image/')) {
              return url;
            }
            // If it's a URL, return as is
            if (url.startsWith('http://') || url.startsWith('https://')) {
              return url;
            }
            // Otherwise, assume it's base64 and add data URI prefix
            return `data:image/jpeg;base64,${url}`;
          });
        images = [...images, ...urlImages];
      }
      
      // Also check photoPath (legacy field)
      if (issue.photoPath && issue.photoPath.trim().length > 0) {
        const photoPath = issue.photoPath.trim();
        if (photoPath.startsWith('data:image/') || photoPath.startsWith('http://') || photoPath.startsWith('https://')) {
          images.push(photoPath);
        } else {
          images.push(`data:image/jpeg;base64,${photoPath}`);
        }
      }
    }
    
    // Also check imageDataUrls from form (for issues being edited)
    if (formIssue) {
      const formImageUrls = formIssue.get('imageDataUrls')?.value || [];
      if (Array.isArray(formImageUrls) && formImageUrls.length > 0) {
        const formUrls = formImageUrls
          .filter((url: string) => url && url.trim().length > 0)
          .map((url: string) => {
            // If it's already a data URL, return as is
            if (url.startsWith('data:image/')) {
              return url;
            }
            // If it's a URL, return as is
            if (url.startsWith('http://') || url.startsWith('https://')) {
              return url;
            }
            // Otherwise, assume it's base64 and add data URI prefix
            return `data:image/jpeg;base64,${url}`;
          });
        images = [...images, ...formUrls];
      }
    }

    // Remove duplicates
    images = [...new Set(images)];

    console.log('[QA/QC] Total images extracted:', images.length);
    console.log('[QA/QC] Image URLs:', images);

    // Store selected issue details
    this.selectedQualityIssueDetails = {
      issue: issue,
      formIssue: formIssue,
      images: images.length > 0 ? images : undefined
    };

    // Open the modal
    this.isQualityIssueDetailsModalOpen = true;
  }

  closeQualityIssueDetailsModal(): void {
    this.isQualityIssueDetailsModalOpen = false;
    this.selectedQualityIssueDetails = null;
  }

  openImageInNewTab(imageUrl: string): void {
    window.open(imageUrl, '_blank');
  }
  
  getQualityIssueStatusLabel(status?: QualityIssueStatus | string): string {
    const normalized = (status || 'Open') as QualityIssueStatus;
    return this.qualityIssueStatusMeta[normalized]?.label || 'OPEN';
  }

  getQualityIssueStatusClass(status?: QualityIssueStatus | string): string {
    const normalized = (status || 'Open') as QualityIssueStatus;
    return this.qualityIssueStatusMeta[normalized]?.class || 'status-open';
  }
  
  formatIssueDate(date?: string | Date): string {
    if (!date) return '—';
    const d = date instanceof Date ? date : new Date(date);
    if (isNaN(d.getTime())) return '—';
    return d.toISOString().split('T')[0];
  }
  
  getQualityIssueImageUrls(): string[] {
    if (!this.selectedQualityIssueDetails) return [];
    
    const images = this.selectedQualityIssueDetails.images || [];
    
    // Process image URLs - handle base64 data URLs and regular URLs
    return images
      .map((img: string) => {
        // If it's already a data URL, return as is
        if (img.startsWith('data:image/')) {
          return img;
        }
        // If it's a URL, return as is
        if (img.startsWith('http://') || img.startsWith('https://')) {
          return img;
        }
        // Otherwise, assume it's base64 and add data URI prefix
        return `data:image/jpeg;base64,${img}`;
      })
      .filter((url: string) => url && url.trim().length > 0);
  }
  
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
  
  getCurrentLightboxImage(): string {
    if (this.lightboxImages.length === 0) return '';
    return this.lightboxImages[this.lightboxImageIndex] || this.lightboxImages[0];
  }
  
  previousImage(): void {
    if (this.lightboxImages.length === 0) return;
    this.lightboxImageIndex = (this.lightboxImageIndex - 1 + this.lightboxImages.length) % this.lightboxImages.length;
  }
  
  nextImage(): void {
    if (this.lightboxImages.length === 0) return;
    this.lightboxImageIndex = (this.lightboxImageIndex + 1) % this.lightboxImages.length;
  }
  
  onImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.src = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjAwIiBoZWlnaHQ9IjIwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iMjAwIiBoZWlnaHQ9IjIwMCIgZmlsbD0iI2VlZSIvPjx0ZXh0IHg9IjUwJSIgeT0iNTAlIiBmb250LWZhbWlseT0iQXJpYWwiIGZvbnQtc2l6ZT0iMTQiIGZpbGw9IiM5OTkiIHRleHQtYW5jaG9yPSJtaWRkbGUiIGR5PSIuM2VtIj5JbWFnZSBub3QgYXZhaWxhYmxlPC90ZXh0Pjwvc3ZnPg==';
  }

  getQualityIssueDetail(field: string): any {
    if (!this.selectedQualityIssueDetails) return '—';
    
    const { issue, formIssue } = this.selectedQualityIssueDetails;
    
    // Try form issue first, then existing issue
    if (formIssue) {
      const value = formIssue.get(field)?.value;
      if (value !== null && value !== undefined && value !== '') {
        return value;
      }
    }
    
    if (issue) {
      const value = (issue as any)[field];
      if (value !== null && value !== undefined && value !== '') {
        return value;
      }
    }
    
    return '—';
  }

  removeQualityIssue(index: number): void {
    if (!confirm('Are you sure you want to delete this quality issue?')) {
      return;
    }
    
    this.qualityIssueFilesMap.delete(index);
    // Reindex the map after removal
    const newMap = new Map<number, File[]>();
    this.qualityIssueFilesMap.forEach((files, oldIndex) => {
      if (oldIndex < index) {
        newMap.set(oldIndex, files);
      } else if (oldIndex > index) {
        newMap.set(oldIndex - 1, files);
      }
    });
    this.qualityIssueFilesMap = newMap;
    this.qualityIssuesArray.removeAt(index);
  }

  private notifyChecklistLocked(): void {
    document.dispatchEvent(new CustomEvent('app-toast', {
      detail: {
        message: 'This checkpoint has already been reviewed. Checklist items can no longer be modified.',
        type: 'warning'
      }
    }));
  }

  private syncReviewFormFromCheckpoint(): void {
    const notes = this.wirCheckpoint?.comments || '';
    this.checklistForm.get('inspectionNotes')?.setValue(notes, { emitEvent: false });

    const role = this.wirCheckpoint?.inspectorRole || '';
    this.checklistForm.get('inspectorRole')?.setValue(role, { emitEvent: false });
  }

  private toDateInputValue(date: string | Date): string {
    const d = date instanceof Date ? date : new Date(date);
    if (isNaN(d.getTime())) {
      return '';
    }
    return d.toISOString().split('T')[0];
  }

  getExistingQualityIssue(index: number): QualityIssueItem | undefined {
    const issues = this.existingQualityIssues;
    return index >= 0 && index < issues.length ? issues[index] : undefined;
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

  formatShortDate(date?: string | Date): string {
    if (!date) {
      return '—';
    }
    const parsed = date instanceof Date ? date : new Date(date);
    if (isNaN(parsed.getTime())) {
      return '—';
    }
    const month = parsed.toLocaleDateString('en-US', { month: 'short' });
    const day = parsed.getDate();
    return `${month} ${day}`;
  }

  /**
   * Check if user can navigate to a specific step (enforces sequential workflow)
   */
  canNavigateToStep(step: ReviewStep): boolean {
    const stepIndex = this.getStepIndex(step);
    
    // Step 1: Accessible if checkpoint doesn't exist OR if checkpoint status is still Pending
    // Once reviewed (Approved/Rejected/ConditionalApproval), user cannot go back to Step 1
    if (step === 'create-checkpoint') {
      return !this.wirCheckpoint || this.wirCheckpoint.status === 'Pending';
    }
    
    // Step 2: Only accessible if Step 1 is completed
    if (step === 'add-items') {
      return this.isStep1Completed() && !this.isChecklistLocked;
    }
    
    // Step 3: Only accessible if Step 2 is completed AND user has review permission
    if (step === 'review') {
      return this.isStep2Completed() && this.canReviewWIR;
    }
    
    // Step 4: Only accessible if Step 3 is completed
    if (step === 'quality-issues') {
      return this.isStep3Completed();
    }
    
    return false;
  }

  handleStepClick(step: ReviewStep): void {
    // Strict validation: prevent navigation if step is not accessible
    if (!this.canNavigateToStep(step)) {
      console.warn(`Cannot navigate to step ${step}. Previous steps must be completed.`);
      // Optionally show a user-friendly message
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: {
          message: `Please complete previous steps before accessing ${this.stepMeta.find(s => s.id === step)?.title || step}`,
          type: 'warning'
        }
      }));
      return;
    }

    switch (step) {
      case 'create-checkpoint':
        this.currentStep = 'create-checkpoint';
        this.pendingAction = null;
        // If checkpoint exists and is Pending, populate the form with existing data
        if (this.wirCheckpoint && this.wirCheckpoint.status === 'Pending') {
          this.createCheckpointForm.patchValue({
            wirName: this.wirCheckpoint.wirName || '',
            wirDescription: this.wirCheckpoint.wirDescription || '',
            inspectorName: this.wirCheckpoint.inspectorName || '',
            inspectorRole: this.wirCheckpoint.inspectorRole || ''
          });
          
          // Load existing attachment path into images array if it exists
          if (this.wirCheckpoint.attachmentPath) {
            if (this.wirCheckpoint.attachmentPath.startsWith('http://') || this.wirCheckpoint.attachmentPath.startsWith('https://')) {
              this.attachmentImages = [{
                type: 'url',
                url: this.wirCheckpoint.attachmentPath,
                preview: this.wirCheckpoint.attachmentPath
              }];
            } else {
              this.attachmentImages = [{
                type: 'url',
                url: this.wirCheckpoint.attachmentPath,
                preview: this.wirCheckpoint.attachmentPath
              }];
            }
          }
        }
        break;
      case 'add-items':
        this.startAddChecklistFlow();
        break;
      case 'review':
        this.startReviewFlow();
        break;
      case 'quality-issues':
        this.startQualityIssuesFlow();
        break;
    }
  }

  private applyInitialStepFromQuery(): void {
    if (this.initialStepApplied || !this.initialStepFromQuery) {
      return;
    }

    // Validate that the requested step is accessible before allowing navigation
    if (!this.canNavigateToStep(this.initialStepFromQuery)) {
      // If step is not accessible, redirect to the next accessible step
      const nextStep = this.getNextAccessibleStep();
      if (nextStep) {
        console.warn(`Step ${this.initialStepFromQuery} is not accessible. Redirecting to ${nextStep}`);
        this.router.navigate([], {
          relativeTo: this.route,
          queryParams: { step: nextStep },
          queryParamsHandling: 'merge'
        });
        // Set the step after navigation
        if (nextStep === 'create-checkpoint') {
          this.currentStep = 'create-checkpoint';
          this.pendingAction = null;
        } else {
          this.handleStepClick(nextStep);
        }
      }
      this.initialStepApplied = true;
      return;
    }

    // Step is accessible, proceed with navigation
    switch (this.initialStepFromQuery) {
      case 'quality-issues':
        if (this.wirCheckpoint) {
          this.startQualityIssuesFlow();
          this.initialStepApplied = true;
        }
        break;
      case 'create-checkpoint':
        // Allow if checkpoint doesn't exist OR if checkpoint is still Pending
        if (!this.wirCheckpoint || this.wirCheckpoint.status === 'Pending') {
          this.currentStep = 'create-checkpoint';
          this.pendingAction = null;
          // If checkpoint exists, populate form with existing data
          if (this.wirCheckpoint) {
            this.createCheckpointForm.patchValue({
              wirName: this.wirCheckpoint.wirName || '',
              wirDescription: this.wirCheckpoint.wirDescription || '',
              inspectorName: this.wirCheckpoint.inspectorName || '',
              inspectorRole: this.wirCheckpoint.inspectorRole || ''
            });
            
            // Load existing attachment path into images array if it exists
            if (this.wirCheckpoint.attachmentPath) {
              if (this.wirCheckpoint.attachmentPath.startsWith('http://') || this.wirCheckpoint.attachmentPath.startsWith('https://')) {
                this.attachmentImages = [{
                  type: 'url',
                  url: this.wirCheckpoint.attachmentPath,
                  preview: this.wirCheckpoint.attachmentPath
                }];
              } else {
                this.attachmentImages = [{
                  type: 'url',
                  url: this.wirCheckpoint.attachmentPath,
                  preview: this.wirCheckpoint.attachmentPath
                }];
              }
            }
          }
          this.initialStepApplied = true;
        } else {
          // Checkpoint exists and is reviewed, redirect to next accessible step
          const nextStep = this.getNextAccessibleStep();
          if (nextStep) {
            this.handleStepClick(nextStep);
          }
          this.initialStepApplied = true;
        }
        break;
      case 'add-items':
        // Ensure checkpoint is loaded before starting flow
        if (this.wirCheckpoint) {
          this.startAddChecklistFlow();
        } else {
          // If checkpoint not loaded yet, wait for it
          console.warn('Checkpoint not loaded yet when trying to navigate to add-items');
        }
        this.initialStepApplied = true;
        break;
      case 'review':
        this.startReviewFlow();
        this.initialStepApplied = true;
        break;
    }
  }

  private loadBoxQualityIssues(): void {
    if (!this.boxId) {
      return;
    }

    this.boxIssuesLoading = true;
    this.boxIssuesError = '';
    this.wirService.getQualityIssuesByBox(this.boxId).subscribe({
      next: (issues) => {
        this.boxQualityIssues = issues || [];
        this.boxIssuesLoading = false;
      },
      error: (err) => {
        console.error('Failed to load quality issues for box:', err);
        this.boxIssuesError = err?.error?.message || err?.message || 'Failed to load quality issues';
        this.boxIssuesLoading = false;
      }
    });
  }

  exitQualityIssuesList(): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { view: null, step: null },
      queryParamsHandling: 'merge'
    });
    this.qualityIssuesOnlyView = false;
    this.initialStepFromQuery = null;
    this.initialStepApplied = false;
  }

  async exportQualityIssuesToExcel(): Promise<void> {
    if (this.boxQualityIssues.length === 0) {
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
    this.boxQualityIssues.forEach((issue, index) => {
      const row = worksheet.addRow([
        index + 1,
        issue.wirNumber || issue.wirName || '—',
        issue.boxTag || '—',
        issue.boxName || '—',
        issue.status || 'Open',
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

  get isRejectedSelected(): boolean {
    return this.finalStatusControl.value === WIRCheckpointStatus.Rejected;
  }

  get existingQualityIssues(): QualityIssueItem[] {
    return this.wirCheckpoint?.qualityIssues || [];
  }

  private applyRejectionValidation(): void {
    // No-op since backend does not track rejection reason
  }

  getStatusLabel(status: CheckpointStatus | string | undefined): string {
    if (!status) return 'Pending';
    const statusStr = typeof status === 'string' ? status : String(status);
    switch (statusStr) {
      case CheckpointStatus.Pass:
      case 'Pass':
        return 'Pass';
      case CheckpointStatus.Fail:
      case 'Fail':
        return 'Fail';
      case CheckpointStatus.Pending:
      case 'Pending':
        return 'Pending';
      default:
        return 'Pending';
    }
  }

  getStatusBadgeClass(status: CheckpointStatus | string | undefined): string {
    if (!status) return 'status-badge-pending';
    const statusStr = typeof status === 'string' ? status : String(status);
    switch (statusStr) {
      case CheckpointStatus.Pass:
      case 'Pass':
        return 'status-badge-pass';
      case CheckpointStatus.Fail:
      case 'Fail':
        return 'status-badge-fail';
      case CheckpointStatus.Pending:
      case 'Pending':
        return 'status-badge-pending';
      default:
        return 'status-badge-pending';
    }
  }

  getTotalItemsCount(sections: { sectionName: string; items: any[] }[]): number {
    return sections.reduce((acc, section) => acc + section.items.length, 0);
  }

  getGroupedChecklistItems(): { checklistName: string; sections: { sectionName: string; items: any[] }[] }[] {
    const itemsArray = this.addChecklistItemsArray;
    if (itemsArray.length === 0) {
      return [];
    }

    // Group by checklist -> section -> items
    const grouped = new Map<string, Map<string, any[]>>();

    itemsArray.controls.forEach((control, index) => {
      const formGroup = control as FormGroup;
      const checklistName = formGroup.get('checklistName')?.value || 'General Checklist';
      const sectionName = formGroup.get('sectionName')?.value || 'General Items';
      const checkpointDescription = formGroup.get('checkpointDescription')?.value;
      const sequence = formGroup.get('sequence')?.value;
      const referenceDocument = formGroup.get('referenceDocument')?.value;
      
      if (!grouped.has(checklistName)) {
        grouped.set(checklistName, new Map());
      }
      
      const checklistMap = grouped.get(checklistName)!;
      if (!checklistMap.has(sectionName)) {
        checklistMap.set(sectionName, []);
      }
      
      checklistMap.get(sectionName)!.push({
        control: formGroup,
        index: index,
        checkpointDescription: checkpointDescription,
        referenceDocument: referenceDocument,
        sequence: sequence,
        checklistName: checklistName,
        sectionName: sectionName
      });
    });

    // Sort checklists alphabetically
    const checklists = Array.from(grouped.keys()).sort((a, b) => {
      if (a === 'General Checklist') return -1;
      if (b === 'General Checklist') return 1;
      return a.localeCompare(b);
    });

    return checklists.map(checklist => {
      const checklistMap = grouped.get(checklist)!;
      const sections = Array.from(checklistMap.keys()).sort((a, b) => {
        if (a === 'General Items') return -1;
        if (b === 'General Items') return 1;
        return a.localeCompare(b);
      });

      return {
        checklistName: checklist,
        sections: sections.map(sectionName => ({
          sectionName: sectionName,
          items: checklistMap.get(sectionName)!.sort((a, b) => (a.sequence || 0) - (b.sequence || 0))
        }))
      };
    });
  }

  canReviewItem(status: CheckpointStatus | string | undefined): boolean {
    if (!status) return true; // Pending items can be reviewed
    const statusStr = typeof status === 'string' ? status : String(status);
    // Only allow review for Fail or Pending status
    return statusStr === CheckpointStatus.Fail || 
           statusStr === 'Fail' || 
           statusStr === CheckpointStatus.Pending || 
           statusStr === 'Pending';
  }

  getReviewButtonTitle(status: CheckpointStatus | string | undefined, isLocked: boolean): string {
    if (isLocked) {
      return 'Checklist is locked after review';
    }
    const statusStr = typeof status === 'string' ? status : String(status);
    if (statusStr === CheckpointStatus.Pass || statusStr === 'Pass') {
      return 'Cannot review passed items';
    }
    return 'Review item';
  }

  get addChecklistItemsArray(): FormArray {
    return this.addChecklistItemsForm.get('checklistItems') as FormArray;
  }

  /**
   * Get the category for a checklist item by looking it up from predefined items
   */
  getCategoryForChecklistItem(item: any): string {
    const predefinedItemId = item.get('predefinedItemId')?.value;
    if (!predefinedItemId) {
      return 'Other';
    }
    
    const predefinedItem = this.predefinedChecklistItems.find(
      p => p.predefinedItemId === predefinedItemId
    );
    
    return predefinedItem?.category || 'Other';
  }

  /**
   * Group checklist items by category for display in the table
   */
  getGroupedAddChecklistItems(): { category: string; items: { index: number; control: any }[] }[] {
    const grouped = new Map<string, { index: number; control: any }[]>();
    const categoryOrder = ['General', 'Setting Out', 'Installation Activity'];
    
    // Group items by category
    this.addChecklistItemsArray.controls.forEach((control, index) => {
      const category = this.getCategoryForChecklistItem(control);
      if (!grouped.has(category)) {
        grouped.set(category, []);
      }
      grouped.get(category)!.push({ index, control });
    });

    // Convert to array and sort by predefined category order
    const result: { category: string; items: { index: number; control: any }[] }[] = [];
    
    // Add categories in order
    categoryOrder.forEach(cat => {
      if (grouped.has(cat)) {
        result.push({
          category: cat,
          items: grouped.get(cat)!
        });
      }
    });

    // Add any remaining categories
    grouped.forEach((items, category) => {
      if (!categoryOrder.includes(category)) {
        result.push({
          category,
          items
        });
      }
    });

    return result;
  }

  private mapCheckListItemStatus(status: string): CheckpointStatus {
    const statusMap: Record<string, CheckpointStatus> = {
      'Pending': CheckpointStatus.Pending,
      'Pass': CheckpointStatus.Pass,
      'Fail': CheckpointStatus.Fail
    };
    return statusMap[status] || CheckpointStatus.Pending;
  }

  onCheckpointStatusChange(index: number, status: CheckpointStatus): void {
    const item = this.checklistItems.at(index);
    item.patchValue({ status });
    
    // If marked as Fail, make remarks required
    if (status === CheckpointStatus.Fail) {
      item.get('remarks')?.setValidators([Validators.required]);
    } else {
      item.get('remarks')?.clearValidators();
    }
    item.get('remarks')?.updateValueAndValidity();
  }

  // Photo Upload Methods (Enhanced - Multiple Images Support)
  onPhotosSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const files = Array.from(input.files);
      const imageFiles: File[] = [];
      const invalidFiles: string[] = [];
      const maxSizeBytes = 5 * 1024 * 1024; // 5MB in bytes
      
      // Process all selected files
      files.forEach(file => {
        // Check if file is an image
        if (!file.type.startsWith('image/')) {
          invalidFiles.push(`${file.name} (not an image)`);
          return;
        }
        
        // Check file size (5MB limit)
        if (file.size > maxSizeBytes) {
          invalidFiles.push(`${file.name} (exceeds 5MB limit)`);
          return;
        }
        
        // File is valid - add to processing list
        imageFiles.push(file);
      });
      
      // Add all valid image files to the preview list
      imageFiles.forEach(file => {
        this.addImageFromFile(file);
      });
      
      // Show error message if any files were invalid
      if (invalidFiles.length > 0) {
        const errorMsg = invalidFiles.length === 1 
          ? `${invalidFiles[0]} was skipped.`
          : `${invalidFiles.length} file(s) were skipped: ${invalidFiles.slice(0, 3).join(', ')}${invalidFiles.length > 3 ? '...' : ''}`;
        this.photoUploadError = errorMsg;
      } else {
        this.photoUploadError = '';
      }
      
      // Reset input to allow selecting the same file again
      input.value = '';
      this.photoInputMethod = 'upload';
      this.showCamera = false;
      this.stopCamera();
    }
  }
  
  addImageFromFile(file: File): void {
    const reader = new FileReader();
    reader.onload = (e) => {
      const preview = e.target?.result as string;
      // Generate unique ID with timestamp, random number, and file name hash
      const uniqueId = `file-${Date.now()}-${Math.random().toString(36).substring(2, 9)}-${file.name.substring(0, 5)}`;
      this.selectedImages.push({
        id: uniqueId,
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

  openFileInput(): void {
    this.showCamera = false;
    const fileInput = document.getElementById('wir-photo-file-input') as HTMLInputElement;
    if (fileInput) {
      // Ensure multiple attribute is set to allow multi-select
      if (!fileInput.hasAttribute('multiple')) {
        fileInput.setAttribute('multiple', '');
      }
      // Clear previous selection to allow re-selecting the same files
      fileInput.value = '';
      // Trigger file picker
      fileInput.click();
    }
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
        this.photoInputMethod = 'url';
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
  
  // Legacy methods for backward compatibility (deprecated)
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
    this.uploadedPhotos = [];
    this.photoPreviewUrls = [];
  }

  uploadPhoto(file: File): Observable<string> {
    // Use WIRService to upload to the correct endpoint
    // Note: This method uploads a single file, but we'll batch uploads in onSubmitReview
    if (!this.wirRecord?.wirRecordId) {
      return new Observable(observer => {
        observer.error(new Error('WIR Record ID is not available'));
        observer.complete();
      });
    }
    
    // Use the WIRService method which handles the correct endpoint
    return this.wirService.uploadInspectionPhotos(this.wirRecord.wirRecordId, [file]).pipe(
      map((urls: string[]) => {
        // Return the first URL (since we uploaded one file)
        return urls && urls.length > 0 ? urls[0] : '';
      })
    );
  }

  // Camera methods
  async openCamera(): Promise<void> {
    try {
      // Stop any existing camera stream first
      this.stopCamera();
      
      // Set showCamera to true first so video element is rendered
      this.showCamera = true;
      this.photoInputMethod = 'camera';
      this.photoUploadError = '';
      
      // Get user media stream
      const stream = await navigator.mediaDevices.getUserMedia({ 
        video: { facingMode: 'environment' } // Use back camera on mobile
      });
      
      this.cameraStream = stream;
      
      // Wait for video element to be rendered, then initialize
      setTimeout(() => {
        const video = document.getElementById('wir-camera-preview') as HTMLVideoElement;
        if (video) {
          video.srcObject = stream;
          video.play().catch(err => {
            console.error('Error playing video:', err);
            this.photoUploadError = 'Unable to start camera preview.';
            this.stopCamera();
          });
        } else {
          // If video element not found, stop camera
          this.stopCamera();
        }
      }, 100);
    } catch (err) {
      console.error('Error accessing camera:', err);
      this.photoUploadError = 'Unable to access camera. Please check permissions.';
      this.showCamera = false;
    }
  }

  stopCamera(): void {
    // Stop all tracks in the stream
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach(track => track.stop());
      this.cameraStream = null;
    }
    
    // Clear video element's srcObject
    const video = document.getElementById('wir-camera-preview') as HTMLVideoElement;
    if (video) {
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach(track => track.stop());
      }
      video.srcObject = null;
      video.pause();
    }
    
    // Close camera UI
    this.showCamera = false;
  }

  capturePhoto(): void {
    const video = document.getElementById('wir-camera-preview') as HTMLVideoElement;
    if (!video || !video.srcObject) return;

    // Get the stream before stopping
    const stream = video.srcObject as MediaStream;
    
    // Create canvas and capture image
    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    
    const ctx = canvas.getContext('2d');
    if (!ctx) return;
    
    // Draw the current video frame to canvas
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
    
    // Convert to base64 image data
    const imageData = canvas.toDataURL('image/jpeg', 0.9);
    
    // Stop camera stream immediately BEFORE adding image to prevent black screen
    if (stream) {
      stream.getTracks().forEach((track) => track.stop());
    }
    
    // Clear video element
    video.srcObject = null;
    video.pause();
    
    // Close camera UI immediately
    this.showCamera = false;
    this.cameraStream = null;
    
    // Add captured image (this happens after UI is closed, so no black screen)
    this.addImageFromDataUrl(imageData);
  }
  
  addImageFromDataUrl(imageData: string): void {
    // Convert data URL to File for consistency with existing structure
    fetch(imageData)
      .then(res => res.blob())
      .then(blob => {
        const file = new File([blob], `wir-inspection-${Date.now()}.jpg`, { type: 'image/jpeg' });
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

  removePhoto(index: number): void {
    // Legacy method for backward compatibility
    this.uploadedPhotos.splice(index, 1);
    this.photoPreviewUrls.splice(index, 1);
    if (index === 0 && this.uploadedPhotos.length === 0) {
      this.selectedFile = null;
      this.photoPreview = null;
    }
  }

  ngOnDestroy(): void {
    this.stopCamera();
    this.stopQualityIssueCamera();
    this.stopAttachmentCamera();
  }

  // Quality Issue Image Methods
  onQualityIssuePhotosSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) return;

    Array.from(input.files).forEach(file => {
      this.addQualityIssueImageFromFile(file);
    });

    // Clear the input so the same file can be selected again
    input.value = '';
  }

  addQualityIssueImageFromFile(file: File): void {
    if (!file.type.startsWith('image/')) {
      return;
    }

    if (file.size > 10 * 1024 * 1024) {
      return;
    }

    const reader = new FileReader();
    reader.onload = (e) => {
      const preview = e.target?.result as string;
      if (preview) {
        this.qualityIssueImages.push({
          id: `file-${Date.now()}-${Math.random()}`,
          type: 'file',
          file: file,
          preview: preview,
          name: file.name,
          size: file.size
        });
      }
    };
    reader.readAsDataURL(file);
  }

  openQualityIssueFileInput(): void {
    const fileInput = document.getElementById('quality-issue-photo-file-input') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
      fileInput.click();
    }
  }

  addQualityIssueImageFromUrl(): void {
    const url = this.qualityIssueCurrentUrlInput.trim();
    if (!url) return;

    if (!url.startsWith('http://') && !url.startsWith('https://') && !url.startsWith('data:image/')) {
      return;
    }

    this.qualityIssueImages.push({
      id: `url-${Date.now()}-${Math.random()}`,
      type: 'url',
      url: url.trim(),
      preview: url.trim()
    });

    this.qualityIssueCurrentUrlInput = '';
  }

  removeQualityIssueImage(imageId: string): void {
    this.qualityIssueImages = this.qualityIssueImages.filter(img => img.id !== imageId);
  }

  clearAllQualityIssueImages(): void {
    this.qualityIssueImages = [];
    this.qualityIssueCurrentUrlInput = '';
    this.stopQualityIssueCamera();
  }

  openQualityIssueCamera(): void {
    if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
      return;
    }

    this.showQualityIssueCamera = true;
    this.qualityIssuePhotoInputMethod = 'camera';

    navigator.mediaDevices.getUserMedia({ video: true })
      .then(stream => {
        this.qualityIssueCameraStream = stream;
        setTimeout(() => {
          const video = document.getElementById('quality-issue-video') as HTMLVideoElement;
          if (video) {
            video.srcObject = stream;
            video.play();
          }
        }, 100);
      })
      .catch(err => {
        console.error('Error accessing camera:', err);
        this.showQualityIssueCamera = false;
      });
  }

  stopQualityIssueCamera(): void {
    if (this.qualityIssueCameraStream) {
      this.qualityIssueCameraStream.getTracks().forEach(track => track.stop());
      this.qualityIssueCameraStream = null;
    }
    const video = document.getElementById('quality-issue-video') as HTMLVideoElement;
    if (video) {
      video.srcObject = null;
      video.pause();
    }
    this.showQualityIssueCamera = false;
  }

  captureQualityIssuePhoto(): void {
    const video = document.getElementById('quality-issue-video') as HTMLVideoElement;
    if (!video || !this.qualityIssueCameraStream) return;

    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    ctx.drawImage(video, 0, 0);
    const imageData = canvas.toDataURL('image/jpeg', 0.9);

    // Stop camera first
    this.qualityIssueCameraStream.getTracks().forEach(track => track.stop());
    this.qualityIssueCameraStream = null;
    video.srcObject = null;
    video.pause();
    this.showQualityIssueCamera = false;

    // Add captured image
    fetch(imageData)
      .then(res => res.blob())
      .then(blob => {
        const file = new File([blob], `quality-issue-${Date.now()}.jpg`, { type: 'image/jpeg' });
        this.qualityIssueImages.push({
          id: `camera-${Date.now()}-${Math.random()}`,
          type: 'camera',
          file: file,
          preview: imageData,
          name: file.name,
          size: file.size
        });
      })
      .catch(err => {
        console.error('Error converting data URL to file:', err);
      });
  }

  // Validation
  private hasPendingChecklistItems(): boolean {
    const items = this.checklistItems.value;
    return items.some((item: any) => item.status === CheckpointStatus.Pending);
  }

  private hasFailedChecklistItems(): boolean {
    const items = this.checklistItems.value;
    return items.some((item: any) => item.status === CheckpointStatus.Fail);
  }

  canSubmitReview(): boolean {
    if (!this.wirCheckpoint) return false;
    const status = this.finalStatusControl.value;
    if (!status) return false;
    if (this.hasPendingChecklistItems()) return false;
    if (status === WIRCheckpointStatus.Approved && this.hasFailedChecklistItems()) return false;
    return true;
  }

  onSubmitReview(): void {
    if (!this.canSubmitReview() || !this.wirCheckpoint) {
      this.markFormGroupTouched(this.checklistForm);
      return;
    }
    
    const status = this.finalStatusControl.value as WIRCheckpointStatus;
    this.submitting = true;
    this.error = '';
    this.photoUploadError = '';

    let missingChecklistId = false;
    const reviewItems = this.checklistItems.controls.map((control, index) => {
      const controlValue = control.value;
      const checklistItemId = controlValue.checklistItemId || this.wirCheckpoint?.checklistItems?.[index]?.checklistItemId;
      if (!checklistItemId) {
        missingChecklistId = true;
      }
      return {
        checklistItemId: checklistItemId as string,
        remarks: controlValue.remarks?.trim() || undefined,
        status: this.mapToCheckListItemStatus(controlValue.status)
      };
    });

    if (missingChecklistId) {
      this.submitting = false;
      this.error = 'Unable to submit review because one or more checklist items are missing identifiers. Please refresh the page and try again.';
      return;
    }

    // Handle multiple images: send files directly in review request
    const filesToUpload = this.selectedImages
      .filter(img => (img.type === 'file' || img.type === 'camera') && img.file)
      .map(img => img.file!)
      .filter((file): file is File => file !== undefined);
    
    const urlImages = this.selectedImages
      .filter(img => img.type === 'url' && img.url)
      .map(img => img.url!.trim())
      .filter((url): url is string => url !== '');

    // Send files and URLs directly in the review request (no separate upload needed)
    this.submitReviewWithFiles(filesToUpload, urlImages, status, reviewItems);
  }

  private submitReviewWithFiles(files: File[], imageUrls: string[], status: WIRCheckpointStatus, reviewItems: any[]): void {
    const request: ReviewWIRCheckpointRequest = {
      wIRId: this.wirCheckpoint!.wirId,
      status,
      comment: this.checklistForm.value.inspectionNotes?.trim() || undefined,
      inspectorRole: this.checklistForm.value.inspectorRole?.trim() || undefined,
      files: files.length > 0 ? files : undefined,
      imageUrls: imageUrls.length > 0 ? imageUrls : undefined,
      items: reviewItems
    };

    this.wirService.reviewWIRCheckpoint(request).subscribe({
      next: (updatedCheckpoint) => {
        this.handleReviewSuccess(updatedCheckpoint, status);
      },
      error: (err) => this.handleReviewError(err)
    });
  }

  private handleReviewSuccess(updatedCheckpoint: WIRCheckpoint, status: WIRCheckpointStatus): void {
        this.submitting = false;
        this.wirCheckpoint = updatedCheckpoint;
    this.syncReviewFormFromCheckpoint();
    this.pendingAction = null;
    this.finalStatusControl.setValue(status);
    this.qualityIssuesArray.clear();
    const messageMap: Record<WIRCheckpointStatus, string> = {
      [WIRCheckpointStatus.Approved]: 'WIR checkpoint approved successfully.',
      [WIRCheckpointStatus.Rejected]: 'WIR checkpoint rejected and sent for rework.',
      [WIRCheckpointStatus.ConditionalApproval]: 'WIR checkpoint conditionally approved.',
      [WIRCheckpointStatus.Pending]: 'WIR checkpoint updated.'
    };
    document.dispatchEvent(new CustomEvent('app-toast', {
      detail: {
        message: messageMap[status] || 'WIR checkpoint updated.',
        type: 'success'
      }
    }));
    
    // Navigate to quality issues step
    this.currentStep = 'quality-issues';
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { step: 'quality-issues' },
      queryParamsHandling: 'merge'
    });
  }

  private handleReviewError(err: any): void {
        this.submitting = false;
    this.error = err.error?.message || err.message || 'Failed to update WIR checkpoint';
    console.error('Review error:', err);
  }


  private mapToCheckListItemStatus(status: CheckpointStatus): CheckListItemStatus {
    const statusMap: Record<string, CheckListItemStatus> = {
      'Pending': CheckListItemStatus.Pending,
      'Pass': CheckListItemStatus.Pass,
      'Fail': CheckListItemStatus.Fail
    };
    return statusMap[status] || CheckListItemStatus.Pending;
  }

  /**
   * Get image URLs from the current WIR checkpoint
   */
  getCheckpointImageUrls(): string[] {
    if (!this.wirCheckpoint) {
      console.log('getCheckpointImageUrls - no checkpoint');
      return [];
    }
    
    if (!this.wirCheckpoint.images) {
      console.log('getCheckpointImageUrls - no images property');
      return [];
    }
    
    if (!Array.isArray(this.wirCheckpoint.images)) {
      console.log('getCheckpointImageUrls - images is not an array:', typeof this.wirCheckpoint.images);
      return [];
    }
    
    if (this.wirCheckpoint.images.length === 0) {
      console.log('getCheckpointImageUrls - images array is empty');
      return [];
    }
    
    console.log('getCheckpointImageUrls - found', this.wirCheckpoint.images.length, 'images');
    
    return this.wirCheckpoint.images
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
  
  /**
   * Get the API base URL (without /api suffix, since imageUrl already includes /api)
   */
  private getApiBaseUrl(): string {
    // Use environment.apiUrl and remove /api suffix if present
    return environment.apiUrl.replace(/\/api\/?$/, '');
  }

  /**
   * Open image in new tab
   */
  openCheckpointImageInNewTab(imageUrl: string): void {
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
    // For base64/data images, convert to blob URL
    else if (imageUrl.startsWith('data:image/')) {
      // For data URLs, convert to blob URL
      fetch(imageUrl)
        .then(response => response.blob())
        .then(blob => {
          const blobUrl = URL.createObjectURL(blob);
          const newWindow = window.open(blobUrl, '_blank', 'noopener,noreferrer');
          if (!newWindow) {
            console.error('Failed to open image in new tab. Popup may be blocked.');
            // Fallback: try to open data URL directly
            const fallbackWindow = window.open();
            if (fallbackWindow) {
              fallbackWindow.document.write(`
                <!DOCTYPE html>
                <html>
                  <head>
                    <title>Image Viewer</title>
                    <style>
                      body {
                        margin: 0;
                        padding: 20px;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        min-height: 100vh;
                        background: #1a1a1a;
                      }
                      img {
                        max-width: 100%;
                        max-height: 100vh;
                        height: auto;
                        object-fit: contain;
                      }
                    </style>
                  </head>
                  <body>
                    <img src="${imageUrl}" alt="Image" />
                  </body>
                </html>
              `);
              fallbackWindow.document.close();
            }
          }
          // Clean up blob URL after a delay
          setTimeout(() => URL.revokeObjectURL(blobUrl), 100);
        })
        .catch(error => {
          console.error('Error converting data URL to blob:', error);
          // Fallback: try to open data URL directly
          const fallbackWindow = window.open();
          if (fallbackWindow) {
            fallbackWindow.document.write(`
              <!DOCTYPE html>
              <html>
                <head>
                  <title>Image Viewer</title>
                  <style>
                    body {
                      margin: 0;
                      padding: 20px;
                      display: flex;
                      justify-content: center;
                      align-items: center;
                      min-height: 100vh;
                      background: #1a1a1a;
                    }
                    img {
                      max-width: 100%;
                      max-height: 100vh;
                      height: auto;
                      object-fit: contain;
                    }
                  </style>
                </head>
                <body>
                  <img src="${imageUrl}" alt="Image" />
                </body>
              </html>
            `);
            fallbackWindow.document.close();
          }
        });
      return;
    }
    // For external URLs (http/https), open directly
    else if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) {
      absoluteUrl = imageUrl;
    }

    // Open the absolute URL in a new tab
    const newWindow = window.open(absoluteUrl, '_blank', 'noopener,noreferrer');
    if (!newWindow) {
      console.error('Failed to open image in new tab. Popup may be blocked.');
    }
  }

  /**
   * Handle image load error
   */
  onCheckpointImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.style.display = 'none';
  }

  /**
   * Download checkpoint image
   */
  downloadCheckpointImage(imageUrl: string): void {
    if (!imageUrl) {
      console.error('No image URL provided');
      return;
    }

    // Ensure URL is absolute for download
    let absoluteUrl = imageUrl;
    
    // If it's a relative URL, make it absolute
    if (imageUrl.startsWith('/')) {
      const baseUrl = this.getApiBaseUrl();
      absoluteUrl = `${baseUrl}${imageUrl}`;
    }

    // For data URLs, convert to blob and download
    if (imageUrl.startsWith('data:image/')) {
      fetch(imageUrl)
        .then(response => response.blob())
        .then(blob => {
          const blobUrl = URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = blobUrl;
          link.download = `checkpoint-image-${Date.now()}.jpg`;
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
    fetch(absoluteUrl)
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
        link.download = `checkpoint-image-${Date.now()}.jpg`;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(blobUrl);
      })
      .catch(error => {
        console.error('Error downloading image:', error);
        // Fallback: try direct download link
        const link = document.createElement('a');
        link.href = absoluteUrl;
        link.download = `checkpoint-image-${Date.now()}.jpg`;
        link.target = '_blank';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      });
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      if (control instanceof FormArray) {
        control.controls.forEach(item => {
          if (item instanceof FormGroup) {
            this.markFormGroupTouched(item);
          } else {
            item.markAsTouched();
          }
        });
      } else if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      } else {
        control?.markAsTouched();
      }
    });
  }

  getStepNumber(step: ReviewStep): number {
    return this.stepFlow.indexOf(step) + 1;
  }

  /**
   * Check if a step is completed (based on actual backend status, not UI state)
   */
  isStepCompleted(step: ReviewStep): boolean {
    return this.isStepCompletedByStatus(step);
  }

  isStepActive(step: ReviewStep): boolean {
    if (this.currentStep) {
      return step === this.currentStep;
    }
    return this.pendingAction === step;
  }

  formatDateTime(date?: Date | string): string {
    if (!date) {
      return '—';
    }
    const parsed = date instanceof Date ? date : new Date(date);
    if (isNaN(parsed.getTime())) {
      return '—';
    }
    return parsed.toLocaleString('en-US', {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  getCheckpointStatusClass(status?: string): string {
    const normalized = (status || 'Pending').toString();
    const map: Record<string, string> = {
      'Pending': 'chip-pending',
      'Approved': 'chip-approved',
      'Rejected': 'chip-rejected',
      'ConditionalApproval': 'chip-conditional',
      'Conditional': 'chip-conditional'
    };
    return map[normalized] || 'chip-pending';
  }

  getCheckpointStatusLabel(status?: string | number | WIRCheckpointStatus): string {
    const normalized = status !== undefined && status !== null
      ? status.toString()
      : 'Pending';
    const labelMap: Record<string, string> = {
      'Pending': 'Pending',
      'Approved': 'Approved',
      'Rejected': 'Rejected',
      'ConditionalApproval': 'Conditional Approval',
      '1': 'Pending',
      '2': 'Approved',
      '3': 'Rejected',
      '4': 'Conditional Approval'
    };
    return labelMap[normalized] || 'Pending';
  }

  goBack(): void {
    if (this.fromContext === 'quality-control') {
      this.router.navigate(['/qc']);
      return;
    }

    this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId]);
  }

  get showOverviewState(): boolean {
    return !!this.wirCheckpoint && !this.currentStep;
  }

  get pendingActionLabel(): string | null {
    if (!this.pendingAction) return null;
    const map: Record<ReviewStep, string> = {
      'create-checkpoint': 'Create the checkpoint to continue',
      'add-items': 'Add checklist items to prepare this checkpoint for review',
      'review': 'Review and sign off this checkpoint',
      'quality-issues': 'Log quality issues for follow-up (optional)'
    };
    return map[this.pendingAction];
  }

  triggerPendingAction(): void {
    if (!this.pendingAction) {
      return;
    }
    
    // Validate that the pending action step is accessible
    if (!this.canNavigateToStep(this.pendingAction)) {
      console.warn(`Cannot trigger pending action ${this.pendingAction}. Previous steps must be completed.`);
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: {
          message: `Please complete previous steps before continuing to ${this.stepMeta.find(s => s.id === this.pendingAction)?.title || this.pendingAction}`,
          type: 'warning'
        }
      }));
      return;
    }
    
    if (this.pendingAction === 'add-items') {
      this.startAddChecklistFlow();
    } else if (this.pendingAction === 'review') {
      this.startReviewFlow();
    } else if (this.pendingAction === 'create-checkpoint') {
      this.currentStep = 'create-checkpoint';
    } else if (this.pendingAction === 'quality-issues') {
      this.startQualityIssuesFlow();
    }
  }

  get canStartReview(): boolean {
    return this.hasChecklistItems;
  }

  get displayChecklistItems(): WIRCheckpointChecklistItem[] {
    if (!this.wirCheckpoint?.checklistItems) return [];
    return [...this.wirCheckpoint.checklistItems].sort((a, b) => (a.sequence || 0) - (b.sequence || 0));
  }

  onFinalStatusSelect(status: WIRCheckpointStatus): void {
    this.finalStatusControl.setValue(status);
  }

  private handleFinalStatusChange(status: WIRCheckpointStatus | null): void {
    if (!status) return;
    this.applyRejectionValidation();
  }

  private getStepIndex(step: ReviewStep | null): number {
    if (!step) return -1;
    return this.stepFlow.indexOf(step);
  }

  get activeStep(): ReviewStep | null {
    if (this.currentStep) return this.currentStep;
    if (this.pendingAction) return this.pendingAction;
    return this.wirCheckpoint ? 'review' : 'create-checkpoint';
  }
}

