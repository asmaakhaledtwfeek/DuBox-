import { Injectable } from '@angular/core';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';
import { Permission, UserRole } from '../models/user.model';

export interface PermissionCheck {
  module: string;
  action: string;
}

export interface PermissionDto {
  permissionId: string;
  module: string;
  action: string;
  permissionKey: string;
  displayName?: string;
  description?: string;
  category?: string;
  displayOrder: number;
  isActive: boolean;
}

export interface PermissionGroupDto {
  category: string;
  module: string;
  permissions: PermissionDto[];
}

export interface RolePermissionMatrixDto {
  roleId: string;
  roleName: string;
  roleDescription?: string;
  permissionKeys: string[];
}

export interface UserPermissionsDto {
  userId: string;
  email: string;
  allRoles: string[];
  permissionKeys: string[];
}

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  private readonly permissionsEndpoint = 'permissions';
  
  // Cache for user permissions loaded from backend
  private userPermissionsCache = new BehaviorSubject<string[]>([]);
  private permissionsLoaded = false;
  
  // Define module permissions based on Group AMANA roles (fallback if backend not available)
  private readonly modulePermissions: Record<string, Record<UserRole, string[]>> = {
    // Projects Module
    projects: {
      [UserRole.SystemAdmin]: ['view', 'create', 'edit', 'delete', 'manage', 'export'],
      [UserRole.ProjectManager]: ['view', 'create', 'edit', 'delete', 'manage', 'export'],
      [UserRole.DesignEngineer]: ['view', 'create', 'edit', 'export'],
      [UserRole.SiteEngineer]: ['view', 'edit', 'export'],
      [UserRole.CostEstimator]: ['view', 'export'],
      [UserRole.Foreman]: ['view'],
      [UserRole.QCInspector]: ['view'],
      [UserRole.ProcurementOfficer]: ['view'],
      [UserRole.HSEOfficer]: ['view'],
      [UserRole.Viewer]: ['view']
    },
    
    // Boxes Module
    boxes: {
      [UserRole.SystemAdmin]: ['view', 'create', 'edit', 'delete', 'update-status', 'manage', 'export'],
      [UserRole.ProjectManager]: ['view', 'create', 'edit', 'delete', 'update-status', 'manage', 'export'],
      [UserRole.DesignEngineer]: ['view', 'create', 'edit', 'update-status', 'export'],
      [UserRole.SiteEngineer]: ['view', 'edit', 'update-status', 'export'],
      [UserRole.Foreman]: ['view', 'update-status'],
      [UserRole.QCInspector]: ['view', 'update-status'],
      [UserRole.CostEstimator]: ['view', 'export'],
      [UserRole.ProcurementOfficer]: ['view'],
      [UserRole.HSEOfficer]: ['view'],
      [UserRole.Viewer]: ['view']
    },
    
    // Activities Module
    activities: {
      [UserRole.SystemAdmin]: ['view', 'create', 'edit', 'delete', 'approve', 'reject', 'manage'],
      [UserRole.ProjectManager]: ['view', 'create', 'edit', 'delete', 'approve', 'reject', 'manage'],
      [UserRole.DesignEngineer]: ['view', 'create', 'edit', 'submit'],
      [UserRole.SiteEngineer]: ['view', 'create', 'edit', 'approve', 'reject'],
      [UserRole.Foreman]: ['view', 'edit', 'submit'],
      [UserRole.QCInspector]: ['view', 'approve', 'reject'],
      [UserRole.CostEstimator]: ['view'],
      [UserRole.ProcurementOfficer]: ['view'],
      [UserRole.HSEOfficer]: ['view'],
      [UserRole.Viewer]: ['view']
    },
    
    // QA/QC Module
    qaqc: {
      [UserRole.SystemAdmin]: ['view', 'create', 'edit', 'approve', 'reject', 'manage'],
      [UserRole.ProjectManager]: ['view', 'create', 'edit', 'approve', 'reject', 'manage'],
      [UserRole.QCInspector]: ['view', 'create', 'edit', 'approve', 'reject'],
      [UserRole.SiteEngineer]: ['view', 'approve', 'reject'],
      [UserRole.DesignEngineer]: ['view', 'submit'],
      [UserRole.Foreman]: ['view', 'submit'],
      [UserRole.CostEstimator]: ['view'],
      [UserRole.ProcurementOfficer]: ['view'],
      [UserRole.HSEOfficer]: ['view', 'approve'],
      [UserRole.Viewer]: ['view']
    },
    
    // Users & Admin Module
    users: {
      [UserRole.SystemAdmin]: ['view', 'create', 'edit', 'delete', 'manage', 'assign-roles'],
      [UserRole.ProjectManager]: ['view', 'create', 'edit'],
      [UserRole.DesignEngineer]: [],
      [UserRole.SiteEngineer]: [],
      [UserRole.Foreman]: [],
      [UserRole.QCInspector]: [],
      [UserRole.ProcurementOfficer]: [],
      [UserRole.CostEstimator]: [],
      [UserRole.HSEOfficer]: [],
      [UserRole.Viewer]: []
    },
    
    // Reports Module
    reports: {
      [UserRole.SystemAdmin]: ['view', 'create', 'export', 'manage'],
      [UserRole.ProjectManager]: ['view', 'create', 'export'],
      [UserRole.CostEstimator]: ['view', 'create', 'export'],
      [UserRole.DesignEngineer]: ['view', 'export'],
      [UserRole.SiteEngineer]: ['view', 'export'],
      [UserRole.QCInspector]: ['view', 'export'],
      [UserRole.Foreman]: ['view'],
      [UserRole.ProcurementOfficer]: ['view', 'export'],
      [UserRole.HSEOfficer]: ['view', 'export'],
      [UserRole.Viewer]: ['view']
    },
    
    // Notifications Module
    notifications: {
      [UserRole.SystemAdmin]: ['view', 'send', 'manage'],
      [UserRole.ProjectManager]: ['view', 'send'],
      [UserRole.DesignEngineer]: ['view'],
      [UserRole.SiteEngineer]: ['view'],
      [UserRole.Foreman]: ['view'],
      [UserRole.QCInspector]: ['view'],
      [UserRole.ProcurementOfficer]: ['view'],
      [UserRole.CostEstimator]: ['view'],
      [UserRole.HSEOfficer]: ['view'],
      [UserRole.Viewer]: ['view']
    },
    
    // Procurement Module
    procurement: {
      [UserRole.SystemAdmin]: ['view', 'create', 'edit', 'delete', 'approve', 'manage'],
      [UserRole.ProjectManager]: ['view', 'create', 'approve'],
      [UserRole.ProcurementOfficer]: ['view', 'create', 'edit', 'approve'],
      [UserRole.CostEstimator]: ['view', 'create'],
      [UserRole.DesignEngineer]: ['view'],
      [UserRole.SiteEngineer]: ['view'],
      [UserRole.Foreman]: ['view'],
      [UserRole.QCInspector]: ['view'],
      [UserRole.HSEOfficer]: ['view'],
      [UserRole.Viewer]: ['view']
    },
    
    // HSE (Health, Safety, Environment) Module
    hse: {
      [UserRole.SystemAdmin]: ['view', 'create', 'edit', 'delete', 'manage'],
      [UserRole.ProjectManager]: ['view', 'create', 'edit'],
      [UserRole.HSEOfficer]: ['view', 'create', 'edit', 'approve'],
      [UserRole.SiteEngineer]: ['view', 'create'],
      [UserRole.Foreman]: ['view', 'create'],
      [UserRole.QCInspector]: ['view'],
      [UserRole.DesignEngineer]: ['view'],
      [UserRole.ProcurementOfficer]: ['view'],
      [UserRole.CostEstimator]: ['view'],
      [UserRole.Viewer]: ['view']
    },
    
    // Cost Estimation Module
    costing: {
      [UserRole.SystemAdmin]: ['view', 'create', 'edit', 'delete', 'approve', 'manage'],
      [UserRole.ProjectManager]: ['view', 'create', 'edit', 'approve'],
      [UserRole.CostEstimator]: ['view', 'create', 'edit', 'approve'],
      [UserRole.DesignEngineer]: ['view', 'create'],
      [UserRole.SiteEngineer]: ['view'],
      [UserRole.ProcurementOfficer]: ['view'],
      [UserRole.Foreman]: [],
      [UserRole.QCInspector]: [],
      [UserRole.HSEOfficer]: [],
      [UserRole.Viewer]: ['view']
    },
    
    // Teams Module
    teams: {
      [UserRole.SystemAdmin]: ['view', 'create', 'edit', 'delete', 'manage'],
      [UserRole.ProjectManager]: ['view', 'create', 'edit', 'manage'],
      [UserRole.SiteEngineer]: ['view', 'edit'],
      [UserRole.Foreman]: ['view'],
      [UserRole.DesignEngineer]: ['view'],
      [UserRole.QCInspector]: ['view'],
      [UserRole.ProcurementOfficer]: ['view'],
      [UserRole.HSEOfficer]: ['view'],
      [UserRole.CostEstimator]: ['view'],
      [UserRole.Viewer]: ['view']
    }
  };

  constructor(
    private authService: AuthService,
    private apiService: ApiService
  ) {}

  /**
   * Check if user has permission for a specific module and action
   */
  hasPermission(module: string, action: string): boolean {
    const user = this.authService.getCurrentUser();
    
    if (!user || !user.allRoles || user.allRoles.length === 0) {
      return false;
    }

    // SystemAdmin has all permissions
    if (user.allRoles.includes(UserRole.SystemAdmin)) {
      return true;
    }

    // Check module permissions for all user roles
    const modulePerms = this.modulePermissions[module];
    if (!modulePerms) {
      return false;
    }

    // Check if any of the user's roles grants the permission
    for (const role of user.allRoles) {
      const rolePerms = modulePerms[role];
      if (rolePerms && rolePerms.includes(action)) {
        return true;
      }
    }

    return false;
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
    
    if (!user || !user.allRoles || user.allRoles.length === 0) {
      return [];
    }

    // SystemAdmin has all actions
    if (user.allRoles.includes(UserRole.SystemAdmin)) {
      return ['view', 'create', 'edit', 'delete', 'manage', 'approve', 'reject', 'export'];
    }

    const modulePerms = this.modulePermissions[module];
    if (!modulePerms) {
      return [];
    }

    // Combine all permissions from all user roles
    const allActions = new Set<string>();
    for (const role of user.allRoles) {
      const rolePerms = modulePerms[role];
      if (rolePerms) {
        rolePerms.forEach(action => allActions.add(action));
      }
    }

    return Array.from(allActions);
  }

  /**
   * Check if user has specific role
   */
  hasRole(role: UserRole): boolean {
    const user = this.authService.getCurrentUser();
    return user?.allRoles?.includes(role) ?? false;
  }

  /**
   * Check if user has any of the specified roles
   */
  hasAnyRole(roles: UserRole[]): boolean {
    const user = this.authService.getCurrentUser();
    return user ? roles.some(role => user.allRoles?.includes(role)) : false;
  }

  /**
   * Check if user is System Admin
   */
  isSystemAdmin(): boolean {
    return this.hasRole(UserRole.SystemAdmin);
  }

  /**
   * Check if user is Project Manager
   */
  isProjectManager(): boolean {
    return this.hasRole(UserRole.ProjectManager);
  }

  /**
   * Check if user is Site Engineer
   */
  isSiteEngineer(): boolean {
    return this.hasRole(UserRole.SiteEngineer);
  }

  /**
   * Check if user is QC Inspector
   */
  isQCInspector(): boolean {
    return this.hasRole(UserRole.QCInspector);
  }

  /**
   * Check if user has only Viewer role
   */
  isViewerOnly(): boolean {
    const user = this.authService.getCurrentUser();
    return user ? user.allRoles?.length === 1 && user.allRoles[0] === UserRole.Viewer : false;
  }

  // ============= Backend API Methods =============

  /**
   * Get all permissions from backend
   */
  getAllPermissions(): Observable<PermissionDto[]> {
    return this.apiService.get<PermissionDto[]>(this.permissionsEndpoint).pipe(
      catchError(() => of([]))
    );
  }

  /**
   * Get permissions grouped by category and module
   */
  getPermissionsGrouped(): Observable<PermissionGroupDto[]> {
    return this.apiService.get<PermissionGroupDto[]>(`${this.permissionsEndpoint}/grouped`).pipe(
      catchError(() => of([]))
    );
  }

  /**
   * Get permission matrix for all roles
   */
  getPermissionMatrix(): Observable<RolePermissionMatrixDto[]> {
    return this.apiService.get<RolePermissionMatrixDto[]>(`${this.permissionsEndpoint}/matrix`).pipe(
      catchError(() => of([]))
    );
  }

  /**
   * Get permissions for a specific role
   */
  getRolePermissions(roleId: string): Observable<PermissionDto[]> {
    return this.apiService.get<PermissionDto[]>(`${this.permissionsEndpoint}/role/${roleId}`).pipe(
      catchError(() => of([]))
    );
  }

  /**
   * Get all permissions for a user (from direct roles and group roles)
   */
  getUserPermissionsFromBackend(userId: string): Observable<UserPermissionsDto | null> {
    return this.apiService.get<UserPermissionsDto>(`${this.permissionsEndpoint}/user/${userId}`).pipe(
      catchError(() => of(null))
    );
  }

  /**
   * Assign permissions to a role
   */
  assignPermissionsToRole(roleId: string, permissionIds: string[]): Observable<boolean> {
    return this.apiService.post<boolean>(`${this.permissionsEndpoint}/role/${roleId}`, permissionIds).pipe(
      catchError(() => of(false))
    );
  }

  /**
   * Load and cache current user's permissions from backend
   */
  loadCurrentUserPermissions(): Observable<string[]> {
    const user = this.authService.getCurrentUser();
    if (!user) {
      return of([]);
    }

    return this.getUserPermissionsFromBackend(user.id).pipe(
      map(result => result?.permissionKeys || []),
      tap(permissions => {
        this.userPermissionsCache.next(permissions);
        this.permissionsLoaded = true;
      }),
      catchError(() => {
        this.permissionsLoaded = true;
        return of([]);
      })
    );
  }

  /**
   * Check if user has a specific permission key (from backend)
   */
  hasPermissionKey(permissionKey: string): boolean {
    const cachedPermissions = this.userPermissionsCache.value;
    if (cachedPermissions.length > 0) {
      return cachedPermissions.includes(permissionKey);
    }
    
    // Fallback to role-based check
    return this.hasPermission(
      permissionKey.split('.')[0], 
      permissionKey.split('.')[1] || 'view'
    );
  }

  /**
   * Get cached user permissions
   */
  getCachedPermissions(): string[] {
    return this.userPermissionsCache.value;
  }

  /**
   * Observable for cached permissions
   */
  get permissions$(): Observable<string[]> {
    return this.userPermissionsCache.asObservable();
  }
}
