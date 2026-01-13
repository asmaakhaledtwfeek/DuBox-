import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-phase-readiness-report',
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
          <h1 class="page-title">Phase Readiness Report</h1>
          <p class="page-subtitle">Check readiness of phases and activities to launch successors</p>
        </div>
        <div class="coming-soon">
          <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
            <rect x="3" y="3" width="18" height="18" rx="2" ry="2"/>
            <line x1="3" y1="9" x2="21" y2="9"/>
            <line x1="9" y1="21" x2="9" y2="9"/>
          </svg>
          <h2>Coming Soon</h2>
          <p>Phase readiness tracking and reporting will be available soon.</p>
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
export class PhaseReadinessReportComponent {}
