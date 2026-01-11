import { Component, OnInit, OnDestroy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from './core/services/auth.service';
import { PermissionService } from './core/services/permission.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: '<router-outlet></router-outlet>',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'DuBox - Box Management System';
  private authSubscription?: Subscription;

  constructor(
    private authService: AuthService,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    // Permissions are automatically loaded from localStorage in PermissionService constructor
    // This ensures they're available immediately before any component initializes
    
    // Refresh permissions from backend if user is authenticated (page refresh case)
    // Note: Skip if permissions are already loading to avoid duplicate API calls
    if (this.authService.isAuthenticated() && !this.permissionService.arePermissionsLoaded()) {
      this.permissionService.initializePermissions().subscribe({
        next: () => console.log('✅ Permissions loaded on app init'),
        error: (err) => console.error('❌ Failed to load permissions on app init:', err)
      });
    }

    // Subscribe to auth state changes to clear permissions on logout
    // Note: We DON'T load permissions here because login.component.ts handles that
    this.authSubscription = this.authService.authState$.subscribe(state => {
      if (!state.isAuthenticated) {
        // Clear permissions on logout
        this.permissionService.clearPermissions();
        console.log('🧹 Permissions cleared after logout');
      }
    });
  }

  ngOnDestroy(): void {
    this.authSubscription?.unsubscribe();
  }
}
