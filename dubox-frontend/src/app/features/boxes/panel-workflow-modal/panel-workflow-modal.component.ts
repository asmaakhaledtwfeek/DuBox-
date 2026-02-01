import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
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
  styleUrls: ['./panel-workflow-modal.component.scss']
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

  workflowStatuses = [
    { value: 'InProgress', label: 'In Progress', color: 'blue' },
    { value: 'Completed', label: 'Completed (Ready to Move from Site)', color: 'green' },
    { value: 'PutOnHold', label: 'Put On Hold', color: 'orange' }
  ];

  panelStages = PANEL_STAGES;
  PanelStage = PanelStage;

  constructor(
    private fb: FormBuilder,
    private panelService: PanelService
  ) {}

  readonly notesMinLength = 50;

  ngOnInit(): void {
    this.workflowForm = this.fb.group({
      workflowStatus: ['', Validators.required],
      currentStage: [null],
      notes: ['', [Validators.required, Validators.minLength(this.notesMinLength)]]
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['panel'] && this.panel && this.workflowForm) {
      // Store the previous status when modal opens
      this.previousStatus = this.panel.workflowStatus || 'NotStarted';
      
      console.log('🔍 Panel data loaded:', {
        workflowStatus: this.panel.workflowStatus,
        currentStage: this.panel.currentStage,
        moldPreparationComplete: this.panel.moldPreparationComplete,
        reinforcementSetupComplete: this.panel.reinforcementSetupComplete,
        concreteCastingComplete: this.panel.concreteCastingComplete,
        curingAndDemoldingComplete: this.panel.curingAndDemoldingComplete
      });
      
      // Pre-fill form with current status
      this.workflowForm.patchValue({
        workflowStatus: this.panel.workflowStatus || '',
        currentStage: this.panel.currentStage || null,
        notes: ''
      });
    }
    
    // Also track when modal opens/closes
    if (changes['isOpen'] && changes['isOpen'].currentValue && this.panel) {
      this.previousStatus = this.panel.workflowStatus || 'NotStarted';
      
      console.log('🔍 Modal opened with panel:', {
        panelName: this.panel.panelName,
        workflowStatus: this.panel.workflowStatus,
        stageCompletion: {
          mold: this.panel.moldPreparationComplete,
          reinforcement: this.panel.reinforcementSetupComplete,
          casting: this.panel.concreteCastingComplete,
          curing: this.panel.curingAndDemoldingComplete
        }
      });
    }
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
          setTimeout(() => {
            this.workflowUpdated.emit();
            this.close();
          }, 1500);
        } else {
          const errObj = response?.error ?? response?.Error;
          this.error = (typeof errObj === 'string' ? errObj : errObj?.description ?? errObj?.Description) 
            || response?.message || 'Failed to update workflow status';
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
      }
    });
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
      'InProgress': 'Panel is currently being worked on at site. Select current stage (1-4) as work progresses.',
      'Completed': 'All 4 stages complete. Panel ready to move from site for First & Second Approval.',
      'PutOnHold': 'Pause panel work. Automatically creates quality issue assigned to QC team for tracking.'
    };
    return infoMap[status] || '';
  }

  isStageComplete(stage: number): boolean {
    if (!this.panel) return false;
    
    let isComplete = false;
    switch (stage) {
      case PanelStage.MoldPreparation:
        isComplete = this.panel.moldPreparationComplete || false;
        break;
      case PanelStage.ReinforcementSetup:
        isComplete = this.panel.reinforcementSetupComplete || false;
        break;
      case PanelStage.ConcreteCasting:
        isComplete = this.panel.concreteCastingComplete || false;
        break;
      case PanelStage.CuringAndDemolding:
        isComplete = this.panel.curingAndDemoldingComplete || false;
        break;
      default:
        isComplete = false;
    }
    
    console.log(`🔍 Checking stage ${stage} completion:`, isComplete, this.panel);
    return isComplete;
  }

  getCompletedStagesCount(): number {
    if (!this.panel) return 0;
    let count = 0;
    if (this.panel.moldPreparationComplete) count++;
    if (this.panel.reinforcementSetupComplete) count++;
    if (this.panel.concreteCastingComplete) count++;
    if (this.panel.curingAndDemoldingComplete) count++;
    return count;
  }
}

