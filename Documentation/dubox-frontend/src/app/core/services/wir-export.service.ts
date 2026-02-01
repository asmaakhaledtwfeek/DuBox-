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
    
    // Convert image URLs to base64 before generating content (including application logo)
    const logoData = await this.loadLogosAsBase64(project);
    const content = this.generateWIRPrintContent(checkpoint, box, projectInfo, project, logoData);
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
      // Wait for content to render and images to load
      await new Promise(resolve => setTimeout(resolve, 500));
      
      // Wait for all images to load (with longer timeout for SVG)
      await this.waitForImagesToLoad(contentDiv);
      
      // Additional wait to ensure SVG images are fully rendered
      await new Promise(resolve => setTimeout(resolve, 200));

      // Check if content div has actual content
      if (!contentDiv || !contentDiv.innerHTML || contentDiv.innerHTML.trim().length === 0) {
        console.warn('No content to generate PDF');
        alert('No content available to export. Please ensure the checkpoint has checklist items.');
        return;
      }

      // Configure html2pdf options
      const opt = {
        margin: [5, 5, 5, 5] as [number, number, number, number],
        filename: `${fileName}.pdf`,
        image: { type: 'jpeg' as const, quality: 0.95 },
        html2canvas: { 
          scale: 2,
          useCORS: true,
          logging: false,
          backgroundColor: '#ffffff',
          windowWidth: 794,
          windowHeight: 1123,
          removeContainer: true,
        },
        jsPDF: { 
          unit: 'mm', 
          format: 'a4', 
          orientation: 'portrait' as const,
          compress: true,
          hotfixes: ['px_scaling']
        },
        pagebreak: { 
          mode: ['avoid-all', 'css', 'legacy'],
          before: '.page-break',
          avoid: ['.signature-section', '.item-row', '.section-header-row']
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
  async exportWIRToPrintDialog(checkpoint: WIRCheckpoint, box?: Box | null, project?: Project | null): Promise<void> {
    const projectInfo = this.buildProjectInfo(checkpoint, box, project);
    
    // Convert image URLs to base64 before generating content
    const logoData = await this.loadLogosAsBase64(project);
    const content = this.generateWIRPrintContent(checkpoint, box, projectInfo, project, logoData);
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
  async downloadWIRAsHTML(checkpoint: WIRCheckpoint, box?: Box | null, project?: Project | null): Promise<void> {
    const projectInfo = this.buildProjectInfo(checkpoint, box, project);
    
    // Convert image URLs to base64 before generating content
    const logoData = await this.loadLogosAsBase64(project);
    const content = this.generateWIRPrintContent(checkpoint, box, projectInfo, project, logoData);
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
  generateWIRPrintContent(checkpoint: WIRCheckpoint, box?: Box | null, projectInfo?: ProjectInfo, project?: Project | null, logoData?: { contractor?: string; client?: string; application?: string }): string {
    const items = checkpoint.checklistItems || [];
    const groupedChecklists = this.groupItemsByChecklist(items);
    const referenceDocuments = this.extractReferenceDocuments(items);
  
    // Filter out checklists that have no items FIRST
    const checklistsWithItems = groupedChecklists.filter(checklist => {
      return checklist.sections.some(section => section.items.length > 0);
    });
  
    // Generate pages ONLY for checklists with items
    let html = '';
    let validPageIndex = 0;
    checklistsWithItems.forEach((checklist) => {
      const pageContent = this.generateChecklistPage(
        checkpoint, 
        checklist, 
        projectInfo!, 
        box, 
        referenceDocuments,
        validPageIndex > 0, // Add page break for subsequent pages (only if we have valid pages)
        project,
        logoData
      );
      
      // Double-check the page has content before adding
      if (pageContent && pageContent.trim().length > 0) {
        html += pageContent;
        validPageIndex++; // Only increment if we actually added a page
      }
    });
  
    return html;
  }

  /**
   * Build project info from available data
   */
  private buildProjectInfo(checkpoint: WIRCheckpoint, box?: Box | null, project?: Project | null): ProjectInfo {
    return {
      projectName: checkpoint?.projectName || 'AMANA STAFF VILLAGE ZONE-05',
      projectCode: checkpoint.projectCode || project?.code || '',
      clientName: checkpoint.client || project?.clientName || 'AMAALA',
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
    addPageBreak: boolean = false,
    project?: Project | null,
    logoData?: { contractor?: string; client?: string; application?: string }
  ): string {
    // Filter out sections that have no items
    const sectionsWithItems = checklist.sections.filter(section => section.items.length > 0);
    
    // If no sections have items, return empty string to avoid empty page
    if (sectionsWithItems.length === 0) {
      return '';
    }
    
    // Count total items to ensure we have content
    const totalItems = sectionsWithItems.reduce((sum, section) => sum + section.items.length, 0);
    if (totalItems === 0) {
      return '';
    }
    
    // Only add page break if there's actual content AND it's not the first page
    // Add page-break class directly to container to avoid empty pages
    const pageBreakClass = addPageBreak ? ' page-break-before' : '';
   
   let html = `
     <div class="inspection-form-container${pageBreakClass}">
      <!-- DuBox Logo Watermark -->
      <div class="watermark">
        ${this.getDuBoxLogoSVG()}
      </div>

      <!-- Form Header with Border -->
      <div class="form-header-border">
        <!-- Company Logos Row -->
        <div class="company-logos-row">
          ${this.generateContractorLogoSection(project, logoData)}
          ${this.generateClientLogoSection(project, logoData)}
          ${this.generateApplicationAndContractorLogoSection(project, logoData)}
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
            <td class="value" colspan="3">: ${projectInfo.projectName}${projectInfo.projectCode ? ` (${projectInfo.projectCode})` : ''}</td>
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
  
  sectionsWithItems.forEach((section, sectionIndex) => {
    // Add section header row
    // colspan="5" matches: number, description, Y, N, remarks
    html += `
            <tr class="section-header-row">
              <td colspan="5" class="section-header-cell">
                <strong>${this.escapeHtml(section.sectionName)}</strong>
              </td>
            </tr>`;
    
    // Add items under this section
    section.items.forEach((item, itemIndex) => {
      const isPassed = item.status === 'Pass';
      const isFailed = item.status === 'Fail';
      const statusClass = isPassed ? 'status-pass' : isFailed ? 'status-fail' : 'status-pending';
      
      html += `
            <tr class="item-row ${statusClass}">
              <td class="col-number">${itemCounter}</td>
              <td class="col-description">${this.escapeHtml(item.checkpointDescription || '')}</td>
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
        <div class="comments-content">${checkpoint.comments ? this.escapeHtml(checkpoint.comments) : ''}</div>
      </div>

      <!-- Signature Section -->
      <div class="signature-section">
        <table class="signature-table">
          <tr>
            <td class="signature-block">
              <div class="signature-title">CIVIL ENGINEER</div>
              <div class="signature-row">
                <span class="signature-label">Name</span>
                <div class="signature-line">${checkpoint.requestedBy ? this.escapeHtml(checkpoint.requestedBy) : ''}</div>
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
                <div class="signature-line">${checkpoint.inspectorName ? this.escapeHtml(checkpoint.inspectorName) : '0'}</div>
              </div>
              <div class="signature-row">
                <span class="signature-label">Position:</span>
                <div class="signature-value">${checkpoint.inspectorRole ? this.escapeHtml(checkpoint.inspectorRole) : 'QC Engineer'}</div>
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
   * Generate Contractor Logo Section (Section 1 - AMAALA)
   */
  private generateContractorLogoSection(project?: Project | null, logoData?: { contractor?: string; client?: string }): string {
    // Prefer base64 data if available, otherwise use URL
    const logoSource = logoData?.contractor || project?.contractorImageUrl;
    
    if (logoSource && logoSource.trim() !== '') {
      return `
          <div class="logo-box logo-amaala">
            <div class="logo-container">
              <img src="${this.escapeHtml(logoSource)}" alt="Contractor Logo" class="logo-image" style="object-fit: contain; width: 100%; height: 100%; display: block; background: transparent;" onerror="this.style.display='none';" />
            </div>
            <span class="logo-text" style="display: none;">AMAALA</span>
          </div>`;
    }
    
    // Fallback to original text
    return `
          <div class="logo-box logo-amaala">
            <span class="logo-text">AMAALA</span>
          </div>`;
  }

  /**
   * Generate Client Logo Section (Section 2 - PARSONS)
   */
  private generateClientLogoSection(project?: Project | null, logoData?: { contractor?: string; client?: string }): string {
    // Prefer base64 data if available, otherwise use URL
    const logoSource = logoData?.client || project?.clientImageUrl;
    
    if (logoSource && logoSource.trim() !== '') {
      return `
          <div class="logo-box logo-parsons">
            <div class="logo-container">
              <img src="${this.escapeHtml(logoSource)}" alt="Client Logo" class="logo-image" style="object-fit: contain; width: 100%; height: 100%; display: block; background: transparent;" onerror="this.style.display='none';" />
            </div>
            <span class="logo-text" style="display: none;">PARSONS</span>
          </div>`;
    }
    
    // Fallback to original text
    return `
          <div class="logo-box logo-parsons">
            <span class="logo-text">PARSONS</span>
          </div>`;
  }

  /**
   * Generate Application and Contractor Logo Section (Section 3 - AMANA)
   * Contains: Application Logo (DuBox) and Contractor Logo
   */
  private generateApplicationAndContractorLogoSection(project?: Project | null, logoData?: { contractor?: string; client?: string; application?: string }): string {
    // Prefer base64 data if available, otherwise use URL
    const contractorLogoSource = logoData?.contractor || project?.contractorImageUrl;
    const hasContractorLogo = contractorLogoSource && contractorLogoSource.trim() !== '';
    
    // Application Logo (DuBox logo) - use inline SVG for better PDF rendering
    const applicationLogo = `<div style="width: 100%; height: 100%; display: flex; align-items: center; justify-content: center; background: transparent;">${this.getDuBoxLogoSVG()}</div>`;
    
    // Contractor Logo (if available) - with error handling fallback
    const contractorLogo = hasContractorLogo 
      ? `<img src="${this.escapeHtml(contractorLogoSource)}" alt="Contractor Logo" class="logo-image logo-contractor" style="object-fit: contain; width: 100%; height: 100%; display: block; background: transparent;" onerror="this.style.display='none';" />`
      : '';
    
    // If we have both logos, display them stacked vertically
    if (hasContractorLogo) {
      return `
          <div class="logo-box logo-amana logo-dual">
            <div class="logo-app-container">
              ${applicationLogo}
            </div>
            <div class="logo-contractor-container">
              ${contractorLogo}
            </div>
          </div>`;
    }
    
    // If only application logo, show it with fallback text
    return `
          <div class="logo-box logo-amana">
            <div class="logo-app-container">
              ${applicationLogo}
            </div>
            <span class="logo-text logo-fallback">AMANA</span>
          </div>`;
  }

  /**
   * Load project logos as base64 for PDF generation
   * This ensures images are embedded in the PDF and don't rely on external URLs
   */
  private async loadLogosAsBase64(project?: Project | null): Promise<{ contractor?: string; client?: string; application?: string }> {
    const logoData: { contractor?: string; client?: string; application?: string } = {};
    
    // Load application logo (DuBox logo from assets)
    try {
      const appLogoUrl = this.getApplicationLogoUrl();
      console.log('Loading application logo from:', appLogoUrl);
      logoData.application = await this.imageUrlToBase64(appLogoUrl);
      console.log('Application logo loaded successfully, length:', logoData.application?.length);
    } catch (error) {
      console.warn('Failed to load application logo:', error);
      // Fallback to SVG if image fails (undefined means use SVG fallback)
      logoData.application = undefined;
    }
    
    if (!project) {
      return logoData;
    }
    
    // Load contractor logo
    if (project.contractorImageUrl && project.contractorImageUrl.trim() !== '') {
      try {
        logoData.contractor = await this.imageUrlToBase64(project.contractorImageUrl);
      } catch (error) {
        console.warn('Failed to load contractor logo:', error);
        // Keep original URL as fallback
        logoData.contractor = project.contractorImageUrl;
      }
    }
    
    // Load client logo
    if (project.clientImageUrl && project.clientImageUrl.trim() !== '') {
      try {
        logoData.client = await this.imageUrlToBase64(project.clientImageUrl);
      } catch (error) {
        console.warn('Failed to load client logo:', error);
        // Keep original URL as fallback
        logoData.client = project.clientImageUrl;
      }
    }
    
    return logoData;
  }

  /**
   * Get the application logo URL (DuBox logo from assets)
   */
  private getApplicationLogoUrl(): string {
    // Convert relative assets path to absolute URL
    // Use the same path as in the header component
    const baseUrl = window.location.origin;
    const baseHref = document.querySelector('base')?.getAttribute('href') || '/';
    return `${baseUrl}${baseHref}assets/images/dubox-logo-dark.svg`;
  }

  /**
   * Convert image URL to base64 data URL
   */
  private async imageUrlToBase64(url: string): Promise<string> {
    // If already a data URL, return as is
    if (url.startsWith('data:')) {
      return url;
    }
    
    try {
      console.log('Fetching image from URL:', url);
      const response = await fetch(url, { 
        mode: 'cors',
        credentials: 'include', // Include cookies for authenticated requests
        cache: 'no-cache' // Ensure fresh fetch
      });
      
      if (!response.ok) {
        throw new Error(`Failed to fetch image: ${response.status} ${response.statusText}`);
      }
      
      const blob = await response.blob();
      console.log('Image blob received, type:', blob.type, 'size:', blob.size);
      
      return new Promise<string>((resolve, reject) => {
        const reader = new FileReader();
        reader.onloadend = () => {
          const base64String = reader.result as string;
          console.log('Image converted to base64, length:', base64String.length);
          resolve(base64String);
        };
        reader.onerror = (error) => {
          console.error('FileReader error:', error);
          reject(error);
        };
        reader.readAsDataURL(blob);
      });
    } catch (error) {
      console.error('Error converting image to base64:', error);
      // Return original URL as fallback
      return url;
    }
  }

  /**
   * Wait for all images in the container to load
   */
  private async waitForImagesToLoad(container: HTMLElement): Promise<void> {
    const images = container.querySelectorAll('img');
    if (images.length === 0) {
      return;
    }
    
    const imagePromises = Array.from(images).map((img) => {
      return new Promise<void>((resolve) => {
        if (img.complete && img.naturalHeight !== 0) {
          // Image already loaded
          resolve();
        } else {
          // Wait for image to load or fail
          const onLoad = () => {
            img.removeEventListener('load', onLoad);
            img.removeEventListener('error', onError);
            resolve();
          };
          const onError = () => {
            img.removeEventListener('load', onLoad);
            img.removeEventListener('error', onError);
            // Still resolve even if image fails (fallback will show)
            resolve();
          };
          img.addEventListener('load', onLoad);
          img.addEventListener('error', onError);
          
          // Timeout after 5 seconds
          setTimeout(() => {
            img.removeEventListener('load', onLoad);
            img.removeEventListener('error', onError);
            resolve();
          }, 5000);
        }
      });
    });
    
    await Promise.all(imagePromises);
  }

  /**
   * Get DuBox logo SVG matching the header exactly
   * Optimized for PDF rendering with proper viewBox and no hardcoded dimensions
   */
  private getDuBoxLogoSVG(): string {
    return `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 180 50" width="100%" height="100%" preserveAspectRatio="xMidYMid meet" style="object-fit: contain; width: 100%; height: 100%; display: block; background: transparent;">
      <defs>
        <style>
          .logo-yellow{fill:#ffcb1b;}
          .logo-dark{fill:#2d3748;}
          .logo-gray{fill:#718096;}
        </style>
      </defs>
      <!-- Yellow Box Icon (left side) -->
      <rect class="logo-yellow" x="0" y="5" width="15" height="30" rx="1"/>
      <!-- DUBOX Text -->
      <text x="20" y="25" font-family="Arial, sans-serif" font-size="24" font-weight="700" class="logo-dark">DUBOX</text>
      <!-- AN AMANA. COMPANY Text (subtitle) -->
      <text x="20" y="40" font-family="Arial, sans-serif" font-size="8" font-weight="400" class="logo-gray" letter-spacing="0.5">AN AMANA. COMPANY</text>
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
        font-family: 'QA', sans-serif; 
        background: white; 
        padding: 0;
        margin: 0;
      }
      
      .page-break {
        page-break-before: always;
        break-before: always;
      }
      
      .page-break-before {
        page-break-before: always;
        break-before: page;
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
        min-height: 60px;
        max-height: 60px;
      }
      
      .logo-box {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        padding: 5px;
        min-height: 60px;
        max-height: 60px;
        border-right: 2px solid #000;
        background: transparent;
        position: relative;
        overflow: hidden;
      }
      
      .logo-box:last-child {
        border-right: none;
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
      
      /* Logo Container - Fixed height wrapper */
      .logo-container {
        height: 40px;
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
        background: transparent;
      }
      
      /* Logo Image Styling */
      .logo-image {
        object-fit: contain;
        width: 100%;
        height: 100%;
        display: block;
      }
      
      /* Dual Logo Container (Section 3) - Vertical Stack */
      .logo-box.logo-dual {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        padding: 5px;
        gap: 4px;
        background: transparent;
      }
      
      /* Application Logo Container (Top in Section 3) */
      .logo-app-container {
        height: 28px;
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
        background: transparent;
      }
      
      .logo-app-container svg {
        object-fit: contain;
        width: 100%;
        height: 100%;
        display: block;
      }
      
      .logo-app-container .logo-app {
        object-fit: contain;
        width: 100%;
        height: 100%;
        display: block;
      }
      
      .logo-app-container img.logo-app {
        object-fit: contain;
        width: 100%;
        height: 100%;
        display: block;
      }
      
      .logo-app-container div {
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
        background: transparent;
      }
      
      /* Contractor Logo Container (Bottom in Section 3) */
      .logo-contractor-container {
        height: 22px;
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
        background: transparent;
      }
      
      .logo-contractor-container .logo-image,
      .logo-contractor {
        object-fit: contain;
        width: 100%;
        height: 100%;
      }
      
      .logo-fallback {
        position: absolute;
        bottom: 6px;
        font-size: 11px;
        opacity: 0.6;
        z-index: 1;
      }
      
      /* Single logo box (non-dual) styling */
      .logo-box:not(.logo-dual) .logo-container {
        height: 40px;
      }
      
      .logo-box:not(.logo-dual) .logo-image {
        object-fit: contain;
        width: 100%;
        height: 100%;
      }
      
      /* Single logo box (non-dual) application logo */
      .logo-box:not(.logo-dual) .logo-app-container {
        height: 40px;
        background: transparent;
      }
      
      .logo-box:not(.logo-dual) .logo-app-container img.logo-app,
      .logo-box:not(.logo-dual) .logo-app-container svg {
        object-fit: contain;
        width: 100%;
        height: 100%;
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
        margin: 0;
        padding: 0;
      }
      
      .activity-table {
        width: 100%;
        border-collapse: collapse;
        margin: 0;
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
        width: 5%;
        border-left: 1px solid #000;
      }
      
       .activity-table td.activity-value {
         width: 40%;
       }
      
      .activity-table td.yn-header-cell {
        width: 8%;
        text-align: center;
        font-weight: bold;
        font-size: 10px;
        background: #f0f0f0;
        line-height: 1.3;
        padding: 8px 4px;
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
        padding: 8px 4px;
      }
      
      /* Checklist Items Table */
      .checklist-sections-form {
        border-bottom: 2px solid #000;
        margin: 0;
        padding: 0;
        margin-top: -1px;
      }
      
      .checklist-items-table {
        width: 100%;
        border-collapse: collapse;
        margin: 0;
      }
      
      .checklist-items-table tbody tr.section-header-row {
        background: #e8e8e8;
        border-top: 1px solid #000;
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
        border-top: 1px solid #000;
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
        padding: 6px 4px;
      }
      
      .checklist-items-table td.col-description {
        width: 45%;
        line-height: 1.4;
        text-align: left;
        padding: 6px 8px;
      }
      
      .checklist-items-table td.col-checkbox {
        width: 8%;
        text-align: center;
        font-weight: bold;
        font-size: 16px;
        min-height: 24px;
        line-height: 24px;
        padding: 8px 4px;
        vertical-align: middle;
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
        padding: 6px 10px;
        min-height: 40px;
      }
      
      .comments-header {
        font-weight: bold;
        font-size: 10px;
        margin-bottom: 4px;
        text-transform: uppercase;
      }
      
      .comments-content {
        font-size: 9px;
        line-height: 1.3;
        min-height: 25px;
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
        padding: 8px;
        vertical-align: top;
        border-right: 2px solid #000;
      }
      
      .signature-table td.signature-block:last-child {
        border-right: none;
      }
      
      .signature-title {
        font-weight: bold;
        font-size: 10px;
        margin-bottom: 6px;
        text-transform: uppercase;
        padding-bottom: 3px;
        border-bottom: 1px solid #333;
      }
      
      .signature-row {
        display: flex;
        align-items: center;
        margin-bottom: 5px;
        gap: 6px;
      }
      
      .signature-label {
        font-size: 9px;
        font-weight: normal;
        min-width: 50px;
      }
      
      .signature-line {
        flex: 1;
        border-bottom: 1px solid #000;
        min-height: 14px;
        font-size: 9px;
        padding: 1px 3px;
      }
      
      .signature-value {
        flex: 1;
        font-size: 9px;
        padding: 1px 3px;
      }
      
      .signature-box {
        flex: 1;
        height: 25px;
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

