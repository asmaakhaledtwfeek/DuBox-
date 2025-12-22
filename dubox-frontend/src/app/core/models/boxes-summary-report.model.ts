/**
 * Models for Boxes Summary Report
 */

export interface BoxSummaryReportItem {
  boxId: string;
  projectId: string;
  projectCode: string;
  projectName: string;
  boxTag: string;
  serialNumber?: string;
  boxName?: string;
  boxType: string;
  floor?: string;
  buildingNumber?: string;
  boxFunction?: string;
  zone?: string;
  progressPercentage: number;
  progressPercentageFormatted: string;
  status: string;
  currentLocationId?: string;
  currentLocationName?: string;
  plannedStartDate?: Date;
  plannedEndDate?: Date;
  actualStartDate?: Date;
  actualEndDate?: Date;
  duration?: number;
  lastUpdateDate?: Date;
  activitiesCount: number;
  assetsCount: number;
  qualityIssuesCount: number;
}

export interface BoxSummaryReportKpis {
  totalBoxes: number;
  inProgressCount: number;
  completedCount: number;
  notStartedCount: number;
  averageProgress: number;
  averageProgressFormatted: string;
}

export interface BoxSummaryReportAggregations {
  statusDistribution: Record<string, number>;
  progressRangeDistribution: Record<string, number>;
  topProjects: ProjectBoxCount[];
}

export interface ProjectBoxCount {
  projectId: string;
  projectCode: string;
  projectName: string;
  boxCount: number;
}

export interface PaginatedBoxSummaryReportResponse {
  items: BoxSummaryReportItem[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  kpis: BoxSummaryReportKpis;
  aggregations: BoxSummaryReportAggregations;
}

export interface BoxSummaryReportQueryParams {
  pageNumber?: number;
  pageSize?: number;
  sortBy?: string;
  sortDir?: string;
  projectId?: string;
  boxType?: string[];
  floor?: string;
  buildingNumber?: string;
  zone?: string;
  status?: number[];
  progressMin?: number;
  progressMax?: number;
  search?: string;
  dateFrom?: string;
  dateTo?: string;
  dateFilterType?: string;
}

