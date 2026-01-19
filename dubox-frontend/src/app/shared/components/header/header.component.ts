import { Component, OnInit, HostListener, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { User, UserRole, getUserPrimaryRole } from '../../../core/models/user.model';
import { SidebarService } from '../../../core/services/sidebar.service';
import { ToastService } from '../../../core/services/toast.service';
import { SignalRService } from '../../../core/services/signalr.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  primaryRole: UserRole | null = null;
  unreadNotifications = 0;
  showUserMenu = false;
  isMobile = false;
  
  // Toast notifications
  toasts: Array<{ message: string; type: string; id: number; notificationData?: any }> = [];
  private toastSubscription?: Subscription;
  private toastIdCounter = 0;
  
  // SignalR subscriptions
  private signalRSubscription?: Subscription;
  private notificationCountSubscription?: Subscription;
  private routerSubscription?: Subscription;

  constructor(
    private authService: AuthService,
    private router: Router,
    private sidebarService: SidebarService,
    private toastService: ToastService,
    private signalRService: SignalRService,
    private notificationService: NotificationService
  ) {
    this.checkMobile();
  }

  @HostListener('window:resize', ['$event'])
  onResize(): void {
    this.checkMobile();
  }

  checkMobile(): void {
    this.isMobile = window.innerWidth <= 768;
  }

  toggleMobileSidebar(): void {
    this.sidebarService.toggleSidebar();
  }

  ngOnInit(): void {
    this.authService.authState$.subscribe(state => {
      this.currentUser = state.user;
      this.primaryRole = getUserPrimaryRole(state.user);
      
      if (state.isAuthenticated) {
        // Load initial notification count
        this.loadNotificationCount();
      }
    });
    
    // Subscribe to toast service
    this.toastSubscription = this.toastService.toast$.subscribe(toast => {
      this.showToast(toast.message, toast.type, toast.duration);
    });
    
    // Subscribe to SignalR real-time notification count updates
    this.signalRSubscription = this.signalRService.notificationCount$.subscribe(count => {
      if (count !== null && count !== this.unreadNotifications) {
        this.unreadNotifications = count;
      }
    });
    
    // Subscribe to notification service count updates (for manual refresh)
    this.notificationCountSubscription = this.notificationService.unreadCount$.subscribe(count => {
      this.unreadNotifications = count;
    });
    
    // Subscribe to new notifications for toast display
    this.signalRService.notificationReceived$.subscribe(notification => {
      if (notification) {
        this.showNotificationToast(notification);
      }
    });
    
    // Clear notification toasts when navigating to the related page
    this.routerSubscription = this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        // Check if any toast should be dismissed based on the current URL
        this.toasts = this.toasts.filter(toast => {
          if (toast.notificationData && toast.notificationData.directLink) {
            // If the current URL matches the notification's direct link, remove the toast
            return !event.url.includes(toast.notificationData.directLink);
          }
          return true;
        });
      });
    
    // Listen for custom app-toast events (for backward compatibility)
    document.addEventListener('app-toast', this.handleCustomToastEvent.bind(this));
  }
  
  private loadNotificationCount(): void {
    this.notificationService.getUnreadCount().subscribe({
      next: (result) => {
        this.unreadNotifications = result.count || 0;
      },
      error: (error) => {
        console.error('Error loading notification count:', error);
      }
    });
  }
  
  ngOnDestroy(): void {
    this.toastSubscription?.unsubscribe();
    this.signalRSubscription?.unsubscribe();
    this.notificationCountSubscription?.unsubscribe();
    this.routerSubscription?.unsubscribe();
    document.removeEventListener('app-toast', this.handleCustomToastEvent.bind(this));
  }
  
  private handleCustomToastEvent(event: Event): void {
    const customEvent = event as CustomEvent;
    if (customEvent.detail) {
      this.showToast(
        customEvent.detail.message,
        customEvent.detail.type || 'info',
        customEvent.detail.duration || 3000
      );
    }
  }
  
  private showToast(message: string, type: string, duration: number = 3000): void {
    const id = this.toastIdCounter++;
    this.toasts.push({ message, type, id });
    
    setTimeout(() => {
      this.removeToast(id);
    }, duration);
  }
  
  private showNotificationToast(notification: any): void {
    const id = this.toastIdCounter++;
    const message = notification.title || 'New Notification';
    
    this.toasts.push({ 
      message, 
      type: 'info', 
      id,
      notificationData: notification 
    });
    
    // Auto-dismiss after 8 seconds
    setTimeout(() => {
      this.removeToast(id);
    }, 8000);
  }
  
  handleToastClick(toast: any): void {
    // If toast has notification data, navigate to it
    if (toast.notificationData && toast.notificationData.directLink) {
      this.router.navigateByUrl(toast.notificationData.directLink);
    }
    // Remove the toast
    this.removeToast(toast.id);
  }
  
  removeToast(id: number): void {
    this.toasts = this.toasts.filter(toast => toast.id !== id);
  }

  toggleUserMenu(): void {
    this.showUserMenu = !this.showUserMenu;
  }

  navigateToNotifications(): void {
    this.router.navigate(['/notifications']);
  }

  navigateToProfile(): void {
    this.showUserMenu = false;
    this.router.navigate(['/profile']);
  }

  getRoleDisplayName(role: UserRole | null): string {
    if (!role) return 'User';
    
    const roleNames: Record<UserRole, string> = {
      [UserRole.SystemAdmin]: 'System Admin',
      [UserRole.ProjectManager]: 'Project Manager',
      [UserRole.SiteEngineer]: 'Site Engineer',
      [UserRole.Foreman]: 'Foreman',
      [UserRole.QCInspector]: 'QC Inspector',
      [UserRole.ProcurementOfficer]: 'Procurement Officer',
      [UserRole.HSEOfficer]: 'HSE Officer',
      [UserRole.CostEstimator]: 'Cost Estimator',
      [UserRole.Viewer]: 'Viewer'
    };
    
    return roleNames[role] || role;
  }

  getDepartmentInfo(): string {
    if (!this.currentUser) return '';
    
    const parts = [];
    if (this.currentUser.department) {
      parts.push(this.currentUser.department);
    }
    
    return parts.join(' • ');
  }

  logout(): void {
    this.authService.logout();
  }
}
