import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ProjectService } from '../../../core/services/project.service';
import { BoxService } from '../../../core/services/box.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { Project } from '../../../core/models/project.model';
import { ProjectTypeCategory } from '../../../core/models/box.model';

@Component({
  selector: 'app-create-project',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './create-project.component.html',
  styleUrl: './create-project.component.scss'
})
export class CreateProjectComponent implements OnInit {
  projectForm!: FormGroup;
  loading = false;
  error = '';
  successMessage = '';
  initializing = false;
  isEdit = false;
  projectId: string | null = null;
  originalProject: Project | null = null;
  canEditPlannedStartDate = true;
  categories: ProjectTypeCategory[] = [];
  loadingCategories = false;
  
  locations = [
    { value: 1, label: 'KSA' },
    { value: 2, label: 'UAE' }
  ];

  constructor(
    private fb: FormBuilder,
    @Inject(ProjectService) private projectService: ProjectService,
    private boxService: BoxService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadCategories();
    this.detectModeAndLoadProject();
  }

  private initForm(): void {
    this.projectForm = this.fb.group({
      projectName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      projectCode: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      clientName: ['', Validators.maxLength(200)],
      location: [null, Validators.required],
      projectCategoryId: [null, Validators.required],
      duration: [null, [Validators.required, Validators.min(1)]],
      plannedStartDate: ['', Validators.required],
      description: ['', Validators.maxLength(500)]
    });
  }

