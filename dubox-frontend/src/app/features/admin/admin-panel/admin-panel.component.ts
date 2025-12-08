import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { PermissionService } from '../../../core/services/permission.service';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  template: `
    <app-header></app-header>
    <app-sidebar></app-sidebar>
    <div class="page-container">
      <div class="page-content">
        <h1 class="page-title">Admin Panel</h1>
        <div class="admin-grid">
          <a routerLink="/admin/users" class="admin-card">
            <svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/>
            </svg>
            <h3>User Management</h3>
            <p>Manage users and roles</p>
          </a>
          <div class="admin-card">
            <svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <circle cx="12" cy="12" r="3"/><path d="M12 1v6m0 6v6"/></svg>
            <h3>Settings</h3>
            <p>System configuration</p>
          </div>
          <a *ngIf="canViewAuditLogs" routerLink="/admin/audit-logs" class="admin-card">
            <svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/>
            </svg>
            <h3>Audit Logs</h3>
            <p>View system logs</p>
          </a>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .page-container {
      margin-left: var(--sidebar-width);
      margin-top: var(--header-height);
      min-height: calc(100vh - var(--header-height));
      background: var(--gray-50);
    }
    .page-content {
      padding: var(--spacing-2xl);
      max-width: 1400px;
      margin: 0 auto;
    }
    .page-title {
      font-size: var(--font-size-3xl);
      font-weight: 700;
      color: var(--gray-900);
      margin: 0 0 var(--spacing-2xl) 0;
    }
    .admin-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
      gap: var(--spacing-lg);
    }
    .admin-card {
      background: var(--white);
      border-radius: var(--radius-lg);
      box-shadow: var(--shadow-md);
      padding: var(--spacing-2xl);
      text-align: center;
      transition: transform var(--transition-fast);
      text-decoration: none;
      color: inherit;
      display: block;
    }
    .admin-card:hover {
      transform: translateY(-4px);
      box-shadow: var(--shadow-xl);
    }
    .admin-card svg {
      color: var(--primary-color);
      margin-bottom: var(--spacing-md);
    }
    .admin-card h3 {
      font-size: var(--font-size-xl);
      margin: 0 0 var(--spacing-sm) 0;
    }
    .admin-card p {
      color: var(--gray-600);
      font-size: var(--font-size-sm);
      margin: 0;
    }
    @media (max-width: 768px) {
      .page-container {
        margin-left: 0;
      }
    }
  `]
})
export class AdminPanelComponent implements OnInit {
  canViewAuditLogs = false;

  constructor(private permissionService: PermissionService) {}

  ngOnInit(): void {
    // Check if user has permission to view audit logs
    this.canViewAuditLogs = this.permissionService.hasPermission('audit-logs', 'view');
  }
}
