import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import { 
  PaginatedBoxSummaryReportResponse, 
  BoxSummaryReportQueryParams,
  BoxSummaryReportItem 
} from '../models/boxes-summary-report.model';
import {
  ProjectsSummaryReportResponse,
  ProjectsSummaryReportQueryParams
} from '../models/projects-summary-report.model';
import {
  PaginatedActivitiesReportResponse,
  ActivitiesReportQueryParams,
  ActivitiesSummary
} from '../models/activities-report.model';
import {
  PaginatedTeamsPerformanceResponse,
  TeamsPerformanceReportQueryParams,
  TeamsPerformanceSummary,
  TeamActivitiesResponse
} from '../models/teams-performance-report.model';

export interface BoxProgressData {
  buildingNumber: string;
  projectId: string;
  nonAssembled: number;
  backing: number;
  released1stFix: number;
  released2ndFix: number;
  released3rdFix: number;
  total: number;
}


export interface ReportSummary {
  totalBoxes: number;
  averageProgress: number;
  pendingActivities: number;
  activeTeams: number;
}

export interface MissingMaterialsData {
  materialName: string;
  materialCode: string;
  requiredQuantity: number;
  availableQuantity: number;
  shortageQuantity: number;
  affectedBoxes: number;
  unit: string;
  expectedDeliveryDate?: Date;
}

export interface PhaseReadinessData {
  phaseName: string;
  totalBoxes: number;
  readyBoxes: number;
  pendingBoxes: number;
  readinessPercentage: number;
  blockingIssues: string[];
}

@Injectable({
  providedIn: 'root'
})
export class ReportsService {
  private readonly endpoint = 'reports';

  constructor(private apiService: ApiService) {}

  /**
   * Get box progress report data
   * Groups boxes by building number and phase status
   */
  getBoxProgressReport(projectId?: string): Observable<BoxProgressData[]> {
    const params = projectId ? { projectId } : {};
    return this.apiService.get<any[]>(`${this.endpoint}/box-progress`, params).pipe(
      map(data => this.transformBoxProgressData(data))
    );
  }


  /**
   * Get report summary statistics
   */
  getReportSummary(projectId?: string): Observable<ReportSummary> {
    const params = projectId ? { projectId } : {};
    return this.apiService.get<any>(`${this.endpoint}/summary`, params).pipe(
      map(data => this.transformReportSummary(data))
    );
  }

  /**
   * Get missing materials report
   */
  getMissingMaterialsReport(projectId?: string): Observable<MissingMaterialsData[]> {
    const params = projectId ? { projectId } : {};
    return this.apiService.get<any[]>(`${this.endpoint}/missing-materials`, params).pipe(
      map(data => this.transformMissingMaterialsData(data))
    );
  }

  /**
   * Get phase readiness report
   */
  getPhaseReadinessReport(projectId?: string): Observable<PhaseReadinessData[]> {
    const params = projectId ? { projectId } : {};
    return this.apiService.get<any[]>(`${this.endpoint}/phase-readiness`, params).pipe(
      map(data => this.transformPhaseReadinessData(data))
    );
  }

  /**
   * Get box progress data directly from boxes endpoint
   * Fallback method if reports endpoint not available
   */
  getBoxProgressFromBoxes(projectId?: string): Observable<BoxProgressData[]> {
    const params = projectId ? { projectId } : {};
    return this.apiService.get<any[]>('boxes', params).pipe(
      map(boxes => this.calculateBoxProgressFromRawData(boxes))
    );
  }


  /**
   * Transform backend box progress data to frontend model
   */
  private transformBoxProgressData(backendData: any[]): BoxProgressData[] {
    return (backendData || []).map(item => ({
      buildingNumber: item.buildingNumber || item.BuildingNumber || '',
      projectId: item.projectId || item.ProjectId || '',
      nonAssembled: item.nonAssembled || item.NonAssembled || 0,
      backing: item.backing || item.Backing || item.dueBacking || item.DueBacking || 0,
      released1stFix: item.released1stFix || item.Released1stFix || item.firstFix || 0,
      released2ndFix: item.released2ndFix || item.Released2ndFix || item.secondFix || 0,
      released3rdFix: item.released3rdFix || item.Released3rdFix || item.thirdFix || 0,
      total: item.total || item.Total || item.totalBoxes || item.TotalBoxes || 0
    }));
  }


