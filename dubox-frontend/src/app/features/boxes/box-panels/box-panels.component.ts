import { Component, Input, Output, EventEmitter, OnInit, OnDestroy, OnChanges, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BoxPanel, PanelStatus } from '../../../core/models/box.model';
import { PANEL_STAGES } from '../../../core/models/panel-stage.model';
import { PanelService } from '../../../core/services/panel.service';
import { BarcodeScannerComponent, ScanResult } from '../barcode-scanner/barcode-scanner.component';
import { PanelWorkflowModalComponent } from '../panel-workflow-modal/panel-workflow-modal.component';
import QRCode from 'qrcode';
import { jsPDF } from 'jspdf';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-box-panels',
  standalone: true,
  imports: [CommonModule, FormsModule, BarcodeScannerComponent, PanelWorkflowModalComponent],
  templateUrl: './box-panels.component.html',
  styleUrl: './box-panels.component.scss'
})
export class BoxPanelsComponent implements OnInit, OnDestroy, OnChanges {
  @Input() boxId!: string;
  @Input() projectId!: string;
  @Input() panels: BoxPanel[] = [];
  @Output() refreshRequested = new EventEmitter<void>();
  @Output() navigateToIssue = new EventEmitter<string>(); // Emit quality issue ID

  loading = false;
  error = '';
  
  selectedPanel: BoxPanel | null = null;
  showApprovalModal = false;
  approvalType: 'first' | 'second' = 'first';
  approvalAction: 'approve' | 'reject' = 'approve';
  approvalNotes = '';
  submitting = false;
  successMessage = '';

