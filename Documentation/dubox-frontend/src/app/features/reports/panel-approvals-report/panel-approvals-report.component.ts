import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { BoxService } from '../../../core/services/box.service';
import { ProjectService } from '../../../core/services/project.service';
import { ReportsService } from '../../../core/services/reports.service';
import { FactoryService, Factory } from '../../../core/services/factory.service';
import { Box, BoxPanel } from '../../../core/models/box.model';
import { Project } from '../../../core/models/project.model';
import * as XLSX from 'xlsx';

interface PanelApprovalReport {
  panelId: string;  // Will use boxPanelId
  panelName: string;
  panelType: string;
  boxCode: string;
  boxName: string;
  projectName: string;
  projectNumber: string;  // Will use project.code
  firstApprovalDate?: Date;
  secondApprovalDate?: Date;
  secondApprovalBy?: string;
  status: string;
}

interface PanelApprovalReportSummary {
  totalPanels: number;
  firstApprovalCount: number;
  secondApprovalCount: number;
  pendingFirstApproval: number;
  pendingSecondApproval: number;
  totalBoxes: number;
  totalPanelTypes: number;
  firstApprovalPercentage: number;
  secondApprovalPercentage: number;
}

interface PanelTypeGroupedReport {
  panelType: string;
  panelTypeId?: string;
  totalPanels: number;
  firstApprovalCount: number;
  secondApprovalCount: number;
  rejectedCount: number;
  pendingFirstApproval: number;
  pendingSecondApproval: number;
  boxesUsingThisType: number;
  firstApprovalPercentage: number;
  secondApprovalPercentage: number;
  projectNames: string[];
}

interface BoxApprovalReport {
  boxId: string;  // Will use box.id
  boxCode: string;
  boxName: string;
  projectName: string;
  projectNumber: string;  // Will use project.code
  totalPanels: number;
  approvedPanels: number;
  approvalPercentage: number;
  allPanelsApproved: boolean;
}

interface BoxApprovalReportSummary {
  totalBoxes: number;
  fullyApprovedBoxes: number;
  partiallyApprovedBoxes: number;
  averageApprovalPercentage: number;
  totalPanels: number;
  fullyApprovedPercentage: number;
}

interface ProjectPanelTypeSummary {
  totalProjects: number;
  totalPanelTypes: number;
  totalPanels: number;
  totalApproved: number;
  totalPending: number;
  averageApprovalPercentage: number;
}

interface ProjectPanelTypeReport {
  projectId: string;  // Will use project.id
  projectName: string;
  projectNumber: string;  // Will use project.code
  panelType: string;
  totalPanels: number;
  approvedPanels: number;
  pendingPanels: number;
  approvalPercentage: number;
}

@Component({
  selector: 'app-panel-approvals-report',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './panel-approvals-report.component.html',
  styleUrls: ['./panel-approvals-report.component.scss']
})
export class PanelApprovalsReportComponent implements OnInit {
  loading = false;
  error = '';
  
  // Active report type
  activeReportType: 'panels' | 'boxes' | 'project-panel-types' = 'panels';
  
  // Data
  projects: Project[] = [];
  factories: Factory[] = [];
  boxes: Box[] = [];
  
  // Filters
  selectedProjectId = '';
  selectedFactoryId = '';
  selectedPanelType = '';
  selectedApprovalStatus = 'all'; // all, first-approval, second-approval, rejected
  
  // Report data
  panelReports: PanelApprovalReport[] = [];
  filteredPanelReports: PanelApprovalReport[] = [];
  groupedPanelReports: PanelTypeGroupedReport[] = [];
  filteredGroupedPanelReports: PanelTypeGroupedReport[] = [];
  panelReportSummary: PanelApprovalReportSummary = {
    totalPanels: 0,
    firstApprovalCount: 0,
    secondApprovalCount: 0,
    pendingFirstApproval: 0,
    pendingSecondApproval: 0,
    totalBoxes: 0,
    totalPanelTypes: 0,
    firstApprovalPercentage: 0,
    secondApprovalPercentage: 0
  };
  
