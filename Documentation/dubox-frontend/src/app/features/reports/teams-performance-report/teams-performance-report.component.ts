import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ReportsService } from '../../../core/services/reports.service';
import { ProjectService } from '../../../core/services/project.service';
import { TeamService } from '../../../core/services/team.service';
import { Project } from '../../../core/models/project.model';
import { Team } from '../../../core/models/team.model';
import {
  PaginatedTeamsPerformanceResponse,
  TeamsPerformanceReportQueryParams,
  TeamsPerformanceSummary,
  TeamPerformanceItem,
  TeamActivitiesResponse
} from '../../../core/models/teams-performance-report.model';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-teams-performance-report',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './teams-performance-report.component.html',
  styleUrls: ['./teams-performance-report.component.scss']
})
export class TeamsPerformanceReportComponent implements OnInit, OnDestroy {
  // Data
  reportData: PaginatedTeamsPerformanceResponse | null = null;
  summary: TeamsPerformanceSummary | null = null;
  loading = false;
  loadingSummary = false;
  error = '';

  // Filter state
  filtersCollapsed = false;
  projects: Project[] = [];
  teams: Team[] = [];
  selectedProjectId = '';
  selectedTeamId = '';
  selectedStatus: number | null = null;
  searchTerm = '';

  // Pagination
  currentPage = 1;
  pageSize = 25;
  totalCount = 0;
  totalPages = 0;

  // Drill-down state
  selectedTeam: TeamPerformanceItem | null = null;
  teamActivities: TeamActivitiesResponse | null = null;
  loadingActivities = false;
  showActivitiesModal = false;

  // Status options
  statusOptions = [
    { value: 1, label: 'Not Started' },
    { value: 2, label: 'In Progress' },
    { value: 3, label: 'Completed' },
    { value: 4, label: 'On Hold' },
    { value: 5, label: 'Delayed' },
    { value: 6, label: 'Dispatched' }
  ];

  // Search debounce
  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  readonly Math = Math;

