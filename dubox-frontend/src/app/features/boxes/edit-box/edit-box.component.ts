import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';
import { Box, getBoxStatusNumber } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ProjectService } from '../../../core/services/project.service';

@Component({
  selector: 'app-edit-box',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './edit-box.component.html',
  styleUrl: './edit-box.component.scss'
})
export class EditBoxComponent implements OnInit {
  boxForm!: FormGroup;
  loading = true;
  saving = false;
  error = '';
  successMessage = '';
  projectId!: string;
  boxId!: string;
  box: Box | null = null;
  projectPlannedStartDate?: Date;
  projectPlannedEndDate?: Date;
  scheduleErrorMessage: string | null = null;

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
    'BF', 'RF'
  ];

  // Zones dropdown
  zones: { value: number; name: string; displayName: string }[] = [];
  loadingZones = false;

  constructor(
    private fb: FormBuilder,
    private boxService: BoxService,
    private router: Router,
    private route: ActivatedRoute,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['projectId'];
    this.boxId = this.route.snapshot.params['boxId'];
    
    if (!this.projectId || !this.boxId) {
      this.error = 'Project ID and Box ID are required';
      this.loading = false;
      return;
    }
    
    this.initForm();
    this.loadZones();
    this.loadBox();
    this.loadProjectSchedule();
  }

  private loadZones(): void {
    this.loadingZones = true;
    this.boxService.getBoxZones().subscribe({
      next: (response: any) => {
        const zonesData = response?.data || response;
        this.zones = zonesData || [];
        this.loadingZones = false;
      },
      error: (error) => {
        console.error('❌ Error loading zones:', error);
        this.loadingZones = false;
        // Fallback to default zones if API fails
        this.zones = [
          { value: 0, name: 'ZoneA', displayName: 'Zone A' },
          { value: 1, name: 'ZoneB', displayName: 'Zone B' },
          { value: 2, name: 'ZoneC', displayName: 'Zone C' },
          { value: 3, name: 'ZoneD', displayName: 'Zone D' },
          { value: 4, name: 'ZoneE', displayName: 'Zone E' },
          { value: 5, name: 'ZoneF', displayName: 'Zone F' },
          { value: 6, name: 'ZoneG', displayName: 'Zone G' },
          { value: 7, name: 'ZoneH', displayName: 'Zone H' },
          { value: 8, name: 'ZoneI', displayName: 'Zone I' },
          { value: 9, name: 'ZoneJ', displayName: 'Zone J' }
        ];
      }
    });
  }

  private initForm(): void {
    this.boxForm = this.fb.group({
      boxTag: ['', [Validators.required, Validators.maxLength(50)]],
      boxName: ['', Validators.maxLength(200)],
      boxType: ['Living Room', Validators.required],
      floor: ['GF', Validators.required],
      buildingNumber: ['', Validators.maxLength(100)],
      zone: ['', Validators.maxLength(50)],
      length: ['', [Validators.min(0), Validators.max(99999)]],
      width: ['', [Validators.min(0), Validators.max(99999)]],
      height: ['', [Validators.min(0), Validators.max(99999)]],
      bimModelReference: ['', Validators.maxLength(100)],
      revitElementId: ['', Validators.maxLength(100)],
      plannedStartDate: [''],
      duration: [null, [Validators.min(1)]],
      notes: ['', Validators.maxLength(1000)]
    });

    this.boxForm.get('plannedStartDate')?.valueChanges.subscribe(() => this.validateSchedule());
    this.boxForm.get('duration')?.valueChanges.subscribe(() => this.validateSchedule());
  }

  private loadBox(): void {
    this.loading = true;
    this.error = '';
    
    this.boxService.getBox(this.boxId).subscribe({
      next: (box) => {
        this.box = box;
        this.populateForm(box);
        this.loading = false;
        this.validateSchedule();
      },
      error: (err) => {
        this.error = err.message || 'Failed to load box details';
        this.loading = false;
        console.error('Error loading box:', err);
      }
    });
  }

  private loadProjectSchedule(): void {
    if (!this.projectId) {
      return;
    }

    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.projectPlannedStartDate = project.plannedStartDate;
        this.projectPlannedEndDate = project.plannedEndDate;
        this.validateSchedule();
      },
      error: (err) => {
        console.error('Error loading project schedule', err);
      }
    });
  }

  private populateForm(box: Box): void {
    console.log('📝 Populating form with box data:', {
      floor: box.floor,
      buildingNumber: box.buildingNumber,
      zone: box.zone,
      length: box.length,
      width: box.width,
      height: box.height,
      bimModelReference: box.bimModelReference,
      revitElementId: box.revitElementId
    });
    
    this.boxForm.patchValue({
      boxTag: box.code,
      boxName: box.name || '',
      boxType: box.type || 'Living Room',
      floor: box.floor || 'GF',
      buildingNumber: box.buildingNumber || '',
      boxLetter: box.boxLetter || '',
      zone: box.zone || '',
      length: box.length || '',
      width: box.width || '',
      height: box.height || '',
      bimModelReference: box.bimModelReference || '',
      revitElementId: box.revitElementId || '',
      plannedStartDate: box.plannedStartDate ? box.plannedStartDate.toISOString().split('T')[0] : '',
      duration: box.duration ?? '',
      notes: box.notes || ''
    });
    
    console.log('✅ Form populated, current values:', this.boxForm.value);
  }

  onSubmit(): void {
    this.validateSchedule();
    if (this.boxForm.invalid) {
      this.markFormGroupTouched(this.boxForm);
      return;
    }

    if (!this.box) {
      this.error = 'Box data not loaded';
      return;
    }

    this.saving = true;
    this.error = '';
    this.successMessage = '';

    const formValue = this.boxForm.value;
    
    // Convert status string to number
    const statusNumber = getBoxStatusNumber(this.box.status);
    console.log(`🔄 Status conversion: "${this.box.status}" → ${statusNumber}`);

    // Map frontend fields to backend expected format (UpdateBoxDto)
    const plannedStartDate = formValue.plannedStartDate
      ? new Date(formValue.plannedStartDate)
      : null;
    const duration = formValue.duration !== null && formValue.duration !== ''
      ? Number(formValue.duration)
      : null;
    const notes = formValue.notes ? formValue.notes.trim() : '';

    const boxData: any = {
      boxId: this.boxId,  // REQUIRED (Guid)
      boxTag: formValue.boxTag || null,
      boxName: formValue.boxName || null,
      boxType: formValue.boxType || null,
      floor: formValue.floor || null,
      buildingNumber: formValue.buildingNumber || null,
      boxLetter: formValue.boxLetter || null,
      zone: formValue.zone || null,
      status: statusNumber,  // REQUIRED (int) - converted from string
      length: formValue.length ? parseFloat(formValue.length) : null,
      width: formValue.width ? parseFloat(formValue.width) : null,
      height: formValue.height ? parseFloat(formValue.height) : null,
      plannedStartDate: plannedStartDate ? plannedStartDate.toISOString() : null,
      duration,
      notes: notes || null
    };

    console.log('🚀 Updating box with payload:', boxData);

    this.boxService.updateBox(this.boxId, boxData).subscribe({
      next: (box: any) => {
        this.saving = false;
        this.successMessage = 'Box updated successfully!';
        console.log('✅ Box updated:', box);
        setTimeout(() => {
          this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId]);
        }, 1500);
      },
      error: (err) => {
        this.saving = false;
        
        // Extract validation errors if available
        let errorMessage = 'Failed to update box. Please try again.';
        if (err.error?.errors) {
          const errors = Object.values(err.error.errors).flat();
          errorMessage = errors.join(', ');
        } else if (err.error?.message) {
          errorMessage = err.error.message;
        } else if (err.message) {
          errorMessage = err.message;
        }
        
        this.error = errorMessage;
        console.error('❌ Error updating box:', err);
        console.error('❌ Error details:', err.error);
        console.error('❌ Sent payload:', boxData);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId]);
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
      buildingNumber: 'Building number',
      zone: 'Zone',
      length: 'Length',
      width: 'Width',
      height: 'Height',
      bimModelReference: 'BIM model reference',
      revitElementId: 'Revit element ID',
      plannedStartDate: 'Planned start date',
      duration: 'Duration',
      notes: 'Notes'
    };
    return labels[fieldName] || fieldName;
  }

  private validateSchedule(): void {
    if (!this.boxForm) {
      return;
    }

    const startControl = this.boxForm.get('plannedStartDate');
    const durationControl = this.boxForm.get('duration');
    if (!startControl) {
      return;
    }

    const existingErrors = { ...(startControl.errors || {}) };
    delete existingErrors['projectSchedule'];

    this.scheduleErrorMessage = null;

    const rawStart = startControl.value;
    const rawDuration = durationControl?.value;

    const startDate = rawStart ? new Date(rawStart) : null;
    const duration = rawDuration !== null && rawDuration !== '' ? Number(rawDuration) : null;

    if (startDate && this.projectPlannedStartDate && startDate < this.projectPlannedStartDate) {
      this.scheduleErrorMessage = `Planned start date must be on or after ${this.projectPlannedStartDate.toLocaleDateString()}.`;
      startControl.setErrors({ ...existingErrors, projectSchedule: true });
      return;
    }

    if (startDate && duration && duration > 0 && this.projectPlannedEndDate) {
      const boxPlannedEnd = new Date(startDate);
      boxPlannedEnd.setDate(boxPlannedEnd.getDate() + duration);

      if (boxPlannedEnd > this.projectPlannedEndDate) {
        this.scheduleErrorMessage = `Box schedule must finish on or before ${this.projectPlannedEndDate.toLocaleDateString()}.`;
        startControl.setErrors({ ...existingErrors, projectSchedule: true });
        return;
      }
    }

    startControl.setErrors(Object.keys(existingErrors).length ? existingErrors : null);
  }
}
