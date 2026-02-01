import { Injectable } from '@angular/core';
import { WIRCheckpoint } from '../models/wir.model';
import { Box } from '../models/box.model';

@Injectable({
  providedIn: 'root'
})
export class WirPdfExportService {

  constructor() { }

  /**
   * Export WIR checkpoint to PDF with professional inspection form layout
   */
  exportToPDF(checkpoint: WIRCheckpoint, box?: Box): void {
    const printWindow = window.open('', '_blank');
    if (!printWindow) {
      alert('Please allow popups to export PDF');
      return;
    }

    const content = this.generatePrintContent(checkpoint, box);

    printWindow.document.write(`
      <!DOCTYPE html>
      <html>
      <head>
        <title>${checkpoint.wirNumber} - ${checkpoint.wirName || 'Inspection Checklist'}</title>
        <style>${this.getPrintStyles()}</style>
      </head>
      <body>${content}</body>
      </html>
    `);

    printWindow.document.close();
    setTimeout(() => {
      printWindow.print();
      printWindow.close();
    }, 250);
  }

  /**
   * Generate HTML content for WIR inspection form
   */
  private generatePrintContent(checkpoint: WIRCheckpoint, box?: Box): string {
    const items = checkpoint.checklistItems || [];
    
    // Filter out items that don't have descriptions or are invalid
    const validItems = items.filter(item => 
      item && item.checkpointDescription && item.checkpointDescription.trim() !== ''
    );
    
    // If no valid items, don't generate empty sections
    if (validItems.length === 0) {
      return this.generateEmptyForm(checkpoint, box);
    }
    
    const groupedItems = this.groupChecklistItemsByCategory(validItems);
    const projectInfo = this.getProjectInfo();

    let html = `
      <div class="inspection-form-container">
        
        <!-- Form Header with Logos -->
        <div class="form-header">
          <div class="logos-row">
            <div class="logo-placeholder">PARSONS</div>
            <div class="logo-placeholder">AMAALA</div>
            <div class="logo-placeholder">AMANA</div>
            <div class="logo-placeholder">DUBOX</div>
          </div>
          
          <div class="form-title">
            <h1>${checkpoint.wirName || 'Inspection Checklist'}</h1>
            <div class="form-code">${checkpoint.wirNumber}</div>
          </div>
        </div>

        <!-- Project Information Table -->
        <div class="project-info-section">
          <table class="info-table">
            <tr>
              <td class="label">Project:</td>
              <td class="value">${projectInfo.project}</td>
              <td class="label">Insp. No.:</td>
              <td class="value">${checkpoint.wirNumber}</td>
            </tr>
            <tr>
              <td class="label">Client:</td>
              <td class="value">${projectInfo.client}</td>
              <td class="label">Date:</td>
              <td class="value">${this.formatDate(checkpoint.inspectionDate) || '_____'}</td>
            </tr>
            <tr>
              <td class="label">Consultant:</td>
              <td class="value">${projectInfo.consultant}</td>
              <td class="label">Location:</td>
              <td class="value">${box?.buildingNumber || 'Dubox Factory'}</td>
            </tr>
            <tr>
              <td class="label">Contractor:</td>
              <td class="value">${projectInfo.contractor}</td>
              <td class="label">Element/Box:</td>
              <td class="value">${box?.code || box?.name || '_____'}</td>
            </tr>
            <tr>
              <td class="label">Sub-Contractor:</td>
              <td class="value" colspan="3">${projectInfo.subContractor}</td>
            </tr>
          </table>
        </div>

        <!-- Reference Documents -->
        <div class="reference-section">
          <strong>Reference Documents (Specifications, Drawings, others…):</strong>
          <div class="reference-text">${this.getReferenceDocuments(validItems) || 'As per approved drawings and specifications'}</div>
        </div>

        <!-- Activity/Item -->
        <div class="activity-section">
          <strong>Activity/Item:</strong> ${checkpoint.wirDescription || checkpoint.wirName || 'Work Inspection'}
        </div>

        <!-- Checklist Sections -->
        <div class="checklist-sections-form">`;

    // Group items by category and filter out empty categories
    const categoriesWithItems = Object.entries(groupedItems).filter(([category, categoryItems]) => {
      return categoryItems && categoryItems.length > 0;
    });

    // Only generate sections that have items
    categoriesWithItems.forEach(([category, categoryItems]) => {
      html += `
          <div class="section-block">
            <!-- Section Header -->
            <div class="section-title-bar">
              <h2>${this.escapeHtml(category)}</h2>
            </div>

            <!-- Checklist Items Table -->
            <table class="checklist-items-table">
              <thead>
                <tr>
                  <th class="col-sr">Sr.#</th>
                  <th class="col-desc">Description / Acceptance Criteria</th>
                  <th class="col-ref">Reference</th>
                  <th class="col-check">Y</th>
                  <th class="col-check">N</th>
                  <th class="col-check">N/A</th>
                  <th class="col-remarks">Remarks (if Any)</th>
                </tr>
              </thead>
              <tbody>`;

      categoryItems.forEach((item, idx) => {
        const isPass = item.status === 'Pass';
        const isFail = item.status === 'Fail';
        const isNA = item.status === 'N/A' || item.status === 'NA';
        const isPending = !item.status || item.status === 'Pending';

        html += `
                <tr>
                  <td class="col-sr">${idx + 1}</td>
                  <td class="col-desc">${this.escapeHtml(item.checkpointDescription || '')}</td>
                  <td class="col-ref">${this.escapeHtml(item.referenceDocument || '-')}</td>
                  <td class="col-check">
                    <div class="checkbox-cell">
                      <div class="check-mark ${isPass ? 'checked' : ''}">${isPass ? '✓' : ''}</div>
                    </div>
                  </td>
                  <td class="col-check">
                    <div class="checkbox-cell">
                      <div class="check-mark ${isFail ? 'checked' : ''}">${isFail ? '✗' : ''}</div>
                    </div>
                  </td>
                  <td class="col-check">
                    <div class="checkbox-cell">
                      <div class="check-mark ${isNA ? 'checked' : ''}">${isNA ? 'N/A' : ''}</div>
                    </div>
                  </td>
                  <td class="col-remarks">${this.escapeHtml(item.remarks || '')}</td>
                </tr>`;
      });

      html += `
              </tbody>
            </table>
          </div>`;
    });

    html += `
        </div>

        <!-- Comments Section -->
        <div class="comments-section">
          <div class="section-title-bar">
            <h2>Comments:</h2>
          </div>
          <div class="comments-box">
            ${this.escapeHtml(checkpoint.comments || '')}
          </div>
        </div>

        <!-- Signature Section -->
        <div class="signature-section">
          <div class="section-title-bar">
            <h2>Checked by:</h2>
          </div>
          
          <div class="signature-grid">
            <!-- Civil Works / Sub Contractor -->
            <div class="signature-block">
              <div class="block-title">Civil Works / Sub Contractor</div>
              <div class="signature-fields">
                <div class="field-row">
                  <label>Name:</label>
                  <span class="signature-line">${this.escapeHtml(checkpoint.requestedBy || '____________________')}</span>
                </div>
                <div class="field-row">
                  <label>Position:</label>
                  <span class="signature-line">Project Engineer</span>
                </div>
                <div class="field-row">
                  <label>Signature:</label>
                  <div class="signature-box"></div>
                </div>
                <div class="field-row">
                  <label>Date:</label>
                  <span class="signature-line">${this.formatDate(checkpoint.requestedDate) || '____________________'}</span>
                </div>
              </div>
            </div>

            <!-- QC Engineer -->
            <div class="signature-block">
              <div class="block-title">QC / QA Engineer</div>
              <div class="signature-fields">
                <div class="field-row">
                  <label>Name:</label>
                  <span class="signature-line">${this.escapeHtml(checkpoint.inspectorName || '____________________')}</span>
                </div>
                <div class="field-row">
                  <label>Position:</label>
                  <span class="signature-line">QA/QC Engineer</span>
                </div>
                <div class="field-row">
                  <label>Signature:</label>
                  <div class="signature-box"></div>
                </div>
                <div class="field-row">
                  <label>Date:</label>
                  <span class="signature-line">${this.formatDate(checkpoint.inspectionDate) || '____________________'}</span>
                </div>
              </div>
            </div>

            <!-- Consultant (Optional) -->
            <div class="signature-block optional">
              <div class="block-title">Consultant (Optional)</div>
              <div class="signature-fields">
                <div class="field-row">
                  <label>Name:</label>
                  <span class="signature-line">____________________</span>
                </div>
                <div class="field-row">
                  <label>Position:</label>
                  <span class="signature-line">____________________</span>
                </div>
                <div class="field-row">
                  <label>Signature:</label>
                  <div class="signature-box"></div>
                </div>
                <div class="field-row">
                  <label>Date:</label>
                  <span class="signature-line">____________________</span>
                </div>
              </div>
            </div>

            <!-- Client (Optional) -->
            <div class="signature-block optional">
              <div class="block-title">Client (Optional)</div>
              <div class="signature-fields">
                <div class="field-row">
                  <label>Name:</label>
                  <span class="signature-line">____________________</span>
                </div>
                <div class="field-row">
                  <label>Position:</label>
                  <span class="signature-line">____________________</span>
                </div>
                <div class="field-row">
                  <label>Signature:</label>
                  <div class="signature-box"></div>
                </div>
                <div class="field-row">
                  <label>Date:</label>
                  <span class="signature-line">____________________</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Status Watermark -->
        <div class="status-watermark" data-status="${checkpoint.status || 'Pending'}">
          ${checkpoint.status || 'PENDING'}
        </div>
      </div>
    `;

    return html;
  }

