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
    
    // Refresh permissions from backend if user is authenticated
    // This ensures we have the latest permissions
    if (this.authService.isAuthenticated()) {
      this.permissionService.initializePermissions();
    }

    // Subscribe to auth state changes to load/clear permissions
    this.authSubscription = this.authService.authState$.subscribe(state => {
      if (state.isAuthenticated && state.user) {
        // Always refresh permissions from backend after login
        this.permissionService.initializePermissions();
      } else if (!state.isAuthenticated) {
        this.permissionService.clearPermissions();
      }
    });
  }

  ngOnDestroy(): void {
    this.authSubscription?.unsubscribe();
  }
}
