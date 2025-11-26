import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const token = authService.getToken();

  // Public endpoints that don't require authentication
  const publicEndpoints = ['/auth/login', '/auth/register', '/Department'];
  const isPublicEndpoint = publicEndpoints.some(endpoint => req.url.includes(endpoint));

  // Clone the request and add authorization header if token exists
  let authReq = req;
  if (token && !isPublicEndpoint) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(authReq).pipe(
    catchError((error) => {
      console.error('HTTP Error:', error);
      
      // Don't redirect to login if:
      // 1. It's a public endpoint (login, register, public APIs)
      // 2. We're already on a public route (login, register, forgot-password)
      const currentUrl = router.url;
      const isPublicRoute = currentUrl === '/login' || currentUrl === '/register' || currentUrl === '/forgot-password';
      
      if (error.status === 401 && !isPublicEndpoint && !isPublicRoute) {
        // Token expired or invalid - only logout if not on public pages
        console.warn('Unauthorized - logging out');
        authService.logout();
        router.navigate(['/login']);
      }
      
      return throwError(() => error);
    })
  );
};
