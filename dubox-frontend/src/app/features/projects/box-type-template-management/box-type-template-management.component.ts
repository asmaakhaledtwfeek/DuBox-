import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { AssignTemplateModalComponent, TemplateAssignment } from '../../../shared/components/assign-template-modal/assign-template-modal.component';

import { ProjectService } from '../../../core/services/project.service';
import { MaterialTemplateService } from '../../../core/services/material-template.service';
import { ToastService } from '../../../core/services/toast.service';

import { Project } from '../../../core/models/project.model';
import { ProjectBoxType, ProjectConfiguration } from '../../../core/models/project-configuration.model';
import { MaterialTemplate, BoxTypeMaterialTemplate, ProjectMaterialTemplate } from '../../../core/models/material-template.model';
import { ProjectMaterialDto } from '../../../core/models/project-material.model';

interface BoxTypeWithTemplate extends ProjectBoxType {
  assignedTemplate?: MaterialTemplate;
  assignmentDate?: Date;
  assignedBy?: string;
}

@Component({
  selector: 'app-box-type-template-management',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    HeaderComponent,
    SidebarComponent,
    AssignTemplateModalComponent
  ],
  templateUrl: './box-type-template-management.component.html',
  styleUrls: ['./box-type-template-management.component.scss']
})
export class BoxTypeTemplateManagementComponent implements OnInit {
  projectId!: string;
  project: Project | null = null;
  
  boxTypes: BoxTypeWithTemplate[] = [];
  filteredBoxTypes: BoxTypeWithTemplate[] = [];
  availableTemplates: MaterialTemplate[] = [];
  projectTemplates: ProjectMaterialTemplate[] = []; // Changed to array for multiple templates
  projectMaterials: ProjectMaterialDto[] = [];
  
  loading = false;
  loadingTemplates = false;
  error = '';
  
  searchControl = new FormControl('');
  selectedFilter: 'all' | 'assigned' | 'unassigned' = 'all';
  
  showTemplateModal = false;
  templateAssignment: TemplateAssignment | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private materialTemplateService: MaterialTemplateService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['projectId'];
    
    if (!this.projectId) {
      this.error = 'Project ID is required';
      return;
    }

    // Setup search
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(() => this.applyFilters());

