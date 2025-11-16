import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-box-details',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  template: `
    <app-header></app-header>
    <app-sidebar></app-sidebar>
    <div class="page-container">
      <div class="page-content">
        <h1 class="page-title">Box Details</h1>
        <div class="card" style="padding: var(--spacing-2xl); text-align: center;">
          <p>Box Details Page - Implementation Ready</p>
          <p style="color: var(--gray-600); font-size: var(--font-size-sm); margin-top: var(--spacing-md);">
            This page will show full box details including activities, logs, QR code, and attachments.
          </p>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['./box-details.component.scss']
})
export class BoxDetailsComponent {}
