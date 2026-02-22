import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ActivityTemplateService } from '../../../core/services/activity-template.service';
import { ActivityTemplate } from '../../../core/models/activity-template.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-activity-templates-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './activity-templates-list.component.html',
  styleUrl: './activity-templates-list.component.scss'
})
export class ActivityTemplatesListComponent implements OnInit {
  templates: ActivityTemplate[] = [];
  filteredTemplates: ActivityTemplate[] = [];
  loading = false;
  error: string | null = null;

  // Filters
  searchTerm = '';
  showActiveOnly = true;
  selectedTemplateId = '';

  constructor(
    private templateService: ActivityTemplateService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadTemplates();
  }

  loadTemplates(): void {
    this.loading = true;
    this.error = null;

    this.templateService.getAllTemplates(this.showActiveOnly, this.searchTerm)
      .subscribe({
        next: (templates) => {
          this.templates = templates || [];
          this.filteredTemplates = this.templates;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load templates';
          this.loading = false;
          console.error('Error loading templates:', err);
        }
      });
  }

  applyFilters(): void {
    if (this.selectedTemplateId) {
      this.filteredTemplates = this.templates.filter(t => 
        t.activityTemplateId === this.selectedTemplateId
      );
    } else {
      this.filteredTemplates = this.templates;
    }
  }

  onTemplateSelect(): void {
    this.applyFilters();
  }

  clearFilters(): void {
    this.selectedTemplateId = '';
    this.showActiveOnly = true;
    this.loadTemplates();
  }

  navigateToCreate(): void {
    this.router.navigate(['/schedule/activity-templates/create']);
  }

  navigateToEdit(templateId: string): void {
    this.router.navigate(['/schedule/activity-templates', templateId, 'edit']);
  }

  viewTemplate(templateId: string): void {
    this.router.navigate(['/schedule/activity-templates', templateId]);
  }

  getStatusClass(isActive: boolean): string {
    return isActive ? 'status-active' : 'status-inactive';
  }

  getStatusText(isActive: boolean): string {
    return isActive ? 'Active' : 'Inactive';
  }
}
