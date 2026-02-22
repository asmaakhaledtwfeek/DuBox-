import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MaterialTemplateService } from '../../../core/services/material-template.service';
import { MaterialTemplate } from '../../../core/models/material-template.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-material-templates-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './material-templates-list.component.html',
  styleUrl: './material-templates-list.component.scss'
})
export class MaterialTemplatesListComponent implements OnInit {
  templates: MaterialTemplate[] = [];
  filteredTemplates: MaterialTemplate[] = [];
  loading = false;
  error: string | null = null;

  // Filters
  searchTerm = '';
  selectedCategory = '';
  showActiveOnly = true;

  // Categories for filter dropdown
  categories: string[] = [];

  // Delete confirmation
  showDeleteModal = false;
  templateToDelete: MaterialTemplate | null = null;
  deleting = false;
  deleteError = '';

  constructor(
    private templateService: MaterialTemplateService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadAllCategories();
    this.loadTemplates();
  }

  loadAllCategories(): void {
    // Load all templates to extract all available categories (without filters)
    this.templateService.getAllTemplates(false, '', '')
      .subscribe({
        next: (templates) => {
          this.extractCategories(templates || []);
        },
        error: (err) => {
          console.error('Error loading categories:', err);
        }
      });
  }

  loadTemplates(): void {
    this.loading = true;
    this.error = null;

    this.templateService.getAllTemplates(this.showActiveOnly, this.selectedCategory, this.searchTerm)
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

  extractCategories(templates: MaterialTemplate[]): void {
    const categorySet = new Set<string>();
    templates.forEach(t => {
      if (t.category) {
        categorySet.add(t.category);
      }
    });
    this.categories = Array.from(categorySet).sort();
  }

  applyFilters(): void {
    this.loadTemplates();
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedCategory = '';
    this.showActiveOnly = true;
    this.loadTemplates();
  }

  navigateToCreate(): void {
    this.router.navigate(['/schedule/material-templates/create']);
  }

  navigateToEdit(templateId: string): void {
    this.router.navigate(['/schedule/material-templates', templateId, 'edit']);
  }

  viewTemplate(templateId: string): void {
    this.router.navigate(['/schedule/material-templates', templateId]);
  }

  openDeleteModal(template: MaterialTemplate): void {
    this.templateToDelete = template;
    this.showDeleteModal = true;
    this.deleteError = '';
  }

  closeDeleteModal(): void {
    this.showDeleteModal = false;
    this.templateToDelete = null;
    this.deleteError = '';
  }

  confirmDelete(): void {
    if (!this.templateToDelete) return;

    this.deleting = true;
    this.deleteError = '';

    this.templateService.deleteTemplate(this.templateToDelete.materialTemplateId)
      .subscribe({
        next: () => {
          this.deleting = false;
          this.closeDeleteModal();
          this.loadTemplates(); // Reload list
        },
        error: (err) => {
          this.deleting = false;
          this.deleteError = 'Failed to delete template';
          console.error('Error deleting template:', err);
        }
      });
  }

  getStatusClass(isActive: boolean): string {
    return isActive ? 'status-active' : 'status-inactive';
  }

  getStatusText(isActive: boolean): string {
    return isActive ? 'Active' : 'Inactive';
  }
}

