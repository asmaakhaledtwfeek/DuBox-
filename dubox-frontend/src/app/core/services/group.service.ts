import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import { RoleDto } from './role.service';

export interface GroupDto {
  groupId: string;
  groupName: string;
  description?: string;
  isActive: boolean;
  createdDate: Date;
  roles: RoleDto[];
}

export interface CreateGroupRequest {
  groupName: string;
  description?: string;
}

export interface UpdateGroupRequest {
  groupId: string;
  groupName: string;
  description?: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private readonly endpoint = 'groups';

  constructor(private apiService: ApiService) {}

  getGroups(): Observable<GroupDto[]> {
    return this.apiService.get<GroupDto[]>(this.endpoint).pipe(
      map(groups => (groups || []).map(g => this.transformGroup(g)))
    );
  }

  createGroup(payload: CreateGroupRequest): Observable<GroupDto> {
    return this.apiService.post<GroupDto>(this.endpoint, payload).pipe(
      map(g => this.transformGroup(g))
    );
  }

  updateGroup(payload: UpdateGroupRequest): Observable<GroupDto> {
    return this.apiService.put<GroupDto>(`${this.endpoint}/${payload.groupId}`, payload).pipe(
      map(g => this.transformGroup(g))
    );
  }

  deleteGroup(groupId: string): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${groupId}`);
  }

  assignRolesToGroup(groupId: string, roleIds: string[]): Observable<void> {
    return this.apiService.post<void>(`${this.endpoint}/${groupId}/roles`, roleIds);
  }

  private transformRole(role: any): RoleDto {
    return {
      roleId: role.roleId || role.id || '',
      roleName: role.roleName || role.name || '',
      description: role.description,
      isActive: role.isActive !== undefined ? role.isActive : true,
      createdDate: role.createdDate ? new Date(role.createdDate) : new Date()
    };
  }

  private transformGroup(backendGroup: any): GroupDto {
    return {
      groupId: backendGroup.groupId || backendGroup.id || '',
      groupName: backendGroup.groupName || backendGroup.name || '',
      description: backendGroup.description,
      isActive: backendGroup.isActive !== undefined ? backendGroup.isActive : true,
      createdDate: backendGroup.createdDate ? new Date(backendGroup.createdDate) : new Date(),
      roles: (backendGroup.roles || []).map((r: any) => this.transformRole(r))
    };
  }
}
