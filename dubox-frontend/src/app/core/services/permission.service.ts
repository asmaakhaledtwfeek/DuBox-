import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';
import { Permission, UserRole } from '../models/user.model';

export interface PermissionCheck {
  module: string;
  action: string;
}

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  
  // Define module permissions
  private readonly modulePermissions: Record<string, Record<UserRole, string[]>> = {
    projects: {
      [UserRole.Admin]: ['view', 'create', 'edit', 'delete', 'manage'],
      [UserRole.Factory]: ['view', 'edit'],
      [UserRole.Site]: ['view'],
      [UserRole.Viewer]: ['view']
    },
    boxes: {
      [UserRole.Admin]: ['view', 'create', 'edit', 'delete', 'manage'],
      [UserRole.Factory]: ['view', 'create', 'edit', 'update-status'],
      [UserRole.Site]: ['view', 'update-status'],
      [UserRole.Viewer]: ['view']
    },
    activities: {
      [UserRole.Admin]: ['view', 'create', 'edit', 'delete', 'approve', 'reject'],
      [UserRole.Factory]: ['view', 'create', 'edit', 'submit'],
      [UserRole.Site]: ['view', 'approve', 'reject'],
      [UserRole.Viewer]: ['view']
    },
    qaqc: {
      [UserRole.Admin]: ['view', 'approve', 'reject', 'manage'],
      [UserRole.Factory]: ['view', 'submit'],
      [UserRole.Site]: ['view', 'approve', 'reject'],
      [UserRole.Viewer]: ['view']
    },
    users: {
      [UserRole.Admin]: ['view', 'create', 'edit', 'delete', 'manage'],
      [UserRole.Factory]: [],
      [UserRole.Site]: [],
      [UserRole.Viewer]: []
    },
    reports: {
      [UserRole.Admin]: ['view', 'export', 'manage'],
      [UserRole.Factory]: ['view', 'export'],
      [UserRole.Site]: ['view', 'export'],
      [UserRole.Viewer]: ['view']
    },
    notifications: {
      [UserRole.Admin]: ['view', 'send', 'manage'],
      [UserRole.Factory]: ['view'],
      [UserRole.Site]: ['view'],
      [UserRole.Viewer]: ['view']
    }
  };

  constructor(
    private authService: AuthService,
    private apiService: ApiService
  ) {}

  /**
   * Check if user has permission
   */
  hasPermission(module: string, action: string): boolean {
    const user = this.authService.getCurrentUser();
    
    if (!user) {
      return false;
    }

    // Admin has all permissions
    if (user.role === UserRole.Admin) {
      return true;
    }

    // Check module permissions
    const modulePerms = this.modulePermissions[module];
    if (!modulePerms) {
      return false;
    }

    const rolePerms = modulePerms[user.role];
    return rolePerms ? rolePerms.includes(action) : false;
  }

  /**
   * Check multiple permissions (user must have ALL)
   */
  hasAllPermissions(checks: PermissionCheck[]): boolean {
    return checks.every(check => this.hasPermission(check.module, check.action));
  }

  /**
   * Check multiple permissions (user must have ANY)
   */
  hasAnyPermission(checks: PermissionCheck[]): boolean {
    return checks.some(check => this.hasPermission(check.module, check.action));
  }

  /**
   * Get all permissions for current user
   */
  getUserPermissions(): Observable<Permission[]> {
    return this.apiService.get<Permission[]>('permissions/user');
  }

  /**
   * Check if user can view module
   */
  canViewModule(module: string): boolean {
    return this.hasPermission(module, 'view');
  }

  /**
   * Check if user can create in module
   */
  canCreate(module: string): boolean {
    return this.hasPermission(module, 'create');
  }

  /**
   * Check if user can edit in module
   */
  canEdit(module: string): boolean {
    return this.hasPermission(module, 'edit');
  }

  /**
   * Check if user can delete in module
   */
  canDelete(module: string): boolean {
    return this.hasPermission(module, 'delete');
  }

  /**
   * Check if user can approve
   */
  canApprove(module: string): boolean {
    return this.hasPermission(module, 'approve');
  }

  /**
   * Check if user can manage module
   */
  canManage(module: string): boolean {
    return this.hasPermission(module, 'manage');
  }

  /**
   * Get allowed actions for module
   */
  getAllowedActions(module: string): string[] {
    const user = this.authService.getCurrentUser();
    
    if (!user) {
      return [];
    }

    if (user.role === UserRole.Admin) {
      return ['view', 'create', 'edit', 'delete', 'manage', 'approve', 'reject'];
    }

    const modulePerms = this.modulePermissions[module];
    if (!modulePerms) {
      return [];
    }

    return modulePerms[user.role] || [];
  }

  /**
   * Check if user is Admin
   */
  isAdmin(): boolean {
    return this.authService.hasRole(UserRole.Admin);
  }

  /**
   * Check if user is Factory role
   */
  isFactory(): boolean {
    return this.authService.hasRole(UserRole.Factory);
  }

  /**
   * Check if user is Site role
   */
  isSite(): boolean {
    return this.authService.hasRole(UserRole.Site);
  }

  /**
   * Check if user is Viewer role
   */
  isViewer(): boolean {
    return this.authService.hasRole(UserRole.Viewer);
  }
}
