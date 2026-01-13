import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxType, BoxSubType, getBoxStatusNumber } from '../../../core/models/box.model';
import { FactoryService, Factory, ProjectLocation } from '../../../core/services/factory.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ProjectService } from '../../../core/services/project.service';
import { 
  ProjectConfiguration, 
  ProjectBuilding, 
  ProjectLevel, 
  ProjectBoxType,
  ProjectZone, 
  ProjectBoxFunction 
} from '../../../core/models/project-configuration.model';

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
  project: any = null; // Store project details
  isProjectArchived = false; // Track if project is archived
  isProjectOnHold = false; // Track if project is on hold
  isProjectClosed = false; // Track if project is closed
  projectPlannedStartDate?: Date;
  projectPlannedEndDate?: Date;
  scheduleErrorMessage: string | null = null;
  projectCategoryId: number | null = null;
  projectCategoryName: string = '';
  projectLocation: string = ''; // Store project location

  // Project Configuration
  projectConfiguration: ProjectConfiguration | null = null;
  loadingConfiguration = false;
  
  // Box types - can be from project config or system default
  boxTypes: any[] = [];
  boxSubTypes: any[] = [];
  selectedBoxType: any | null = null;
  loadingBoxTypes = false;
  loadingBoxSubTypes = false;

  // Buildings from project config or default
  buildingNumbers: string[] = [];
  
  // Floors/Levels from project config or default
  floors: string[] = [];

  // Zones from project config or system default
  zones: any[] = [];
  loadingZones = false;
  
  // Box Functions from project config
  boxFunctions: ProjectBoxFunction[] = [];

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
    this.loadProjectConfigurationAndData();
    this.loadBox();
    this.loadProjectSchedule();
  }

  private loadProjectConfigurationAndData(): void {
    if (!this.projectId) return;
    
    this.loadingConfiguration = true;
    this.loadingBoxTypes = true;
    console.log('🔍 Loading project configuration for projectId:', this.projectId);
    
    // Load project details first
    this.projectService.getProject(this.projectId).subscribe({
      next: (project: any) => {
        const projectData = project?.data || project;
        this.project = projectData;
        
        // Check if project is archived, on hold, or closed
        this.isProjectArchived = projectData?.status === 'Archived';
        this.isProjectOnHold = projectData?.status === 'OnHold';
        this.isProjectClosed = projectData?.status === 'Closed';
        if (this.isProjectArchived || this.isProjectClosed) {
          this.boxForm.disable();
          this.error = 'This project is archived. You can only view the box details but cannot make any modifications.';
        } else if (this.isProjectOnHold) {
          this.boxForm.disable();
          this.error = 'This project is on hold. You can only view the box details but cannot make any modifications. Only project status changes are allowed.';
        } else if (this.isProjectClosed) {
          this.boxForm.disable();
          this.error = 'This project is closed. You can only view the box details but cannot make any modifications. Only project status changes are allowed.';
        }
        
        // Extract project details
        this.projectCategoryName = projectData?.categoryName || '';
        this.projectLocation = projectData?.location || '';
        
        // Extract category ID for fallback
        const categoryId = projectData?.categoryId || projectData?.projectCategoryId;
        this.projectCategoryId = categoryId;
        
        console.log('📂 Project Category ID:', categoryId);
        console.log('📍 Project Location:', this.projectLocation);
        
        // Load project configuration
        this.loadProjectConfiguration();
      },
      error: (err: any) => {
        console.error('❌ Error loading project:', err);
        this.loadingConfiguration = false;
        this.loadingBoxTypes = false;
      }
    });
  }

  private loadProjectConfiguration(): void {
    console.log('🔧 Loading project configuration...');
    
    this.projectService.getProjectConfiguration(this.projectId).subscribe({
      next: (config: ProjectConfiguration) => {
        console.log('✅ Project configuration loaded:', config);
        this.projectConfiguration = config;
        
        // Use project-specific buildings or default
        if (config.buildings && config.buildings.length > 0) {
          this.buildingNumbers = config.buildings
            .sort((a, b) => (a.displayOrder || 0) - (b.displayOrder || 0))
            .map(b => b.buildingCode);
          console.log('📦 Using project buildings:', this.buildingNumbers);
        } else {
          this.buildingNumbers = ['B01', 'B02', 'B03', 'B04', 'B05'];
          console.log('📦 Using default buildings');
        }
        
        // Use project-specific levels or default
        if (config.levels && config.levels.length > 0) {
          this.floors = config.levels
            .sort((a, b) => (a.displayOrder || 0) - (b.displayOrder || 0))
            .map(l => l.levelCode);
          console.log('🏢 Using project levels:', this.floors);
        } else {
          this.floors = ['GF', 'FF', '1F', '2F', '3F', '4F', '5F', 'BF', 'RF'];
          console.log('🏢 Using default levels');
        }
        
        // Use project-specific box types ONLY
        if (config.boxTypes && config.boxTypes.length > 0) {
          this.boxTypes = config.boxTypes
            .sort((a, b) => (a.displayOrder || 0) - (b.displayOrder || 0))
            .map(t => ({
              boxTypeId: t.id,
              boxTypeName: t.typeName,
              abbreviation: t.abbreviation,
              hasSubTypes: t.hasSubTypes,
              subTypes: t.subTypes || []
            }));
          console.log('📋 Using project box types:', this.boxTypes);
          this.loadingBoxTypes = false;
          
          // Preselect box type if box is loaded
          if (this.box) {
            this.preselectBoxTypeAndSubType();
          }
        } else {
          console.warn('⚠️ No box types configured for this project. Please configure box types in project settings.');
          this.loadingBoxTypes = false;
        }
        
        // Use project-specific zones or load system default
        if (config.zones && config.zones.length > 0) {
          this.zones = config.zones
            .sort((a, b) => (a.displayOrder || 0) - (b.displayOrder || 0))
            .map(z => ({
              value: z.zoneCode,  // Use zoneCode (string) instead of id (numeric)
              name: z.zoneCode,
              displayName: z.zoneName || z.zoneCode
            }));
          console.log('🗺️ Using project zones:', this.zones);
          this.loadingZones = false;
        } else {
          console.log('🗺️ No project zones, loading system defaults');
          this.loadZones();
        }
        
        // Load box functions if available
        if (config.boxFunctions && config.boxFunctions.length > 0) {
          this.boxFunctions = config.boxFunctions
            .sort((a, b) => (a.displayOrder || 0) - (b.displayOrder || 0));
          console.log('⚙️ Box functions loaded:', this.boxFunctions);
        }
        
        this.loadingConfiguration = false;
      },
      error: (err: any) => {
        console.log('⚠️ No project configuration found:', err);
        this.projectConfiguration = null;
        
        // Use defaults for buildings and floors only
        this.buildingNumbers = ['B01', 'B02', 'B03', 'B04', 'B05'];
        this.floors = ['GF', 'FF', '1F', '2F', '3F', '4F', '5F', 'BF', 'RF'];
        
        // No box types if no configuration
        this.boxTypes = [];
        this.loadingBoxTypes = false;
        console.warn('⚠️ No box types available. Please configure box types for this project.');
        
        this.loadZones();
        
        this.loadingConfiguration = false;
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
    
    // Load sub types from project configuration
    if (this.selectedBoxType?.hasSubTypes && this.selectedBoxType.subTypes && this.selectedBoxType.subTypes.length > 0) {
      console.log('✅ HAS SUBTYPES - Loading from project configuration');
      this.boxSubTypes = this.selectedBoxType.subTypes.map((st: any) => ({
        boxSubTypeId: st.id,
        boxSubTypeName: st.subTypeName,
        abbreviation: st.abbreviation,
        boxTypeId: st.projectBoxTypeId || st.boxTypeId
      }));
      console.log('📋 Sub types loaded:', this.boxSubTypes);
    } else if (this.selectedBoxType?.hasSubTypes) {
      console.log('⚠️ Box type has subTypes flag but no subtypes configured');
    } else {
      console.log('❌ NO SUBTYPES - hasSubTypes value:', this.selectedBoxType?.hasSubTypes);
    }
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
   * Trigger update when building or floor changes
   */
  onBuildingOrFloorChange(): void {
    this.updateBoxTag();
  }

  private loadZones(): void {
    this.loadingZones = true;
    this.boxService.getBoxZones().subscribe({
      next: (response: any) => {
        const zonesData = response?.data || response;
        
        // Map zones to ensure value is the zone code string, not numeric
        if (Array.isArray(zonesData)) {
          this.zones = zonesData.map((z: any) => ({
            value: z.zoneCode || z.name || z.value,
            name: z.zoneCode || z.name,
            displayName: z.zoneName || z.displayName || z.zoneCode || z.name
          }));
        } else {
          this.zones = [];
        }
        
        this.loadingZones = false;
        
        // After zones are loaded, preselect if box is already loaded
        if (this.box) {
          this.preselectZone();
        }
      },
      error: (error) => {
        console.error('❌ Error loading zones:', error);
        this.loadingZones = false;
        // Fallback to default zones if API fails - use string codes
        this.zones = [
          { value: 'Zone A', name: 'ZoneA', displayName: 'Zone A' },
          { value: 'Zone B', name: 'ZoneB', displayName: 'Zone B' },
          { value: 'Zone C', name: 'ZoneC', displayName: 'Zone C' },
          { value: 'Zone D', name: 'ZoneD', displayName: 'Zone D' },
          { value: 'Zone E', name: 'ZoneE', displayName: 'Zone E' },
          { value: 'Zone F', name: 'ZoneF', displayName: 'Zone F' },
          { value: 'Zone G', name: 'ZoneG', displayName: 'Zone G' },
          { value: 'Zone H', name: 'ZoneH', displayName: 'Zone H' },
          { value: 'Zone I', name: 'ZoneI', displayName: 'Zone I' },
          { value: 'Zone J', name: 'ZoneJ', displayName: 'Zone J' }
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
      boxTypeId: [null], // Legacy field - kept for backward compatibility but not required (uses project config now)
      boxSubTypeId: [null], // Legacy field - kept for backward compatibility but not required (uses project config now)
      buildingNumber: ['', Validators.required],
      floor: ['GF', Validators.required],
      boxFunction: [''],
      zone: [''], // Zone is a string (ZoneCode from ProjectZone)
      length: ['', [Validators.min(0), Validators.max(99999)]],
      width: ['', [Validators.min(0), Validators.max(99999)]],
      height: ['', [Validators.min(0), Validators.max(99999)]],
      revitElementId: ['', Validators.maxLength(100)],
      boxPlannedStartDate: [''],
      boxDuration: [null, [Validators.min(1)]],
      notes: ['', Validators.maxLength(1000)]
    });

    // Subscribe to form changes to auto-update BoxTag
    this.boxForm.valueChanges.subscribe(() => {
      this.updateBoxTag();
    });

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
    // Use abbreviation if available, otherwise use the type name
    const boxTypeAbbr = this.selectedBoxType?.abbreviation || this.selectedBoxType?.boxTypeName || '';
    
    // Convert to number for comparison if needed
    const selectedSubTypeId = formValue.boxSubTypeId ? Number(formValue.boxSubTypeId) : null;
    const selectedSubType = selectedSubTypeId 
      ? this.boxSubTypes.find(st => st.boxSubTypeId === selectedSubTypeId)
      : null;
    
    // Use abbreviation if available, otherwise use the subtype name
    const boxSubTypeAbbr = selectedSubType?.abbreviation || selectedSubType?.boxSubTypeName || '';

    // Generate BoxTag with separators (-)
    const components: string[] = [];
    if (projectNumber) components.push(projectNumber);
    if (buildingNumber) components.push(buildingNumber);
    if (level) components.push(level);
    if (boxTypeAbbr) components.push(boxTypeAbbr);
    if (boxSubTypeAbbr) components.push(boxSubTypeAbbr);

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
      revitElementId: box.revitElementId,
      boxTypeId: box.type,
      boxSubTypeId: box.subType
    });
    
    this.boxForm.patchValue({
      boxTag: box.code,
      boxName: box.name || '',
      boxType: box.boxTypeName || box.type || '',
      boxTypeId: box.type || null,
      boxSubTypeId: box.subType || null,
      floor: box.floor || 'GF',
      buildingNumber: box.buildingNumber || '',
      boxFunction: (box as any).boxFunction || '',
      // Don't set zone here - let preselectZone handle it after zones are loaded
      // zone: box.zone || '',
      length: box.length || '',
      width: box.width || '',
      height: box.height || '',
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

    // Zone is now a string (ZoneCode from ProjectZone)
    const zoneValue = this.box.zone as string;
    
    // Try to find the zone by matching value (zoneCode), name, or displayName
    const zone = this.zones.find(z => 
      z.value === zoneValue || 
      z.name === zoneValue || 
      z.displayName === zoneValue ||
      z.value?.toLowerCase() === zoneValue?.toLowerCase() ||
      z.name?.toLowerCase() === zoneValue?.toLowerCase() ||
      z.displayName?.toLowerCase() === zoneValue?.toLowerCase()
    );

    if (zone) {
      this.boxForm.patchValue({ zone: zone.value });
      console.log(`✅ Preselected Zone: ${zone.value} (${zone.displayName})`);
    } else {
      console.warn(`⚠️ Zone "${zoneValue}" not found in zones list. Available zones:`, this.zones.map(z => z.value));
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

      // If box type has subtypes, load them from project configuration
      if (boxType.hasSubTypes && boxType.subTypes && boxType.subTypes.length > 0) {
        console.log('✅ Loading subtypes from project configuration');
        this.boxSubTypes = boxType.subTypes.map((st: any) => ({
          boxSubTypeId: st.id,
          boxSubTypeName: st.subTypeName,
          abbreviation: st.abbreviation,
          boxTypeId: st.projectBoxTypeId || st.boxTypeId
        }));
        console.log('📋 Sub types loaded:', this.boxSubTypes);
        
        // Preselect the sub type if box has one
        if (this.box.boxSubTypeId) {
          const subTypeId = Number(this.box.boxSubTypeId);
          console.log(`🔍 Preselecting Sub Type ID: ${subTypeId}`);
          this.boxForm.patchValue({ boxSubTypeId: subTypeId });
        }
      } else if (boxType.hasSubTypes) {
        console.log('⚠️ Box type has subTypes flag but no subtypes configured');
      }
    } else {
      console.warn(`⚠️ Box Type ID ${boxTypeId} not found in loaded box types`);
      console.warn(`📋 Available IDs:`, this.boxTypes.map(bt => bt.boxTypeId));
    }
  }

  onSubmit(): void {
    if (this.isProjectArchived || this.isProjectClosed) {
      this.error = 'Cannot save changes. This project is archived and read-only.';
      return;
    }
    if (this.isProjectOnHold || this.isProjectClosed) {
      this.error = 'Cannot save changes. This project is on hold. Only project status changes are allowed.';
      return;
    }

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
      boxTypeId: null, // Always null - box type info is in BoxTag from project config (avoids FK constraint to old BoxTypes table)
      boxSubTypeId: null, // Always null - box subtype info is in BoxTag from project config (avoids FK constraint to old BoxSubTypes table)
      floor: formValue.floor || null,
      buildingNumber: formValue.buildingNumber || null,
      boxFunction: formValue.boxFunction || null,
      zone: formValue.zone || null,
      status: statusNumber,  // REQUIRED (int) - converted from string
      length: formValue.length ? parseFloat(formValue.length) : null,
      width: formValue.width ? parseFloat(formValue.width) : null,
      height: formValue.height ? parseFloat(formValue.height) : null,
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
      boxFunction: 'Box function',
      zone: 'Zone',
      length: 'Length',
      width: 'Width',
      height: 'Height',
      revitElementId: 'Revit Group Element ID',
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