  boxReports: BoxApprovalReport[] = [];
  filteredBoxReports: BoxApprovalReport[] = [];
  boxReportSummary: BoxApprovalReportSummary = {
    totalBoxes: 0,
    fullyApprovedBoxes: 0,
    partiallyApprovedBoxes: 0,
    averageApprovalPercentage: 0,
    totalPanels: 0,
    fullyApprovedPercentage: 0
  };
  
  projectPanelTypeReports: ProjectPanelTypeReport[] = [];
  filteredProjectPanelTypeReports: ProjectPanelTypeReport[] = [];
  projectPanelTypeSummary: ProjectPanelTypeSummary = {
    totalProjects: 0,
    totalPanelTypes: 0,
    totalPanels: 0,
    totalApproved: 0,
    totalPending: 0,
    averageApprovalPercentage: 0
  };
  
  // Available panel types for filter
  availablePanelTypes: string[] = [];
  
  // Pagination
  currentPage = 1;
  pageSize = 25;
  
  // Expose Math to template
  readonly Math = Math;
  
  constructor(
    private boxService: BoxService,
    private projectService: ProjectService,
    private reportsService: ReportsService,
    private factoryService: FactoryService
  ) {}
  
  ngOnInit(): void {
    this.loadProjects();
    this.loadFactories();
  }
  
  loadProjects(): void {
    this.loading = true;
    this.projectService.getProjects().subscribe({
      next: (projects) => {
        this.projects = projects;
        this.loadAllData();
      },
      error: (err) => {
        console.error('Failed to load projects:', err);
        this.error = 'Failed to load projects';
        this.loading = false;
      }
    });
  }
  
  loadFactories(): void {
    this.factoryService.getAllFactories().subscribe({
      next: (factories) => {
        this.factories = factories;
        console.log('Loaded factories:', this.factories);
      },
      error: (err) => {
        console.error('Failed to load factories:', err);
        // Don't show error to user, just log it
      }
    });
  }
  
  loadAllData(): void {
    this.loading = true;
    this.error = '';
    
    // Call the Panel Approvals API
    const apiParams: any = {};
    if (this.selectedProjectId) {
      apiParams.projectId = this.selectedProjectId;
    }
    if (this.selectedFactoryId) {
      apiParams.factoryId = this.selectedFactoryId;
    }
    if (this.selectedPanelType) {
      apiParams.panelType = this.selectedPanelType;
    }
    if (this.selectedApprovalStatus && this.selectedApprovalStatus !== 'all') {
      apiParams.approvalStatus = this.selectedApprovalStatus;
    }
    
    this.reportsService.getPanelApprovalsReport(apiParams).subscribe({
      next: (response) => {
        // Map API response to component properties
        this.panelReports = response.panels || [];
        this.groupedPanelReports = response.groupedByPanelType || [];
        this.panelReportSummary = response.summary || {
          totalPanels: 0,
          firstApprovalCount: 0,
          secondApprovalCount: 0,
          pendingFirstApproval: 0,
          pendingSecondApproval: 0,
          totalBoxes: 0,
          totalPanelTypes: 0,
          firstApprovalPercentage: 0,
          secondApprovalPercentage: 0
        };
        
        // Extract panel types for dropdown
        const types = new Set<string>();
        this.panelReports.forEach(report => types.add(report.panelType));
        this.availablePanelTypes = Array.from(types).sort();
        
        // Load boxes for other report types
        this.loadBoxesForOtherReports();
      },
      error: (err) => {
        console.error('Failed to load panel approvals report:', err);
        this.error = 'Failed to load panel approvals report';
        this.loading = false;
      }
    });
  }
  
