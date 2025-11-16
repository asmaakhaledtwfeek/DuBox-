import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { PermissionService } from '../../../core/services/permission.service';
import { AuthService } from '../../../core/services/auth.service';

interface MenuItem {
  label: string;
  icon: string;
  route: string;
  permission?: { module: string; action: string };
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
        label: 'Dashboard',
        icon: 'dashboard',
        route: '/projects',
        permission: { module: 'projects', action: 'view' }
      },
      {
        label: 'Projects',
        icon: 'projects',
        route: '/projects',
        permission: { module: 'projects', action: 'view' }
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
        permission: { module: 'users', action: 'view' }
      }
    ];

    this.menuItems = allMenuItems.filter(item => {
      if (!item.permission) return true;
      return this.permissionService.hasPermission(item.permission.module, item.permission.action);
    });
  }

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
  }

  isActive(route: string): boolean {
    return this.activeRoute.startsWith(route);
  }

  getIconSvg(icon: string): string {
    const icons: Record<string, string> = {
      dashboard: '<path d="M3 3h7v7H3z"/><path d="M14 3h7v7h-7z"/><path d="M14 14h7v7h-7z"/><path d="M3 14h7v7H3z"/>',
      projects: '<path d="M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z"/>',
      notifications: '<path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"/><path d="M13.73 21a2 2 0 0 1-3.46 0"/>',
      admin: '<path d="M12 2v20"/><path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"/>'
    };
    return icons[icon] || icons['dashboard'];
  }
}
