import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { PermissionService, NavigationMenuItemDto } from '../../../core/services/permission.service';

interface MenuItem {
  label: string;
  icon: string;
  route: string;
  aliases?: string[];
  permissionModule: string;
  permissionAction: string;
  children?: MenuItem[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent implements OnInit, OnDestroy {
  isCollapsed = false;
  activeRoute = '';
  menuItems: MenuItem[] = [];
  private allMenuItemsFromDb: NavigationMenuItemDto[] = [];
  private subscriptions: Subscription[] = [];
  private menuLoaded = false;

  constructor(
    private router: Router,
    private permissionService: PermissionService
  ) {
    this.subscriptions.push(
      this.router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe((event: any) => {
          this.activeRoute = event.url;
        })
    );
  }

  ngOnInit(): void {
    // Load menu items from database only once
    this.loadMenuItems();

    // Rebuild whenever permissions change (e.g., on first load or role switch)
    this.subscriptions.push(
      this.permissionService.permissions$.subscribe(() => {
        if (this.menuLoaded) {
          this.buildMenuItems();
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadMenuItems(): void {
    // Only load if not already loaded
    if (this.menuLoaded) {
      return;
    }

    this.permissionService.getNavigationMenuItems().subscribe({
      next: (items) => {
        this.allMenuItemsFromDb = items;
        this.menuLoaded = true;
        // Build as soon as permissions are ready
        if (this.permissionService.arePermissionsLoaded()) {
          this.buildMenuItems();
        }
      },

      error: (err) => {
        console.warn('⚠️ Failed to load menu items, using fallback:', err);
        this.menuLoaded = true;
        // Build as soon as permissions are ready
        if (this.permissionService.arePermissionsLoaded()) {
          this.buildMenuItems();
        }
      }
    });
  }

  buildMenuItems(): void {
    // If permissions aren't loaded yet, skip to avoid empty menu; a later emission will rebuild.
    if (!this.permissionService.arePermissionsLoaded()) {
      return;
    }

    let allMenuItems: MenuItem[];

    // Use database menu items if loaded, otherwise use fallback
    if (this.allMenuItemsFromDb.length > 0) {
      allMenuItems = this.allMenuItemsFromDb.map(item => ({
        label: item.label,
        icon: item.icon,
        route: item.route,
        aliases: item.aliases,
        permissionModule: item.permissionModule,
        permissionAction: item.permissionAction,
        children: item.children?.map(child => ({
          label: child.label,
          icon: child.icon,
          route: child.route,
          aliases: child.aliases,
          permissionModule: child.permissionModule,
          permissionAction: child.permissionAction
        }))
      }));
    } else {
      // Fallback menu items if database not available
      allMenuItems = this.getFallbackMenuItems();
    }

    // Ensure Admin menu exists for users with users.view, even if backend filtered it out
    const hasAdmin = allMenuItems.some(
      item => item.route?.startsWith('/admin')
    );
    if (!hasAdmin && this.permissionService.hasPermission('users', 'view')) {
      allMenuItems = [
        ...allMenuItems,
        {
          label: 'Admin',
          icon: 'admin',
          route: '/admin',
          permissionModule: 'users',
          permissionAction: 'view',
          aliases: ['/admin/users']
        }
      ];
    }

    // Filter menu items based on user permissions (done once)
    this.menuItems = allMenuItems.filter(item => {
      const hasExactPermission = this.permissionService.hasPermission(
        item.permissionModule,
        item.permissionAction
      );

      // Special case: allow users with 'users.view' to see the Admin menu
      // even if the menu item action is still 'manage' in the DB seed data.
      if (item.permissionModule === 'users' && item.route?.startsWith('/admin')) {
        return (
          hasExactPermission ||
          this.permissionService.hasPermission('users', 'view')
        );
      }

      return hasExactPermission;
    });
  }

  private getFallbackMenuItems(): MenuItem[] {
    return [
      { label: 'Projects', icon: 'projects', route: '/projects', permissionModule: 'projects', permissionAction: 'view' },
      { label: 'Materials', icon: 'materials', route: '/materials', permissionModule: 'materials', permissionAction: 'view' },
      { label: 'Locations', icon: 'location', route: '/locations', permissionModule: 'locations', permissionAction: 'view' },
      { label: 'Teams', icon: 'teams', route: '/teams', permissionModule: 'teams', permissionAction: 'view' },
      { label: 'Quality Control', icon: 'qc', route: '/qc', aliases: ['/quality'], permissionModule: 'wir', permissionAction: 'view' },
      { label: 'Reports', icon: 'reports', route: '/reports', permissionModule: 'reports', permissionAction: 'view' },
      { label: 'Notifications', icon: 'notifications', route: '/notifications', permissionModule: 'notifications', permissionAction: 'view' },
      { label: 'Admin', icon: 'admin', route: '/admin', permissionModule: 'users', permissionAction: 'view' }
    ];
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
      location: '<path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/><circle cx="12" cy="10" r="3"/>',
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