  constructor(
    private reportsService: ReportsService,
    private projectService: ProjectService,
    private teamService: TeamService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadProjects();
    this.loadTeams();
    this.setupSearchDebounce();
    this.loadReportData();
    this.loadSummary();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.searchSubject.complete();
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
        this.loadSummary();
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

  loadTeams(): void {
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.teams = teams.filter(team => team.isActive);
      },
      error: (err) => {
        console.error('Failed to load teams:', err);
      }
    });
  }

  loadReportData(): void {
    this.loading = true;
    this.error = '';

    const params: TeamsPerformanceReportQueryParams = {
      page: this.currentPage,
      pageSize: this.pageSize,
      projectId: this.selectedProjectId || undefined,
      teamId: this.selectedTeamId || undefined,
      status: this.selectedStatus !== null ? this.selectedStatus : undefined,
      search: this.searchTerm || undefined
    };

    this.reportsService.getTeamsPerformanceReport(params).subscribe({
      next: (data) => {
        this.reportData = data;
        this.totalCount = data.totalCount;
        this.totalPages = data.totalPages;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load teams performance report:', err);
        this.error = err?.error?.message || err?.message || 'Failed to load teams performance report';
        this.loading = false;
      }
    });
  }

  loadSummary(): void {
    this.loadingSummary = true;

    const params: Partial<TeamsPerformanceReportQueryParams> = {
      projectId: this.selectedProjectId || undefined,
      teamId: this.selectedTeamId || undefined,
      status: this.selectedStatus !== null ? this.selectedStatus : undefined,
      search: this.searchTerm || undefined
    };

    this.reportsService.getTeamsPerformanceSummary(params).subscribe({
      next: (data) => {
        this.summary = data;
        this.loadingSummary = false;
      },
      error: (err) => {
        console.error('Failed to load teams performance summary:', err);
        this.loadingSummary = false;
      }
    });
  }

  onSearchChange(): void {
    this.searchSubject.next(this.searchTerm);
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.loadReportData();
    this.loadSummary();
  }

  onResetFilters(): void {
    this.selectedProjectId = '';
    this.selectedTeamId = '';
    this.selectedStatus = null;
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadReportData();
    this.loadSummary();
  }

  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadReportData();
    }
  }

  onPageChangePage(page: number | string): void {
    if (typeof page === 'number') {
      this.onPageChange(page);
    }
  }

  getStatusLabel(status: string): string {
    return status || 'N/A';
  }

  getStatusClass(status: string): string {
    const statusLower = status.toLowerCase();
    if (statusLower.includes('completed')) return 'status-completed';
    if (statusLower.includes('progress')) return 'status-in-progress';
    if (statusLower.includes('delayed')) return 'status-delayed';
    if (statusLower.includes('hold')) return 'status-on-hold';
    return 'status-default';
  }

  getWorkloadClass(workload: string): string {
    const workloadLower = workload.toLowerCase();
    if (workloadLower === 'overloaded') return 'workload-overloaded';
    if (workloadLower === 'normal') return 'workload-normal';
    return 'workload-low';
  }

  formatProgress(progress: number): string {
    return `${progress.toFixed(1)}%`;
  }

  formatDate(dateString?: string): string {
    if (!dateString) return 'N/A';
    try {
      const date = new Date(dateString);
      return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
    } catch {
      return dateString;
    }
  }

  getPageNumbers(): (number | string)[] {
    const pages: (number | string)[] = [];
    const maxVisible = 5;
    
    if (this.totalPages <= maxVisible) {
      for (let i = 1; i <= this.totalPages; i++) {
        pages.push(i);
      }
    } else {
      if (this.currentPage <= 3) {
        for (let i = 1; i <= 4; i++) {
          pages.push(i);
        }
        pages.push('...');
        pages.push(this.totalPages);
      } else if (this.currentPage >= this.totalPages - 2) {
        pages.push(1);
        pages.push('...');
        for (let i = this.totalPages - 3; i <= this.totalPages; i++) {
          pages.push(i);
        }
      } else {
        pages.push(1);
        pages.push('...');
        for (let i = this.currentPage - 1; i <= this.currentPage + 1; i++) {
          pages.push(i);
        }
        pages.push('...');
        pages.push(this.totalPages);
      }
    }
    
    return pages;
  }

  viewTeamActivities(team: TeamPerformanceItem): void {
    this.selectedTeam = team;
    this.showActivitiesModal = true;
    this.loadingActivities = true;

    this.reportsService.getTeamActivities(team.teamId, {
      projectId: this.selectedProjectId || undefined,
      status: this.selectedStatus !== null ? this.selectedStatus : undefined
    }).subscribe({
      next: (data) => {
        this.teamActivities = data;
        this.loadingActivities = false;
      },
      error: (err) => {
        console.error('Failed to load team activities:', err);
        this.loadingActivities = false;
        alert('Failed to load team activities. Please try again.');
      }
    });
  }

  closeActivitiesModal(): void {
    this.showActivitiesModal = false;
    this.selectedTeam = null;
    this.teamActivities = null;
  }

  navigateToActivityDetails(activity: any): void {
    if (!activity.activityId || !activity.boxId || !activity.projectId) {
      console.error('Missing required IDs for navigation');
      return;
    }
    
    this.closeActivitiesModal();
    this.router.navigate([
      '/projects',
      activity.projectId,
      'boxes',
      activity.boxId,
      'activities',
      activity.activityId
    ]);
  }

  exportToExcel(): void {
    if (!this.reportData || this.reportData.items.length === 0) {
      alert('No data to export');
      return;
    }

    this.loading = true;

    const params: TeamsPerformanceReportQueryParams = {
      projectId: this.selectedProjectId || undefined,
      teamId: this.selectedTeamId || undefined,
      status: this.selectedStatus !== null ? this.selectedStatus : undefined,
      search: this.searchTerm || undefined
    };

    this.reportsService.exportTeamsPerformanceReportToExcel(params).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        const dateStr = new Date().toISOString().split('T')[0];
        link.download = `teams_performance_report_${dateStr}.xlsx`;
        link.click();
        window.URL.revokeObjectURL(url);
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to export teams performance report:', err);
        alert('Failed to export report. Please try again.');
        this.loading = false;
      }
    });
  }
}

