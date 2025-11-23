import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';

export interface UserDto {
  userId: string;
  email: string;
  fullName?: string;
  departmentId?: string;
  department?: string;
  isActive: boolean;
  lastLoginDate?: Date;
  createdDate: Date;
  directRoles?: string[];
  groups?: any[];
  allRoles?: string[];
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly endpoint = 'users';

  constructor(private apiService: ApiService) {}

  /**
   * Get all users
   */
  getUsers(): Observable<UserDto[]> {
    return this.apiService.get<UserDto[]>(this.endpoint).pipe(
      map(users => (users || []).map(u => this.transformUser(u)))
    );
  }

  /**
   * Get users by department ID
   */
  getUsersByDepartment(departmentId: string): Observable<UserDto[]> {
    return this.getUsers().pipe(
      map(users => users.filter(u => u.departmentId === departmentId && u.isActive))
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

  /**
   * Transform backend user to frontend model
   */
  private transformUser(backendUser: any): UserDto {
    return {
      userId: backendUser.userId || backendUser.id || backendUser.userId?.toString(),
      email: backendUser.email || '',
      fullName: backendUser.fullName,
      departmentId: backendUser.departmentId ? String(backendUser.departmentId) : undefined,
      department: backendUser.department || backendUser.departmentName,
      isActive: backendUser.isActive !== undefined ? backendUser.isActive : true,
      lastLoginDate: backendUser.lastLoginDate ? new Date(backendUser.lastLoginDate) : undefined,
      createdDate: backendUser.createdDate ? new Date(backendUser.createdDate) : new Date(),
      directRoles: backendUser.directRoles || [],
      groups: backendUser.groups || [],
      allRoles: backendUser.allRoles || []
    };
  }
}


