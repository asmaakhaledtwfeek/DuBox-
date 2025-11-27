import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
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
  
  // Expose WIRStatus enum to template
  WIRStatus = WIRStatus;

  constructor(
    private fb: FormBuilder,
    private wirService: WIRService
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
    if (changes['wirRecord'] && this.wirRecord) {
      // Reset forms first
      this.approvalForm.reset();
      this.rejectionForm.reset();
      
      // Pre-fill forms with existing data if available
      if (this.wirRecord.inspectionNotes) {
        this.approvalForm.patchValue({ inspectionNotes: this.wirRecord.inspectionNotes });
        this.rejectionForm.patchValue({ inspectionNotes: this.wirRecord.inspectionNotes });
      }
      
      // Pre-fill rejection reason if available
      if (this.wirRecord.rejectionReason) {
        this.rejectionForm.patchValue({ rejectionReason: this.wirRecord.rejectionReason });
      }
      
      // Set default tab based on status, but allow switching
      if (this.wirRecord.status === WIRStatus.Rejected) {
        this.activeTab = 'reject';
      } else {
        this.activeTab = 'approve';
      }
    }
    
    // Reset error and success messages when modal opens/closes
    if (changes['isOpen']) {
      if (this.isOpen) {
        this.error = '';
        this.successMessage = '';
      }
    }
  }

  /**
   * Check if WIR is already reviewed (Approved or Rejected)
   */
  isAlreadyReviewed(): boolean {
    return this.wirRecord?.status === WIRStatus.Approved || 
           this.wirRecord?.status === WIRStatus.Rejected ||
           this.wirRecord?.status === WIRStatus.ConditionalApproval;
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
        this.successMessage = 'WIR record rejected successfully!';
        this.wirUpdated.emit(updatedWIR);
        
        // Close modal after showing success message
        setTimeout(() => {
          this.close();
        }, 1500);
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