  loadBoxesForOtherReports(): void {
    // Load boxes for the other two report types (boxes and project-panel-types)
    if (this.selectedProjectId) {
      this.boxService.getBoxesByProject(this.selectedProjectId).subscribe({
        next: (boxes) => {
          this.boxes = this.applyFactoryFilterToBoxes(boxes);
          this.processOtherReports();
        },
        error: (err) => {
          console.error('Failed to load boxes:', err);
          this.processOtherReports(); // Continue with empty boxes
        }
      });
    } else {
      // For "All Projects", load all boxes from all projects
      const projectIds = this.projects.map(p => p.id);
      let allBoxes: Box[] = [];
      let loadedCount = 0;
      
      if (projectIds.length === 0) {
        this.processOtherReports();
        return;
      }
      
      projectIds.forEach(projectId => {
        this.boxService.getBoxesByProject(projectId).subscribe({
          next: (boxes) => {
            allBoxes = [...allBoxes, ...boxes];
            loadedCount++;
            
            if (loadedCount === projectIds.length) {
              this.boxes = this.applyFactoryFilterToBoxes(allBoxes);
              this.processOtherReports();
            }
          },
          error: (err) => {
            console.error(`Failed to load boxes for project ${projectId}:`, err);
            loadedCount++;
            
            if (loadedCount === projectIds.length) {
              this.boxes = this.applyFactoryFilterToBoxes(allBoxes);
              this.processOtherReports();
            }
          }
        });
      });
    }
  }
  
  applyFactoryFilterToBoxes(boxes: Box[]): Box[] {
    if (!this.selectedFactoryId) {
      return boxes;
    }
    console.log('Filtering boxes by factoryId:', this.selectedFactoryId);
    const filtered = boxes.filter(box => box.factoryId === this.selectedFactoryId);
    console.log(`Filtered ${filtered.length} boxes out of ${boxes.length} total`);
    return filtered;
  }
  
  processOtherReports(): void {
    // Generate box and project-panel-type reports from boxes data
    this.generateBoxReports();
    this.generateProjectPanelTypeReports();
    this.applyFilters();
    this.loading = false;
  }
  
  // Panel reports are now loaded from API in loadAllData()
  // generatePanelReports() and generateGroupedPanelReports() are no longer needed
  
  generateBoxReports(): void {
    this.boxReports = [];
    
    this.boxes.forEach(box => {
          const project = this.projects.find(p => p.id === box.projectId);
      
      if (box.boxPanels && box.boxPanels.length > 0) {
        const totalPanels = box.boxPanels.length;
        const approvedPanels = box.boxPanels.filter(p => 
          p.secondApprovalStatus?.toLowerCase() === 'approved'
        ).length;
        const approvalPercentage = totalPanels > 0 ? (approvedPanels / totalPanels) * 100 : 0;
        const allPanelsApproved = approvedPanels === totalPanels && totalPanels > 0;
        
        this.boxReports.push({
          boxId: box.id || '',
          boxCode: box.code || '',
          boxName: box.name || '',
          projectName: project?.name || 'Unknown Project',
          projectNumber: project?.code || '',
          totalPanels,
          approvedPanels,
          approvalPercentage,
          allPanelsApproved
        });
      }
    });
  }
  
  generateProjectPanelTypeReports(): void {
    this.projectPanelTypeReports = [];
    
    // Group by project and panel type
    const groupedData: Map<string, Map<string, BoxPanel[]>> = new Map();
    
    this.boxes.forEach(box => {
      if (box.boxPanels && box.boxPanels.length > 0) {
        box.boxPanels.forEach(panel => {
          const panelType = panel.typeName || panel.typeCode || 'Unknown';
          
          if (!groupedData.has(box.projectId)) {
            groupedData.set(box.projectId, new Map());
          }
          
          const projectGroup = groupedData.get(box.projectId)!;
          if (!projectGroup.has(panelType)) {
            projectGroup.set(panelType, []);
          }
          
          projectGroup.get(panelType)!.push(panel);
        });
      }
    });
    
    // Generate reports
    groupedData.forEach((panelTypeMap, projectId) => {
            const project = this.projects.find(p => p.id === projectId);
      
      panelTypeMap.forEach((panels, panelType) => {
        const totalPanels = panels.length;
        const approvedPanels = panels.filter(p => 
          p.secondApprovalStatus?.toLowerCase() === 'approved'
        ).length;
        const pendingPanels = totalPanels - approvedPanels;
        const approvalPercentage = totalPanels > 0 ? (approvedPanels / totalPanels) * 100 : 0;
        
        this.projectPanelTypeReports.push({
          projectId: projectId || '',
          projectName: project?.name || 'Unknown Project',
          projectNumber: project?.code || '',
          panelType,
          totalPanels,
          approvedPanels,
          pendingPanels,
          approvalPercentage
        });
      });
    });
    
    // Sort by project name and panel type
    this.projectPanelTypeReports.sort((a, b) => {
      const projectCompare = a.projectName.localeCompare(b.projectName);
      if (projectCompare !== 0) return projectCompare;
      return a.panelType.localeCompare(b.panelType);
    });
  }
  
