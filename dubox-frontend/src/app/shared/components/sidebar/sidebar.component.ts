import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { PermissionService } from '../../../core/services/permission.service';
import { AuthService } from '../../../core/services/auth.service';
import { UserRole } from '../../../core/models/user.model';

interface MenuItem {
  label: string;
  icon: string;
  route: string;
  aliases?: string[];
  permission?: { module: string; action: string };
  requiredRoles?: UserRole[];
  children?: MenuItem[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent implements OnInit {
  isCollapsed = false;
  activeRoute = '';
  menuItems: MenuItem[] = [];

  constructor(
    private router: Router,
    private permissionService: PermissionService,
    private authService: AuthService
  ) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.activeRoute = event.url;
      });
  }

  ngOnInit(): void {
    this.buildMenuItems();
  }

  buildMenuItems(): void {
    const allMenuItems: MenuItem[] = [
      {
        label: 'Projects',
        icon: 'projects',
        route: '/projects',
        permission: { module: 'projects', action: 'view' }
      },
      {
        label: 'Materials',
        icon: 'materials',
        route: '/materials',
        permission: { module: 'materials', action: 'view' }
      },
      {
        label: 'Teams',
        icon: 'teams',
        route: '/teams',
        permission: { module: 'teams', action: 'view' }
      },
      {
        label: 'Quality Control',
        icon: 'qc',
        route: '/qc',
        aliases: ['/quality'],
        permission: { module: 'qaqc', action: 'view' },
        requiredRoles: [UserRole.QCInspector, UserRole.SystemAdmin, UserRole.ProjectManager]
      },
      {
        label: 'Procurement',
        icon: 'procurement',
        route: '/procurement',
        permission: { module: 'procurement', action: 'view' },
        requiredRoles: [UserRole.ProcurementOfficer, UserRole.SystemAdmin, UserRole.ProjectManager]
      },
      {
        label: 'HSE',
        icon: 'hse',
        route: '/hse',
        permission: { module: 'hse', action: 'view' },
        requiredRoles: [UserRole.HSEOfficer, UserRole.SystemAdmin, UserRole.ProjectManager]
      },
      {
        label: 'Cost Estimation',
        icon: 'costing',
        route: '/costing',
        permission: { module: 'costing', action: 'view' },
        requiredRoles: [UserRole.CostEstimator, UserRole.SystemAdmin, UserRole.ProjectManager]
      },
      {
        label: 'Reports',
        icon: 'reports',
        route: '/reports',
        permission: { module: 'reports', action: 'view' }
      },
      {
        label: 'Notifications',
        icon: 'notifications',
        route: '/notifications',
        permission: { module: 'notifications', action: 'view' }
      },
      {
        label: 'Admin',
        icon: 'admin',
        route: '/admin',
        permission: { module: 'users', action: 'view' },
        requiredRoles: [UserRole.SystemAdmin, UserRole.ProjectManager]
      }
    ];

    this.menuItems = allMenuItems.filter(item => {
      // Check permission if specified
      if (item.permission) {
        const hasPermission = this.permissionService.hasPermission(item.permission.module, item.permission.action);
        if (!hasPermission) return false;
      }

      // Check required roles if specified
      if (item.requiredRoles && item.requiredRoles.length > 0) {
        const hasRole = this.authService.hasAnyRole(item.requiredRoles);
        if (!hasRole) return false;
      }

      return true;
    });
  }

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
  }

  isActive(route: string, aliases?: string[]): boolean {
    const match = (path: string) => this.activeRoute === path || this.activeRoute.startsWith(`${path}/`);

    if (match(route)) {
      return true;
    }

    if (aliases?.length) {
      return aliases.some((alias: string) => match(alias));
    }

    return false;
  }

  getIconSvg(icon: string): string {
    const icons: Record<string, string> = {
      dashboard: '<path d="M3 3h7v7H3z"/><path d="M14 3h7v7h-7z"/><path d="M14 14h7v7h-7z"/><path d="M3 14h7v7H3z"/>',
      projects: '<path d="M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z"/>',
      materials: '<path d="M20 7h-4M4 7h4m0 0V5a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2m-6 0h6m-6 0v10m6-10v10"/>',
      teams: '<path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2M23 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75M13 7a4 4 0 1 1-8 0 4 4 0 0 1 8 0z"/>',
      qc: '<path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/><polyline points="22 4 12 14.01 9 11.01"/>',
      procurement: '<path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/>',
      hse: '<path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/>',
      costing: '<line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"/>',
      reports: '<path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="16" y1="13" x2="8" y2="13"/><line x1="16" y1="17" x2="8" y2="17"/><polyline points="10 9 9 9 8 9"/>',
      notifications: '<path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"/><path d="M13.73 21a2 2 0 0 1-3.46 0"/>',
      admin: '<path d="M12 2v20"/><path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"/>'
    };
    return icons[icon] || icons['dashboard'];
  }
}
