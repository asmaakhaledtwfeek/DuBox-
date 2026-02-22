import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialTemplateService } from '../../../core/services/material-template.service';
import { ProjectService } from '../../../core/services/project.service';
import { MaterialTemplate, ProjectMaterialTemplate, BoxTypeMaterialTemplate } from '../../../core/models/material-template.model';

export type AssignmentLevel = 'project' | 'boxType';

export interface TemplateAssignment {
  level: AssignmentLevel;
  projectId: string;
  boxTypeId?: number;
  boxTypeName?: string;
  projectTemplateIds?: string[]; // For filtering box type templates to only project-assigned templates
}

@Component({
  selector: 'app-assign-template-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './assign-template-modal.component.html',
  styleUrls: ['./assign-template-modal.component.scss']
})
export class AssignTemplateModalComponent implements OnInit, OnChanges {
  @Input() isOpen: boolean = false;
  @Input() assignment: TemplateAssignment | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() assigned = new EventEmitter<{ templateId: string }>();

  availableTemplates: MaterialTemplate[] = [];
  filteredTemplates: MaterialTemplate[] = [];
  currentAssignments: (ProjectMaterialTemplate | BoxTypeMaterialTemplate)[] = [];
  selectedTemplateId: string | null = null;
  
  loadingTemplates = false;
  loadingAssignments = false;
  submitting = false;
  error: string | null = null;

  constructor(
    private materialTemplateService: MaterialTemplateService,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    if (this.isOpen && this.assignment) {
      this.initializeModal();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen'] && changes['isOpen'].currentValue && this.assignment) {
      this.initializeModal();
    } else if (changes['isOpen'] && !changes['isOpen'].currentValue) {
      this.resetModal();
    }
  }

  private initializeModal(): void {
    if (!this.assignment) return;

    this.error = null;
    this.selectedTemplateId = null;
    this.loadAvailableTemplates();
    this.loadCurrentAssignments();
  }

  private resetModal(): void {
    this.selectedTemplateId = null;
    this.availableTemplates = [];
    this.currentAssignments = [];
    this.error = null;
  }

  loadAvailableTemplates(): void {
    this.loadingTemplates = true;
    this.error = null;

    this.materialTemplateService.getAllTemplates().subscribe({
      next: (templates: MaterialTemplate[]) => {
        this.availableTemplates = templates.filter((t: MaterialTemplate) => t.isActive);
        this.applyTemplateFilters();
        this.loadingTemplates = false;
      },
      error: (error: any) => {
        console.error('Error loading templates:', error);
        this.error = 'Failed to load available templates';
        this.loadingTemplates = false;
      }
    });
  }

  private applyTemplateFilters(): void {
    let filtered = [...this.availableTemplates];

    // Show all active templates for both project and box type assignments
    // Users can assign any template to a box type, not just project-assigned ones
    // This allows more flexibility in template management
    
    this.filteredTemplates = filtered;
  }

  loadCurrentAssignments(): void {
    if (!this.assignment) return;

    this.loadingAssignments = true;

    if (this.assignment.level === 'project') {
      this.materialTemplateService.getProjectTemplates(this.assignment.projectId).subscribe({
        next: (assignments: ProjectMaterialTemplate[]) => {
          this.currentAssignments = assignments;
          this.loadingAssignments = false;
        },
        error: (error: any) => {
          console.error('Error loading project assignments:', error);
          this.loadingAssignments = false;
        }
      });
    } else if (this.assignment.level === 'boxType' && this.assignment.boxTypeId) {
      // Get all box type templates for the project, then filter to this specific box type
      this.materialTemplateService.getBoxTypeTemplates(this.assignment.projectId).subscribe({
        next: (assignments: BoxTypeMaterialTemplate[]) => {
          // Filter to only the assignment for this specific box type
          this.currentAssignments = assignments.filter(
            (a: BoxTypeMaterialTemplate) => a.projectBoxTypeId === this.assignment!.boxTypeId
          );
          this.loadingAssignments = false;
        },
        error: (error: any) => {
          console.error('Error loading box type assignments:', error);
          this.loadingAssignments = false;
        }
      });
    }
  }

  get hasCurrentAssignment(): boolean {
    return this.currentAssignments.length > 0;
  }

  get currentTemplate(): MaterialTemplate | null {
    if (this.currentAssignments.length === 0) return null;
    return this.currentAssignments[0].template;
  }

  onAssignTemplate(): void {
    if (!this.selectedTemplateId || !this.assignment) {
      this.error = 'Please select a template';
      return;
    }

    this.submitting = true;
    this.error = null;

    if (this.assignment.level === 'project') {
      // Route the project-level change through UpdateProject so that
      // UpdateProjectCommandHandler handles propagation to box types internally.
      this.projectService.updateProject(this.assignment.projectId, {
        projectId: this.assignment.projectId,
        materialTemplateId: this.selectedTemplateId
      } as any).subscribe({
        next: () => {
          this.submitting = false;
          this.assigned.emit({ templateId: this.selectedTemplateId! });
          this.onClose();
        },
        error: (error: any) => {
          console.error('Error assigning template:', error);
          this.error = 'Failed to assign template to project';
          this.submitting = false;
        }
      });
    } else if (this.assignment.level === 'boxType' && this.assignment.boxTypeId) {
      this.materialTemplateService.assignToBoxType(
        this.selectedTemplateId,
        { projectBoxTypeId: this.assignment.boxTypeId }
      ).subscribe({
        next: () => {
          this.submitting = false;
          this.assigned.emit({ templateId: this.selectedTemplateId! });
          this.onClose();
        },
        error: (error: any) => {
          console.error('Error assigning template:', error);
          this.error = 'Failed to assign template to box type';
          this.submitting = false;
        }
      });
    }
  }

  onRemoveAssignment(): void {
    if (!this.hasCurrentAssignment || !this.assignment) return;

    this.submitting = true;
    this.error = null;

    const currentAssignment = this.currentAssignments[0];

    if (this.assignment.level === 'project') {
      const projectAssignment = currentAssignment as ProjectMaterialTemplate;
      this.materialTemplateService.removeFromProject(
        projectAssignment.projectId,
        projectAssignment.materialTemplateId
      ).subscribe({
        next: () => {
          this.submitting = false;
          this.assigned.emit({ templateId: '' });
          this.onClose();
        },
        error: (error: any) => {
          console.error('Error removing template:', error);
          this.error = 'Failed to remove template from project';
          this.submitting = false;
        }
      });
    } else if (this.assignment.level === 'boxType') {
      const boxTypeAssignment = currentAssignment as BoxTypeMaterialTemplate;
      this.materialTemplateService.removeFromBoxType(
        boxTypeAssignment.projectBoxTypeId,
        boxTypeAssignment.materialTemplateId
      ).subscribe({
        next: () => {
          this.submitting = false;
          this.assigned.emit({ templateId: '' });
          this.onClose();
        },
        error: (error: any) => {
          console.error('Error removing template:', error);
          this.error = 'Failed to remove template from box type';
          this.submitting = false;
        }
      });
    }
  }

  onClose(): void {
    this.close.emit();
  }

  get modalTitle(): string {
    if (!this.assignment) return 'Assign Template';
    
    if (this.assignment.level === 'project') {
      return 'Assign Material Template to Project';
    } else {
      return `Assign Material Template to ${this.assignment.boxTypeName || 'Box Type'}`;
    }
  }
}

