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

export interface NavigationMenuItemDto {
  menuItemId: string;
  label: string;
  icon: string;
  route: string;
  aliases?: string[];
  permissionModule: string;
  permissionAction: string;
  displayOrder: number;
  isActive: boolean;
  children?: NavigationMenuItemDto[];
}

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  private readonly permissionsEndpoint = 'permissions';
  private readonly PERMISSIONS_STORAGE_KEY = 'user_permissions';
  
  // Cache for user permissions loaded from backend
  private userPermissionsCache = new BehaviorSubject<string[]>([]);
  private permissionsLoaded = false;
  private permissionsLoading = false;

  constructor(
    private authService: AuthService,
    private apiService: ApiService
  ) {
    // Load permissions from localStorage immediately on service creation
    this.loadPermissionsFromStorage();
  }

  /**
   * Check if user has permission for a specific module and action
   * ONLY uses backend permissions - no fallback
   * 
   * @param module - The permission module (e.g., 'projects', 'boxes')
   * @param action - The permission action (e.g., 'view', 'create', 'edit')
   * @returns true if user has the permission, false otherwise
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

    // Build the permission key to check (e.g., "projects.view" - lowercase with dot)
    const permissionKey = `${module.toLowerCase()}.${action.toLowerCase()}`;
    const cachedPermissions = this.userPermissionsCache.value;
    
    // Check if permissions are loaded from backend
    if (!this.permissionsLoaded) {
      return false;
    }

    // Check backend permissions
    const hasPermission = cachedPermissions.some(p => 
      p.toLowerCase() === permissionKey
    );
    
    return hasPermission;
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
   * Get allowed actions for a specific module from backend permissions
   */
  getAllowedActions(module: string): string[] {
    const user = this.authService.getCurrentUser();
    
    if (!user || !user.allRoles || user.allRoles.length === 0) {
      return [];
    }

    // SystemAdmin has all actions
    if (user.allRoles.includes(UserRole.SystemAdmin)) {
      return ['view', 'create', 'edit', 'delete', 'manage', 'approve', 'reject', 'export', 'import', 'review', 'resolve', 'assign-team', 'assign-roles', 'assign-groups', 'update-progress', 'update-status', 'restock', 'manage-members', 'change-status', 'send'];
    }

    // Extract actions from cached permissions for the specified module
    const cachedPermissions = this.userPermissionsCache.value;
    const modulePrefix = `${module.toLowerCase()}.`;
    const allActions = new Set<string>();

    cachedPermissions.forEach(permissionKey => {
      const lowerKey = permissionKey.toLowerCase();
      if (lowerKey.startsWith(modulePrefix)) {
        const action = lowerKey.substring(modulePrefix.length);
        if (action) {
          allActions.add(action);
        }
      }
    });

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
   * Load permissions from localStorage (synchronous)
   * Called on service initialization to restore cached permissions
   */
  private loadPermissionsFromStorage(): void {
    try {
      const storedPermissions = localStorage.getItem(this.PERMISSIONS_STORAGE_KEY);
      if (storedPermissions) {
        const permissions = JSON.parse(storedPermissions);
        if (Array.isArray(permissions)) {
          this.userPermissionsCache.next(permissions);
          this.permissionsLoaded = true;
        }
      }
    } catch (err) {
      console.error('Failed to load permissions from storage:', err);
    }
  }

  /**
   * Save permissions to localStorage (synchronous)
   */
  private savePermissionsToStorage(permissions: string[]): void {
    try {
      localStorage.setItem(this.PERMISSIONS_STORAGE_KEY, JSON.stringify(permissions));
    } catch (err) {
      console.error('Failed to save permissions to storage:', err);
    }
  }

  /**
   * Remove permissions from localStorage
   */
  private clearPermissionsFromStorage(): void {
    try {
      localStorage.removeItem(this.PERMISSIONS_STORAGE_KEY);
    } catch (err) {
      console.error('Failed to clear permissions from storage:', err);
    }
  }

  /**
   * Load and cache current user's permissions from backend
   * This is the ONLY source of truth for permissions
   */
  loadCurrentUserPermissions(): Observable<string[]> {
    const user = this.authService.getCurrentUser();
    if (!user) {
      this.permissionsLoaded = true;
      return of([]);
    }

    // Prevent multiple simultaneous loads
    if (this.permissionsLoading) {
      return this.userPermissionsCache.asObservable();
    }

    this.permissionsLoading = true;

    return this.getUserPermissionsFromBackend(user.id).pipe(
      map(result => result?.permissionKeys || []),
      tap(permissions => {
        this.userPermissionsCache.next(permissions);
        this.savePermissionsToStorage(permissions); // Save to localStorage
        this.permissionsLoaded = true;
        this.permissionsLoading = false;
      }),
      catchError(err => {
        console.error('Failed to load permissions from backend:', err);
        
        this.permissionsLoaded = true;
        this.permissionsLoading = false;
        
        // Return cached permissions if available, otherwise empty array
        return of(this.userPermissionsCache.value);
      })
    );
  }

  /**
   * Initialize permissions - call after login
   */
  initializePermissions(): void {
    this.loadCurrentUserPermissions().subscribe();
  }

  /**
   * Clear cached permissions - call on logout
   */
  clearPermissions(): void {
    this.userPermissionsCache.next([]);
    this.clearPermissionsFromStorage(); // Clear from localStorage
    this.permissionsLoaded = false;
    this.permissionsLoading = false;
    this.clearNavigationMenuCache(); // Clear navigation menu cache
  }

  /**
   * Check if permissions have been loaded from backend
   */
  arePermissionsLoaded(): boolean {
    return this.permissionsLoaded;
  }

  /**
   * Check if user has a specific permission key (from backend)
   * 
   * @param permissionKey - The full permission key (e.g., "projects.view")
   * @returns true if user has the permission, false otherwise
   */
  hasPermissionKey(permissionKey: string): boolean {
    const user = this.authService.getCurrentUser();
    
    // SystemAdmin has all permissions
    if (user?.allRoles?.includes(UserRole.SystemAdmin)) {
      return true;
    }

    const cachedPermissions = this.userPermissionsCache.value;
    return cachedPermissions.some(p => p.toLowerCase() === permissionKey.toLowerCase());
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

  // ============= Navigation Menu Items =============

  private navigationMenuCache: NavigationMenuItemDto[] | null = null;
  private navigationMenuLoading = false;

  /**
   * Get navigation menu items from database (cached)
   * This ensures the API is called only ONCE even if sidebar component is recreated
   */
  getNavigationMenuItems(): Observable<NavigationMenuItemDto[]> {
    // Return cached menu if available
    if (this.navigationMenuCache !== null) {
      console.log('📋 Returning cached navigation menu items (no API call)');
      return of(this.navigationMenuCache);
    }

    // Prevent multiple simultaneous API calls
    if (this.navigationMenuLoading) {
      console.log('📋 Navigation menu already loading, waiting...');
      // Wait a bit and retry (menu will be cached by then)
      return new Observable<NavigationMenuItemDto[]>(observer => {
        const checkInterval = setInterval(() => {
          if (this.navigationMenuCache !== null) {
            clearInterval(checkInterval);
            observer.next(this.navigationMenuCache);
            observer.complete();
          }
        }, 100);
      });
    }

    console.log('📋 Loading navigation menu items from backend (one-time API call)');
    this.navigationMenuLoading = true;

    return this.apiService.get<NavigationMenuItemDto[]>('navigation/menu').pipe(
      tap(items => {
        this.navigationMenuCache = items;
        this.navigationMenuLoading = false;
        console.log('✅ Navigation menu cached successfully:', items.length, 'items');
      }),
      catchError(err => {
        console.warn('⚠️ Failed to load navigation menu from backend:', err);
        this.navigationMenuCache = []; // Cache empty array to prevent retries
        this.navigationMenuLoading = false;
        return of([]);
      })
    );
  }

  /**
   * Clear navigation menu cache (call on logout)
   */
  clearNavigationMenuCache(): void {
    this.navigationMenuCache = null;
    this.navigationMenuLoading = false;
  }
}
