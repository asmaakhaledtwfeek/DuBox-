import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QualityIssueDetails, QualityIssueStatus, WIRCheckpointStatus } from '../../../core/models/wir.model';
import { WIRCheckpoint } from '../../../core/models/wir.model';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-quality-issue-details-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './quality-issue-details-modal.component.html',
  styleUrls: ['./quality-issue-details-modal.component.scss']
})
export class QualityIssueDetailsModalComponent implements OnInit, OnChanges {
  @Input() issue: QualityIssueDetails | null = null;
  @Input() wirCheckpoints: WIRCheckpoint[] = []; // For WIR lookup
  @Input() isOpen = false;
  @Output() close = new EventEmitter<void>();

  qualityIssueStatusMeta: Record<QualityIssueStatus, { label: string; class: string }> = {
    Open: { label: 'Open', class: 'status-open' },
    InProgress: { label: 'In Progress', class: 'status-inprogress' },
    Resolved: { label: 'Resolved', class: 'status-resolved' },
    Closed: { label: 'Closed', class: 'status-closed' }
  };

  imageUrls: string[] = [];

  ngOnInit(): void {
    this.updateImageUrls();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['issue'] && this.issue) {
      this.updateImageUrls();
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
    return environment.apiUrl || window.location.origin;
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

  getQualityIssueWir(issue: QualityIssueDetails | null): string {
    if (!issue) return '—';
    
    if (issue.wirNumber) {
      return issue.wirNumber;
    }

    if (issue.wirId) {
      const matchedCheckpoint = this.wirCheckpoints.find(cp => cp.wirId === issue.wirId);
      if (matchedCheckpoint?.wirNumber) {
        return matchedCheckpoint.wirNumber;
      }
    }

    return issue.wirName || '—';
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
    window.open(imageUrl, '_blank');
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
}

