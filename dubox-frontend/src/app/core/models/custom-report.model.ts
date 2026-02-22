// ─── Config model (mirrors C# CustomReportConfig) ──────────────────────────

export interface CustomReportConfig {
  dataSource: string;
  columns: string[];
  filters: ReportFilterConfig[];
  /** bar | line | pie | table */
  chartType: 'bar' | 'line' | 'pie' | 'table';
  chartXAxis?: string;
  chartYAxis?: string;
  groupBy?: string;
}

export interface ReportFilterConfig {
  field: string;
  /** eq | neq | gt | lt | contains | between */
  operator: string;
  value: any;
  /** second value for the "between" operator */
  value2?: any;
}

// ─── API response DTOs ──────────────────────────────────────────────────────

export interface CustomReportDto {
  id: string;
  name: string;
  description?: string;
  config: CustomReportConfig;
  createdAt: string;
  updatedAt?: string;
}

export interface CustomReportListItemDto {
  id: string;
  name: string;
  description?: string;
  dataSource: string;
  chartType: string;
  columnCount: number;
  createdAt: string;
}

export interface ReportExecutionResult {
  columns: string[];
  columnMeta: ColumnMetadata[];
  rows: Record<string, any>[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  chartData?: ChartDataPoint[];
}

export interface ChartDataPoint {
  label: string;
  value: number;
}

// ─── Data source / column metadata ─────────────────────────────────────────

export interface ColumnMetadata {
  key: string;
  label: string;
  /** string | number | date | status | guid */
  dataType: 'string' | 'number' | 'date' | 'status' | 'guid';
  isFilterable: boolean;
  isGroupable: boolean;
  allowedOperators: string[];
  options?: OptionItem[];
}

export interface OptionItem {
  value: string;
  label: string;
}

export interface DataSourceDefinition {
  key: string;
  label: string;
  columns: ColumnMetadata[];
}

// ─── UI helper types ─────────────────────────────────────────────────────────

export const OPERATOR_LABELS: Record<string, string> = {
  eq: 'equals',
  neq: 'not equals',
  gt: 'greater than',
  lt: 'less than',
  contains: 'contains',
  between: 'between',
};

export const CHART_TYPE_LABELS: Record<string, string> = {
  table: 'Table only',
  bar: 'Bar chart',
  line: 'Line chart',
  pie: 'Pie chart',
};

export const META_FILTER_KEYS = ['projectId', 'teamId', 'search'];

/** Returns only the data-column filter configs (not meta-filters). */
export function isMetaFilter(key: string): boolean {
  return META_FILTER_KEYS.includes(key);
}
