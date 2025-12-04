import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProgressUpdate, ActivityProgressStatus } from '../../../core/models/progress-update.model';

@Component({
  selector: 'app-progress-updates-table',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './progress-updates-table.component.html',
  styleUrls: ['./progress-updates-table.component.scss']
})
export class ProgressUpdatesTableComponent {
  @Input() updates: ProgressUpdate[] | null = [];
  @Input() loading = false;
  @Input() error = '';
  @Input() currentPage = 1;
  @Input() pageSize = 10;
  @Input() totalCount = 0;
  @Input() totalPages = 0;
  @Input() showSearch = false;
  @Input() searchTerm = '';
  @Input() activityName = '';
  @Input() status = '';
  @Input() fromDate = '';
  @Input() toDate = '';
  @Input() hideActivityColumn = false;
  @Input() hideActivityFilter = false;
  @Input() subtitle = 'Latest updates recorded for this box';
  @Output() refresh = new EventEmitter<void>();
  @Output() viewDetails = new EventEmitter<ProgressUpdate>();
  @Output() pageChange = new EventEmitter<number>();
  @Output() pageSizeChange = new EventEmitter<number>();
  @Output() searchChange = new EventEmitter<{
    searchTerm: string;
    activityName: string;
    status: string;
    fromDate: string;
    toDate: string;
  }>();
  @Output() clearSearch = new EventEmitter<void>();
  @Output() toggleSearch = new EventEmitter<void>();

  readonly Math = Math;
  readonly ActivityProgressStatus = ActivityProgressStatus;

  statusOptions = [
    { value: '', label: 'All Statuses' },
    { value: 'InProgress', label: 'In Progress' },
    { value: 'Completed', label: 'Completed' },
    { value: 'OnHold', label: 'On Hold' },
    { value: 'Delayed', label: 'Delayed' }
  ];

  onSearchChange(): void {
    this.searchChange.emit({
      searchTerm: this.searchTerm,
      activityName: this.activityName,
      status: this.status,
      fromDate: this.fromDate,
      toDate: this.toDate
    });
  }

  onClearSearch(): void {
    this.searchTerm = '';
    this.activityName = '';
    this.status = '';
    this.fromDate = '';
    this.toDate = '';
    this.clearSearch.emit();
  }

  onToggleSearch(): void {
    this.toggleSearch.emit();
  }

  onRefresh(): void {
    this.refresh.emit();
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.pageChange.emit(page);
      // Scroll to top of table
      const tableElement = document.querySelector('.table-wrapper');
      if (tableElement) {
        tableElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    }
  }

  changePageSize(size: number): void {
    this.pageSizeChange.emit(size);
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 7;
    const current = this.currentPage;
    const total = this.totalPages;
    
    let startPage = Math.max(1, current - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(total, startPage + maxPagesToShow - 1);
    
    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  trackById(_index: number, item: ProgressUpdate): string | undefined {
    return item.progressUpdateId;
  }

  openDetails(update: ProgressUpdate): void {
    this.viewDetails.emit(update);
  }

  getStatusClass(status: string | ActivityProgressStatus | undefined): string {
    if (!status) return 'status-unknown';
    
    const statusStr = typeof status === 'string' ? status : String(status);
    const normalized = statusStr.toLowerCase();
    
    switch (normalized) {
      case 'notstarted':
        return 'status-not-started';
      case 'inprogress':
        return 'status-in-progress';
      case 'completed':
        return 'status-completed';
      case 'onhold':
        return 'status-on-hold';
      case 'delayed':
        return 'status-delayed';
      default:
        return 'status-unknown';
    }
  }

  getStatusLabel(status: string | ActivityProgressStatus | undefined): string {
    if (!status) return '—';
    
    const statusStr = typeof status === 'string' ? status : String(status);
    const normalized = statusStr.toLowerCase();
    
    switch (normalized) {
      case 'notstarted':
        return 'Not Started';
      case 'inprogress':
        return 'In Progress';
      case 'completed':
        return 'Completed';
      case 'onhold':
        return 'On Hold';
      case 'delayed':
        return 'Delayed';
      default:
        return statusStr;
    }
  }

  hasPhotos(update: ProgressUpdate): boolean {
    if (!update.photo) return false;
    try {
      const parsed = JSON.parse(update.photo);
      if (Array.isArray(parsed)) {
        return parsed.length > 0;
      }
    } catch {
      if (typeof update.photo === 'string') {
        return update.photo.trim().length > 0;
      }
    }
    return false;
  }

  getPhotoCount(update: ProgressUpdate): number {
    if (!update.photo) return 0;
    try {
      const parsed = JSON.parse(update.photo);
      if (Array.isArray(parsed)) {
        return parsed.length;
      }
    } catch {
      if (typeof update.photo === 'string') {
        const urls = update.photo.split(',').filter(url => url.trim().length > 0);
        return urls.length;
      }
    }
    return 0;
  }
}

