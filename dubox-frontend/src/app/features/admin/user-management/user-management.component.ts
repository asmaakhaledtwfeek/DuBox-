import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  template: `
    <app-header></app-header>
    <app-sidebar></app-sidebar>
    <div class="page-container">
      <div class="page-content">
        <h1 class="page-title">User Management</h1>
        <div class="card" style="padding: var(--spacing-2xl); text-align: center;">
          <p>User Management Page - Implementation Ready</p>
          <p style="color: var(--gray-600); font-size: var(--font-size-sm); margin-top: var(--spacing-md);">
            This page will allow admins to create, edit, and manage users and their roles.
          </p>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent {}
