import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxType, BoxSubType, getBoxStatusNumber } from '../../../core/models/box.model';
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
  projectCategoryId: number | null = null;
  projectCategoryName: string = '';

  // Box types loaded from backend based on project category
  boxTypes: BoxType[] = [];
  boxSubTypes: BoxSubType[] = [];
  selectedBoxType: BoxType | null = null;
  loadingBoxTypes = false;
  loadingBoxSubTypes = false;

  floors = [
    'GF', 'FF', '1F', '2F', '3F', '4F', '5F',
    'BF', 'RF'
  ];

  // Building numbers dropdown
  buildingNumbers = ['B01', 'B02', 'B03', 'B04', 'B05', 'B06', 'B07', 'B08', 'B09'];

  // Zones dropdown
  zones: { value: number; name: string; displayName: string }[] = [];
  loadingZones = false;

  // Box letters dropdown (will be filtered based on usage)
  allBoxLetters = ['A', 'B', 'C', 'D', 'E', 'F'];
  availableBoxLetters: string[] = [];
  loadingBoxLetters = false;

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
    this.loadProjectAndBoxTypes();
    this.loadBox();
    this.loadProjectSchedule();
  }

  private loadProjectAndBoxTypes(): void {
    if (!this.projectId) return;
    
    this.loadingBoxTypes = true;
    console.log('🔍 Loading project details for projectId:', this.projectId);
    
    this.projectService.getProject(this.projectId).subscribe({
      next: (project: any) => {
        const projectData = project?.data || project;
        
        // Extract project details
        this.projectCategoryName = projectData?.categoryName || '';
        
        // Extract category ID
        const categoryId = projectData?.categoryId || projectData?.projectCategoryId;
        this.projectCategoryId = categoryId;
        console.log('📂 Project Category ID:', categoryId);
        
        if (categoryId) {
          console.log('🔄 Loading Box Types for Category ID:', categoryId);
          this.loadBoxTypes(categoryId);
        } else {
          console.warn('⚠️ No category ID found for project. Cannot load box types.');
          this.loadingBoxTypes = false;
        }
      },
      error: (err: any) => {
        console.error('❌ Error loading project:', err);
        this.loadingBoxTypes = false;
      }
    });
  }

  private loadBoxTypes(categoryId: number): void {
    this.boxService.getBoxTypesByCategory(categoryId).subscribe({
      next: (types) => {
        this.boxTypes = types;
        this.loadingBoxTypes = false;
        
        console.log(`✅ Loaded ${types.length} Box Type(s) for Category ID ${categoryId}`);
        
        if (types.length === 0) {
          console.warn('⚠️ No box types found for this project category.');
        }
        
        // After box types are loaded, preselect if box is already loaded
        if (this.box) {
          this.preselectBoxTypeAndSubType();
        }
      },
      error: (err) => {
        console.error('❌ Error loading box types for category', categoryId, ':', err);
        this.loadingBoxTypes = false;
      }
    });
  }

  onBoxTypeChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const boxTypeId = parseInt(selectElement.value, 10);
    
    this.selectedBoxType = this.boxTypes.find(bt => bt.boxTypeId === boxTypeId) || null;
    this.boxSubTypes = [];
    
    // Update boxType field with the name for submission
    this.boxForm.patchValue({ 
      boxSubTypeId: null,
      boxType: this.selectedBoxType?.boxTypeName || ''
    });
    
    if (this.selectedBoxType?.hasSubTypes) {
      console.log('✅ HAS SUBTYPES - Loading Box Sub Types for Box Type ID:', boxTypeId);
      this.loadBoxSubTypes(boxTypeId);
    }

    // Load available box letters when box type changes
    this.loadAvailableBoxLetters();
  }

  private loadBoxSubTypes(boxTypeId: number, preselectSubTypeId?: number): void {
    this.loadingBoxSubTypes = true;
    this.boxService.getBoxSubTypesByBoxType(boxTypeId).subscribe({
      next: (subTypes) => {
        this.boxSubTypes = subTypes;
        this.loadingBoxSubTypes = false;
        
        console.log(`✅ Loaded ${subTypes.length} Box Sub Type(s) for Box Type ID ${boxTypeId}`);
        console.log(`📋 Available Sub Types:`, subTypes.map(st => ({ id: st.boxSubTypeId, name: st.boxSubTypeName })));
        
        // If we need to preselect a subtype, do it now
        if (preselectSubTypeId !== undefined && preselectSubTypeId !== null) {
          const subTypeId = Number(preselectSubTypeId);
          const subTypeExists = subTypes.some(st => Number(st.boxSubTypeId) === subTypeId);
          if (subTypeExists) {
            this.boxForm.patchValue({ boxSubTypeId: subTypeId });
            console.log(`✅ Preselected Box Sub Type ID: ${subTypeId}`);
          } else {
            console.warn(`⚠️ Box Sub Type ID ${subTypeId} not found in loaded subtypes`);
            console.warn(`📋 Available Sub Type IDs:`, subTypes.map(st => st.boxSubTypeId));
          }
        }
      },
      error: (err) => {
        console.error('❌ Error loading box subtypes:', err);
        this.loadingBoxSubTypes = false;
        this.boxSubTypes = [];
      }
    });
  }

  /**
   * Load available box letters by filtering out used ones
   */
  private loadAvailableBoxLetters(): void {
    const formValue = this.boxForm.getRawValue();
    
    // Check if we have all required fields to query
    if (!this.projectId || !formValue.buildingNumber || !formValue.floor || !formValue.boxTypeId) {
      this.availableBoxLetters = [...this.allBoxLetters];
      return;
    }

    this.loadingBoxLetters = true;

    // Build query parameters
    const params: any = {
      projectId: this.projectId,
      buildingNumber: formValue.buildingNumber,
      floor: formValue.floor,
      boxTypeId: formValue.boxTypeId
    };

    // Fetch used box letters from backend
    this.boxService.getUsedBoxLetters(params).subscribe({
      next: (usedLetters: string[]) => {
        // Filter out used letters
        this.availableBoxLetters = this.allBoxLetters.filter(
          letter => !usedLetters.includes(letter)
        );
        
        // If current selection is no longer available, keep it (for edit mode)
        const currentLetter = formValue.boxLetter;
        if (currentLetter && !this.availableBoxLetters.includes(currentLetter)) {
          this.availableBoxLetters.push(currentLetter);
        }
        
        this.loadingBoxLetters = false;
      },
      error: (err) => {
        console.error('Error loading used box letters:', err);
        // Fallback: show all letters
        this.availableBoxLetters = [...this.allBoxLetters];
        this.loadingBoxLetters = false;
      }
    });
  }

  /**
   * Trigger box letter loading when relevant fields change
   */
  onBuildingOrFloorChange(): void {
    this.loadAvailableBoxLetters();
  }

  private loadZones(): void {
    this.loadingZones = true;
    this.boxService.getBoxZones().subscribe({
      next: (response: any) => {
        const zonesData = response?.data || response;
        this.zones = zonesData || [];
        this.loadingZones = false;
        
        // After zones are loaded, preselect if box is already loaded
        if (this.box) {
          this.preselectZone();
        }
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
        
        // After fallback zones are set, preselect if box is already loaded
        if (this.box) {
          this.preselectZone();
        }
      }
    });
  }

  private initForm(): void {
    this.boxForm = this.fb.group({
      boxTag: [{ value: '', disabled: true }, Validators.maxLength(50)],
      boxName: ['', Validators.maxLength(200)],
      boxType: [''], // Used for submission, populated from selectedBoxType
      boxTypeId: [null, Validators.required],
      boxSubTypeId: [null],
      buildingNumber: ['', Validators.required],
      floor: ['GF', Validators.required],
      boxLetter: ['', Validators.required],
      zone: [null], // Zone is a number (BoxZone enum value)
      length: ['', [Validators.min(0), Validators.max(99999)]],
      width: ['', [Validators.min(0), Validators.max(99999)]],
      height: ['', [Validators.min(0), Validators.max(99999)]],
      bimModelReference: ['', Validators.maxLength(100)],
      revitElementId: ['', Validators.maxLength(100)],
      boxPlannedStartDate: [''],
      boxDuration: [null, [Validators.min(1)]],
      notes: ['', Validators.maxLength(1000)]
    });

    // Subscribe to form changes to auto-update BoxTag
    this.boxForm.valueChanges.subscribe(() => {
      this.updateBoxTag();
    });

    // Initialize available box letters
    this.availableBoxLetters = [...this.allBoxLetters];

    this.boxForm.get('boxPlannedStartDate')?.valueChanges.subscribe(() => this.validateSchedule());
    this.boxForm.get('boxDuration')?.valueChanges.subscribe(() => this.validateSchedule());
  }

  /**
   * Update BoxTag field automatically based on form values
   */
  private updateBoxTag(): void {
    const formValue = this.boxForm.getRawValue();
    
    // Get project number from box code (first part before first dash)
    const boxCode = this.box?.code || '';
    const projectNumber = boxCode.split('-')[0] || '';
    const buildingNumber = formValue.buildingNumber || '';
    const level = formValue.floor || '';
    const boxTypeAbbr = this.selectedBoxType?.abbreviation || '';
    
    // Convert to number for comparison if needed
    const selectedSubTypeId = formValue.boxSubTypeId ? Number(formValue.boxSubTypeId) : null;
    const selectedSubType = selectedSubTypeId 
      ? this.boxSubTypes.find(st => st.boxSubTypeId === selectedSubTypeId)
      : null;
    
    const boxSubTypeAbbr = selectedSubType?.abbreviation || '';
    const boxLetter = formValue.boxLetter || '';

    // Generate BoxTag with separators (-)
    const components: string[] = [];
    if (projectNumber) components.push(projectNumber);
    if (buildingNumber) components.push(buildingNumber);
    if (level) components.push(level);
    if (boxTypeAbbr) components.push(boxTypeAbbr);
    if (boxSubTypeAbbr) components.push(boxSubTypeAbbr);
    if (boxLetter) components.push(boxLetter);

    const boxTag = components.join('-');

    // Update the boxTag field (it's disabled, so we use patchValue)
    this.boxForm.patchValue({ boxTag }, { emitEvent: false });
  }

  private loadBox(): void {
    this.loading = true;
    this.error = '';
    
    this.boxService.getBox(this.boxId).subscribe({
      next: (box) => {
        console.log(box);
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
      revitElementId: box.revitElementId,
      boxTypeId: box.type,
      boxSubTypeId: box.subType,
      boxLetter: box.boxLetter
    });
    
    this.boxForm.patchValue({
      boxTag: box.code,
      boxName: box.name || '',
      boxType: box.boxTypeName || box.type || '',
      boxTypeId: box.type || null,
      boxSubTypeId: box.subType || null,
      floor: box.floor || 'GF',
      buildingNumber: box.buildingNumber || '',
      boxLetter: box.boxLetter || '',
      // Don't set zone here - let preselectZone handle it after zones are loaded
      // zone: box.zone || '',
      length: box.length || '',
      width: box.width || '',
      height: box.height || '',
      bimModelReference: box.bimModelReference || '',
      revitElementId: box.revitElementId || '',
      boxPlannedStartDate: box.plannedStartDate ? box.plannedStartDate.toISOString().split('T')[0] : '',
      boxDuration: box.duration ?? null,
      notes: box.notes || ''
    });
    
    // Preselect box type and subtype after form is populated
    // This will work if box types are already loaded, otherwise it will be called from loadBoxTypes
    // Use setTimeout to ensure form is fully updated
    setTimeout(() => {
      this.preselectBoxTypeAndSubType();
    }, 100);
    
    // Preselect zone after form is populated
    // This will work if zones are already loaded, otherwise it will be called from loadZones
    setTimeout(() => {
      this.preselectZone();
    }, 100);
    
    // Load available box letters after form is populated
    setTimeout(() => {
      this.loadAvailableBoxLetters();
    }, 500);
    
    console.log('✅ Form populated, current values:', this.boxForm.value);
  }

  /**
   * Preselect the zone based on the current box data
   */
  private preselectZone(): void {
    if (!this.box || !this.box.zone) {
      return;
    }

    // Check if zones are loaded
    if (this.zones.length === 0) {
      console.log('⏳ Zones not loaded yet, will preselect after they load');
      return;
    }

    // Convert zone to number if it's a string or enum name
    let zoneValue: number | null = null;
    
    if (typeof this.box.zone === 'number') {
      zoneValue = this.box.zone;
    } else if (typeof this.box.zone === 'string') {
      // Try to parse as number first
      const parsed = Number(this.box.zone);
      if (!isNaN(parsed)) {
        zoneValue = parsed;
      } else {
        // Try to find by name (e.g., "ZoneA", "Zone B", etc.)
        const zone = this.zones.find(z => 
          z.name === this.box!.zone || 
          z.displayName === this.box!.zone ||
          z.name.toLowerCase() === (this.box!.zone as string).toLowerCase() ||
          z.displayName.toLowerCase() === (this.box!.zone as string).toLowerCase()
        );
        if (zone) {
          zoneValue = zone.value;
        }
      }
    }

    if (zoneValue !== null) {
      // Check if the zone value exists in the zones list
      const zoneExists = this.zones.some(z => z.value === zoneValue);
      if (zoneExists) {
        this.boxForm.patchValue({ zone: zoneValue });
        console.log(`✅ Preselected Zone: ${zoneValue}`);
      } else {
        console.warn(`⚠️ Zone value ${zoneValue} not found in loaded zones`);
      }
    } else {
      console.warn(`⚠️ Could not determine zone value from: ${this.box.zone}`);
    }
  }

  /**
   * Preselect the box type and subtype based on the current box data
   */
  private preselectBoxTypeAndSubType(): void {
    if (!this.box || !this.box.boxTypeId) {
      console.log('⏳ Cannot preselect: box or boxTypeId not available');
      return;
    }

    // Check if box types are loaded
    if (this.boxTypes.length === 0) {
      console.log('⏳ Box types not loaded yet, will preselect after they load');
      return;
    }

    // Convert to number for comparison (in case of string/number mismatch)
    const boxTypeId = Number(this.box.boxTypeId);
    console.log(`🔍 Looking for Box Type ID: ${boxTypeId} (type: ${typeof boxTypeId})`);
    console.log(`📋 Available Box Types:`, this.boxTypes.map(bt => ({ id: bt.boxTypeId, name: bt.boxTypeName })));

    // Find and select the box type
    const boxType = this.boxTypes.find(bt => Number(bt.boxTypeId) === boxTypeId);
    if (boxType) {
      this.selectedBoxType = boxType;
      
      // Use setValue to ensure the form control is properly updated
      const currentValue = this.boxForm.get('boxTypeId')?.value;
      if (currentValue !== boxType.boxTypeId) {
        this.boxForm.patchValue({
          boxTypeId: boxType.boxTypeId,
          boxType: boxType.boxTypeName
        });
        console.log(`✅ Preselected Box Type: ${boxType.boxTypeName} (ID: ${boxType.boxTypeId})`);
      } else {
        console.log(`✅ Box Type already selected: ${boxType.boxTypeName} (ID: ${boxType.boxTypeId})`);
      }

      // If box has a subtype and the box type has subtypes, load and preselect it
      if (this.box.boxSubTypeId && boxType.hasSubTypes) {
        console.log(`🔄 Loading subtypes for Box Type ID ${boxType.boxTypeId} and preselecting Sub Type ID ${this.box.boxSubTypeId}`);
        this.loadBoxSubTypes(boxType.boxTypeId, Number(this.box.boxSubTypeId));
      } else if (boxType.hasSubTypes) {
        // Box type has subtypes but box doesn't have one selected, just load them
        console.log(`🔄 Loading subtypes for Box Type ID ${boxType.boxTypeId} (no preselection)`);
        this.loadBoxSubTypes(boxType.boxTypeId);
      }
    } else {
      console.warn(`⚠️ Box Type ID ${boxTypeId} not found in loaded box types`);
      console.warn(`📋 Available IDs:`, this.boxTypes.map(bt => bt.boxTypeId));
    }
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

    const formValue = this.boxForm.getRawValue(); // Use getRawValue to get disabled fields
    
    // Convert status string to number
    const statusNumber = getBoxStatusNumber(this.box.status);
    console.log(`🔄 Status conversion: "${this.box.status}" → ${statusNumber}`);

    // Map frontend fields to backend expected format (UpdateBoxDto)
    const plannedStartDate = formValue.boxPlannedStartDate
      ? new Date(formValue.boxPlannedStartDate)
      : null;
    const duration = formValue.boxDuration !== null && formValue.boxDuration !== ''
      ? Number(formValue.boxDuration)
      : null;
    const notes = formValue.notes ? formValue.notes.trim() : '';

    const boxData: any = {
      boxId: this.boxId,  // REQUIRED (Guid)
      boxTag: formValue.boxTag || null,
      boxName: formValue.boxName || null,
      boxType: this.selectedBoxType?.boxTypeName || formValue.boxType || null,
      boxTypeId: formValue.boxTypeId || null,
      boxSubTypeId: formValue.boxSubTypeId || null,
      floor: formValue.floor || null,
      buildingNumber: formValue.buildingNumber || null,
      boxLetter: formValue.boxLetter || null,
      zone: formValue.zone || null,
      status: statusNumber,  // REQUIRED (int) - converted from string
      length: formValue.length ? parseFloat(formValue.length) : null,
      width: formValue.width ? parseFloat(formValue.width) : null,
      height: formValue.height ? parseFloat(formValue.height) : null,
      bimModelReference: formValue.bimModelReference || null,
      revitElementId: formValue.revitElementId || null,
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
      boxTypeId: 'Box type',
      boxSubTypeId: 'Box sub type',
      floor: 'Floor',
      buildingNumber: 'Building number',
      boxLetter: 'Box letter',
      zone: 'Zone',
      length: 'Length',
      width: 'Width',
      height: 'Height',
      bimModelReference: 'BIM model reference',
      revitElementId: 'Revit element ID',
      boxPlannedStartDate: 'Box planned start date',
      boxDuration: 'Box duration',
      notes: 'Notes'
    };
    return labels[fieldName] || fieldName;
  }

  private validateSchedule(): void {
    if (!this.boxForm) {
      return;
    }

    const startControl = this.boxForm.get('boxPlannedStartDate');
    const durationControl = this.boxForm.get('boxDuration');
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
