import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';

export interface RoleDto {
  roleId: string;
  roleName: string;
  description?: string;
  isActive: boolean;
  createdDate: Date;
}

export interface CreateRoleRequest {
  roleName: string;
  description?: string;
}

export interface UpdateRoleRequest {
  roleId: string;
  roleName: string;
  description?: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private readonly endpoint = 'roles';

  constructor(private apiService: ApiService) {}

  getRoles(): Observable<RoleDto[]> {
    return this.apiService.get<RoleDto[]>(this.endpoint).pipe(
      map(roles => (roles || []).map(r => this.transformRole(r)))
    );
  }

  createRole(payload: CreateRoleRequest): Observable<RoleDto> {
    return this.apiService.post<RoleDto>(this.endpoint, payload).pipe(
      map(r => this.transformRole(r))
    );
  }

  updateRole(payload: UpdateRoleRequest): Observable<RoleDto> {
    return this.apiService.put<RoleDto>(`${this.endpoint}/${payload.roleId}`, payload).pipe(
      map(r => this.transformRole(r))
    );
  }

  deleteRole(roleId: string): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${roleId}`);
  }

  private transformRole(backendRole: any): RoleDto {
    return {
      roleId: backendRole.roleId || backendRole.id || '',
      roleName: backendRole.roleName || backendRole.name || '',
      description: backendRole.description,
      isActive: backendRole.isActive !== undefined ? backendRole.isActive : true,
      createdDate: backendRole.createdDate ? new Date(backendRole.createdDate) : new Date()
    };
  }
}
