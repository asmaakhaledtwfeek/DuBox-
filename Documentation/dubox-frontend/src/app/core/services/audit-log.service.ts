import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService, PaginatedResponse } from './api.service';
import { AuditLog, AuditLogQueryParams, PaginatedAuditLogsResponse } from '../models/audit-log.model';

@Injectable({
  providedIn: 'root'
})
export class AuditLogService {
  private readonly endpoint = 'AuditLogs';

  constructor(private apiService: ApiService) {}

  getAuditLogs(params?: AuditLogQueryParams): Observable<PaginatedAuditLogsResponse> {
    // Ensure pagination parameters have defaults
    const queryParams: AuditLogQueryParams = {
      pageNumber: 1,
      pageSize: 25,
      ...params
    };

    return this.apiService.get<PaginatedResponse<AuditLog>>(this.endpoint, queryParams).pipe(
      map(response => {
        // Backend returns camelCase (configured in Program.cs)
        // Use type assertion to handle any case variations
        const responseAny = response as any;
        const items = responseAny.items || responseAny.Items || [];
        const totalCount = responseAny.totalCount ?? responseAny.TotalCount ?? 0;
        const pageNumber = responseAny.pageNumber ?? responseAny.PageNumber ?? 1;
        const pageSize = responseAny.pageSize ?? responseAny.PageSize ?? 25;
        const totalPages = responseAny.totalPages ?? responseAny.TotalPages ?? 0;

        return {
          items: items.map((log: any) => this.transformAuditLog(log)),
          totalCount,
          pageNumber,
          pageSize,
          totalPages
        };
      })
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

