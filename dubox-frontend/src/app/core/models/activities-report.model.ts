export interface ActivitiesReportQueryParams {
  page?: number;
  pageSize?: number;
  projectId?: string;
  teamId?: string;
  status?: number;
  plannedStartDateFrom?: string;
  plannedStartDateTo?: string;
  plannedEndDateFrom?: string;
  plannedEndDateTo?: string;
  search?: string;
}

export interface ReportActivity {
  activityId: string;
  activityName: string;
  boxTag: string;
  projectName: string;
  assignedTeam?: string;
  status: string;
  progressPercentage: number;
  plannedStartDate?: string;
  plannedEndDate?: string;
  actualStartDate?: string;
  actualEndDate?: string;
  actualDuration?: number;
  delayDays?: number;
  boxId: string;
  projectId: string;
}

export interface PaginatedActivitiesReportResponse {
  items: ReportActivity[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface ActivitiesSummary {
  totalActivities: number;
  completed: number;
  inProgress: number;
  pending: number;
  delayed: number;
  averageProgress: number;
  overdue: number;
  dueThisWeek: number;
}

