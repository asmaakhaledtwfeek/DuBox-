import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ReportsService } from '../../../core/services/reports.service';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import { BoxStatus, getBoxStatusNumber } from '../../../core/models/box.model';
import { 
  PaginatedBoxSummaryReportResponse, 
  BoxSummaryReportItem,
  BoxSummaryReportQueryParams 
} from '../../../core/models/boxes-summary-report.model';
import { formatProgress } from '../../../core/utils/progress.util';
import * as XLSX from 'xlsx';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';

@Component({
  selector: 'app-boxes-summary-report',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './boxes-summary-report.component.html',
  styleUrls: ['./boxes-summary-report.component.scss']
})
export class BoxesSummaryReportComponent implements OnInit, OnDestroy {
  // Data
  reportData: PaginatedBoxSummaryReportResponse | null = null;
  selectedItems = new Set<string>();
  loading = false;
  error = '';

  // Filter state
  filtersCollapsed = false;
  boxTypeMultiselectOpen = false;
  projects: Project[] = [];
  selectedProjectId = '';
  selectedBoxTypes: string[] = [];
  availableBoxTypes: string[] = [];
  availableFloors: string[] = [];
  availableBuildings: string[] = [];
  availableZones: string[] = [];
  selectedFloor = '';
  selectedBuilding = '';
  selectedZone = '';
  selectedStatuses: number[] = [];
  progressMin = 0;
  progressMax = 100;
  searchTerm = '';
  dateFrom = '';
  dateTo = '';
  dateFilterType: 'LastUpdate' | 'PlannedStart' = 'LastUpdate';

  // Pagination
  currentPage = 1;
  pageSize = 25;
  totalCount = 0;
  totalPages = 0;

  // Sorting
  sortBy = 'boxtag';
  sortDir: 'asc' | 'desc' = 'asc';

  // Search debounce
  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  readonly BoxStatus = BoxStatus;
  readonly formatProgress = formatProgress;
  readonly getBoxStatusNumber = getBoxStatusNumber;
  readonly Math = Math; // Expose Math to template

  constructor(
    private reportsService: ReportsService,
    private projectService: ProjectService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadProjects();
    this.setupSearchDebounce();
    this.loadReportData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.searchSubject.complete();
  }

