import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ProjectService } from '../../../core/services/project.service';
import { BoxService } from '../../../core/services/box.service';
import { AuthService } from '../../../core/services/auth.service';
import { ToastService } from '../../../core/services/toast.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { Project } from '../../../core/models/project.model';
import { ProjectTypeCategory } from '../../../core/models/box.model';
import { User, UserRole, userHasRole } from '../../../core/models/user.model';
import { 
  ProjectConfiguration, 
  ProjectBuilding, 
  ProjectLevel, 
  ProjectBoxType, 
  ProjectZone, 
  ProjectBoxFunction 
} from '../../../core/models/project-configuration.model';
import { forkJoin } from 'rxjs';
import { toTitleCase, toUpperCase } from '../../../core/utils/text-transform.util';

@Component({
  selector: 'app-create-project',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
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
  loadingCategories = false;
  minStartDate: string = '';
  maxStartDate: string = '';
  
  locations = [
    { value: 1, label: 'KSA' },
    { value: 2, label: 'UAE' }
  ];

  projectManagers: Array<{ userId: string; fullName: string; email: string }> = [];
  loadingProjectManagers = false;
  isCurrentUserProjectManager = false;
  currentUserFullName = '';

  // Project Configuration
  buildings: ProjectBuilding[] = [];
  levels: ProjectLevel[] = [];
  boxTypes: ProjectBoxType[] = [];
  zones: ProjectZone[] = [];
  boxFunctions: ProjectBoxFunction[] = [];
  
  // Temp forms for adding new items
  newBuilding = '';
  newLevel = '';
  newBoxType = '';
  newBoxSubTypes: string[] = []; // Array to track subtype input for each box type
  selectedTypeForSubType = -1;
  newZone = '';
  newBoxFunction = '';

  constructor(
    private fb: FormBuilder,
    @Inject(ProjectService) private projectService: ProjectService,
    private boxService: BoxService,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.setDateLimits();
    this.initForm();
    this.loadProjectManagers();
    this.detectModeAndLoadProject();
  }

  private loadProjectManagers(): void {
    this.loadingProjectManagers = true;
    this.projectService.getProjectManagers().subscribe({
      next: (managers) => {
        this.projectManagers = managers;
        this.loadingProjectManagers = false;
        
        // Auto-select current user if they are a Project Manager and form is not in edit mode
        if (!this.isEdit) {
          this.autoSelectCurrentUserAsProjectManager();
        }
      },
      error: (err) => {
        console.error('Error loading project managers:', err);
        this.loadingProjectManagers = false;
      }
    });
  }

  private autoSelectCurrentUserAsProjectManager(): void {
    const currentUser = this.authService.getCurrentUser();
    
    // Check if user is logged in and has Project Manager role
    if (currentUser && userHasRole(currentUser, UserRole.ProjectManager)) {
      // Check if current user is in the project managers list
      const userInList = this.projectManagers.find(pm => pm.userId === currentUser.id);
      
      if (userInList) {
        // Set flags to show readonly input instead of dropdown
        this.isCurrentUserProjectManager = true;
        this.currentUserFullName = `${currentUser.firstName} ${currentUser.lastName} (${currentUser.email})`;
        
        // Set the current user as project manager in the form
        this.projectForm.patchValue({
          projectManager: currentUser.id
        });
        
        console.log('Current user is project manager, showing readonly field:', currentUser.id);
      }
    }
  }

  private setDateLimits(): void {
    const today = new Date();
    
    // Set minimum date to 1 month ago
    const oneMonthAgo = new Date();
    oneMonthAgo.setMonth(today.getMonth() - 1);
    this.minStartDate = this.formatDateForInput(oneMonthAgo);
    
    // Set maximum date to 5 years in the future (reasonable limit)
    const fiveYearsFromNow = new Date();
    fiveYearsFromNow.setFullYear(today.getFullYear() + 5);
    this.maxStartDate = this.formatDateForInput(fiveYearsFromNow);
  }

  private initForm(): void {
    this.projectForm = this.fb.group({
      projectName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      projectCode: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      clientName: ['', Validators.maxLength(200)],
      location: [null, Validators.required],
      duration: [null, [Validators.min(1)]],
      plannedStartDate: ['', Validators.required],
      projectedEndDate: [''],
      projectManager: [''],
      projectValue: [null, [Validators.min(0)]],
      description: ['', Validators.maxLength(500)],
      bimLink: ['', Validators.maxLength(500)]
    });
    
    // Add listeners for auto-calculation between duration and projectedEndDate
    this.setupDateCalculations();
  }
  
  private setupDateCalculations(): void {
    // When duration changes, calculate projectedEndDate
    this.projectForm.get('duration')?.valueChanges.subscribe(duration => {
      const startDate = this.projectForm.get('plannedStartDate')?.value;
      if (startDate && duration > 0) {
        const start = new Date(startDate);
        const endDate = new Date(start);
        endDate.setDate(start.getDate() + Number(duration));
        this.projectForm.get('projectedEndDate')?.setValue(
          this.formatDateForInput(endDate),
          { emitEvent: false }
        );
      }
    });
    
    // When projectedEndDate changes, calculate duration
    this.projectForm.get('projectedEndDate')?.valueChanges.subscribe(endDate => {
      const startDate = this.projectForm.get('plannedStartDate')?.value;
      if (startDate && endDate) {
        const start = new Date(startDate);
        const end = new Date(endDate);
        const duration = Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
        if (duration > 0) {
          this.projectForm.get('duration')?.setValue(duration, { emitEvent: false });
        }
      }
    });
    
    // When plannedStartDate changes, recalculate projectedEndDate
    this.projectForm.get('plannedStartDate')?.valueChanges.subscribe(startDate => {
      const duration = this.projectForm.get('duration')?.value;
      if (startDate && duration > 0) {
        const start = new Date(startDate);
        const endDate = new Date(start);
        endDate.setDate(start.getDate() + Number(duration));
        this.projectForm.get('projectedEndDate')?.setValue(
          this.formatDateForInput(endDate),
          { emitEvent: false }
        );
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
        this.loadProjectConfiguration(id);
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

  private loadProjectConfiguration(id: string): void {
    this.projectService.getProjectConfiguration(id).subscribe({
      next: (config) => {
        this.buildings = config.buildings || [];
        this.levels = config.levels || [];
        this.boxTypes = config.boxTypes || [];
        this.zones = config.zones || [];
        this.boxFunctions = config.boxFunctions || [];
        // Initialize subtype inputs array for loaded box types
        this.newBoxSubTypes = new Array(this.boxTypes.length).fill('');
      },
      error: (err) => {
        console.log('No existing configuration or error loading:', err);
        // It's okay if there's no configuration yet
      }
    });
  }

  private patchForm(project: Project): void {
    const plannedStart = project.plannedStartDate || project.startDate;
    const duration = this.getDurationValue(project);
    
    // Convert location string/number to numeric value for dropdown
    let locationValue: number | null = null;
    if (project.location) {
      // If location is already a number (string representation)
      const locationNum = Number(project.location);
      if (!isNaN(locationNum) && (locationNum === 1 || locationNum === 2)) {
        locationValue = locationNum;
      } else {
        // If location is a string like "KSA" or "UAE", convert to number
        const locationStr = project.location.toString().toUpperCase();
        if (locationStr === 'KSA' || locationStr === '1') {
          locationValue = 1;
        } else if (locationStr === 'UAE' || locationStr === '2') {
          locationValue = 2;
        }
      }
    }

    this.projectForm.patchValue({
      projectName: project.name || '',
      projectCode: project.code || '',
      clientName: project.clientName || '',
      location: locationValue,
      duration: duration,
      plannedStartDate: this.formatDateForInput(plannedStart),
      projectedEndDate: this.formatDateForInput(project.projectedEndDate),
      projectManager: project.projectManagerId || '',
      projectValue: project.projectValue || null,
      description: project.description || '',
      bimLink: project.bimLink || ''
    });

    // Disable projectCode and location fields in edit mode (they cannot be updated)
    if (this.isEdit) {
      this.projectForm.get('projectCode')?.disable();
      this.projectForm.get('location')?.disable();
    }
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
      
      // Show validation errors in toast
      const errors: string[] = [];
      Object.keys(this.projectForm.controls).forEach(key => {
        const control = this.projectForm.get(key);
        if (control?.invalid && (control.dirty || control.touched)) {
          errors.push(this.getFieldError(key));
        }
      });
      
      if (errors.length > 0) {
        this.toastService.error(errors[0]); // Show first error
      }
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
        duration: formValue.duration || undefined,
        plannedStartDate: formValue.plannedStartDate ? new Date(formValue.plannedStartDate).toISOString() : undefined,
        projectedEndDate: formValue.projectedEndDate ? new Date(formValue.projectedEndDate).toISOString() : undefined,
        projectMangerId: formValue.projectManager || undefined,
        projectValue: formValue.projectValue || undefined,
        description: formValue.description || undefined,
        bimLink: formValue.bimLink || undefined
      };
    } else {
      projectData = {};
      const compare = (newVal: any, oldVal: any) => newVal !== oldVal && !(newVal === undefined && oldVal === undefined);

      if (this.originalProject) {
        // Note: projectCode and location cannot be updated - they are read-only in edit mode
        // if (compare(formValue.projectCode, this.originalProject.code)) {
        //   projectData.projectCode = formValue.projectCode;
        // }
        if (compare(formValue.projectName, this.originalProject.name)) {
          projectData.projectName = formValue.projectName;
        }
        if (compare(formValue.clientName, this.originalProject.clientName)) {
          projectData.clientName = formValue.clientName || undefined;
        }
        // Note: location cannot be updated - it is read-only in edit mode
        // if (compare(formValue.location, this.originalProject.location)) {
        //   projectData.location = formValue.location || 1;
        // }
       
        if (compare(formValue.description, this.originalProject.description)) {
          projectData.description = formValue.description || undefined;
        }
        if (compare(formValue.bimLink, this.originalProject.bimLink)) {
          projectData.bimLink = formValue.bimLink || undefined;
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
        if (formValue.projectedEndDate) {
          const originalProjectedEnd = this.originalProject.projectedEndDate ? this.formatDateForInput(this.originalProject.projectedEndDate) : '';
          if (compare(this.formatDateForInput(formValue.projectedEndDate ? new Date(formValue.projectedEndDate) : undefined), originalProjectedEnd)) {
            projectData.projectedEndDate = new Date(formValue.projectedEndDate).toISOString();
          }
        }
        if (formValue.projectManager) {
          if (compare(formValue.projectManager, this.originalProject.projectManagerId)) {
            projectData.projectMangerId = formValue.projectManager;
          }
        }
        if (formValue.projectValue !== null && formValue.projectValue !== undefined) {
          if (compare(formValue.projectValue, this.originalProject.projectValue)) {
            projectData.projectValue = formValue.projectValue;
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
        const projectId = project.id || project.projectId || project.ProjectId || this.projectId;

        if (!projectId) {
          console.error('⚠️ WARNING: Saved project has no ID!');
          console.error('📦 Full project object:', JSON.stringify(project, null, 2));
          this.loading = false;
          this.error = 'Project saved but has no ID. Please contact support.';
          return;
        }

        // Save configuration if any configuration items exist
        if (this.hasConfiguration()) {
          this.saveConfiguration(projectId);
        } else {
          this.loading = false;
          this.successMessage = this.isEdit ? 'Project updated successfully!' : 'Project created successfully!';
          console.log('✅ Project saved:', project);
          
          setTimeout(() => {
            this.router.navigate(['/projects', projectId, 'dashboard']);
          }, 1200);
        }
      },
      error: (err) => {
        this.loading = false;
        const errorMessage = err.error?.message || err.message || `Failed to ${this.isEdit ? 'update' : 'create'} project. Please try again.`;
        this.error = errorMessage;
        this.toastService.error(errorMessage);
        console.error('❌ Error saving project:', err);
      }
    });
  }

  private hasConfiguration(): boolean {
    return this.buildings.length > 0 || 
           this.levels.length > 0 || 
           this.boxTypes.length > 0 || 
           this.zones.length > 0 || 
           this.boxFunctions.length > 0;
  }

  private saveConfiguration(projectId: string): void {
    const configuration: ProjectConfiguration = {
      projectId: projectId,
      buildings: this.buildings,
      levels: this.levels,
      boxTypes: this.boxTypes,
      zones: this.zones,
      boxFunctions: this.boxFunctions
    };

    this.projectService.saveProjectConfiguration(projectId, configuration).subscribe({
      next: () => {
        this.loading = false;
        this.successMessage = this.isEdit ? 'Project and configuration updated successfully!' : 'Project and configuration created successfully!';
        console.log('✅ Project and configuration saved');
        
        setTimeout(() => {
          this.router.navigate(['/projects', projectId, 'dashboard']);
        }, 1200);
      },
      error: (err) => {
        this.loading = false;
        const errorMessage = 'Project saved but failed to save configuration: ' + (err.error?.message || err.message);
        this.error = errorMessage;
        this.toastService.error(errorMessage);
        console.error('❌ Error saving configuration:', err);
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
      projectedEndDate: 'Projected end date',
      projectManager: 'Project manager',
      projectValue: 'Project value',
      description: 'Description',
      bimLink: 'BIM Link'
    };
    return labels[fieldName] || fieldName;
  }

  // Configuration Management Methods

  // Building methods
  onBuildingInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value;
    
    // Check if there's at least one comma
    if (value.includes(',')) {
      // Split by comma
      const parts = value.split(',');
      
      // Add all parts except the last one (the incomplete value) to the list
      for (let i = 0; i < parts.length - 1; i++) {
        const item = toUpperCase(parts[i]);
        if (item) {
          const exists = this.buildings.some(b => b.buildingCode.toLowerCase() === item.toLowerCase());
          if (!exists) {
            this.buildings.push({
              buildingCode: item,
              buildingName: item
            });
          } else {
            // Show toast error
            this.toastService.error(`Building "${item}" already exists`);
          }
        }
      }
      
      // Clear the input after comma
      this.newBuilding = '';
      
      // Set cursor position at the end
      setTimeout(() => {
        input.setSelectionRange(0, 0);
      }, 0);
    }
  }

  addBuilding(): void {
    if (this.newBuilding.trim()) {
      // Split by comma and add each item
      const items = this.newBuilding.split(',').map(item => toUpperCase(item)).filter(item => item);
      let duplicateFound = false;
      items.forEach(item => {
        // Check if building already exists
        const exists = this.buildings.some(b => b.buildingCode.toLowerCase() === item.toLowerCase());
        if (!exists) {
      this.buildings.push({
            buildingCode: item,
            buildingName: item
          });
        } else {
          duplicateFound = true;
          // Show toast error
          this.toastService.error(`Building "${item}" already exists`);
        }
      });
      // Clear input after adding if no duplicates
      if (!duplicateFound) {
        this.newBuilding = '';
      }
    }
  }

  removeBuilding(index: number): void {
    this.buildings.splice(index, 1);
  }

  // Level methods
  onLevelInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    
    // Check if there's at least one comma
    if (value.includes(',')) {
      // Split by comma
      const parts = value.split(',');
      
      // Add all parts except the last one (the incomplete value) to the list
      for (let i = 0; i < parts.length - 1; i++) {
        const item = toUpperCase(parts[i].trim());
        if (item) {
          const exists = this.levels.some(l => l.levelCode.toUpperCase() === item);
          if (!exists) {
            this.levels.push({
              levelCode: item,
              levelName: item
            });
          } else {
            // Show toast error
            this.toastService.error(`Level "${item}" already exists`);
          }
        }
      }
      
      // Clear the input after comma
      this.newLevel = '';
      
      // Set cursor position at the end
      setTimeout(() => {
        input.setSelectionRange(0, 0);
      }, 0);
    }
  }

  addLevel(): void {
    if (this.newLevel.trim()) {
      // Split by comma and add each item
      const items = this.newLevel.split(',').map(item => toUpperCase(item.trim())).filter(item => item);
      let duplicateFound = false;
      items.forEach(item => {
        // Check if level already exists
        const exists = this.levels.some(l => l.levelCode.toLowerCase() === item.toLowerCase());
        if (!exists) {
      this.levels.push({
            levelCode: item,
            levelName: item
          });
        } else {
          duplicateFound = true;
          // Show toast error
          this.toastService.error(`Level "${item}" already exists`);
        }
      });
      // Clear input after adding if no duplicates
      if (!duplicateFound) {
        this.newLevel = '';
      }
    }
  }

  removeLevel(index: number): void {
    this.levels.splice(index, 1);
  }

  // Box Type methods
  onBoxTypeInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value;
    
    // Check if there's at least one comma
    if (value.includes(',')) {
      // Split by comma
      const parts = value.split(',');
      
      // Add all parts except the last one (the incomplete value) to the list
      for (let i = 0; i < parts.length - 1; i++) {
        const item = toUpperCase(parts[i]);
        if (item) {
          const exists = this.boxTypes.some(t => t.typeName.toLowerCase() === item.toLowerCase());
          if (!exists) {
            this.boxTypes.push({
              typeName: item,
              hasSubTypes: false,
              subTypes: []
            });
          } else {
            // Show toast error
            this.toastService.error(`Box Type "${item}" already exists`);
          }
        }
      }
      
      // Clear the input after comma
      this.newBoxType = '';
      
      // Set cursor position at the end
      setTimeout(() => {
        input.setSelectionRange(0, 0);
      }, 0);
    }
  }

  addBoxType(): void {
    if (this.newBoxType.trim()) {
      // Split by comma and add each item
      const items = this.newBoxType.split(',').map(item => toUpperCase(item)).filter(item => item);
      let duplicateFound = false;
      items.forEach(item => {
        // Check if box type already exists
        const exists = this.boxTypes.some(t => t.typeName.toLowerCase() === item.toLowerCase());
        if (!exists) {
      this.boxTypes.push({
            typeName: item,
        hasSubTypes: false,
        subTypes: []
          });
          // Initialize empty subtype input for this new box type
          this.newBoxSubTypes.push('');
        } else {
          duplicateFound = true;
          // Show toast error
          this.toastService.error(`Box Type "${item}" already exists`);
        }
      });
      // Clear input after adding if no duplicates
      if (!duplicateFound) {
        this.newBoxType = '';
      }
    }
  }

  removeBoxType(index: number): void {
    this.boxTypes.splice(index, 1);
    // Also remove the corresponding subtype input
    this.newBoxSubTypes.splice(index, 1);
  }

  toggleHasSubTypes(index: number): void {
    const type = this.boxTypes[index];
    type.hasSubTypes = !type.hasSubTypes;
    if (!type.hasSubTypes) {
      type.subTypes = [];
    }
  }

  // Box SubType methods
  onSubTypeInput(event: Event, typeIndex: number): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    
    // Ensure the array has enough elements
    while (this.newBoxSubTypes.length <= typeIndex) {
      this.newBoxSubTypes.push('');
    }
    
    // Check if there's at least one comma
    if (value.includes(',')) {
      // Split by comma
      const parts = value.split(',');
      
      // Initialize subTypes array if needed
      if (!this.boxTypes[typeIndex].subTypes) {
        this.boxTypes[typeIndex].subTypes = [];
      }
      
      // Add all parts except the last one (the incomplete value) to the list - convert to uppercase
      for (let i = 0; i < parts.length - 1; i++) {
        const item = toUpperCase(parts[i]);
        if (item) {
          const exists = this.boxTypes[typeIndex].subTypes?.some(s => s.subTypeName.toLowerCase() === item.toLowerCase());
          if (!exists) {
            this.boxTypes[typeIndex].subTypes!.push({
              subTypeName: item
            });
          } else {
            // Show toast error
            this.toastService.error(`Sub Type "${item}" already exists for ${this.boxTypes[typeIndex].typeName}`);
          }
        }
      }
      
      // Clear the input after comma
      this.newBoxSubTypes[typeIndex] = '';
      this.selectedTypeForSubType = typeIndex;
      
      // Set cursor position at the end
      setTimeout(() => {
        input.setSelectionRange(0, 0);
      }, 0);
    }
  }

  addSubType(typeIndex: number): void {
    // Ensure the array has enough elements
    while (this.newBoxSubTypes.length <= typeIndex) {
      this.newBoxSubTypes.push('');
    }
    
    if (this.newBoxSubTypes[typeIndex]?.trim()) {
      if (!this.boxTypes[typeIndex].subTypes) {
        this.boxTypes[typeIndex].subTypes = [];
      }
      // Split by comma and add each item - convert to uppercase
      const items = this.newBoxSubTypes[typeIndex].split(',').map(item => toUpperCase(item)).filter(item => item);
      let duplicateFound = false;
      items.forEach(item => {
        // Check if subtype already exists
        const exists = this.boxTypes[typeIndex].subTypes?.some(s => s.subTypeName.toLowerCase() === item.toLowerCase());
        if (!exists) {
      this.boxTypes[typeIndex].subTypes!.push({
            subTypeName: item
          });
        } else {
          duplicateFound = true;
          // Show toast error
          this.toastService.error(`Sub Type "${item}" already exists for ${this.boxTypes[typeIndex].typeName}`);
        }
      });
      // Clear input after adding if no duplicates
      if (!duplicateFound) {
        this.newBoxSubTypes[typeIndex] = '';
      }
    }
  }

  removeSubType(typeIndex: number, subTypeIndex: number): void {
    this.boxTypes[typeIndex].subTypes?.splice(subTypeIndex, 1);
  }

  // Zone methods
  onZoneInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value;
    
    // Check if there's at least one comma
    if (value.includes(',')) {
      // Split by comma
      const parts = value.split(',');
      
      // Add all parts except the last one (the incomplete value) to the list
      for (let i = 0; i < parts.length - 1; i++) {
        const item = toTitleCase(parts[i]);
        if (item) {
          const exists = this.zones.some(z => z.zoneCode.toLowerCase() === item.toLowerCase());
          if (!exists) {
            this.zones.push({
              zoneCode: item,
              zoneName: item
            });
          } else {
            // Show toast error
            this.toastService.error(`Zone "${item}" already exists`);
          }
        }
      }
      
      // Clear the input after comma
      this.newZone = '';
      
      // Set cursor position at the end
      setTimeout(() => {
        input.setSelectionRange(0, 0);
      }, 0);
    }
  }

  addZone(): void {
    if (this.newZone.trim()) {
      // Split by comma and add each item
      const items = this.newZone.split(',').map(item => toTitleCase(item)).filter(item => item);
      let duplicateFound = false;
      items.forEach(item => {
        // Check if zone already exists
        const exists = this.zones.some(z => z.zoneCode.toLowerCase() === item.toLowerCase());
        if (!exists) {
      this.zones.push({
            zoneCode: item,
            zoneName: item
          });
        } else {
          duplicateFound = true;
          // Show toast error
          this.toastService.error(`Zone "${item}" already exists`);
        }
      });
      // Clear input after adding if no duplicates
      if (!duplicateFound) {
        this.newZone = '';
      }
    }
  }

  removeZone(index: number): void {
    this.zones.splice(index, 1);
  }

  // Box Function methods
  onBoxFunctionInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    let value = input.value;
    
    // Check if there's at least one comma
    if (value.includes(',')) {
      // Split by comma
      const parts = value.split(',');
      
      // Add all parts except the last one (the incomplete value) to the list
      for (let i = 0; i < parts.length - 1; i++) {
        const item = toTitleCase(parts[i]);
        if (item) {
          const exists = this.boxFunctions.some(f => f.functionName.toLowerCase() === item.toLowerCase());
          if (!exists) {
            this.boxFunctions.push({
              functionName: item
            });
          } else {
            // Show toast error
            this.toastService.error(`Box Function "${item}" already exists`);
          }
        }
      }
      
      // Clear the input after comma
      this.newBoxFunction = '';
      
      // Set cursor position at the end
      setTimeout(() => {
        input.setSelectionRange(0, 0);
      }, 0);
    }
  }

  addBoxFunction(): void {
    if (this.newBoxFunction.trim()) {
      // Split by comma and add each item
      const items = this.newBoxFunction.split(',').map(item => toTitleCase(item)).filter(item => item);
      let duplicateFound = false;
      items.forEach(item => {
        // Check if function already exists
        const exists = this.boxFunctions.some(f => f.functionName.toLowerCase() === item.toLowerCase());
        if (!exists) {
      this.boxFunctions.push({
            functionName: item
          });
        } else {
          duplicateFound = true;
          // Show toast error
          this.toastService.error(`Box Function "${item}" already exists`);
        }
      });
      // Clear input after adding if no duplicates
      if (!duplicateFound) {
        this.newBoxFunction = '';
      }
    }
  }

  removeBoxFunction(index: number): void {
    this.boxFunctions.splice(index, 1);
  }
  getLocationLabel(value: number | string): string {
    return this.locations.find(l => l.value === value)?.label ?? '';
  }

  // Text Capitalization Methods
  
  /**
   * Capitalizes project name on blur
   */
  capitalizeProjectName(): void {
    const projectName = this.projectForm.get('projectName')?.value;
    if (projectName) {
      this.projectForm.patchValue({
        projectName: toTitleCase(projectName)
      });
    }
  }

  /**
   * Capitalizes client name on blur
   */
  capitalizeClientName(): void {
    const clientName = this.projectForm.get('clientName')?.value;
    if (clientName) {
      this.projectForm.patchValue({
        clientName: toTitleCase(clientName)
      });
    }
  }

  /**
   * Converts building input to uppercase on blur (used for comma-separated input)
   */
  capitalizeBuildingInput(): void {
    if (this.newBuilding) {
      this.newBuilding = toUpperCase(this.newBuilding);
    }
  }

  /**
   * Converts level input to uppercase on blur (used for comma-separated input)
   */
  capitalizeLevelInput(): void {
    if (this.newLevel) {
      this.newLevel = toUpperCase(this.newLevel);
    }
  }

  /**
   * Converts box type input to uppercase on blur (used for comma-separated input)
   */
  capitalizeBoxTypeInput(): void {
    if (this.newBoxType) {
      this.newBoxType = toUpperCase(this.newBoxType);
    }
  }

  /**
   * Converts zone input to title case on blur (used for comma-separated input)
   */
  capitalizeZoneInput(): void {
    if (this.newZone) {
      this.newZone = toTitleCase(this.newZone);
    }
  }

  /**
   * Converts box function input to title case on blur (used for comma-separated input)
   */
  capitalizeBoxFunctionInput(): void {
    if (this.newBoxFunction) {
      this.newBoxFunction = toTitleCase(this.newBoxFunction);
    }
  }

  /**
   * Converts subtype input to uppercase on blur
   */
  capitalizeSubTypeInput(typeIndex: number): void {
    if (this.newBoxSubTypes[typeIndex]) {
      this.newBoxSubTypes[typeIndex] = toUpperCase(this.newBoxSubTypes[typeIndex]);
    }
  }
  
}

