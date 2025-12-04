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

  // Photo upload state - multiple images support
  selectedImages: Array<{ type: 'file' | 'url'; file?: File; url?: string; preview?: string; name?: string }> = [];
  currentPhotoUrl: string = '';
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
      this.selectedImages = [];
      this.currentPhotoUrl = '';
      this.showCamera = false;
      this.stopCamera();
      }
    }
  }

  ngOnDestroy(): void {
    // Cleanup: stop camera stream on component unmount
    const video = document.getElementById('progress-camera-preview') as HTMLVideoElement;
    if (video) {
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach((track) => track.stop());
      }
      video.srcObject = null;
    }
    
    // Also stop any stored stream reference
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach((track) => track.stop());
      this.cameraStream = null;
    }
    
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
      Array.from(files as FileList).forEach((file: File) => {
        if (file.type.startsWith('image/')) {
          this.addImageFile(file);
        } else {
          this.photoUploadError = 'Please select image files only';
        }
      });
      // Reset file input to allow selecting the same file again
      const fileInput = event.target as HTMLInputElement;
      if (fileInput) {
        fileInput.value = '';
      }
    }
  }

  addImageFile(file: File): void {
    const reader = new FileReader();
    reader.onload = (e) => {
      this.selectedImages.push({
        type: 'file',
        file: file,
        preview: e.target?.result as string,
        name: file.name
      });
      this.photoInputMethod = 'upload';
      this.photoUploadError = '';
    };
    reader.readAsDataURL(file);
  }

  addImageFromDataUrl(imageData: string): void {
    // Convert data URL to File for consistency with existing structure
    fetch(imageData)
      .then(res => res.blob())
      .then(blob => {
        const file = new File([blob], `progress-photo-${Date.now()}.jpg`, { type: 'image/jpeg' });
        this.selectedImages.push({
          type: 'file',
          file: file,
          preview: imageData,
          name: file.name
        });
        this.photoInputMethod = 'camera';
        this.photoUploadError = '';
      })
      .catch(err => {
        console.error('Error converting data URL to file:', err);
        this.photoUploadError = 'Failed to process captured image.';
      });
  }

  addImageUrl(url: string): void {
    if (url && url.trim()) {
      // Validate URL format
      try {
        new URL(url);
        this.selectedImages.push({
          type: 'url',
          url: url.trim(),
          preview: url.trim() // Use URL as preview
        });
        this.currentPhotoUrl = '';
        this.photoInputMethod = 'url';
        this.photoUploadError = '';
      } catch {
        this.photoUploadError = 'Please enter a valid URL';
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

  removeImage(index: number): void {
    this.selectedImages.splice(index, 1);
    this.photoUploadError = '';
  }

  clearAllImages(): void {
    this.selectedImages = [];
    this.currentPhotoUrl = '';
    this.photoUploadError = '';
  }


  // Camera methods
  async openCamera(): Promise<void> {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ 
        video: { 
          facingMode: 'environment' // Use back camera on mobile
        } 
      });
      this.cameraStream = stream;
      this.showCamera = true;
      this.photoInputMethod = 'camera';
      
      // Wait for video element to be rendered
      setTimeout(() => {
        const video = document.getElementById('progress-camera-preview') as HTMLVideoElement;
        const cameraContainer = document.getElementById('camera-preview-container') as HTMLElement;
        
        if (!video) {
          console.error('Video element not found');
          return;
        }
        
        // Set video source and play
        video.srcObject = stream;
        video.muted = true;
        video.playsInline = true;
        video.autoplay = true;
        video.play();
        
        // Wait for video to start playing before going fullscreen
        const handlePlaying = () => {
          console.log('Video is playing, requesting fullscreen');
          
          // Request fullscreen for camera container
          if (cameraContainer) {
            const requestFullscreen = () => {
              if (cameraContainer.requestFullscreen) {
                return cameraContainer.requestFullscreen();
              } else if ((cameraContainer as any).webkitRequestFullscreen) {
                return (cameraContainer as any).webkitRequestFullscreen();
              } else if ((cameraContainer as any).mozRequestFullScreen) {
                return (cameraContainer as any).mozRequestFullScreen();
              } else if ((cameraContainer as any).msRequestFullscreen) {
                return (cameraContainer as any).msRequestFullscreen();
              }
              return Promise.reject('Fullscreen not supported');
            };
            
            requestFullscreen().catch((err: unknown) => {
              console.warn('Error attempting to enable fullscreen:', err);
            });
          }
        };
        
        // Ensure video plays
        video.play().then(() => {
          console.log('Video play() resolved');
          // Wait a bit more to ensure video is actually rendering
          setTimeout(() => {
            if (video.readyState >= 2 && video.videoWidth > 0) {
              handlePlaying();
            } else {
              // Fallback: wait for playing event
              video.addEventListener('playing', handlePlaying, { once: true });
            }
          }, 300);
        }).catch(err => {
          console.error('Error playing video:', err);
          this.photoUploadError = 'Error starting camera preview.';
        });
        
        // Also listen for playing event as backup
        video.addEventListener('playing', () => {
          console.log('Video playing event fired');
        }, { once: true });
        
      }, 100);
    } catch (err) {
      console.error('Error accessing camera:', err);
      this.photoUploadError = 'Unable to access camera. Please check permissions.';
      this.showCamera = false;
    }
  }

  stopCamera(): void {
    // Exit fullscreen if active
    this.exitFullscreen();
    
    // Stop video stream
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach(track => track.stop());
      this.cameraStream = null;
    }
    
    // Clear video element
    const video = document.getElementById('progress-camera-preview') as HTMLVideoElement;
    if (video) {
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach(track => track.stop());
      }
      video.srcObject = null;
      video.pause();
    }
    
    this.showCamera = false;
  }

  capturePhoto(): void {
    const video = document.getElementById('progress-camera-preview') as HTMLVideoElement;
    
    if (!video) {
      this.photoUploadError = 'Camera element not found.';
      return;
    }
    
    // Check if video has valid dimensions
    if (!video.videoWidth || !video.videoHeight || video.videoWidth === 0 || video.videoHeight === 0) {
      console.warn('Video dimensions not ready:', { 
        width: video.videoWidth, 
        height: video.videoHeight,
        readyState: video.readyState 
      });
      this.photoUploadError = 'Camera not ready. Please wait a moment and try again.';
      return;
    }

    // Check if video is actually playing
    if (video.readyState < 2) {
      console.warn('Video not ready:', { readyState: video.readyState });
      this.photoUploadError = 'Camera stream not ready. Please wait a moment.';
      return;
    }
    
    // Check if video is paused
    if (video.paused) {
      console.warn('Video is paused, attempting to play');
      video.play().catch(err => {
        console.error('Error playing video for capture:', err);
        this.photoUploadError = 'Camera is paused. Please try again.';
        return;
      });
    }

    try {
      // Create canvas and draw video frame
      const canvas = document.createElement('canvas');
      canvas.width = video.videoWidth;
      canvas.height = video.videoHeight;
      
      const ctx = canvas.getContext('2d');
      
      if (!ctx) {
        this.photoUploadError = 'Unable to create canvas context.';
        return;
      }
      
      // Draw the current video frame to canvas
      ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
      
      // Convert to Base64 data URL
      const imageData = canvas.toDataURL('image/jpeg', 0.9);
      
      // Stop camera stream immediately before any async operations
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach((track) => track.stop());
      }
      
      // Clear video element srcObject
      video.srcObject = null;
      
      // Clear camera stream reference
      if (this.cameraStream) {
        this.cameraStream.getTracks().forEach((track) => track.stop());
        this.cameraStream = null;
      }
      
      // Close camera UI immediately
      this.showCamera = false;
      
      // Exit fullscreen
      this.exitFullscreen();
      
      // Add the captured image to the list
      this.addImageFromDataUrl(imageData);
      
    } catch (err) {
      console.error('Error capturing photo:', err);
      this.photoUploadError = 'Error capturing image. Please try again.';
      // Ensure camera is stopped even on error
      this.stopCamera();
    }
  }

  private exitFullscreen(): void {
    if (document.fullscreenElement || (document as any).webkitFullscreenElement || 
        (document as any).mozFullScreenElement || (document as any).msFullscreenElement) {
      if (document.exitFullscreen) {
        document.exitFullscreen().catch(err => console.warn('Error exiting fullscreen:', err));
      } else if ((document as any).webkitExitFullscreen) {
        (document as any).webkitExitFullscreen();
      } else if ((document as any).mozCancelFullScreen) {
        (document as any).mozCancelFullScreen();
      } else if ((document as any).msExitFullscreen) {
        (document as any).msExitFullscreen();
      }
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

      // Separate files and URLs
      const files: File[] = this.selectedImages
        .filter(img => img.type === 'file' && img.file)
        .map(img => img.file!);
      
      const imageUrls: string[] = this.selectedImages
        .filter(img => img.type === 'url' && img.url)
        .map(img => img.url!);

      const request = {
        boxId: this.activity.boxId,
        boxActivityId: this.activity.boxActivityId,
        progressPercentage: this.progressForm.value.progressPercentage || 0,
        workDescription: this.progressForm.value.workDescription || undefined,
        issuesEncountered: this.progressForm.value.issuesEncountered || undefined,
        updateMethod: 'Web',
        deviceInfo: deviceInfo
      };

      this.progressUpdateService.createProgressUpdate(request, files, imageUrls).subscribe({
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
    // Stop camera if it's open
    this.stopCamera();
    
    this.progressForm.reset();
    this.selectedFiles = [];
    this.selectedImages = [];
    this.currentPhotoUrl = '';
    this.showCamera = false;
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

