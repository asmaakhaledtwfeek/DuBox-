import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { PermissionService, NavigationMenuItemDto } from '../../../core/services/permission.service';
import { SidebarService } from '../../../core/services/sidebar.service';

interface MenuItem {
  label: string;
  icon: string;
  route: string;
  aliases?: string[];
  permissionModule: string;
  permissionAction: string;
  comingSoon?: boolean;
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
  isMobileOpen = false; // Track mobile sidebar state
  activeRoute = '';
  menuItems: MenuItem[] = [];
  private allMenuItemsFromDb: NavigationMenuItemDto[] = [];
  private subscriptions: Subscription[] = [];
  private menuLoaded = false;
  private menuBuilt = false; // Flag to prevent unnecessary rebuilds
  private isLoadingMenu = false; // Flag to prevent multiple API calls

  constructor(
    private router: Router,
    private permissionService: PermissionService,
    private sidebarService: SidebarService
  ) {
    this.subscriptions.push(
      this.router.events
        .pipe(filter(event => event instanceof NavigationEnd))
        .subscribe((event: any) => {
          this.activeRoute = event.url;
          // Close mobile sidebar on navigation
          if (this.isMobileOpen) {
            this.closeMobileSidebar();
          }
          // Don't rebuild menu on navigation - just update active route
        })
    );

    // Subscribe to sidebar toggle from header
    this.subscriptions.push(
      this.sidebarService.toggleSidebar$.subscribe(() => {
        this.toggleSidebar();
      })
    );
  }