  @HostListener('document:click', ['$event'])
  handleDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.multiselect-container')) {
      this.boxTypeMultiselectOpen = false;
    }
  }

  private setupSearchDebounce(): void {
    this.searchSubject
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.currentPage = 1;
        this.loadReportData();
      });
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe({
      next: (projects) => {
        this.projects = projects;
      },
      error: (err) => {
        console.error('Failed to load projects:', err);
      }
    });
  }

  loadReportData(): void {
    this.loading = true;
    this.error = '';

    const params: BoxSummaryReportQueryParams = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      sortDir: this.sortDir,
      search: this.searchTerm || undefined,
      projectId: this.selectedProjectId || undefined,
      boxType: this.selectedBoxTypes.length > 0 ? this.selectedBoxTypes : undefined,
      floor: this.selectedFloor || undefined,
      building: this.selectedBuilding || undefined,
      zone: this.selectedZone || undefined,
      status: this.selectedStatuses.length > 0 ? this.selectedStatuses : undefined,
      progressMin: this.progressMin > 0 ? this.progressMin : undefined,
      progressMax: this.progressMax < 100 ? this.progressMax : undefined,
      dateFrom: this.dateFrom || undefined,
      dateTo: this.dateTo || undefined,
      dateFilterType: this.dateFilterType
    };

    this.reportsService.getBoxesSummaryReport(params).subscribe({
      next: (data) => {
        this.reportData = data;
        this.totalCount = data.totalCount;
        this.totalPages = data.totalPages;
        this.updateAvailableFilters(data.items);
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load boxes summary report:', err);
        this.error = err?.error?.message || err?.message || 'Failed to load boxes summary report';
        this.loading = false;
      }
    });
  }

  private updateAvailableFilters(items: BoxSummaryReportItem[]): void {
    const boxTypes = new Set<string>();
    const floors = new Set<string>();
    const buildings = new Set<string>();
    const zones = new Set<string>();

    items.forEach(item => {
      if (item.boxType) boxTypes.add(item.boxType);
      if (item.floor) floors.add(item.floor);
      if (item.building) buildings.add(item.building);
      if (item.zone) zones.add(item.zone);
    });

    this.availableBoxTypes = Array.from(boxTypes).sort();
    this.availableFloors = Array.from(floors).sort();
    this.availableBuildings = Array.from(buildings).sort();
    this.availableZones = Array.from(zones).sort();
  }

  onSearchChange(): void {
    this.searchSubject.next(this.searchTerm);
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.loadReportData();
  }

  onApplyFilters(): void {
    this.currentPage = 1;
    this.loadReportData();
  }

  onResetFilters(): void {
    this.selectedProjectId = '';
    this.selectedBoxTypes = [];
    this.selectedFloor = '';
    this.selectedBuilding = '';
    this.selectedZone = '';
    this.selectedStatuses = [];
    this.progressMin = 0;
    this.progressMax = 100;
    this.searchTerm = '';
    this.dateFrom = '';
    this.dateTo = '';
    this.dateFilterType = 'LastUpdate';
    this.currentPage = 1;
    this.sortBy = 'boxtag';
    this.sortDir = 'asc';
    this.loadReportData();
  }

  onSort(column: string): void {
    if (this.sortBy === column) {
      this.sortDir = this.sortDir === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = column;
      this.sortDir = 'asc';
    }
    this.loadReportData();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadReportData();
  }

  onPageSizeChange(): void {
    // Ensure pageSize is a number
    this.pageSize = Number(this.pageSize);
    this.currentPage = 1;
    this.loadReportData();
  }

  toggleBoxType(boxType: string): void {
    const index = this.selectedBoxTypes.indexOf(boxType);
    if (index >= 0) {
      this.selectedBoxTypes.splice(index, 1);
    } else {
      this.selectedBoxTypes.push(boxType);
    }
  }

  toggleBoxTypeMultiselect(): void {
    this.boxTypeMultiselectOpen = !this.boxTypeMultiselectOpen;
  }

  toggleStatus(status: BoxStatus): void {
    const statusNum = getBoxStatusNumber(status);
    const index = this.selectedStatuses.indexOf(statusNum);
    if (index >= 0) {
      this.selectedStatuses.splice(index, 1);
    } else {
      this.selectedStatuses.push(statusNum);
    }
  }

  toggleSelectAll(): void {
    if (this.selectedItems.size === this.reportData?.items.length) {
      this.selectedItems.clear();
    } else {
      this.selectedItems.clear();
      this.reportData?.items.forEach(item => this.selectedItems.add(item.boxId));
    }
  }

  toggleSelectItem(boxId: string): void {
    if (this.selectedItems.has(boxId)) {
      this.selectedItems.delete(boxId);
    } else {
      this.selectedItems.add(boxId);
    }
  }

  isSelected(boxId: string): boolean {
    return this.selectedItems.has(boxId);
  }

  getStatusBadgeClass(status: string): string {
    const statusLower = status.toLowerCase();
    if (statusLower.includes('completed')) return 'badge-completed';
    if (statusLower.includes('progress')) return 'badge-in-progress';
    if (statusLower.includes('notstarted')) return 'badge-not-started';
    if (statusLower.includes('hold')) return 'badge-on-hold';
    if (statusLower.includes('delayed')) return 'badge-delayed';
    return 'badge-default';
  }

  getProgressBarClass(progress: number): string {
    if (progress >= 75) return 'progress-high';
    if (progress >= 50) return 'progress-medium';
    if (progress >= 25) return 'progress-low';
    return 'progress-very-low';
  }

  exportToCSV(selectedOnly = false): void {
    const items = selectedOnly 
      ? this.reportData?.items.filter(item => this.selectedItems.has(item.boxId)) || []
      : this.reportData?.items || [];

    if (items.length === 0) {
      alert('No data to export');
      return;
    }

    const headers = [
      'Box Tag', 'Serial Number', 'Project', 'Box Type', 'Floor', 'Building', 'Zone',
      'Progress', 'Status', 'Non-Resolved Issues', 'Current Location', 'Activities', 'Assets'
    ];

    const rows = items.map(item => [
      item.boxTag,
      item.serialNumber || '',
      item.projectName,
      item.boxType,
      item.floor || '',
      item.building || '',
      item.zone || '',
      item.progressPercentageFormatted,
      item.status,
      item.qualityIssuesCount || 0,
      item.currentLocationName || '',
      item.activitiesCount,
      item.assetsCount
    ]);

    const csvContent = [
      headers.join(','),
      ...rows.map(row => row.map(cell => `"${cell}"`).join(','))
    ].join('\n');

    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', `boxes_summary_report_${new Date().toISOString().split('T')[0]}.csv`);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  exportToExcel(selectedOnly = false): void {
    const items = selectedOnly 
      ? this.reportData?.items.filter(item => this.selectedItems.has(item.boxId)) || []
      : this.reportData?.items || [];

    if (items.length === 0) {
      alert('No data to export');
      return;
    }

    const exportData = items.map(item => ({
      'Box Tag': item.boxTag,
      'Serial Number': item.serialNumber || '',
      'Project': item.projectName,
      'Box Type': item.boxType,
      'Floor': item.floor || '',
      'Building': item.building || '',
      'Zone': item.zone || '',
      'Progress': item.progressPercentageFormatted,
      'Status': item.status,
      'Non-Resolved Issues': item.qualityIssuesCount || 0,
      'Current Location': item.currentLocationName || '',
      'Activities': item.activitiesCount,
      'Assets': item.assetsCount
    }));

    const worksheet = XLSX.utils.json_to_sheet(exportData);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Boxes Summary');

    const dateStr = new Date().toISOString().split('T')[0];
    XLSX.writeFile(workbook, `boxes_summary_report_${dateStr}.xlsx`);
  }

  getStatusDistributionChartData(): any {
    if (!this.reportData?.aggregations.statusDistribution) return { labels: [], data: [] };
    
    const distribution = this.reportData.aggregations.statusDistribution;
    return {
      labels: Object.keys(distribution),
      data: Object.values(distribution)
    };
  }

  getProgressRangeChartData(): any {
    if (!this.reportData?.aggregations.progressRangeDistribution) return { labels: [], data: [] };
    
    const distribution = this.reportData.aggregations.progressRangeDistribution;
    return {
      labels: Object.keys(distribution),
      data: Object.values(distribution)
    };
  }

  getTopProjectsChartData(): any {
    if (!this.reportData?.aggregations.topProjects) return { labels: [], data: [] };
    
    const topProjects = this.reportData.aggregations.topProjects;
    return {
      labels: topProjects.map(p => p.projectName),
      data: topProjects.map(p => p.boxCount)
    };
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPages = Math.min(10, this.totalPages);
    const startPage = Math.max(1, this.currentPage - 5);
    const endPage = Math.min(this.totalPages, startPage + maxPages - 1);
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  navigateToBoxDetails(projectId: string, boxId: string, event: Event): void {
    // Prevent navigation if clicking on checkbox or other interactive elements
    const target = event.target as HTMLElement;
    if (target.tagName === 'INPUT' || target.tagName === 'BUTTON' || target.closest('input, button')) {
      return;
    }
    
    this.router.navigate(['/projects', projectId, 'boxes', boxId]);
  }
}

