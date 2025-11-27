import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import { AuditLog, AuditLogQueryParams } from '../models/audit-log.model';

@Injectable({
  providedIn: 'root'
})
export class AuditLogService {
  private readonly endpoint = 'AuditLogs';

  constructor(private apiService: ApiService) {}

  getAuditLogs(params?: AuditLogQueryParams): Observable<AuditLog[]> {
    return this.apiService.get<AuditLog[]>(this.endpoint, params).pipe(
      map(logs => logs.map(log => this.transformAuditLog(log)))
    );
  }

  private transformAuditLog(log: any): AuditLog {
    return {
      auditLogId: log.auditLogId || log.AuditLogId || log.auditId || log.AuditId,
      tableName: log.tableName || log.TableName || '',
      recordId: log.recordId || log.RecordId,
      entityDisplayName: log.entityDisplayName || log.EntityDisplayName || null,
      action: log.action || log.Action || '',
      description: log.description || log.Description || '',
      timestamp: log.timestamp ? new Date(log.timestamp) : (log.Timestamp ? new Date(log.Timestamp) : new Date()),
      oldValues: log.oldValues || log.OldValues || null,
      newValues: log.newValues || log.NewValues || null,
      changes: (log.changes || log.Changes || []).map((c: any) => ({
        field: c.field || c.Field || '',
        oldValue: c.oldValue || c.OldValue || null,
        newValue: c.newValue || c.NewValue || null
      })),
      changedBy: log.changedBy || log.ChangedBy || null,
      changedByUsername: log.changedByUsername || log.ChangedByUsername || null,
      changedByFullName: log.changedByFullName || log.ChangedByFullName || null
    };
  }
}

