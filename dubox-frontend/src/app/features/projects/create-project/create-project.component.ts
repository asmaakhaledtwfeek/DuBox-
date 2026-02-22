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
import { environment } from '../../../../environments/environment';
import { BoxMaterialService } from '../../../core/services/box-material.service';
import { ProjectMaterialSelectionComponent } from '../../materials/components/project-material-selection/project-material-selection.component';
import { ConfirmationDialogComponent } from '../../../shared/components/confirmation-dialog/confirmation-dialog.component';
import { MaterialTemplateService } from '../../../core/services/material-template.service';
import { MaterialTemplate, BoxTypeMaterialTemplate, ProjectMaterialTemplate } from '../../../core/models/material-template.model';
import { AssignTemplateModalComponent, TemplateAssignment } from '../../../shared/components/assign-template-modal/assign-template-modal.component';
import { ActivityTemplateService } from '../../../core/services/activity-template.service';
import { ActivityTemplate } from '../../../core/models/activity-template.model';

@Component({
  selector: 'app-create-project',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent, ProjectMaterialSelectionComponent, ConfirmationDialogComponent, AssignTemplateModalComponent],
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
  
  // Step management for create flow (1: basic info, 2: logos, 3: materials)
  currentStep: number = 1;
  createdProjectId: string | null = null;
  
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
  
  // Collapse states
  isProjectConfigCollapsed = true; // Collapsed by default
  isBoxTypesCollapsed = true; // Collapsed by default
  isActivityTemplatesCollapsed = true; // Collapsed by default
  isMaterialTemplatesCollapsed = true; // Collapsed by default
  isProjectLogosCollapsed = true; // Collapsed by default
  
  // Temp forms for adding new items
  newBuilding = '';
  newLevel = '';
  newBoxType = '';
  newBoxSubTypes: string[] = []; // Array to track subtype input for each box type
  selectedTypeForSubType = -1;
  newZone = '';
  newBoxFunction = '';

  // Logo uploads
  contractorImage: File | null = null;
  contractorImagePreview: string | null = null;
  contractorImageUrl: string | null = null; // Existing logo URL from server
  subContractorImage: File | null = null;
  subContractorImagePreview: string | null = null;
  subContractorImageUrl: string | null = null; // Existing logo URL from server
  clientImage: File | null = null;
  clientImagePreview: string | null = null;
  clientImageUrl: string | null = null; // Existing logo URL from server
  
  // Material Lead Time Warning Dialog
  showMaterialWarningDialog = false;
  materialWarningDialogTitle = '';
  materialWarningDialogMessage = '';

  // Material Templates
  availableTemplates: MaterialTemplate[] = [];
  projectTemplates: MaterialTemplate[] = [];
  boxTypeTemplates: Map<number, MaterialTemplate> = new Map();
  loadingTemplates = false;
  showTemplateModal = false;
  templateAssignment: TemplateAssignment | null = null;

  // Pending material template (selected in UI but not yet saved to DB)
  pendingMaterialTemplateId: string | null = null;
  /** The template ID currently persisted in the database for this project */
  originalMaterialTemplateId: string | null = null;

  // Activity Templates
  availableActivityTemplates: any[] = [];
  selectedProjectActivityTemplateId: string | null = null;
  /** The activity template ID currently persisted in the database for this project */
  originalActivityTemplateId: string | null = null;
  boxTypeActivityTemplates: Map<number, string> = new Map(); // Map boxTypeId to activityTemplateId
  loadingActivityTemplates = false;

  constructor(
    private fb: FormBuilder,
    @Inject(ProjectService) private projectService: ProjectService,
    private boxService: BoxService,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router,
    private route: ActivatedRoute,
    private boxMaterialService: BoxMaterialService,
    private materialTemplateService: MaterialTemplateService,
    private activityTemplateService: ActivityTemplateService
  ) {}

  ngOnInit(): void {
    this.setDateLimits();
    this.initForm();
    this.loadProjectManagers();
    this.detectModeAndLoadProject();
    
    // Check if we're in step 2 or 3 (from query param or state)
    const step = this.route.snapshot.queryParamMap.get('step');
    const projectId = this.route.snapshot.queryParamMap.get('projectId');
    if (step && projectId && !this.isEdit) {
      this.currentStep = parseInt(step, 10);
      this.createdProjectId = projectId;
      
      // Load templates and configuration if in step 3
      if (this.currentStep === 3) {
        this.loadProjectConfiguration(projectId); // Load box types for template assignment
        this.loadMaterialTemplates();
        this.loadActivityTemplates();
      }
    }
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
      bimLink: ['', Validators.maxLength(500)],
      allowCompletionWithConditionalApproval: [false],
      activityTemplateId: [null]
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
      
      // Check material lead time warning
      if (startDate) {
        this.checkMaterialLeadTimeWarning(startDate);
      }
    });
  }
  
  private checkMaterialLeadTimeWarning(plannedStartDate: string): void {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    const plannedStart = new Date(plannedStartDate);
    plannedStart.setHours(0, 0, 0, 0);
    
    // Calculate days difference
    const daysDifference = Math.ceil((plannedStart.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
    
    // Show warning if less than 7 days
    if (daysDifference < 7 && daysDifference >= 0) {
      const daysText = daysDifference === 1 ? '1 day' : `${daysDifference} days`;
      this.materialWarningDialogTitle = 'Material Lead Time Warning';
      this.materialWarningDialogMessage = `The planned start date is only <strong>${daysText}</strong> away. Please note that materials typically need at least <strong>7 days</strong> to arrive. Some materials may require up to <strong>1 month</strong> lead time.<br><br>This may affect your project timeline and material readiness.`;
      this.showMaterialWarningDialog = true;
    } else if (daysDifference < 0) {
      this.materialWarningDialogTitle = 'Past Date Warning';
      this.materialWarningDialogMessage = `The planned start date is in the past. Materials typically need at least <strong>7 days</strong> to arrive from today. Please consider selecting a future date that allows adequate time for material procurement.`;
      this.showMaterialWarningDialog = true;
    }
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
        this.loadProjectImages(project);
        
        // Load configuration first, then load templates after configuration is loaded
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

  private loadProjectImages(project: Project): void {
    // Load existing logo URLs from the project
    // Handle empty strings as null and ensure URLs are absolute
    this.contractorImageUrl = this.normalizeImageUrl(project.contractorImageUrl);
    this.subContractorImageUrl = this.normalizeImageUrl(project.subContractorImageUrl);
    this.clientImageUrl = this.normalizeImageUrl(project.clientImageUrl);
    
    console.log('🖼️ Loaded project logos:', {
      contractorImageUrl: this.contractorImageUrl,
      subContractorImageUrl: this.subContractorImageUrl,
      clientImageUrl: this.clientImageUrl,
      originalProject: {
        contractorImageUrl: project.contractorImageUrl,
        subContractorImageUrl: project.subContractorImageUrl,
        clientImageUrl: project.clientImageUrl
      }
    });
  }

  /**
   * Format logo URL - same logic as progress updates
   * Handles relative URLs, absolute URLs, and empty values
   * Matches the pattern used in box-details.component.ts for progress update images
   */
  private normalizeImageUrl(url: string | null | undefined): string | null {
    if (!url || url.trim() === '') {
      return null;
    }

    const trimmedUrl = url.trim();
    
    // If it's a relative URL starting with /api/ or just /, convert to absolute URL
    // Use window.location.origin (not environment.apiUrl) to avoid double /api/
    if (trimmedUrl.startsWith('/api/') || (trimmedUrl.startsWith('/') && !trimmedUrl.startsWith('http'))) {
      const baseUrl = `${window.location.protocol}//${window.location.host}`;
      const fullUrl = `${baseUrl}${trimmedUrl}`;
      console.log('🔗 Formatting relative URL:', trimmedUrl, '→', fullUrl);
      return fullUrl;
    }
    
    // If it's already an absolute URL (starts with http:// or https://), return as-is
    if (trimmedUrl.startsWith('http://') || trimmedUrl.startsWith('https://')) {
      console.log('🔗 Using absolute URL:', trimmedUrl);
      return trimmedUrl;
    }
    
    // For any other URL format, return as is (might be a blob URL or other valid URL)
    console.log('🔗 Using URL as-is:', trimmedUrl);
    return trimmedUrl;
  }

  /**
   * Handle logo load error
   */
  onImageError(imageType: 'contractor' | 'subContractor' | 'client'): void {
    console.error(`❌ Failed to load ${imageType} logo`);
    // Clear the failed logo URL
    if (imageType === 'contractor') {
      this.contractorImageUrl = null;
    } else if (imageType === 'subContractor') {
      this.subContractorImageUrl = null;
    } else if (imageType === 'client') {
      this.clientImageUrl = null;
    }
    this.toastService.error(`Failed to load ${imageType} logo. The logo may have been deleted or moved.`);
  }

  /**
   * Trigger file input click
   */
  triggerFileInput(inputId: string): void {
    const input = document.getElementById(inputId) as HTMLInputElement;
    if (input) {
      input.click();
    }
  }

  private loadProjectConfiguration(id: string): void {
    console.log('🔄 Loading project configuration for project:', id);
    
    this.projectService.getProjectConfiguration(id).subscribe({
      next: (config) => {
        this.buildings = config.buildings || [];
        this.levels = config.levels || [];
        this.boxTypes = config.boxTypes || [];
        this.zones = config.zones || [];
        this.boxFunctions = config.boxFunctions || [];
        // Initialize subtype inputs array for loaded box types
        this.newBoxSubTypes = new Array(this.boxTypes.length).fill('');
        
        console.log('✅ Project configuration loaded successfully:', {
          projectId: id,
          buildings: this.buildings.length,
          levels: this.levels.length,
          boxTypes: this.boxTypes.length,
          zones: this.zones.length,
          boxFunctions: this.boxFunctions.length,
          boxTypesData: this.boxTypes.map(bt => ({
            id: bt.id,
            typeName: bt.typeName,
            activityTemplateId: bt.activityTemplateId
          }))
        });
        
        // Load templates in edit mode
        if (this.isEdit) {
          console.log('📋 Loading templates after configuration...');
          this.loadMaterialTemplates();
          this.loadActivityTemplates();
        }
      },
      error: (err) => {
        console.log('⚠️ No existing configuration or error loading:', err);
        // It's okay if there's no configuration yet
        
        // Still load templates in edit mode even if config fails
        if (this.isEdit) {
          this.loadMaterialTemplates();
          this.loadActivityTemplates();
        }
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
      bimLink: project.bimLink || '',
      allowCompletionWithConditionalApproval: project.allowCompletionWithConditionalApproval ?? false
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
    // Handle step 2 (image upload)
    if (this.currentStep === 2 && !this.isEdit) {
      this.uploadImagesStep2();
      return;
    }

    // Handle step 3 (material selection - optional)
    if (this.currentStep === 3 && !this.isEdit) {
      // Save template assignments (deferred from selection time), then navigate
      if (!this.createdProjectId) {
        this.router.navigate(['/projects']);
        return;
      }
      this.loading = true;
      this.saveAllTemplateAssignments(this.createdProjectId, () => {
        this.loading = false;
        this.router.navigate(['/projects', this.createdProjectId, 'dashboard']);
      });
      return;
    }

    // Step 1: Validate and create project
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
      // Step 1: Create project (without images)
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
        bimLink: formValue.bimLink || undefined,
        allowCompletionWithConditionalApproval: formValue.allowCompletionWithConditionalApproval ?? false
      };
      
      // Create project and move to step 2
      this.projectService.createProject(projectData).subscribe({
        next: (project: any) => {
          const projectId = project.id || project.projectId || project.ProjectId;

          if (!projectId) {
            console.error('⚠️ WARNING: Created project has no ID!');
            this.loading = false;
            this.error = 'Project created but has no ID. Please contact support.';
            return;
          }

          // Save configuration if any exists
          if (this.hasConfiguration()) {
            this.saveConfigurationAndMoveToStep2(projectId);
          } else {
            // Move to step 2
            this.loading = false;
            this.createdProjectId = projectId;
            this.currentStep = 2;
            this.successMessage = 'Project created successfully! Now you can optionally add images.';
            this.router.navigate([], {
              relativeTo: this.route,
              queryParams: { step: '2', projectId: projectId },
              queryParamsHandling: 'merge'
            });
          }
        },
        error: (err) => {
          this.loading = false;
          const errorMessage = err.error?.message || err.message || 'Failed to create project. Please try again.';
          this.error = errorMessage;
          this.toastService.error(errorMessage);
          console.error('❌ Error creating project:', err);
        }
      });
    } else {
      // Edit mode - keep existing behavior
      projectData = {};
      const compare = (newVal: any, oldVal: any) => newVal !== oldVal && !(newVal === undefined && oldVal === undefined);

      if (this.originalProject) {
        if (compare(formValue.projectName, this.originalProject.name)) {
          projectData.projectName = formValue.projectName;
        }
        if (compare(formValue.clientName, this.originalProject.clientName)) {
          projectData.clientName = formValue.clientName || undefined;
        }
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
        // Check if allowCompletionWithConditionalApproval has changed
        if (compare(formValue.allowCompletionWithConditionalApproval, this.originalProject.allowCompletionWithConditionalApproval)) {
          projectData.allowCompletionWithConditionalApproval = formValue.allowCompletionWithConditionalApproval ?? false;
        }
      }

      if (this.projectId) {
        projectData.projectId = this.projectId;
      }

      // Include material template change in the same request so only ONE PUT /projects/{id}
      // is sent. UpdateProjectCommandHandler propagates the change to box types internally.
      const materialChangedInEdit = this.pendingMaterialTemplateId !== this.originalMaterialTemplateId;
      if (materialChangedInEdit && this.pendingMaterialTemplateId) {
        (projectData as any).materialTemplateId = this.pendingMaterialTemplateId;
      }

      // Edit mode - upload images if any are selected
      this.projectService.updateProject(this.projectId!, projectData).subscribe({
        next: (project: any) => {
          const projectId = project.id || project.projectId || project.ProjectId || this.projectId;

          if (!projectId) {
            console.error('⚠️ WARNING: Updated project has no ID!');
            this.loading = false;
            this.error = 'Project updated but has no ID. Please contact support.';
            return;
          }

          // Material template was already sent in the request above — mark it as synced so
          // saveAllTemplateAssignments does not fire a redundant second updateProject call.
          if (materialChangedInEdit && this.pendingMaterialTemplateId) {
            this.originalMaterialTemplateId = this.pendingMaterialTemplateId;
          }

          // Save template assignments (deferred from selection), then handle images/config/navigate
          this.saveAllTemplateAssignments(projectId, () => {
            if (this.hasImagesToUpload()) {
              this.uploadImages(projectId);
            } else if (this.hasConfiguration()) {
              this.saveConfiguration(projectId);
            } else {
              this.loading = false;
              this.successMessage = 'Project updated successfully!';
              console.log('✅ Project updated:', project);
              
              setTimeout(() => {
                this.router.navigate(['/projects', projectId, 'dashboard']);
              }, 1200);
            }
          });
        },
        error: (err) => {
          this.loading = false;
          const errorMessage = err.error?.message || err.message || 'Failed to update project. Please try again.';
          this.error = errorMessage;
          this.toastService.error(errorMessage);
          console.error('❌ Error updating project:', err);
      }
    });
  }
  }
  /**
   * Load available activity templates
   */
  loadActivityTemplates(): void {
    this.loadingActivityTemplates = true;
    this.activityTemplateService.getAllTemplates(true).subscribe({
      next: (templates: ActivityTemplate[]) => {
        this.availableActivityTemplates = templates.filter((t: ActivityTemplate) => t.isActive);
        this.loadingActivityTemplates = false;
        
        // Load existing assignments if in edit mode or step 3
        const projectId = this.isEdit ? this.projectId : this.createdProjectId;
        if (projectId) {
          this.loadProjectActivityTemplateAssignments(projectId);
        }
      },
      error: (error) => {
        console.error('Error loading activity templates:', error);
        this.toastService.error('Failed to load activity templates');
        this.loadingActivityTemplates = false;
      }
    });
  }

  /**
   * Load existing activity template assignments for the project
   */
  loadProjectActivityTemplateAssignments(projectId: string): void {
    console.log('🔄 Loading activity template assignments for project:', projectId);
    
    // Load project-level activity template
    this.projectService.getProject(projectId).subscribe({
      next: (project: Project) => {
        this.selectedProjectActivityTemplateId = project.activityTemplateId || null;
        // Store original so we can detect changes on save
        this.originalActivityTemplateId = this.selectedProjectActivityTemplateId;
        
        console.log('✅ Project-level activity template loaded:', {
          projectId: projectId,
          activityTemplateId: project.activityTemplateId,
          activityTemplateName: project.activityTemplateName
        });
        
        // Update form value
        this.projectForm.patchValue({
          activityTemplateId: project.activityTemplateId
        }, { emitEvent: false });
      },
      error: (error: any) => {
        console.error('❌ Error loading project activity template:', error);
      }
    });

    // Load box-type-level activity templates from already-loaded boxTypes
    // (no need to make another API call since loadProjectConfiguration already loaded them)
    this.boxTypeActivityTemplates.clear();
    
    console.log('📦 Processing box types for activity templates:', {
      projectId: projectId,
      boxTypesCount: this.boxTypes?.length || 0,
      boxTypes: this.boxTypes
    });
    
    if (this.boxTypes && this.boxTypes.length > 0) {
      this.boxTypes.forEach((boxType: ProjectBoxType) => {
        console.log('🔍 Processing box type:', {
          id: boxType.id,
          typeName: boxType.typeName,
          activityTemplateId: boxType.activityTemplateId
        });
        
        if (boxType.activityTemplateId && boxType.id !== undefined) {
          this.boxTypeActivityTemplates.set(boxType.id!, boxType.activityTemplateId);
          console.log('✅ Box type activity template assigned:', {
            boxTypeId: boxType.id,
            boxTypeName: boxType.typeName,
            activityTemplateId: boxType.activityTemplateId
          });
        }
      });
      
      console.log('📊 Final boxTypeActivityTemplates Map:', {
        size: this.boxTypeActivityTemplates.size,
        entries: Array.from(this.boxTypeActivityTemplates.entries())
      });
    } else {
      console.warn('⚠️ No box types loaded yet. Will load when configuration is available.');
    }
  }

  /**
   * Handle project-level activity template selection.
   * The selection is stored locally and only saved to the database when the user
   * clicks "Complete & Go to Dashboard" (create flow) or "Save Changes" (edit mode).
   */
  onProjectActivityTemplateChange(templateId: string | null): void {
    this.selectedProjectActivityTemplateId = templateId;
  }

  /**
   * Called when the project-material-selection child component emits a selection change.
   * Stores the pending choice — actual API call is deferred to form submit.
   */
  onPendingMaterialTemplateChange(templateId: string | null): void {
    this.pendingMaterialTemplateId = templateId;
  }

  /**
   * Save both activity template and material template assignments for the given project.
   * Only calls the API if the selections differ from what is already persisted.
   * Invokes `onSuccess` when all saves complete (or when there is nothing to save).
   */
  saveAllTemplateAssignments(projectId: string, onSuccess: () => void): void {
    const activityChanged = this.selectedProjectActivityTemplateId !== this.originalActivityTemplateId;
    const materialChanged = this.pendingMaterialTemplateId !== this.originalMaterialTemplateId;

    const saves: Array<() => void> = [];
    let pending = 0;
    let hasError = false;

    const tryComplete = () => {
      pending--;
      if (pending === 0 && !hasError) {
        onSuccess();
      }
    };

    const handleError = (label: string, err: any) => {
      hasError = true;
      pending = 0; // stop waiting
      console.error(`Error saving ${label}:`, err);
      this.toastService.error(`Failed to save ${label}`);
      this.loading = false;
    };

    if (activityChanged) {
      pending++;
      saves.push(() => {
        this.projectService.updateProject(projectId, {
          projectId: projectId,
          activityTemplateId: this.selectedProjectActivityTemplateId
        } as any).subscribe({
          next: () => {
            this.originalActivityTemplateId = this.selectedProjectActivityTemplateId;
            tryComplete();
          },
          error: (err: any) => handleError('activity template', err)
        });
      });
    }

    if (materialChanged) {
      if (this.pendingMaterialTemplateId) {
        // Assign or change: include materialTemplateId in the UpdateProject request.
        // UpdateProjectCommandHandler reads the old template from the DB internally
        // and propagates the change to eligible box types — no separate API call needed.
        pending++;
        saves.push(() => {
          this.projectService.updateProject(projectId, {
            projectId,
            materialTemplateId: this.pendingMaterialTemplateId
          } as any).subscribe({
            next: () => {
              this.originalMaterialTemplateId = this.pendingMaterialTemplateId;
              tryComplete();
            },
            error: (err: any) => handleError('material template', err)
          });
        });
      } else if (this.originalMaterialTemplateId && !this.pendingMaterialTemplateId) {
        // Clear / remove the existing assignment
        pending++;
        saves.push(() => {
          this.materialTemplateService.removeFromProject(projectId, this.originalMaterialTemplateId!).subscribe({
            next: () => {
              this.originalMaterialTemplateId = null;
              tryComplete();
            },
            error: (err: any) => handleError('material template removal', err)
          });
        });
      }
    }

    if (pending === 0) {
      // Nothing to save
      onSuccess();
      return;
    }

    saves.forEach(fn => fn());
  }

  /**
   * Handle box type activity template selection
   */
  onBoxTypeActivityTemplateChange(boxTypeId: number, templateId: string | null): void {
    const projectId = this.isEdit ? this.projectId : this.createdProjectId;
    if (!projectId) {
      this.toastService.error('Project must be created first');
      return;
    }

    if (templateId) {
      this.boxTypeActivityTemplates.set(boxTypeId, templateId);
    } else {
      this.boxTypeActivityTemplates.delete(boxTypeId);
    }

    // Update box type with activity template
    // Note: Backend requires projectId and boxTypeId in body for validation
    this.projectService.updateProjectBoxType(projectId, boxTypeId, {
      projectId: projectId,
      boxTypeId: boxTypeId,
      activityTemplateId: templateId
    } as any).subscribe({
      next: () => {
        this.toastService.success('Box type activity template updated');
      },
      error: (error: any) => {
        console.error('Error updating box type activity template:', error);
        this.toastService.error('Failed to update box type activity template');
      }
    });
  }

  /**
   * Get activity template name by ID
   */
  getActivityTemplateName(templateId: string | null): string {
    if (!templateId) return 'None';
    const template = this.availableActivityTemplates.find(t => t.activityTemplateId === templateId);
    return template ? template.templateName : 'Unknown';
  }

  /**
   * Get activity template activity count by ID
   */
  getTemplateActivityCount(templateId: string | null): number {
    if (!templateId) return 0;
    const template = this.availableActivityTemplates.find(t => t.activityTemplateId === templateId);
    return template ? template.activityCount : 0;
  }
  
  private saveConfigurationAndMoveToStep2(projectId: string): void {
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
        // Reload configuration to get IDs assigned by backend
        // This is crucial for template assignment to work
        this.projectService.getProjectConfiguration(projectId).subscribe({
          next: (config) => {
            this.buildings = config.buildings || [];
            this.levels = config.levels || [];
            this.boxTypes = config.boxTypes || [];
            this.zones = config.zones || [];
            this.boxFunctions = config.boxFunctions || [];
            
            // Load material templates after configuration is loaded
            this.loadMaterialTemplates();
            this.loadActivityTemplates();
            
            this.loading = false;
            this.createdProjectId = projectId;
            this.currentStep = 2;
            this.successMessage = 'Project created successfully! Now you can optionally add logos.';
            this.router.navigate([], {
              relativeTo: this.route,
              queryParams: { step: '2', projectId: projectId },
              queryParamsHandling: 'merge'
            });
          },
          error: (err) => {
            console.error('❌ Error reloading configuration:', err);
            // Continue even if reload fails
            this.loading = false;
            this.createdProjectId = projectId;
            this.currentStep = 2;
            this.successMessage = 'Project created successfully! Now you can optionally add logos.';
            this.router.navigate([], {
              relativeTo: this.route,
              queryParams: { step: '2', projectId: projectId },
              queryParamsHandling: 'merge'
            });
          }
        });
      },
      error: (err) => {
        // Even if configuration save fails, move to step 2
        console.error('❌ Error saving configuration:', err);
        this.loading = false;
        this.createdProjectId = projectId;
        this.currentStep = 2;
        this.successMessage = 'Project created successfully! (Configuration save had issues)';
        this.toastService.error('Project created but configuration save failed: ' + (err.error?.message || err.message));
        this.router.navigate([], {
          relativeTo: this.route,
          queryParams: { step: '2', projectId: projectId },
          queryParamsHandling: 'merge'
        });
      }
    });
  }
  
  private uploadImagesStep2(): void {
    if (!this.createdProjectId) {
      this.error = 'Project ID is missing. Please go back and try again.';
      return;
    }

    if (!this.hasImagesToUpload()) {
      // Skip images and go to step 3 (materials)
      this.moveToStep3();
      return;
    }

    this.loading = true;
    this.error = '';
    this.successMessage = '';

    this.projectService.uploadProjectImages(
      this.createdProjectId,
      this.contractorImage,
      this.subContractorImage,
      this.clientImage
    ).subscribe({
      next: () => {
        this.loading = false;
        this.successMessage = 'Project logos uploaded successfully!';
        this.toastService.success('Project and logos uploaded successfully!');
        
        // Move to step 3 (materials)
        setTimeout(() => {
          this.moveToStep3();
        }, 800);
      },
      error: (err) => {
        this.loading = false;
        const errorMessage = err.error?.message || err.message || 'Failed to upload logos.';
        this.error = errorMessage;
        this.toastService.error(errorMessage);
        console.error('❌ Error uploading logos:', err);
      }
    });
  }
  
  skipImages(): void {
    if (this.createdProjectId) {
      this.moveToStep3();
    }
  }

  private moveToStep3(): void {
    this.currentStep = 3;
    this.successMessage = 'Now assign material templates to this project (optional)';
    
    // Load project configuration to get box types
    if (this.createdProjectId) {
      this.loadProjectConfiguration(this.createdProjectId);
    }
    
    this.loadMaterialTemplates(); // Load templates when entering step 3
    this.loadActivityTemplates(); // Load activity templates when entering step 3
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { step: '3', projectId: this.createdProjectId },
      queryParamsHandling: 'merge'
    });
  }

  skipMaterials(): void {
    if (this.createdProjectId) {
      this.router.navigate(['/projects', this.createdProjectId, 'dashboard']);
    }
  }

  private hasConfiguration(): boolean {
    return this.buildings.length > 0 || 
           this.levels.length > 0 || 
           this.boxTypes.length > 0 || 
           this.zones.length > 0 || 
           this.boxFunctions.length > 0;
  }

  private hasImagesToUpload(): boolean {
    return this.contractorImage !== null || 
           this.subContractorImage !== null || 
           this.clientImage !== null;
  }

  private uploadImages(projectId: string): void {
    this.projectService.uploadProjectImages(
      projectId,
      this.contractorImage,
      this.subContractorImage,
      this.clientImage
    ).subscribe({
      next: () => {
        console.log('✅ Project images uploaded successfully');
        
        // After images are uploaded, save configuration if exists
        if (this.hasConfiguration()) {
          this.saveConfiguration(projectId);
        } else {
          this.loading = false;
          this.successMessage = this.isEdit ? 'Project updated successfully!' : 'Project created successfully!';
          
          setTimeout(() => {
            this.router.navigate(['/projects', projectId, 'dashboard']);
          }, 1200);
        }
      },
      error: (err) => {
        console.error('❌ Error uploading images:', err);
        // Continue with configuration save even if image upload fails
        // Show warning but don't block the flow
        this.toastService.error('Project saved but image upload failed: ' + (err.error?.message || err.message));
        
        if (this.hasConfiguration()) {
          this.saveConfiguration(projectId);
        } else {
          this.loading = false;
          this.successMessage = 'Project saved (with image upload errors)';
          
          setTimeout(() => {
            this.router.navigate(['/projects', projectId, 'dashboard']);
          }, 1200);
        }
      }
    });
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

  // Logo Upload Methods
  
  /**
   * Handle contractor logo selection
   */
  onContractorImageSelect(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      
      // Validate file type
      if (!file.type.startsWith('image/')) {
        this.toastService.error('Please select a valid logo file');
        return;
      }
      
      // Validate file size (max 5MB)
      if (file.size > 5 * 1024 * 1024) {
        this.toastService.error('Logo size must be less than 5MB');
        return;
      }
      
      this.contractorImage = file;
      this.contractorImageUrl = null; // Clear existing URL when new logo is selected
      
      // Generate preview
      const reader = new FileReader();
      reader.onload = (e) => {
        this.contractorImagePreview = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }
  
  /**
   * Remove contractor logo
   */
  removeContractorImage(): void {
    this.contractorImage = null;
    this.contractorImagePreview = null;
    this.contractorImageUrl = null; // Also clear existing URL when removing
  }
  
  /**
   * Handle sub-contractor logo selection
   */
  onSubContractorImageSelect(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      
      // Validate file type
      if (!file.type.startsWith('image/')) {
        this.toastService.error('Please select a valid logo file');
        return;
      }
      
      // Validate file size (max 5MB)
      if (file.size > 5 * 1024 * 1024) {
        this.toastService.error('Logo size must be less than 5MB');
        return;
      }
      
      this.subContractorImage = file;
      this.subContractorImageUrl = null; // Clear existing URL when new logo is selected
      
      // Generate preview
      const reader = new FileReader();
      reader.onload = (e) => {
        this.subContractorImagePreview = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }
  
  /**
   * Remove sub-contractor logo
   */
  removeSubContractorImage(): void {
    this.subContractorImage = null;
    this.subContractorImagePreview = null;
    this.subContractorImageUrl = null; // Also clear existing URL when removing
  }
  
  /**
   * Handle client logo selection
   */
  onClientImageSelect(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      
      // Validate file type
      if (!file.type.startsWith('image/')) {
        this.toastService.error('Please select a valid logo file');
        return;
      }
      
      // Validate file size (max 5MB)
      if (file.size > 5 * 1024 * 1024) {
        this.toastService.error('Logo size must be less than 5MB');
        return;
      }
      
      this.clientImage = file;
      this.clientImageUrl = null; // Clear existing URL when new logo is selected
      
      // Generate preview
      const reader = new FileReader();
      reader.onload = (e) => {
        this.clientImagePreview = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }
  
  /**
   * Remove client logo
   */
  removeClientImage(): void {
    this.clientImage = null;
    this.clientImagePreview = null;
    this.clientImageUrl = null; // Also clear existing URL when removing
  }

  /**
   * Load available material templates
   */
  loadMaterialTemplates(): void {
    this.loadingTemplates = true;
    this.materialTemplateService.getAllTemplates().subscribe({
      next: (templates: MaterialTemplate[]) => {
        this.availableTemplates = templates.filter((t: MaterialTemplate) => t.isActive);
        this.loadingTemplates = false;
        
        // Load existing assignments if in edit mode or step 3
        if (this.isEdit && this.projectId) {
          this.loadProjectTemplateAssignments(this.projectId);
        } else if (!this.isEdit && this.createdProjectId) {
          this.loadProjectTemplateAssignments(this.createdProjectId);
        }
      },
      error: (error) => {
        console.error('Error loading templates:', error);
        this.toastService.error('Failed to load material templates');
        this.loadingTemplates = false;
      }
    });
  }

  /**
   * Load existing template assignments for the project (single template per project)
   */
  loadProjectTemplateAssignments(projectId: string): void {
    // Load project-level template (single)
    this.materialTemplateService.getProjectTemplates(projectId).subscribe({
      next: (assignments: ProjectMaterialTemplate[]) => {
        // Single template per project
        this.projectTemplates = assignments
          .map((assignment: ProjectMaterialTemplate) => {
            return this.availableTemplates.find((t: MaterialTemplate) => t.materialTemplateId === assignment.materialTemplateId);
          })
          .filter((t): t is MaterialTemplate => t !== undefined);

        // Track the original and pending template IDs for deferred save
        const firstAssignment = assignments.length > 0 ? assignments[0] : null;
        this.originalMaterialTemplateId = firstAssignment?.materialTemplateId ?? null;
        this.pendingMaterialTemplateId = this.originalMaterialTemplateId;
      },
      error: (error: any) => {
        console.error('Error loading project template assignments:', error);
      }
    });

    // Load all box-type-level templates for this project
    this.materialTemplateService.getBoxTypeTemplates(projectId).subscribe({
      next: (assignments: BoxTypeMaterialTemplate[]) => {
        // Map box type templates by box type ID
        this.boxTypeTemplates.clear();
        assignments.forEach(assignment => {
          const template = this.availableTemplates.find(t => t.materialTemplateId === assignment.materialTemplateId);
          if (template) {
            this.boxTypeTemplates.set(assignment.projectBoxTypeId, template);
          }
        });
      },
      error: (error) => {
        console.error('Error loading box type templates:', error);
      }
    });
  }

  /**
   * Open modal to assign project-level template
   */
  assignProjectTemplate(): void {
    const projectId = this.isEdit ? this.projectId : this.createdProjectId;
    if (!projectId) {
      this.toastService.error('Project must be created first');
      return;
    }

    this.templateAssignment = {
      level: 'project',
      projectId: projectId
    };
    this.showTemplateModal = true;
  }

  /**
   * Open modal to assign box-type-level template
   */
  assignBoxTypeTemplate(boxType: ProjectBoxType): void {
    const projectId = this.isEdit ? this.projectId : this.createdProjectId;
    if (!projectId || !boxType.id) {
      this.toastService.error('Project and box type must be created first');
      return;
    }

    // Show all available templates for box type assignment
    this.templateAssignment = {
      level: 'boxType',
      projectId: projectId,
      boxTypeId: boxType.id,
      boxTypeName: boxType.typeName
    };
    this.showTemplateModal = true;
  }
  
  /**
   * Remove a project template
   */
  removeProjectTemplate(template: MaterialTemplate): void {
    const projectId = this.isEdit ? this.projectId : this.createdProjectId;
    if (!projectId) {
      this.toastService.error('Project must be created first');
      return;
    }

    this.materialTemplateService.removeFromProject(projectId, template.materialTemplateId).subscribe({
      next: () => {
        this.projectTemplates = this.projectTemplates.filter(t => t.materialTemplateId !== template.materialTemplateId);
        this.toastService.success('Template removed from project');
      },
      error: (err) => {
        console.error('Error removing template:', err);
        this.toastService.error('Failed to remove template');
      }
    });
  }

  /**
   * Handle template assignment completion
   */
  onTemplateAssigned(event: { templateId: string }): void {
    const projectId = this.isEdit ? this.projectId : this.createdProjectId;
    
    if (event.templateId) {
      // Find the assigned template
      const template = this.availableTemplates.find(t => t.materialTemplateId === event.templateId);
      
      if (template && this.templateAssignment) {
        if (this.templateAssignment.level === 'project') {
          // Add to project templates array if not already present
          if (!this.projectTemplates.find(t => t.materialTemplateId === template.materialTemplateId)) {
            this.projectTemplates.push(template);
          }
          this.toastService.success('Project template assigned successfully');
        } else if (this.templateAssignment.level === 'boxType' && this.templateAssignment.boxTypeId) {
          this.boxTypeTemplates.set(this.templateAssignment.boxTypeId, template);
          this.toastService.success('Box type template assigned successfully');
        }
      }
    } else {
      // Template was removed - reload all assignments to sync
      if (this.templateAssignment && this.templateAssignment.level === 'project' && projectId) {
        this.loadProjectTemplateAssignments(projectId);
        this.toastService.success('Project template removed');
      } else if (this.templateAssignment && this.templateAssignment.level === 'boxType' && this.templateAssignment.boxTypeId) {
        this.boxTypeTemplates.delete(this.templateAssignment.boxTypeId);
        this.toastService.success('Box type template removed');
      }
    }
    
    this.showTemplateModal = false;
    this.templateAssignment = null;
  }

  /**
   * Close template modal
   */
  closeTemplateModal(): void {
    this.showTemplateModal = false;
    this.templateAssignment = null;
  }

  /**
   * Get template for a specific box type
   */
  getBoxTypeTemplate(boxTypeId: number): MaterialTemplate | null {
    return this.boxTypeTemplates.get(boxTypeId) || null;
  }

  /**
   * Remove template from box type
   */
  removeBoxTypeTemplate(boxType: ProjectBoxType): void {
    const projectId = this.isEdit ? this.projectId : this.createdProjectId;
    if (!projectId || !boxType.id) {
      this.toastService.error('Project and box type must be created first');
      return;
    }

    const template = this.getBoxTypeTemplate(boxType.id);
    if (!template) {
      return;
    }

    // Confirm removal
    if (!confirm(`Are you sure you want to remove the template "${template.templateName}" from box type "${boxType.typeName}"?\n\nThis will remove the template assignment but will not remove materials that have already been added to this box type.`)) {
      return;
    }

    // Call service to remove template from box type
    this.loadingTemplates = true;
    this.materialTemplateService.removeFromBoxType(boxType.id, template.materialTemplateId).subscribe({
      next: () => {
        // Remove from local state
        this.boxTypeTemplates.delete(boxType.id!);
        this.toastService.success(`Template "${template.templateName}" removed from box type "${boxType.typeName}"`);
        this.loadingTemplates = false;
      },
      error: (error) => {
        console.error('Error removing template from box type:', error);
        this.toastService.error('Failed to remove template from box type');
        this.loadingTemplates = false;
      }
    });
  }

  // Toggle methods for collapsible sections
  toggleProjectConfig(): void {
    this.isProjectConfigCollapsed = !this.isProjectConfigCollapsed;
  }

  toggleBoxTypes(): void {
    this.isBoxTypesCollapsed = !this.isBoxTypesCollapsed;
  }

  toggleActivityTemplates(): void {
    this.isActivityTemplatesCollapsed = !this.isActivityTemplatesCollapsed;
  }

  toggleMaterialTemplates(): void {
    this.isMaterialTemplatesCollapsed = !this.isMaterialTemplatesCollapsed;
  }

  toggleProjectLogos(): void {
    this.isProjectLogosCollapsed = !this.isProjectLogosCollapsed;
  }
}

