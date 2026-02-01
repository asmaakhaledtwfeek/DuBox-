import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WIRWithChecklist, ChecklistSection } from '../../../../core/services/wir-checklist.service';

@Component({
  selector: 'app-detailed-form-view',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './detailed-form-view.component.html',
  styleUrls: ['./detailed-form-view.component.scss']
})
export class DetailedFormViewComponent {
  @Input() wir!: WIRWithChecklist;
  @Input() readonly: boolean = false;
  @Input() boxNumber?: string;
  @Input() location?: string;
  @Input() inspectionDate?: string;

  get projectInfo() {
    return {
      project: 'AMAALA STAFF VILLAGE – ZONE 05',
      client: 'AMAALA COMPANY',
      consultant: 'SAUDI ARABIAN PARSONS LTD',
      contractor: 'SAUDI AMANA CONTRACTING CO. L.L.C.',
      subContractor: 'DUBOX INDUSTRIAL PRECAST PRODUCTS LLC'
    };
  }

  get referenceDocuments(): string {
    // Extract unique reference documents from all items
    const refs = new Set<string>();
    this.wir.sections.forEach(section => {
      section.items.forEach(item => {
        if (item.referenceDocument && item.referenceDocument !== 'General') {
          refs.add(item.referenceDocument);
        }
      });
    });
    return Array.from(refs).join(', ');
  }

  getStatusIcon(status: string): string {
    switch (status.toLowerCase()) {
      case 'pass': return '✓';
      case 'fail': return '✗';
      default: return '';
    }
  }

  printForm(): void {
    window.print();
  }

  exportToPDF(): void {
    // Method 1: Use browser's print-to-PDF
    // This opens print dialog with PDF as default option
    if (window.matchMedia('print').media === 'print') {
      window.print();
    } else {
      // Fallback: Trigger print which user can save as PDF
      const printWindow = window.open('', '_blank');
      if (printWindow) {
        const content = document.querySelector('.inspection-form-container');
        if (content) {
          printWindow.document.write(`
            <!DOCTYPE html>
            <html>
            <head>
              <title>${this.wir.wirNumber} - ${this.wir.wirName}</title>
              <style>
                /* Copy print styles */
                body { margin: 0; padding: 20px; font-family: 'QA', sans-serif; }
                ${this.getPrintStyles()}
              </style>
            </head>
            <body>
              ${content.outerHTML}
            </body>
            </html>
          `);
          printWindow.document.close();
          setTimeout(() => {
            printWindow.print();
            printWindow.close();
          }, 250);
        }
      }
    }
  }

  private getPrintStyles(): string {
    // Extract print styles from component
    return `
      .inspection-form-container { background: white; padding: 20px; max-width: 1200px; margin: 0 auto; }
      .form-header { border: 2px solid #000; padding: 15px; margin-bottom: 20px; }
      .logos-row { display: grid; grid-template-columns: repeat(4, 1fr); gap: 10px; margin-bottom: 15px; padding-bottom: 15px; border-bottom: 2px solid #000; }
      .logo-placeholder { text-align: center; font-weight: bold; font-size: 18px; padding: 20px 10px; border: 1px solid #ccc; background: #f5f5f5; }
      .form-title h1 { margin: 0; font-size: 20px; font-weight: bold; text-align: center; }
      .info-table { width: 100%; border-collapse: collapse; border: 2px solid #000; margin-bottom: 15px; }
      .info-table td { padding: 8px 12px; font-size: 13px; border: 1px solid #000; }
      .info-table td.label { font-weight: bold; background: #f0f0f0; }
      .reference-section { margin-bottom: 15px; padding: 10px; background: #f9f9f9; border: 1px solid #ccc; }
      .activity-section { margin-bottom: 20px; padding: 10px; background: #f0f0f0; border: 1px solid #000; }
      .section-title-bar { background: #4a5568; color: white; padding: 10px 15px; border: 2px solid #000; border-bottom: none; }
      .section-title-bar h2 { margin: 0; font-size: 16px; font-weight: bold; }
      .checklist-items-table { width: 100%; border-collapse: collapse; border: 2px solid #000; font-size: 12px; margin-bottom: 25px; }
      .checklist-items-table th, .checklist-items-table td { border: 1px solid #000; padding: 8px; text-align: left; }
      .checklist-items-table thead tr { background: #e8e8e8; font-weight: bold; }
      .signature-grid { display: grid; grid-template-columns: repeat(2, 1fr); gap: 20px; border: 2px solid #000; padding: 20px; }
      .signature-block { border: 1px solid #ccc; padding: 15px; background: white; }
      .form-actions { display: none !important; }
      .status-watermark { position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%) rotate(-45deg); font-size: 120px; opacity: 0.08; }
      @page { margin: 1cm; }
    `;
  }
}
