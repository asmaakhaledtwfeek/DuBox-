import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserRole } from '../models/user.model';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    router.navigate(['/login']);
    return false;
  }

  // Get required roles from route data
  const expectedRoles = route.data['roles'] as UserRole[];
  
  // If no roles specified, allow access
  if (!expectedRoles || expectedRoles.length === 0) {
    return true;
  }

  // Check if user has any of the required roles
  if (authService.hasAnyRole(expectedRoles)) {
    return true;
  }

  // User doesn't have required role
  console.warn('Access denied. Required roles:', expectedRoles);
  router.navigate(['/unauthorized']);
  return false;
};
