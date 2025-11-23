import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { WIRService } from '../../../core/services/wir.service';
import { BoxService } from '../../../core/services/box.service';
import { AuthService } from '../../../core/services/auth.service';
import { 
  WIRRecord, 
  WIRChecklistItem, 
  CheckpointStatus, 
  ApproveWIRRequest, 
  RejectWIRRequest,
  WIRCheckpoint,
  CreateWIRCheckpointRequest,
  AddChecklistItemsRequest,
  ReviewWIRCheckpointRequest,
  WIRCheckpointStatus,
  CheckListItemStatus,
  WIR_CHECKLIST_TEMPLATES
} from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

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
  
  loading = true;
  error = '';
  submitting = false;
  creatingCheckpoint = false;
  addingChecklistItems = false;
  
  signatureDataUrl: string = '';
  uploadedPhotos: File[] = [];
  photoPreviewUrls: string[] = [];
  
  CheckpointStatus = CheckpointStatus;
  WIRCheckpointStatus = WIRCheckpointStatus;
  CheckListItemStatus = CheckListItemStatus;
  isDrawing = false;
  signatureCanvas!: HTMLCanvasElement;
  signatureContext!: CanvasRenderingContext2D;
  
  // Step tracking: 'create-checkpoint' | 'add-items' | 'review'
  currentStep: 'create-checkpoint' | 'add-items' | 'review' = 'create-checkpoint';

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
    
    this.initForm();
    this.loadWIRRecord();
  }

  private initForm(): void {
    // Review form (for final review)
    this.checklistForm = this.fb.group({
      checklistItems: this.fb.array([]),
      inspectionNotes: ['', [Validators.maxLength(1000)]],
      rejectionReason: [''],
      signature: ['', Validators.required]
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
  }

  get checklistItems(): FormArray {
    return this.checklistForm.get('checklistItems') as FormArray;
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
          // Checkpoint exists, check if it has checklist items
          this.wirCheckpoint = checkpoint;
          if (checkpoint.checklistItems && checkpoint.checklistItems.length > 0) {
            // Has items, go to review step
            this.currentStep = 'review';
            this.loadChecklistItems();
          } else {
            // No items, go to add items step
            this.currentStep = 'add-items';
            this.loadChecklistTemplate();
          }
        } else {
          // No checkpoint exists, stay on create step
          this.currentStep = 'create-checkpoint';
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error checking WIR checkpoint:', err);
        // Stay on create step
        this.currentStep = 'create-checkpoint';
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
        this.creatingCheckpoint = false;
        // Move to add checklist items step
        this.currentStep = 'add-items';
        this.loadChecklistTemplate();
      },
      error: (err) => {
        this.creatingCheckpoint = false;
        this.error = err.error?.message || err.message || 'Failed to create WIR checkpoint';
        console.error('Error creating WIR checkpoint:', err);
      }
    });
  }

  loadChecklistTemplate(): void {
    if (!this.wirRecord) return;

    // Get checklist template based on WIR code
    const template = WIR_CHECKLIST_TEMPLATES[this.wirRecord.wirCode] || [];
    
    // Clear existing items
    const itemsArray = this.addChecklistItemsForm.get('checklistItems') as FormArray;
    itemsArray.clear();
    
    // Add checklist items to form from template
    template.forEach(item => {
      itemsArray.push(this.fb.group({
        checkpointDescription: [item.checkpointDescription, Validators.required],
        referenceDocument: [item.referenceDocument || ''],
        sequence: [item.sequence, Validators.required]
      }));
    });
  }

  onAddChecklistItems(): void {
    if (this.addChecklistItemsForm.invalid || !this.wirCheckpoint) {
      this.markFormGroupTouched(this.addChecklistItemsForm);
      return;
    }

    this.addingChecklistItems = true;
    this.error = '';

    const itemsArray = this.addChecklistItemsForm.get('checklistItems') as FormArray;
    const request: AddChecklistItemsRequest = {
      wirId: this.wirCheckpoint.wirId,
      items: itemsArray.value.map((item: any) => ({
        checkpointDescription: item.checkpointDescription.trim(),
        referenceDocument: item.referenceDocument?.trim() || undefined,
        sequence: item.sequence
      }))
    };

    this.wirService.addChecklistItems(request).subscribe({
      next: (updatedCheckpoint) => {
        this.wirCheckpoint = updatedCheckpoint;
        this.addingChecklistItems = false;
        // Move to review step
        this.currentStep = 'review';
        this.loadChecklistItems();
      },
      error: (err) => {
        this.addingChecklistItems = false;
        this.error = err.error?.message || err.message || 'Failed to add checklist items';
        console.error('Error adding checklist items:', err);
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
          checklistItemId: [item.checklistItemId],
          sequence: [item.sequence],
          checkpointDescription: [item.checkpointDescription],
          referenceDocument: [item.referenceDocument],
          status: [this.mapCheckListItemStatus(item.status), Validators.required],
          remarks: [item.remarks || '']
        }));
      });
    }
  }

  get addChecklistItemsArray(): FormArray {
    return this.addChecklistItemsForm.get('checklistItems') as FormArray;
  }

  private mapCheckListItemStatus(status: string): CheckpointStatus {
    const statusMap: Record<string, CheckpointStatus> = {
      'Pending': CheckpointStatus.Pending,
      'Pass': CheckpointStatus.Pass,
      'Fail': CheckpointStatus.Fail,
      'NA': CheckpointStatus.NA
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

  // Signature Pad Methods
  initSignaturePad(): void {
    setTimeout(() => {
      this.signatureCanvas = document.getElementById('signatureCanvas') as HTMLCanvasElement;
      if (this.signatureCanvas) {
        this.signatureContext = this.signatureCanvas.getContext('2d')!;
        this.signatureContext.strokeStyle = '#000';
        this.signatureContext.lineWidth = 2;
        this.signatureContext.lineCap = 'round';
      }
    }, 100);
  }

  startDrawing(event: MouseEvent | TouchEvent): void {
    this.isDrawing = true;
    const pos = this.getMousePos(event);
    this.signatureContext.beginPath();
    this.signatureContext.moveTo(pos.x, pos.y);
  }

  draw(event: MouseEvent | TouchEvent): void {
    if (!this.isDrawing) return;
    const pos = this.getMousePos(event);
    this.signatureContext.lineTo(pos.x, pos.y);
    this.signatureContext.stroke();
  }

  stopDrawing(): void {
    if (!this.isDrawing) return;
    this.isDrawing = false;
    this.signatureContext.closePath();
    this.signatureDataUrl = this.signatureCanvas.toDataURL();
    this.checklistForm.patchValue({ signature: this.signatureDataUrl });
  }

  clearSignature(): void {
    if (this.signatureContext) {
      this.signatureContext.clearRect(0, 0, this.signatureCanvas.width, this.signatureCanvas.height);
      this.signatureDataUrl = '';
      this.checklistForm.patchValue({ signature: '' });
    }
  }

  private getMousePos(event: MouseEvent | TouchEvent): { x: number; y: number } {
    const rect = this.signatureCanvas.getBoundingClientRect();
    const clientX = event instanceof MouseEvent ? event.clientX : event.touches[0].clientX;
    const clientY = event instanceof MouseEvent ? event.clientY : event.touches[0].clientY;
    return {
      x: clientX - rect.left,
      y: clientY - rect.top
    };
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
  canApprove(): boolean {
    if (!this.wirCheckpoint) return false;
    const items = this.checklistItems.value;
    const allChecked = items.every((item: any) => item.status !== CheckpointStatus.Pending);
    const noFailures = items.every((item: any) => item.status !== CheckpointStatus.Fail);
    const hasSignature = !!this.checklistForm.value.signature;
    
    return allChecked && noFailures && hasSignature && this.checklistForm.valid;
  }

  canReject(): boolean {
    if (!this.wirCheckpoint) return false;
    const hasRejectionReason = !!this.checklistForm.value.rejectionReason;
    const hasSignature = !!this.checklistForm.value.signature;
    
    return hasRejectionReason && hasSignature;
  }

  hasFailedItems(): boolean {
    const items = this.checklistItems.value;
    return items.some((item: any) => item.status === CheckpointStatus.Fail);
  }

  // Submit Methods
  onApprove(): void {
    if (!this.canApprove() || !this.wirCheckpoint) return;
    
    this.submitReview(WIRCheckpointStatus.Approved);
  }

  onReject(): void {
    if (!this.canReject() || !this.wirCheckpoint) return;
    
    if (!confirm('Are you sure you want to reject this WIR checkpoint? This will require rework.')) {
      return;
    }
    
    this.submitReview(WIRCheckpointStatus.Rejected);
  }

  private submitReview(status: WIRCheckpointStatus): void {
    if (!this.wirCheckpoint) return;

    this.submitting = true;
    this.error = '';

    // Map form items to review items
    const reviewItems = this.checklistItems.value.map((item: any) => ({
      checklistItemId: item.checklistItemId || '',
      remarks: item.remarks?.trim() || undefined,
      status: this.mapToCheckListItemStatus(item.status)
    }));

    const request: ReviewWIRCheckpointRequest = {
      wirId: this.wirCheckpoint.wirId,
      status: status,
      comment: this.checklistForm.value.inspectionNotes?.trim() || undefined,
      items: reviewItems
    };

    this.wirService.reviewWIRCheckpoint(request).subscribe({
      next: (updatedCheckpoint) => {
        this.submitting = false;
        this.wirCheckpoint = updatedCheckpoint;
        const message = status === WIRCheckpointStatus.Approved 
          ? 'WIR Checkpoint Approved Successfully!' 
          : 'WIR Checkpoint Rejected. Notification sent for rework.';
        alert(message);
        this.goBack();
      },
      error: (err) => {
        this.submitting = false;
        this.error = err.error?.message || err.message || `Failed to ${status === WIRCheckpointStatus.Approved ? 'approve' : 'reject'} WIR checkpoint`;
        console.error('Review error:', err);
      }
    });
  }

  private mapToCheckListItemStatus(status: CheckpointStatus): CheckListItemStatus {
    const statusMap: Record<string, CheckListItemStatus> = {
      'Pending': CheckListItemStatus.Pending,
      'Pass': CheckListItemStatus.Pass,
      'Fail': CheckListItemStatus.Fail,
      'N/A': CheckListItemStatus.NA,
      'NA': CheckListItemStatus.NA
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

  goBack(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId]);
  }

  ngAfterViewInit(): void {
    this.initSignaturePad();
  }
}
