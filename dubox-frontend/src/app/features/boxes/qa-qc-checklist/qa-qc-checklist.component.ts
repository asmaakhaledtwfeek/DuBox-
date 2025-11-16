import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { WIRService } from '../../../core/services/wir.service';
import { BoxService } from '../../../core/services/box.service';
import { AuthService } from '../../../core/services/auth.service';
import { WIRRecord, WIRChecklistItem, CheckpointStatus, ApproveWIRRequest, RejectWIRRequest } from '../../../core/models/wir.model';
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
  projectId!: string;
  boxId!: string;
  activityId!: string;
  
  checklistForm!: FormGroup;
  loading = true;
  error = '';
  submitting = false;
  
  signatureDataUrl: string = '';
  uploadedPhotos: File[] = [];
  photoPreviewUrls: string[] = [];
  
  CheckpointStatus = CheckpointStatus;
  isDrawing = false;
  signatureCanvas!: HTMLCanvasElement;
  signatureContext!: CanvasRenderingContext2D;

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
    this.checklistForm = this.fb.group({
      checklistItems: this.fb.array([]),
      inspectionNotes: ['', [Validators.maxLength(1000)]],
      rejectionReason: [''],
      signature: ['', Validators.required]
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
          // Get the pending WIR record
          this.wirRecord = wirs.find(w => w.status === 'Pending') || wirs[0];
          this.loadChecklistItems();
        } else {
          // Create new WIR record if none exists
          this.error = 'No WIR record found for this activity. Please create one first.';
        }
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load WIR record';
        this.loading = false;
        console.error('Error loading WIR:', err);
      }
    });
  }

  private loadChecklistItems(): void {
    if (!this.wirRecord) return;
    
    // Get predefined checklist template based on WIR code
    const template = this.wirService.getChecklistTemplate(this.wirRecord.wirCode);
    
    // Clear existing items
    this.checklistItems.clear();
    
    // Add checklist items to form
    template.forEach(item => {
      this.checklistItems.push(this.fb.group({
        sequence: [item.sequence],
        checkpointDescription: [item.checkpointDescription],
        referenceDocument: [item.referenceDocument],
        status: [CheckpointStatus.Pending, Validators.required],
        remarks: ['']
      }));
    });
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
    const items = this.checklistItems.value;
    const allChecked = items.every((item: any) => item.status !== CheckpointStatus.Pending);
    const noFailures = items.every((item: any) => item.status !== CheckpointStatus.Fail);
    const hasSignature = !!this.checklistForm.value.signature;
    
    return allChecked && noFailures && hasSignature && this.checklistForm.valid;
  }

  canReject(): boolean {
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
    if (!this.canApprove() || !this.wirRecord) return;
    
    this.submitting = true;
    
    const request: ApproveWIRRequest = {
      wirRecordId: this.wirRecord.wirRecordId,
      inspectionNotes: this.checklistForm.value.inspectionNotes,
      signature: this.checklistForm.value.signature,
      checklistItems: this.checklistItems.value
    };
    
    // Upload photos first if any
    if (this.uploadedPhotos.length > 0) {
      this.wirService.uploadInspectionPhotos(this.wirRecord.wirRecordId, this.uploadedPhotos).subscribe({
        next: (photoUrls) => {
          request.photoUrls = photoUrls.join(',');
          this.submitApproval(request);
        },
        error: (err) => {
          console.error('Photo upload failed:', err);
          // Continue with approval even if photos fail
          this.submitApproval(request);
        }
      });
    } else {
      this.submitApproval(request);
    }
  }

  private submitApproval(request: ApproveWIRRequest): void {
    this.wirService.approveWIRRecord(request).subscribe({
      next: () => {
        this.submitting = false;
        alert('WIR Record Approved Successfully!');
        this.goBack();
      },
      error: (err) => {
        this.submitting = false;
        this.error = err.error?.message || 'Failed to approve WIR record';
        console.error('Approval error:', err);
      }
    });
  }

  onReject(): void {
    if (!this.canReject() || !this.wirRecord) return;
    
    if (!confirm('Are you sure you want to reject this WIR? This will require rework.')) {
      return;
    }
    
    this.submitting = true;
    
    const request: RejectWIRRequest = {
      wirRecordId: this.wirRecord.wirRecordId,
      rejectionReason: this.checklistForm.value.rejectionReason,
      inspectionNotes: this.checklistForm.value.inspectionNotes,
      signature: this.checklistForm.value.signature
    };
    
    this.wirService.rejectWIRRecord(request).subscribe({
      next: () => {
        this.submitting = false;
        alert('WIR Record Rejected. Notification sent for rework.');
        this.goBack();
      },
      error: (err) => {
        this.submitting = false;
        this.error = err.error?.message || 'Failed to reject WIR record';
        console.error('Rejection error:', err);
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
