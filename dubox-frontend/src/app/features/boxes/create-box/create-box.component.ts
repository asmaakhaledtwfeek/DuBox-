import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormArray } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';
import { ProjectService } from '../../../core/services/project.service';
import { FactoryService, Factory, ProjectLocation } from '../../../core/services/factory.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { BoxType, BoxSubType } from '../../../core/models/box.model';

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
  projectNumber: string = '';
  projectName: string = '';
  projectCategoryId: number | null = null;
  projectCategoryName: string = '';
  projectLocation: string = ''; // Store project location
  currentStep = 1;
  totalSteps = 2;

  // Asset modal state
  isAssetModalOpen = false;
  editingAssetIndex: number | null = null;
  assetForm!: FormGroup;

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

  // Factory dropdown
  factories: Factory[] = [];
  loadingFactories = false;

  constructor(
    private fb: FormBuilder,
    private boxService: BoxService,
    private projectService: ProjectService,
    private factoryService: FactoryService,
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
    this.loadProjectAndBoxTypes();
    this.loadZones();
  }

  private loadProjectAndBoxTypes(): void {
    if (!this.projectId) return;
    
    this.loadingBoxTypes = true;
    console.log('🔍 Loading project details for projectId:', this.projectId);
    
    this.projectService.getProject(this.projectId).subscribe({
      next: (project: any) => {
        const projectData = project?.data || project;
        
        // Extract project details
        this.projectNumber = projectData?.code || projectData?.projectNumber || '';
        this.projectName = projectData?.name || projectData?.projectName || '';
        this.projectCategoryName = projectData?.categoryName || '';
        this.projectLocation = projectData?.location || '';
        
        console.log('📋 Project:', {
          code: this.projectNumber,
          name: this.projectName,
          category: this.projectCategoryName,
          location: this.projectLocation
        });

        // Load factories based on project location
        if (this.projectLocation) {
          this.loadFactoriesByLocation();
        }
        
        // Extract category ID
        const categoryId = projectData?.categoryId || projectData?.projectCategoryId;
        this.projectCategoryId = categoryId;
        console.log('📂 Project Category ID:', categoryId);
        
        if (categoryId) {
          console.log('🔄 Loading Box Types for Category ID:', categoryId);
          this.loadBoxTypes(categoryId);
        } else {
          console.warn('⚠️ No category ID found for project. Cannot load box types.');
          this.error = 'Project does not have a category assigned. Please assign a category to the project first.';
          this.loadingBoxTypes = false;
        }
      },
      error: (err: any) => {
        console.error('❌ Error loading project:', err);
        this.error = 'Failed to load project details. Please try again.';
        this.loadingBoxTypes = false;
      }
    });
  }

  private loadZones(): void {
    this.loadingZones = true;
    console.log('🔍 Loading zones...');

    this.boxService.getBoxZones().subscribe({
      next: (response: any) => {
        const zonesData = response?.data || response;
        this.zones = zonesData || [];
        console.log('✅ Zones loaded:', this.zones);
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

  private loadBoxTypes(categoryId: number): void {
    this.boxService.getBoxTypesByCategory(categoryId).subscribe({
      next: (types) => {
        this.boxTypes = types;
        this.loadingBoxTypes = false;
        
        console.log(`✅ Loaded ${types.length} Box Type(s) for Category ID ${categoryId}:`, 
          types.map(t => ({ 
            id: t.boxTypeId, 
            name: t.boxTypeName, 
            abbr: t.abbreviation,
            hasSubTypes: t.hasSubTypes  // ✅ Check this value
          }))
        );
        
        console.log('🔍 Full box types data:', types);
        
        if (types.length === 0) {
          console.warn('⚠️ No box types found for this project category.');
          this.error = 'No box types available for this project category. Please configure box types for this category first.';
        }
      },
      error: (err) => {
        console.error('❌ Error loading box types for category', categoryId, ':', err);
        this.error = 'Failed to load box types. Please try again.';
        this.loadingBoxTypes = false;
      }
    });
  }

  onBoxTypeChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const boxTypeId = parseInt(selectElement.value, 10);
    
    this.selectedBoxType = this.boxTypes.find(bt => bt.boxTypeId === boxTypeId) || null;
    this.boxSubTypes = [];
    
    console.log('📦 Box Type Selected:', {
      id: this.selectedBoxType?.boxTypeId,
      name: this.selectedBoxType?.boxTypeName,
      hasSubTypes: this.selectedBoxType?.hasSubTypes,
      hasSubTypesType: typeof this.selectedBoxType?.hasSubTypes  // Check data type
    });
    
    console.log('🔍 Full selected box type object:', this.selectedBoxType);
    
    // Update boxType field with the name for submission
    this.boxForm.patchValue({ 
      boxSubTypeId: null,
      boxType: this.selectedBoxType?.boxTypeName || ''
    });
    
    if (this.selectedBoxType?.hasSubTypes) {
      console.log('✅ HAS SUBTYPES - Loading Box Sub Types for Box Type ID:', boxTypeId);
      this.loadBoxSubTypes(boxTypeId);
    } else {
      console.log('❌ NO SUBTYPES - hasSubTypes value:', this.selectedBoxType?.hasSubTypes);
    }

    // Load available box letters when box type changes
    this.loadAvailableBoxLetters();
  }

  /**
   * Update BoxTag field automatically based on form values
   */
  private updateBoxTag(): void {
    const formValue = this.boxForm.getRawValue();
    
    // Components: ProjectNumber + BuildingNumber + Level + BoxType + BoxSubType + BoxLetter
    const projectNumber = this.projectNumber || '';
    const buildingNumber = formValue.buildingNumber || '';
    const level = formValue.floor || '';
    const boxTypeAbbr = this.selectedBoxType?.abbreviation || '';
    
    // Debug subtype selection
    console.log('🏷️ BoxTag Generation Debug:', {
      boxSubTypeId: formValue.boxSubTypeId,
      boxSubTypeIdType: typeof formValue.boxSubTypeId,
      availableSubTypes: this.boxSubTypes,
      boxSubTypesCount: this.boxSubTypes.length
    });
    
    // Convert to number for comparison if needed
    const selectedSubTypeId = formValue.boxSubTypeId ? Number(formValue.boxSubTypeId) : null;
    const selectedSubType = selectedSubTypeId 
      ? this.boxSubTypes.find(st => st.boxSubTypeId === selectedSubTypeId)
      : null;
    
    console.log('🔍 SubType Lookup:', {
      selectedSubTypeId,
      foundSubType: selectedSubType,
      abbreviation: selectedSubType?.abbreviation
    });
    
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
    
    console.log('✅ BoxTag Components:', {
      project: projectNumber,
      buildingNumber: buildingNumber,
      floor: level,
      type: boxTypeAbbr,
      subType: boxSubTypeAbbr,
      letter: boxLetter,
      final: boxTag
    });

    // Update the boxTag field (it's disabled, so we use patchValue)
    this.boxForm.patchValue({ boxTag }, { emitEvent: false });
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
    const params = {
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
        
        // If current selection is no longer available, clear it
        if (formValue.boxLetter && !this.availableBoxLetters.includes(formValue.boxLetter)) {
          this.boxForm.patchValue({ boxLetter: '' });
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

  private loadBoxSubTypes(boxTypeId: number): void {
    this.loadingBoxSubTypes = true;
    this.boxService.getBoxSubTypesByBoxType(boxTypeId).subscribe({
      next: (subTypes) => {
        this.boxSubTypes = subTypes;
        this.loadingBoxSubTypes = false;
        
        console.log(`✅ Loaded ${subTypes.length} Box Sub Type(s) for Box Type ID ${boxTypeId}:`,
          subTypes.map(st => ({ id: st.boxSubTypeId, name: st.boxSubTypeName, abbr: st.abbreviation }))
        );
        
        if (subTypes.length === 0) {
          console.warn('⚠️ No box sub types found for this box type.');
        }
      },
      error: (err) => {
        console.error('❌ Error loading box subtypes:', err);
        this.loadingBoxSubTypes = false;
        this.boxSubTypes = [];
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
      zone: ['', Validators.maxLength(50)],
      factoryId: [null],
      length: ['', [Validators.min(0), Validators.max(99999)]],
      width: ['', [Validators.min(0), Validators.max(99999)]],
      height: ['', [Validators.min(0), Validators.max(99999)]],
      bimModelReference: ['', Validators.maxLength(100)],
      revitElementId: ['', Validators.maxLength(100)],
      boxPlannedStartDate: [''],
      boxDuration: [null, [Validators.min(1)]],
      assets: this.fb.array([])
    });

    // Subscribe to form changes to auto-update BoxTag
    this.boxForm.valueChanges.subscribe(() => {
      this.updateBoxTag();
    });

    // Initialize available box letters
    this.availableBoxLetters = [...this.allBoxLetters];
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
    const step1Fields = ['boxTypeId', 'buildingNumber', 'floor', 'boxLetter'];
    let isValid = true;

    step1Fields.forEach(field => {
      const control = this.boxForm.get(field);
      if (control && control.invalid) {
        control.markAsTouched();
        isValid = false;
      }
    });

    // Check if BoxTag was generated
    const boxTag = this.boxForm.getRawValue().boxTag;
    if (!boxTag || boxTag.trim() === '') {
      isValid = false;
      this.error = 'Box Tag could not be generated. Please ensure all required fields are filled.';
      setTimeout(() => this.error = '', 5000);
    }

    if (!isValid && !this.error) {
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

    const formValue = this.boxForm.getRawValue(); // Use getRawValue to get disabled fields

    // Map frontend fields to backend expected format
    const boxData: any = {
      projectId: this.projectId,
      boxTag: formValue.boxTag,
      boxName: formValue.boxName || undefined,
      boxType: this.selectedBoxType?.boxTypeName || formValue.boxType,
      boxTypeId: formValue.boxTypeId || undefined,
      boxSubTypeId: formValue.boxSubTypeId || undefined,
      floor: formValue.floor,
      buildingNumber: formValue.buildingNumber || undefined,
      boxLetter: formValue.boxLetter || undefined,
      zone: formValue.zone || undefined,
      length: formValue.length ? parseFloat(formValue.length) : undefined,
      width: formValue.width ? parseFloat(formValue.width) : undefined,
      height: formValue.height ? parseFloat(formValue.height) : undefined,
      bimModelReference: formValue.bimModelReference || undefined,
      revitElementId: formValue.revitElementId || undefined,
      boxPlannedStartDate: formValue.boxPlannedStartDate ? new Date(formValue.boxPlannedStartDate).toISOString() : undefined,
      boxDuration: formValue.boxDuration ? parseInt(formValue.boxDuration, 10) : undefined,
      factoryId: formValue.factoryId || undefined,
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
      boxTypeId: 'Box type',
      buildingNumber: 'Building number',
      floor: 'Floor',
      boxLetter: 'Box letter',
      zone: 'Zone',
      length: 'Length',
      width: 'Width',
      height: 'Height',
      bimModelReference: 'BIM model reference',
      revitElementId: 'Revit element ID',
      boxPlannedStartDate: 'Box planned start date',
      boxDuration: 'Box duration',
      factoryId: 'Factory'
    };
    return labels[fieldName] || fieldName;
  }

  /**
   * Load factories based on project location
   */
  private loadFactoriesByLocation(): void {
    if (!this.projectLocation) {
      return;
    }

    // Map project location string to ProjectLocation enum
    let locationEnum: ProjectLocation;
    const locationUpper = this.projectLocation.toUpperCase();
    if (locationUpper === 'KSA') {
      locationEnum = ProjectLocation.KSA;
    } else if (locationUpper === 'UAE') {
      locationEnum = ProjectLocation.UAE;
    } else {
      console.warn('⚠️ Unknown project location:', this.projectLocation);
      return;
    }

    this.loadingFactories = true;
    this.factoryService.getFactoriesByLocation(locationEnum).subscribe({
      next: (factories) => {
        this.factories = factories;
        this.loadingFactories = false;
        console.log(`✅ Loaded ${factories.length} factory(ies) for location: ${this.projectLocation}`);
      },
      error: (err) => {
        console.error('❌ Error loading factories:', err);
        this.loadingFactories = false;
        this.factories = [];
      }
    });
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

