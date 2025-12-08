import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { PermissionService } from '../../../core/services/permission.service';
import { ReportsService } from '../../../core/services/reports.service';
import {
  ProjectsSummaryReportResponse,
  ProjectsSummaryReportQueryParams,
  ProjectSummaryItem
} from '../../../core/models/projects-summary-report.model';
import { formatProgress } from '../../../core/utils/progress.util';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-projects-summary-report',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './projects-summary-report.component.html',
  styleUrls: ['./projects-summary-report.component.scss']
})
export class ProjectsSummaryReportComponent implements OnInit {
  reportData: ProjectsSummaryReportResponse | null = null;
  loading = false;
  error = '';
  canExport = false;

  // Filters
  filtersCollapsed = false;
  isActiveFilter: boolean | null = null;
  selectedStatuses: number[] = [];
  searchTerm = '';

  // Pagination
  currentPage = 1;
  pageSize = 25;
  totalCount = 0;
  totalPages = 0;

  // Available status options
  statusOptions = [
    { value: 1, label: 'Active' },
    { value: 2, label: 'OnHold' },
    { value: 3, label: 'Completed' },
    { value: 4, label: 'Archived' }
  ];

  readonly formatProgress = formatProgress;
  readonly Math = Math;
  readonly Object = Object;

  constructor(
    private reportsService: ReportsService,
    private permissionService: PermissionService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.canExport = this.permissionService.hasPermission('reports', 'export');
    this.loadReportData();
  }

  loadReportData(): void {
    this.loading = true;
    this.error = '';

    const params: ProjectsSummaryReportQueryParams = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    };
    
    if (this.isActiveFilter !== null) {
      params.isActive = this.isActiveFilter;
    }
    
    if (this.selectedStatuses.length > 0) {
      params.status = this.selectedStatuses;
    }
    
    if (this.searchTerm.trim()) {
      params.search = this.searchTerm.trim();
    }

    this.reportsService.getProjectsSummaryReport(params).subscribe({
      next: (data) => {
        this.reportData = data;
        this.totalCount = data.totalCount;
        this.totalPages = data.totalPages;
        // Reset to page 1 if current page is out of bounds
        if (this.currentPage > this.totalPages && this.totalPages > 0) {
          this.currentPage = 1;
          // Reload with page 1
          this.loadReportData();
          return;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load projects summary report:', err);
        this.error = 'Failed to load report. Please try again.';
        this.loading = false;
      }
    });
  }

  get paginatedProjects(): ProjectSummaryItem[] {
    if (!this.reportData || !this.reportData.items) {
      return [];
    }
    // Server-side pagination - items are already paginated
    return this.reportData.items;
  }

  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      // Scroll to top of table
      const tableElement = document.querySelector('.table-section');
      if (tableElement) {
        tableElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    }
  }

  onPageSizeChange(): void {
    this.pageSize = Number(this.pageSize);
    this.currentPage = 1;
    this.totalPages = Math.ceil(this.totalCount / this.pageSize);
  }

  firstPage(): void {
    if (this.currentPage > 1) {
      this.onPageChange(1);
    }
  }

  lastPage(): void {
    if (this.currentPage < this.totalPages) {
      this.onPageChange(this.totalPages);
    }
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisible = 5;
    let start = Math.max(1, this.currentPage - Math.floor(maxVisible / 2));
    let end = Math.min(this.totalPages, start + maxVisible - 1);
    
    if (end - start < maxVisible - 1) {
      start = Math.max(1, end - maxVisible + 1);
    }
    
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }

  toggleStatus(status: number): void {
    const index = this.selectedStatuses.indexOf(status);
    if (index >= 0) {
      this.selectedStatuses.splice(index, 1);
    } else {
      this.selectedStatuses.push(status);
    }
  }

  applyFilters(): void {
    this.currentPage = 1;
    this.loadReportData();
  }

  resetFilters(): void {
    this.isActiveFilter = null;
    this.selectedStatuses = [];
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadReportData();
  }

  exportToExcel(): void {
    if (!this.reportData || !this.reportData.items || this.reportData.items.length === 0) {
      alert('No data to export');
      return;
    }

    const exportData = this.reportData.items.map(project => ({
      'Project Code': project.projectCode,
      'Project Name': project.projectName,
      'Client Name': project.clientName || '',
      'Location': project.location || '',
      'Status': project.status,
      'Total Boxes': project.totalBoxes,
      'Progress %': project.progressPercentageFormatted
    }));

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Projects Summary': worksheet },
      SheetNames: ['Projects Summary']
    };

    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    XLSX.writeFile(workbook, `Projects_Summary_Report_${dateStr}.xlsx`);
  }

  exportToCSV(): void {
    if (!this.reportData || !this.reportData.items || this.reportData.items.length === 0) {
      alert('No data to export');
      return;
    }

    const headers = ['Project Code', 'Project Name', 'Client Name', 'Location', 'Status', 'Total Boxes', 'Progress %'];
    const rows = this.reportData.items.map(project => [
      project.projectCode,
      project.projectName,
      project.clientName || '',
      project.location || '',
      project.status,
      project.totalBoxes,
      project.progressPercentageFormatted
    ]);

    const csvContent = [
      headers.join(','),
      ...rows.map(row => row.map(cell => `"${cell}"`).join(','))
    ].join('\n');

    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    
    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    link.setAttribute('download', `Projects_Summary_Report_${dateStr}.csv`);
    
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  getStatusColor(status: string): string {
    switch (status.toLowerCase()) {
      case 'active': return 'green';
      case 'completed': return 'blue';
      case 'onhold': return 'orange';
      case 'archived': return 'gray';
      case 'closed': return 'red';
      default: return 'gray';
    }
  }

  viewProject(projectId: string): void {
    if (projectId) {
      this.router.navigate(['/projects', projectId, 'dashboard']);
    }
  }
}