  /**
   * Transform report summary data
   */
  private transformReportSummary(backendData: any): ReportSummary {
    return {
      totalBoxes: backendData?.totalBoxes || backendData?.TotalBoxes || 0,
      averageProgress: backendData?.averageProgress || backendData?.AverageProgress || 0,
      pendingActivities: backendData?.pendingActivities || backendData?.PendingActivities || 0,
      activeTeams: backendData?.activeTeams || backendData?.ActiveTeams || 0
    };
  }

  /**
   * Transform missing materials data
   */
  private transformMissingMaterialsData(backendData: any[]): MissingMaterialsData[] {
    return (backendData || []).map(item => ({
      materialName: item.materialName || item.MaterialName || '',
      materialCode: item.materialCode || item.MaterialCode || '',
      requiredQuantity: item.requiredQuantity || item.RequiredQuantity || 0,
      availableQuantity: item.availableQuantity || item.AvailableQuantity || 0,
      shortageQuantity: item.shortageQuantity || item.ShortageQuantity || 0,
      affectedBoxes: item.affectedBoxes || item.AffectedBoxes || 0,
      unit: item.unit || item.Unit || '',
      expectedDeliveryDate: item.expectedDeliveryDate ? new Date(item.expectedDeliveryDate) : undefined
    }));
  }

  /**
   * Transform phase readiness data
   */
  private transformPhaseReadinessData(backendData: any[]): PhaseReadinessData[] {
    return (backendData || []).map(item => ({
      phaseName: item.phaseName || item.PhaseName || '',
      totalBoxes: item.totalBoxes || item.TotalBoxes || 0,
      readyBoxes: item.readyBoxes || item.ReadyBoxes || 0,
      pendingBoxes: item.pendingBoxes || item.PendingBoxes || 0,
      readinessPercentage: item.readinessPercentage || item.ReadinessPercentage || 0,
      blockingIssues: item.blockingIssues || item.BlockingIssues || []
    }));
  }

  /**
   * Calculate box progress from raw box data
   * Groups boxes by building number and calculates phase distribution
   */
  private calculateBoxProgressFromRawData(boxes: any[]): BoxProgressData[] {
    if (!boxes || boxes.length === 0) return [];

    // Group boxes by building number
    const buildingGroups = new Map<string, any[]>();
    
    boxes.forEach(box => {
      const buildingNumber = box.buildingNumber || box.BuildingNumber || 'Unknown';
      if (!buildingGroups.has(buildingNumber)) {
        buildingGroups.set(buildingNumber, []);
      }
      buildingGroups.get(buildingNumber)!.push(box);
    });

    // Calculate statistics for each building number
    const result: BoxProgressData[] = [];
    buildingGroups.forEach((boxList, buildingNumber) => {
      const stats: BoxProgressData = {
        buildingNumber,
        projectId: boxList[0]?.projectId || '',
        nonAssembled: 0,
        backing: 0,
        released1stFix: 0,
        released2ndFix: 0,
        released3rdFix: 0,
        total: boxList.length
      };

      boxList.forEach(box => {
        const status = (box.status || box.Status || '').toString().toLowerCase();
        const progress = box.progressPercentage || box.progress || 0;
        
        // Classify based on status and progress
        if (status === 'notstarted' || progress === 0) {
          stats.nonAssembled++;
        } else if (status === 'onhold' || status === 'delayed') {
          stats.backing++;
        } else if (progress > 0 && progress < 33) {
          stats.released1stFix++;
        } else if (progress >= 33 && progress < 66) {
          stats.released2ndFix++;
        } else if (progress >= 66) {
          stats.released3rdFix++;
        }
      });

      result.push(stats);
    });

    return result;
  }


  /**
   * Get projects summary report - Aggregated information about all projects
   */
  getProjectsSummaryReport(params?: ProjectsSummaryReportQueryParams): Observable<ProjectsSummaryReportResponse> {
    const queryParams: any = {};
    
    if (params?.pageNumber !== undefined) queryParams.pageNumber = params.pageNumber;
    if (params?.pageSize !== undefined) queryParams.pageSize = params.pageSize;
    if (params?.isActive !== undefined) queryParams.isActive = params.isActive;
    if (params?.status && params.status.length > 0) queryParams.status = params.status;
    if (params?.search) queryParams.search = params.search;

    return this.apiService.get<any>(`${this.endpoint}/projects`, queryParams).pipe(
      map(response => this.transformProjectsSummaryReport(response))
    );
  }

