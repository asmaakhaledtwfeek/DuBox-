import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormArray } from '@angular/forms';
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
  currentStep = 1;
  totalSteps = 2;

  // Asset modal state
  isAssetModalOpen = false;
  editingAssetIndex: number | null = null;
  assetForm!: FormGroup;

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
    this.initAssetForm();
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
      revitElementId: ['', Validators.maxLength(100)],
      boxPlannedStartDate: [''],
      boxDuration: [null, [Validators.min(1)]],
      assets: this.fb.array([])
    });
  }

  get assets(): FormArray {
    return this.boxForm.get('assets') as FormArray;
  }

  // Step navigation
  goToStep(step: number): void {
    if (step >= 1 && step <= this.totalSteps) {
      // Validate step 1 before moving to step 2
      if (step === 2 && this.currentStep === 1) {
        if (!this.validateStep1()) {
          return;
        }
      }
      this.currentStep = step;
    }
  }

  nextStep(): void {
    if (this.currentStep < this.totalSteps) {
      if (this.currentStep === 1 && !this.validateStep1()) {
        return;
      }
      this.currentStep++;
    }
  }

  previousStep(): void {
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  private validateStep1(): boolean {
    const step1Fields = ['boxTag', 'boxType', 'floor'];
    let isValid = true;

    step1Fields.forEach(field => {
      const control = this.boxForm.get(field);
      if (control && control.invalid) {
        control.markAsTouched();
        isValid = false;
      }
    });

    if (!isValid) {
      this.error = 'Please fill in all required fields in Step 1 before proceeding.';
      setTimeout(() => this.error = '', 5000);
    }

    return isValid;
  }

  // Asset modal methods
  initAssetForm(): void {
    this.assetForm = this.fb.group({
      assetType: ['', Validators.required],
      assetCode: [''],
      assetName: [''],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unit: [''],
      specifications: [''],
      notes: ['']
    });
  }

  openAssetModal(index?: number): void {
    this.editingAssetIndex = index !== undefined ? index : null;
    
    if (index !== undefined && this.assets.at(index)) {
      // Edit existing asset
      const asset = this.assets.at(index).value;
      this.assetForm.patchValue(asset);
    } else {
      // Add new asset
      this.assetForm.reset({
        assetType: '',
        assetCode: '',
        assetName: '',
        quantity: 1,
        unit: '',
        specifications: '',
        notes: ''
      });
    }
    
    this.isAssetModalOpen = true;
    document.body.style.overflow = 'hidden';
  }

  closeAssetModal(): void {
    this.isAssetModalOpen = false;
    this.editingAssetIndex = null;
    this.assetForm.reset({
      assetType: '',
      assetCode: '',
      assetName: '',
      quantity: 1,
      unit: '',
      specifications: '',
      notes: ''
    });
    document.body.style.overflow = '';
  }

  saveAsset(): void {
    if (this.assetForm.invalid) {
      this.markFormGroupTouched(this.assetForm);
      return;
    }

    const assetValue = this.assetForm.value;
    
    if (this.editingAssetIndex !== null) {
      // Update existing asset
      const assetGroup = this.assets.at(this.editingAssetIndex) as FormGroup;
      assetGroup.patchValue(assetValue);
    } else {
      // Add new asset
      this.assets.push(this.fb.group({
        assetType: [assetValue.assetType, Validators.required],
        assetCode: [assetValue.assetCode || ''],
        assetName: [assetValue.assetName || ''],
        quantity: [assetValue.quantity || 1, [Validators.required, Validators.min(1)]],
        unit: [assetValue.unit || ''],
        specifications: [assetValue.specifications || ''],
        notes: [assetValue.notes || '']
      }));
    }

    this.closeAssetModal();
  }

  removeAsset(index: number): void {
    this.assets.removeAt(index);
  }

  editAsset(index: number): void {
    this.openAssetModal(index);
  }


  onSubmit(): void {
    // Validate step 1 before submission
    if (!this.validateStep1()) {
      this.currentStep = 1;
      return;
    }

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
      boxPlannedStartDate: formValue.boxPlannedStartDate ? new Date(formValue.boxPlannedStartDate).toISOString() : undefined,
      boxDuration: formValue.boxDuration ? parseInt(formValue.boxDuration, 10) : undefined,
      assets: this.getAssetsPayload()
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
      revitElementId: 'Revit element ID',
      boxPlannedStartDate: 'Box planned start date',
      boxDuration: 'Box duration'
    };
    return labels[fieldName] || fieldName;
  }

  private getAssetsPayload() {
    if (!this.assets.length) {
      return undefined;
    }

    const assets = this.assets.controls
      .map(control => control.value)
      .filter(asset => asset.assetType?.trim());

    if (!assets.length) {
      return undefined;
    }

    return assets.map(asset => ({
      assetType: asset.assetType,
      assetCode: asset.assetCode || undefined,
      assetName: asset.assetName || undefined,
      quantity: asset.quantity ? Number(asset.quantity) : 1,
      unit: asset.unit || undefined,
      specifications: asset.specifications || undefined,
      notes: asset.notes || undefined
    }));
  }
}

