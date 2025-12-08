import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';

export interface RoleSummary {
  roleId: string;
  roleName: string;
  description?: string;
  isActive?: boolean;
  createdDate?: Date;
}

export interface UserGroupSummary {
  groupId: string;
  groupName: string;
  roles: RoleSummary[];
}

export interface UserDto {
  userId: string;
  email: string;
  fullName?: string;
  departmentId?: string;
  department?: string;
  departmentDetails?: {
    departmentId?: string;
    departmentName?: string;
  };
  isActive: boolean;
  lastLoginDate?: Date;
  createdDate: Date;
  directRoles?: string[];
  directRoleIds?: string[];
  directRoleSummaries?: RoleSummary[];
  groups?: UserGroupSummary[];
  allRoles?: string[];
  allRoleSummaries?: RoleSummary[];
}

export interface CreateUserRequest {
  email: string;
  password: string;
  fullName: string;
  departmentId: string;
  isActive?: boolean;
}

export interface UpdateUserRequest {
  userId: string;
  email: string;
  fullName?: string;
  departmentId?: string;
  isActive: boolean;
}

export interface PaginatedUsersResponse {
  items: UserDto[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly endpoint = 'users';

  constructor(private apiService: ApiService) {}

  /**
   * Get all users with pagination
   */
  getUsers(pageNumber: number = 1, pageSize: number = 25): Observable<PaginatedUsersResponse> {
    const params = new URLSearchParams();
    params.set('pageNumber', pageNumber.toString());
    params.set('pageSize', pageSize.toString());
    
    return this.apiService.get<any>(`${this.endpoint}?${params.toString()}`).pipe(
      map((response: any) => {
        console.log('🔍 UserService.getUsers - Raw API response:', response);
        
        // The API service already extracts data/Data, but handle Result wrapper if still present
        const data = response.data || response.Data || response;
        console.log('🔍 UserService.getUsers - Extracted data:', data);
        
        // Handle both camelCase and PascalCase properties
        const items = data.items || data.Items || [];
        const totalCount = data.totalCount ?? data.TotalCount ?? 0;
        const responsePageNumber = data.pageNumber ?? data.PageNumber ?? pageNumber;
        const responsePageSize = data.pageSize ?? data.PageSize ?? pageSize;
        const totalPages = data.totalPages ?? data.TotalPages ?? 0;
        
        console.log('🔍 UserService.getUsers - Parsed values:', {
          itemsCount: items.length,
          totalCount,
          pageNumber: responsePageNumber,
          pageSize: responsePageSize,
          totalPages
        });
        
        // Ensure we have the paginated response structure
        const paginatedResponse: PaginatedUsersResponse = {
          items: items.map((u: any) => this.transformUser(u)),
          totalCount,
          pageNumber: responsePageNumber,
          pageSize: responsePageSize,
          totalPages
        };
        
        console.log('🔍 UserService.getUsers - Final response:', paginatedResponse);
        return paginatedResponse;
      })
    );
  }

  /**
   * Get users by department ID
   * Note: This method fetches all users (may need pagination in the future)
   */
  getUsersByDepartment(departmentId: string): Observable<UserDto[]> {
    // Fetch a large page size to get all users for department filtering
    return this.getUsers(1, 1000).pipe(
      map((response: PaginatedUsersResponse) => response.items.filter((u: UserDto) => u.departmentId === departmentId && u.isActive))
    );
  }

  /**
   * Get user by ID
   */
  getUserById(userId: string): Observable<UserDto> {
    return this.apiService.get<UserDto>(`${this.endpoint}/${userId}`).pipe(
      map(u => this.transformUser(u))
    );
  }

  createUser(payload: CreateUserRequest): Observable<UserDto> {
    return this.apiService.post<UserDto>(this.endpoint, payload).pipe(
      map(u => this.transformUser(u))
    );
  }

  updateUser(payload: UpdateUserRequest): Observable<UserDto> {
    return this.apiService.put<UserDto>(`${this.endpoint}/${payload.userId}`, payload).pipe(
      map(u => this.transformUser(u))
    );
  }

  assignRolesToUser(userId: string, roleIds: string[]): Observable<void> {
    return this.apiService.post<void>(`${this.endpoint}/${userId}/roles`, roleIds);
  }

  assignUserToGroups(userId: string, groupIds: string[]): Observable<void> {
    return this.apiService.post<void>(`${this.endpoint}/${userId}/groups`, groupIds);
  }

  deleteUser(userId: string): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${userId}`);
  }

  /**
   * Transform backend user to frontend model
   */
  private transformUser(backendUser: any): UserDto {
    const departmentInfo = backendUser.department || backendUser.departmentDetails;
    const departmentId = backendUser.departmentId || departmentInfo?.departmentId;
    const departmentName =
      typeof departmentInfo === 'string'
        ? departmentInfo
        : departmentInfo?.departmentName ||
          departmentInfo?.name ||
          backendUser.department ||
          backendUser.departmentName;

    const normalizeRole = (role: any): RoleSummary => ({
      roleId: role.roleId || role.id,
      roleName: role.roleName || role.name,
      description: role.description,
      isActive: role.isActive ?? true,
      createdDate: role.createdDate ? new Date(role.createdDate) : undefined
    });

    const parseRoleEntry = (entry: any): RoleSummary | null => {
      if (!entry) {
        return null;
      }
      if (typeof entry === 'string') {
        const idMatch = entry.match(/RoleId\s*=\s*([^,}]+)/i);
        const nameMatch = entry.match(/RoleName\s*=\s*([^,}]+)/i);
        const descMatch = entry.match(/Description\s*=\s*([^,}]+)/i);
        const activeMatch = entry.match(/IsActive\s*=\s*([^,}]+)/i);
        const createdMatch = entry.match(/CreatedDate\s*=\s*([^,}]+)/i);
        return {
          roleId: (idMatch?.[1]?.trim()) || '',
          roleName: (nameMatch?.[1]?.trim()) || '',
          description: descMatch?.[1]?.trim(),
          isActive: activeMatch ? activeMatch[1].trim().toLowerCase() === 'true' : undefined,
          createdDate: createdMatch ? new Date(createdMatch[1].trim()) : undefined
        };
      }
      return normalizeRole(entry);
    };

    const directRoleSource =
      backendUser.directRoleSummaries ??
      backendUser.directRoleDetails ??
      backendUser.directRoles ??
      [];

    const directRoleObjects: RoleSummary[] = (Array.isArray(directRoleSource) ? directRoleSource : [directRoleSource])
      .map(parseRoleEntry)
      .filter((role): role is RoleSummary => !!role);

    const directRoleNames: string[] = directRoleObjects
      .map(role => role.roleName)
      .filter((name): name is string => !!name);

    const normalizedGroups: UserGroupSummary[] = (backendUser.userGroups || backendUser.groups || []).map((group: any) => {
      const groupRef = group.group || group;
      const groupRolesSource = group.roles || group.groupRoles || groupRef?.roles || [];

      return {
        groupId: group.groupId || groupRef?.groupId || group.id,
        groupName: group.groupName || groupRef?.groupName || groupRef?.name,
        roles: groupRolesSource.map((role: any) => normalizeRole(role))
      };
    });

    const allRoleSource = backendUser.allRoleSummaries ?? backendUser.allRoles ?? [];
    const allRoleObjects: RoleSummary[] = (Array.isArray(allRoleSource) ? allRoleSource : [allRoleSource])
      .map(parseRoleEntry)
      .filter((role): role is RoleSummary => !!role);

    const groupedRoleNames = normalizedGroups.flatMap(group => group.roles?.map(role => role.roleName).filter(Boolean) || []);
    const allRoleNames = allRoleObjects.length
      ? allRoleObjects.map(role => role.roleName || '').filter(Boolean)
      : Array.from(new Set([...(directRoleNames || []), ...groupedRoleNames]));

    return {
      userId: backendUser.userId || backendUser.id || backendUser.userId?.toString(),
      email: backendUser.email || '',
      fullName: backendUser.fullName,
      departmentId: departmentId ? String(departmentId) : undefined,
      department: departmentName,
      departmentDetails: {
        departmentId: departmentId ? String(departmentId) : undefined,
        departmentName
      },
      isActive: backendUser.isActive !== undefined ? backendUser.isActive : true,
      lastLoginDate: backendUser.lastLoginDate ? new Date(backendUser.lastLoginDate) : undefined,
      createdDate: backendUser.createdDate ? new Date(backendUser.createdDate) : new Date(),
      directRoles: directRoleNames,
      directRoleIds: directRoleObjects.map(role => role.roleId).filter(Boolean) as string[],
      directRoleSummaries: directRoleObjects,
      groups: normalizedGroups,
      allRoles: allRoleNames,
      allRoleSummaries: allRoleObjects
    };
  }
}


