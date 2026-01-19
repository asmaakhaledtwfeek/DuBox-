import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { QualityIssueDetails, QualityIssueStatus, WIRCheckpointStatus } from '../../../core/models/wir.model';
import { WIRCheckpoint } from '../../../core/models/wir.model';
import { environment } from '../../../../environments/environment';
import { IssueCommentsComponent } from '../issue-comments/issue-comments.component';
import { HttpClient } from '@angular/common/http';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-quality-issue-details-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, IssueCommentsComponent],
  templateUrl: './quality-issue-details-modal.component.html',
  styleUrls: ['./quality-issue-details-modal.component.scss']
})
export class QualityIssueDetailsModalComponent implements OnInit, OnChanges {
  @Input() issue: QualityIssueDetails | null = null;
  @Input() wirCheckpoints: WIRCheckpoint[] = []; // For WIR lookup
  @Input() isOpen = false;
  @Input() commentId?: string; // Optional: Scroll to specific comment
  @Output() close = new EventEmitter<void>();
  @Output() statusUpdated = new EventEmitter<void>();

  // Update Status
  showUpdateStatus = false;
  selectedStatus: string = '';
  resolutionDescription: string = '';
  statusComment: string = '';
  selectedFiles: File[] = [];
  isSubmitting = false;

  statuses = [
    { value: 'Open', label: 'Open', icon: 'circle', class: 'status-open' },
    { value: 'InProgress', label: 'In Progress', icon: 'arrow-repeat', class: 'status-inprogress' },
    { value: 'Resolved', label: 'Resolved', icon: 'check-circle', class: 'status-resolved' },
    { value: 'Closed', label: 'Closed', icon: 'x-circle', class: 'status-closed' }
  ];

  qualityIssueStatusMeta: Record<QualityIssueStatus, { label: string; class: string }> = {
    Open: { label: 'Open', class: 'status-open' },
    InProgress: { label: 'In Progress', class: 'status-inprogress' },
    Resolved: { label: 'Resolved', class: 'status-resolved' },
    Closed: { label: 'Closed', class: 'status-closed' }
  };

  imageUrls: string[] = [];