  private loadCategories(): void {
    this.loadingCategories = true;
    this.boxService.getAllProjectTypeCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
        this.loadingCategories = false;
      },
      error: (err: any) => {
        console.error('Error loading categories:', err);
        this.loadingCategories = false;
      }
    });
  }

  private detectModeAndLoadProject(): void {
    const modeQuery = this.route.snapshot.queryParamMap.get('mode');
    const projectIdQuery = this.route.snapshot.queryParamMap.get('projectId');
    const projectIdParam = this.route.snapshot.params['projectId'];

    this.projectId = projectIdParam || projectIdQuery;
    this.isEdit = modeQuery === 'edit' || !!projectIdParam;

    if (this.isEdit) {
      if (!this.projectId) {
        this.error = 'Invalid project selection. Please go back and choose a project again.';
        return;
      }

      this.initializing = true;
      this.projectForm.disable();
      this.loadProject(this.projectId);
    }
  }

  private loadProject(id: string): void {
    this.projectService.getProject(id).subscribe({
      next: (project) => {
        this.originalProject = project;
        this.patchForm(project);
        this.initializing = false;
        this.projectForm.enable();

        this.canEditPlannedStartDate = !project.startDate;
        if (!this.canEditPlannedStartDate) {
          this.projectForm.get('plannedStartDate')?.disable({ emitEvent: false });
        }
      },
      error: (err) => {
        this.error = err.message || 'Failed to load project details. Please try again.';
        this.initializing = false;
        this.projectForm.enable();
        console.error('❌ Error loading project for edit:', err);
      }
    });
  }

  private patchForm(project: Project): void {
    const plannedStart = project.plannedStartDate || project.startDate;
    const duration = this.getDurationValue(project);

    this.projectForm.patchValue({
      projectName: project.name || '',
      projectCode: project.code || '',
      clientName: project.clientName || '',
      location: project.location || null,
      projectCategoryId: project.categoryId || null,
      duration: duration,
      plannedStartDate: this.formatDateForInput(plannedStart),
      description: project.description || ''
    });
  }

  private getDurationValue(project: Project): number | null {
    if (project.duration) {
      return project.duration;
    }

    if (project.startDate && project.endDate) {
      const diff = project.endDate.getTime() - project.startDate.getTime();
      const days = Math.ceil(diff / (1000 * 60 * 60 * 24));
      return days > 0 ? days : null;
    }

    return null;
  }

  private formatDateForInput(date?: Date): string {
    if (!date) return '';
    const iso = date.toISOString();
    return iso.split('T')[0];
  }

  onSubmit(): void {
    if (this.initializing || this.projectForm.invalid) {
      this.markFormGroupTouched(this.projectForm);
      return;
    }

    this.loading = true;
    this.error = '';
    this.successMessage = '';

    const formValue = this.projectForm.value;
    
    let projectData: any;

    if (!this.isEdit) {
      projectData = {
        projectCode: formValue.projectCode,
        projectName: formValue.projectName,
        clientName: formValue.clientName || undefined,
        location: formValue.location || 1,
        projectCategoryId: formValue.projectCategoryId,
        duration: formValue.duration,
        plannedStartDate: formValue.plannedStartDate ? new Date(formValue.plannedStartDate).toISOString() : undefined,
        description: formValue.description || undefined
      };
    } else {
      projectData = {};
      const compare = (newVal: any, oldVal: any) => newVal !== oldVal && !(newVal === undefined && oldVal === undefined);

      if (this.originalProject) {
        if (compare(formValue.projectCode, this.originalProject.code)) {
          projectData.projectCode = formValue.projectCode;
        }
        if (compare(formValue.projectName, this.originalProject.name)) {
          projectData.projectName = formValue.projectName;
        }
        if (compare(formValue.clientName, this.originalProject.clientName)) {
          projectData.clientName = formValue.clientName || undefined;
        }
        if (compare(formValue.location, this.originalProject.location)) {
          projectData.location = formValue.location || 1;
        }
        if (compare(formValue.projectCategoryId, this.originalProject.categoryId)) {
          projectData.categoryId = formValue.projectCategoryId;
        }
        if (compare(formValue.description, this.originalProject.description)) {
          projectData.description = formValue.description || undefined;
        }
        if (compare(formValue.duration, this.getDurationValue(this.originalProject))) {
          projectData.duration = formValue.duration;
        }
        if (this.canEditPlannedStartDate && formValue.plannedStartDate) {
          const originalPlanned = this.originalProject.plannedStartDate ? this.formatDateForInput(this.originalProject.plannedStartDate) : '';
          if (compare(this.formatDateForInput(formValue.plannedStartDate ? new Date(formValue.plannedStartDate) : undefined), originalPlanned)) {
            projectData.plannedStartDate = new Date(formValue.plannedStartDate).toISOString();
          }
        }
      }

      if (this.projectId) {
        projectData.projectId = this.projectId;
      }
    }

    console.log('🚀 Submitting project data:', projectData);

    const request$ = this.isEdit && this.projectId
      ? this.projectService.updateProject(this.projectId, projectData)
      : this.projectService.createProject(projectData);

    request$.subscribe({
      next: (project: any) => {
        this.loading = false;
        this.successMessage = this.isEdit ? 'Project updated successfully!' : 'Project created successfully!';
        console.log('✅ Project saved:', project);

        const projectId = project.id || project.projectId || project.ProjectId || this.projectId;

        if (!projectId) {
          console.error('⚠️ WARNING: Saved project has no ID!');
          console.error('📦 Full project object:', JSON.stringify(project, null, 2));
          alert('Warning: Project saved but has no ID. Please check console.');
        }

        setTimeout(() => {
          if (projectId) {
            this.router.navigate(['/projects', projectId, 'dashboard']);
          } else {
            this.router.navigate(['/projects']);
          }
        }, 1200);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || err.message || `Failed to ${this.isEdit ? 'update' : 'create'} project. Please try again.`;
        console.error('❌ Error saving project:', err);
      }
    });
  }

  onCancel(): void {
    if (this.isEdit && this.projectId) {
      this.router.navigate(['/projects', this.projectId, 'dashboard']);
    } else {
      this.router.navigate(['/projects']);
    }
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.projectForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.projectForm.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) return `${this.getFieldLabel(fieldName)} is required`;
      if (field.errors['minlength']) return `Minimum length is ${field.errors['minlength'].requiredLength} characters`;
      if (field.errors['maxlength']) return `Maximum length is ${field.errors['maxlength'].requiredLength} characters`;
    }
    return '';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: Record<string, string> = {
      projectName: 'Project name',
      projectCode: 'Project code',
      clientName: 'Client name',
      location: 'Location',
      projectCategoryId: 'Project category',
      duration: 'Duration',
      plannedStartDate: 'Planned start date',
      description: 'Description'
    };
    return labels[fieldName] || fieldName;
  }
}

