import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { WIRRecord, ApproveWIRRequest, RejectWIRRequest, WIRStatus } from '../../../core/models/wir.model';
import { WIRService } from '../../../core/services/wir.service';

@Component({
  selector: 'app-wir-approval-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './wir-approval-modal.component.html',
  styleUrls: ['./wir-approval-modal.component.scss']
})
export class WIRApprovalModalComponent implements OnInit, OnChanges {
  @Input() wirRecord: WIRRecord | null = null;
  @Input() isOpen: boolean = false;
  @Input() projectId: string = '';
  @Input() boxId: string = '';
  @Output() closeModal = new EventEmitter<void>();
  @Output() wirUpdated = new EventEmitter<WIRRecord>();

  approvalForm!: FormGroup;
  rejectionForm!: FormGroup;
  activeTab: 'approve' | 'reject' = 'approve';
  processing = false;
  error = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private wirService: WIRService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.approvalForm = this.fb.group({
      inspectionNotes: ['', Validators.maxLength(1000)]
    });

    this.rejectionForm = this.fb.group({
      rejectionReason: ['', [Validators.required, Validators.maxLength(500)]],
      inspectionNotes: ['', Validators.maxLength(1000)]
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.wirRecord) {
      // Pre-fill forms if WIR already has data
      if (this.wirRecord.inspectionNotes) {
        this.approvalForm.patchValue({ inspectionNotes: this.wirRecord.inspectionNotes });
        this.rejectionForm.patchValue({ inspectionNotes: this.wirRecord.inspectionNotes });
      }
      
      // Set default tab based on status
      if (this.wirRecord.status === WIRStatus.Rejected) {
        this.activeTab = 'reject';
      } else {
        this.activeTab = 'approve';
      }
    }
  }

  setActiveTab(tab: 'approve' | 'reject'): void {
    this.activeTab = tab;
    this.error = '';
    this.successMessage = '';
  }

  onApprove(): void {
    if (this.approvalForm.invalid || !this.wirRecord) {
      this.markFormGroupTouched(this.approvalForm);
      return;
    }

    this.processing = true;
    this.error = '';
    this.successMessage = '';

    const formValue = this.approvalForm.value;
    const request: ApproveWIRRequest = {
      wirRecordId: this.wirRecord.wirRecordId,
      inspectionNotes: formValue.inspectionNotes?.trim() || undefined
    };

    this.wirService.approveWIRRecord(request).subscribe({
      next: (updatedWIR) => {
        this.processing = false;
        this.successMessage = 'WIR record approved successfully!';
        this.wirUpdated.emit(updatedWIR);
        setTimeout(() => {
          this.close();
        }, 1500);
      },
      error: (err) => {
        this.processing = false;
        this.error = err.error?.message || err.message || 'Failed to approve WIR record. Please try again.';
        console.error('Error approving WIR:', err);
      }
    });
  }

  onReject(): void {
    if (this.rejectionForm.invalid || !this.wirRecord) {
      this.markFormGroupTouched(this.rejectionForm);
      return;
    }

    this.processing = true;
    this.error = '';
    this.successMessage = '';

    const formValue = this.rejectionForm.value;
    const request: RejectWIRRequest = {
      wirRecordId: this.wirRecord.wirRecordId,
      rejectionReason: formValue.rejectionReason.trim(),
      inspectionNotes: formValue.inspectionNotes?.trim() || undefined
    };

    this.wirService.rejectWIRRecord(request).subscribe({
      next: (updatedWIR) => {
        this.processing = false;
        this.successMessage = 'WIR record rejected. Redirecting to create WIR checkpoint...';
        this.wirUpdated.emit(updatedWIR);
        
        // Navigate to create WIR checkpoint form
        if (this.projectId && this.boxId && this.wirRecord?.boxActivityId) {
          const boxActivityId = this.wirRecord.boxActivityId;
          setTimeout(() => {
            this.close();
            this.router.navigate([
              '/projects',
              this.projectId,
              'boxes',
              this.boxId,
              'activities',
              boxActivityId,
              'create-wir-checkpoint'
            ]);
          }, 1500);
        } else {
          setTimeout(() => {
            this.close();
          }, 1500);
        }
      },
      error: (err) => {
        this.processing = false;
        this.error = err.error?.message || err.message || 'Failed to reject WIR record. Please try again.';
        console.error('Error rejecting WIR:', err);
      }
    });
  }

  close(): void {
    this.closeModal.emit();
    this.approvalForm.reset();
    this.rejectionForm.reset();
    this.error = '';
    this.successMessage = '';
    this.activeTab = 'approve';
  }

  canApprove(): boolean {
    return this.wirRecord?.status !== WIRStatus.Approved;
  }

  canReject(): boolean {
    return this.wirRecord?.status !== WIRStatus.Rejected;
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}

