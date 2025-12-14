import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoxLog } from '../../../core/models/box.model';
import { DiffUtil } from '../../../core/utils/diff.util';

interface ValueRow {
  field: string;
  oldValue: string;
  newValue: string;
}

@Component({
  selector: 'app-box-log-details-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './box-log-details-modal.component.html',
  styleUrls: ['./box-log-details-modal.component.scss']
})
export class BoxLogDetailsModalComponent {
  @Input() visible = false;
  @Input() log: BoxLog | null = null;
  @Input() boxName?: string;
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

    // First, try to parse oldValues and newValues (like audit logs)
    // This is the preferred method as it contains all property changes
    if (this.log.oldValues || this.log.newValues) {
      const oldValues = this.parseRawValues(this.log.oldValues);
      const newValues = this.parseRawValues(this.log.newValues);

      const keys = new Set<string>([
        ...Object.keys(oldValues),
        ...Object.keys(newValues)
      ]);

      const rows: ValueRow[] = Array.from(keys)
        .map((key) => ({
          field: this.formatFieldName(key),
          oldValue: this.formatValue(oldValues[key]),
          newValue: this.formatValue(newValues[key])
        }))
        .filter(row => row.oldValue !== row.newValue) // Only show changed fields
        .sort((a, b) => a.field.localeCompare(b.field));

      if (rows.length > 0) {
        return rows;
      }
    }

    // Fallback: Parse description for field changes
    // Example: "Box progress updated automatically from 4.44% to 7.40%."
    const rows: ValueRow[] = [];
    const descriptionChanges = this.parseDescriptionForChanges(this.log.description);
    if (descriptionChanges.length > 0) {
      rows.push(...descriptionChanges);
    }

    // Also check if there's a direct field/oldValue/newValue
    if (this.log.field || this.log.oldValue || this.log.newValue) {
      const directField = this.log.field || this.extractFieldFromDescription(this.log.description) || 'Field';
      // Check if this field is already in the rows from description parsing
      const existingRow = rows.find(r => r.field.toLowerCase() === this.formatFieldName(directField).toLowerCase());
      if (!existingRow) {
        rows.push({
          field: this.formatFieldName(directField),
          oldValue: this.formatValue(this.log.oldValue),
          newValue: this.formatValue(this.log.newValue)
        });
      } else {
        // Update existing row with direct values if they're more complete
        if (this.log.oldValue && existingRow.oldValue === '—') {
          existingRow.oldValue = this.formatValue(this.log.oldValue);
        }
        if (this.log.newValue && existingRow.newValue === '—') {
          existingRow.newValue = this.formatValue(this.log.newValue);
        }
      }
    }

    // Sort by field name
    return rows.sort((a, b) => a.field.localeCompare(b.field));
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
        // fallback for delimited key:value entries (pipe, comma, or newline separated)
        // Format: "Key: Value | Key: Value | Key: Value"
        const fallback: Record<string, string> = {};
        
        // First try pipe separator (most common for box logs)
        let segments: string[] = [];
        if (trimmed.includes('|')) {
          segments = trimmed.split(/\s*\|\s*/);
        } else if (trimmed.includes('\n')) {
          segments = trimmed.split(/\r?\n/);
        } else {
          // Try comma, but be careful with commas in values (like dates)
          segments = trimmed.split(/,(?=\s*[A-Za-z][A-Za-z0-9]*\s*:)/);
        }

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

  private parseDescriptionForChanges(description?: string): ValueRow[] {
    if (!description) {
      return [];
    }

    const changes: ValueRow[] = [];

    // Pattern 1: "FieldName updated [automatically] from X to Y"
    // Example: "Box progress updated automatically from 4.44% to 7.4074074074074074074074074074%."
    // This pattern looks for a word before "updated" (like "progress")
    const fieldUpdatedPattern = /\b(\w+)\s+updated\s+(?:automatically\s+)?from\s+([^\s]+(?:\s+[^\s]+)*?)\s+to\s+([^\s]+(?:\s+[^\s]+)*?)/gi;
    let match;
    while ((match = fieldUpdatedPattern.exec(description)) !== null) {
      const fieldName = match[1]; // e.g., "progress"
      const oldVal = match[2].replace(/[.,;]$/, '').trim();
      const newVal = match[3].replace(/[.,;]$/, '').trim();
      
      changes.push({
        field: this.formatFieldName(fieldName),
        oldValue: this.formatValue(oldVal),
        newValue: this.formatValue(newVal)
      });
    }

    // Pattern 2: "from X to Y" without field name (extract field from context)
    // Example: "from 4.44% to 7.40%"
    if (changes.length === 0) {
      const simpleFromToPattern = /from\s+([^\s]+(?:\s+[^\s]+)*?)\s+to\s+([^\s]+(?:\s+[^\s]+)*?)/gi;
      while ((match = simpleFromToPattern.exec(description)) !== null) {
        const oldVal = match[1].replace(/[.,;]$/, '').trim();
        const newVal = match[2].replace(/[.,;]$/, '').trim();
        
        // Try to extract field name from context
        const fieldName = this.extractFieldFromDescription(description) || 'Value';
        
        changes.push({
          field: this.formatFieldName(fieldName),
          oldValue: this.formatValue(oldVal),
          newValue: this.formatValue(newVal)
        });
      }
    }

    // Pattern 3: Parse if description contains structured data
    // Example: "Status: Active → InProgress" or "Progress: 10% → 20%"
    const arrowPattern = /(\w+):\s*([^→]+?)\s*→\s*([^\s,.;]+)/gi;
    while ((match = arrowPattern.exec(description)) !== null) {
      changes.push({
        field: this.formatFieldName(match[1]),
        oldValue: this.formatValue(match[2].trim()),
        newValue: this.formatValue(match[3].trim())
      });
    }

    return changes;
  }

  private extractFieldFromDescription(description?: string): string | null {
    if (!description) {
      return null;
    }

    // Common field names in box logs - check in order of specificity
    const fieldPatterns = [
      { pattern: /\bprogress\b/gi, name: 'Progress' },
      { pattern: /\bstatus\b/gi, name: 'Status' },
      { pattern: /\blocation\b/gi, name: 'Location' },
      { pattern: /\bassigned\b/gi, name: 'Assigned' },
      { pattern: /\bname\b/gi, name: 'Name' },
      { pattern: /\bcode\b/gi, name: 'Code' },
      { pattern: /\bdescription\b/gi, name: 'Description' }
    ];

    for (const { pattern, name } of fieldPatterns) {
      if (pattern.test(description)) {
        return name;
      }
    }

    return null;
  }

  private formatFieldName(fieldName: string): string {
    if (!fieldName) {
      return 'Field';
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
