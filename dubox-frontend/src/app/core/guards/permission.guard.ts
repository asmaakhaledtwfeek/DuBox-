import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { PermissionService } from '../services/permission.service';
import { map, take, filter } from 'rxjs/operators';

/**
 * Permission guard that checks if user has required permission before allowing route access
 * This guard waits for permissions to be loaded from backend before making a decision
 */
export const permissionGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const permissionService = inject(PermissionService);
  const router = inject(Router);

  // Check authentication first
  if (!authService.isAuthenticated()) {
    console.warn('🔐 Not authenticated, redirecting to login');
    router.navigate(['/login']);
    return false;
  }

  // Get required permission from route data
  const permission = route.data['permission'] as { module: string; action: string };
  
  // If no permission specified, allow access
  if (!permission || !permission.module || !permission.action) {
    console.warn('⚠️ No permission specified in route data, allowing access');
    return true;
  }

  // Wait for permissions to load from backend, then check
  // Skip initial empty value and take first non-empty value or wait for loaded flag
  if (!permissionService.arePermissionsLoaded()) {
    console.log('⏳ Permissions not yet loaded, waiting...');
    
    // Return observable that resolves when permissions are loaded
    return permissionService.permissions$.pipe(
      // Wait until permissions are loaded (either with data or empty but loaded)
      filter(() => permissionService.arePermissionsLoaded()),
      take(1),
      map(() => {
        const hasPermission = permissionService.hasPermission(permission.module, permission.action);
        
        if (hasPermission) {
          console.log(`✅ Permission granted: ${permission.module}.${permission.action}`);
          return true;
        }

        console.warn(`🔐 Access denied. Required permission: ${permission.module}.${permission.action}`);
        router.navigate(['/unauthorized']);
        return false;
      })
    );
  }

  // Permissions already loaded, check immediately
  const hasPermission = permissionService.hasPermission(permission.module, permission.action);
  
  if (hasPermission) {
    console.log(`✅ Permission granted: ${permission.module}.${permission.action}`);
    return true;
  }

  // User doesn't have required permission
  console.warn(`🔐 Access denied. Required permission: ${permission.module}.${permission.action}`);
  router.navigate(['/unauthorized']);
  return false;
};