  // Workflow modal
  showWorkflowModal = false;

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
      case PanelStatus.InProgress:
        return 'status-in-progress';
      case PanelStatus.Completed:
        return 'status-completed';
      case PanelStatus.OnHold:
        return 'status-on-hold';
      case PanelStatus.Rejected:
        return 'status-rejected';
      case PanelStatus.FirstApprovalPending:
        return 'status-first-approval-pending';
      case PanelStatus.FirstApprovalApproved:
        return 'status-first-approval-approved';
      case PanelStatus.SecondApprovalPending:
        return 'status-second-approval-pending';
      case PanelStatus.SecondApprovalApproved:
        return 'status-second-approval-approved';
      case PanelStatus.SecondApprovalRejected:
        return 'status-second-approval-rejected';
      default:
        return 'status-default';
    }
  }

  openWorkflowModal(panel: BoxPanel): void {
    this.selectedPanel = panel;
    this.showWorkflowModal = true;
  }

  closeWorkflowModal(): void {
    this.showWorkflowModal = false;
    this.selectedPanel = null;
  }

  onWorkflowUpdated(): void {
    this.showWorkflowModal = false;
    this.selectedPanel = null;
    this.loadPanels();
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

    // Validate: Notes are required for rejection
    if (this.approvalAction === 'reject' && (!this.approvalNotes || this.approvalNotes.trim() === '')) {
      this.error = 'Notes are required when rejecting a panel';
      return;
    }

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
      next: (response) => {
        this.submitting = false;
        const dto = response?.data ?? response?.Data ?? response;
        const createdQualityIssue = dto?.qualityIssueId ?? dto?.QualityIssueId;
        if (this.approvalAction === 'reject') {
          this.successMessage = createdQualityIssue
            ? 'Panel rejected. A quality issue has been created automatically.'
            : 'Panel rejected.';
          this.loadPanels();
          setTimeout(() => this.closeApprovalModal(), 2000);
        } else {
          this.closeApprovalModal();
          this.loadPanels();
        }
      },
      error: (err) => {
        this.error = err?.error?.message || 'Failed to submit approval';
        this.submitting = false;
      }
    });
  }

  canApproveFirst(panel: BoxPanel): boolean {
    // First approval only when panel is completed:
    // 1. WorkflowStatus is "Completed" (all 4 stages done) AND panel status is Completed or FirstApprovalPending
    // 2. First approval has not been done yet
    const status = this.normalizeStatus(panel.panelStatus);
    const panelCompleted = panel.workflowStatus === 'Completed' &&
      (status === PanelStatus.Completed || status === PanelStatus.FirstApprovalPending);
    const firstApprovalNotDone = !panel.firstApprovalStatus || panel.firstApprovalStatus === 'Pending';
    return panelCompleted && firstApprovalNotDone;
  }

  canApproveSecond(panel: BoxPanel): boolean {
    // Second approval only when first approval is done:
    // 1. First approval has been approved AND
    // 2. Second approval status is Pending (either not set, or explicitly "Pending")
    const isFirstApprovalApproved = panel.firstApprovalStatus === 'Approved';
    const isSecondApprovalPending = !panel.secondApprovalStatus || panel.secondApprovalStatus === 'Pending';
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

  getCurrentStageLabel(panel: BoxPanel): string | null {
    let stage = panel.currentStage ?? 0;
    if (stage <= 0) {
      // Infer from completion flags when currentStage not set
      const count = this.getCompletedStagesCount(panel);
      if (count > 0 && count < 7) stage = count; // Current stage = last completed when in progress
      else if (count >= 7) stage = 7; // All complete
      else return null;
    }
    const stageInfo = PANEL_STAGES.find(s => s.stage === stage);
    return stageInfo ? stageInfo.label : null;
  }

  getCompletedStagesCount(panel: BoxPanel): number {
    let count = 0;
    if (panel.moldPreparationComplete) count++;
    if (panel.initialComplete) count++;
    if (panel.mepInsertsInstallationComplete) count++;
    if (panel.reinforcementSetupComplete) count++;
    if (panel.concreteCastingComplete) count++;
    if (panel.surfaceFinishingComplete) count++;
    if (panel.curingAndDemoldingComplete) count++;
    return count;
  }

  shouldShowStageInfo(panel: BoxPanel): boolean {
    const completed = this.getCompletedStagesCount(panel);
    const hasStage = (panel.currentStage ?? 0) > 0;
    return hasStage || completed > 0;
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
   * Download all approval QR codes (with public URLs) in a single PDF file
   */
  async downloadAllApprovalQRCodes(): Promise<void> {
    // Filter panels that need approval
    const panelsNeedingApproval = this.panels.filter(panel => 
      this.canApproveFirst(panel) || this.canApproveSecond(panel)
    );
    
    if (panelsNeedingApproval.length === 0) {
      this.error = 'No panels requiring approval found';
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

      // Add approval QR codes for each panel
      for (const panel of panelsNeedingApproval) {
        try {
          // Generate ONE approval QR code (backend auto-determines which approval)
          const qrCodeImage = await this.generateApprovalQRCode(panel);
          if (qrCodeImage) {
            // Add new page for each QR code (except the first one)
            if (!isFirstPage) {
              pdf.addPage();
            }
            isFirstPage = false;

            // Add approval QR code to the current page
            await this.addApprovalQRCodeToPDF(pdf, panel, qrCodeImage);
            successCount++;
          } else {
            failCount++;
            console.warn(`Failed to generate approval QR code for panel: ${panel.panelName}`);
          }
        } catch (err) {
          failCount++;
          console.error(`Error processing approval QR code for panel ${panel.panelName}:`, err);
        }
      }

      if (successCount === 0) {
        this.error = 'Failed to generate any approval QR codes';
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
      link.download = `approval-qrcodes-box-${this.boxId}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      
      // Clean up the object URL
      URL.revokeObjectURL(link.href);

      // Show success message
      this.lastScanResult = `✅ Successfully downloaded ${successCount} approval QR code${successCount !== 1 ? 's' : ''} in PDF${failCount > 0 ? ` (${failCount} failed)` : ''}`;
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
   * Add an approval QR code to the current PDF page
   */
  private async addApprovalQRCodeToPDF(pdf: jsPDF, panel: BoxPanel, qrCodeImageDataUrl: string): Promise<void> {
    return new Promise((resolve, reject) => {
      // Convert data URL to image
      const img = new Image();
      
      img.onload = () => {
        try {
          // Calculate dimensions to fit QR code nicely on the page
          const pageWidth = 210; // A4 width in mm
          const pageHeight = 297; // A4 height in mm
          const margin = 15; // Reduced margin to maximize QR code size
          const textSpace = 40; // space reserved for text above QR code
          
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
          pdf.setFontSize(18);
          pdf.setFont('helvetica', 'bold');
          pdf.text(panel.panelName || 'Panel', pageWidth / 2, margin + 10, { align: 'center' });
          
          pdf.setFontSize(14);
          pdf.setFont('helvetica', 'bold');
          pdf.setTextColor(16, 185, 129); // Green
          pdf.text('Panel Approval - Mobile Scan', pageWidth / 2, margin + 22, { align: 'center' });
          
          pdf.setTextColor(0, 0, 0); // Reset to black
          pdf.setFontSize(11);
          pdf.setFont('helvetica', 'normal');
          pdf.text('Scan with phone camera to approve (1st or 2nd)', pageWidth / 2, margin + 32, { align: 'center' });
          
          // Add the QR code image with high quality
          pdf.addImage(qrCodeImageDataUrl, 'PNG', x, qrCodeY, finalWidth, finalHeight, undefined, 'FAST');
          
          // Add footer text
          pdf.setFontSize(10);
          pdf.setTextColor(100, 100, 100);
          pdf.text('No login required - System auto-detects approval type - Location captured automatically', pageWidth / 2, pageHeight - 10, { align: 'center' });
          
          resolve();
        } catch (error) {
          console.error('Error adding approval QR code to PDF:', error);
          reject(error);
        }
      };
      
      img.onerror = () => {
        console.error('Error loading approval QR code image');
        reject(new Error('Failed to load approval QR code image'));
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

  /**
   * Generate approval token for panel
   * Must match the token generation in backend
   */
  async generateApprovalToken(boxPanelId: string, qrCode: string): Promise<string> {
    const data = `${boxPanelId}|${qrCode}|DUBOX_PANEL_APPROVAL`;
    const encoder = new TextEncoder();
    const dataBuffer = encoder.encode(data);
    const hashBuffer = await crypto.subtle.digest('SHA-256', dataBuffer);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    const hashBase64 = btoa(String.fromCharCode(...hashArray));
    return hashBase64.substring(0, 16); // Take first 16 chars for brevity
  }

  /**
   * Generate ONE public approval URL (auto-detects first or second approval on backend)
   */
  async getApprovalUrl(panel: BoxPanel): Promise<string> {
    if (!panel.qrCode) return '';
    const token = await this.generateApprovalToken(panel.boxPanelId, panel.qrCode);
    const baseUrl = window.location.origin;
    // No "type" parameter - backend auto-determines based on panel status
    return `${baseUrl}/public-approve?panelId=${panel.boxPanelId}&token=${token}`;
  }

  /**
   * Generate ONE QR code with approval URL embedded (works for both first and second approval)
   * The backend automatically determines which approval to perform based on panel status
   */
  async generateApprovalQRCode(panel: BoxPanel): Promise<string | null> {
    try {
      const approvalUrl = await this.getApprovalUrl(panel);
      
      if (!approvalUrl) return null;

      // Generate QR code from approval URL (high resolution for scanning)
      const dataUrl = await QRCode.toDataURL(approvalUrl, {
        errorCorrectionLevel: 'H',
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
      console.error('Error generating approval QR code:', error);
      return null;
    }
  }

  /**
   * Download ONE approval QR code for scanning from mobile (auto-detects first or second approval)
   */
  async downloadApprovalQRCode(panel: BoxPanel): Promise<void> {
    try {
      const qrCodeImage = await this.generateApprovalQRCode(panel);
      if (!qrCodeImage) {
        this.error = 'Failed to generate approval QR code';
        setTimeout(() => {
          this.error = '';
        }, 3000);
        return;
      }

      // Create a temporary anchor element
      const link = document.createElement('a');
      link.href = qrCodeImage;
      link.download = `approval-qr-${panel.panelName}.png`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    } catch (error) {
      console.error('Error downloading approval QR code:', error);
      this.error = 'Failed to download approval QR code';
      setTimeout(() => {
        this.error = '';
      }, 3000);
    }
  }

  /**
   * Check if panel has a quality issue
   */
  hasQualityIssue(panel: BoxPanel): boolean {
    return !!panel.qualityIssueId;
  }

  /**
   * Navigate to quality issues tab and highlight the specific issue
   * This emits an event to the parent component (box-details) to switch tabs
   */
  navigateToQualityIssue(panel: BoxPanel): void {
    if (!panel.qualityIssueId) {
      return;
    }
    
    // Emit event to parent component to switch to quality issues tab and highlight
    this.navigateToIssue.emit(panel.qualityIssueId);
  }
}

