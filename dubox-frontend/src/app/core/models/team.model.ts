export interface Team {
  teamId: string;
  teamCode: string;
  teamName: string;
  departmentName: string;
  departmentId?: string;
  trade?: string;
  teamLeaderName?: string;
  teamLeaderMemberId?: string;
  teamSize?: number;
  isActive: boolean;
  createdDate: Date;
}

export interface CreateTeam {
  teamCode: string;
  teamName: string;
  departmentId: string;
  trade?: string;
}

export interface UpdateTeam {
  teamId: string;
  teamCode?: string;
  teamName?: string;
  departmentId?: string;
  trade?: string;
  teamLeaderName?: string;
  isActive?: boolean;
}

export interface TeamMember {
  teamMemberId: string;
  userId: string;
  teamId: string;
  teamCode: string;
  teamName: string;
  email: string;
  fullName?: string;
  employeeCode: string;
  employeeName: string;
  mobileNumber?: string;
  isActive?: boolean;
}

export interface TeamMembersDto {
  teamId: string;
  teamCode: string;
  teamName: string;
  teamSize: number;
  members: TeamMember[];
}

export interface AssignTeamMembers {
  teamId: string;
  userIds: string[];
}

export interface CompleteTeamMemberProfile {
  teamMemberId: string;
  employeeCode: string;
  employeeName: string;
  mobileNumber?: string;
}

export interface Department {
  departmentId: string;
  departmentName: string;
  code?: string;
  description?: string;
  location?: string;
  isActive: boolean;
}


