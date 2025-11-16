import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-boxes-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  template: `
    <app-header></app-header>
    <app-sidebar></app-sidebar>
    <div class="page-container">
      <div class="page-content">
        <div class="page-header">
          <h1 class="page-title">Boxes - Project {{ projectId }}</h1>
          <button class="btn btn-primary" *ngIf="canCreate">
            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <line x1="12" y1="5" x2="12" y2="19"/><line x1="5" y1="12" x2="19" y2="12"/>
            </svg>
            Add Box
          </button>
        </div>
        <div class="card" style="padding: var(--spacing-2xl); text-align: center;">
          <p>Boxes List Page - Implementation Ready</p>
          <p style="color: var(--gray-600); font-size: var(--font-size-sm); margin-top: var(--spacing-md);">
            This page will display all boxes for the project with filtering, search, and status updates.
          </p>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['./boxes-list.component.scss']
})
export class BoxesListComponent implements OnInit {
  projectId: string = '';
  boxes: Box[] = [];
  loading = true;
  canCreate = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxService: BoxService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
  }
}
