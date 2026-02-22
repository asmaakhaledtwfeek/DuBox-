import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiService } from './api.service';
import {
  CustomReportConfig,
  CustomReportDto,
  CustomReportListItemDto,
  DataSourceDefinition,
  ReportExecutionResult,
} from '../models/custom-report.model';

@Injectable({ providedIn: 'root' })
export class CustomReportsService {
  private readonly endpoint = 'custom-reports';
  private readonly baseUrl = environment.apiUrl;

  constructor(private api: ApiService, private http: HttpClient) {}

  // ─── Data sources ─────────────────────────────────────────────────────────

  getDataSources(): Observable<DataSourceDefinition[]> {
    return this.api.get<DataSourceDefinition[]>(`${this.endpoint}/data-sources`);
  }

  // ─── Filter options ───────────────────────────────────────────────────────

  /** Fetch distinct values for a field — used to populate filter dropdowns. */
  getFilterOptions(dataSource: string, field: string): Observable<string[]> {
    return this.api.get<string[]>(
      `${this.endpoint}/filter-options?dataSource=${encodeURIComponent(dataSource)}&field=${encodeURIComponent(field)}`
    );
  }

  // ─── CRUD ─────────────────────────────────────────────────────────────────

  getAll(search?: string): Observable<CustomReportListItemDto[]> {
    const params: any = {};
    if (search) params['search'] = search;
    return this.api.get<CustomReportListItemDto[]>(this.endpoint, params);
  }

  getById(id: string): Observable<CustomReportDto> {
    return this.api.get<CustomReportDto>(`${this.endpoint}/${id}`);
  }

  create(name: string, description: string | null, config: CustomReportConfig): Observable<CustomReportDto> {
    return this.api.post<CustomReportDto>(this.endpoint, { id: null, name, description, config });
  }

  update(id: string, name: string, description: string | null, config: CustomReportConfig): Observable<CustomReportDto> {
    return this.api.put<CustomReportDto>(`${this.endpoint}/${id}`, { id, name, description, config });
  }

  delete(id: string): Observable<any> {
    return this.api.delete<any>(`${this.endpoint}/${id}`);
  }

  // ─── Execution ────────────────────────────────────────────────────────────

  /** Live preview — run without saving a report. */
  preview(config: CustomReportConfig, page = 1, pageSize = 50): Observable<ReportExecutionResult> {
    return this.api.post<ReportExecutionResult>(`${this.endpoint}/preview`, { config, page, pageSize });
  }

  /** Execute a saved report by id. */
  execute(id: string, page = 1, pageSize = 50): Observable<ReportExecutionResult> {
    return this.api.post<ReportExecutionResult>(
      `${this.endpoint}/${id}/execute?page=${page}&pageSize=${pageSize}`,
      {},
    );
  }

  // ─── Export ───────────────────────────────────────────────────────────────

  /** Export a saved report by its id to Excel (GET → blob). */
  exportById(id: string): Observable<Blob> {
    return this.api.download(`${this.endpoint}/${id}/export`);
  }

  /** Export an inline config to Excel via POST (returns blob). */
  exportInline(config: CustomReportConfig): Observable<Blob> {
    const token = this.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    });
    return this.http.post(`${this.baseUrl}/${this.endpoint}/export`, { config }, {
      headers,
      responseType: 'blob',
    });
  }

  /** Trigger a file download from a Blob. */
  triggerDownload(blob: Blob, fileName: string): void {
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  }

  private getToken(): string | null {
    return localStorage.getItem('token') || localStorage.getItem('auth_token') || sessionStorage.getItem('token');
  }
}
