import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PanelService } from '../../../core/services/panel.service';

export interface ScanResult {
  barcode: string;
  scanType: 'SiteArrival' | 'Installation';
  success: boolean;
  panelName?: string;
  message: string;
}

@Component({
  selector: 'app-barcode-scanner',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './barcode-scanner.component.html',
  styleUrl: './barcode-scanner.component.scss'
})
export class BarcodeScannerComponent {
  @Input() projectId!: string;
  @Input() scanType: 'SiteArrival' | 'Installation' = 'SiteArrival';
  @Output() scanComplete = new EventEmitter<ScanResult>();
  @Output() close = new EventEmitter<void>();

  manualBarcode = '';
  scanning = false;
  error = '';
  successMessage = '';
  latitude: number | null = null;
  longitude: number | null = null;

  constructor(private panelService: PanelService) {
    this.getLocation();
  }

  getLocation(): void {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          this.latitude = position.coords.latitude;
          this.longitude = position.coords.longitude;
        },
        (error) => {
          console.warn('Geolocation not available:', error);
        }
      );
    }
  }

  async scanManualBarcode(): Promise<void> {
    if (!this.manualBarcode.trim()) {
      this.error = 'Please enter a barcode';
      return;
    }

    await this.processBarcode(this.manualBarcode.trim());
  }

  async processBarcode(barcode: string): Promise<void> {
    this.scanning = true;
    this.error = '';
    this.successMessage = '';

    const command = {
      barcode: barcode,
      scanType: this.scanType,
      latitude: this.latitude ?? undefined,
      longitude: this.longitude ?? undefined
    };

    this.panelService.scanPanel(command).subscribe({
      next: (response: any) => {
        this.scanning = false;
        const result = response?.data || response;
        
        this.successMessage = `Panel scanned successfully! Status updated.`;
        
        const scanResult: ScanResult = {
          barcode: barcode,
          scanType: this.scanType,
          success: true,
          panelName: result?.panelName,
          message: this.successMessage
        };

        this.scanComplete.emit(scanResult);
        
        // Auto-close after 2 seconds
        setTimeout(() => {
          this.closeScanner();
        }, 2000);
      },
      error: (err: any) => {
        this.scanning = false;
        this.error = err?.error?.message || err?.message || 'Failed to scan barcode. Panel not found.';
        
        const scanResult: ScanResult = {
          barcode: barcode,
          scanType: this.scanType,
          success: false,
          message: this.error
        };

        this.scanComplete.emit(scanResult);
      }
    });
  }

  closeScanner(): void {
    this.close.emit();
  }

  getScanTypeLabel(): string {
    switch (this.scanType) {
      case 'SiteArrival':
        return 'Arrival at Site';
      case 'Installation':
        return 'Installation on Site';
      default:
        return 'Scan';
    }
  }

  getScanTypeDescription(): string {
    switch (this.scanType) {
      case 'SiteArrival':
        return 'Scan panel barcode to mark as arrived at site and approve';
      case 'Installation':
        return 'Scan panel barcode to mark as installed on site';
      default:
        return 'Scan panel barcode';
    }
  }

  clearInput(): void {
    this.manualBarcode = '';
    this.error = '';
    this.successMessage = '';
  }
}

