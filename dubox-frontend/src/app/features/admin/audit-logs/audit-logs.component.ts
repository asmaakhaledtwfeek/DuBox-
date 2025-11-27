import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { AuditLogService } from '../../../core/services/audit-log.service';
import { AuditLog, AuditLogQueryParams } from '../../../core/models/audit-log.model';
import { DiffUtil } from '../../../core/utils/diff.util';
import { AuditLogDetailsModalComponent } from './audit-log-details-modal/audit-log-details-modal.component';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-audit-logs',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent, AuditLogDetailsModalComponent],
  templateUrl: './audit-logs.component.html',
  styleUrls: ['./audit-logs.component.scss']
})
export class AuditLogsComponent implements OnInit {
  logs: AuditLog[] = [];
  filteredLogs: AuditLog[] = [];
  availableActions: string[] = [];
  availableTables: string[] = [];
  private actionSet = new Set<string>();
  private tableSet = new Set<string>();
  loading = false;
  error = '';
  selectedLog: AuditLog | null = null;
  isDetailsModalOpen = false;

  // Filters
  filterTableName = '';
  filterAction = '';
  filterSearchTerm = '';
  filterFromDate = '';
  filterToDate = '';

  // Expand/collapse state
  expandedLogs = new Set<string>();

  readonly DiffUtil = DiffUtil;
  private readonly numericValueRegex = /([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)(%?)/g;

  constructor(private auditLogService: AuditLogService) {}

  ngOnInit(): void {
    this.loadAuditLogs();
  }

  loadAuditLogs(): void {
    this.loading = true;
    this.error = '';

    const params: AuditLogQueryParams = {};
    if (this.filterTableName) params.tableName = this.filterTableName;
    if (this.filterAction) params.action = this.filterAction;
    if (this.filterSearchTerm) params.searchTerm = this.filterSearchTerm;
    if (this.filterFromDate) {
      const from = new Date(this.filterFromDate);
      from.setHours(0, 0, 0, 0);
      params.fromDate = from.toISOString();
    }
    if (this.filterToDate) {
      const to = new Date(this.filterToDate);
      to.setHours(23, 59, 59, 999);
      params.toDate = to.toISOString();
    }

    this.auditLogService.getAuditLogs(params).subscribe({
      next: (logs) => {
        this.logs = logs;
        this.filteredLogs = logs;
        this.actionSet.clear();
        this.tableSet.clear();
        logs.forEach(log => {
          if (log.action) {
            this.actionSet.add(log.action);
          }
          if (log.tableName) {
            this.tableSet.add(log.tableName);
          }
        });
        this.availableActions = Array.from(this.actionSet).sort((a, b) => a.localeCompare(b));
        this.availableTables = Array.from(this.tableSet).sort((a, b) => a.localeCompare(b));
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load audit logs:', err);
        this.error = err?.error?.message || err?.message || 'Failed to load audit logs';
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    this.loadAuditLogs();
  }

  resetFilters(): void {
    this.filterTableName = '';
    this.filterAction = '';
    this.filterSearchTerm = '';
    this.filterFromDate = '';
    this.filterToDate = '';
    this.loadAuditLogs();
  }

  toggleExpand(logId: string): void {
    if (this.expandedLogs.has(logId)) {
      this.expandedLogs.delete(logId);
    } else {
      this.expandedLogs.add(logId);
    }
  }

  isExpanded(logId: string): boolean {
    return this.expandedLogs.has(logId);
  }

  shouldShowExpandButton(changes: any[]): boolean {
    return changes && changes.length > 5;
  }

  getVisibleChanges(changes: any[]): any[] {
    if (!changes || changes.length === 0) return [];
    if (changes.length <= 5) return changes;
    return changes.slice(0, 5);
  }

  getHiddenChanges(changes: any[]): any[] {
    if (!changes || changes.length <= 5) return [];
    return changes.slice(5);
  }

  formatDate(date: Date | string): string {
    const d = typeof date === 'string' ? new Date(date) : date;
    return d.toLocaleDateString('en-US', { 
      year: 'numeric', 
      month: 'short', 
      day: 'numeric' 
    });
  }

  trackByAction(_: number, action: string): string {
    return action;
  }

  trackByTable(_: number, table: string): string {
    return table;
  }

  getTableLabel(tableName: string): string {
    const labelMap: Record<string, string> = {
      Project: 'Project',
      Projects: 'Project',
      Box: 'Box',
      Boxes: 'Box',
      BoxActivity: 'Activity',
      BoxActivities: 'Activity',
      QualityIssue: 'Quality Issue',
      QualityIssues: 'Quality Issue',
      User: 'User',
      Users: 'User',
      Team: 'Team',
      Teams: 'Team',
      Material: 'Material',
      Materials: 'Material'
    };
    return labelMap[tableName] || tableName;
  }

  getNoChangeSummary(log: AuditLog): string {
    if (log.description) {
      return this.formatDescription(log.description);
    }

    const entityLabel = this.getTableLabel(log.tableName);
    const actionLabel = this.DiffUtil.getActionLabel(log.action);
    const entityName = log.entityDisplayName ? ` "${log.entityDisplayName}"` : '';

    return `${actionLabel} ${entityLabel}${entityName} with no tracked field changes.`;
  }

  getLogAuthor(log: AuditLog): string | null {
    return log.changedByFullName || log.changedByUsername || null;
  }

  formatDescription(description?: string | null): string {
    if (!description) {
      return '';
    }
    return this.formatNumericValues(description);
  }

  private formatNumericValues(text: string): string {
    return text.replace(this.numericValueRegex, (_match, valuePart: string, percentPart: string) => {
      const parsedValue = Number(valuePart);
      if (!isFinite(parsedValue)) {
        return _match;
      }

      const absValue = Math.abs(parsedValue);
      const decimals = absValue >= 1 ? 2 : 4;
      let formatted: string;

      if (parsedValue !== 0 && absValue < Math.pow(10, -decimals)) {
        formatted = parsedValue.toExponential(2);
      } else {
        formatted = parsedValue.toFixed(decimals);
        formatted = parseFloat(formatted).toString();
      }

      return percentPart ? `${formatted}${percentPart}` : formatted;
    });
  }

  openLogDetails(log: AuditLog): void {
    this.selectedLog = log;
    this.isDetailsModalOpen = true;
    document.body.style.overflow = 'hidden';
  }

  closeLogDetails(): void {
    this.isDetailsModalOpen = false;
    this.selectedLog = null;
    document.body.style.overflow = '';
  }

  exportToExcel(): void {
    if (!this.filteredLogs || this.filteredLogs.length === 0) {
      alert('No data to export');
      return;
    }

    // Prepare data for Excel
    const exportData = this.filteredLogs.map(log => {
      const row: any = {
        'Timestamp': this.formatDateForExcel(log.timestamp),
        'Action': log.action || '',
        'Entity Type': this.getTableLabel(log.tableName),
        'Entity Name': log.entityDisplayName || '',
        'Description': this.formatDescription(log.description) || this.getNoChangeSummary(log),
        'Changed By': this.getLogAuthor(log) || 'System',
      };

      // Add changes as separate columns
      if (log.changes && log.changes.length > 0) {
        const changesText = log.changes
          .map(change => `${change.field}: ${DiffUtil.formatValue(change.oldValue)} → ${DiffUtil.formatValue(change.newValue)}`)
          .join('; ');
        row['Changes'] = changesText;
      } else {
        row['Changes'] = 'No changes';
      }

      return row;
    });

    // Create worksheet
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);

    // Set column widths
    const columnWidths = [
      { wch: 20 }, // Timestamp
      { wch: 12 }, // Action
      { wch: 15 }, // Entity Type
      { wch: 25 }, // Entity Name
      { wch: 50 }, // Description
      { wch: 20 }, // Changed By
      { wch: 60 }, // Changes
    ];
    worksheet['!cols'] = columnWidths;

    // Create workbook
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Audit Logs': worksheet },
      SheetNames: ['Audit Logs']
    };

    // Generate file name with current date
    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    const fileName = `Audit_Logs_${dateStr}.xlsx`;

    // Save file
    XLSX.writeFile(workbook, fileName);
  }

  private formatDateForExcel(date: Date | string): string {
    const d = typeof date === 'string' ? new Date(date) : date;
    return d.toLocaleString('en-US', { 
      year: 'numeric', 
      month: 'short', 
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    });
  }
}

