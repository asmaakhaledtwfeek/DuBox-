import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { PublicBoxService, PublicBox } from '../../../core/services/public-box.service';
import { BoxSummary } from '../../../core/models/box.model';

@Component({
  selector: 'app-public-box-view',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './public-box-view.component.html',
  styleUrls: ['./public-box-view.component.scss']
})
export class PublicBoxViewComponent implements OnInit, OnDestroy {
  box: PublicBox | null = null;
  loading = true;
  error = '';
  boxId = '';
  currentYear = new Date().getFullYear();
  boxSummary: BoxSummary | null = null;
  loadingBoxSummary = false;
  boxSummaryError = '';
  
  boxAttachments: any = null;
  loadingAttachments = false;
  attachmentsError = '';
  
  boxDrawings: any[] = [];
  loadingDrawings = false;
  drawingsError = '';
  
  showAttachments = false;
  showDrawings = false;

  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private publicBoxService: PublicBoxService
  ) {}

  ngOnInit(): void {
    this.boxId = this.route.snapshot.paramMap.get('boxId') || '';
    if (this.boxId) {
      this.loadBox();
    } else {
      this.error = 'Invalid box ID';
      this.loading = false;
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadBox(): void {
    this.loading = true;
    this.error = '';

    this.publicBoxService.getPublicBox(this.boxId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (box) => {
          this.box = box;
          this.loading = false;
          this.loadBoxSummary();
          this.loadBoxAttachments();
          this.loadBoxDrawings();
        },
        error: (err) => {
          console.error('Error loading box:', err);
          if (err.status === 404) {
            this.error = 'Box not found. The QR code may be invalid or the box has been removed.';
          } else {
            this.error = 'Unable to load box details. Please try again later.';
          }
          this.loading = false;
        }
      });
  }

  loadBoxSummary(): void {
    if (!this.boxId) return;
    this.loadingBoxSummary = true;
    this.boxSummaryError = '';
    
    this.publicBoxService.getPublicBoxSummary(this.boxId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (summary) => {
          this.boxSummary = summary;
          this.loadingBoxSummary = false;
          console.log('✅ Box Summary loaded:', summary);
        },
        error: (err) => {
          console.error('❌ Error loading box summary:', err);
          this.boxSummaryError = 'Failed to load box summary.';
          this.loadingBoxSummary = false;
        }
      });
  }

  getStatusInfo(status: string): { label: string; class: string; color: string } {
    return this.publicBoxService.getStatusInfo(status);
  }

  getProgressColor(progress: number): string {
    if (progress >= 100) return '#10b981';
    if (progress >= 75) return '#3b82f6';
    if (progress >= 50) return '#f59e0b';
    if (progress >= 25) return '#f97316';
    return '#ef4444';
  }

  formatDate(date: Date | undefined): string {
    if (!date) return '—';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  getDimensions(): string {
    if (!this.box) return '—';
    const parts: string[] = [];
    if (this.box.length) parts.push(`L: ${this.box.length}`);
    if (this.box.width) parts.push(`W: ${this.box.width}`);
    if (this.box.height) parts.push(`H: ${this.box.height}`);
    if (parts.length === 0) return '—';
    return parts.join(' × ') + (this.box.unitOfMeasure ? ` ${this.box.unitOfMeasure}` : '');
  }

  getLocationDisplay(): string {
    if (!this.box) return '—';
    const parts: string[] = [];
    if (this.box.factoryName) parts.push(this.box.factoryName);
    if (this.box.currentLocationName) parts.push(this.box.currentLocationName);
    if (this.box.bay) parts.push(`Bay ${this.box.bay}`);
    if (this.box.row) parts.push(`Row ${this.box.row}`);
    if (this.box.position) parts.push(`Pos ${this.box.position}`);
    return parts.length > 0 ? parts.join(' • ') : '—';
  }

  loadBoxAttachments(): void {
    if (!this.boxId) return;
    this.loadingAttachments = true;
    this.attachmentsError = '';
    
    this.publicBoxService.getPublicBoxAttachments(this.boxId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (attachments) => {
          this.boxAttachments = attachments;
          this.loadingAttachments = false;
          console.log('✅ Box Attachments loaded:', attachments);
        },
        error: (err) => {
          console.error('❌ Error loading box attachments:', err);
          this.attachmentsError = 'Failed to load attachments.';
          this.loadingAttachments = false;
        }
      });
  }

  loadBoxDrawings(): void {
    if (!this.boxId) return;
    this.loadingDrawings = true;
    this.drawingsError = '';
    
    this.publicBoxService.getPublicBoxDrawings(this.boxId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (drawings) => {
          this.boxDrawings = drawings;
          this.loadingDrawings = false;
          console.log('✅ Box Drawings loaded:', drawings);
        },
        error: (err) => {
          console.error('❌ Error loading box drawings:', err);
          this.drawingsError = 'Failed to load drawings.';
          this.loadingDrawings = false;
        }
      });
  }

  getFileIcon(fileName: string): string {
    const ext = fileName?.split('.').pop()?.toLowerCase();
    switch (ext) {
      case 'pdf': return '📄';
      case 'dwg': return '📐';
      case 'jpg':
      case 'jpeg':
      case 'png':
      case 'gif': return '🖼️';
      default: return '📎';
    }
  }

  formatFileSize(bytes: number): string {
    if (!bytes) return '—';
    if (bytes < 1024) return bytes + ' B';
    if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB';
    return (bytes / (1024 * 1024)).toFixed(1) + ' MB';
  }

  getAllAttachments(): any[] {
    if (!this.boxAttachments) return [];
    const all: any[] = [];
    
    // Add source type to each attachment for better identification
    if (this.boxAttachments.wirCheckpointImages) {
      all.push(...this.boxAttachments.wirCheckpointImages.map((img: any) => ({
        ...img,
        sourceType: 'WIR Checkpoint',
        sourceIcon: 'checkpoint'
      })));
    }
    if (this.boxAttachments.progressUpdateImages) {
      all.push(...this.boxAttachments.progressUpdateImages.map((img: any) => ({
        ...img,
        sourceType: 'Progress Update',
        sourceIcon: 'progress'
      })));
    }
    if (this.boxAttachments.qualityIssueImages) {
      all.push(...this.boxAttachments.qualityIssueImages.map((img: any) => ({
        ...img,
        sourceType: 'Quality Issue',
        sourceIcon: 'quality'
      })));
    }
    
    return all;
  }

  getGroupedAttachments(): any[] {
    const attachments = this.getAllAttachments();
    if (!attachments || attachments.length === 0) return [];
    
    // Group attachments by original file name
    const grouped = new Map<string, any[]>();
    
    attachments.forEach(attachment => {
      const fileName = attachment.originalName || attachment.fileName || 'Unnamed';
      if (!grouped.has(fileName)) {
        grouped.set(fileName, []);
      }
      grouped.get(fileName)!.push(attachment);
    });
    
    // Convert to array and determine latest version
    const groupedArray = Array.from(grouped.entries()).map(([fileName, versions]) => {
      // Sort by creation date (newest first)
      const sortedVersions = versions.sort((a, b) => {
        const dateA = a.createdDate ? new Date(a.createdDate).getTime() : 0;
        const dateB = b.createdDate ? new Date(b.createdDate).getTime() : 0;
        return dateB - dateA;
      });
      
      const latestVersion = sortedVersions[0];
      
      // Debug: Log the latest version imageUrl
      console.log('📸 Latest version for', fileName, ':', {
        imageUrl: latestVersion.imageUrl,
        originalName: latestVersion.originalName,
        createdDate: latestVersion.createdDate
      });
      
      return {
        fileName,
        versions: sortedVersions,
        latestVersion: latestVersion,
        versionCount: sortedVersions.length,
        expanded: false
      };
    });
    
    console.log('📦 Grouped attachments:', groupedArray);
    return groupedArray;
  }

  /**
   * Check if attachment is unavailable and return reason
   * Unavailable means: no imageUrl/imageData available for viewing
   * This could be due to:
   * - File data was not loaded from backend
   * - File was deleted or corrupted
   * - Data transmission error
   */
  getUnavailableReason(attachment: any): string | null {
    const imageUrl = this.getImageUrl(attachment);
    if (!imageUrl) {
      return 'This file is currently unavailable. The image data may not have been loaded properly or the file may have been removed.';
    }
    return null;
  }

  isAttachmentAvailable(attachment: any): boolean {
    return !!this.getImageUrl(attachment);
  }

  getGroupedDrawings(): any[] {
    if (!this.boxDrawings || this.boxDrawings.length === 0) return [];
    
    // Group drawings by original file name
    const grouped = new Map<string, any[]>();
    
    this.boxDrawings.forEach(drawing => {
      const fileName = drawing.originalFileName || drawing.drawingUrl || 'Unknown';
      if (!grouped.has(fileName)) {
        grouped.set(fileName, []);
      }
      grouped.get(fileName)!.push(drawing);
    });
    
    // Convert to array and sort versions
    return Array.from(grouped.entries()).map(([fileName, versions]) => ({
      fileName,
      versions: versions.sort((a, b) => (b.version || 0) - (a.version || 0)),
      latestVersion: versions.reduce((latest, current) => 
        (current.version || 0) > (latest.version || 0) ? current : latest
      , versions[0])
    }));
  }

  getFileExtension(fileName: string): string {
    return fileName?.split('.').pop()?.toUpperCase() || '';
  }

  isImageFile(fileName: string): boolean {
    const ext = fileName?.split('.').pop()?.toLowerCase();
    return ['jpg', 'jpeg', 'png', 'gif', 'webp', 'svg', 'bmp', 'ico'].includes(ext || '');
  }

  onImageError(event: any): void {
    // If image fails to load, hide it and show fallback
    event.target.style.display = 'none';
    console.warn('Failed to load image:', event.target.src);
  }

  /**
   * Safely open image in new tab
   * Prevents navigation if URL is invalid
   */
  viewImageInFullSize(event: Event, imageUrl: string): void {
    event.preventDefault();
    
    if (!imageUrl || imageUrl.trim() === '') {
      console.error('Cannot open image: URL is empty or undefined');
      alert('Image URL is not available. This file may require authentication to view.');
      return;
    }

    console.log('Opening image in new tab:', imageUrl);
    window.open(imageUrl, '_blank', 'noopener,noreferrer');
  }

  /**
   * Format image data to ensure it has proper data URL format
   * (Same logic as box-details page)
   */
  formatImageData(imageData: string, imageType?: string): string {
    if (!imageData) {
      return '';
    }

    // Check if imageData is already a valid data URL or regular URL
    if (imageData.startsWith('data:') || imageData.startsWith('http://') || imageData.startsWith('https://')) {
      return imageData;
    }

    // It's a base64 string without the data URL prefix
    // Determine MIME type from imageType field or default to PNG
    let mimeType = 'image/png'; // Default
    
    if (imageType) {
      const type = imageType.toLowerCase();
      if (type.includes('jpg') || type.includes('jpeg')) {
        mimeType = 'image/jpeg';
      } else if (type.includes('png')) {
        mimeType = 'image/png';
      } else if (type.includes('gif')) {
        mimeType = 'image/gif';
      } else if (type.includes('webp')) {
        mimeType = 'image/webp';
      } else if (type.includes('bmp')) {
        mimeType = 'image/bmp';
      }
    }
    
    return `data:${mimeType};base64,${imageData}`;
  }

  /**
   * Get the image URL for display and linking
   * Returns the imageUrl if available (including data URLs), otherwise returns null
   */
  getImageUrl(attachment: any): string | null {
    if (!attachment) return null;
    
    // Check for imageUrl property (includes data URLs from base64)
    if (attachment.imageUrl && attachment.imageUrl.trim() !== '') {
      return this.formatImageData(attachment.imageData, attachment.imageType);
    }
    
    // Fallback: Convert imageData to data URL if available
    if (attachment.imageData && attachment.imageData.trim() !== '') {
      return this.formatImageData(attachment.imageData, attachment.imageType);
    }
    
    // Fallback to other possible URL fields
    if (attachment.url && attachment.url.trim() !== '') {
      return this.formatImageData(attachment.url, attachment.imageType);
    }
    
    if (attachment.filePath && attachment.filePath.trim() !== '') {
      return this.formatImageData(attachment.filePath, attachment.imageType);
    }
    
    console.warn('⚠️ No valid image URL or data found for attachment:', attachment.originalName);
    return null;
  }
}

