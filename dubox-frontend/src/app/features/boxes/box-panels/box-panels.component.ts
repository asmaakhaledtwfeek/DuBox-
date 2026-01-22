import { Component, Input, Output, EventEmitter, OnInit, OnDestroy, OnChanges, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BoxPanel, PanelStatus } from '../../../core/models/box.model';
import { PanelService } from '../../../core/services/panel.service';
import { BarcodeScannerComponent, ScanResult } from '../barcode-scanner/barcode-scanner.component';
import QRCode from 'qrcode';
import { jsPDF } from 'jspdf';

@Component({
  selector: 'app-box-panels',
  standalone: true,
  imports: [CommonModule, FormsModule, BarcodeScannerComponent],
  templateUrl: './box-panels.component.html',
  styleUrl: './box-panels.component.scss'
})
export class BoxPanelsComponent implements OnInit, OnDestroy, OnChanges {
  @Input() boxId!: string;
  @Input() projectId!: string;
  @Input() panels: BoxPanel[] = [];
  @Output() refreshRequested = new EventEmitter<void>();

  loading = false;
  error = '';
  
  selectedPanel: BoxPanel | null = null;
  showApprovalModal = false;
  approvalType: 'first' | 'second' = 'first';
  approvalAction: 'approve' | 'reject' = 'approve';
  approvalNotes = '';
  submitting = false;

  // Barcode Scanner
  showBarcodeScanner = false;
  scanType: 'SiteArrival' | 'Installation' = 'SiteArrival';
  lastScanResult: string = '';

  // QR code image cache
  qrCodeImageCache: Map<string, string> = new Map();

  // Download all QR codes
  downloadingAll = false;

  PanelStatus = PanelStatus;

