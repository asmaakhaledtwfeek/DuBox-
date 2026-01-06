import { Component, OnInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { User, UserRole, getUserPrimaryRole } from '../../../core/models/user.model';
import { SidebarService } from '../../../core/services/sidebar.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  currentUser: User | null = null;
  primaryRole: UserRole | null = null;
  unreadNotifications = 0;
  showUserMenu = false;
  isMobile = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private sidebarService: SidebarService
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
