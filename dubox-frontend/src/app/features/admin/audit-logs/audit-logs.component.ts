import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { AuditLogService } from '../../../core/services/audit-log.service';
import { AuditLog, AuditLogQueryParams, FieldChange } from '../../../core/models/audit-log.model';
import { DiffUtil } from '../../../core/utils/diff.util';
import { AuditLogDetailsModalComponent } from './audit-log-details-modal/audit-log-details-modal.component';
import { UserService, UserDto } from '../../../core/services/user.service';
import * as ExcelJS from 'exceljs';

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
  availableUsers: UserDto[] = [];
  private actionSet = new Set<string>();
  private tableSet = new Set<string>();
  loading = false;
  loadingUsers = false;
  error = '';
  selectedLog: AuditLog | null = null;
  isDetailsModalOpen = false;

  // Pagination
  currentPage = 1;
  pageSize = 25;
  totalCount = 0;
  totalPages = 0;

  // Filters
  filterTableName = '';
  filterAction = '';
  filterSearchTerm = '';
  filterFromDate = '';
  filterToDate = '';
  filterChangedBy = '';

  // Expand/collapse state
  expandedLogs = new Set<string>();

  readonly DiffUtil = DiffUtil;
  readonly Math = Math;
  private readonly numericValueRegex = /([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)(%?)/g;

  constructor(
    private auditLogService: AuditLogService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.loadAuditLogs();
    this.loadUsers();
  }

  loadAuditLogs(): void {
    this.loading = true;
    this.error = '';

    const params: AuditLogQueryParams = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    };
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
    if (this.filterChangedBy) params.changedBy = this.filterChangedBy;

    this.auditLogService.getAuditLogs(params).subscribe({
      next: (response) => {
        this.logs = response.items;
        this.filteredLogs = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.currentPage = response.pageNumber;
        this.pageSize = response.pageSize;

        // Update available filters from current page (could be enhanced to load all unique values separately)
        this.actionSet.clear();
        this.tableSet.clear();
        response.items.forEach(log => {
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

  loadUsers(): void {
    this.loadingUsers = true;
    this.userService.getUsers(1, 1000).subscribe({
      next: (response) => {
        this.availableUsers = response.items.sort((a, b) => {
          const nameA = a.fullName || a.email || '';
          const nameB = b.fullName || b.email || '';
          return nameA.localeCompare(nameB);
        });
        this.loadingUsers = false;
      },
      error: (err) => {
        console.error('Failed to load users:', err);
        this.loadingUsers = false;
      }
    });
  }

  applyFilters(): void {
    this.currentPage = 1; // Reset to first page when filters change
    this.loadAuditLogs();
  }

  resetFilters(): void {
    this.filterTableName = '';
    this.filterAction = '';
    this.filterSearchTerm = '';
    this.filterFromDate = '';
    this.filterToDate = '';
    this.filterChangedBy = '';
    this.currentPage = 1;
    this.loadAuditLogs();
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadAuditLogs();
      // Scroll to top of logs table
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  changePageSize(size: number): void {
    this.pageSize = size;
    this.currentPage = 1; // Reset to first page when page size changes
    this.loadAuditLogs();
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 7;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(this.totalPages, startPage + maxPagesToShow - 1);
    
    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
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

  getActionType(action: string): 'create' | 'update' | 'delete' | 'assignment' | 'default' {
    const upperAction = action.toUpperCase();
    if (upperAction.includes('CREATE') || upperAction.includes('INSERT')) {
      return 'create';
    } else if (upperAction.includes('UPDATE') || upperAction.includes('MODIFY')) {
      return 'update';
    } else if (upperAction.includes('DELETE') || upperAction.includes('REMOVE')) {
      return 'delete';
    } else if (upperAction.includes('ASSIGN')) {
      return 'assignment';
    }
    return 'default';
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

  async exportToExcel(): Promise<void> {
    if (!this.filteredLogs || this.filteredLogs.length === 0) {
      alert('No data to export');
      return;
    }

    // Helper function to get changes from log
    const getChangesFromLog = (log: AuditLog): FieldChange[] => {
      // First, try to use the changes array from backend (preferred)
      if (log.changes && Array.isArray(log.changes) && log.changes.length > 0) {
        return log.changes;
      }

      // Fallback: try to parse from oldValues and newValues if changes array is empty
      const changes: FieldChange[] = [];
      
      // Try to get oldValues and newValues (could be string, JSON string, or object)
      let oldVals: any = null;
      let newVals: any = null;
      
      // Helper to safely parse JSON - only parse if it looks like JSON
      const safeParseJSON = (value: any): any => {
        if (!value) return null;
        
        // If already an object, return as-is
        if (typeof value === 'object' && value !== null && !Array.isArray(value)) {
          return value;
        }
        
        // If it's a string, check if it looks like JSON before parsing
        if (typeof value === 'string') {
          const trimmed = value.trim();
          // Only try to parse if it starts with { or [ (JSON object/array)
          if (trimmed.startsWith('{') || trimmed.startsWith('[')) {
            try {
              return JSON.parse(value);
            } catch (e) {
              // If JSON parsing fails, return null (can't extract field changes)
              return null;
            }
          }
          // Plain string (not JSON) - return as string for manual parsing
          return value;
        }
        
        return null;
      };
      
      oldVals = safeParseJSON(log.oldValues);
      newVals = safeParseJSON(log.newValues);
      
      // Helper to parse plain string format like "FieldName: Value"
      const parsePlainStringValue = (str: string): { field: string; value: string } | null => {
        if (!str || typeof str !== 'string') return null;
        
        // Check if it's in format "FieldName: Value"
        const colonIndex = str.indexOf(':');
        if (colonIndex > 0) {
          const field = str.substring(0, colonIndex).trim();
          const value = str.substring(colonIndex + 1).trim();
          return { field, value };
        }
        
        return null;
      };
      
      // If we have both old and new values as objects (JSON), compare them
      if (oldVals && newVals && typeof oldVals === 'object' && typeof newVals === 'object' && !Array.isArray(oldVals) && !Array.isArray(newVals)) {
        // Get all unique keys from both objects
        const allKeys = new Set([...Object.keys(oldVals), ...Object.keys(newVals)]);
        
        allKeys.forEach(key => {
          // Skip internal/system fields
          const skipFields = ['Id', 'CreatedDate', 'ModifiedDate', 'CreatedBy', 'ModifiedBy', 'AuditId', 'ChangedBy', 'ChangedDate'];
          if (skipFields.some(f => key.toLowerCase().includes(f.toLowerCase()))) {
            return;
          }

          const oldVal = oldVals[key];
          const newVal = newVals[key];
          
          // Convert to strings for comparison
          const oldStr = oldVal !== null && oldVal !== undefined ? String(oldVal) : null;
          const newStr = newVal !== null && newVal !== undefined ? String(newVal) : null;
          
          // Only include if values are different
          if (oldStr !== newStr) {
            changes.push({
              field: key,
              oldValue: oldStr,
              newValue: newStr
            });
          }
        });
      } 
      // If oldValues and newValues are plain strings (like "Status: NotStarted")
      else if (typeof oldVals === 'string' && typeof newVals === 'string') {
        const oldParsed = parsePlainStringValue(oldVals);
        const newParsed = parsePlainStringValue(newVals);
        
        if (oldParsed && newParsed && oldParsed.field === newParsed.field) {
          // Same field, different values - this is a change
          if (oldParsed.value !== newParsed.value) {
            changes.push({
              field: oldParsed.field,
              oldValue: oldParsed.value,
              newValue: newParsed.value
            });
          }
        } else if (oldParsed || newParsed) {
          // One or both parsed, add them
          const parsed = oldParsed || newParsed;
          if (parsed) {
            changes.push({
              field: parsed.field,
              oldValue: oldParsed?.value || null,
              newValue: newParsed?.value || null
            });
          }
        }
      }
      // If only one exists (e.g., INSERT or DELETE), include all fields
      else if (oldVals || newVals) {
        const source = oldVals || newVals;
        if (typeof source === 'object' && source !== null && !Array.isArray(source)) {
          Object.keys(source).forEach(key => {
            const skipFields = ['Id', 'CreatedDate', 'ModifiedDate', 'CreatedBy', 'ModifiedBy', 'AuditId', 'ChangedBy', 'ChangedDate'];
            if (!skipFields.some(f => key.toLowerCase().includes(f.toLowerCase()))) {
              changes.push({
                field: key,
                oldValue: oldVals?.[key] ? String(oldVals[key]) : null,
                newValue: newVals?.[key] ? String(newVals[key]) : null
              });
            }
          });
        }
      }
      
      return changes;
    };

    // Helper function to format changes text for Excel
    const formatChangesText = (changes: FieldChange[]): string => {
      if (!changes || !Array.isArray(changes) || changes.length === 0) {
        return 'No changes';
      }

      try {
        const formatted = changes
          .filter(change => change && change.field) // Filter out invalid changes
          .map(change => {
            const fieldName = change.field || 'Unknown Field';
            const oldVal = change.oldValue !== null && change.oldValue !== undefined 
              ? DiffUtil.formatValue(change.oldValue) 
              : '—';
            const newVal = change.newValue !== null && change.newValue !== undefined 
              ? DiffUtil.formatValue(change.newValue) 
              : '—';
            return `${fieldName}: ${oldVal} → ${newVal}`;
          })
          .join('; ');
        
        return formatted || 'No changes';
      } catch (e) {
        console.error('Error formatting changes:', e, changes);
        return 'Error formatting changes';
      }
    };

    // Create a new workbook and worksheet
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Audit Logs');

    // Define column headers
    const headers = [
      'Timestamp',
      'Action',
      'Entity Type',
      'Entity Name',
      'Description',
      'Changed By',
      'Changes'
    ];

    // Set column widths
    worksheet.columns = [
      { width: 20 }, // Timestamp
      { width: 12 }, // Action
      { width: 15 }, // Entity Type
      { width: 25 }, // Entity Name
      { width: 50 }, // Description
      { width: 20 }, // Changed By
      { width: 60 }  // Changes
    ];

    // Add header row with styling
    const headerRow = worksheet.addRow(headers);
    headerRow.eachCell((cell) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FF4472C4' } // Blue background (same as quality issues)
      };
      cell.font = {
        bold: true,
        color: { argb: 'FFFFFFFF' }, // White text
        size: 11
      };
      cell.alignment = {
        horizontal: 'center',
        vertical: 'middle',
        wrapText: true
      };
      cell.border = {
        top: { style: 'thin', color: { argb: 'FF000000' } },
        bottom: { style: 'thin', color: { argb: 'FF000000' } },
        left: { style: 'thin', color: { argb: 'FF000000' } },
        right: { style: 'thin', color: { argb: 'FF000000' } }
      };
    });
    headerRow.height = 25; // Set header row height

    // Add data rows - use the same logs that are displayed in the UI
    this.filteredLogs.forEach((log, index) => {
      // Get changes from log (backend provides changes array, fallback to parsing oldValues/newValues)
      const changes = getChangesFromLog(log);
      const changesText = formatChangesText(changes);

      const row = worksheet.addRow([
        this.formatDateForExcel(log.timestamp),
        log.action || '',
        this.getTableLabel(log.tableName),
        log.entityDisplayName || '',
        this.formatDescription(log.description) || this.getNoChangeSummary(log),
        this.getLogAuthor(log) || 'System',
        changesText
      ]);

      // Style data rows
      row.eachCell((cell, colNumber) => {
        cell.border = {
          top: { style: 'thin', color: { argb: 'FFE0E0E0' } },
          bottom: { style: 'thin', color: { argb: 'FFE0E0E0' } },
          left: { style: 'thin', color: { argb: 'FFE0E0E0' } },
          right: { style: 'thin', color: { argb: 'FFE0E0E0' } }
        };
        cell.alignment = {
          vertical: 'middle',
          wrapText: true
        };
        // Alternate row colors for better readability
        if (index % 2 === 0) {
          cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFF9F9F9' } // Very light gray
          };
        }
      });
    });

    // Freeze header row
    worksheet.views = [
      {
        state: 'frozen',
        ySplit: 1 // Freeze first row
      }
    ];

    // Generate filename with current date
    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    const fileName = `Audit_Logs_${dateStr}.xlsx`;

    // Export to Excel
    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    link.click();
    window.URL.revokeObjectURL(url);
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

