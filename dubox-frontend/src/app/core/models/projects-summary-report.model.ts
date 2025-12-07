export interface ProjectsSummaryReportQueryParams {
  pageNumber?: number;
  pageSize?: number;
  isActive?: boolean;
  status?: number[];
  search?: string;
}

export interface ProjectsSummaryReportResponse {
  items: ProjectSummaryItem[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  kpis: ProjectsSummaryReportKpis;
  statusDistribution: Record<string, number>;
}

export interface ProjectsSummaryReportKpis {
  totalProjects: number;
  activeProjects: number;
  inactiveProjects: number;
  totalBoxes: number;
  averageProgressPercentage: number;
  averageProgressPercentageFormatted: string;
}

export interface ProjectSummaryItem {
  projectId: string;
  projectCode: string;
  projectName: string;
  clientName?: string;
  location?: string;
  status: string;
  totalBoxes: number;
  progressPercentage: number;
  progressPercentageFormatted: string;
  isActive: boolean;
}

