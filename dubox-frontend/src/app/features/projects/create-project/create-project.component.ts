import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ProjectService } from '../../../core/services/project.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

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

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.projectForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      code: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      location: ['', Validators.maxLength(200)],
      description: ['', Validators.maxLength(500)],
      startDate: ['', Validators.required],
      endDate: ['']
    });
  }

  onSubmit(): void {
    if (this.projectForm.invalid) {
      this.markFormGroupTouched(this.projectForm);
      return;
    }

    this.loading = true;
    this.error = '';
    this.successMessage = '';

    const formValue = this.projectForm.value;
    
    // Map frontend fields to backend expected format
    const projectData: any = {
      projectCode: formValue.code,
      projectName: formValue.name,
      location: formValue.location || undefined,
      description: formValue.description || undefined,
      startDate: formValue.startDate ? new Date(formValue.startDate).toISOString() : undefined,
      plannedEndDate: formValue.endDate ? new Date(formValue.endDate).toISOString() : undefined,
      clientName: undefined
    };

    console.log('🚀 Submitting project data:', projectData);

        this.projectService.createProject(projectData).subscribe({
          next: (project: any) => {
            this.loading = false;
            this.successMessage = 'Project created successfully!';
            console.log('✅ Project created:', project);
            console.log('🔑 Project keys:', Object.keys(project));
            console.log('🆔 Project ID:', project.id);
            console.log('🆔 Project projectId:', project.projectId);
            console.log('🆔 Project ProjectId:', project.ProjectId);
            
            const projectId = project.id || project.projectId || project.ProjectId;
            
            if (!projectId) {
              console.error('⚠️ WARNING: Created project has NO ID!');
              console.error('📦 Full project object:', JSON.stringify(project, null, 2));
              alert('Warning: Project created but has no ID. Please check console.');
            }
            
            setTimeout(() => {
              if (projectId) {
                this.router.navigate(['/projects', projectId, 'dashboard']);
              } else {
                this.router.navigate(['/projects']);
              }
            }, 1500);
          },
          error: (err) => {
            this.loading = false;
            this.error = err.error?.message || err.message || 'Failed to create project. Please try again.';
            console.error('❌ Error creating project:', err);
            console.error('📦 Full error:', err);
          }
        });
  }

  onCancel(): void {
    this.router.navigate(['/projects']);
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
      name: 'Project name',
      code: 'Project code',
      location: 'Location',
      description: 'Description',
      startDate: 'Start date',
      endDate: 'End date'
    };
    return labels[fieldName] || fieldName;
  }
}