  // extractPanelTypes() is now handled in loadAllData()
  
  switchReportType(type: 'panels' | 'boxes' | 'project-panel-types'): void {
    this.activeReportType = type;
    this.currentPage = 1;
    this.applyFilters();
  }
  
  onFilterChange(): void {
    this.currentPage = 1;
    if (this.selectedProjectId || this.selectedFactoryId) {
      this.loadAllData();
    } else {
      this.applyFilters();
    }
  }
  
  updatePanelReportSummary(): void {
    // Calculate summary from filtered grouped panel reports (not individual panels)
    // This ensures the summary matches what's displayed in the table
    const totalPanels = this.filteredGroupedPanelReports.reduce((sum, r) => sum + r.totalPanels, 0);
    const firstApprovalCount = this.filteredGroupedPanelReports.reduce((sum, r) => sum + r.firstApprovalCount, 0);
    const secondApprovalCount = this.filteredGroupedPanelReports.reduce((sum, r) => sum + r.secondApprovalCount, 0);
    const pendingFirstApproval = totalPanels - firstApprovalCount;
    const pendingSecondApproval = totalPanels - secondApprovalCount;
    const totalPanelTypes = this.filteredGroupedPanelReports.length;
    
    // Get unique boxes from filtered grouped data
    const totalBoxes = this.filteredGroupedPanelReports.reduce((sum, r) => sum + r.boxesUsingThisType, 0);
    
    const firstApprovalPercentage = totalPanels > 0 ? (firstApprovalCount / totalPanels) * 100 : 0;
    const secondApprovalPercentage = totalPanels > 0 ? (secondApprovalCount / totalPanels) * 100 : 0;
    
    this.panelReportSummary = {
      totalPanels,
      firstApprovalCount,
      secondApprovalCount,
      pendingFirstApproval,
      pendingSecondApproval,
      totalBoxes,
      totalPanelTypes,
      firstApprovalPercentage: Math.round(firstApprovalPercentage * 100) / 100,
      secondApprovalPercentage: Math.round(secondApprovalPercentage * 100) / 100
    };
  }
  
