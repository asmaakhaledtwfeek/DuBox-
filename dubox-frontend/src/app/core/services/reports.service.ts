import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';

export interface BoxProgressData {
  building: string;
  projectId: string;
  nonAssembled: number;
  backing: number;
  released1stFix: number;
  released2ndFix: number;
  released3rdFix: number;
  total: number;
}

export interface TeamProductivityData {
  teamId: string;
  teamName: string;
  totalActivities: number;
  completedActivities: number;
  inProgress: number;
  pending: number;
  averageCompletionTime: number;
  efficiency: number;
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
   * Groups boxes by building and phase status
   */
  getBoxProgressReport(projectId?: string): Observable<BoxProgressData[]> {
    const params = projectId ? { projectId } : {};
    return this.apiService.get<any[]>(`${this.endpoint}/box-progress`, params).pipe(
      map(data => this.transformBoxProgressData(data))
    );
  }

  /**
   * Get team productivity report data
   */
  getTeamProductivityReport(projectId?: string): Observable<TeamProductivityData[]> {
    const params = projectId ? { projectId } : {};
    return this.apiService.get<any[]>(`${this.endpoint}/team-productivity`, params).pipe(
      map(data => this.transformTeamProductivityData(data))
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
   * Get team productivity from activities and teams
   * Fallback method if reports endpoint not available
   */
  getTeamProductivityFromActivities(projectId?: string): Observable<TeamProductivityData[]> {
    return this.apiService.get<any[]>('teams').pipe(
      map(teams => this.calculateTeamProductivityFromRawData(teams))
    );
  }

  /**
   * Transform backend box progress data to frontend model
   */
  private transformBoxProgressData(backendData: any[]): BoxProgressData[] {
    return (backendData || []).map(item => ({
      building: item.building || item.Building || '',
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
   * Transform backend team productivity data to frontend model
   */
  private transformTeamProductivityData(backendData: any[]): TeamProductivityData[] {
    return (backendData || []).map(item => ({
      teamId: item.teamId || item.TeamId || '',
      teamName: item.teamName || item.TeamName || '',
      totalActivities: item.totalActivities || item.TotalActivities || 0,
      completedActivities: item.completedActivities || item.CompletedActivities || 0,
      inProgress: item.inProgress || item.InProgress || item.inProgressActivities || 0,
      pending: item.pending || item.Pending || item.pendingActivities || 0,
      averageCompletionTime: item.averageCompletionTime || item.AverageCompletionTime || 0,
      efficiency: item.efficiency || item.Efficiency || 0
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
   * Groups boxes by building and calculates phase distribution
   */
  private calculateBoxProgressFromRawData(boxes: any[]): BoxProgressData[] {
    if (!boxes || boxes.length === 0) return [];

    // Group boxes by building
    const buildingGroups = new Map<string, any[]>();
    
    boxes.forEach(box => {
      const building = box.building || box.Building || 'Unknown';
      if (!buildingGroups.has(building)) {
        buildingGroups.set(building, []);
      }
      buildingGroups.get(building)!.push(box);
    });

    // Calculate statistics for each building
    const result: BoxProgressData[] = [];
    buildingGroups.forEach((boxList, building) => {
      const stats: BoxProgressData = {
        building,
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
   * Calculate team productivity from raw team and activity data
   */
  private calculateTeamProductivityFromRawData(teams: any[]): TeamProductivityData[] {
    if (!teams || teams.length === 0) return [];

    return teams.map(team => {
      // For now, return basic data - will need activities data for accurate metrics
      return {
        teamId: team.teamId || team.TeamId || '',
        teamName: team.teamName || team.TeamName || '',
        totalActivities: 0, // Would need to query activities by team
        completedActivities: 0,
        inProgress: 0,
        pending: 0,
        averageCompletionTime: 0,
        efficiency: 0
      };
    });
  }
}