  /**
   * Transform backend projects summary report response to frontend model
   */
  private transformProjectsSummaryReport(backendData: any): ProjectsSummaryReportResponse {
    const data = backendData.data || backendData.Data || backendData;
    
    return {
      items: (data.items || data.Items || data.projects || data.Projects || []).map((p: any) => ({
        projectId: p.projectId || p.ProjectId || '',
        projectCode: p.projectCode || p.ProjectCode || '',
        projectName: p.projectName || p.ProjectName || '',
        clientName: p.clientName || p.ClientName,
        location: p.location || p.Location,
        status: p.status || p.Status || '',
        totalBoxes: p.totalBoxes ?? p.TotalBoxes ?? 0,
        progressPercentage: p.progressPercentage ?? p.ProgressPercentage ?? 0,
        progressPercentageFormatted: p.progressPercentageFormatted || p.ProgressPercentageFormatted || '0.00%',
        isActive: p.isActive ?? p.IsActive ?? false
      })),
      totalCount: data.totalCount ?? data.TotalCount ?? 0,
      pageNumber: data.pageNumber ?? data.PageNumber ?? 1,
      pageSize: data.pageSize ?? data.PageSize ?? 25,
      totalPages: data.totalPages ?? data.TotalPages ?? 0,
      kpis: {
        totalProjects: data.kpis?.totalProjects ?? data.kpis?.TotalProjects ?? 0,
        activeProjects: data.kpis?.activeProjects ?? data.kpis?.ActiveProjects ?? 0,
        inactiveProjects: data.kpis?.inactiveProjects ?? data.kpis?.InactiveProjects ?? 0,
        onHoldProjects: data.kpis?.onHoldProjects ?? data.kpis?.OnHoldProjects ?? 0,
        completedProjects: data.kpis?.completedProjects ?? data.kpis?.CompletedProjects ?? 0,
        archivedProjects: data.kpis?.archivedProjects ?? data.kpis?.ArchivedProjects ?? 0,
        closedProjects: data.kpis?.closedProjects ?? data.kpis?.ClosedProjects ?? 0,
        totalBoxes: data.kpis?.totalBoxes ?? data.kpis?.TotalBoxes ?? 0,
        averageProgressPercentage: data.kpis?.averageProgressPercentage ?? data.kpis?.AverageProgressPercentage ?? 0,
        averageProgressPercentageFormatted: data.kpis?.averageProgressPercentageFormatted || data.kpis?.AverageProgressPercentageFormatted || '0.00%'
      },
      statusDistribution: data.statusDistribution || data.StatusDistribution || {}
    };
  }

  /**
   * Get boxes summary report with filtering, pagination, sorting, KPIs, and charts
   */
  getBoxesSummaryReport(params: BoxSummaryReportQueryParams): Observable<PaginatedBoxSummaryReportResponse> {
    // Build query string from params
    const queryParams: any = {};
    
    if (params.pageNumber !== undefined) queryParams.pageNumber = params.pageNumber;
    if (params.pageSize !== undefined) queryParams.pageSize = params.pageSize;
    if (params.sortBy) queryParams.sortBy = params.sortBy;
    if (params.sortDir) queryParams.sortDir = params.sortDir;
    if (params.projectId) queryParams.projectId = params.projectId;
    if (params.floor) queryParams.floor = params.floor;
    if (params.buildingNumber) queryParams.buildingNumber = params.buildingNumber;
    if (params.zone) queryParams.zone = params.zone;
    if (params.progressMin !== undefined) queryParams.progressMin = params.progressMin;
    if (params.progressMax !== undefined) queryParams.progressMax = params.progressMax;
    if (params.search) queryParams.search = params.search;
    if (params.dateFrom) queryParams.dateFrom = params.dateFrom;
    if (params.dateTo) queryParams.dateTo = params.dateTo;
    if (params.dateFilterType) queryParams.dateFilterType = params.dateFilterType;

    // Handle arrays - backend expects multiple query params
    if (params.boxType && params.boxType.length > 0) {
      queryParams.boxType = params.boxType;
    }
    if (params.status && params.status.length > 0) {
      queryParams.status = params.status;
    }

    return this.apiService.get<any>(`${this.endpoint}/boxes`, queryParams).pipe(
      map(response => this.transformBoxesSummaryReport(response))
    );
  }

