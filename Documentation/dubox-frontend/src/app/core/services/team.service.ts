import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import { Team, CreateTeam, UpdateTeam, TeamMember, TeamMembersDto, AssignTeamMembers, CompleteTeamMemberProfile, Department, PaginatedTeamsResponse, TeamGroup, PaginatedTeamGroupsResponse, CreateTeamGroup, UpdateTeamGroup, TeamGroupMembers, AddTeamMember } from '../models/team.model';
import { UserDto } from './user.service';

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
   * Get inactive team members (IsActive = false)
   */
  getInactiveTeamMembers(teamId: string): Observable<TeamMembersDto> {
    return this.apiService.get<TeamMembersDto>(`${this.endpoint}/team-members/${teamId}/inactive`).pipe(
      map(data => this.transformTeamMembers(data))
    );
  }

  /**
   * Get all active team members across all teams
   */
  getAllActiveMembers(): Observable<TeamMember[]> {
    return this.apiService.get<any>(`${this.endpoint}/all-active-members`).pipe(
      map(response => {
        const data = response?.data || response?.Data || response;
        const members = Array.isArray(data) ? data : [];
        return members.map((m: any) => this.transformTeamMember(m));
      })
    );
  }

  /**
   * Reactivate an inactive team member
   */
  reactivateTeamMember(teamId: string, teamMemberId: string): Observable<TeamMember> {
    return this.apiService.post<any>(`${this.endpoint}/team-members/${teamId}/reactivate/${teamMemberId}`, {}).pipe(
      map(response => {
        const data = response?.data || response?.Data || response;
        return this.transformTeamMember(data);
      })
    );
  }

  /**
   * Get team users for assignment purposes
   */
  getTeamUsers(teamId: string): Observable<{userId: string, userName: string, userEmail: string}[]> {
    return this.apiService.get<any>(`${this.endpoint}/${teamId}/users`).pipe(
      map(response => {
        const data = response?.data || response?.Data || response;
        return data || [];
      })
    );
  }

  /**
   * Get available users that can be added to a team
   * Returns users from the same department who are not already team members and are active
   */
  getAvailableUsersForTeam(teamId: string): Observable<UserDto[]> {
    return this.apiService.get<any>(`${this.endpoint}/${teamId}/available-users`).pipe(
      map(response => {
        // apiService already extracts data, but handle both wrapped and direct array responses
        const data = Array.isArray(response) ? response : (response?.data || response?.Data || response || []);
        return Array.isArray(data) ? data : [];
      })
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
   * Add a new team member
   */
  addTeamMember(teamId: string, member: AddTeamMember): Observable<TeamMember> {
    return this.apiService.post<any>(`${this.endpoint}/${teamId}/members`, member).pipe(
      map(response => {
        const data = response?.data || response?.Data || response;
        return {
          teamMemberId: data.teamMemberId || data.id,
          userId: data.userId,
          teamId: data.teamId,
          teamCode: data.teamCode || '',
          teamName: data.teamName || '',
          email: data.email || '',
          fullName: data.fullName,
          employeeCode: data.employeeCode || '',
          employeeName: data.employeeName || '',
          mobileNumber: data.mobileNumber,
          isActive: data.isActive !== undefined ? data.isActive : true
        };
      })
    );
  }

  /**
   * Remove team member
   */
  removeTeamMember(teamId: string, teamMemberId: string): Observable<boolean> {
    return this.apiService.delete<boolean>(`${this.endpoint}/team-members/${teamId}/member/${teamMemberId}`);
  }

  /**
   * Create team group
   */
  createTeamGroup(teamGroup: CreateTeamGroup): Observable<TeamGroup> {
    return this.apiService.post<TeamGroup>(`${this.endpoint}/team-groups`, teamGroup).pipe(
      map(tg => this.transformTeamGroup(tg))
    );
  }

  /**
   * Get all team groups
   */
  getAllTeamGroups(): Observable<TeamGroup[]> {
    return this.apiService.get<any>(`${this.endpoint}/team-groups`).pipe(
      map(response => {
        const data = response?.data || response?.Data || response;
        const groups = Array.isArray(data) ? data : [];
        return groups.map((g: any) => this.transformTeamGroup(g));
      })
    );
  }

  /**
   * Get team groups with pagination and filters
   */
  getTeamGroupsPaginated(params: {
    page?: number;
    pageSize?: number;
    search?: string;
    teamId?: string;
    isActive?: boolean;
  }): Observable<PaginatedTeamGroupsResponse> {
    const queryParams: any = {};
    
    if (params.page !== undefined) queryParams.page = params.page;
    if (params.pageSize !== undefined) queryParams.pageSize = params.pageSize;
    if (params.search) queryParams.search = params.search;
    if (params.teamId) queryParams.teamId = params.teamId;
    if (params.isActive !== undefined) queryParams.isActive = params.isActive;

    return this.apiService.get<any>(`${this.endpoint}/team-groups`, queryParams).pipe(
      map((response: any) => {
        const data = response.data || response.Data || response;
        const items = data.items || data.Items || [];
        const totalCount = data.totalCount ?? data.TotalCount ?? 0;
        const page = data.page ?? data.Page ?? params.page ?? 1;
        const pageSize = data.pageSize ?? data.PageSize ?? params.pageSize ?? 25;
        const totalPages = data.totalPages ?? data.TotalPages ?? 0;

        return {
          items: items.map((g: any) => this.transformTeamGroup(g)),
          totalCount,
          page,
          pageSize,
          totalPages
        };
      })
    );
  }

  /**
   * Get team group by ID
   */
  getTeamGroupById(groupId: string): Observable<TeamGroup> {
    return this.apiService.get<any>(`${this.endpoint}/team-groups/${groupId}`).pipe(
      map(response => {
        const data = response?.data || response?.Data || response;
        return this.transformTeamGroup(data);
      })
    );
  }

  /**
   * Get team group members
   */
  getTeamGroupMembers(groupId: string): Observable<TeamGroupMembers> {
    return this.apiService.get<any>(`${this.endpoint}/team-groups/${groupId}/members`).pipe(
      map(response => {
        const data = response?.data || response?.Data || response;
        return {
          teamGroupId: data.teamGroupId?.toString() || '',
          teamId: data.teamId?.toString() || '',
          teamCode: data.teamCode || '',
          teamName: data.teamName || '',
          groupTag: data.groupTag || '',
          groupType: data.groupType || '',
          groupLeaderId: data.groupLeaderId?.toString(),
          groupLeaderName: data.groupLeaderName,
          memberCount: data.memberCount || 0,
          isActive: data.isActive !== undefined ? data.isActive : true,
          members: (data.members || []).map((m: any) => ({
            teamMemberId: m.teamMemberId || m.id,
            userId: m.userId,
            teamId: m.teamId,
            teamCode: m.teamCode || '',
            teamName: m.teamName || '',
            email: m.email || '',
            fullName: m.fullName,
            employeeCode: m.employeeCode || '',
            employeeName: m.employeeName || '',
            mobileNumber: m.mobileNumber,
            isActive: m.isActive !== undefined ? m.isActive : true,
            teamGroupId: m.teamGroupId?.toString()
          }))
        };
      })
    );
  }

  /**
   * Update a team group
   */
  updateTeamGroup(groupId: string, teamGroup: UpdateTeamGroup): Observable<TeamGroup> {
    return this.apiService.put<any>(`${this.endpoint}/team-groups/${groupId}`, teamGroup).pipe(
      map(response => {
        const data = response?.data || response?.Data || response;
        return this.transformTeamGroup(data);
      })
    );
  }

  /**
   * Add members to a team group
   */
  addMembersToGroup(groupId: string, teamMemberIds: string[]): Observable<TeamGroup> {
    return this.apiService.post<any>(`${this.endpoint}/team-groups/${groupId}/add-members`, { teamMemberIds }).pipe(
      map(response => {
        const data = response?.data || response?.Data || response;
        return this.transformTeamGroup(data);
      })
    );
  }

  /**
   * Assign a leader to a team group
   */
  assignGroupLeader(groupId: string, teamMemberId: string): Observable<TeamGroup> {
    return this.apiService.post<any>(`${this.endpoint}/team-groups/${groupId}/assign-leader`, { teamMemberId }).pipe(
      map(response => {
        const data = response?.data || response?.Data || response;
        return this.transformTeamGroup(data);
      })
    );
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
   * Transform backend team member to frontend model
   */
  private transformTeamMember(backendMember: any): TeamMember {
    return {
      teamMemberId: backendMember.teamMemberId || backendMember.id,
      userId: backendMember.userId,
      teamId: backendMember.teamId?.toString() || '',
      teamCode: backendMember.teamCode || '',
      teamName: backendMember.teamName || '',
      email: backendMember.email || '',
      fullName: backendMember.fullName,
      employeeCode: backendMember.employeeCode || '',
      employeeName: backendMember.employeeName || '',
      mobileNumber: backendMember.mobileNumber,
      isActive: backendMember.isActive !== undefined ? backendMember.isActive : true
    };
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

  /**
   * Transform backend team group to frontend model
   */
  private transformTeamGroup(backendGroup: any): TeamGroup {
    return {
      teamGroupId: backendGroup.teamGroupId || backendGroup.id?.toString(),
      teamId: backendGroup.teamId?.toString() || '',
      teamName: backendGroup.teamName || '',
      teamCode: backendGroup.teamCode || '',
      groupTag: backendGroup.groupTag || '',
      groupType: backendGroup.groupType || '',
      groupLeaderId: backendGroup.groupLeaderId?.toString(),
      groupLeaderName: backendGroup.groupLeaderName,
      isActive: backendGroup.isActive !== undefined ? backendGroup.isActive : true,
      createdDate: backendGroup.createdDate ? new Date(backendGroup.createdDate) : new Date(),
      createdBy: backendGroup.createdBy?.toString(),
      memberCount: backendGroup.memberCount || 0,
      members: (backendGroup.members || []).map((m: any) => ({
        teamMemberId: m.teamMemberId || m.id,
        userId: m.userId,
        teamId: m.teamId || backendGroup.teamId?.toString(),
        teamCode: m.teamCode || backendGroup.teamCode || '',
        teamName: m.teamName || backendGroup.teamName || '',
        email: m.email || '',
        fullName: m.fullName,
        employeeCode: m.employeeCode || '',
        employeeName: m.employeeName || '',
        mobileNumber: m.mobileNumber,
        isActive: m.isActive !== undefined ? m.isActive : true,
        teamGroupId: m.teamGroupId?.toString()
      }))
    };
  }
}

