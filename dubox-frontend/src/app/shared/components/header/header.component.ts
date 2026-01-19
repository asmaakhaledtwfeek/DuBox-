import { Component, OnInit, HostListener, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { User, UserRole, getUserPrimaryRole } from '../../../core/models/user.model';
import { SidebarService } from '../../../core/services/sidebar.service';
import { ToastService } from '../../../core/services/toast.service';
import { Subscription } from 'rxjs';

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
  toasts: Array<{ message: string; type: string; id: number }> = [];
  private toastSubscription?: Subscription;
  private toastIdCounter = 0;

  constructor(
    private authService: AuthService,
    private router: Router,
    private sidebarService: SidebarService,
    private toastService: ToastService
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
    });
    
    // Subscribe to toast service
    this.toastSubscription = this.toastService.toast$.subscribe(toast => {
      this.showToast(toast.message, toast.type, toast.duration);
    });
    
    // Listen for custom app-toast events (for backward compatibility)
    document.addEventListener('app-toast', this.handleCustomToastEvent.bind(this));
  }
  
  ngOnDestroy(): void {
    this.toastSubscription?.unsubscribe();
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
