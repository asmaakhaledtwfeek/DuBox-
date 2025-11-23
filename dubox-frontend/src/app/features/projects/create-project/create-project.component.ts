import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ProjectService } from '../../../core/services/project.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { Project } from '../../../core/models/project.model';

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

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.detectModeAndLoadProject();
  }

  private initForm(): void {
    this.projectForm = this.fb.group({
      projectName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      projectCode: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      clientName: ['', Validators.maxLength(200)],
      location: ['', Validators.maxLength(200)],
      duration: [null, [Validators.required, Validators.min(1)]],
      plannedStartDate: ['', Validators.required],
      description: ['', Validators.maxLength(500)]
    });
  }

  private detectModeAndLoadProject(): void {
    const mode = this.route.snapshot.queryParamMap.get('mode');
    const projectId = this.route.snapshot.queryParamMap.get('projectId');

    this.isEdit = mode === 'edit';
    this.projectId = projectId;

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
        this.patchForm(project);
        this.initializing = false;
        this.projectForm.enable();
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
      location: project.location || '',
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
    
    // Map frontend fields to backend expected format
    const projectData: any = {
      projectCode: formValue.projectCode,
      projectName: formValue.projectName,
      clientName: formValue.clientName || undefined,
      location: formValue.location || undefined,
      duration: formValue.duration,
      plannedStartDate: formValue.plannedStartDate ? new Date(formValue.plannedStartDate).toISOString() : undefined,
      description: formValue.description || undefined
    };

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
      duration: 'Duration',
      plannedStartDate: 'Planned start date',
      description: 'Description'
    };
    return labels[fieldName] || fieldName;
  }
}

