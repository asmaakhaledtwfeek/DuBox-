import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-create-box',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './create-box.component.html',
  styleUrl: './create-box.component.scss'
})
export class CreateBoxComponent implements OnInit {
  boxForm!: FormGroup;
  loading = false;
  error = '';
  successMessage = '';
  projectId!: string;

  boxTypes = [
    'Living Room',
    'Bedroom',
    'Bathroom',
    'Kitchen',
    'Office',
    'Storage',
    'Utility Room',
    'Other'
  ];

  floors = [
    'GF', 'FF', '1F', '2F', '3F', '4F', '5F',
    'Basement', 'Roof'
  ];

  constructor(
    private fb: FormBuilder,
    private boxService: BoxService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.queryParams['projectId'];
    if (!this.projectId) {
      this.error = 'Project ID is required';
    }
    this.initForm();
  }

  private initForm(): void {
    this.boxForm = this.fb.group({
      boxTag: ['', [Validators.required, Validators.maxLength(50)]],
      boxName: ['', Validators.maxLength(200)],
      boxType: ['Living Room', Validators.required],
      floor: ['GF', Validators.required],
      building: ['', Validators.maxLength(100)],
      zone: ['', Validators.maxLength(50)],
      length: ['', [Validators.min(0), Validators.max(99999)]],
      width: ['', [Validators.min(0), Validators.max(99999)]],
      height: ['', [Validators.min(0), Validators.max(99999)]],
      bimModelReference: ['', Validators.maxLength(100)],
      revitElementId: ['', Validators.maxLength(100)]
    });
  }

  onSubmit(): void {
    if (this.boxForm.invalid) {
      this.markFormGroupTouched(this.boxForm);
      return;
    }

    if (!this.projectId) {
      this.error = 'Project ID is missing';
      return;
    }

    this.loading = true;
    this.error = '';
    this.successMessage = '';

    const formValue = this.boxForm.value;

    // Map frontend fields to backend expected format
    const boxData: any = {
      projectId: this.projectId,
      boxTag: formValue.boxTag,
      boxName: formValue.boxName || undefined,
      boxType: formValue.boxType,
      floor: formValue.floor,
      building: formValue.building || undefined,
      zone: formValue.zone || undefined,
      length: formValue.length ? parseFloat(formValue.length) : undefined,
      width: formValue.width ? parseFloat(formValue.width) : undefined,
      height: formValue.height ? parseFloat(formValue.height) : undefined,
      bimModelReference: formValue.bimModelReference || undefined,
      revitElementId: formValue.revitElementId || undefined,
      assets: undefined
    };

    console.log('🚀 Submitting box data:', boxData);

    this.boxService.createBox(boxData).subscribe({
      next: (box: any) => {
        this.loading = false;
        this.successMessage = 'Box created successfully!';
        console.log('✅ Box created:', box);
        setTimeout(() => {
          this.router.navigate(['/projects', this.projectId, 'boxes']);
        }, 1500);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || err.message || 'Failed to create box. Please try again.';
        console.error('❌ Error creating box:', err);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes']);
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
    const field = this.boxForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.boxForm.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) return `${this.getFieldLabel(fieldName)} is required`;
      if (field.errors['maxlength']) return `Maximum length is ${field.errors['maxlength'].requiredLength} characters`;
      if (field.errors['min']) return `Minimum value is ${field.errors['min'].min}`;
      if (field.errors['max']) return `Maximum value is ${field.errors['max'].max}`;
    }
    return '';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: Record<string, string> = {
      boxTag: 'Box tag',
      boxName: 'Box name',
      boxType: 'Box type',
      floor: 'Floor',
      building: 'Building',
      zone: 'Zone',
      length: 'Length',
      width: 'Width',
      height: 'Height',
      bimModelReference: 'BIM model reference',
      revitElementId: 'Revit element ID'
    };
    return labels[fieldName] || fieldName;
  }
}