  constructor(
    private http: HttpClient,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.updateImageUrls();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['issue'] && this.issue) {
      console.log('🎭 Modal received issue data:', this.issue);
      console.log('🎭 Modal IssueNumber:', this.issue.issueNumber);
      console.log('🎭 Modal AssignedTeamName:', this.issue.assignedTeamName);
      console.log('🎭 Modal AssignedToUserName:', this.issue.assignedToUserName);
      console.log('🎭 Modal CCUserName:', this.issue.ccUserName);
      this.updateImageUrls();
      this.selectedStatus = this.issue.status || 'Open';
      this.resolutionDescription = this.issue.resolutionDescription || '';
      this.statusComment = '';
      this.selectedFiles = [];
      this.showUpdateStatus = false;
    }
  }

  private updateImageUrls(): void {
    if (!this.issue) {
      this.imageUrls = [];
      return;
    }

    // First, try to use the new Images array
    if (this.issue.images && Array.isArray(this.issue.images) && this.issue.images.length > 0) {
      this.imageUrls = this.issue.images
        .sort((a, b) => (a.sequence || 0) - (b.sequence || 0))
        .map((img) => {
          // Use imageUrl if available (new API returns URL for on-demand loading)
          if (img.imageUrl) {
            // Convert relative URL to full API URL
            const baseUrl = this.getApiBaseUrl();
            return img.imageUrl.startsWith('/') ? `${baseUrl}${img.imageUrl}` : img.imageUrl;
          }
          
          // Fallback to imageData for backward compatibility
          const imageData = img.imageData || '';
          // If it's already a data URL, return as is
          if (imageData.startsWith('data:image/')) {
            return imageData;
          }
          // If it's a URL, return as is
          if (imageData.startsWith('http://') || imageData.startsWith('https://')) {
            return imageData;
          }
          // Otherwise, assume it's base64 and add data URI prefix
          if (imageData) {
            return `data:image/jpeg;base64,${imageData}`;
          }
          return '';
        })
        .filter((url: string) => url && url.trim().length > 0);
    } else if (this.issue.photoPath) {
      // Fallback to old PhotoPath field (backward compatibility)
      this.imageUrls = [this.issue.photoPath];
    } else {
      this.imageUrls = [];
    }
  }

  private getApiBaseUrl(): string {
    // Use environment.apiUrl and remove /api suffix if present
    const apiUrl = environment.apiUrl || window.location.origin;
    return apiUrl.replace(/\/api\/?$/, '');
  }

  formatIssueDate(date?: string | Date): string {
    if (!date) {
      return '—';
    }

    const parsed = date instanceof Date ? date : new Date(date);
    if (isNaN(parsed.getTime())) {
      return '—';
    }

    return parsed.toISOString().split('T')[0];
  }

  /**
   * Transform WIR number to Stage number for display purposes only
   * E.g., "WIR-1" -> "Stage-1", "WIR-2" -> "Stage-2"
   */
  getDisplayWirNumber(wirNumber: string | undefined | null): string {
    if (!wirNumber) return '—';
    return wirNumber.replace(/WIR-/gi, 'Stage-');
  }

  /**
   * Transform WIR text to Stage text for display purposes only
   */
  getDisplayWirText(text: string | undefined | null): string {
    if (!text) return '—';
    return text.replace(/WIR-/gi, 'Stage-').replace(/WIR /gi, 'Stage ');
  }

  getQualityIssueWir(issue: QualityIssueDetails | null): string {
    if (!issue) return '—';
    
    if (issue.wirNumber) {
      return this.getDisplayWirNumber(issue.wirNumber);
    }

    if (issue.wirId) {
      const matchedCheckpoint = this.wirCheckpoints.find(cp => cp.wirId === issue.wirId);
      if (matchedCheckpoint?.wirNumber) {
        return this.getDisplayWirNumber(matchedCheckpoint.wirNumber);
      }
    }

    return this.getDisplayWirText(issue.wirName);
  }

  getQualityIssueStatusLabel(status?: QualityIssueStatus | string): string {
    const normalized = (status || 'Open') as QualityIssueStatus;
    return this.qualityIssueStatusMeta[normalized]?.label || 'Open';
  }

  getQualityIssueStatusClass(status?: QualityIssueStatus | string): string {
    const normalized = (status || 'Open') as QualityIssueStatus;
    return this.qualityIssueStatusMeta[normalized]?.class || 'status-open';
  }

  getWIRCheckpointStatusLabel(status?: WIRCheckpointStatus | string): string {
    const normalized = (status || WIRCheckpointStatus.Pending).toString();
    const labelMap: Record<string, string> = {
      [WIRCheckpointStatus.Pending]: 'Pending Review',
      [WIRCheckpointStatus.Approved]: 'Approved',
      [WIRCheckpointStatus.Rejected]: 'Rejected',
      [WIRCheckpointStatus.ConditionalApproval]: 'Conditional Approval'
    };
    return labelMap[normalized] || normalized;
  }

  onClose(): void {
    this.close.emit();
  }

  onBackdropClick(): void {
    this.onClose();
  }

  openImageInNewTab(imageUrl: string): void {
    if (!imageUrl) {
      console.error('No image URL provided');
      return;
    }

    // Ensure URL is absolute
    let absoluteUrl = imageUrl;
    
    // If it's a relative URL, make it absolute
    if (imageUrl.startsWith('/')) {
      const baseUrl = this.getApiBaseUrl();
      absoluteUrl = `${baseUrl}${imageUrl}`;
    }
    // For base64/data images, convert to blob URL
    else if (imageUrl.startsWith('data:image/')) {
      // For data URLs, convert to blob URL
      fetch(imageUrl)
        .then(response => response.blob())
        .then(blob => {
          const blobUrl = URL.createObjectURL(blob);
          const newWindow = window.open(blobUrl, '_blank', 'noopener,noreferrer');
          if (!newWindow) {
            console.error('Failed to open image in new tab. Popup may be blocked.');
            // Fallback: try to open data URL directly
            const fallbackWindow = window.open();
            if (fallbackWindow) {
              fallbackWindow.document.write(`
                <!DOCTYPE html>
                <html>
                  <head>
                    <title>Image Viewer</title>
                    <style>
                      body {
                        margin: 0;
                        padding: 20px;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        min-height: 100vh;
                        background: #1a1a1a;
                      }
                      img {
                        max-width: 100%;
                        max-height: 100vh;
                        height: auto;
                        object-fit: contain;
                      }
                    </style>
                  </head>
                  <body>
                    <img src="${imageUrl}" alt="Image" />
                  </body>
                </html>
              `);
              fallbackWindow.document.close();
            }
          }
          // Clean up blob URL after a delay
          setTimeout(() => URL.revokeObjectURL(blobUrl), 100);
        })
        .catch(error => {
          console.error('Error converting data URL to blob:', error);
          // Fallback: try to open data URL directly
          const fallbackWindow = window.open();
          if (fallbackWindow) {
            fallbackWindow.document.write(`
              <!DOCTYPE html>
              <html>
                <head>
                  <title>Image Viewer</title>
                  <style>
                    body {
                      margin: 0;
                      padding: 20px;
                      display: flex;
                      justify-content: center;
                      align-items: center;
                      min-height: 100vh;
                      background: #1a1a1a;
                    }
                    img {
                      max-width: 100%;
                      max-height: 100vh;
                      height: auto;
                      object-fit: contain;
                    }
                  </style>
                </head>
                <body>
                  <img src="${imageUrl}" alt="Image" />
                </body>
              </html>
            `);
            fallbackWindow.document.close();
          }
        });
      return;
    }
    // For external URLs (http/https), open directly
    else if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) {
      absoluteUrl = imageUrl;
    }

    // Open the absolute URL in a new tab
    const newWindow = window.open(absoluteUrl, '_blank', 'noopener,noreferrer');
    if (!newWindow) {
      console.error('Failed to open image in new tab. Popup may be blocked.');
    }
  }

  downloadImage(imageUrl: string): void {
    const link = document.createElement('a');
    link.href = imageUrl;
    link.download = `quality-issue-image-${Date.now()}.jpg`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  onImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.style.display = 'none';
  }

  // Update Status Methods
  toggleUpdateStatus(): void {
    this.showUpdateStatus = !this.showUpdateStatus;
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      const newFiles = Array.from(input.files);
      const validTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
      const invalidFiles = newFiles.filter(file => !validTypes.includes(file.type));
      
      if (invalidFiles.length > 0) {
        this.toastService.showError('Only image files (JPEG, PNG, GIF) are allowed');
        return;
      }
      this.selectedFiles = [...this.selectedFiles, ...newFiles];
    }
  }

  removeFile(index: number): void {
    this.selectedFiles.splice(index, 1);
  }

  requiresResolution(): boolean {
    return this.selectedStatus === 'Resolved' || this.selectedStatus === 'Closed';
  }

  isUpdateValid(): boolean {
    if (!this.selectedStatus) return false;
    if (this.requiresResolution() && !this.resolutionDescription.trim()) return false;
    return true;
  }

  async updateStatus(): Promise<void> {
    if (!this.isUpdateValid() || !this.issue) {
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

      if (this.statusComment.trim()) {
        formData.append('Comment', this.statusComment.trim());
      }

      this.selectedFiles.forEach(file => {
        formData.append('Files', file, file.name);
      });

      const response = await this.http.put<any>(
        `${environment.apiUrl}/qualityissues/${this.issue.issueId}/status`,
        formData
      ).toPromise();

      if (response.isSuccess) {
        this.toastService.showSuccess('Issue status updated successfully');
        this.statusUpdated.emit();
        this.showUpdateStatus = false;
        this.statusComment = '';
        this.selectedFiles = [];
        // Refresh issue data
        if (this.issue) {
          this.issue.status = this.selectedStatus as QualityIssueStatus;
          this.issue.resolutionDescription = this.resolutionDescription;
        }
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
}

