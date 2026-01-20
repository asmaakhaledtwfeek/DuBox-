import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BoxPanel, PanelStatus } from '../../../core/models/box.model';
import { PanelService } from '../../../core/services/panel.service';
import { BarcodeScannerComponent, ScanResult } from '../barcode-scanner/barcode-scanner.component';

@Component({
  selector: 'app-box-panels',
  standalone: true,
  imports: [CommonModule, FormsModule, BarcodeScannerComponent],
  templateUrl: './box-panels.component.html',
  styleUrl: './box-panels.component.scss'
})
export class BoxPanelsComponent implements OnInit {
  @Input() boxId!: string;
  @Input() projectId!: string;
  @Input() panels: BoxPanel[] = [];

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

  PanelStatus = PanelStatus;

  constructor(private panelService: PanelService) {}

  ngOnInit(): void {
    // Panels are passed via Input
  }

  loadPanels(): void {
    // Refresh panels by emitting an event to parent to reload box details
    // For now, panels are passed via Input
  }

  getPanelStatusClass(status: PanelStatus): string {
    switch (status) {
      case PanelStatus.Manufacturing:
        return 'status-manufacturing';
      case PanelStatus.InTransit:
        return 'status-in-transit';
      case PanelStatus.ArrivedFactory:
        return 'status-arrived';
      case PanelStatus.FirstApprovalPending:
      case PanelStatus.SecondApprovalPending:
        return 'status-pending';
      case PanelStatus.FirstApprovalApproved:
      case PanelStatus.SecondApprovalApproved:
        return 'status-approved';
      case PanelStatus.FirstApprovalRejected:
      case PanelStatus.SecondApprovalRejected:
      case PanelStatus.Rejected:
        return 'status-rejected';
      case PanelStatus.Installed:
        return 'status-installed';
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
      canApprove: status === PanelStatus.FirstApprovalPending || status === PanelStatus.ArrivedFactory
    });
    return status === PanelStatus.FirstApprovalPending || 
           status === PanelStatus.ArrivedFactory;
  }

  canApproveSecond(panel: BoxPanel): boolean {
    const status = this.normalizeStatus(panel.panelStatus);
    console.log(`🔍 canApproveSecond for ${panel.panelName}:`, { 
      rawStatus: panel.panelStatus, 
      normalizedStatus: status,
      canApprove: status === PanelStatus.FirstApprovalApproved || status === PanelStatus.SecondApprovalPending
    });
    return status === PanelStatus.FirstApprovalApproved ||
           status === PanelStatus.SecondApprovalPending;
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
      // In a real app, you'd reload from parent or emit event
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
}