  /**
   * Transform backend boxes summary report response to frontend model
   */
  private transformBoxesSummaryReport(backendData: any): PaginatedBoxSummaryReportResponse {
    const data = backendData.data || backendData.Data || backendData;
    
    return {
      items: (data.items || data.Items || []).map((item: any) => this.transformBoxSummaryItem(item)),
      totalCount: data.totalCount ?? data.TotalCount ?? 0,
      pageNumber: data.pageNumber ?? data.PageNumber ?? 1,
      pageSize: data.pageSize ?? data.PageSize ?? 25,
      totalPages: data.totalPages ?? data.TotalPages ?? 0,
      kpis: this.transformKpis(data.kpis || data.Kpis || {}),
      aggregations: this.transformAggregations(data.aggregations || data.Aggregations || {})
    };
  }

  private transformBoxSummaryItem(item: any): BoxSummaryReportItem {
    return {
      boxId: item.boxId || item.BoxId || '',
      projectId: item.projectId || item.ProjectId || '',
      projectCode: item.projectCode || item.ProjectCode || '',
      projectName: item.projectName || item.ProjectName || '',
      projectStatus: item.projectStatus || item.ProjectStatus,
      boxTag: item.boxTag || item.BoxTag || '',
      serialNumber: item.serialNumber || item.SerialNumber,
      boxName: item.boxName || item.BoxName,
      boxType: item.boxType || item.BoxType || '',
      boxTypeName: item.boxTypeName || item.BoxTypeName,
      boxSubTypeName: item.boxSubTypeName || item.BoxSubTypeName,
      floor: item.floor || item.Floor,
      buildingNumber: item.buildingNumber || item.BuildingNumber,
      boxFunction: item.boxFunction || item.BoxFunction,
      zone: item.zone || item.Zone,
      progressPercentage: item.progressPercentage ?? item.ProgressPercentage ?? 0,
      progressPercentageFormatted: item.progressPercentageFormatted || item.ProgressPercentageFormatted || '0.00%',
      status: item.status || item.Status || '',
      currentLocationId: item.currentLocationId || item.CurrentLocationId,
      currentLocationName: item.currentLocationName || item.CurrentLocationName,
      factoryName: item.factoryName || item.FactoryName,
      factoryPosition: item.factoryPosition || item.FactoryPosition,
      plannedStartDate: item.plannedStartDate ? new Date(item.plannedStartDate) : (item.PlannedStartDate ? new Date(item.PlannedStartDate) : undefined),
      plannedEndDate: item.plannedEndDate ? new Date(item.plannedEndDate) : (item.PlannedEndDate ? new Date(item.PlannedEndDate) : undefined),
      actualStartDate: item.actualStartDate ? new Date(item.actualStartDate) : (item.ActualStartDate ? new Date(item.ActualStartDate) : undefined),
      actualEndDate: item.actualEndDate ? new Date(item.actualEndDate) : (item.ActualEndDate ? new Date(item.ActualEndDate) : undefined),
      duration: item.duration ?? item.Duration,
      lastUpdateDate: item.lastUpdateDate ? new Date(item.lastUpdateDate) : (item.LastUpdateDate ? new Date(item.LastUpdateDate) : undefined),
      activitiesCount: item.activitiesCount ?? item.ActivitiesCount ?? 0,
      assetsCount: item.assetsCount ?? item.AssetsCount ?? 0,
      qualityIssuesCount: item.qualityIssuesCount ?? item.QualityIssuesCount ?? 0
    };
  }

  private transformKpis(kpis: any): any {
    return {
      totalBoxes: kpis.totalBoxes ?? kpis.TotalBoxes ?? 0,
      inProgressCount: kpis.inProgressCount ?? kpis.InProgressCount ?? 0,
      completedCount: kpis.completedCount ?? kpis.CompletedCount ?? 0,
      notStartedCount: kpis.notStartedCount ?? kpis.NotStartedCount ?? 0,
      dispatchedCount: kpis.dispatchedCount ?? kpis.DispatchedCount ?? 0,
      averageProgress: kpis.averageProgress ?? kpis.AverageProgress ?? 0,
      averageProgressFormatted: kpis.averageProgressFormatted || kpis.AverageProgressFormatted || '0.00%'
    };
  }

