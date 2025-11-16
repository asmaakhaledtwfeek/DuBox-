import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { ApiService } from './api.service';
import { User, LoginRequest, LoginResponse, AuthState } from '../models/user.model';

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
   * Login user
   */
  login(credentials: LoginRequest): Observable<LoginResponse> {
    this.setLoading(true);
    
    return this.apiService.post<LoginResponse>('auth/login', credentials)
      .pipe(
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
   * Refresh authentication token
   */
  refreshToken(): Observable<LoginResponse> {
    const refreshToken = this.getRefreshToken();
    
    if (!refreshToken) {
      this.logout();
      throw new Error('No refresh token available');
    }

    return this.apiService.post<LoginResponse>('auth/refresh-token', { refreshToken })
      .pipe(
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
   * Check if user has specific role
   */
  hasRole(role: string): boolean {
    const user = this.getCurrentUser();
    return user?.role === role;
  }

  /**
   * Check if user has any of the specified roles
   */
  hasAnyRole(roles: string[]): boolean {
    const user = this.getCurrentUser();
    return user ? roles.includes(user.role) : false;
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
