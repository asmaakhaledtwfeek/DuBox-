import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-notifications-center',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  template: `
    <app-header></app-header>
    <app-sidebar></app-sidebar>
    <div class="page-container">
      <div class="page-content">
        <h1 class="page-title">Notifications</h1>
        <div class="card" style="padding: var(--spacing-2xl); text-align: center;">
          <p>Notifications Center - Implementation Ready</p>
          <p style="color: var(--gray-600); font-size: var(--font-size-sm); margin-top: var(--spacing-md);">
            This page will display all system notifications with filtering and mark as read functionality.
          </p>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['./notifications-center.component.scss']
})
export class NotificationsCenterComponent {}
