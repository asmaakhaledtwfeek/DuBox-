import { FieldChange } from '../models/audit-log.model';

/**
 * Utility functions for formatting audit log diffs
 */
export class DiffUtil {
  /**
   * Format a value for display
   */
  static formatValue(value: string | null | undefined): string {
    if (!value) return '—';
    
    // Try to parse as date
    const dateMatch = value.match(/^\d{4}-\d{2}-\d{2}/);
    if (dateMatch) {
      try {
        const date = new Date(value);
        if (!isNaN(date.getTime())) {
          return date.toLocaleDateString('en-US', { 
            year: 'numeric', 
            month: 'short', 
            day: 'numeric' 
          });
        }
      } catch {}
    }

    // Try to parse as number
    const num = parseFloat(value);
    if (!isNaN(num) && isFinite(num)) {
      // Format large numbers with K/M suffix
      if (Math.abs(num) >= 1000000) {
        return `${(num / 1000000).toFixed(1)}M`;
      } else if (Math.abs(num) >= 1000) {
        return `${(num / 1000).toFixed(1)}k`;
      }
      return num.toLocaleString('en-US', { maximumFractionDigits: 2 });
    }

    return value;
  }

  /**
   * Get action icon emoji
   */
  static getActionIcon(action: string): string {
    const upperAction = action.toUpperCase();
    if (upperAction.includes('INSERT') || upperAction.includes('CREATE')) {
      return '🟢';
    } else if (upperAction.includes('UPDATE') || upperAction.includes('MODIFY')) {
      return '🟡';
    } else if (upperAction.includes('DELETE') || upperAction.includes('REMOVE')) {
      return '🔴';
    }
    return '⚪';
  }

  /**
   * Get action label
   */
  static getActionLabel(action: string): string {
    const upperAction = action.toUpperCase();
    if (upperAction.includes('INSERT') || upperAction.includes('CREATE')) {
      return 'Created';
    } else if (upperAction.includes('UPDATE') || upperAction.includes('MODIFY')) {
      return 'Updated';
    } else if (upperAction.includes('DELETE') || upperAction.includes('REMOVE')) {
      return 'Deleted';
    }
    return action;
  }

  /**
   * Format entity name for display
   */
  static formatEntityName(tableName: string): string {
    const nameMap: Record<string, string> = {
      'Projects': 'Project',
      'Boxes': 'Box',
      'BoxActivities': 'Activity',
      'QualityIssues': 'Quality Issue',
      'Users': 'User',
      'Teams': 'Team',
      'Materials': 'Material'
    };
    return nameMap[tableName] || tableName;
  }

  /**
   * Format changes for display
   */
  static formatChanges(changes: FieldChange[]): string {
    if (!changes || changes.length === 0) {
      return 'No changes';
    }

    return changes
      .map(change => {
        const oldVal = this.formatValue(change.oldValue);
        const newVal = this.formatValue(change.newValue);
        return `${change.field}: ${oldVal} → ${newVal}`;
      })
      .join('\n');
  }
}


