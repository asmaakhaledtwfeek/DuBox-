import { Injectable } from '@angular/core';
import { WIRCheckpoint, WIRCheckpointChecklistItem } from '../models/wir.model';
import { Box } from '../models/box.model';
import { Project } from '../models/project.model';
import html2pdf from 'html2pdf.js';

export interface ChecklistGroup {
  checklistId: string;
  checklistName: string;
  checklistCode?: string;
  sections: SectionGroup[];
}

export interface SectionGroup {
  sectionId: string;
  sectionName: string;
  sectionOrder: number;
  items: WIRCheckpointChecklistItem[];
}

export interface ProjectInfo {
  projectName: string;
  projectCode: string;
  clientName: string;
  consultant: string;
  contractor: string;
  subContractor: string;
  location: string;
}

@Injectable({
  providedIn: 'root'
})
export class WirExportService {

  constructor() { }

  /**
   * Download WIR checkpoint as PDF (Direct Download)
   */
  async downloadWIRAsPDF(checkpoint: WIRCheckpoint, box?: Box | null, project?: Project | null): Promise<void> {
    const projectInfo = this.buildProjectInfo(checkpoint, box, project);
    const content = this.generateWIRPrintContent(checkpoint, box, projectInfo);
    const styles = this.getWIRPrintStyles();
    const fileName = `${checkpoint.wirNumber}_${checkpoint.wirName || 'Inspection_Checklist'}`.replace(/\s+/g, '_');
    
    // Create a temporary container that's visible but positioned off-screen
    const container = document.createElement('div');
    container.style.position = 'fixed';
    container.style.top = '0';
    container.style.left = '0';
    container.style.width = '210mm';
    container.style.zIndex = '-1000';
    container.style.opacity = '0';
    container.style.pointerEvents = 'none';
    
    // Create style element
    const styleElement = document.createElement('style');
    styleElement.textContent = styles;
    container.appendChild(styleElement);
    
    // Add content
    const contentDiv = document.createElement('div');
    contentDiv.innerHTML = content;
    container.appendChild(contentDiv);
    
    document.body.appendChild(container);

    try {
      // Wait for content to render
      await new Promise(resolve => setTimeout(resolve, 100));

      // Configure html2pdf options
      const opt = {
        margin: [5, 5, 5, 5] as [number, number, number, number],
        filename: `${fileName}.pdf`,
        image: { type: 'jpeg' as const, quality: 0.95 },
        html2canvas: { 
          scale: 2,
          useCORS: true,
          logging: true,
          backgroundColor: '#ffffff',
          windowWidth: 794, // A4 width in pixels at 96 DPI
          windowHeight: 1123 // A4 height in pixels at 96 DPI
        },
        jsPDF: { 
          unit: 'mm', 
          format: 'a4', 
          orientation: 'portrait' as const,
          compress: true
        },
        pagebreak: { 
          mode: ['avoid-all', 'css', 'legacy'],
          before: '.page-break'
        }
      };

      // Generate and download PDF
      console.log('Generating PDF from container...');
      await html2pdf().set(opt).from(contentDiv).save();
      console.log('PDF generated successfully');
      
    } catch (error) {
      console.error('Error generating PDF:', error);
      alert('Failed to generate PDF. Please try the Print PDF option instead.');
    } finally {
      // Clean up after a delay to ensure PDF is downloaded
      setTimeout(() => {
        if (document.body.contains(container)) {
          document.body.removeChild(container);
        }
      }, 1000);
    }
  }

  /**
   * Export WIR to PDF via Print Dialog (Alternative method)
   */
  exportWIRToPrintDialog(checkpoint: WIRCheckpoint, box?: Box | null, project?: Project | null): void {
    const projectInfo = this.buildProjectInfo(checkpoint, box, project);
    const content = this.generateWIRPrintContent(checkpoint, box, projectInfo);
    const styles = this.getWIRPrintStyles();
    const fileName = `${checkpoint.wirNumber}_${checkpoint.wirName || 'Inspection_Checklist'}`.replace(/\s+/g, '_');
    
    // Open print dialog which allows saving as PDF
    const printWindow = window.open('', '_blank');
    if (!printWindow) {
      alert('Please allow popups to generate PDF');
      return;
    }

    printWindow.document.write(`<!DOCTYPE html>
<html>
<head>
  <meta charset="UTF-8">
  <title>${checkpoint.wirNumber} - ${checkpoint.wirName || 'Inspection Checklist'}</title>
  <style>${styles}</style>
</head>
<body>
  ${content}
  <script>
    window.onload = function() {
      setTimeout(function() {
        window.print();
      }, 500);
    };
  </script>
</body>
</html>`);
    
    printWindow.document.close();
  }