  ngOnInit(): void {
    // Load menu items from database only once
    this.loadMenuItems();

    // Rebuild only on first permission load (not on every emission)
    this.subscriptions.push(
      this.permissionService.permissions$.subscribe(() => {
        // Only rebuild if menu is loaded but not yet built
        if (this.menuLoaded && !this.menuBuilt && this.permissionService.arePermissionsLoaded()) {
          this.buildMenuItems();
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  /**
   * Reset menu state (useful for testing or when user switches roles)
   * Call this only if you need to force a reload (e.g., after logout/login)
   */
  resetMenu(): void {
    this.menuLoaded = false;
    this.menuBuilt = false;
    this.isLoadingMenu = false;
    this.allMenuItemsFromDb = [];
    this.menuItems = [];
    // Clear navigation menu cache
    this.permissionService.clearNavigationMenuCache();
    this.loadMenuItems();
  }

  loadMenuItems(): void {
    // Only load if not already loaded and not currently loading
    if (this.menuLoaded || this.isLoadingMenu) {
      return;
    }

    this.isLoadingMenu = true;

    // The service now handles caching, so this won't make duplicate API calls
    this.permissionService.getNavigationMenuItems().subscribe({
      next: (items) => {
        console.log('📋 Loaded menu items from backend:', items.length, 'items');
        console.log('📋 Menu items:', items.map(i => `${i.label} (${i.permissionModule}.${i.permissionAction})`));
        this.allMenuItemsFromDb = items;
        this.menuLoaded = true;
        this.isLoadingMenu = false;
        // Build as soon as permissions are ready
        if (this.permissionService.arePermissionsLoaded()) {
          this.buildMenuItems();
        }
      },

      error: (err) => {
        console.warn('⚠️ Failed to load menu items, using fallback:', err);
        this.menuLoaded = true;
        this.isLoadingMenu = false;
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

    // Skip if menu is already built (prevents rebuilds on navigation)
    if (this.menuBuilt) {
      return;
    }

    let allMenuItems: MenuItem[];

    // Use database menu items if loaded, otherwise use fallback
    if (this.allMenuItemsFromDb.length > 0) {
      allMenuItems = this.allMenuItemsFromDb
        .filter(item => {
          // Filter out Locations menu item (commented out but kept in code)
          return !(item.label?.toLowerCase() === 'locations' || item.route?.toLowerCase() === '/locations');
        })
        .map(item => {
          // Ensure route starts with / for proper navigation
          const normalizedRoute = item.route?.startsWith('/') ? item.route : `/${item.route}`;
          console.log(`🔗 Menu item "${item.label}": route="${item.route}" -> normalized="${normalizedRoute}"`);
          return {
            label: item.label,
            icon: item.icon,
            route: normalizedRoute,
            aliases: item.aliases,
            permissionModule: item.permissionModule,
            permissionAction: item.permissionAction,
            comingSoon: item.comingSoon,
            children: item.children?.map(child => ({
              label: child.label,
              icon: child.icon,
              route: child.route?.startsWith('/') ? child.route : `/${child.route}`,
              aliases: child.aliases,
              permissionModule: child.permissionModule,
              permissionAction: child.permissionAction,
              comingSoon: child.comingSoon
            }))
          };
        });
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
    const filteredItems = allMenuItems.filter(item => {
      const hasExactPermission = this.permissionService.hasPermission(
        item.permissionModule,
        item.permissionAction
      );

      // Debug logging for Factories menu item
      if (item.label === 'Factories' || item.permissionModule === 'factories') {
        console.log('🏭 Checking Factories menu item permission:');
        console.log('  - Module:', item.permissionModule);
        console.log('  - Action:', item.permissionAction);
        console.log('  - Has Permission:', hasExactPermission);
        console.log('  - User has factories.view:', this.permissionService.hasPermission('factories', 'view'));
        console.log('  - All user permissions:', this.permissionService.getCachedPermissions());
      }

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

    // Sort menu items: regular items first, then "Coming Soon" items at the bottom
    this.menuItems = filteredItems.sort((a, b) => {
      // If one has comingSoon and the other doesn't, put comingSoon items at the bottom
      if (a.comingSoon && !b.comingSoon) return 1;
      if (!a.comingSoon && b.comingSoon) return -1;
      // Otherwise maintain original order
      return 0;
    });

    console.log('✅ Filtered menu items:', this.menuItems.length, 'visible items');
    console.log('✅ Visible menu items:', this.menuItems.map(i => i.label));

    // Mark menu as built to prevent rebuilds
    this.menuBuilt = true;
  }

  private getFallbackMenuItems(): MenuItem[] {
    return [
      { label: 'Projects', icon: 'projects', route: '/projects', permissionModule: 'projects', permissionAction: 'view' },
      { label: 'Quality Control', icon: 'qc', route: '/qc', aliases: ['/quality'], permissionModule: 'wir', permissionAction: 'view' },
      { label: 'Teams', icon: 'teams', route: '/teams', permissionModule: 'teams', permissionAction: 'view' },
      { label: 'Materials', icon: 'materials', route: '/materials', permissionModule: 'materials', permissionAction: 'view', comingSoon: true },
      { label: 'Cost', icon: 'cost', route: '/cost', permissionModule: 'cost', permissionAction: 'view', comingSoon: true },
      { label: 'Schedule', icon: 'schedule', route: '/schedule', permissionModule: 'schedule', permissionAction: 'view', comingSoon: true },
      { label: 'Reports', icon: 'reports', route: '/reports', permissionModule: 'reports', permissionAction: 'view' },
      { label: 'Factories', icon: 'factory', route: '/factories', permissionModule: 'factories', permissionAction: 'view' },
      { label: 'BIM', icon: 'bim', route: '/bim', permissionModule: 'bim', permissionAction: 'view', comingSoon: true },
      { label: 'Admin', icon: 'admin', route: '/admin', permissionModule: 'users', permissionAction: 'view' },
      { label: 'Help', icon: 'help', route: '/help', permissionModule: 'help', permissionAction: 'view', comingSoon: true }
    ];
  }

  toggleSidebar(): void {
    // Check if we're on mobile (window width <= 768px)
    if (window.innerWidth <= 768) {
      this.isMobileOpen = !this.isMobileOpen;
      console.log('Mobile sidebar toggled. isMobileOpen:', this.isMobileOpen);
    } else {
      this.isCollapsed = !this.isCollapsed;
    }
  }

  closeMobileSidebar(): void {
    this.isMobileOpen = false;
  }

  @HostListener('window:resize', ['$event'])
  onResize(): void {
    // If screen becomes larger than mobile, close mobile sidebar
    if (window.innerWidth > 768 && this.isMobileOpen) {
      this.isMobileOpen = false;
    }
  }

  onMenuItemClick(item: MenuItem): void {
    console.log(`🖱️ Menu item clicked: "${item.label}" -> route: "${item.route}"`);
    console.log(`📍 Current route: ${this.router.url}`);
    // Close mobile sidebar when menu item is clicked
    if (this.isMobileOpen) {
      this.closeMobileSidebar();
    }
    // routerLink will handle navigation, this is just for debugging
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

  trackByRoute(index: number, item: MenuItem): string {
    return item.route;
  }

  getIconSvg(icon: string): string {
    const icons: Record<string, string> = {
      dashboard: '<path d="M3 3h7v7H3z"/><path d="M14 3h7v7h-7z"/><path d="M14 14h7v7h-7z"/><path d="M3 14h7v7H3z"/>',
      projects: '<path d="M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z"/>',
      materials: '<path d="M20 7h-4M4 7h4m0 0V5a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2m-6 0h6m-6 0v10m6-10v10"/>',
      //location: '<path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/><circle cx="12" cy="10" r="3"/>',
      factory: '<path d="M12 2L2 7v10c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V7l-10-5z"/><path d="M12 8v8M8 12h8"/>',
      teams: '<path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2M23 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75M13 7a4 4 0 1 1-8 0 4 4 0 0 1 8 0z"/>',
      qc: '<path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/><polyline points="22 4 12 14.01 9 11.01"/>',
      procurement: '<path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"/><line x1="3" y1="6" x2="21" y2="6"/><path d="M16 10a4 4 0 0 1-8 0"/>',
      hse: '<path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/>',
      costing: '<line x1="12" y1="1" x2="12" y2="23"/><path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"/>',
      cost: '<circle cx="12" cy="12" r="10"/><line x1="12" y1="1" x2="12" y2="5"/><path d="M16 8H9.5a2.5 2.5 0 0 0 0 5h5a2.5 2.5 0 0 1 0 5H8"/><line x1="12" y1="19" x2="12" y2="23"/>',
      schedule: '<circle cx="12" cy="12" r="10"/><polyline points="12 6 12 12 16 14"/>',
      bim: '<path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/><polyline points="3.27 6.96 12 12.01 20.73 6.96"/><line x1="12" y1="22.08" x2="12" y2="12"/>',
      reports: '<path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="16" y1="13" x2="8" y2="13"/><line x1="16" y1="17" x2="8" y2="17"/><polyline points="10 9 9 9 8 9"/>',
      notifications: '<path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"/><path d="M13.73 21a2 2 0 0 1-3.46 0"/>',
      admin: '<path d="M12 2v20"/><path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"/>',
      help: '<circle cx="12" cy="12" r="10"/><path d="M9.09 9a3 3 0 0 1 5.83 1c0 2-3 3-3 3"/><line x1="12" y1="17" x2="12.01" y2="17"/>'
    };
    return icons[icon] || icons['dashboard'];
  }
}
