import {
  AfterViewInit,
  Component,
  ElementRef,
  NgZone,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Chart, registerables } from 'chart.js';
import { Subject, takeUntil } from 'rxjs';

import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { CustomReportsService } from '../../../core/services/custom-reports.service';
import { ProjectService } from '../../../core/services/project.service';
import { TeamService } from '../../../core/services/team.service';
import { Project } from '../../../core/models/project.model';
import { Team } from '../../../core/models/team.model';
import {
  CHART_TYPE_LABELS,
  ChartDataPoint,
  ColumnMetadata,
  CustomReportConfig,
  DataSourceDefinition,
  OPERATOR_LABELS,
  ReportExecutionResult,
  ReportFilterConfig,
} from '../../../core/models/custom-report.model';

Chart.register(...registerables);

// ─── Step identifiers ────────────────────────────────────────────────────────

type BuilderStep = 'source' | 'columns' | 'filters' | 'chart';

@Component({
  selector: 'app-report-builder',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './report-builder.component.html',
  styleUrls: ['./report-builder.component.scss'],
})
export class ReportBuilderComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('chartCanvas') chartCanvasRef?: ElementRef<HTMLCanvasElement>;
  @ViewChild('previewPanel') previewPanelRef?: ElementRef<HTMLElement>;

  // ─── State ──────────────────────────────────────────────────────────────────
  currentStep: BuilderStep = 'source';
  savedReportId: string | null = null;
  reportName = '';
  reportDescription = '';
  isSaving = false;
  isRunning = false;
  isExporting = false;
  isPdfExporting = false;
  error = '';
  saveError = '';
  saveSuccess = '';
  showSaveDialog = false;

  // ─── Data sources ────────────────────────────────────────────────────────────
  dataSources: DataSourceDefinition[] = [];
  selectedDataSource: DataSourceDefinition | null = null;
  loadingDataSources = false;

  // ─── Columns ─────────────────────────────────────────────────────────────────
  availableColumns: ColumnMetadata[] = [];
  selectedColumnKeys: Set<string> = new Set();
  columnOrder: string[] = [];

  // ─── Filters ─────────────────────────────────────────────────────────────────
  filterRows: ReportFilterConfig[] = [];
  // Context filters (projectId, teamId, search) shown separately
  selectedProjectId = '';
  selectedTeamId = '';
  searchValue = '';
  projects: Project[] = [];
  teams: Team[] = [];

  // ─── Chart ───────────────────────────────────────────────────────────────────
  chartType: 'bar' | 'line' | 'pie' | 'table' = 'table';
  chartXAxis = '';
  chartYAxis = '';
  groupBy = '';
  chartTypeOptions = Object.entries(CHART_TYPE_LABELS).map(([k, v]) => ({ key: k, label: v }));
  private chartInstance: Chart | null = null;

  // ─── Execution results ───────────────────────────────────────────────────────
  executionResult: ReportExecutionResult | null = null;
  currentPage = 1;
  pageSize = 50;

  // ─── Filter dropdown options ─────────────────────────────────────────────────
  /** cache: "dataSource::field" → string[] of distinct values */
  filterOptionsCache = new Map<string, string[]>();
  /** tracks which filter-row indices are currently loading options */
  filterOptionsLoading = new Set<number>();

  // ─── Helpers ─────────────────────────────────────────────────────────────────
  operatorLabels = OPERATOR_LABELS;
  private readonly destroy$ = new Subject<void>();

  constructor(
    private service: CustomReportsService,
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private teamService: TeamService,
    private ngZone: NgZone,
  ) {}

  ngOnInit(): void {
    this.loadDataSources();
    this.loadProjects();
    this.loadTeams();

    // Load existing report if editing
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.savedReportId = id;
      this.loadExistingReport(id);
    }
  }

  ngAfterViewInit(): void {
    // Chart will be drawn after preview runs
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.chartInstance?.destroy();
  }

  // ─── Initialization ──────────────────────────────────────────────────────────

  loadDataSources(): void {
    this.loadingDataSources = true;
    this.service.getDataSources().subscribe({
      next: (sources) => {
        this.dataSources = sources ?? [];
        this.loadingDataSources = false;
      },
      error: () => {
        this.error = 'Could not load data sources.';
        this.loadingDataSources = false;
      },
    });
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe({
      next: (p) => (this.projects = p ?? []),
      error: () => {},
    });
  }

  loadTeams(): void {
    this.teamService.getTeams().subscribe({
      next: (t) => (this.teams = t ?? []),
      error: () => {},
    });
  }

  loadExistingReport(id: string): void {
    this.service.getById(id).subscribe({
      next: (report) => {
        this.reportName = report.name;
        this.reportDescription = report.description ?? '';

        const ds = this.dataSources.find((d) => d.key === report.config.dataSource);
        if (ds) this.selectDataSource(ds);

        // Restore column selection
        report.config.columns.forEach((k) => this.selectedColumnKeys.add(k));
        this.columnOrder = [...report.config.columns];

        // Restore filters
        report.config.filters.forEach((f) => {
          if (f.field === 'projectId') { this.selectedProjectId = f.value; return; }
          if (f.field === 'teamId') { this.selectedTeamId = f.value; return; }
          if (f.field === 'search') { this.searchValue = f.value; return; }
          this.filterRows.push({ ...f });
        });

        // Restore chart config
        this.chartType = (report.config.chartType as any) ?? 'table';
        this.chartXAxis = report.config.chartXAxis ?? '';
        this.chartYAxis = report.config.chartYAxis ?? '';
        this.groupBy = report.config.groupBy ?? '';

        // Auto-run preview
        this.runPreview();
      },
      error: () => (this.error = 'Failed to load report.'),
    });
  }

  // ─── Step navigation ─────────────────────────────────────────────────────────

  get steps(): { id: BuilderStep; label: string; shortLabel: string }[] {
    return [
      { id: 'source',  label: 'Data Source',         shortLabel: 'Source'  },
      { id: 'columns', label: 'Columns',              shortLabel: 'Columns' },
      { id: 'filters', label: 'Filters',              shortLabel: 'Filters' },
      { id: 'chart',   label: 'Chart & Visualization', shortLabel: 'Chart'  },
    ];
  }

  isStepDone(step: BuilderStep): boolean {
    switch (step) {
      case 'source':  return !!this.selectedDataSource;
      case 'columns': return this.selectedColumnCount > 0;
      case 'filters': return this.activeFilterCount > 0;
      case 'chart':   return this.chartType !== 'table';
      default: return false;
    }
  }

  getStepSummary(step: BuilderStep): string {
    switch (step) {
      case 'source':  return this.selectedDataSource ? this.selectedDataSource.label : 'Not selected';
      case 'columns': return this.selectedColumnCount > 0 ? `${this.selectedColumnCount} selected` : 'None selected';
      case 'filters': return this.activeFilterCount > 0 ? `${this.activeFilterCount} active` : 'No filters';
      case 'chart':   return this.chartType !== 'table' ? `${CHART_TYPE_LABELS[this.chartType] ?? this.chartType} chart` : 'Table view';
      default: return '';
    }
  }

  getDataSourceDescription(key: string): string {
    const descs: Record<string, string> = {
      activities:        'Tasks, assignments & activity logs',
      teams_performance: 'Team KPIs and performance metrics',
      boxes:             'Box status, contents & tracking',
      projects:          'Project details and progress',
      quality_issues:    'WIRs, defects & quality records',
    };
    return descs[key] ?? 'Data source';
  }

  getChartTitle(): string {
    if (this.groupBy) {
      const groupLabel = this.getColumnLabel(this.groupBy);
      const valueLabel = this.chartYAxis ? this.getColumnLabel(this.chartYAxis) : 'Count';
      return `${valueLabel} by ${groupLabel}`;
    }
    return 'Chart Preview';
  }

  getChartTypeLabel(key: string): string {
    return CHART_TYPE_LABELS[key as keyof typeof CHART_TYPE_LABELS] ?? key;
  }

  goToStep(step: BuilderStep): void {
    if (step === 'columns' && !this.selectedDataSource) return;
    this.currentStep = step;
    if (step === 'filters') {
      // Eagerly pre-load options for all existing string filter rows
      this.preloadAllFilterOptions();
    }
  }

  // ─── Data source selection ───────────────────────────────────────────────────

  selectDataSource(ds: DataSourceDefinition): void {
    if (this.selectedDataSource?.key === ds.key) return;

    this.selectedDataSource = ds;
    this.availableColumns = ds.columns.filter(
      (c) => !['projectId', 'teamId', 'search'].includes(c.key),
    );

    // Default: select first 5 non-meta columns
    this.selectedColumnKeys = new Set(this.availableColumns.slice(0, 5).map((c) => c.key));
    this.columnOrder = [...this.selectedColumnKeys];

    // Reset the rest
    this.filterRows = [];
    this.groupBy = '';
    this.chartXAxis = '';
    this.chartYAxis = '';
    this.executionResult = null;
    this.chartInstance?.destroy();
    this.chartInstance = null;
  }

  // ─── Column management ───────────────────────────────────────────────────────

  toggleColumn(key: string): void {
    if (this.selectedColumnKeys.has(key)) {
      this.selectedColumnKeys.delete(key);
      this.columnOrder = this.columnOrder.filter((k) => k !== key);
    } else {
      this.selectedColumnKeys.add(key);
      this.columnOrder.push(key);
    }
  }

  selectAllColumns(): void {
    this.availableColumns.forEach((c) => this.selectedColumnKeys.add(c.key));
    this.columnOrder = this.availableColumns.map((c) => c.key);
  }

  clearAllColumns(): void {
    this.selectedColumnKeys.clear();
    this.columnOrder = [];
  }

  moveColumn(fromIdx: number, toIdx: number): void {
    const arr = [...this.columnOrder];
    const [item] = arr.splice(fromIdx, 1);
    arr.splice(toIdx, 0, item);
    this.columnOrder = arr;
  }

  getColumnMeta(key: string): ColumnMetadata | undefined {
    return this.selectedDataSource?.columns.find((c) => c.key === key);
  }

  // ─── Filter management ────────────────────────────────────────────────────────

  addFilter(): void {
    const firstFilterable = this.availableColumns.find((c) => c.isFilterable);
    if (!firstFilterable) return;
    const newIndex = this.filterRows.length;
    this.filterRows.push({
      field: firstFilterable.key,
      operator: firstFilterable.allowedOperators[0] ?? 'eq',
      value: '',
    });
    // Pre-load options for the default selected field
    if (firstFilterable.dataType === 'string') {
      this.loadFilterOptions(newIndex, firstFilterable.key);
    }
  }

  removeFilter(index: number): void {
    this.filterRows.splice(index, 1);
  }

  onFilterFieldChange(index: number): void {
    const col = this.getColumnMeta(this.filterRows[index].field);
    this.filterRows[index].operator = col?.allowedOperators[0] ?? 'eq';
    this.filterRows[index].value = '';
    this.filterRows[index].value2 = undefined;
    // Pre-load dropdown options for string fields
    if (col && col.dataType === 'string' && col.isFilterable) {
      this.loadFilterOptions(index, this.filterRows[index].field);
    }
  }

  /** Returns cached options for a field, or empty array while loading. */
  getFilterOptions(field: string): string[] {
    if (!this.selectedDataSource) return [];
    const key = `${this.selectedDataSource.key}::${field}`;
    return this.filterOptionsCache.get(key) ?? [];
  }

  /** True while fetching options for a filter row. */
  isFilterOptionsLoading(index: number): boolean {
    return this.filterOptionsLoading.has(index);
  }

  /** Load distinct values for a field from the backend and cache them. */
  loadFilterOptions(index: number, field: string): void {
    if (!this.selectedDataSource) return;
    const col = this.getColumnMeta(field);
    // Only fetch for string columns without predefined options
    if (!col || col.dataType !== 'string' || (col.options && col.options.length > 0)) return;

    const key = `${this.selectedDataSource.key}::${field}`;
    if (this.filterOptionsCache.has(key)) return; // already cached
    if (this.filterOptionsLoading.has(index)) return; // in-flight

    this.filterOptionsLoading.add(index);
    this.service.getFilterOptions(this.selectedDataSource.key, field)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (values) => {
          this.filterOptionsCache.set(key, values);
          this.filterOptionsLoading.delete(index);
        },
        error: () => {
          // Cache empty array so UI still shows a dropdown (with just "Any")
          this.filterOptionsCache.set(key, []);
          this.filterOptionsLoading.delete(index);
        },
      });
  }

  /** Pre-load dropdown options for all current string filter rows. */
  preloadAllFilterOptions(): void {
    this.filterRows.forEach((row, i) => {
      const col = this.getColumnMeta(row.field);
      if (col && col.dataType === 'string') {
        this.loadFilterOptions(i, row.field);
      }
    });
  }

  /** Whether a filter field should show a dropdown (has options or loaded values). */
  filterHasOptions(field: string): boolean {
    const col = this.getColumnMeta(field);
    if (!col) return false;
    if (col.options && col.options.length > 0) return true;
    if (!this.selectedDataSource) return false;
    const key = `${this.selectedDataSource.key}::${field}`;
    const cached = this.filterOptionsCache.get(key);
    return !!(cached && cached.length > 0);
  }

  getFilterableColumns(): ColumnMetadata[] {
    return this.availableColumns.filter((c) => c.isFilterable);
  }

  hasProjectFilter(): boolean {
    return !!this.selectedDataSource?.columns.find((c) => c.key === 'projectId');
  }

  hasTeamFilter(): boolean {
    return !!this.selectedDataSource?.columns.find((c) => c.key === 'teamId');
  }

  hasSearchFilter(): boolean {
    return !!this.selectedDataSource?.columns.find((c) => c.key === 'search');
  }

  // ─── Groupable columns for chart ─────────────────────────────────────────────

  setChartType(key: string): void {
    this.chartType = key as 'bar' | 'line' | 'pie' | 'table';
  }

  getGroupableColumns(): ColumnMetadata[] {
    return this.availableColumns.filter((c) => c.isGroupable && this.selectedColumnKeys.has(c.key));
  }

  getNumericColumns(): ColumnMetadata[] {
    return this.availableColumns.filter(
      (c) => c.dataType === 'number' && this.selectedColumnKeys.has(c.key),
    );
  }

  // ─── Preview / Execution ─────────────────────────────────────────────────────

  runPreview(): void {
    if (!this.selectedDataSource || this.selectedColumnKeys.size === 0) return;
    this.isRunning = true;
    this.error = '';

    this.service.preview(this.buildConfig(), this.currentPage, this.pageSize).subscribe({
      next: (result) => {
        this.executionResult = result;
        this.isRunning = false;
        // Give Angular one extra tick to show/hide the canvas before Chart.js measures it
        setTimeout(() => this.renderChart(result), 100);
      },
      error: (err) => {
        this.error = 'Preview failed. Please check your filters and try again.';
        this.isRunning = false;
      },
    });
  }

  changePage(page: number): void {
    this.currentPage = page;
    this.runPreview();
  }

  // ─── Chart rendering ─────────────────────────────────────────────────────────

  renderChart(result: ReportExecutionResult): void {
    if (this.chartType === 'table' || !result.chartData?.length) return;
    if (!this.chartCanvasRef?.nativeElement) return;

    // Destroy previous instance before creating a new one
    if (this.chartInstance) {
      this.chartInstance.destroy();
      this.chartInstance = null;
    }

    const labels = result.chartData.map((p) => p.label);
    const values = result.chartData.map((p) => p.value);
    const colors = this.generateColors(labels.length);
    const canvas = this.chartCanvasRef.nativeElement;

    // Set explicit pixel dimensions so Chart.js doesn't need to observe the container
    canvas.width = canvas.parentElement?.clientWidth || 600;
    canvas.height = 280;

    const ctx = canvas.getContext('2d')!;
    const chartType = this.chartType;

    // Run Chart.js entirely outside Angular zone to prevent change-detection loops
    this.ngZone.runOutsideAngular(() => {
      this.chartInstance = new Chart(ctx, {
        type: chartType === 'pie' ? 'pie' : chartType === 'line' ? 'line' : 'bar',
        data: {
          labels,
          datasets: [
            {
              label: this.getColumnLabel(this.chartYAxis || this.groupBy),
              data: values,
              backgroundColor: chartType === 'line' ? 'rgba(59,130,246,0.15)' : colors,
              borderColor: chartType === 'line' ? '#3b82f6' : colors,
              borderWidth: chartType === 'line' ? 2 : 1,
              fill: chartType === 'line',
              tension: 0.4,
            },
          ],
        },
        options: {
          responsive: false,       // use explicit canvas pixel size — avoids resize loops
          animation: false,        // no animation to prevent re-render cascades
          plugins: {
            legend: {
              display: chartType === 'pie',
              position: 'right',
            },
          },
          scales: chartType === 'pie'
            ? {}
            : {
                x: { grid: { display: false } },
                y: { beginAtZero: true },
              },
        },
      });
    });
  }

  private generateColors(count: number): string[] {
    const palette = [
      '#3b82f6', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6',
      '#ec4899', '#06b6d4', '#84cc16', '#f97316', '#6366f1',
    ];
    return Array.from({ length: count }, (_, i) => palette[i % palette.length]);
  }

  getColumnLabel(key: string): string {
    // First check execution result column meta (most up-to-date)
    const fromResult = this.executionResult?.columnMeta?.find((c) => c.key === key);
    if (fromResult) return fromResult.label;
    return this.getColumnMeta(key)?.label ?? key;
  }

  // ─── Build config ─────────────────────────────────────────────────────────────

  buildConfig(): CustomReportConfig {
    const filters: ReportFilterConfig[] = [];

    if (this.selectedProjectId) {
      filters.push({ field: 'projectId', operator: 'eq', value: this.selectedProjectId });
    }
    if (this.selectedTeamId) {
      filters.push({ field: 'teamId', operator: 'eq', value: this.selectedTeamId });
    }
    if (this.searchValue.trim()) {
      filters.push({ field: 'search', operator: 'contains', value: this.searchValue.trim() });
    }

    this.filterRows
      .filter((f) => f.value !== null && f.value !== '')
      .forEach((f) => filters.push(f));

    return {
      dataSource: this.selectedDataSource!.key,
      columns: this.columnOrder.filter((k) => this.selectedColumnKeys.has(k)),
      filters,
      chartType: this.chartType,
      chartXAxis: this.chartXAxis || undefined,
      chartYAxis: this.chartYAxis || undefined,
      groupBy: this.groupBy || undefined,
    };
  }

  // ─── Save ─────────────────────────────────────────────────────────────────────

  openSaveDialog(): void {
    this.saveError = '';
    this.saveSuccess = '';
    this.showSaveDialog = true;
  }

  closeSaveDialog(): void {
    this.showSaveDialog = false;
  }

  saveReport(): void {
    if (!this.reportName.trim()) {
      this.saveError = 'Report name is required.';
      return;
    }
    if (!this.selectedDataSource) {
      this.saveError = 'Please select a data source first.';
      return;
    }

    this.isSaving = true;
    this.saveError = '';
    const config = this.buildConfig();

    const save$ = this.savedReportId
      ? this.service.update(this.savedReportId, this.reportName, this.reportDescription || null, config)
      : this.service.create(this.reportName, this.reportDescription || null, config);

    save$.subscribe({
      next: (saved) => {
        this.savedReportId = saved.id;
        this.saveSuccess = 'Report saved successfully!';
        this.isSaving = false;
        setTimeout(() => {
          this.showSaveDialog = false;
          this.saveSuccess = '';
        }, 1500);
      },
      error: () => {
        this.saveError = 'Failed to save report. Please try again.';
        this.isSaving = false;
      },
    });
  }

  // ─── Export ─────────────────────────────────────────────────────────────────

  exportReport(): void {
    if (!this.selectedDataSource) return;
    this.isExporting = true;
    const config = this.buildConfig();
    const name = this.reportName || this.selectedDataSource.label;

    this.service.exportInline(config).subscribe({
      next: (blob) => {
        this.service.triggerDownload(blob, `${name}_${new Date().toISOString().slice(0, 10)}.xlsx`);
        this.isExporting = false;
      },
      error: () => {
        this.error = 'Export failed. Please try again.';
        this.isExporting = false;
      },
    });
  }

  // ─── PDF Export ──────────────────────────────────────────────────────────────

  async exportPdf(): Promise<void> {
    if (!this.executionResult || !this.selectedDataSource) return;
    this.isPdfExporting = true;

    try {
      const pdf   = new jsPDF({ orientation: 'landscape', unit: 'mm', format: 'a4' });
      const pageW = pdf.internal.pageSize.getWidth();
      const pageH = pdf.internal.pageSize.getHeight();
      const margin = 14;
      let   curY  = margin;

      // ── Header bar ──────────────────────────────────────────────────────────
      pdf.setFillColor(27, 154, 170);
      pdf.rect(0, 0, pageW, 18, 'F');
      pdf.setFont('helvetica', 'bold');
      pdf.setFontSize(13);
      pdf.setTextColor(255, 255, 255);
      pdf.text(this.reportName || this.selectedDataSource.label, margin, 12);

      // Timestamp on the right
      pdf.setFontSize(8);
      pdf.setFont('helvetica', 'normal');
      const ts = new Date().toLocaleString();
      pdf.text(ts, pageW - margin, 12, { align: 'right' });

      curY = 26;

      // ── Metadata row ────────────────────────────────────────────────────────
      pdf.setFontSize(8);
      pdf.setTextColor(100, 100, 100);
      const meta = [
        `Data source: ${this.selectedDataSource.label}`,
        `Rows: ${this.executionResult.totalCount.toLocaleString()}`,
        `Columns: ${this.selectedColumnCount}`,
        this.chartType !== 'table' ? `Chart: ${this.chartType}` : '',
      ].filter(Boolean).join('   •   ');
      pdf.text(meta, margin, curY);
      curY += 7;

      // ── Chart (if rendered) ──────────────────────────────────────────────────
      const canvas = this.chartCanvasRef?.nativeElement;
      if (canvas && this.chartType !== 'table' && this.executionResult.chartData?.length) {
        try {
          const chartImg = await html2canvas(canvas, { scale: 2, backgroundColor: '#ffffff', logging: false });
          const imgData  = chartImg.toDataURL('image/png');
          const maxW     = pageW - margin * 2;
          const ratio    = chartImg.height / chartImg.width;
          const imgW     = Math.min(maxW, 160);
          const imgH     = imgW * ratio;
          const imgX     = (pageW - imgW) / 2;
          pdf.addImage(imgData, 'PNG', imgX, curY, imgW, imgH);
          curY += imgH + 8;
        } catch {
          // chart capture failed — skip silently
        }
      }

      // ── Separator ───────────────────────────────────────────────────────────
      pdf.setDrawColor(200, 200, 200);
      pdf.line(margin, curY, pageW - margin, curY);
      curY += 5;

      // ── Table ───────────────────────────────────────────────────────────────
      const cols      = this.executionResult.columns;
      const colMetas  = this.executionResult.columnMeta;
      const rows      = this.executionResult.rows;
      const colW      = Math.min(40, (pageW - margin * 2) / Math.max(cols.length, 1));
      const rowH      = 6;
      const headerH   = 8;

      // Table header
      pdf.setFillColor(27, 154, 170);
      pdf.rect(margin, curY, colW * cols.length, headerH, 'F');
      pdf.setFont('helvetica', 'bold');
      pdf.setFontSize(7);
      pdf.setTextColor(255, 255, 255);
      cols.forEach((key, ci) => {
        const label = colMetas.find(m => m.key === key)?.label ?? key;
        pdf.text(label, margin + ci * colW + 2, curY + 5.5, { maxWidth: colW - 3 });
      });
      curY += headerH;

      // Table rows
      pdf.setFont('helvetica', 'normal');
      pdf.setFontSize(6.5);
      rows.forEach((row, ri) => {
        if (curY + rowH > pageH - 10) {
          pdf.addPage();
          curY = margin;
        }
        // Alternate row background
        if (ri % 2 === 0) {
          pdf.setFillColor(240, 249, 250);
          pdf.rect(margin, curY, colW * cols.length, rowH, 'F');
        }
        pdf.setTextColor(40, 40, 40);
        cols.forEach((key, ci) => {
          const val   = row[key];
          const label = this.formatCellValue(val);
          pdf.text(label, margin + ci * colW + 2, curY + 4, { maxWidth: colW - 3 });
        });
        // Row bottom border
        pdf.setDrawColor(220, 220, 220);
        pdf.line(margin, curY + rowH, margin + colW * cols.length, curY + rowH);
        curY += rowH;
      });

      // ── Footer on each page ─────────────────────────────────────────────────
      const totalPages = (pdf as any).internal.getNumberOfPages();
      for (let p = 1; p <= totalPages; p++) {
        pdf.setPage(p);
        pdf.setFontSize(7);
        pdf.setTextColor(160, 160, 160);
        pdf.text(`Page ${p} of ${totalPages}`, pageW / 2, pageH - 5, { align: 'center' });
        pdf.text('Generated by DUBOX', pageW - margin, pageH - 5, { align: 'right' });
      }

      const fileName = `${this.reportName || this.selectedDataSource.label}_${new Date().toISOString().slice(0, 10)}.pdf`;
      pdf.save(fileName);
    } finally {
      this.isPdfExporting = false;
    }
  }

  // ─── Pagination helpers ──────────────────────────────────────────────────────

  getPageNumbers(): number[] {
    if (!this.executionResult) return [];
    const total = this.executionResult.totalPages;
    const pages: number[] = [];
    for (let i = 1; i <= Math.min(total, 7); i++) pages.push(i);
    return pages;
  }

  // ─── Row value display ────────────────────────────────────────────────────────

  formatCellValue(value: any): string {
    if (value == null) return '—';
    if (value instanceof Date || (typeof value === 'string' && /^\d{4}-\d{2}-\d{2}/.test(value))) {
      const d = new Date(value);
      if (!isNaN(d.getTime())) return d.toLocaleDateString();
    }
    if (typeof value === 'number') return value.toLocaleString();
    return String(value);
  }

  isNumericValue(value: any): boolean {
    return typeof value === 'number';
  }

  // ─── UI labels ────────────────────────────────────────────────────────────────

  getStepLabel(step: BuilderStep): string {
    const labels: Record<BuilderStep, string> = {
      source: 'Data Source',
      columns: 'Columns',
      filters: 'Filters',
      chart: 'Chart & Visualization',
    };
    return labels[step];
  }

  get selectedColumnCount(): number {
    return this.selectedColumnKeys.size;
  }

  get activeFilterCount(): number {
    let count = this.filterRows.filter((f) => f.value !== '' && f.value != null).length;
    if (this.selectedProjectId) count++;
    if (this.selectedTeamId) count++;
    if (this.searchValue) count++;
    return count;
  }
}