    this.loadData();
  }

  async loadData(): Promise<void> {
    this.loading = true;
    this.error = '';

    try {
      // Load project details
      await this.loadProject();
      
      // Load project materials (to filter compatible templates)
      await this.loadProjectMaterials();
      
      // Load all templates
      await this.loadTemplates();
      
      // Load box types with assignments
      await this.loadBoxTypes();
      
      this.applyFilters();
    } catch (err: any) {
      console.error('Error loading data:', err);
      this.error = err.message || 'Failed to load data';
      this.toastService.error('Failed to load template assignments');
    } finally {
      this.loading = false;
    }
  }

  private loadProject(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.projectService.getProject(this.projectId).subscribe({
        next: (project: Project) => {
          this.project = project;
          resolve();
        },
        error: (err: any) => reject(err)
      });
    });
  }

  private loadProjectMaterials(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.projectService.getProjectMaterials(this.projectId, true).subscribe({
        next: (materials: ProjectMaterialDto[]) => {
          this.projectMaterials = materials;
          resolve();
        },
        error: (err: any) => {
          console.warn('Failed to load project materials:', err);
          resolve(); // Don't block if materials fail to load
        }
      });
    });
  }

  private loadTemplates(): Promise<void> {
    this.loadingTemplates = true;
    return new Promise((resolve, reject) => {
      this.materialTemplateService.getAllTemplates().subscribe({
        next: (templates: MaterialTemplate[]) => {
          this.availableTemplates = templates.filter((t: MaterialTemplate) => t.isActive);
          this.loadingTemplates = false;
          
          // Also load project-level template
          this.loadProjectTemplate();
          resolve();
        },
        error: (err: any) => {
          this.loadingTemplates = false;
          reject(err);
        }
      });
    });
  }

  private loadProjectTemplate(): void {
    this.materialTemplateService.getProjectTemplates(this.projectId).subscribe({
      next: (assignments: ProjectMaterialTemplate[]) => {
        this.projectTemplates = assignments; // Store all project templates
      },
      error: (err: any) => {
        console.warn('Failed to load project template:', err);
      }
    });
  }

  private loadBoxTypes(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.projectService.getProjectConfiguration(this.projectId).subscribe({
        next: (config: ProjectConfiguration) => {
          const boxTypes = config.boxTypes || [];
          
          // Load box type template assignments
          this.materialTemplateService.getBoxTypeTemplates(this.projectId).subscribe({
            next: (assignments: BoxTypeMaterialTemplate[]) => {
              // Map box types with their assignments
              this.boxTypes = boxTypes.map((boxType: ProjectBoxType) => {
                const assignment = assignments.find((a: BoxTypeMaterialTemplate) => a.projectBoxTypeId === boxType.id);
                
                if (assignment) {
                  const template = this.availableTemplates.find(
                    (t: MaterialTemplate) => t.materialTemplateId === assignment.materialTemplateId
                  );
                  
                  return {
                    ...boxType,
                    assignedTemplate: template,
                    assignmentDate: assignment.assignedDate,
                    assignedBy: assignment.assignedBy
                  };
                }
                
                return boxType;
              });
              
              resolve();
            },
            error: (err: any) => reject(err)
          });
        },
        error: (err: any) => reject(err)
      });
    });
  }

  applyFilters(): void {
    let filtered = [...this.boxTypes];

    // Apply search filter
    const searchTerm = this.searchControl.value?.toLowerCase() || '';
    if (searchTerm) {
      filtered = filtered.filter((bt: BoxTypeWithTemplate) =>
        bt.typeName.toLowerCase().includes(searchTerm) ||
        bt.abbreviation?.toLowerCase().includes(searchTerm) ||
        bt.assignedTemplate?.templateName.toLowerCase().includes(searchTerm)
      );
    }

    // Apply assignment filter
    if (this.selectedFilter === 'assigned') {
      filtered = filtered.filter((bt: BoxTypeWithTemplate) => bt.assignedTemplate);
    } else if (this.selectedFilter === 'unassigned') {
      filtered = filtered.filter((bt: BoxTypeWithTemplate) => !bt.assignedTemplate);
    }

    this.filteredBoxTypes = filtered;
  }

  onFilterChange(filter: 'all' | 'assigned' | 'unassigned'): void {
    this.selectedFilter = filter;
    this.applyFilters();
  }

  openAssignTemplateModal(boxType: BoxTypeWithTemplate): void {
    if (!boxType.id) {
      this.toastService.error('Invalid box type');
      return;
    }

    // Show all available templates for box type assignment
    this.templateAssignment = {
      level: 'boxType',
      projectId: this.projectId,
      boxTypeId: boxType.id,
      boxTypeName: boxType.typeName
    };
    this.showTemplateModal = true;
  }

  onTemplateAssigned(event: { templateId: string }): void {
    this.showTemplateModal = false;
    
    if (event.templateId) {
      this.toastService.success('Template assigned successfully');
    } else {
      this.toastService.success('Template removed successfully');
    }
    
    // Reload box types to show updated assignments
    this.loadBoxTypes().then(() => {
      this.applyFilters();
    });
  }

  closeTemplateModal(): void {
    this.showTemplateModal = false;
    this.templateAssignment = null;
  }

  getCompatibleTemplates(boxType: BoxTypeWithTemplate): MaterialTemplate[] {
    // If no project materials are selected, show all templates
    if (!this.projectMaterials || this.projectMaterials.length === 0) {
      return this.availableTemplates;
    }

    // Filter templates that only contain materials selected in the project
    const projectMaterialIds = new Set(this.projectMaterials.map((pm: ProjectMaterialDto) => pm.materialId));
    
    return this.availableTemplates.filter((template: MaterialTemplate) => {
      if (!template.items || template.items.length === 0) return false;
      
      // Check if all template materials are in project materials
      return template.items.every((item: any) => projectMaterialIds.has(item.materialId));
    });
  }

  isTemplateCompatible(template: MaterialTemplate): boolean {
    if (!this.projectMaterials || this.projectMaterials.length === 0) {
      return true;
    }

    const projectMaterialIds = new Set(this.projectMaterials.map((pm: ProjectMaterialDto) => pm.materialId));
    
    if (!template.items || template.items.length === 0) return false;
    
    return template.items.every((item: any) => projectMaterialIds.has(item.materialId));
  }

  get stats() {
    return {
      total: this.boxTypes.length,
      assigned: this.boxTypes.filter((bt: BoxTypeWithTemplate) => bt.assignedTemplate).length,
      unassigned: this.boxTypes.filter((bt: BoxTypeWithTemplate) => !bt.assignedTemplate).length,
      compatibleTemplates: this.getCompatibleTemplatesCount()
    };
  }

  private getCompatibleTemplatesCount(): number {
    if (!this.projectMaterials || this.projectMaterials.length === 0) {
      return this.availableTemplates.length;
    }

    const projectMaterialIds = new Set(this.projectMaterials.map((pm: ProjectMaterialDto) => pm.materialId));
    
    return this.availableTemplates.filter((template: MaterialTemplate) => {
      if (!template.items || template.items.length === 0) return false;
      return template.items.every((item: any) => projectMaterialIds.has(item.materialId));
    }).length;
  }

  navigateToProjectConfiguration(): void {
    this.router.navigate(['/projects', this.projectId, 'configuration']);
  }

  navigateBack(): void {
    this.router.navigate(['/projects', this.projectId]);
  }
}

