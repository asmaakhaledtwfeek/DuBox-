import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule, FormsModule, FormControl } from '@angular/forms';
import { WIRService } from '../../../core/services/wir.service';
import { BoxService } from '../../../core/services/box.service';
import { AuthService } from '../../../core/services/auth.service';
import { ApiService } from '../../../core/services/api.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
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
  AddQualityIssuesRequest,
  IssueType,
  SeverityType,
  QualityIssueItem,
  QualityIssueDetails,
  WIRCheckpointChecklistItem
} from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import * as ExcelJS from 'exceljs';

type ReviewStep = 'create-checkpoint' | 'add-items' | 'review' | 'quality-issues';

type QualityIssueFormValue = {
  issueType: IssueType;
  severity: SeverityType;
  issueDescription: string;
  assignedTo?: string;
  dueDate?: string;
  photoPath?: string;
  reportedBy?: string;
  issueDate?: string;
};

@Component({
  selector: 'app-qa-qc-checklist',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
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
  
  // Photo upload state (enhanced)
  photoUrl: string = '';
  selectedFile: File | null = null;
  photoPreview: string | null = null;
  isUploadingPhoto = false;
  photoUploadError = '';
  cameraStream: MediaStream | null = null;
  showCamera = false;
  photoInputMethod: 'url' | 'upload' | 'camera' = 'upload';
  
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
    private apiService: ApiService
  ) {}

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
    this.loadPredefinedChecklistItems();
  }

  private loadPredefinedChecklistItems(): void {
    this.loadingPredefinedItems = true;
    this.wirService.getPredefinedChecklistItems().subscribe({
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

    // Filter out items that are already added
    this.availablePredefinedItems = this.predefinedChecklistItems.filter(
      item => !addedPredefinedIds.has(item.predefinedItemId)
    );
  }

  getGroupedPredefinedItems(): { category: string; items: PredefinedChecklistItem[] }[] {
    const grouped = new Map<string, PredefinedChecklistItem[]>();
    
    // Group items by category
    this.availablePredefinedItems.forEach(item => {
      const category = item.category || 'Other';
      if (!grouped.has(category)) {
        grouped.set(category, []);
      }
      grouped.get(category)!.push(item);
    });

    // Convert to array and sort by predefined category order
    const categoryOrder = ['General', 'Setting Out', 'Installation Activity'];
    const result: { category: string; items: PredefinedChecklistItem[] }[] = [];
    
    // Add categories in order
    categoryOrder.forEach(cat => {
      if (grouped.has(cat)) {
        result.push({
          category: cat,
          items: grouped.get(cat)!.sort((a, b) => a.sequence - b.sequence)
        });
      }
    });

    // Add any remaining categories
    grouped.forEach((items, category) => {
      if (!categoryOrder.includes(category)) {
        result.push({
          category,
          items: items.sort((a, b) => a.sequence - b.sequence)
        });
      }
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
    
    // If there's a step query parameter, apply it first
    if (this.initialStepFromQuery) {
      // Validate query parameter step before applying
      if (this.initialStepFromQuery === 'create-checkpoint') {
        // Checkpoint exists, so redirect to next accessible step instead of create-checkpoint
        const nextStep = this.getNextAccessibleStep();
        if (nextStep && nextStep !== 'create-checkpoint') {
          this.router.navigate([], {
            relativeTo: this.route,
            queryParams: { step: nextStep },
            queryParamsHandling: 'merge'
          });
          this.handleStepClick(nextStep);
        } else {
          this.currentStep = null;
          this.pendingAction = nextStep || 'add-items';
        }
        this.initialStepApplied = true;
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
      // No step query parameter, determine the next accessible step
      const nextStep = this.getNextAccessibleStep();
      if (nextStep && nextStep !== 'create-checkpoint') {
        // Auto-navigate to the next accessible step
        this.router.navigate([], {
          relativeTo: this.route,
          queryParams: { step: nextStep },
          queryParamsHandling: 'merge'
        });
        this.handleStepClick(nextStep);
      } else {
        this.currentStep = null;
        this.pendingAction = nextStep || 'add-items';
      }
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
    const request: CreateWIRCheckpointRequest = {
      boxActivityId: this.activityId, // Get from route param
      wirNumber: this.wirRecord.wirCode || '', // Get from WIRRecord
      wirName: formValue.wirName?.trim() || undefined,
      wirDescription: formValue.wirDescription?.trim() || undefined,
      attachmentPath: formValue.attachmentPath?.trim() || undefined,
      comments: formValue.comments?.trim() || undefined
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
            sequence: item.sequence
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
    this.updateAvailablePredefinedItems();
    this.selectedPredefinedItemIds = [];
    this.isAddPredefinedItemsModalOpen = true;
    document.body.style.overflow = 'hidden';
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
          remarks: [item.remarks || '']
        });
        this.checklistItems.push(itemGroup);
      });
      
      console.log('loadChecklistItems: loaded', this.checklistItems.length, 'items');
    } else {
      console.warn('loadChecklistItems: No checklist items found in checkpoint');
    }
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
    console.log('startAddChecklistFlow - checklistItems length:', this.wirCheckpoint.checklistItems?.length || 0);
    
    // Always build the form with existing items from checkpoint
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

  private createChecklistItemGroup(item?: Partial<{ checklistItemId?: string; checkpointDescription: string; referenceDocument?: string; sequence?: number }>): FormGroup {
    return this.fb.group({
      checklistItemId: [item?.checklistItemId || null],
      checkpointDescription: [item?.checkpointDescription || '', Validators.required],
      referenceDocument: [item?.referenceDocument || ''],
      sequence: [item?.sequence || 1, [Validators.required, Validators.min(1)]]
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
      dueDate: '',
      photoPath: ''
    });
    this.isQualityIssueModalOpen = true;
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

  addQualityIssueFromModal(): void {
    if (this.newQualityIssueForm.invalid) {
      this.newQualityIssueForm.markAllAsTouched();
      return;
    }

    const value = this.newQualityIssueForm.value;
    this.addQualityIssueRow({
      issueDescription: value.issueDescription,
      severity: value.severity,
      issueType: value.issueType,
      assignedTo: value.assignedTo,
      dueDate: value.dueDate,
      photoPath: value.photoPath,
      reportedBy: 'Current User',
      issueDate: this.toDateInputValue(new Date())
    });
    this.closeQualityIssueModal();
  }

  submitQualityIssues(): void {
    if (!this.wirCheckpoint || this.qualityIssuesArray.length === 0) {
      return;
    }

    if (this.qualityIssuesForm.invalid) {
      this.markFormGroupTouched(this.qualityIssuesForm);
      return;
    }

    const request = this.buildQualityIssuesRequest(this.wirCheckpoint.wirId);
    this.savingQualityIssues = true;

    this.wirService.addQualityIssues(request).subscribe({
      next: (updatedCheckpoint) => {
        this.wirCheckpoint = updatedCheckpoint;
        this.refreshCheckpointDetails();
        this.resetQualityIssuesForm();
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Quality issues saved successfully.', type: 'success' }
        }));
        this.savingQualityIssues = false;
      },
      error: (err) => {
        console.error('Failed to add quality issues:', err);
        this.savingQualityIssues = false;
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
    const existing = this.existingQualityIssues;
    if (existing.length) {
      existing.forEach(issue => {
        this.addQualityIssueRow({
          issueDescription: issue.issueDescription,
          severity: (issue.severity as SeverityType) || this.severityLevels[0],
          issueType: (issue.issueType as IssueType) || this.issueTypes[0],
          assignedTo: issue.assignedTo,
          dueDate: issue.dueDate ? this.toDateInputValue(issue.dueDate) : '',
          photoPath: issue.photoPath
        });
      });
      return;
    }
    this.addQualityIssueRow();
  }

  private addQualityIssueRow(issue?: Partial<QualityIssueFormValue>): void {
    this.qualityIssuesArray.push(this.fb.group({
      issueType: [issue?.issueType || this.issueTypes[0], Validators.required],
      severity: [issue?.severity || this.severityLevels[0], Validators.required],
      issueDescription: [issue?.issueDescription || '', [Validators.required, Validators.maxLength(500)]],
      assignedTo: [issue?.assignedTo || '', [Validators.maxLength(200)]],
      dueDate: [issue?.dueDate || ''],
      photoPath: [issue?.photoPath || '', [Validators.maxLength(500)]],
      reportedBy: [issue?.reportedBy || null],
      issueDate: [issue?.issueDate || null]
    }));
  }

  removeQualityIssue(index: number): void {
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

  /**
   * Check if user can navigate to a specific step (enforces sequential workflow)
   */
  canNavigateToStep(step: ReviewStep): boolean {
    const stepIndex = this.getStepIndex(step);
    
    // Step 1: Always accessible if checkpoint doesn't exist
    if (step === 'create-checkpoint') {
      return !this.wirCheckpoint;
    }
    
    // Step 2: Only accessible if Step 1 is completed
    if (step === 'add-items') {
      return this.isStep1Completed() && !this.isChecklistLocked;
    }
    
    // Step 3: Only accessible if Step 2 is completed
    if (step === 'review') {
      return this.isStep2Completed();
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
        // Only allow if checkpoint doesn't exist
        if (!this.wirCheckpoint) {
          this.currentStep = 'create-checkpoint';
          this.pendingAction = null;
          this.initialStepApplied = true;
        } else {
          // Checkpoint exists, redirect to next accessible step
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

  get addChecklistItemsArray(): FormArray {
    return this.addChecklistItemsForm.get('checklistItems') as FormArray;
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

  // Photo Upload Methods (Enhanced)
  onPhotosSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      if (file.type.startsWith('image/')) {
        this.selectedFile = file;
        this.photoUrl = ''; // Clear URL when file is selected
        this.photoInputMethod = 'upload';
        this.showCamera = false; // Stop camera if active
        this.stopCamera(); // Ensure camera is stopped
        this.photoUploadError = ''; // Clear any previous errors
        this.previewImage(file);
        // Also add to uploadedPhotos array for backward compatibility
        this.uploadedPhotos = [file];
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.photoPreviewUrls = [e.target.result];
        };
        reader.readAsDataURL(file);
      } else {
        this.photoUploadError = 'Please select an image file';
      }
    }
  }

  openFileInput(): void {
    this.showCamera = false;
    const fileInput = document.getElementById('wir-photo-file-input') as HTMLInputElement;
    if (fileInput) {
      fileInput.click();
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
    this.uploadedPhotos = [];
    this.photoPreviewUrls = [];
  }

  uploadPhoto(file: File): Observable<string> {
    return this.apiService.upload<{ url: string }>('upload/wir-inspection-photo', file).pipe(
      map((response: any) => {
        if (typeof response === 'string') return response;
        return response?.url || response?.photoPath || response?.attachmentPath || response?.data?.url || response?.data?.photoPath || response?.data?.attachmentPath || '';
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
      this.photoUrl = '';
      this.photoInputMethod = 'camera';
      
      // Wait for video element to be rendered
      setTimeout(() => {
        const video = document.getElementById('wir-camera-preview') as HTMLVideoElement;
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
    const video = document.getElementById('wir-camera-preview') as HTMLVideoElement;
    if (!video) return;

    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const ctx = canvas.getContext('2d');
    if (ctx) {
      ctx.drawImage(video, 0, 0);
      canvas.toBlob((blob) => {
        if (blob) {
          const file = new File([blob], `wir-inspection-${Date.now()}.jpg`, { type: 'image/jpeg' });
          this.selectedFile = file;
          this.previewImage(file);
          this.uploadedPhotos = [file];
          const reader = new FileReader();
          reader.onload = (e: any) => {
            this.photoPreviewUrls = [e.target.result];
          };
          reader.readAsDataURL(file);
          this.stopCamera();
        }
      }, 'image/jpeg', 0.9);
    }
  }

  removePhoto(index: number): void {
    this.uploadedPhotos.splice(index, 1);
    this.photoPreviewUrls.splice(index, 1);
    if (index === 0 && this.uploadedPhotos.length === 0) {
      this.selectedFile = null;
      this.photoPreview = null;
    }
  }

  ngOnDestroy(): void {
    this.stopCamera();
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

    // Upload photo if file is selected, otherwise proceed with URL
    if (this.selectedFile) {
      this.isUploadingPhoto = true;
      this.uploadPhoto(this.selectedFile).subscribe({
        next: (uploadResult) => {
          this.isUploadingPhoto = false;
          const attachmentPath = uploadResult || this.photoUrl?.trim() || null;
          this.submitReviewWithAttachment(attachmentPath, status, reviewItems);
        },
        error: (err: any) => {
          console.error('❌ Failed to upload photo:', err);
          this.isUploadingPhoto = false;
          this.photoUploadError = err?.error?.message || err?.message || 'Failed to upload photo';
          this.submitting = false;
        }
      });
    } else {
      const attachmentPath = this.photoUrl?.trim() || null;
      this.submitReviewWithAttachment(attachmentPath, status, reviewItems);
    }
  }

  private submitReviewWithAttachment(attachmentPath: string | null, status: WIRCheckpointStatus, reviewItems: any[]): void {
    const request: ReviewWIRCheckpointRequest = {
      wirId: this.wirCheckpoint!.wirId,
      status,
      comment: this.checklistForm.value.inspectionNotes?.trim() || undefined,
      inspectorRole: this.checklistForm.value.inspectorRole?.trim() || undefined,
      attachmentPath: attachmentPath || undefined,
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
    this.currentStep = null;
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
        this.goBack();
  }

  private handleReviewError(err: any): void {
        this.submitting = false;
    this.error = err.error?.message || err.message || 'Failed to update WIR checkpoint';
    console.error('Review error:', err);
  }

  private buildQualityIssuesRequest(wirId: string): AddQualityIssuesRequest {
    return {
      wirId,
      issues: this.qualityIssuesArray.value.map((issue: QualityIssueFormValue) => ({
        issueType: issue.issueType,
        severity: issue.severity,
        issueDescription: issue.issueDescription?.trim() || '',
        assignedTo: issue.assignedTo?.trim() || undefined,
        dueDate: issue.dueDate ? new Date(issue.dueDate) : undefined,
        photoPath: issue.photoPath?.trim() || undefined
      }))
    };
  }

  private mapToCheckListItemStatus(status: CheckpointStatus): CheckListItemStatus {
    const statusMap: Record<string, CheckListItemStatus> = {
      'Pending': CheckListItemStatus.Pending,
      'Pass': CheckListItemStatus.Pass,
      'Fail': CheckListItemStatus.Fail
    };
    return statusMap[status] || CheckListItemStatus.Pending;
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