  /**
   * Generate empty form when no items exist
   */
  private generateEmptyForm(checkpoint: WIRCheckpoint, box?: Box): string {
    const projectInfo = this.getProjectInfo();
    
    return `
      <div class="inspection-form-container">
        <div class="form-header">
          <div class="logos-row">
            <div class="logo-placeholder">PARSONS</div>
            <div class="logo-placeholder">AMAALA</div>
            <div class="logo-placeholder">AMANA</div>
            <div class="logo-placeholder">DUBOX</div>
          </div>
          <div class="form-title">
            <h1>${checkpoint.wirName || 'Inspection Checklist'}</h1>
            <div class="form-code">${checkpoint.wirNumber}</div>
          </div>
        </div>
        
        <div class="project-info-section">
          <table class="info-table">
            <tr>
              <td class="label">Project:</td>
              <td class="value">${projectInfo.project}</td>
              <td class="label">Insp. No.:</td>
              <td class="value">${checkpoint.wirNumber}</td>
            </tr>
          </table>
        </div>
        
        <div class="empty-message">
          <p>No checklist items available for this inspection.</p>
        </div>
      </div>
    `;
  }

  /**
   * Group checklist items by category (only non-empty items)
   */
  private groupChecklistItemsByCategory(items: any[]): Record<string, any[]> {
    const grouped: Record<string, any[]> = {};

    items.forEach(item => {
      // Skip items without descriptions
      if (!item || !item.checkpointDescription || item.checkpointDescription.trim() === '') {
        return;
      }
      
      const category = item.categoryName || item.sectionName || 'General';
      if (!grouped[category]) {
        grouped[category] = [];
      }
      grouped[category].push(item);
    });

    // Remove empty categories
    Object.keys(grouped).forEach(key => {
      if (!grouped[key] || grouped[key].length === 0) {
        delete grouped[key];
      }
    });

    return grouped;
  }

