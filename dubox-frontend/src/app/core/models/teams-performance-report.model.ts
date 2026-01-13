export interface TeamsPerformanceReportQueryParams {
  page?: number;
  pageSize?: number;
  projectId?: string;
  teamId?: string;
  status?: number;
  search?: string;
}

export interface TeamsPerformanceSummary {
  totalTeams: number;
  totalTeamMembers: number;
  totalAssignedActivities: number;
  completedActivities: number;
  inProgressActivities: number;
  delayedActivities: number;
  averageTeamProgress: number;
  teamWorkloadIndicator: number; // activities per team
}

export interface TeamPerformanceItem {
  teamId: string;
  teamCode: string;
  teamName: string;
  membersCount: number;
  totalAssignedActivities: number;
  completed: number;
  inProgress: number;
  pending: number;
  delayed: number;
  averageTeamProgress: number;
  workloadLevel: 'Low' | 'Normal' | 'Overloaded';
}

export interface TeamActivityDetail {
  activityId: string;
  activityName: string;
  boxTag: string;
  projectName: string;
  status: string;
  progressPercentage: number;
  plannedStartDate?: string;
  plannedEndDate?: string;
  actualStartDate?: string;
  actualEndDate?: string;
  duration?: number;
  boxId: string;
  projectId: string;
}

export interface PaginatedTeamsPerformanceResponse {
  items: TeamPerformanceItem[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface TeamActivitiesResponse {
  teamId: string;
  teamName: string;
  activities: TeamActivityDetail[];
  totalCount: number;
}

