export interface AuditLog {
  auditLogId: string;
  tableName: string;
  recordId: string;
  entityDisplayName: string | null;
  action: string;
  description: string;
  timestamp: Date;
  oldValues: string | null;
  newValues: string | null;
  changes: FieldChange[];
  changedBy: string | null;
  changedByUsername: string | null;
  changedByFullName?: string | null;
}

export interface FieldChange {
  field: string;
  oldValue: string | null;
  newValue: string | null;
}

export interface AuditLogQueryParams {
  tableName?: string;
  recordId?: string;
  action?: string;
  searchTerm?: string;
  fromDate?: string;
  toDate?: string;
}

export type AuditAction = 'INSERT' | 'UPDATE' | 'DELETE';

