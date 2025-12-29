import { Component, EventEmitter, Input, Output, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';

@Component({
  selector: 'app-upload-drawing-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './upload-drawing-modal.component.html',
  styleUrls: ['./upload-drawing-modal.component.scss']
})
export class UploadDrawingModalComponent implements OnInit, OnChanges {
  @Input() isOpen = false;
  @Input() boxId: string = '';
  @Input() boxName: string = '';
  @Input() isProjectOnHold: boolean = false; // Track if project is on hold
  @Input() isProjectArchived: boolean = false; // Track if project is archived
  @Output() closed = new EventEmitter<void>();
  @Output() uploaded = new EventEmitter<void>();

  // Input modes
  inputMethod: 'url' | 'file' = 'url';
  
  // URL input
  drawingUrl: string = '';
  
  // File input
  selectedFile: File | null = null;
  filePreview: { name: string; size: number; type: string } | null = null;
  
  // State
  isSubmitting = false;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(private boxService: BoxService) {}

  ngOnInit(): void {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen'] && changes['isOpen'].currentValue) {
      this.resetForm();
    }
    
    // Show error message when modal opens if project is on hold or archived
    if (changes['isOpen'] && changes['isOpen'].currentValue && (this.isProjectOnHold || this.isProjectArchived)) {
      this.errorMessage = this.isProjectArchived 
        ? 'Cannot upload drawings. This project is archived and read-only.' 
        : 'Cannot upload drawings. This project is on hold. Only project status changes are allowed.';
    }
  }

  close(): void {
    if (!this.isSubmitting) {
      this.resetForm();
      this.closed.emit();
    }
  }

  resetForm(): void {
    this.inputMethod = 'url';
    this.drawingUrl = '';
    this.selectedFile = null;
    this.filePreview = null;
    this.errorMessage = '';
    this.successMessage = '';
    this.isSubmitting = false;
  }

  setInputMethod(method: 'url' | 'file'): void {
    if (this.isProjectOnHold || this.isProjectArchived) {
      this.errorMessage = this.isProjectArchived 
        ? 'Cannot upload drawings. This project is archived and read-only.' 
        : 'Cannot upload drawings. This project is on hold. Only project status changes are allowed.';
      return;
    }
    this.inputMethod = method;
    this.errorMessage = '';
    this.successMessage = '';
  }

  openFileInput(): void {
    if (this.isProjectOnHold || this.isProjectArchived) {
      this.errorMessage = this.isProjectArchived 
        ? 'Cannot upload drawings. This project is archived and read-only.' 
        : 'Cannot upload drawings. This project is on hold. Only project status changes are allowed.';
      return;
    }
    this.setInputMethod('file');
    const fileInput = document.getElementById('drawing-file-input') as HTMLInputElement;
    if (fileInput) {
      fileInput.click();
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      
      // Validate file type
      const extension = file.name.split('.').pop()?.toLowerCase();
      if (extension !== 'pdf' && extension !== 'dwg') {
        this.errorMessage = 'Only PDF and DWG files are allowed.';
        this.selectedFile = null;
        this.filePreview = null;
        return;
      }
      
      // Validate file size (50 MB max)
      const maxSize = 50 * 1024 * 1024; // 50 MB
      if (file.size > maxSize) {
        this.errorMessage = 'File size cannot exceed 50 MB.';
        this.selectedFile = null;
        this.filePreview = null;
        return;
      }
      
      this.selectedFile = file;
      this.filePreview = {
        name: file.name,
        size: file.size,
        type: file.type
      };
      this.errorMessage = '';
    }
  }

  removeFile(): void {
    this.selectedFile = null;
    this.filePreview = null;
    const fileInput = document.getElementById('drawing-file-input') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 KB';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i];
  }

  canSubmit(): boolean {
    if (this.isSubmitting) return false;
    
    if (this.inputMethod === 'url') {
      return this.drawingUrl.trim().length > 0;
    } else {
      return this.selectedFile !== null;
    }
  }

  onSubmit(): void {
    if (this.isProjectArchived || this.isProjectOnHold) {
      this.errorMessage = this.isProjectArchived 
        ? 'Cannot upload drawings. This project is archived and read-only.' 
        : 'Cannot upload drawings. This project is on hold. Only project status changes are allowed.';
      return;
    }

    if (!this.canSubmit()) return;
    
    this.errorMessage = '';
    this.successMessage = '';
    this.isSubmitting = true;
    
    const request: { boxId: string; drawingUrl?: string; file?: File } = {
      boxId: this.boxId
    };
    
    if (this.inputMethod === 'url') {
      request.drawingUrl = this.drawingUrl.trim();
    } else if (this.selectedFile) {
      request.file = this.selectedFile;
    }
    
    this.boxService.uploadBoxDrawing(request).subscribe({
      next: (response) => {
        console.log('✅ Drawing uploaded successfully:', response);
        this.successMessage = 'Drawing uploaded successfully!';
        this.isSubmitting = false;
        
        // Wait a moment to show success message, then close and notify parent
        setTimeout(() => {
          this.uploaded.emit();
          this.close();
        }, 1000);
      },
      error: (error) => {
        console.error('❌ Error uploading drawing:', error);
        this.errorMessage = error?.error?.message || error?.message || 'Failed to upload drawing. Please try again.';
        this.isSubmitting = false;
      }
    });
  }
}

