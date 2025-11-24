import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { WIRService } from '../../../core/services/wir.service';
import { BoxService } from '../../../core/services/box.service';
import { AuthService } from '../../../core/services/auth.service';
import { 
  WIRRecord, 
  CheckpointStatus, 
  WIRCheckpoint,
  CreateWIRCheckpointRequest,
  AddChecklistItemsRequest,
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
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './qa-qc-checklist.component.html',
  styleUrls: ['./qa-qc-checklist.component.scss']
})
export class QaQcChecklistComponent implements OnInit {
  wirRecord: WIRRecord | null = null;
  wirCheckpoint: WIRCheckpoint | null = null;
  projectId!: string;
  boxId!: string;
  activityId!: string;
  
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
  pendingDeleteIndex: number | null = null;
  
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
  
  CheckpointStatus = CheckpointStatus;
  WIRCheckpointStatus = WIRCheckpointStatus;
  CheckListItemStatus = CheckListItemStatus;
  
  // Step tracking
  currentStep: ReviewStep | null = 'create-checkpoint';
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
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['projectId'];
    this.boxId = this.route.snapshot.params['boxId'];
    this.activityId = this.route.snapshot.params['activityId'];
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
    if (stepParam === 'quality-issues') {
      this.initialStepFromQuery = 'quality-issues';
    }
    
    this.initForm();
    this.loadWIRRecord();
    this.loadBoxQualityIssues();
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
    if (!this.wirRecord) {
      this.loading = false;
      return;
    }

    // Check if WIR checkpoint exists for this box/activity by WIR code
    this.wirService.getWIRCheckpointByActivity(this.boxId, this.wirRecord.wirCode).subscribe({
      next: (checkpoint) => {
        if (checkpoint) {
          this.wirCheckpoint = checkpoint;
          this.syncReviewFormFromCheckpoint();
          this.pendingAction = checkpoint.checklistItems && checkpoint.checklistItems.length > 0 ? 'review' : 'add-items';
          this.currentStep = null;
          this.applyInitialStepFromQuery();
        } else {
          this.currentStep = 'create-checkpoint';
          this.pendingAction = null;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error checking WIR checkpoint:', err);
        this.currentStep = 'create-checkpoint';
        this.pendingAction = null;
        this.loading = false;
      }
    });
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
        this.wirCheckpoint = checkpoint;
        this.syncReviewFormFromCheckpoint();
        this.applyInitialStepFromQuery();
        this.creatingCheckpoint = false;
        this.pendingAction = 'add-items';
        this.currentStep = null;
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

    if (sourceItems && sourceItems.length > 0) {
      sourceItems
        .sort((a, b) => (a.sequence || 0) - (b.sequence || 0))
        .forEach(item => {
          itemsArray.push(this.createChecklistItemGroup({
            checkpointDescription: item.checkpointDescription,
            referenceDocument: item.referenceDocument,
            sequence: item.sequence
          }));
        });
    }

    this.updateAddChecklistSequences();
  }

