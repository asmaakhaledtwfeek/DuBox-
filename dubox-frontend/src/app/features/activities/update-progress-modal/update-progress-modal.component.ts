import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { ActivityProgressStatus, BoxActivityDetail } from '../../../core/models/progress-update.model';

@Component({
  selector: 'app-update-progress-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './update-progress-modal.component.html',
  styleUrls: ['./update-progress-modal.component.scss']
})
export class UpdateProgressModalComponent implements OnInit, OnChanges, OnDestroy {
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

  // Photo upload state
  photoUrl: string = '';
  selectedFile: File | null = null;
  photoPreview: string | null = null;
  isUploadingPhoto = false;
  photoUploadError = '';
  cameraStream: MediaStream | null = null;
  showCamera = false;
  photoInputMethod: 'url' | 'upload' | 'camera' = 'url';

  constructor(
    private fb: FormBuilder,
    private progressUpdateService: ProgressUpdateService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Reinitialize form when activity changes or modal opens
    if (changes['activity'] || (changes['isOpen'] && this.isOpen && this.activity)) {
      this.initializeForm();
      // Clear previous errors and messages when opening
      if (this.isOpen) {
        this.errorMessage = '';
        this.successMessage = '';
        this.selectedFiles = [];
        this.photoUrl = '';
        this.selectedFile = null;
        this.photoPreview = null;
        this.showCamera = false;
        this.stopCamera();
      }
    }
  }

  ngOnDestroy(): void {
    this.stopCamera();
  }

  initializeForm(): void {
    if (!this.activity) return;
    
    this.progressForm = this.fb.group({
      progressPercentage: [
        this.activity.progressPercentage || 0, 
        [Validators.required, Validators.min(0), Validators.max(100)]
      ],
      workDescription: [''],
      issuesEncountered: ['']
    });
  }

  onFileSelected(event: any): void {
    const files = event.target.files;
    if (files && files.length > 0) {
      const file = files[0];
      if (file.type.startsWith('image/')) {
        this.selectedFile = file;
        this.photoUrl = ''; // Clear URL when file is selected
        this.photoInputMethod = 'upload';
        this.previewImage(file);
      } else {
        this.photoUploadError = 'Please select an image file';
      }
    }
  }

  openFileInput(): void {
    this.showCamera = false;
    const fileInput = document.getElementById('progress-photo-file-input') as HTMLInputElement;
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
        const video = document.getElementById('progress-camera-preview') as HTMLVideoElement;
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
    const video = document.getElementById('progress-camera-preview') as HTMLVideoElement;
    if (!video) return;

    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const ctx = canvas.getContext('2d');
    if (ctx) {
      ctx.drawImage(video, 0, 0);
      canvas.toBlob((blob) => {
        if (blob) {
          const file = new File([blob], `progress-photo-${Date.now()}.jpg`, { type: 'image/jpeg' });
          this.selectedFile = file;
          this.previewImage(file);
          this.stopCamera();
        }
      }, 'image/jpeg', 0.9);
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
    this.photoUploadError = '';

    try {
      // Truncate device info to 100 characters (database limit)
      const deviceInfo = navigator.userAgent.substring(0, 100);

      const request = {
        boxId: this.activity.boxId,
        boxActivityId: this.activity.boxActivityId,
        progressPercentage: this.progressForm.value.progressPercentage || 0,
        workDescription: this.progressForm.value.workDescription || undefined,
        issuesEncountered: this.progressForm.value.issuesEncountered || undefined,
        imageUrl: this.photoUrl?.trim() || undefined,
        updateMethod: 'Web',
        deviceInfo: deviceInfo
      };

      // Send file directly with the request if provided
      const fileToSend = this.selectedFile || undefined;

      this.progressUpdateService.createProgressUpdate(request, fileToSend).subscribe({
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
    this.selectedFile = null;
    this.photoPreview = null;
    this.photoUrl = '';
    this.showCamera = false;
    this.stopCamera();
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

