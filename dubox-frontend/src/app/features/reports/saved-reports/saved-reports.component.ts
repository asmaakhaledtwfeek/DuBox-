import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { CustomReportsService } from '../../../core/services/custom-reports.service';
import { CustomReportListItemDto } from '../../../core/models/custom-report.model';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-saved-reports',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './saved-reports.component.html',
  styleUrls: ['./saved-reports.component.scss'],
})
export class SavedReportsComponent implements OnInit {
  reports: CustomReportListItemDto[] = [];
  filteredReports: CustomReportListItemDto[] = [];
  loading = false;
  error = '';
  searchTerm = '';
  confirmDeleteId: string | null = null;
  exportingId: string | null = null;

  private searchSubject = new Subject<string>();

  constructor(
    private service: CustomReportsService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.loadReports();
    this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe(() => {
      this.applyFilter();
    });
  }

  loadReports(): void {
    this.loading = true;
    this.error = '';
    this.service.getAll().subscribe({
      next: (data) => {
        this.reports = data ?? [];
        this.applyFilter();
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load saved reports.';
        this.loading = false;
      },
    });
  }

  applyFilter(): void {
    const term = this.searchTerm.toLowerCase().trim();
    this.filteredReports = term
      ? this.reports.filter(
          (r) => r.name.toLowerCase().includes(term) || r.description?.toLowerCase().includes(term),
        )
      : [...this.reports];
  }

  onSearchChange(): void {
    this.searchSubject.next(this.searchTerm);
  }

  openBuilder(id?: string): void {
    if (id) {
      this.router.navigate(['/reports/builder', id]);
    } else {
      this.router.navigate(['/reports/builder']);
    }
  }

  confirmDelete(id: string): void {
    this.confirmDeleteId = id;
  }

  cancelDelete(): void {
    this.confirmDeleteId = null;
  }

  deleteReport(id: string): void {
    this.service.delete(id).subscribe({
      next: () => {
        this.reports = this.reports.filter((r) => r.id !== id);
        this.applyFilter();
        this.confirmDeleteId = null;
      },
      error: () => {
        this.error = 'Failed to delete report.';
        this.confirmDeleteId = null;
      },
    });
  }

  exportReport(report: CustomReportListItemDto): void {
    this.exportingId = report.id;
    this.service.exportById(report.id).subscribe({
      next: (blob) => {
        this.service.triggerDownload(blob, `${report.name}_${new Date().toISOString().slice(0, 10)}.xlsx`);
        this.exportingId = null;
      },
      error: () => {
        this.error = 'Export failed. Please try again.';
        this.exportingId = null;
      },
    });
  }

  getDataSourceLabel(key: string): string {
    const labels: Record<string, string> = {
      activities: 'Activities',
      teams_performance: 'Teams Performance',
      boxes: 'Boxes',
      projects: 'Projects',
      quality_issues: 'Quality Issues',
    };
    return labels[key] ?? key;
  }

  getChartTypeLabel(type: string): string {
    const labels: Record<string, string> = {
      table: 'Table',
      bar: 'Bar Chart',
      line: 'Line Chart',
      pie: 'Pie Chart',
    };
    return labels[type] ?? type;
  }

  getChartTypeIcon(type: string): string {
    const icons: Record<string, string> = {
      table: `<rect x="3" y="3" width="18" height="18" rx="2"/><line x1="3" y1="9" x2="21" y2="9"/><line x1="3" y1="15" x2="21" y2="15"/><line x1="9" y1="3" x2="9" y2="21"/>`,
      bar: `<line x1="18" y1="20" x2="18" y2="10"/><line x1="12" y1="20" x2="12" y2="4"/><line x1="6" y1="20" x2="6" y2="14"/><line x1="2" y1="20" x2="22" y2="20"/>`,
      line: `<polyline points="22 12 18 12 15 21 9 3 6 12 2 12"/>`,
      pie: `<path d="M21.21 15.89A10 10 0 1 1 8 2.83"/><path d="M22 12A10 10 0 0 0 12 2v10z"/>`,
    };
    return icons[type] ?? icons['table'];
  }
}