  onAddChecklistItems(): void {
    if (!this.wirCheckpoint || this.addChecklistItemsArray.length === 0) {
      this.error = 'Add at least one checklist item before submitting.';
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
      items: this.addChecklistItemsArray.value.map((item: any) => ({
        checkpointDescription: item.checkpointDescription.trim(),
        referenceDocument: item.referenceDocument?.trim() || undefined,
        sequence: item.sequence
      }))
    };

    this.wirService.addChecklistItems(request).subscribe({
      next: () => {
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

  private loadChecklistItems(): void {
    if (!this.wirCheckpoint) return;

    // Clear existing items
    this.checklistItems.clear();
    
    if (this.wirCheckpoint.checklistItems && this.wirCheckpoint.checklistItems.length > 0) {
      // Load existing checklist items from checkpoint
      this.wirCheckpoint.checklistItems.forEach(item => {
        this.checklistItems.push(this.fb.group({
          checklistItemId: [item.checklistItemId, Validators.required],
          sequence: [item.sequence],
          checkpointDescription: [item.checkpointDescription],
          referenceDocument: [item.referenceDocument],
          status: [this.mapCheckListItemStatus(item.status), Validators.required],
          remarks: [item.remarks || '']
        }));
      });
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

    const existingItems = this.wirCheckpoint.checklistItems && this.wirCheckpoint.checklistItems.length > 0
      ? this.wirCheckpoint.checklistItems
      : undefined;
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

  private createChecklistItemGroup(item?: Partial<{ checkpointDescription: string; referenceDocument?: string; sequence?: number }>): FormGroup {
    return this.fb.group({
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

  removeChecklistItemRow(index: number): void {
    this.addChecklistItemsArray.removeAt(index);
    this.updateAddChecklistSequences();
  }

  confirmRemoveChecklistItem(index: number): void {
    if (this.isChecklistLocked) {
      this.notifyChecklistLocked();
      return;
    }
    this.pendingDeleteIndex = index;
    this.isDeleteModalOpen = true;
  }

  private updateAddChecklistSequences(): void {
    this.addChecklistItemsArray.controls.forEach((control, idx) => {
      control.get('sequence')?.setValue(idx + 1, { emitEvent: false });
    });
  }

  cancelDeleteChecklistItem(): void {
    this.pendingDeleteIndex = null;
    this.isDeleteModalOpen = false;
  }

  confirmDeleteChecklistItem(): void {
    if (this.pendingDeleteIndex !== null) {
      this.removeChecklistItemRow(this.pendingDeleteIndex);
    }
    this.cancelDeleteChecklistItem();
  }

  private refreshCheckpointDetails(resetToOverview = false): void {
    if (!this.wirCheckpoint?.wirId) {
      return;
    }

    this.wirService.getWIRCheckpointById(this.wirCheckpoint.wirId).subscribe({
      next: (checkpoint) => {
        this.wirCheckpoint = checkpoint;
        this.syncReviewFormFromCheckpoint();
        this.applyInitialStepFromQuery();
        this.buildAddChecklistItemsForm(checkpoint.checklistItems || []);
        if (resetToOverview) {
          this.pendingAction = 'review';
          this.currentStep = null;
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

  canNavigateToStep(step: ReviewStep): boolean {
    switch (step) {
      case 'create-checkpoint':
        return !this.wirCheckpoint;
      case 'add-items':
        return !!this.wirCheckpoint && !this.isChecklistLocked;
      case 'review':
        return this.hasChecklistItems;
      case 'quality-issues':
        return !!this.wirCheckpoint;
      default:
        return false;
    }
  }

  handleStepClick(step: ReviewStep): void {
    if (!this.canNavigateToStep(step)) {
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

    if (this.initialStepFromQuery === 'quality-issues' && this.wirCheckpoint) {
      this.startQualityIssuesFlow();
      this.initialStepApplied = true;
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

  get isRejectedSelected(): boolean {
    return this.finalStatusControl.value === WIRCheckpointStatus.Rejected;
  }

  get existingQualityIssues(): QualityIssueItem[] {
    return this.wirCheckpoint?.qualityIssues || [];
  }

  private applyRejectionValidation(): void {
    // No-op since backend does not track rejection reason
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

  // Photo Upload Methods
  onPhotosSelected(event: Event): void {
    const files = (event.target as HTMLInputElement).files;
    if (files) {
      Array.from(files).forEach(file => {
        this.uploadedPhotos.push(file);
        
        // Create preview
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.photoPreviewUrls.push(e.target.result);
        };
        reader.readAsDataURL(file);
      });
    }
  }

  removePhoto(index: number): void {
    this.uploadedPhotos.splice(index, 1);
    this.photoPreviewUrls.splice(index, 1);
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

    const request: ReviewWIRCheckpointRequest = {
      wirId: this.wirCheckpoint.wirId,
      status,
      comment: this.checklistForm.value.inspectionNotes?.trim() || undefined,
      inspectorRole: this.checklistForm.value.inspectorRole?.trim() || undefined,
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

  isStepCompleted(step: ReviewStep): boolean {
    const activeIndex = this.getStepIndex(this.activeStep);
    return this.getStepIndex(step) < activeIndex;
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
    if (this.pendingAction === 'add-items') {
      this.startAddChecklistFlow();
    } else if (this.pendingAction === 'review') {
      this.startReviewFlow();
    } else if (this.pendingAction === 'create-checkpoint') {
      this.currentStep = 'create-checkpoint';
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
