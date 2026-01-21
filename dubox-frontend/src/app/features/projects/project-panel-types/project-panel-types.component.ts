import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PanelTypeService, CreatePanelTypeDto, UpdatePanelTypeDto } from '../../../core/services/panel-type.service';
import { PanelType } from '../../../core/models/box.model';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-project-panel-types',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './project-panel-types.component.html',
  styleUrls: ['./project-panel-types.component.scss']
})
export class ProjectPanelTypesComponent implements OnInit {
  projectId: string = '';
  project: Project | null = null;
  panelTypes: PanelType[] = [];
  allPanelTypes: PanelType[] = []; // Store all panel types from backend
  loading = true;
  error = '';
  modalError = ''; // Error message displayed inside the modal
  
  showModal = false;
  editingPanelType: PanelType | null = null;
  panelTypeForm: FormGroup;
  
  includeInactive = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private panelTypeService: PanelTypeService,
    private projectService: ProjectService,
    private fb: FormBuilder
  ) {
    this.panelTypeForm = this.fb.group({
      panelTypeName: ['', Validators.required],
      panelTypeCode: ['', Validators.required],
      description: [''],
      displayOrder: [0, Validators.min(0)],
      isActive: [true]
    });
  }

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'] || this.route.snapshot.queryParams['projectId'];
    this.loadProject();
    this.loadPanelTypes();
  }

  loadProject(): void {
    if (!this.projectId) {
      return;
    }
    
    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.project = project;
      },
      error: (err) => {
        console.error('Error loading project:', err);
        // Don't show error to user, just log it
      }
    });
  }

  loadPanelTypes(): void {
    this.loading = true;
    this.error = '';
    
    // Always fetch all panel types (both active and inactive) to allow filtering
    this.panelTypeService.getPanelTypesByProject(this.projectId, true).subscribe({
      next: (response) => {
        this.allPanelTypes = response.data || [];
        this.applyFilter();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load panel types';
        console.error('Error loading panel types:', err);
        this.loading = false;
      }
    });
  }

  /**
   * Apply filter based on includeInactive flag
   * When includeInactive is false: show only active
   * When includeInactive is true: show only inactive
   */
  private applyFilter(): void {
    if (this.includeInactive) {
      // Show only inactive panel types
      this.panelTypes = this.allPanelTypes.filter(pt => !pt.isActive);
    } else {
      // Show only active panel types
      this.panelTypes = this.allPanelTypes.filter(pt => pt.isActive);
    }
  }

  openCreateModal(): void {
    this.error = ''; // Clear any previous errors
    this.editingPanelType = null;
    this.panelTypeForm.reset({
      panelTypeName: '',
      panelTypeCode: '',
      description: '',
      displayOrder: this.panelTypes.length,
      isActive: true
    });
    this.showModal = true;
  }

  openEditModal(panelType: PanelType): void {
    this.error = ''; // Clear any previous errors
    this.editingPanelType = panelType;
    this.panelTypeForm.patchValue({
      panelTypeName: panelType.panelTypeName,
      panelTypeCode: panelType.panelTypeCode,
      description: panelType.description,
      displayOrder: panelType.displayOrder,
      isActive: panelType.isActive
    });
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.editingPanelType = null;
    this.panelTypeForm.reset();
    this.error = ''; // Clear error when closing modal
    this.modalError = ''; // Clear modal error
  }

  savePanelType(): void {
    if (this.panelTypeForm.invalid) {
      return;
    }

    const formValue = this.panelTypeForm.value;

    if (this.editingPanelType) {
      // Update
      const dto: UpdatePanelTypeDto = {
        panelTypeId: this.editingPanelType.panelTypeId,
        panelTypeName: formValue.panelTypeName,
        panelTypeCode: formValue.panelTypeCode,
        description: formValue.description,
        isActive: formValue.isActive,
        displayOrder: formValue.displayOrder
      };

      this.panelTypeService.updatePanelType(this.projectId, this.editingPanelType.panelTypeId, dto).subscribe({
        next: () => {
          this.closeModal();
          this.loadPanelTypes(); // Reload to refresh the list
        },
        error: (err) => {
          // Extract error message from different possible locations
          const errorMessage = err.error?.message || err.error?.error || err.message || 'Failed to update panel type';
          this.error = errorMessage;
          console.error('Error updating panel type:', err);
        }
      });
    } else {
      // Create
      const dto: CreatePanelTypeDto = {
        projectId: this.projectId,
        panelTypeName: formValue.panelTypeName,
        panelTypeCode: formValue.panelTypeCode,
        description: formValue.description,
        displayOrder: formValue.displayOrder
      };

      this.panelTypeService.createPanelType(this.projectId, dto).subscribe({
        next: () => {
          this.closeModal();
          this.loadPanelTypes(); // Reload to refresh the list
        },
        error: (err) => {
          // Extract error message from different possible locations
          const errorMessage = err.error?.message || err.error?.error || err.message || 'Failed to create panel type';
          this.modalError = errorMessage; // Show error in modal
          console.error('Error creating panel type:', err);
        }
      });
    }
  }

  deletePanelType(panelType: PanelType): void {
    if (!confirm(`Are you sure you want to delete "${panelType.panelTypeName}"? This cannot be undone if panels are using this type.`)) {
      return;
    }

    this.panelTypeService.deletePanelType(this.projectId, panelType.panelTypeId).subscribe({
      next: () => {
        this.loadPanelTypes();
      },
      error: (err) => {
        // Extract error message from different possible locations
        const errorMessage = err.error?.message || err.error?.error || err.message || 'Failed to delete panel type. It may be in use by panels.';
        this.error = errorMessage;
        console.error('Error deleting panel type:', err);
      }
    });
  }

  toggleActiveFilter(): void {
    this.includeInactive = !this.includeInactive;
    this.applyFilter();
  }

  goBack(): void {
    this.router.navigate(['/projects', this.projectId, 'dashboard']);
  }
}

