import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import { Team, CreateTeam, UpdateTeam, TeamMember, TeamMembersDto, AssignTeamMembers, CompleteTeamMemberProfile, Department, PaginatedTeamsResponse } from '../models/team.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  private readonly endpoint = 'teams';

  constructor(private apiService: ApiService) {}

  /**
   * Get all teams
   */
  getTeams(): Observable<Team[]> {
    return this.apiService.get<any>(this.endpoint).pipe(
      map(response => {
        // Handle both paginated response and direct array
        const data = response?.data || response?.Data || response;
        const items = Array.isArray(data) ? data : (data?.items || data?.Items || []);
        return items.map((t: any) => this.transformTeam(t));
      })
    );
  }

  /**
   * Get teams with pagination and filters
   */
  getTeamsPaginated(params: {
    page?: number;
    pageSize?: number;
    search?: string;
    department?: string;
    trade?: string;
    isActive?: boolean;
  }): Observable<PaginatedTeamsResponse> {
    const queryParams: any = {};
    
    if (params.page !== undefined) queryParams.page = params.page;
    if (params.pageSize !== undefined) queryParams.pageSize = params.pageSize;
    if (params.search) queryParams.search = params.search;
    if (params.department) queryParams.department = params.department;
    if (params.trade) queryParams.trade = params.trade;
    if (params.isActive !== undefined) queryParams.isActive = params.isActive;

    return this.apiService.get<any>(this.endpoint, queryParams).pipe(
      map((response: any) => {
        const data = response.data || response.Data || response;
        const items = data.items || data.Items || [];
        const totalCount = data.totalCount ?? data.TotalCount ?? 0;
        const page = data.page ?? data.Page ?? params.page ?? 1;
        const pageSize = data.pageSize ?? data.PageSize ?? params.pageSize ?? 25;
        const totalPages = data.totalPages ?? data.TotalPages ?? 0;

        return {
          items: items.map((t: any) => this.transformTeam(t)),
          totalCount,
          page,
          pageSize,
          totalPages
        };
      })
    );
  }

  /**
   * Get team by ID
   */
  getTeamById(teamId: string): Observable<Team> {
    return this.apiService.get<Team>(`${this.endpoint}/${teamId}`).pipe(
      map(t => this.transformTeam(t))
    );
  }

  /**
   * Get team members
   */
  getTeamMembers(teamId: string): Observable<TeamMembersDto> {
    return this.apiService.get<TeamMembersDto>(`${this.endpoint}/team-members/${teamId}`).pipe(
      map(data => this.transformTeamMembers(data))
    );
  }

  /**
   * Create new team
   */
  createTeam(team: CreateTeam): Observable<Team> {
    return this.apiService.post<Team>(this.endpoint, team).pipe(
      map(t => this.transformTeam(t))
    );
  }

  /**
   * Update team
   */
  updateTeam(teamId: string, team: UpdateTeam): Observable<Team> {
    return this.apiService.put<Team>(`${this.endpoint}/${teamId}`, team).pipe(
      map(t => this.transformTeam(t))
    );
  }

  /**
   * Assign members to team
   */
  assignTeamMembers(assign: AssignTeamMembers): Observable<TeamMembersDto> {
    return this.apiService.post<TeamMembersDto>(`${this.endpoint}/assign-members`, assign).pipe(
      map(data => this.transformTeamMembers(data))
    );
  }

  /**
   * Complete team member profile
   */
  completeMemberProfile(profile: CompleteTeamMemberProfile): Observable<any> {
    return this.apiService.post<any>(`${this.endpoint}/complate-member-profile`, profile);
  }

  /**
   * Remove team member
   */
  removeTeamMember(teamId: string, teamMemberId: string): Observable<boolean> {
    return this.apiService.delete<boolean>(`${this.endpoint}/team-members/${teamId}/member/${teamMemberId}`);
  }

  /**
   * Get all departments
   */
  getDepartments(): Observable<Department[]> {
    return this.apiService.get<Department[]>('Department').pipe(
      map(departments => (departments || []).map(d => this.transformDepartment(d)))
    );
  }

  /**
   * Transform backend department to frontend model
   */
  private transformDepartment(backendDept: any): Department {
    // Ensure departmentId is always a string
    const deptId = backendDept.departmentId || backendDept.id;
    return {
      departmentId: deptId ? String(deptId) : '',
      departmentName: backendDept.departmentName || backendDept.name || '',
      code: backendDept.code,
      description: backendDept.description,
      location: backendDept.location,
      isActive: backendDept.isActive !== undefined ? backendDept.isActive : true
    };
  }

  /**
   * Transform backend team to frontend model
   */
  private transformTeam(backendTeam: any): Team {
    console.log('🔄 Transforming team:', backendTeam);
    console.log('🔄 Team departmentName:', backendTeam.departmentName);
    // Ensure departmentId is always a string
    const deptId = backendTeam.departmentId;
    const transformed = {
      teamId: backendTeam.teamId || backendTeam.id || backendTeam.teamId?.toString(),
      teamCode: backendTeam.teamCode || backendTeam.code || '',
      teamName: backendTeam.teamName || backendTeam.name || '',
      departmentName: backendTeam.departmentName || '',
      departmentId: deptId ? String(deptId) : undefined,
      trade: backendTeam.trade,
      teamLeaderName: backendTeam.teamLeaderName,
      teamLeaderMemberId: backendTeam.teamLeaderMemberId,
      teamSize: backendTeam.teamSize || 0,
      isActive: backendTeam.isActive !== undefined ? backendTeam.isActive : true,
      createdDate: backendTeam.createdDate ? new Date(backendTeam.createdDate) : new Date()
    };
    console.log('✅ Transformed team:', transformed);
    console.log('✅ Transformed team departmentId:', transformed.departmentId);
    return transformed;
  }

  /**
   * Transform backend team members to frontend model
   */
  private transformTeamMembers(backendData: any): TeamMembersDto {
    return {
      teamId: backendData.teamId || backendData.id?.toString(),
      teamCode: backendData.teamCode || '',
      teamName: backendData.teamName || '',
      teamSize: backendData.teamSize || 0,
      members: (backendData.members || []).map((m: any) => ({
        teamMemberId: m.teamMemberId || m.id,
        userId: m.userId,
        teamId: m.teamId || backendData.teamId?.toString(),
        teamCode: m.teamCode || backendData.teamCode || '',
        teamName: m.teamName || backendData.teamName || '',
        email: m.email || '',
        fullName: m.fullName,
        employeeCode: m.employeeCode || '',
        employeeName: m.employeeName || '',
        mobileNumber: m.mobileNumber,
        isActive: m.isActive !== undefined ? m.isActive : true
      }))
    };
  }
}

