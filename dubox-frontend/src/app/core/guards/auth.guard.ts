import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { PermissionService } from '../services/permission.service';
import { filter, take, map } from 'rxjs/operators';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const permissionService = inject(PermissionService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    // Store the attempted URL for redirecting
    router.navigate(['/login'], {
      queryParams: { returnUrl: state.url }
    });
    return false;
  }

  // If permissions are already loaded, allow navigation
  if (permissionService.arePermissionsLoaded()) {
    return true;
  }

  // Wait for permissions to load before allowing navigation
  console.log('⏳ Auth guard: Waiting for permissions to load...');
  
  // Initialize permissions if not already loading
  if (!permissionService.arePermissionsLoaded()) {
    permissionService.initializePermissions().subscribe();
  }

  // Return observable that resolves when permissions are loaded
  return permissionService.permissions$.pipe(
    filter(() => permissionService.arePermissionsLoaded()),
    take(1),
    map(() => {
      console.log('✅ Auth guard: Permissions loaded, allowing navigation');
      return true;
    })
  );
};
