import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { QualityIssueStatus } from '../../../core/models/issue-comment.model';
import { IssueCommentsComponent } from '../issue-comments/issue-comments.component';
import { ToastService } from '../../../core/services/toast.service';
import { environment } from '../../../../environments/environment';

interface QualityIssue {
  issueId: string;
  issueNumber: string;
  issueDescription?: string;
  status: string;
  resolutionDescription?: string;
}

@Component({
  selector: 'app-update-issue-status-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, IssueCommentsComponent],
  templateUrl: './update-issue-status-modal.component.html',
  styleUrls: ['./update-issue-status-modal.component.scss']
})
export class UpdateIssueStatusModalComponent implements OnChanges {
  @Input() isOpen: boolean = false;
  @Input() issue: QualityIssue | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() statusUpdated = new EventEmitter<void>();

  selectedStatus: string = '';
  resolutionDescription: string = '';
  comment: string = '';
  selectedFiles: File[] = [];
  imageUrls: string[] = [];
  isSubmitting: boolean = false;
  showComments: boolean = true;

  statuses = [
    { value: 'Open', label: 'Open', icon: 'circle', class: 'status-open' },
    { value: 'InProgress', label: 'In Progress', icon: 'arrow-clockwise', class: 'status-in-progress' },
    { value: 'Resolved', label: 'Resolved', icon: 'check-circle', class: 'status-resolved' },
    { value: 'Closed', label: 'Closed', icon: 'x-circle', class: 'status-closed' }
  ];

  constructor(
    private http: HttpClient,
    private toastService: ToastService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['issue'] && this.issue) {
      this.selectedStatus = this.issue.status || 'Open';
      this.resolutionDescription = this.issue.resolutionDescription || '';
      this.comment = '';
      this.selectedFiles = [];
      this.imageUrls = [];
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      const newFiles = Array.from(input.files);
      
      // Validate file types
      const validTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
      const invalidFiles = newFiles.filter(file => !validTypes.includes(file.type));
      
      if (invalidFiles.length > 0) {
        this.toastService.showError('Only image files (JPEG, PNG, GIF) are allowed');
        return;
      }

      // Add to selected files
      this.selectedFiles = [...this.selectedFiles, ...newFiles];
    }
  }

  removeFile(index: number): void {
    this.selectedFiles.splice(index, 1);
  }

  getStatusLabel(status: string): string {
    const statusObj = this.statuses.find(s => s.value === status);
    return statusObj?.label || status;
  }

  getStatusClass(status: string): string {
    const statusObj = this.statuses.find(s => s.value === status);
    return statusObj?.class || '';
  }

  requiresResolution(): boolean {
    return this.selectedStatus === 'Resolved' || this.selectedStatus === 'Closed';
  }

  isValid(): boolean {
    if (!this.selectedStatus) return false;
    if (this.requiresResolution() && !this.resolutionDescription.trim()) return false;
    return true;
  }

  async updateStatus(): Promise<void> {
    if (!this.isValid() || !this.issue) {
      this.toastService.showWarning('Please fill in all required fields');
      return;
    }

    this.isSubmitting = true;

    try {
      const formData = new FormData();
      formData.append('Status', this.selectedStatus);
      
      if (this.resolutionDescription) {
        formData.append('ResolutionDescription', this.resolutionDescription);
      }

      if (this.comment.trim()) {
        formData.append('Comment', this.comment.trim());
      }

      // Add files
      this.selectedFiles.forEach((file, index) => {
        formData.append('Files', file, file.name);
      });

      // Add image URLs if any
      this.imageUrls.forEach(url => {
        formData.append('ImageUrls', url);
      });

      const response = await this.http.put<any>(
        `${environment.apiUrl}/qualityissues/${this.issue.issueId}/status`,
        formData
      ).toPromise();

      if (response.isSuccess) {
        this.toastService.showSuccess('Issue status updated successfully');
        this.statusUpdated.emit();
        this.onClose();
      } else {
        this.toastService.showError(response.message || 'Failed to update status');
      }
    } catch (error: any) {
      console.error('Error updating status:', error);
      this.toastService.showError(error?.error?.message || 'Failed to update status');
    } finally {
      this.isSubmitting = false;
    }
  }

  onClose(): void {
    this.close.emit();
  }

  onBackdropClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('modal-backdrop')) {
      this.onClose();
    }
  }

  toggleComments(): void {
    this.showComments = !this.showComments;
  }
}

