import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { WIRService } from '../../../core/services/wir.service';
import { ActivityProgressStatus, BoxActivityDetail } from '../../../core/models/progress-update.model';
import { WIRRecord } from '../../../core/models/wir.model';

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
  @Input() allActivities: BoxActivityDetail[] = []; // All activities to find nearest WIR
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

  // WIR position fields
  nearestWIR: WIRRecord | null = null;
  hasWIRBelow: boolean = false;
  hasWIRActivityBelow: boolean = false; // Track if WIR activity exists (regardless of record)

  constructor(
    private fb: FormBuilder,
    private progressUpdateService: ProgressUpdateService,
    private wirService: WIRService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Reinitialize form when activity changes, allActivities changes, or modal opens
    if (changes['activity'] || changes['allActivities'] || (changes['isOpen'] && this.isOpen && this.activity)) {
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
    
    // Find nearest WIR below this activity in sequence
    this.findNearestWIRBelow();
    
    this.progressForm = this.fb.group({
      progressPercentage: [
        this.activity.progressPercentage || 0, 
        [Validators.required, Validators.min(0), Validators.max(100)]
      ],
      workDescription: [''],
      issuesEncountered: [''],
      // WIR Position fields - editable when empty, read-only when filled
      wirBay: [''],
      wirRow: [''],
      wirPosition: [{value: '', disabled: true}] // Position is always calculated, never manually editable
    });

    // Setup automatic calculation of Position = Bay × Row
    this.setupPositionCalculation();
  }

  /**
   * Setup automatic calculation of Position = Bay × Row
   * Bay is a text field (e.g., "A", "B"), Row is numeric
   * Position is calculated as numeric multiplication if Bay is numeric, otherwise concatenated
   * Note: Fields are NOT disabled here - that's handled by populateWIRPositionFields based on WIR state
   */
  private setupPositionCalculation(): void {
    const bayControl = this.progressForm.get('wirBay');
    const rowControl = this.progressForm.get('wirRow');
    const positionControl = this.progressForm.get('wirPosition');

    if (bayControl && rowControl && positionControl) {
      const calculatePosition = () => {
        const bayValue = bayControl.value?.toString().trim();
        const rowValue = rowControl.value?.toString().trim();
        
        if (bayValue && rowValue) {
          // Try to parse bay as number for multiplication
          const bayNum = parseInt(bayValue);
          const rowNum = parseInt(rowValue) || 0;
          
          // If bay is numeric, multiply; otherwise concatenate with a hyphen
          let position: string;
          if (!isNaN(bayNum)) {
            position = (bayNum * rowNum).toString();
          } else {
            // Bay is text (e.g., "A"), so concatenate: "A-3"
            position = `${bayValue}-${rowValue}`;
          }
          
          positionControl.setValue(position, { emitEvent: false });
        } else {
          positionControl.setValue('', { emitEvent: false });
        }
        
        // Don't disable fields here - let populateWIRPositionFields handle it
        // This allows each WIR section to have editable fields
      };

      bayControl.valueChanges.subscribe(() => calculatePosition());
      rowControl.valueChanges.subscribe(() => calculatePosition());
      
      // Initial calculation in case values are loaded
      calculatePosition();
    }
  }

  /**
   * Find the nearest WIR record below the current activity in sequence
   */
  private findNearestWIRBelow(): void {
    if (!this.activity || !this.allActivities || this.allActivities.length === 0) {
      this.hasWIRBelow = false;
      this.hasWIRActivityBelow = false;
      this.nearestWIR = null;
      return;
    }

    const currentSequence = this.activity.sequence || 0;
    
    // If current activity IS a WIR checkpoint, use it
    if (this.activity.isWIRCheckpoint || this.activity.activityMaster?.isWIRCheckpoint) {
      // Load WIR records to find if this activity has a WIR record
      if (this.activity.boxId) {
        this.wirService.getWIRRecordsByBox(this.activity.boxId).subscribe({
          next: (wirRecords) => {
            const currentWIR = wirRecords.find(wir => wir.boxActivityId === this.activity.boxActivityId);
            if (currentWIR) {
              this.nearestWIR = currentWIR;
              this.hasWIRBelow = true;
              this.hasWIRActivityBelow = true;
              this.populateWIRPositionFields(currentWIR);
            }
          },
          error: (error) => {
            console.error('Error loading WIR records:', error);
          }
        });
      }
      return;
    }
    
    // Find activities with sequence greater than current
    const activitiesBelow = this.allActivities
      .filter(a => (a.sequence || 0) > currentSequence)
      .sort((a, b) => (a.sequence || 0) - (b.sequence || 0));

    if (activitiesBelow.length === 0) {
      this.hasWIRBelow = false;
      this.hasWIRActivityBelow = false;
      this.nearestWIR = null;
      return;
    }

    // Check if there's a WIR activity below (regardless of whether record exists)
    const wirActivityBelow = activitiesBelow.find(a => a.isWIRCheckpoint || a.activityMaster?.isWIRCheckpoint);
    this.hasWIRActivityBelow = !!wirActivityBelow;

    // Load WIR records for the box to find the nearest WIR
    if (this.activity.boxId) {
      this.wirService.getWIRRecordsByBox(this.activity.boxId).subscribe({
        next: (wirRecords) => {
          // Find WIR records for activities below current
          const wirRecordsBelow = wirRecords.filter(wir => {
            const wirActivity = activitiesBelow.find(a => a.boxActivityId === wir.boxActivityId);
            return wirActivity !== undefined;
          });

          if (wirRecordsBelow.length > 0) {
            // Get the nearest WIR (lowest sequence)
            const nearestActivity = activitiesBelow.find(a => 
              wirRecordsBelow.some(wir => wir.boxActivityId === a.boxActivityId)
            );
            
            if (nearestActivity) {
              this.nearestWIR = wirRecordsBelow.find(wir => 
                wir.boxActivityId === nearestActivity.boxActivityId
              ) || null;
              this.hasWIRBelow = !!this.nearestWIR;
              
              // Populate WIR position fields with existing values
              if (this.nearestWIR) {
                this.populateWIRPositionFields(this.nearestWIR);
              }
            } else {
              this.hasWIRBelow = false;
              this.nearestWIR = null;
            }
          } else if (wirActivityBelow) {
            // WIR activity exists but record hasn't been created yet
            // Don't inherit from previous WIRs - each WIR section should have its own values
            // Leave fields empty so user can set new values for this WIR
            this.hasWIRBelow = false;
            this.nearestWIR = null;
          } else {
            this.hasWIRBelow = false;
            this.nearestWIR = null;
          }
        },
        error: (error) => {
          console.error('Error loading WIR records:', error);
          this.hasWIRBelow = false;
          this.nearestWIR = null;
        }
      });
    } else {
      this.hasWIRBelow = false;
      this.nearestWIR = null;
    }
  }

  /**
   * Populate WIR position fields and make them read-only if already set
   * Only disable fields if this is the SAME WIR being edited
   */
  private populateWIRPositionFields(wir: WIRRecord): void {
    if (!this.progressForm) return;
    
    const bayValue = wir.bay || '';
    const rowValue = wir.row || '';
    const positionValue = wir.position || '';
    
    // Patch the form values
    this.progressForm.patchValue({
      wirBay: bayValue,
      wirRow: rowValue,
      wirPosition: positionValue
    }, { emitEvent: false });
    
    // Only make fields read-only if values exist AND this WIR record matches the nearest WIR
    // This allows new WIR sections to have their own editable position values
    const isNearestWIR = this.nearestWIR?.wirRecordId === wir.wirRecordId;
    
    if (isNearestWIR && bayValue) {
      this.progressForm.get('wirBay')?.disable();
    }
    if (isNearestWIR && rowValue) {
      this.progressForm.get('wirRow')?.disable();
    }
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

      // Use getRawValue() to include disabled field values (wirBay, wirRow, wirPosition)
      const formValues = this.progressForm.getRawValue();

      const request = {
        boxId: this.activity.boxId,
        boxActivityId: this.activity.boxActivityId,
        progressPercentage: formValues.progressPercentage || 0,
        workDescription: formValues.workDescription || undefined,
        issuesEncountered: formValues.issuesEncountered || undefined,
        updateMethod: 'Web',
        deviceInfo: deviceInfo,
        // Include WIR position fields: Send values if they exist
        // Bay and Row can be entered by user (editable when empty, locked when filled)
        // Position is always auto-calculated (Bay × Row)
        wirBay: formValues.wirBay?.toString().trim() || undefined,
        wirRow: formValues.wirRow?.toString().trim() || undefined,
        wirPosition: formValues.wirPosition?.toString().trim() || undefined
      };

      this.progressUpdateService.createProgressUpdate(request, files, imageUrls).subscribe({
        next: (response) => {
          this.successMessage = 'Progress updated successfully!';
          
          // Clear cache for this box so progress history shows updated data
          this.progressUpdateService.clearCache(this.activity.boxId);
          
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

