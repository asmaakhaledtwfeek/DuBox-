import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { ActivityProgressStatus, BoxActivityDetail } from '../../../core/models/progress-update.model';

@Component({
  selector: 'app-update-progress-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './update-progress-modal.component.html',
  styleUrls: ['./update-progress-modal.component.scss']
})
export class UpdateProgressModalComponent implements OnInit {
  @Input() activity!: BoxActivityDetail;
  @Input() isOpen: boolean = false;
  @Output() closeModal = new EventEmitter<void>();
  @Output() progressUpdated = new EventEmitter<any>();

  progressForm!: FormGroup;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  selectedFiles: File[] = [];
  ActivityProgressStatus = ActivityProgressStatus;

  constructor(
    private fb: FormBuilder,
    private progressUpdateService: ProgressUpdateService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.progressForm = this.fb.group({
      progressPercentage: [
        this.activity?.progressPercentage || 0, 
        [Validators.required, Validators.min(0), Validators.max(100)]
      ],
      workDescription: [''],
      issuesEncountered: ['']
    });
  }

  onFileSelected(event: any): void {
    const files = event.target.files;
    if (files) {
      this.selectedFiles = Array.from(files);
    }
  }

  incrementProgress(amount: number): void {
    const currentProgress = this.progressForm.get('progressPercentage')?.value || 0;
    const newProgress = Math.min(100, Math.max(0, currentProgress + amount));
    this.progressForm.patchValue({ progressPercentage: newProgress });
  }

  async onSubmit(): Promise<void> {
    if (this.progressForm.invalid) {
      this.errorMessage = 'Please fill in all required fields correctly';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    try {
      let photoUrlsArray: string[] = [];
      
      // Upload photos if any
      if (this.selectedFiles.length > 0) {
        try {
          const uploadedUrls = await this.progressUpdateService.uploadProgressPhotos(this.selectedFiles).toPromise();
          photoUrlsArray = uploadedUrls || [];
        } catch (uploadError) {
          console.error('Photo upload failed:', uploadError);
          // Continue anyway, photos are optional
        }
      }

      // Truncate device info to 100 characters (database limit)
      const deviceInfo = navigator.userAgent.substring(0, 100);

      const request = {
        boxId: this.activity.boxId,
        boxActivityId: this.activity.boxActivityId,
        progressPercentage: this.progressForm.value.progressPercentage || 0,
        workDescription: this.progressForm.value.workDescription || undefined,
        issuesEncountered: this.progressForm.value.issuesEncountered || undefined,
        photoUrls: photoUrlsArray.length > 0 ? photoUrlsArray : undefined,
        updateMethod: 'Web',
        deviceInfo: deviceInfo
      };

      this.progressUpdateService.createProgressUpdate(request).subscribe({
        next: (response) => {
          this.successMessage = 'Progress updated successfully!';
          
          // Show WIR creation message if applicable
          if (response.wirCreated) {
            this.successMessage += ` WIR ${response.wirCode} has been automatically created for QC inspection.`;
          }

          setTimeout(() => {
            this.progressUpdated.emit(response);
            this.close();
          }, 2000);
        },
        error: (error) => {
          console.error('Error updating progress:', error);
          this.errorMessage = error.error?.message || error.error?.title || 'Failed to update progress. Please try again.';
          this.isSubmitting = false;
        }
      });
    } catch (error) {
      console.error('Error:', error);
      this.errorMessage = 'An unexpected error occurred';
      this.isSubmitting = false;
    }
  }

  close(): void {
    this.progressForm.reset();
    this.selectedFiles = [];
    this.errorMessage = '';
    this.successMessage = '';
    this.closeModal.emit();
  }

  get isWIRCheckpoint(): boolean {
    return this.activity?.isWIRCheckpoint || false;
  }

  get wirCode(): string {
    return this.activity?.wirCode || '';
  }
}

