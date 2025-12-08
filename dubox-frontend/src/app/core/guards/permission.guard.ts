import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { PermissionService } from '../services/permission.service';

export const permissionGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const permissionService = inject(PermissionService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    router.navigate(['/login']);
    return false;
  }

  // Get required permission from route data
  const permission = route.data['permission'] as { module: string; action: string };
  
  // If no permission specified, allow access
  if (!permission || !permission.module || !permission.action) {
    console.warn('⚠️ No permission specified in route data');
    return true;
  }

  // Check if user has the required permission
  const hasPermission = permissionService.hasPermission(permission.module, permission.action);
  
  if (hasPermission) {
    return true;
  }

  // User doesn't have required permission
  console.warn('🔐 Access denied. Required permission:', permission);
  router.navigate(['/unauthorized']);
  return false;
};