  private transformAggregations(aggregations: any): any {
    return {
      statusDistribution: aggregations.statusDistribution || aggregations.StatusDistribution || {},
      progressRangeDistribution: aggregations.progressRangeDistribution || aggregations.ProgressRangeDistribution || {},
      topProjects: (aggregations.topProjects || aggregations.TopProjects || []).map((p: any) => ({
        projectId: p.projectId || p.ProjectId || '',
        projectCode: p.projectCode || p.ProjectCode || '',
        projectName: p.projectName || p.ProjectName || '',
        boxCount: p.boxCount ?? p.BoxCount ?? 0
      }))
    };
  }

  /**
   * Get activities report with filtering and pagination
   */
  getActivitiesReport(params: ActivitiesReportQueryParams): Observable<PaginatedActivitiesReportResponse> {
    const queryParams: any = {};
    
    if (params.page !== undefined) queryParams.page = params.page;
    if (params.pageSize !== undefined) queryParams.pageSize = params.pageSize;
    if (params.projectId) queryParams.projectId = params.projectId;
    if (params.teamId) queryParams.teamId = params.teamId;
    if (params.status !== undefined) queryParams.status = params.status;
    if (params.plannedStartDateFrom) queryParams.plannedStartDateFrom = params.plannedStartDateFrom;
    if (params.plannedStartDateTo) queryParams.plannedStartDateTo = params.plannedStartDateTo;
    if (params.plannedEndDateFrom) queryParams.plannedEndDateFrom = params.plannedEndDateFrom;
    if (params.plannedEndDateTo) queryParams.plannedEndDateTo = params.plannedEndDateTo;
    if (params.search) queryParams.search = params.search;

    return this.apiService.get<any>(`${this.endpoint}/activities`, queryParams).pipe(
      map(response => this.transformActivitiesReport(response))
    );
  }

  /**
   * Get activities summary/KPIs
   */
  getActivitiesSummary(params?: Partial<ActivitiesReportQueryParams>): Observable<ActivitiesSummary> {
    const queryParams: any = {};
    
    if (params?.projectId) queryParams.projectId = params.projectId;
    if (params?.teamId) queryParams.teamId = params.teamId;
    if (params?.status !== undefined) queryParams.status = params.status;
    if (params?.plannedStartDateFrom) queryParams.plannedStartDateFrom = params.plannedStartDateFrom;
    if (params?.plannedStartDateTo) queryParams.plannedStartDateTo = params.plannedStartDateTo;
    if (params?.plannedEndDateFrom) queryParams.plannedEndDateFrom = params.plannedEndDateFrom;
    if (params?.plannedEndDateTo) queryParams.plannedEndDateTo = params.plannedEndDateTo;
    if (params?.search) queryParams.search = params.search;

    return this.apiService.get<any>(`${this.endpoint}/activities/summary`, queryParams).pipe(
      map(response => this.transformActivitiesSummary(response))
    );
  }