  applyFilters(): void {
    // Filter panel reports
    this.filteredPanelReports = this.panelReports.filter(report => {
      const matchesPanelType = !this.selectedPanelType || report.panelType === this.selectedPanelType;
      
      const matchesStatus = this.selectedApprovalStatus === 'all' ||
        (this.selectedApprovalStatus === 'first-approval' && report.firstApprovalDate) ||
        (this.selectedApprovalStatus === 'second-approval' && report.status === 'Approved') ||
        (this.selectedApprovalStatus === 'rejected' && report.status === 'Rejected');
      
      return matchesPanelType && matchesStatus;
    });
    
    // Filter grouped panel reports
    this.filteredGroupedPanelReports = this.groupedPanelReports.filter(report => {
      const matchesPanelType = !this.selectedPanelType || report.panelType === this.selectedPanelType;
      
      // For grouped reports:
      // - First Approval: Show only panel types where 100% of panels have first approval
      // - Second Approval: Show only panel types where 100% of panels have second approval
      // - Rejected: Show panel types that have at least one rejected panel
      const matchesStatus = this.selectedApprovalStatus === 'all' ||
        (this.selectedApprovalStatus === 'first-approval' && report.firstApprovalPercentage === 100) ||
        (this.selectedApprovalStatus === 'second-approval' && report.secondApprovalPercentage === 100) ||
        (this.selectedApprovalStatus === 'rejected' && report.rejectedCount > 0);
      
      return matchesPanelType && matchesStatus;
    });
    
    // Recalculate summary based on filtered data
    this.updatePanelReportSummary();
    
    // Filter box reports
    this.filteredBoxReports = this.boxReports.filter(report => {
      const matchesStatus = this.selectedApprovalStatus === 'all' ||
        (this.selectedApprovalStatus === 'first-approval' && report.totalPanels > 0) || // Has panels (first approval tracked per panel)
        (this.selectedApprovalStatus === 'second-approval' && report.allPanelsApproved) ||
        (this.selectedApprovalStatus === 'rejected' && report.totalPanels > 0); // Has panels
      
      return matchesStatus;
    });
    
    // Update box report summary
    this.updateBoxReportSummary();
    
    // Filter project panel type reports
    this.filteredProjectPanelTypeReports = this.projectPanelTypeReports.filter(report => {
      const matchesPanelType = !this.selectedPanelType || report.panelType === this.selectedPanelType;
      
      return matchesPanelType;
    });
    
    // Update project panel type summary
    this.updateProjectPanelTypeSummary();
  }
  
  updateBoxReportSummary(): void {
    const totalBoxes = this.filteredBoxReports.length;
    const fullyApprovedBoxes = this.filteredBoxReports.filter(r => r.allPanelsApproved).length;
    const partiallyApprovedBoxes = this.filteredBoxReports.filter(r => r.approvedPanels > 0 && !r.allPanelsApproved).length;
    const totalPanels = this.filteredBoxReports.reduce((sum, r) => sum + r.totalPanels, 0);
    const averageApprovalPercentage = totalBoxes > 0 
      ? this.filteredBoxReports.reduce((sum, r) => sum + r.approvalPercentage, 0) / totalBoxes 
      : 0;
    const fullyApprovedPercentage = totalBoxes > 0 ? (fullyApprovedBoxes / totalBoxes) * 100 : 0;
    
    this.boxReportSummary = {
      totalBoxes,
      fullyApprovedBoxes,
      partiallyApprovedBoxes,
      averageApprovalPercentage: Math.round(averageApprovalPercentage * 100) / 100,
      totalPanels,
      fullyApprovedPercentage: Math.round(fullyApprovedPercentage * 100) / 100
    };
  }
  
  updateProjectPanelTypeSummary(): void {
    const totalProjects = new Set(this.filteredProjectPanelTypeReports.map(r => r.projectId)).size;
    const totalPanelTypes = new Set(this.filteredProjectPanelTypeReports.map(r => r.panelType)).size;
    const totalPanels = this.filteredProjectPanelTypeReports.reduce((sum, r) => sum + r.totalPanels, 0);
    const totalApproved = this.filteredProjectPanelTypeReports.reduce((sum, r) => sum + r.approvedPanels, 0);
    const totalPending = this.filteredProjectPanelTypeReports.reduce((sum, r) => sum + r.pendingPanels, 0);
    const averageApprovalPercentage = totalPanels > 0 ? (totalApproved / totalPanels) * 100 : 0;
    
    this.projectPanelTypeSummary = {
      totalProjects,
      totalPanelTypes,
      totalPanels,
      totalApproved,
      totalPending,
      averageApprovalPercentage: Math.round(averageApprovalPercentage * 100) / 100
    };
  }
  
  get paginatedData(): any[] {
    let data: any[] = [];
    
    if (this.activeReportType === 'panels') {
      data = this.filteredGroupedPanelReports; // Use grouped data instead
    } else if (this.activeReportType === 'boxes') {
      data = this.filteredBoxReports;
    } else {
      data = this.filteredProjectPanelTypeReports;
    }
    
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return data.slice(startIndex, endIndex);
  }
  