  constructor(
    private panelService: PanelService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    // Generate QR code images for all panels
    this.generateQRCodeImages().then(() => {
      this.cdr.detectChanges();
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Regenerate QR code images when panels change
    if (changes['panels'] && this.panels && this.panels.length > 0) {
      this.generateQRCodeImages().then(() => {
        this.cdr.detectChanges();
      });
    }
  }

  ngOnDestroy(): void {
    // Clean up QR code image cache
    this.qrCodeImageCache.clear();
  }

  loadPanels(): void {
    // Emit event to parent to reload box details
    this.refreshRequested.emit();
  }

  getPanelStatusClass(status: PanelStatus): string {
    switch (status) {
      case PanelStatus.NotStarted:
        return 'status-not-started';
      case PanelStatus.FirstApprovalApproved:
        return 'status-first-approval-approved';
      case PanelStatus.SecondApprovalApproved:
        return 'status-second-approval-approved';
      case PanelStatus.SecondApprovalRejected:
        return 'status-second-approval-rejected';
      default:
        return 'status-default';
    }
  }

  openApprovalModal(panel: BoxPanel, type: 'first' | 'second', action: 'approve' | 'reject'): void {
    this.selectedPanel = panel;
    this.approvalType = type;
    this.approvalAction = action;
    this.approvalNotes = '';
    this.showApprovalModal = true;
  }

  closeApprovalModal(): void {
    this.showApprovalModal = false;
    this.selectedPanel = null;
    this.approvalNotes = '';
    this.submitting = false;
  }

  submitApproval(): void {
    if (!this.selectedPanel) return;

    this.submitting = true;
    this.error = '';

    const command = {
      boxPanelId: this.selectedPanel.boxPanelId,
      isApproved: this.approvalAction === 'approve',
      approvalStatus: this.approvalAction === 'approve' ? 'Approved' : 'Rejected',
      notes: this.approvalNotes
    };

    const approvalObservable = this.approvalType === 'first'
      ? this.panelService.approvePanelFirstApproval(command)
      : this.panelService.approvePanelSecondApproval(command);

    approvalObservable.subscribe({
      next: () => {
        this.submitting = false;
        this.closeApprovalModal();
        this.loadPanels(); // Reload to get updated status
      },
      error: (err) => {
        this.error = err?.error?.message || 'Failed to submit approval';
        this.submitting = false;
      }
    });
  }

  canApproveFirst(panel: BoxPanel): boolean {
    const status = this.normalizeStatus(panel.panelStatus);
    console.log(`🔍 canApproveFirst for ${panel.panelName}:`, { 
      rawStatus: panel.panelStatus, 
      normalizedStatus: status,
      canApprove: status === PanelStatus.NotStarted
    });
    // Can approve first if status is NotStarted
    return status === PanelStatus.NotStarted;
  }

  canApproveSecond(panel: BoxPanel): boolean {
    const status = this.normalizeStatus(panel.panelStatus);
    // Can approve second if:
    // 1. Panel status is FirstApprovalApproved AND
    // 2. Second approval status is Pending (either not set, or explicitly "Pending")
    const isFirstApprovalApproved = status === PanelStatus.FirstApprovalApproved;
    const isSecondApprovalPending = !panel.secondApprovalStatus || panel.secondApprovalStatus === "Pending";
    
    console.log(`🔍 canApproveSecond for ${panel.panelName}:`, { 
      rawStatus: panel.panelStatus, 
      normalizedStatus: status,
      secondApprovalStatus: panel.secondApprovalStatus,
      isFirstApprovalApproved,
      isSecondApprovalPending,
      canApprove: isFirstApprovalApproved && isSecondApprovalPending
    });
    
    return isFirstApprovalApproved && isSecondApprovalPending;
  }

  // Normalize status to handle both string and numeric enum values
  private normalizeStatus(status: PanelStatus | number | string): PanelStatus {
    if (typeof status === 'number') {
      return status as PanelStatus;
    }
    if (typeof status === 'string') {
      // Convert string to enum number
      const enumValue = PanelStatus[status as keyof typeof PanelStatus];
      return typeof enumValue === 'number' ? enumValue : status as unknown as PanelStatus;
    }
    return status;
  }

  copyQRCode(qrCode: string): void {
    navigator.clipboard.writeText(qrCode).then(() => {
      this.lastScanResult = `✅ QR Code copied: ${qrCode}`;
      setTimeout(() => {
        this.lastScanResult = '';
      }, 3000);
    }).catch(err => {
      console.error('Failed to copy QR code:', err);
      this.error = 'Failed to copy QR code to clipboard';
    });
  }

  /**
   * Copy barcode (backward compatibility)
   * @deprecated Use copyQRCode instead
   */
  copyBarcode(qrCode: string): void {
    this.copyQRCode(qrCode);
  }

  getStatusDisplayName(status: PanelStatus): string {
    // Ensure we have a string representation
    let statusName: string;
    
    if (typeof status === 'number') {
      // It's the numeric enum value, convert to string name
      statusName = PanelStatus[status];
    } else {
      // It's already a string or other type
      statusName = String(status);
    }
    
    // Fallback if still not a valid string
    if (!statusName || typeof statusName !== 'string') {
      return 'Unknown Status';
    }
    
    // Add spaces before capital letters and trim
    return statusName.replace(/([A-Z])/g, ' $1').trim();
  }

  // Barcode Scanner Methods
  openBarcodeScanner(type: 'SiteArrival' | 'Installation' = 'SiteArrival'): void {
    this.scanType = type;
    this.showBarcodeScanner = true;
    this.lastScanResult = '';
  }

  closeBarcodeScanner(): void {
    this.showBarcodeScanner = false;
  }

  handleScanComplete(result: ScanResult): void {
    if (result.success) {
      this.lastScanResult = `✅ ${result.panelName || 'Panel'} scanned successfully!`;
      // Refresh panels to show updated status
      this.loadPanels();
      setTimeout(() => {
        this.lastScanResult = '';
      }, 5000);
    } else {
      this.lastScanResult = `❌ ${result.message}`;
      setTimeout(() => {
        this.lastScanResult = '';
      }, 5000);
    }
  }

  /**
   * Generate QR code images for all panels
   */
  async generateQRCodeImages(): Promise<void> {
    const promises = this.panels
      .filter(panel => panel.qrCode && !this.qrCodeImageCache.has(panel.qrCode))
      .map(panel => this.generateQRCodeImage(panel.qrCode!));
    
    await Promise.all(promises);
  }

  /**
   * Generate a QR code image from QR code text (for display)
   */
  async generateQRCodeImage(qrCodeText: string): Promise<string | null> {
    if (!qrCodeText) {
      return null;
    }

    // Check cache first
    if (this.qrCodeImageCache.has(qrCodeText)) {
      return this.qrCodeImageCache.get(qrCodeText) || null;
    }

    try {
      // Generate QR code as data URL
      const dataUrl = await QRCode.toDataURL(qrCodeText, {
        errorCorrectionLevel: 'M',
        type: 'image/png',
        width: 200,
        margin: 2,
        color: {
          dark: '#000000',
          light: '#FFFFFF'
        }
      });
      
      // Cache the result
      this.qrCodeImageCache.set(qrCodeText, dataUrl);
      
      return dataUrl;
    } catch (error) {
      console.error('Error generating QR code:', error);
      return null;
    }
  }

  /**
   * Generate a high-resolution QR code image suitable for scanning from mobile devices
   * This creates a large, sharp QR code that occupies most of the image area
   */
  async generateHighResolutionQRCode(qrCodeText: string): Promise<string | null> {
    if (!qrCodeText) {
      return null;
    }

    try {
      // Generate high-resolution QR code for printing/scanning
      // Use larger size (1200px) for high-quality output
      const dataUrl = await QRCode.toDataURL(qrCodeText, {
        errorCorrectionLevel: 'H', // High error correction for better scanning
        type: 'image/png',
        width: 1200,
        margin: 4,
        color: {
          dark: '#000000',
          light: '#FFFFFF'
        }
      });
      
      return dataUrl;
    } catch (error) {
      console.error('Error generating high-resolution QR code:', error);
      return null;
    }
  }

  /**
   * Get QR code image URL for a panel
   */
  getQRCodeImage(panel: BoxPanel): string | null {
    if (!panel.qrCode) {
      return null;
    }
    // Return from cache if available, otherwise trigger async generation
    if (this.qrCodeImageCache.has(panel.qrCode)) {
      return this.qrCodeImageCache.get(panel.qrCode) || null;
    }
    // Trigger async generation (will update cache when complete)
    this.generateQRCodeImage(panel.qrCode).then(() => {
      this.cdr.detectChanges();
    }).catch(err => {
      console.error('Error generating QR code:', err);
    });
    return null;
  }

  /**
   * Get barcode image URL for a panel (backward compatibility)
   * @deprecated Use getQRCodeImage instead
   */
  getBarcodeImage(panel: BoxPanel): string | null {
    return this.getQRCodeImage(panel);
  }

  /**
   * Download QR code image (high-resolution version for scanning)
   */
  async downloadQRCode(panel: BoxPanel): Promise<void> {
    if (!panel.qrCode) {
      return;
    }

    try {
      // Generate high-resolution QR code for download
      const qrCodeImage = await this.generateHighResolutionQRCode(panel.qrCode);
      if (!qrCodeImage) {
        this.error = 'Failed to generate QR code image';
        setTimeout(() => {
          this.error = '';
        }, 3000);
        return;
      }

      // Create a temporary anchor element
      const link = document.createElement('a');
      link.href = qrCodeImage;
      link.download = `qrcode-${panel.panelName}-${panel.qrCode}.png`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    } catch (error) {
      console.error('Error downloading QR code:', error);
      this.error = 'Failed to download QR code image';
      setTimeout(() => {
        this.error = '';
      }, 3000);
    }
  }

  /**
   * Download barcode image (backward compatibility)
   * @deprecated Use downloadQRCode instead
   */
  downloadBarcode(panel: BoxPanel): void {
    this.downloadQRCode(panel).catch(err => {
      console.error('Error downloading QR code:', err);
    });
  }

  /**
   * Handle image error event
   */
  handleImageError(event: Event): void {
    const target = event.target as HTMLImageElement;
    if (target) {
      target.style.display = 'none';
    }
  }

  /**
   * Download all QR code images in a single PDF file (one QR code per page)
   */
  async downloadAllQRCodes(): Promise<void> {
    // Filter panels that have QR codes
    const panelsWithQRCodes = this.panels.filter(panel => panel.qrCode);
    
    if (panelsWithQRCodes.length === 0) {
      this.error = 'No panels with QR codes found to download';
      setTimeout(() => {
        this.error = '';
      }, 3000);
      return;
    }

    this.downloadingAll = true;
    this.error = '';

    try {
      // Create a single PDF document
      const pdf = new jsPDF({
        orientation: 'portrait',
        unit: 'mm',
        format: 'a4'
      });

      let successCount = 0;
      let failCount = 0;
      let isFirstPage = true;

      // Add each QR code as a new page in the PDF
      for (const panel of panelsWithQRCodes) {
        try {
          // Use high-resolution QR code for PDF generation
          const qrCodeImage = await this.generateHighResolutionQRCode(panel.qrCode!);
          if (qrCodeImage) {
            // Add new page for each QR code (except the first one)
            if (!isFirstPage) {
              pdf.addPage();
            }
            isFirstPage = false;

            // Add QR code to the current page
            await this.addQRCodeToPDF(pdf, panel, qrCodeImage);
            successCount++;
          } else {
            failCount++;
            console.warn(`Failed to generate QR code for panel: ${panel.panelName}`);
          }
        } catch (err) {
          failCount++;
          console.error(`Error processing QR code for panel ${panel.panelName}:`, err);
        }
      }

      if (successCount === 0) {
        this.error = 'Failed to generate any QR code images';
        this.downloadingAll = false;
        setTimeout(() => {
          this.error = '';
        }, 3000);
        return;
      }

      // Generate PDF blob and download
      const pdfBlob = pdf.output('blob');
      
      // Create download link
      const link = document.createElement('a');
      link.href = URL.createObjectURL(pdfBlob);
      link.download = `all-qrcodes-box-${this.boxId}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      
      // Clean up the object URL
      URL.revokeObjectURL(link.href);

      // Show success message
      this.lastScanResult = `✅ Successfully downloaded ${successCount} QR code${successCount !== 1 ? 's' : ''} in PDF${failCount > 0 ? ` (${failCount} failed)` : ''}`;
      setTimeout(() => {
        this.lastScanResult = '';
      }, 5000);

    } catch (error) {
      console.error('Error creating PDF file:', error);
      this.error = 'Failed to create PDF file. Please try again.';
      setTimeout(() => {
        this.error = '';
      }, 5000);
    } finally {
      this.downloadingAll = false;
    }
  }

  /**
   * Download all barcodes (backward compatibility)
   * @deprecated Use downloadAllQRCodes instead
   */
  downloadAllBarcodes(): void {
    this.downloadAllQRCodes().catch(err => {
      console.error('Error downloading QR codes:', err);
    });
  }

  /**
   * Add a QR code to the current PDF page (high-resolution, large size for scanning)
   */
  private async addQRCodeToPDF(pdf: jsPDF, panel: BoxPanel, qrCodeImageDataUrl: string): Promise<void> {
    return new Promise((resolve, reject) => {
      // Convert data URL to image
      const img = new Image();
      
      img.onload = () => {
        try {
          // Calculate dimensions to fit QR code nicely on the page
          const pageWidth = 210; // A4 width in mm
          const pageHeight = 297; // A4 height in mm
          const margin = 15; // Reduced margin to maximize QR code size
          const textSpace = 25; // space reserved for text above QR code
          
          // Calculate available space (accounting for text space)
          const availableWidth = pageWidth - (margin * 2);
          const availableHeight = pageHeight - (margin * 2) - textSpace;
          
          // Calculate scaling to fit the QR code - prioritize large size for scanning
          const imgWidth = img.width;
          const imgHeight = img.height;
          const imgAspectRatio = imgWidth / imgHeight;
          
          // QR codes are square, so use the smaller dimension
          const maxSize = Math.min(availableWidth, availableHeight) * 0.95;
          let finalWidth = maxSize;
          let finalHeight = maxSize;
          
          // If aspect ratio is not 1:1, adjust accordingly
          if (imgAspectRatio > 1) {
            finalHeight = finalWidth / imgAspectRatio;
          } else if (imgAspectRatio < 1) {
            finalWidth = finalHeight * imgAspectRatio;
          }
          
          // Ensure minimum size for scannability (at least 100mm)
          const minSize = 100;
          if (finalWidth < minSize && availableWidth >= minSize) {
            finalWidth = Math.min(minSize, availableWidth * 0.95);
            finalHeight = finalWidth;
          }
          
          // Center the QR code horizontally and position it below the text
          const x = (pageWidth - finalWidth) / 2;
          const qrCodeY = margin + textSpace + (availableHeight - finalHeight) / 2;
          
          // Add panel information text above the QR code
          pdf.setFontSize(16);
          pdf.setFont('helvetica', 'bold');
          pdf.text(panel.panelName || 'Panel', pageWidth / 2, margin + 10, { align: 'center' });
          
          if (panel.qrCode) {
            pdf.setFontSize(12);
            pdf.setFont('helvetica', 'normal');
            pdf.text(`QR Code: ${panel.qrCode}`, pageWidth / 2, margin + 20, { align: 'center' });
          }
          
          // Add the QR code image with high quality
          // Use 'FAST' compression mode for better quality (though PNG is lossless)
          pdf.addImage(qrCodeImageDataUrl, 'PNG', x, qrCodeY, finalWidth, finalHeight, undefined, 'FAST');
          
          resolve();
        } catch (error) {
          console.error('Error adding QR code to PDF:', error);
          reject(error);
        }
      };
      
      img.onerror = () => {
        console.error('Error loading QR code image');
        reject(new Error('Failed to load QR code image'));
      };
      
      img.src = qrCodeImageDataUrl;
    });
  }

  /**
   * Check if there are any panels with QR codes to download
   */
  hasQRCodesToDownload(): boolean {
    return this.panels.some(panel => panel.qrCode);
  }

  /**
   * Check if there are any panels with barcodes to download (backward compatibility)
   * @deprecated Use hasQRCodesToDownload instead
   */
  hasBarcodesToDownload(): boolean {
    return this.hasQRCodesToDownload();
  }
}

