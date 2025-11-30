import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-missing-materials-report',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  template: `
    <app-header></app-header>
    <app-sidebar></app-sidebar>
    <div class="page-container">
      <div class="page-content">
        <div class="page-header">
          <button class="btn-back" routerLink="/reports">
            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <line x1="19" y1="12" x2="5" y2="12"/>
              <polyline points="12 19 5 12 12 5"/>
            </svg>
            Back to Reports
          </button>
          <h1 class="page-title">Missing Materials Report</h1>
          <p class="page-subtitle">Identify missing materials related to any activity or phase</p>
        </div>
        <div class="coming-soon">
          <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
            <line x1="16.5" y1="9.4" x2="7.5" y2="4.21"/>
            <path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/>
            <polyline points="3.27 6.96 12 12.01 20.73 6.96"/>
            <line x1="12" y1="22.08" x2="12" y2="12"/>
          </svg>
          <h2>Coming Soon</h2>
          <p>Materials tracking and shortage reporting will be available soon.</p>
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
    .page-header {
      margin-bottom: var(--spacing-2xl);
    }
    .btn-back {
      display: inline-flex;
      align-items: center;
      gap: var(--spacing-xs);
      padding: var(--spacing-sm) var(--spacing-md);
      background: transparent;
      border: none;
      color: var(--gray-600);
      cursor: pointer;
      border-radius: var(--radius-md);
      margin-bottom: var(--spacing-md);
    }
    .btn-back:hover {
      background: var(--gray-100);
      color: var(--gray-900);
    }
    .page-title {
      font-size: var(--font-size-3xl);
      font-weight: 700;
      color: var(--gray-900);
      margin: 0 0 var(--spacing-xs) 0;
    }
    .page-subtitle {
      font-size: var(--font-size-base);
      color: var(--gray-600);
      margin: 0;
    }
    .coming-soon {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: var(--spacing-3xl);
      background: var(--white);
      border-radius: var(--radius-lg);
      box-shadow: var(--shadow-sm);
      text-align: center;
    }
    .coming-soon svg {
      color: var(--gray-400);
      margin-bottom: var(--spacing-xl);
    }
    .coming-soon h2 {
      font-size: var(--font-size-2xl);
      color: var(--gray-900);
      margin: 0 0 var(--spacing-sm) 0;
    }
    .coming-soon p {
      color: var(--gray-600);
      margin: 0;
    }
    @media (max-width: 768px) {
      .page-container {
        margin-left: 0;
      }
    }
  `]
})
export class MissingMaterialsReportComponent {}
