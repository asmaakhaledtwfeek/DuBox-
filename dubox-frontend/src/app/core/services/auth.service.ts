import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap, map } from 'rxjs';
import { Router } from '@angular/router';
import { ApiService } from './api.service';
import { User, LoginRequest, LoginResponse, AuthState, UserRole, RegisterRequest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'dubox_auth_token';
  private readonly REFRESH_TOKEN_KEY = 'dubox_refresh_token';
  private readonly USER_KEY = 'dubox_user';

  private authStateSubject = new BehaviorSubject<AuthState>({
    user: this.getUserFromStorage(),
    token: this.getToken(),
    isAuthenticated: !!this.getToken(),
    loading: false,
    error: null
  });

  public authState$ = this.authStateSubject.asObservable();

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {}

  /**
   * Transform backend response to match frontend User model
   */
  private transformBackendUser(backendUser: any): User {
    console.log('🔍 Backend user object received:', backendUser);
    console.log('🔍 Backend user fields:', Object.keys(backendUser));
    
    const fullNameParts = (backendUser.fullName || '').split(' ');
    const firstName = fullNameParts[0] || '';
    const lastName = fullNameParts.slice(1).join(' ') || '';

    // Extract roles from backend response or assign based on email (temporary workaround)
    let allRoles = backendUser.allRoles || backendUser.roles || [];
    let directRoles = backendUser.directRoles || [];
    let groups = backendUser.groups || [];

    console.log('🔍 Extracted from backend:', {
      allRoles: allRoles,
      directRoles: directRoles,
      groups: groups,
      department: backendUser.department || backendUser.departmentName
    });

    // If backend doesn't return roles, log warning and use default
    if (!allRoles || allRoles.length === 0) {
      console.error('%c ⚠️ BACKEND NOT UPDATED! ', 'background: #ff0000; color: #fff; font-size: 20px; font-weight: bold; padding: 10px;');
      console.error('%c Backend is not returning role information! ', 'background: #ff6b6b; color: #fff; font-size: 14px; padding: 5px;');
      console.error('%c User: ' + backendUser.email, 'color: #ff6b6b; font-size: 12px;');
      console.error('%c');
      console.error('%c SOLUTION: Rebuild the backend API:', 'background: #4CAF50; color: #fff; font-size: 14px; font-weight: bold; padding: 5px;');
      console.error('%c 1. Stop the running backend (Ctrl+C)', 'color: #4CAF50; font-size: 12px;');
      console.error('%c 2. cd D:/Company/GroupAmana/DuBox-/Dubox.Api', 'color: #4CAF50; font-size: 12px; font-family: monospace;');
      console.error('%c 3. dotnet clean', 'color: #4CAF50; font-size: 12px; font-family: monospace;');
      console.error('%c 4. dotnet build', 'color: #4CAF50; font-size: 12px; font-family: monospace;');
      console.error('%c 5. dotnet run', 'color: #4CAF50; font-size: 12px; font-family: monospace;');
      console.error('%c');
      console.warn('Using default Viewer role as fallback...');
      
      allRoles = [UserRole.Viewer];
      directRoles = [UserRole.Viewer];
    } else {
      console.log('✅ Roles loaded from backend for', backendUser.email);
      console.log('✅ Direct Roles:', directRoles);
      console.log('✅ All Roles:', allRoles);
      console.log('✅ Groups:', groups.map((g: any) => g.groupName || g.name).join(', '));
    }

    return {
      id: backendUser.userId || backendUser.id,
      email: backendUser.email,
      firstName: firstName,
      lastName: lastName,
      department: backendUser.department || backendUser.departmentName,
      allRoles: allRoles,
      directRoles: directRoles,
      groups: groups,
      createdAt: backendUser.createdDate ? new Date(backendUser.createdDate) : undefined,
      updatedAt: backendUser.lastLoginDate ? new Date(backendUser.lastLoginDate) : undefined
    };
  }

  /**
   * Login user
   */
  login(credentials: LoginRequest): Observable<LoginResponse> {
    this.setLoading(true);
    
    return this.apiService.post<any>('auth/login', credentials)
      .pipe(
        map(response => {
          console.log('🔍 Raw login response from backend:', response);
          console.log('🔍 Response structure:', {
            hasToken: !!response.token,
            hasUser: !!response.user,
            hasRefreshToken: !!response.refreshToken,
            hasExpiresIn: !!response.expiresIn,
            userKeys: response.user ? Object.keys(response.user) : []
          });
          
          // Transform the response
          const transformedResponse: LoginResponse = {
            token: response.token,
            refreshToken: response.refreshToken || response.token,
            user: this.transformBackendUser(response.user),
            expiresIn: response.expiresIn || 3600
          };
          
          console.log('✨ Transformed user:', transformedResponse.user);
          console.log('✨ User has roles:', {
            directRoles: transformedResponse.user.directRoles,
            groups: transformedResponse.user.groups,
            allRoles: transformedResponse.user.allRoles
          });
          
          return transformedResponse;
        }),
        tap(response => {
          this.setSession(response);
          this.updateAuthState({
            user: response.user,
            token: response.token,
            isAuthenticated: true,
            loading: false,
            error: null
          });
        })
      );
  }

  /**
   * Register a new user
   */
  register(payload: RegisterRequest): Observable<any> {
    return this.apiService.post<any>('auth/register', payload);
  }

  /**
   * Logout user
   */
  logout(): void {
    this.clearSession();
    this.updateAuthState({
      user: null,
      token: null,
      isAuthenticated: false,
      loading: false,
      error: null
    });
    this.router.navigate(['/login']);
  }

  /**
   * Request password reset
   */
  forgotPassword(email: string): Observable<any> {
    return this.apiService.post('auth/forgot-password', { email });
  }

  /**
   * Reset password
   */
  resetPassword(token: string, newPassword: string): Observable<any> {
    return this.apiService.post('auth/reset-password', { token, newPassword });
  }

  /**
   * Change password for authenticated user
   */
  changePassword(userId: string, currentPassword: string, newPassword: string): Observable<any> {
    return this.apiService.post('auth/change-password', {
      userId,
      currentPassword,
      newPassword
    });
  }

  /**
   * Refresh authentication token
   */
  refreshToken(): Observable<LoginResponse> {
    const refreshToken = this.getRefreshToken();
    
    if (!refreshToken) {
      this.logout();
      throw new Error('No refresh token available');
    }

    return this.apiService.post<any>('auth/refresh-token', { refreshToken })
      .pipe(
        map(response => ({
          token: response.token,
          refreshToken: response.refreshToken || response.token,
          user: this.transformBackendUser(response.user),
          expiresIn: response.expiresIn || 3600
        })),
        tap(response => {
          this.setSession(response);
          this.updateAuthState({
            user: response.user,
            token: response.token,
            isAuthenticated: true,
            loading: false,
            error: null
          });
        })
      );
  }

  /**
   * Get current user
   */
  getCurrentUser(): User | null {
    return this.authStateSubject.value.user;
  }

  /**
   * Get user info (alias for getCurrentUser)
   */
  getUserInfo(): User | null {
    return this.getCurrentUser();
  }

  /**
   * Get authentication token
   */
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  /**
   * Get refresh token
   */
  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  /**
   * Check if user is authenticated
   */
  isAuthenticated(): boolean {
    return this.authStateSubject.value.isAuthenticated && !!this.getToken();
  }

  /**
   * Check if user has specific role (checks allRoles - direct + inherited)
   */
  hasRole(role: UserRole): boolean {
    const user = this.getCurrentUser();
    return user?.allRoles?.includes(role) || false;
  }

  /**
   * Check if user has any of the specified roles
   */
  hasAnyRole(roles: UserRole[]): boolean {
    const user = this.getCurrentUser();
    if (!user || !user.allRoles) return false;
    return roles.some(role => user.allRoles!.includes(role));
  }

  /**
   * Check if user has all specified roles
   */
  hasAllRoles(roles: UserRole[]): boolean {
    const user = this.getCurrentUser();
    if (!user || !user.allRoles) return false;
    return roles.every(role => user.allRoles!.includes(role));
  }

  /**
   * Get user's roles (all - direct + inherited from groups)
   */
  getUserRoles(): UserRole[] {
    const user = this.getCurrentUser();
    // If backend doesn't return allRoles, fallback to directRoles
    if (user?.allRoles && user.allRoles.length > 0) {
      return user.allRoles;
    }
    if (user?.directRoles && user.directRoles.length > 0) {
      return user.directRoles;
    }
    return [];
  }

  /**
   * Get user's direct roles
   */
  getUserDirectRoles(): UserRole[] {
    const user = this.getCurrentUser();
    return user?.directRoles || [];
  }

  /**
   * Get user's groups
   */
  getUserGroups(): string[] {
    const user = this.getCurrentUser();
    return user?.groups?.map(g => g.name) || [];
  }

  /**
   * Get user's department
   */
  getUserDepartment(): string | undefined {
    const user = this.getCurrentUser();
    return user?.department;
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
   * Set session data
   */
  private setSession(authResult: LoginResponse): void {
    localStorage.setItem(this.TOKEN_KEY, authResult.token);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, authResult.refreshToken);
    localStorage.setItem(this.USER_KEY, JSON.stringify(authResult.user));
  }

  /**
   * Clear session data
   */
  private clearSession(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
  }

  /**
   * Get user from storage
   */
  private getUserFromStorage(): User | null {
    const userJson = localStorage.getItem(this.USER_KEY);
    return userJson ? JSON.parse(userJson) : null;
  }

  /**
   * Update authentication state
   */
  private updateAuthState(state: Partial<AuthState>): void {
    this.authStateSubject.next({
      ...this.authStateSubject.value,
      ...state
    });
  }

  /**
   * Set loading state
   */
  private setLoading(loading: boolean): void {
    this.updateAuthState({ loading });
  }

  /**
   * Set error state
   */
  setError(error: string | null): void {
    this.updateAuthState({ error, loading: false });
  }
}
