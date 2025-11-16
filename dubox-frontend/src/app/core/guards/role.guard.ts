import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    router.navigate(['/login']);
    return false;
  }

  const expectedRoles = route.data['roles'] as string[];
  
  if (!expectedRoles || expectedRoles.length === 0) {
    return true;
  }

  if (authService.hasAnyRole(expectedRoles)) {
    return true;
  }

  // User doesn't have required role
  router.navigate(['/unauthorized']);
  return false;
};