  /**
   * Export activities report to Excel
   */
  exportActivitiesReportToExcel(params: ActivitiesReportQueryParams): Observable<Blob> {
    const queryParams: any = {};
    
    if (params.projectId) queryParams.projectId = params.projectId;
    if (params.teamId) queryParams.teamId = params.teamId;
    if (params.status !== undefined) queryParams.status = params.status;
    if (params.plannedStartDateFrom) queryParams.plannedStartDateFrom = params.plannedStartDateFrom;
    if (params.plannedStartDateTo) queryParams.plannedStartDateTo = params.plannedStartDateTo;
    if (params.plannedEndDateFrom) queryParams.plannedEndDateFrom = params.plannedEndDateFrom;
    if (params.plannedEndDateTo) queryParams.plannedEndDateTo = params.plannedEndDateTo;
    if (params.search) queryParams.search = params.search;

    // Build query string
    const queryString = Object.keys(queryParams)
      .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(queryParams[key])}`)
      .join('&');
    
    const endpoint = queryString 
      ? `${this.endpoint}/activities/export/excel?${queryString}`
      : `${this.endpoint}/activities/export/excel`;

    return this.apiService.download(endpoint);
  }

  /**
   * Transform backend activities report response to frontend model
   */
  private transformActivitiesReport(backendData: any): PaginatedActivitiesReportResponse {
    const data = backendData.data || backendData.Data || backendData;
    
    return {
      items: (data.items || data.Items || []).map((item: any) => ({
        activityId: item.activityId || item.ActivityId || '',
        activityName: item.activityName || item.ActivityName || '',
        boxTag: item.boxTag || item.BoxTag || '',
        projectName: item.projectName || item.ProjectName || '',
        assignedTeam: item.assignedTeam || item.AssignedTeam,
        status: item.status || item.Status || '',
        progressPercentage: item.progressPercentage ?? item.ProgressPercentage ?? 0,
        plannedStartDate: item.plannedStartDate || item.PlannedStartDate,
        plannedEndDate: item.plannedEndDate || item.PlannedEndDate,
        actualStartDate: item.actualStartDate || item.ActualStartDate,
        actualEndDate: item.actualEndDate || item.ActualEndDate,
        actualDuration: item.actualDuration ?? item.ActualDuration,
        delayDays: item.delayDays ?? item.DelayDays,
        delayDaysFormatted: item.delayDaysFormatted || item.DelayDaysFormatted,
        boxId: item.boxId || item.BoxId || '',
        projectId: item.projectId || item.ProjectId || ''
      })),
      page: data.page ?? data.Page ?? 1,
      pageSize: data.pageSize ?? data.PageSize ?? 50,
      totalCount: data.totalCount ?? data.TotalCount ?? 0,
      totalPages: data.totalPages ?? data.TotalPages ?? 0
    };
  }

  /**
   * Transform backend activities summary response to frontend model
   */
  private transformActivitiesSummary(backendData: any): ActivitiesSummary {
    const data = backendData.data || backendData.Data || backendData;
    
    return {
      totalActivities: data.totalActivities ?? data.TotalActivities ?? 0,
      completed: data.completed ?? data.Completed ?? 0,
      inProgress: data.inProgress ?? data.InProgress ?? 0,
      pending: data.pending ?? data.Pending ?? 0,
      delayed: data.delayed ?? data.Delayed ?? 0,
      averageProgress: data.averageProgress ?? data.AverageProgress ?? 0,
      overdue: data.overdue ?? data.Overdue ?? 0,
      dueThisWeek: data.dueThisWeek ?? data.DueThisWeek ?? 0
    };
  }

  /**
   * Get teams performance report with filtering and pagination
   */
  getTeamsPerformanceReport(params: TeamsPerformanceReportQueryParams): Observable<PaginatedTeamsPerformanceResponse> {
    const queryParams: any = {};
    
    if (params.page !== undefined) queryParams.page = params.page;
    if (params.pageSize !== undefined) queryParams.pageSize = params.pageSize;
    if (params.projectId) queryParams.projectId = params.projectId;
    if (params.teamId) queryParams.teamId = params.teamId;
    if (params.status !== undefined) queryParams.status = params.status;
    if (params.search) queryParams.search = params.search;

    return this.apiService.get<any>(`${this.endpoint}/teams-performance`, queryParams).pipe(
      map(response => this.transformTeamsPerformanceReport(response))
    );
  }

  /**
   * Get teams performance summary/KPIs
   */
  getTeamsPerformanceSummary(params?: Partial<TeamsPerformanceReportQueryParams>): Observable<TeamsPerformanceSummary> {
    const queryParams: any = {};
    
    if (params?.projectId) queryParams.projectId = params.projectId;
    if (params?.teamId) queryParams.teamId = params.teamId;
    if (params?.status !== undefined) queryParams.status = params.status;
    if (params?.search) queryParams.search = params.search;

    return this.apiService.get<any>(`${this.endpoint}/teams-performance/summary`, queryParams).pipe(
      map(response => this.transformTeamsPerformanceSummary(response))
    );
  }

  /**
   * Get team activities for drill-down
   */
  getTeamActivities(teamId: string, params?: { projectId?: string; status?: number }): Observable<TeamActivitiesResponse> {
    const queryParams: any = {};
    
    if (params?.projectId) queryParams.projectId = params.projectId;
    if (params?.status !== undefined) queryParams.status = params.status;

    return this.apiService.get<any>(`${this.endpoint}/teams-performance/${teamId}/activities`, queryParams).pipe(
      map(response => this.transformTeamActivities(response))
    );
  }

  /**
   * Export teams performance report to Excel
   */
  exportTeamsPerformanceReportToExcel(params: TeamsPerformanceReportQueryParams): Observable<Blob> {
    const queryParams: any = {};
    
    if (params.projectId) queryParams.projectId = params.projectId;
    if (params.teamId) queryParams.teamId = params.teamId;
    if (params.status !== undefined) queryParams.status = params.status;
    if (params.search) queryParams.search = params.search;

    // Build query string
    const queryString = Object.keys(queryParams)
      .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(queryParams[key])}`)
      .join('&');
    
    const endpoint = queryString 
      ? `${this.endpoint}/teams-performance/export/excel?${queryString}`
      : `${this.endpoint}/teams-performance/export/excel`;

    return this.apiService.download(endpoint);
  }

  /**
   * Transform backend teams performance report response to frontend model
   */
  private transformTeamsPerformanceReport(backendData: any): PaginatedTeamsPerformanceResponse {
    const data = backendData.data || backendData.Data || backendData;
    
    return {
      items: (data.items || data.Items || []).map((item: any) => ({
        teamId: item.teamId || item.TeamId || '',
        teamCode: item.teamCode || item.TeamCode || '',
        teamName: item.teamName || item.TeamName || '',
        membersCount: item.membersCount ?? item.MembersCount ?? 0,
        totalAssignedActivities: item.totalAssignedActivities ?? item.TotalAssignedActivities ?? 0,
        completed: item.completed ?? item.Completed ?? 0,
        inProgress: item.inProgress ?? item.InProgress ?? 0,
        pending: item.pending ?? item.Pending ?? 0,
        delayed: item.delayed ?? item.Delayed ?? 0,
        averageTeamProgress: item.averageTeamProgress ?? item.AverageTeamProgress ?? 0,
        workloadLevel: item.workloadLevel || item.WorkloadLevel || 'Normal'
      })),
      page: data.page ?? data.Page ?? 1,
      pageSize: data.pageSize ?? data.PageSize ?? 25,
      totalCount: data.totalCount ?? data.TotalCount ?? 0,
      totalPages: data.totalPages ?? data.TotalPages ?? 0
    };
  }

  /**
   * Transform backend teams performance summary response to frontend model
   */
  private transformTeamsPerformanceSummary(backendData: any): TeamsPerformanceSummary {
    const data = backendData.data || backendData.Data || backendData;
    
    return {
      totalTeams: data.totalTeams ?? data.TotalTeams ?? 0,
      totalTeamMembers: data.totalTeamMembers ?? data.TotalTeamMembers ?? 0,
      totalAssignedActivities: data.totalAssignedActivities ?? data.TotalAssignedActivities ?? 0,
      completedActivities: data.completedActivities ?? data.CompletedActivities ?? 0,
      inProgressActivities: data.inProgressActivities ?? data.InProgressActivities ?? 0,
      delayedActivities: data.delayedActivities ?? data.DelayedActivities ?? 0,
      averageTeamProgress: data.averageTeamProgress ?? data.AverageTeamProgress ?? 0,
      teamWorkloadIndicator: data.teamWorkloadIndicator ?? data.TeamWorkloadIndicator ?? 0
    };
  }

  /**
   * Transform backend team activities response to frontend model
   */
  private transformTeamActivities(backendData: any): TeamActivitiesResponse {
    const data = backendData.data || backendData.Data || backendData;
    
    return {
      teamId: data.teamId || data.TeamId || '',
      teamName: data.teamName || data.TeamName || '',
      activities: (data.activities || data.Activities || []).map((item: any) => ({
        activityId: item.activityId || item.ActivityId || '',
        activityName: item.activityName || item.ActivityName || '',
        boxTag: item.boxTag || item.BoxTag || '',
        projectName: item.projectName || item.ProjectName || '',
        status: item.status || item.Status || '',
        progressPercentage: item.progressPercentage ?? item.ProgressPercentage ?? 0,
        plannedStartDate: item.plannedStartDate || item.PlannedStartDate,
        plannedEndDate: item.plannedEndDate || item.PlannedEndDate,
        actualStartDate: item.actualStartDate || item.ActualStartDate,
        actualEndDate: item.actualEndDate || item.ActualEndDate,
        duration: item.duration ?? item.Duration,
        boxId: item.boxId || item.BoxId || '',
        projectId: item.projectId || item.ProjectId || ''
      })),
      totalCount: data.totalCount ?? data.TotalCount ?? 0
    };
  }
}