  /**
   * Download WIR checkpoint as HTML file (legacy method)
   */
  downloadWIRAsHTML(checkpoint: WIRCheckpoint, box?: Box | null, project?: Project | null): void {
    const projectInfo = this.buildProjectInfo(checkpoint, box, project);
    const content = this.generateWIRPrintContent(checkpoint, box, projectInfo);
    const styles = this.getWIRPrintStyles();
    const fileName = `${checkpoint.wirNumber}_${checkpoint.wirName || 'Inspection_Checklist'}`.replace(/\s+/g, '_');
    
    const fullHTML = `<!DOCTYPE html>
<html>
<head>
  <meta charset="UTF-8">
  <title>${checkpoint.wirNumber} - ${checkpoint.wirName || 'Inspection Checklist'}</title>
  <style>${styles}</style>
</head>
<body>
  ${content}
</body>
</html>`;

    const blob = new Blob([fullHTML], { type: 'text/html' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${fileName}.html`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
  }

  /**
   * Generate WIR print content HTML
   */
  generateWIRPrintContent(checkpoint: WIRCheckpoint, box?: Box | null, projectInfo?: ProjectInfo): string {
    const items = checkpoint.checklistItems || [];
    const groupedChecklists = this.groupItemsByChecklist(items);
    const referenceDocuments = this.extractReferenceDocuments(items);

    let html = '';

    // Generate a page for each checklist
    groupedChecklists.forEach((checklist, index) => {
      html += this.generateChecklistPage(
        checkpoint, 
        checklist, 
        projectInfo!, 
        box, 
        referenceDocuments,
        index > 0 // Add page break for subsequent pages
      );
    });

    return html;
  }

  /**
   * Build project info from available data
   */
  private buildProjectInfo(checkpoint: WIRCheckpoint, box?: Box | null, project?: Project | null): ProjectInfo {
    return {
      projectName: project?.name || 'AMANA STAFF VILLAGE ZONE-05',
      projectCode: checkpoint.projectCode || project?.code || '',
      clientName: project?.clientName || 'AMAALA',
      consultant: 'PARSONS',
      contractor: 'Saudi Amana Contracting Co',
      subContractor: 'DUBOX INDUSTRIAL PRECAST PRODUCTS LLC',
      location: project?.location || box?.zone || 'Dubox Factory'
    };
  }

  /**
   * Group checklist items by checklist, then by section
   */
  private groupItemsByChecklist(items: WIRCheckpointChecklistItem[]): ChecklistGroup[] {
    const checklistMap = new Map<string, ChecklistGroup>();

    items.forEach(item => {
      const checklistId = item.checklistId || 'default';
      const checklistName = item.checklistName || 'General Checklist';
      const checklistCode = item.checklistCode;
      const sectionId = item.sectionId || 'default';
      const sectionName = item.sectionName || item.categoryName || 'General';
      const sectionOrder = item.sectionOrder || 0;

      // Get or create checklist group
      if (!checklistMap.has(checklistId)) {
        checklistMap.set(checklistId, {
          checklistId,
          checklistName,
          checklistCode,
          sections: []
        });
      }

      const checklistGroup = checklistMap.get(checklistId)!;

      // Get or create section within checklist
      let section = checklistGroup.sections.find(s => s.sectionId === sectionId);
      if (!section) {
        section = {
          sectionId,
          sectionName,
          sectionOrder,
          items: []
        };
        checklistGroup.sections.push(section);
      }

      section.items.push(item);
    });

    // Sort sections within each checklist
    checklistMap.forEach(checklist => {
      checklist.sections.sort((a, b) => a.sectionOrder - b.sectionOrder);
      
      // Sort items within each section by sequence
      checklist.sections.forEach(section => {
        section.items.sort((a, b) => (a.sequence || 0) - (b.sequence || 0));
      });
    });

    return Array.from(checklistMap.values());
  }

  /**
   * Generate a single checklist page
   */
  private generateChecklistPage(
    checkpoint: WIRCheckpoint,
    checklist: ChecklistGroup,
    projectInfo: ProjectInfo,
    box?: Box | null,
    referenceDocuments?: string,
    addPageBreak: boolean = false
  ): string {
    let html = addPageBreak ? '<div class="page-break"></div>' : '';
    
    html += `
      <div class="inspection-form-container">
        <!-- DuBox Logo Watermark -->
        <div class="watermark">
          ${this.getDuBoxLogoSVG()}
        </div>

        <!-- Form Header with Border -->
        <div class="form-header-border">
          <!-- Company Logos Row -->
          <div class="company-logos-row">
            <div class="logo-box logo-amaala">
              <span class="logo-text">AMAALA</span>
            </div>
            <div class="logo-box logo-parsons">
              <span class="logo-text">PARSONS</span>
            </div>
            <div class="logo-box logo-amana">
              <span class="logo-text">AMANA</span>
            </div>
          </div>
          
          <!-- Gray Title Bar with Checklist Name -->
          <div class="title-bar">
            <h1>${checklist.checklistName.toUpperCase()}</h1>
          </div>
        </div>

        <!-- Project Information Table -->
        <div class="project-info-section">
          <table class="info-table">
            <tr>
              <td class="label">Project</td>
              <td class="value" colspan="3">: ${projectInfo.projectName}</td>
            </tr>
            <tr>
              <td class="label">Client</td>
              <td class="value">: ${projectInfo.clientName}</td>
              <td class="label">Insp. No.</td>
              <td class="value">${checkpoint.wirNumber || '0'}</td>
            </tr>
            <tr>
              <td class="label">Consultant</td>
              <td class="value">: ${projectInfo.consultant}</td>
              <td class="label">Date</td>
              <td class="value">${this.formatWIRDate(checkpoint.requestedDate) || '0/Jan/00'}</td>
            </tr>
            <tr>
              <td class="label">Contractor</td>
              <td class="value">: ${projectInfo.contractor}</td>
              <td class="label">Location</td>
              <td class="value">${projectInfo.location}</td>
            </tr>
            <tr>
              <td class="label">Sub-Contractor</td>
              <td class="value">: ${projectInfo.subContractor}</td>
              <td class="label">Box:</td>
              <td class="value">${box?.code || box?.name || '0'}</td>
            </tr>
          </table>
        </div>

        <!-- Reference Documents -->
        <div class="reference-section">
          <div class="reference-label">Reference Documents (Specifications, Drawings, others…):</div>
          <div class="reference-content">${referenceDocuments || ''}</div>
        </div>

        <!-- Activity/Item Section -->
        <div class="activity-section">
          <table class="activity-table">
            <tr>
              <td class="activity-label">Activity/Item:</td>
              <td class="activity-value">${checkpoint.wirDescription || checkpoint.wirName || ''}</td>
              <td class="yn-header-cell">Y<br/><small>(Pass)</small></td>
              <td class="yn-header-cell">N<br/><small>(Fail)</small></td>
              <td class="yn-header-cell">N/A</td>
              <td class="remarks-header-cell">Remarks</td>
            </tr>
          </table>
        </div>

        <!-- Checklist Items Table with Sections -->
        <div class="checklist-sections-form">
          <table class="checklist-items-table">
            <tbody>`;

    // Generate sections and items
    let itemCounter = 1;
    checklist.sections.forEach(section => {
      // Add section header row
      html += `
              <tr class="section-header-row">
                <td colspan="7" class="section-header-cell">
                  <strong>${section.sectionName}</strong>
                </td>
              </tr>`;
      
      // Add items under this section
      section.items.forEach(item => {
        const isPassed = item.status === 'Pass';
        const isFailed = item.status === 'Fail';
        const statusClass = isPassed ? 'status-pass' : isFailed ? 'status-fail' : 'status-pending';
        
        html += `
              <tr class="item-row ${statusClass}">
                <td class="col-number">${itemCounter}</td>
                <td class="col-description">${this.escapeHtml(item.checkpointDescription)}</td>
                <td class="col-checkbox checkbox-y ${isPassed ? 'checked' : ''}">${isPassed ? '✓' : ''}</td>
                <td class="col-checkbox checkbox-n ${isFailed ? 'checked' : ''}">${isFailed ? '✗' : ''}</td>
                <td class="col-remarks">${item.remarks ? this.escapeHtml(item.remarks) : ''}</td>
              </tr>`;
        itemCounter++;
      });
    });

    html += `
            </tbody>
          </table>
        </div>

        <!-- Comments Section -->
        <div class="comments-section">
          <div class="comments-header">Comments:</div>
          <div class="comments-content">${checkpoint.comments || ''}</div>
        </div>

        <!-- Signature Section -->
        <div class="signature-section">
          <table class="signature-table">
            <tr>
              <td class="signature-block">
                <div class="signature-title">CIVIL ENGINEER</div>
                <div class="signature-row">
                  <span class="signature-label">Name</span>
                  <div class="signature-line">${checkpoint.requestedBy || ''}</div>
                </div>
                <div class="signature-row">
                  <span class="signature-label">Position:</span>
                  <div class="signature-value">Project Engineer</div>
                </div>
                <div class="signature-row">
                  <span class="signature-label">Signature</span>
                  <div class="signature-box"></div>
                </div>
                <div class="signature-row">
                  <span class="signature-label">Date</span>
                  <div class="signature-line">${this.formatWIRDate(checkpoint.requestedDate) || '0/Jan/00'}</div>
                </div>
              </td>
              <td class="signature-block">
                <div class="signature-title">QC ENGINEER</div>
                <div class="signature-row">
                  <span class="signature-label">Name</span>
                  <div class="signature-line">${checkpoint.inspectorName || '0'}</div>
                </div>
                <div class="signature-row">
                  <span class="signature-label">Position:</span>
                  <div class="signature-value">${checkpoint.inspectorRole || 'QC Engineer'}</div>
                </div>
                <div class="signature-row">
                  <span class="signature-label">Signature</span>
                  <div class="signature-box"></div>
                </div>
                <div class="signature-row">
                  <span class="signature-label">Date</span>
                  <div class="signature-line">${this.formatWIRDate(checkpoint.inspectionDate) || '0/Jan/00'}</div>
                </div>
              </td>
            </tr>
          </table>
        </div>
      </div>
    `;

    return html;
  }

  /**
   * Extract reference documents from items
   */
  private extractReferenceDocuments(items: WIRCheckpointChecklistItem[]): string {
    const refs = new Set<string>();
    items.forEach(item => {
      if (item.referenceDocument && 
          item.referenceDocument !== '-' && 
          item.referenceDocument.trim() !== '' &&
          item.referenceDocument !== 'General') {
        refs.add(item.referenceDocument);
      }
    });
    return Array.from(refs).join(', ');
  }

  /**
   * Format date for WIR display
   */
  private formatWIRDate(date?: Date): string {
    if (!date) return '';
    const d = new Date(date);
    const day = d.getDate();
    const month = d.toLocaleString('en-US', { month: 'short' });
    const year = d.getFullYear().toString().slice(-2);
    return `${day}/${month}/${year}`;
  }

  /**
   * Escape HTML special characters
   */
  private escapeHtml(text: string): string {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }

  /**
   * Get DuBox logo SVG for watermark
   */
  private getDuBoxLogoSVG(): string {
    return `<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 412.52 100">
      <defs>
        <style>
          .logo-yellow{fill:#ffcb1b;}
          .logo-dark{fill:#333;}
          .logo-text{fill:#666;}
        </style>
      </defs>
      <!-- D -->
      <path class="logo-dark" d="M79.89,9.72h37.49v25.54l-11.44,11.23h-26.04V9.72ZM69.98,0v56.2h40.6l16.7-16.38V0h-57.3Z"/>
      <!-- U -->
      <polygon class="logo-dark" points="187.72 0 187.72 46.49 150.23 46.49 150.23 0 140.32 0 140.32 56.21 197.62 56.21 197.62 0 187.72 0"/>
      <!-- B -->
      <path class="logo-dark" d="M220.57,9.72h37.49v13.53h-19.84v9.71h19.84v13.53h-37.49V9.72ZM210.66,56.2h57.3V0h-57.3v56.2Z"/>
      <!-- O -->
      <path class="logo-dark" d="M290.91,9.72h37.49v36.77h-37.49V9.72ZM281,56.2h57.3V0h-57.3v56.2Z"/>
      <!-- X -->
      <polygon class="logo-dark" points="399.43 0 380.73 20.82 362.03 0 348.94 0 374.14 28.15 348.93 56.2 362.19 56.2 380.73 35.51 399.26 56.2 412.52 56.2 387.31 28.15 412.52 0 399.43 0"/>
      <!-- Yellow Box -->
      <polygon class="logo-yellow" points="0 0 0 56.54 40.1 56.54 56.59 40.05 56.59 0 0 0"/>
      <!-- Company Text -->
      <text x="206" y="75" text-anchor="middle" font-size="14" font-weight="600" class="logo-text" font-family="Arial, sans-serif">INDUSTRIAL PRECAST PRODUCTS</text>
    </svg>`;
  }


  /**
   * Get WIR print styles
   */
  getWIRPrintStyles(): string {
    return `
      * { 
        margin: 0; 
        padding: 0; 
        box-sizing: border-box; 
      }
      
      body { 
        font-family: Arial, sans-serif; 
        background: white; 
        padding: 0;
        margin: 0;
      }
      
      .page-break {
        page-break-before: always;
        break-before: always;
      }
      
      .inspection-form-container {
        background: white;
        border: 3px solid #000;
        margin: 20px auto;
        max-width: 210mm;
        position: relative;
      }
      
      /* Watermark */
      .watermark {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%) rotate(-30deg);
        width: 60%;
        max-width: 500px;
        opacity: 0.08;
        z-index: 0;
        pointer-events: none;
      }
      
      .watermark svg {
        width: 100%;
        height: auto;
        filter: grayscale(20%);
      }
      
      /* Ensure content is above watermark */
      .form-header-border,
      .project-info-section,
      .reference-section,
      .activity-section,
      .checklist-sections-form,
      .comments-section,
      .signature-section {
        position: relative;
        z-index: 1;
        background: white;
      }
      
      /* Form Header */
      .form-header-border {
        border-bottom: 2px solid #000;
      }
      
      /* Company Logos Row */
      .company-logos-row {
        display: grid;
        grid-template-columns: 1fr 1fr 1fr;
        border-bottom: 2px solid #000;
      }
      
      .logo-box {
        padding: 20px 15px;
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 70px;
        border-right: 2px solid #000;
        background: #fff;
      }
      
      .logo-box:last-child {
        border-right: none;
      }
      
      .logo-box.logo-amaala {
        background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
      }
      
      .logo-box.logo-parsons {
        background: linear-gradient(135deg, #e3f2fd 0%, #ffffff 100%);
      }
      
      .logo-box.logo-amana {
        background: linear-gradient(135deg, #fff3e0 0%, #ffffff 100%);
      }
      
      .logo-text {
        font-size: 22px;
        font-weight: 900;
        color: #000;
        letter-spacing: 3px;
        text-shadow: 1px 1px 2px rgba(0,0,0,0.1);
      }
      
      .logo-box.logo-parsons .logo-text {
        color: #003DA5;
        font-weight: 900;
      }
      
      .logo-box.logo-amana .logo-text {
        color: #c41230;
        font-weight: 900;
      }
      
      /* Title Bar */
      .title-bar {
        background: linear-gradient(to bottom, #d9d9d9 0%, #c0c0c0 100%);
        padding: 12px 15px;
        text-align: center;
        border-bottom: 2px solid #000;
      }
      
      .title-bar h1 {
        margin: 0;
        font-size: 13px;
        font-weight: 900;
        color: #000;
        text-transform: uppercase;
        letter-spacing: 1.5px;
      }
      
      /* Project Information Table */
      .project-info-section {
        border-bottom: 2px solid #000;
      }
      
      .info-table {
        width: 100%;
        border-collapse: collapse;
      }
      
      .info-table tr {
        border-bottom: 1px solid #000;
      }
      
      .info-table tr:last-child {
        border-bottom: none;
      }
      
      .info-table td {
        padding: 6px 10px;
        font-size: 11px;
        border-right: 1px solid #000;
        vertical-align: top;
      }
      
      .info-table td:last-child {
        border-right: none;
      }
      
      .info-table td.label {
        font-weight: bold;
        width: 20%;
      }
      
      .info-table td.value {
        width: 30%;
      }
      
      /* Reference Documents Section */
      .reference-section {
        padding: 8px 10px;
        border-bottom: 2px solid #000;
        font-size: 10px;
      }
      
      .reference-label {
        font-weight: bold;
        margin-bottom: 4px;
      }
      
      .reference-content {
        min-height: 15px;
      }
      
      /* Activity/Item Section */
      .activity-section {
        border-bottom: 2px solid #000;
      }
      
      .activity-table {
        width: 100%;
        border-collapse: collapse;
      }
      
      .activity-table td {
        padding: 8px 10px;
        font-size: 11px;
        border-right: 1px solid #000;
        border-left: 1px solid #000;
        vertical-align: middle;
      }
      
      .activity-table td:first-child {
        border-left: 1px solid #000;
      }
      
      .activity-table td:last-child {
        border-right: 1px solid #000;
      }
      
      .activity-table td.activity-label {
        font-weight: bold;
        width: 15%;
        border-left: 1px solid #000;
      }
      
       .activity-table td.activity-value {
         width: 50%;
       }
      
      .activity-table td.yn-header-cell {
        width: 8%;
        text-align: center;
        font-weight: bold;
        font-size: 10px;
        background: #f0f0f0;
        line-height: 1.3;
      }
      
      .activity-table td.yn-header-cell small {
        font-size: 8px;
        font-weight: normal;
        display: block;
        margin-top: 2px;
      }
      
      .activity-table td.remarks-header-cell {
        width: 20%;
        text-align: center;
        font-weight: bold;
        font-size: 10px;
        background: #f0f0f0;
      }
      
      /* Checklist Items Table */
      .checklist-sections-form {
        border-bottom: 2px solid #000;
      }
      
      .checklist-items-table {
        width: 100%;
        border-collapse: collapse;
      }
      
      .checklist-items-table tbody tr.section-header-row {
        background: #e8e8e8;
        border-top: 2px solid #000;
        border-bottom: 1px solid #000;
      }
      
      .checklist-items-table tbody td.section-header-cell {
        padding: 10px 12px;
        font-weight: 900;
        font-size: 11px;
        text-align: left;
        color: #000;
        text-transform: uppercase;
        letter-spacing: 1px;
        border: 1px solid #000;
        background: linear-gradient(to bottom, #e8e8e8 0%, #d0d0d0 100%);
      }
      
      .checklist-items-table tbody tr.item-row {
        border-bottom: 1px solid #000;
      }
      
      .checklist-items-table tbody tr.item-row:last-child {
        border-bottom: 1px solid #000;
      }
      
      .checklist-items-table tbody td {
        padding: 6px 8px;
        font-size: 10px;
        vertical-align: middle;
        border-right: 1px solid #000;
        border-bottom: 1px solid #000;
      }
      
      .checklist-items-table tbody td:first-child {
        border-left: 1px solid #000;
      }
      
      .checklist-items-table tbody td:last-child {
        border-right: 1px solid #000;
      }
      
      .checklist-items-table td.col-number {
        width: 5%;
        text-align: center;
        font-weight: bold;
      }
      
      .checklist-items-table td.col-description {
        width: 45%;
        line-height: 1.4;
        text-align: left;
      }
      
      .checklist-items-table td.col-checkbox {
        width: 8%;
        text-align: center;
        font-weight: bold;
        font-size: 16px;
        min-height: 24px;
        line-height: 24px;
        padding: 8px 4px;
      }
      
      .checklist-items-table td.col-remarks {
        width: 20%;
        text-align: left;
        font-size: 10px;
        line-height: 1.3;
        padding: 6px 8px;
        vertical-align: top;
      }
      
      .checklist-items-table td.checkbox-y.checked {
        background-color: #c3e6cb;
        color: #155724;
        font-weight: 900;
      }
      
      .checklist-items-table td.checkbox-n.checked {
        background-color: #f5c6cb;
        color: #721c24;
        font-weight: 900;
      }
      
      .checklist-items-table td.checkbox-na.checked {
        background-color: #d6d8db;
        color: #383d41;
        font-weight: 900;
      }
      
      /* Status row colors - lighter backgrounds */
      .checklist-items-table tbody tr.item-row.status-pass {
        background-color: #f8fff8;
      }
      
      .checklist-items-table tbody tr.item-row.status-fail {
        background-color: #fffafa;
      }
      
      .checklist-items-table tbody tr.item-row.status-na {
        background-color: #fafbfc;
      }
      
      /* Comments Section */
      .comments-section {
        border-bottom: 2px solid #000;
        padding: 10px;
        min-height: 60px;
      }
      
      .comments-header {
        font-weight: bold;
        font-size: 11px;
        margin-bottom: 6px;
        text-transform: uppercase;
      }
      
      .comments-content {
        font-size: 10px;
        line-height: 1.5;
        min-height: 40px;
      }
      
      /* Signature Section */
      .signature-section {
        border-bottom: none;
      }
      
      .signature-table {
        width: 100%;
        border-collapse: collapse;
      }
      
      .signature-table td.signature-block {
        width: 50%;
        padding: 12px;
        vertical-align: top;
        border-right: 2px solid #000;
      }
      
      .signature-table td.signature-block:last-child {
        border-right: none;
      }
      
      .signature-title {
        font-weight: bold;
        font-size: 11px;
        margin-bottom: 10px;
        text-transform: uppercase;
        padding-bottom: 5px;
        border-bottom: 1px solid #333;
      }
      
      .signature-row {
        display: flex;
        align-items: center;
        margin-bottom: 8px;
        gap: 8px;
      }
      
      .signature-label {
        font-size: 10px;
        font-weight: normal;
        min-width: 60px;
      }
      
      .signature-line {
        flex: 1;
        border-bottom: 1px solid #000;
        min-height: 18px;
        font-size: 10px;
        padding: 2px 4px;
      }
      
      .signature-value {
        flex: 1;
        font-size: 10px;
        padding: 2px 4px;
      }
      
      .signature-box {
        flex: 1;
        height: 35px;
        border: 1px solid #000;
      }
      
      /* Print Styles */
      @media print {
        * {
          -webkit-print-color-adjust: exact !important;
          print-color-adjust: exact !important;
          color-adjust: exact !important;
        }
        
        body { 
          padding: 0; 
          margin: 0;
        }
        
        .inspection-form-container { 
          border: 3px solid #000;
          max-width: 100%;
          margin: 0;
          page-break-inside: avoid;
        }
        
        .watermark {
          opacity: 0.12 !important;
        }
        
        .watermark svg {
          filter: grayscale(40%) !important;
        }
        
        .page-break {
          page-break-before: always;
          break-before: always;
        }
        
        .checklist-sections-form {
          page-break-inside: auto;
        }
        
        .section-header-row {
          page-break-after: avoid;
        }
        
        .item-row {
          page-break-inside: avoid;
        }
        
        .signature-section {
          page-break-inside: avoid;
        }
        
        /* Ensure colors print */
        .checkbox-y.checked {
          background-color: #d4edda !important;
          color: #155724 !important;
        }
        
        .checkbox-n.checked {
          background-color: #f8d7da !important;
          color: #721c24 !important;
        }
        
        .checkbox-na.checked {
          background-color: #e2e3e5 !important;
          color: #383d41 !important;
        }
        
        .status-pass {
          background-color: #f0fff0 !important;
        }
        
        .status-fail {
          background-color: #fff5f5 !important;
        }
        
        .status-na {
          background-color: #f8f9fa !important;
        }
        
        @page {
          size: A4;
          margin: 1.5cm 1cm;
        }
      }
    `;
  }
}

