import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { MaterialTemplateService } from '../../../core/services/material-template.service';
import { MaterialTemplate } from '../../../core/models/material-template.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-view-material-template',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './view-material-template.component.html',
  styleUrl: './view-material-template.component.scss'
})
export class ViewMaterialTemplateComponent implements OnInit {
  template: MaterialTemplate | null = null;
  loading = false;
  error: string | null = null;
  templateId: string = '';

  constructor(
    private templateService: MaterialTemplateService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.templateId = this.route.snapshot.params['id'];
    if (this.templateId) {
      this.loadTemplate();
    }
  }

  loadTemplate(): void {
    this.loading = true;
    this.error = null;

    this.templateService.getTemplateById(this.templateId)
      .subscribe({
        next: (template) => {
          this.template = template;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load template details';
          this.loading = false;
          console.error('Error loading template:', err);
        }
      });
  }

  navigateToEdit(): void {
    this.router.navigate(['/schedule/material-templates', this.templateId, 'edit']);
  }

  navigateBack(): void {
    this.router.navigate(['/schedule/material-templates']);
  }

  getStatusClass(isActive: boolean): string {
    return isActive ? 'status-active' : 'status-inactive';
  }

  getStatusText(isActive: boolean): string {
    return isActive ? 'Active' : 'Inactive';
  }

  getRequiredBadgeClass(isRequired: boolean): string {
    return isRequired ? 'badge-required' : 'badge-optional';
  }

  getRequiredText(isRequired: boolean): string {
    return isRequired ? 'Required' : 'Optional';
  }
}
