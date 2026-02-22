import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { BoxPanel } from '../../../core/models/box.model';
import { PanelService, UpdatePanelWorkflowRequest } from '../../../core/services/panel.service';
import { PANEL_STAGES, PanelStage, PanelStageInfo } from '../../../core/models/panel-stage.model';

@Component({
  selector: 'app-panel-workflow-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './panel-workflow-modal.component.html',
  styleUrls: ['./panel-workflow-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PanelWorkflowModalComponent implements OnInit, OnChanges {
  @Input() panel: BoxPanel | null = null;
  @Input() isOpen: boolean = false;
  @Output() closeModal = new EventEmitter<void>();
  @Output() workflowUpdated = new EventEmitter<void>();

  workflowForm!: FormGroup;
  processing = false;
  error = '';
  successMessage = '';
  previousStatus = ''; // Track the status before opening modal
  private cachedCompletedStagesCount: number = 0;

  workflowStatuses = [
    { value: 'InProgress', label: 'In Progress', color: 'blue' },
    { value: 'Completed', label: 'Completed (Ready to Move from Site)', color: 'green' },
    { value: 'PutOnHold', label: 'Put On Hold', color: 'orange' }
  ];

  panelStages = PANEL_STAGES;
  PanelStage = PanelStage;

  constructor(
    private fb: FormBuilder,
    private panelService: PanelService,
    private cdr: ChangeDetectorRef
  ) {}

  readonly notesMinLength = 50;

  ngOnInit(): void {
    this.workflowForm = this.fb.group({
      workflowStatus: ['', Validators.required],
      currentStage: [null],
      notes: [''] // Notes are now optional
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['panel'] && this.panel && this.workflowForm) {
      // Store the previous status when modal opens
      this.previousStatus = this.panel.workflowStatus || 'NotStarted';
      
      // Cache completed stages count to avoid recalculating in template
      this.updateCachedStagesCount();
      
      // Pre-fill form with current status
      this.workflowForm.patchValue({
        workflowStatus: this.panel.workflowStatus || '',
        currentStage: this.panel.currentStage || null,
        notes: ''
      });
      
      // Disable form if panel has second approval
      if (this.isSecondApprovalApproved()) {
        this.workflowForm.disable();
      } else {
        this.workflowForm.enable();
      }
    }
    
    // Also track when modal opens/closes
    if (changes['isOpen'] && changes['isOpen'].currentValue && this.panel) {
      this.previousStatus = this.panel.workflowStatus || 'NotStarted';
      
      // Update cached stages count
      this.updateCachedStagesCount();
      
      // Disable form if panel has second approval
      if (this.isSecondApprovalApproved()) {
        this.workflowForm.disable();
      } else {
        this.workflowForm.enable();
      }
    }
  }
  
  /**
   * Check if panel has second approval approved
   */
  isSecondApprovalApproved(): boolean {
    if (!this.panel) return false;
    return this.panel.secondApprovalStatus?.toLowerCase() === 'approved';
  }

  get currentStatus() {
    return this.panel?.workflowStatus || 'NotStarted';
  }

  get selectedStatus() {
    return this.workflowForm?.get('workflowStatus')?.value || '';
  }

  get hasLinkedIssue() {
    return !!this.panel?.qualityIssueId;
  }

  getStatusDisplayName(status: string): string {
    const statusMap: { [key: string]: string } = {
      'NotStarted': 'Not Started',
      'InProgress': 'In Progress',
      'Completed': 'Completed',
      'PutOnHold': 'Put On Hold',
      'OnHold': 'On Hold',
      'Rejected': 'Rejected'
    };
    return statusMap[status] || status;
  }

  onSubmit(): void {
    if (this.workflowForm.invalid || !this.panel) {
      return;
    }

    this.processing = true;
    this.error = '';
    this.successMessage = '';

    const request: UpdatePanelWorkflowRequest = {
      boxPanelId: this.panel.boxPanelId,
      workflowStatus: this.workflowForm.value.workflowStatus,
      currentStage: this.workflowForm.value.currentStage,
      notes: this.workflowForm.value.notes
    };

    this.panelService.updatePanelWorkflowStatus(request).subscribe({
      next: (response) => {
        this.processing = false;
        // ApiService extracts data from Result wrapper - success means we got here
        const isSuccess = response?.isSuccess ?? response?.IsSuccess ?? true;
        if (isSuccess) {
          this.successMessage = 'Panel workflow status updated successfully!';
          this.cdr.markForCheck();
          // Immediately emit update and close to trigger refresh
          // This ensures panel data (including newly created quality issues) is refreshed
          setTimeout(() => {
            this.workflowUpdated.emit();
            this.close();
          }, 800); // Reduced delay for faster refresh
        } else {
          const errObj = response?.error ?? response?.Error;
          this.error = (typeof errObj === 'string' ? errObj : errObj?.description ?? errObj?.Description) 
            || response?.message || 'Failed to update workflow status';
          this.cdr.markForCheck();
        }
      },
      error: (err) => {
        this.processing = false;
        const errBody = err?.error;
        const errMsg = errBody?.message ?? errBody?.Message 
          ?? (typeof errBody?.error === 'string' ? errBody.error : errBody?.error?.description ?? errBody?.error?.Description)
          ?? errBody?.title ?? err?.message 
          ?? 'An error occurred while updating workflow status';
        this.error = errMsg;
        this.cdr.markForCheck();
      }
    });
  }

  /** Only close when clicking directly on the overlay (backdrop), not when event bubbles from inside */
  onOverlayClick(event: MouseEvent): void {
    if (event.target === event.currentTarget) {
      this.close();
    }
  }

  close(): void {
    this.workflowForm.reset();
    this.error = '';
    this.successMessage = '';
    this.closeModal.emit();
  }

  getStatusBadgeClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      'InProgress': 'badge-blue',
      'Completed': 'badge-green',
      'PutOnHold': 'badge-orange',
      'OnHold': 'badge-orange',
      'Rejected': 'badge-red',
      'NotStarted': 'badge-gray',
      'FirstApprovalPending': 'badge-orange',
      'FirstApprovalApproved': 'badge-green',
      'SecondApprovalPending': 'badge-orange',
      'SecondApprovalApproved': 'badge-green',
      'SecondApprovalRejected': 'badge-red'
    };
    return statusMap[status] || 'badge-gray';
  }

  getStatusLabel(status: string): string {
    const statusMap: { [key: string]: string } = {
      'InProgress': 'In Progress',
      'Completed': 'Completed (Ready to Move)',
      'PutOnHold': 'On Hold',
      'Rejected': 'Rejected',
      'NotStarted': 'Not Started',
      'FirstApprovalPending': 'Pending First Approval',
      'FirstApprovalApproved': 'First Approval Approved',
      'SecondApprovalPending': 'Pending Second Approval',
      'SecondApprovalApproved': 'Second Approval Approved',
      'SecondApprovalRejected': 'Second Approval Rejected'
    };
    return statusMap[status] || status;
  }

  getStatusInfo(status: string): string {
    const infoMap: { [key: string]: string } = {
      'InProgress': 'Panel is currently being worked on at site. Select current stage (1-7) as work progresses.',
      'Completed': 'All 7 stages complete. Panel ready to move from site for First & Second Approval.',
      'PutOnHold': 'Pause panel work. Automatically creates quality issue assigned to QC team for tracking.'
    };
    return infoMap[status] || '';
  }

  /** Last stage number (7) - selecting it auto-sets status to Completed */
  readonly lastStageNumber = PanelStage.CuringAndDemolding;

  /**
   * Check if stages are clickable (when form is enabled, i.e. not second-approved)
   */
  canSelectStage(): boolean {
    return !!this.workflowForm?.enabled;
  }

  /**
   * When user clicks a stage: set current stage and auto-set workflow status.
   * Stages 1-6 → "In Progress", Stage 7 (last) → "Completed"
   */
  selectStage(stageInfo: PanelStageInfo): void {
    if (!this.canSelectStage() || !this.workflowForm) return;
    const isLastStage = stageInfo.stage === this.lastStageNumber;
    this.workflowForm.patchValue({
      currentStage: stageInfo.stage,
      workflowStatus: isLastStage ? 'Completed' : 'InProgress'
    });
  }

  isStageComplete(stage: number): boolean {
    if (!this.panel) return false;
    
    switch (stage) {
      case PanelStage.MoldPreparation:
        return this.panel.moldPreparationComplete || false;
      case PanelStage.Initial:
        return this.panel.initialComplete || false;
      case PanelStage.MEPInsertsInstallation:
        return this.panel.mepInsertsInstallationComplete || false;
      case PanelStage.ReinforcementSetup:
        return this.panel.reinforcementSetupComplete || false;
      case PanelStage.ConcreteCasting:
        return this.panel.concreteCastingComplete || false;
      case PanelStage.SurfaceFinishing:
        return this.panel.surfaceFinishingComplete || false;
      case PanelStage.CuringAndDemolding:
        return this.panel.curingAndDemoldingComplete || false;
      default:
        return false;
    }
  }

  private updateCachedStagesCount(): void {
    if (!this.panel) {
      this.cachedCompletedStagesCount = 0;
      return;
    }
    let count = 0;
    if (this.panel.moldPreparationComplete) count++;
    if (this.panel.initialComplete) count++;
    if (this.panel.mepInsertsInstallationComplete) count++;
    if (this.panel.reinforcementSetupComplete) count++;
    if (this.panel.concreteCastingComplete) count++;
    if (this.panel.surfaceFinishingComplete) count++;
    if (this.panel.curingAndDemoldingComplete) count++;
    this.cachedCompletedStagesCount = count;
  }

  getCompletedStagesCount(): number {
    return this.cachedCompletedStagesCount;
  }

  getTotalStagesCount(): number {
    return 7; // Total number of stages
  }

  /**
   * TrackBy function for stages loop to improve performance
   */
  trackByStage(index: number, stageInfo: PanelStageInfo): number {
    return stageInfo.stage;
  }
}