  /**
   * Get project information
   */
  private getProjectInfo() {
    return {
      project: 'AMAALA STAFF VILLAGE – ZONE 05',
      client: 'AMAALA COMPANY',
      consultant: 'SAUDI ARABIAN PARSONS LTD',
      contractor: 'SAUDI AMANA CONTRACTING CO. L.L.C.',
      subContractor: 'DUBOX INDUSTRIAL PRECAST PRODUCTS LLC'
    };
  }

  /**
   * Extract reference documents from checklist items
   */
  private getReferenceDocuments(items: any[]): string {
    const refs = new Set<string>();
    items.forEach(item => {
      if (item && item.referenceDocument && 
          item.referenceDocument !== 'General' && 
          item.referenceDocument !== '-' && 
          item.referenceDocument.trim() !== '') {
        refs.add(item.referenceDocument);
      }
    });
    return Array.from(refs).join(', ');
  }

  /**
   * Format date for display
   */
  private formatDate(date: any): string {
    if (!date) return '';
    try {
      const d = new Date(date);
      if (isNaN(d.getTime())) return '';
      return d.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
    } catch {
      return '';
    }
  }

  /**
   * Escape HTML to prevent XSS
   */
  private escapeHtml(text: string): string {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }

  /**
   * Get professional print styles matching the inspection form
   */
  private getPrintStyles(): string {
    return `
      * { margin: 0; padding: 0; box-sizing: border-box; }
      body {
        font-family: 'QA', sans-serif;
        padding: 20px;
        color: #000;
        background: white;
      }
      
      .inspection-form-container {
        background: white;
        max-width: 1200px;
        margin: 0 auto;
        position: relative;
      }

      /* Form Header */
      .form-header {
        border: 2px solid #000;
        padding: 15px;
        margin-bottom: 20px;
        page-break-inside: avoid;
      }

      .logos-row {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        gap: 10px;
        margin-bottom: 15px;
        padding-bottom: 15px;
        border-bottom: 2px solid #000;
      }

      .logo-placeholder {
        text-align: center;
        font-weight: bold;
        font-size: 18px;
        padding: 20px 10px;
        border: 1px solid #ccc;
        background: linear-gradient(135deg, #f5f5f5 0%, #e0e0e0 100%);
        color: #333;
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 60px;
      }

      .form-title {
        text-align: center;
      }

      .form-title h1 {
        margin: 0;
        font-size: 20px;
        font-weight: bold;
        text-transform: uppercase;
        color: #000;
      }

      .form-code {
        font-size: 14px;
        color: #666;
        margin-top: 5px;
      }

      /* Project Information Table */
      .project-info-section {
        margin-bottom: 15px;
        page-break-inside: avoid;
      }

      .info-table {
        width: 100%;
        border-collapse: collapse;
        border: 2px solid #000;
      }

      .info-table tr {
        border-bottom: 1px solid #000;
      }

      .info-table tr:last-child {
        border-bottom: none;
      }

      .info-table td {
        padding: 8px 12px;
        font-size: 13px;
        border-right: 1px solid #000;
      }

      .info-table td:last-child {
        border-right: none;
      }

      .info-table td.label {
        font-weight: bold;
        background: #f0f0f0;
        width: 20%;
      }

      .info-table td.value {
        width: 30%;
      }

      /* Reference Documents Section */
      .reference-section {
        margin-bottom: 15px;
        padding: 10px;
        background: #f9f9f9;
        border: 1px solid #ccc;
        font-size: 13px;
        page-break-inside: avoid;
      }

      .reference-section strong {
        display: block;
        margin-bottom: 5px;
      }

      .reference-text {
        font-style: italic;
        color: #555;
      }

      /* Activity Section */
      .activity-section {
        margin-bottom: 20px;
        padding: 10px;
        background: #f0f0f0;
        border: 1px solid #000;
        font-size: 13px;
        page-break-inside: avoid;
      }

      .activity-section strong {
        margin-right: 10px;
      }

      /* Empty Message */
      .empty-message {
        padding: 40px;
        text-align: center;
        border: 2px solid #ccc;
        margin: 20px 0;
        background: #f9f9f9;
      }

      .empty-message p {
        font-size: 16px;
        color: #666;
      }

      /* Checklist Sections */
      .checklist-sections-form {
        page-break-inside: auto;
      }

      .section-block {
        margin-bottom: 25px;
        page-break-inside: avoid;
      }

      .section-title-bar {
        background: linear-gradient(135deg, #4a5568 0%, #2d3748 100%);
        color: white;
        padding: 10px 15px;
        border: 2px solid #000;
        border-bottom: none;
        page-break-after: avoid;
      }

      .section-title-bar h2 {
        margin: 0;
        font-size: 16px;
        font-weight: bold;
        text-transform: uppercase;
      }

      .checklist-items-table {
        width: 100%;
        border-collapse: collapse;
        border: 2px solid #000;
        font-size: 12px;
        page-break-inside: auto;
      }

      .checklist-items-table thead tr {
        background: #e8e8e8;
        font-weight: bold;
        page-break-after: avoid;
      }

      .checklist-items-table th,
      .checklist-items-table td {
        border: 1px solid #000;
        padding: 8px;
        text-align: left;
        vertical-align: middle;
      }

      .checklist-items-table tbody tr {
        page-break-inside: avoid;
      }

      .checklist-items-table th {
        font-size: 11px;
        text-transform: uppercase;
        text-align: center;
      }

      .col-sr { width: 50px; text-align: center !important; }
      .col-desc { width: auto; min-width: 300px; }
      .col-ref { width: 150px; text-align: center !important; }
      .col-check { width: 40px; text-align: center !important; }
      .col-remarks { width: 200px; }

      .col-sr {
        text-align: center;
        font-weight: bold;
        color: #2c5282;
      }

      .col-desc {
        line-height: 1.5;
        font-size: 12px;
      }

      .col-ref {
        font-size: 11px;
        color: #555;
      }

      .checkbox-cell {
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 30px;
      }

      .check-mark {
        width: 24px;
        height: 24px;
        border: 2px solid #000;
        display: flex;
        align-items: center;
        justify-content: center;
        background: white;
        font-size: 18px;
        font-weight: bold;
        color: #2c5282;
      }

      .check-mark.checked {
        background: #e6f7ff;
        border-color: #2c5282;
      }

      /* Comments Section */
      .comments-section {
        margin-bottom: 25px;
        page-break-inside: avoid;
      }

      .comments-box {
        border: 2px solid #000;
        padding: 10px;
        min-height: 80px;
        background: white;
      }

      /* Signature Section */
      .signature-section {
        margin-bottom: 25px;
        page-break-inside: avoid;
      }

      .signature-grid {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 20px;
        border: 2px solid #000;
        padding: 20px;
        background: #fafafa;
      }

      .signature-block {
        border: 1px solid #ccc;
        padding: 15px;
        background: white;
      }

      .signature-block.optional {
        opacity: 0.7;
      }

      .block-title {
        font-weight: bold;
        font-size: 13px;
        margin-bottom: 12px;
        padding-bottom: 8px;
        border-bottom: 2px solid #e0e0e0;
        color: #2c5282;
      }

      .field-row {
        display: flex;
        align-items: center;
        margin-bottom: 10px;
      }

      .field-row label {
        font-weight: 600;
        font-size: 12px;
        width: 80px;
        color: #333;
      }

      .signature-line {
        flex: 1;
        border-bottom: 1px solid #000;
        padding: 4px 8px;
        font-size: 12px;
        display: inline-block;
      }

      .signature-box {
        flex: 1;
        height: 50px;
        border: 1px dashed #999;
        background: #f9f9f9;
      }

      /* Status Watermark */
      .status-watermark {
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%) rotate(-45deg);
        font-size: 120px;
        font-weight: bold;
        opacity: 0.05;
        color: #000;
        pointer-events: none;
        z-index: 1;
        text-transform: uppercase;
      }

      .status-watermark[data-status="Approved"] {
        color: #22c55e;
        opacity: 0.1;
      }

      .status-watermark[data-status="Rejected"] {
        color: #ef4444;
        opacity: 0.1;
      }

      .status-watermark[data-status="ConditionalApproval"] {
        color: #f59e0b;
        opacity: 0.1;
      }

      @page {
        margin: 1cm;
        size: A4;
      }

      @media print {
        body { margin: 0; padding: 10px; }
        
        .inspection-form-container {
          page-break-inside: avoid;
        }
        
        .form-header {
          page-break-inside: avoid;
          page-break-after: avoid;
        }
        
        .section-block { 
          page-break-inside: avoid; 
        }
        
        .section-title-bar {
          page-break-after: avoid;
        }
        
        .checklist-items-table tbody tr {
          page-break-inside: avoid;
        }
        
        .signature-section { 
          page-break-inside: avoid; 
        }
        
        .comments-section {
          page-break-inside: avoid;
        }
      }
    `;
  }
}