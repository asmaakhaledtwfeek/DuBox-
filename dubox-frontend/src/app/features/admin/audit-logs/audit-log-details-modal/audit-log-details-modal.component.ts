import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuditLog } from '../../../../core/models/audit-log.model';
import { DiffUtil } from '../../../../core/utils/diff.util';

interface ValueRow {
  field: string;
  oldValue: string;
  newValue: string;
}

@Component({
  selector: 'app-audit-log-details-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './audit-log-details-modal.component.html',
  styleUrls: ['./audit-log-details-modal.component.scss']
})
export class AuditLogDetailsModalComponent {
  @Input() visible = false;
  @Input() log: AuditLog | null = null;
  @Output() closed = new EventEmitter<void>();

  readonly DiffUtil = DiffUtil;
  private readonly numericValueRegex = /([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)(%?)/g;

  close(): void {
    this.closed.emit();
  }

  get valueRows(): ValueRow[] {
    if (!this.log) {
      return [];
    }

    const oldValues = this.parseRawValues(this.log.oldValues);
    const newValues = this.parseRawValues(this.log.newValues);

    const keys = new Set<string>([
      ...Object.keys(oldValues),
      ...Object.keys(newValues)
    ]);

    return Array.from(keys)
      .map((key) => ({
        field: this.formatFieldName(key),
        oldValue: this.formatValue(oldValues[key]),
        newValue: this.formatValue(newValues[key])
      }))
      .sort((a, b) => a.field.localeCompare(b.field));
  }

  private parseRawValues(raw: any): Record<string, any> {
    if (!raw) {
      return {};
    }

    if (typeof raw === 'object') {
      return raw;
    }

    if (typeof raw === 'string') {
      const trimmed = raw.trim();
      if (!trimmed) {
        return {};
      }

      try {
        return JSON.parse(trimmed);
      } catch {
        // fallback for delimited key:value entries (comma or newline separated)
        const fallback: Record<string, string> = {};
        const segments = trimmed.split(/(?:\r?\n|,(?=(?:[^"]*"[^"]*")*[^"]*$))/);

        segments.forEach((segment) => {
          const cleanSegment = segment.trim();
          if (!cleanSegment) {
            return;
          }

          const separatorIndex = cleanSegment.indexOf(':');
          if (separatorIndex === -1) {
            return;
          }

          const key = cleanSegment.slice(0, separatorIndex).trim();
          const value = cleanSegment.slice(separatorIndex + 1).trim();

          if (key) {
            fallback[key] = value || '—';
          }
        });

        return fallback;
      }
    }

    return {};
  }

  private formatFieldName(fieldName: string): string {
    if (!fieldName) {
      return '';
    }

    return fieldName
      .replace(/([a-z0-9])([A-Z])/g, '$1 $2')
      .replace(/_/g, ' ')
      .replace(/\s+/g, ' ')
      .replace(/^\w/, (c) => c.toUpperCase())
      .trim();
  }

  private formatValue(value: any): string {
    if (value === null || value === undefined) {
      return '—';
    }

    if (typeof value === 'string') {
      const trimmed = value.trim();
      if (!trimmed) {
        return '—';
      }

      // Percentage formatting
      if (trimmed.endsWith('%')) {
        const numericPart = Number(trimmed.slice(0, -1));
        if (!isNaN(numericPart)) {
          return `${this.formatNumber(numericPart)}%`;
        }
      }

      // Numeric string
      const numericValue = Number(trimmed);
      if (!isNaN(numericValue)) {
        return this.formatNumber(numericValue);
      }

      // Date string
      const parsedDate = new Date(trimmed);
      if (!isNaN(parsedDate.getTime())) {
        return this.formatDate(parsedDate);
      }

      return this.formatNumericText(trimmed);
    }

    if (typeof value === 'number') {
      return this.formatNumber(value);
    }

    if (value instanceof Date) {
      return this.formatDate(value);
    }

    if (typeof value === 'boolean') {
      return value ? 'Yes' : 'No';
    }

    if (typeof value === 'object') {
      try {
        return JSON.stringify(value, null, 2);
      } catch {
        return value.toString();
      }
    }

    return value.toString();
  }

  private formatNumber(value: number): string {
    if (!isFinite(value)) {
      return value.toString();
    }

    const absValue = Math.abs(value);
    const decimals = absValue >= 1 ? 2 : 4;

    if (value !== 0 && absValue < Math.pow(10, -decimals)) {
      return value.toExponential(2);
    }

    const fixed = value.toFixed(decimals);
    return parseFloat(fixed).toString();
  }

  private formatDate(date: Date): string {
    return date.toLocaleString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  private formatNumericText(input: string): string {
    return input.replace(this.numericValueRegex, (_match, valuePart: string, percentPart: string) => {
      const parsedValue = Number(valuePart);
      if (!isFinite(parsedValue)) {
        return _match;
      }

      const formatted = this.formatNumber(parsedValue);
      return percentPart ? `${formatted}${percentPart}` : formatted;
    });
  }
}