  get totalPages(): number {
    let totalItems = 0;
    
    if (this.activeReportType === 'panels') {
      totalItems = this.filteredPanelReports.length;
    } else if (this.activeReportType === 'boxes') {
      totalItems = this.filteredBoxReports.length;
    } else {
      totalItems = this.filteredProjectPanelTypeReports.length;
    }
    
    return Math.ceil(totalItems / this.pageSize);
  }
  
  get totalItems(): number {
    if (this.activeReportType === 'panels') {
      return this.filteredGroupedPanelReports.length; // Use grouped data
    } else if (this.activeReportType === 'boxes') {
      return this.filteredBoxReports.length;
    } else {
      return this.filteredProjectPanelTypeReports.length;
    }
  }
  
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }
  
  exportToExcel(): void {
    let data: any[] = [];
    let filename = '';
    
    if (this.activeReportType === 'panels') {
      data = this.filteredPanelReports.map(report => ({
        'Panel Name': report.panelName,
        'Panel Type': report.panelType,
        'Box Code': report.boxCode,
        'Box Name': report.boxName,
        'Project': report.projectName,
        'Project Number': report.projectNumber,
        'First Approval Date': report.firstApprovalDate ? new Date(report.firstApprovalDate).toLocaleDateString() : '',
        'Second Approval Date': report.secondApprovalDate ? new Date(report.secondApprovalDate).toLocaleDateString() : '',
        'Approved By': report.secondApprovalBy || '',
        'Status': report.status
      }));
      filename = 'Panels_Second_Approval_Report';
    } else if (this.activeReportType === 'boxes') {
      data = this.filteredBoxReports.map(report => ({
        'Box Code': report.boxCode,
        'Box Name': report.boxName,
        'Project': report.projectName,
        'Project Number': report.projectNumber,
        'Total Panels': report.totalPanels,
        'Approved Panels': report.approvedPanels,
        'Approval %': report.approvalPercentage.toFixed(2) + '%',
        'All Approved': report.allPanelsApproved ? 'Yes' : 'No'
      }));
      filename = 'Boxes_Panel_Approval_Report';
    } else {
      data = this.filteredProjectPanelTypeReports.map(report => ({
        'Project': report.projectName,
        'Project Number': report.projectNumber,
        'Panel Type': report.panelType,
        'Total Panels': report.totalPanels,
        'Approved': report.approvedPanels,
        'Pending': report.pendingPanels,
        'Approval %': report.approvalPercentage.toFixed(2) + '%'
      }));
      filename = 'Project_Panel_Type_Approval_Report';
    }
    
    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Report');
    
    // Auto-size columns
    const maxWidth = 50;
    const colWidths = Object.keys(data[0] || {}).map(key => ({
      wch: Math.min(
        maxWidth,
        Math.max(
          key.length,
          ...data.map(row => String(row[key] || '').length)
        )
      )
    }));
    worksheet['!cols'] = colWidths;
    
    const timestamp = new Date().toISOString().split('T')[0];
    XLSX.writeFile(workbook, `${filename}_${timestamp}.xlsx`);
    
    // Show toast notification
    document.dispatchEvent(new CustomEvent('app-toast', {
      detail: {
        message: 'Report exported successfully!',
        type: 'success'
      }
    }));
  }
  
  clearFilters(): void {
    this.selectedProjectId = '';
    this.selectedFactoryId = '';
    this.selectedPanelType = '';
    this.selectedApprovalStatus = 'all';
    this.currentPage = 1;
    this.loadAllData();
  }
  
  formatDate(date?: Date): string {
    if (!date) return '-';
    return new Date(date).toLocaleDateString();
  }
  
  getStatusClass(status: string): string {
    return status === 'Approved' ? 'status-approved' : 'status-pending';
  }
  
  getApprovalStatusClass(percentage: number): string {
    if (percentage === 100) return 'status-approved';
    if (percentage >= 50) return 'status-partial';
    return 'status-pending';
  }
}

