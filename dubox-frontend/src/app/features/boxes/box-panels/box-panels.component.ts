import { Component, Input, Output, EventEmitter, OnInit, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BoxPanel, PanelStatus } from '../../../core/models/box.model';
import { PanelService } from '../../../core/services/panel.service';
import { BarcodeScannerComponent, ScanResult } from '../barcode-scanner/barcode-scanner.component';
import JsBarcode from 'jsbarcode';
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

  // Barcode image cache
  barcodeImageCache: Map<string, string> = new Map();

  // Download all barcodes
  downloadingAll = false;

  PanelStatus = PanelStatus;

  constructor(private panelService: PanelService) {}

  ngOnInit(): void {
    // Generate barcode images for all panels
    this.generateBarcodeImages();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Regenerate barcode images when panels change
    if (changes['panels'] && this.panels && this.panels.length > 0) {
      this.generateBarcodeImages();
    }
  }

  ngOnDestroy(): void {
    // Clean up barcode image cache
    this.barcodeImageCache.clear();
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

  copyBarcode(barcode: string): void {
    navigator.clipboard.writeText(barcode).then(() => {
      this.lastScanResult = `✅ Barcode copied: ${barcode}`;
      setTimeout(() => {
        this.lastScanResult = '';
      }, 3000);
    }).catch(err => {
      console.error('Failed to copy barcode:', err);
      this.error = 'Failed to copy barcode to clipboard';
    });
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
   * Generate barcode images for all panels
   */
  generateBarcodeImages(): void {
    this.panels.forEach(panel => {
      if (panel.barcode && !this.barcodeImageCache.has(panel.barcode)) {
        this.generateBarcodeImage(panel.barcode);
      }
    });
  }

  /**
   * Generate a barcode image from barcode text
   */
  generateBarcodeImage(barcodeText: string): string | null {
    if (!barcodeText) {
      return null;
    }

    // Check cache first
    if (this.barcodeImageCache.has(barcodeText)) {
      return this.barcodeImageCache.get(barcodeText) || null;
    }

    try {
      // Create a canvas element
      const canvas = document.createElement('canvas');
      
      // Generate barcode using Code128 format
      JsBarcode(canvas, barcodeText, {
        format: 'CODE128',
        width: 2,
        height: 60,
        displayValue: true,
        fontSize: 14,
        margin: 10,
        background: '#ffffff',
        lineColor: '#000000'
      });

      // Convert canvas to data URL
      const dataUrl = canvas.toDataURL('image/png');
      
      // Cache the result
      this.barcodeImageCache.set(barcodeText, dataUrl);
      
      return dataUrl;
    } catch (error) {
      console.error('Error generating barcode:', error);
      return null;
    }
  }

  /**
   * Get barcode image URL for a panel
   */
  getBarcodeImage(panel: BoxPanel): string | null {
    if (!panel.barcode) {
      return null;
    }
    return this.generateBarcodeImage(panel.barcode);
  }

  /**
   * Download barcode image
   */
  downloadBarcode(panel: BoxPanel): void {
    if (!panel.barcode) {
      return;
    }

    const barcodeImage = this.getBarcodeImage(panel);
    if (!barcodeImage) {
      this.error = 'Failed to generate barcode image';
      setTimeout(() => {
        this.error = '';
      }, 3000);
      return;
    }

    try {
      // Create a temporary anchor element
      const link = document.createElement('a');
      link.href = barcodeImage;
      link.download = `barcode-${panel.panelName}-${panel.barcode}.png`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    } catch (error) {
      console.error('Error downloading barcode:', error);
      this.error = 'Failed to download barcode image';
      setTimeout(() => {
        this.error = '';
      }, 3000);
    }
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
   * Download all barcode images in a single PDF file (one barcode per page)
   */
  async downloadAllBarcodes(): Promise<void> {
    // Filter panels that have barcodes
    const panelsWithBarcodes = this.panels.filter(panel => panel.barcode);
    
    if (panelsWithBarcodes.length === 0) {
      this.error = 'No panels with barcodes found to download';
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

      // Add each barcode as a new page in the PDF
      for (const panel of panelsWithBarcodes) {
        try {
          const barcodeImage = this.getBarcodeImage(panel);
          if (barcodeImage) {
            // Add new page for each barcode (except the first one)
            if (!isFirstPage) {
              pdf.addPage();
            }
            isFirstPage = false;

            // Add barcode to the current page
            await this.addBarcodeToPDF(pdf, panel, barcodeImage);
            successCount++;
          } else {
            failCount++;
            console.warn(`Failed to generate barcode for panel: ${panel.panelName}`);
          }
        } catch (err) {
          failCount++;
          console.error(`Error processing barcode for panel ${panel.panelName}:`, err);
        }
      }

      if (successCount === 0) {
        this.error = 'Failed to generate any barcode images';
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
      link.download = `all-barcodes-box-${this.boxId}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      
      // Clean up the object URL
      URL.revokeObjectURL(link.href);

      // Show success message
      this.lastScanResult = `✅ Successfully downloaded ${successCount} barcode${successCount !== 1 ? 's' : ''} in PDF${failCount > 0 ? ` (${failCount} failed)` : ''}`;
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
   * Add a barcode to the current PDF page
   */
  private async addBarcodeToPDF(pdf: jsPDF, panel: BoxPanel, barcodeImageDataUrl: string): Promise<void> {
    return new Promise((resolve, reject) => {
      // Convert data URL to image
      const img = new Image();
      
      img.onload = () => {
        try {
          // Calculate dimensions to fit barcode nicely on the page
          const pageWidth = 210; // A4 width in mm
          const pageHeight = 297; // A4 height in mm
          const margin = 20; // margin in mm
          const textSpace = 30; // space reserved for text above barcode
          
          // Calculate available space (accounting for text space)
          const availableWidth = pageWidth - (margin * 2);
          const availableHeight = pageHeight - (margin * 2) - textSpace;
          
          // Calculate scaling to fit the barcode
          const imgWidth = img.width;
          const imgHeight = img.height;
          const imgAspectRatio = imgWidth / imgHeight;
          
          let finalWidth = availableWidth;
          let finalHeight = availableWidth / imgAspectRatio;
          
          // If height exceeds available space, scale down
          if (finalHeight > availableHeight) {
            finalHeight = availableHeight;
            finalWidth = availableHeight * imgAspectRatio;
          }
          
          // Center the barcode horizontally and position it below the text
          const x = (pageWidth - finalWidth) / 2;
          const barcodeY = margin + textSpace + (availableHeight - finalHeight) / 2;
          
          // Add panel information text above the barcode
          pdf.setFontSize(14);
          pdf.setFont('helvetica', 'bold');
          pdf.text(panel.panelName || 'Panel', pageWidth / 2, margin + 10, { align: 'center' });
          
          if (panel.barcode) {
            pdf.setFontSize(10);
            pdf.setFont('helvetica', 'normal');
            pdf.text(`Barcode: ${panel.barcode}`, pageWidth / 2, margin + 18, { align: 'center' });
          }
          
          // Add the barcode image
          pdf.addImage(barcodeImageDataUrl, 'PNG', x, barcodeY, finalWidth, finalHeight);
          
          resolve();
        } catch (error) {
          console.error('Error adding barcode to PDF:', error);
          reject(error);
        }
      };
      
      img.onerror = () => {
        console.error('Error loading barcode image');
        reject(new Error('Failed to load barcode image'));
      };
      
      img.src = barcodeImageDataUrl;
    });
  }

  /**
   * Check if there are any panels with barcodes to download
   */
  hasBarcodesToDownload(): boolean {
    return this.panels.some(panel => panel.barcode);
  }
}

