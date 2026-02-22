import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormArray, FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivityTemplateService } from '../../../core/services/activity-template.service';
import { ToastService } from '../../../core/services/toast.service';
import { WirChecklistService } from '../../../core/services/wir-checklist.service';
import { TeamService } from '../../../core/services/team.service';
import { Team } from '../../../core/models/team.model';
import { 
  ActivityMaster, 
  CreateActivityTemplateActivityDto, 
  ActivityStage,
  PredefinedChecklistItem
} from '../../../core/models/activity-template.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-create-activity-template',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './create-activity-template.component.html',
  styleUrl: './create-activity-template.component.scss'
})
export class CreateActivityTemplateComponent implements OnInit {
  templateForm!: FormGroup;
  isEditMode = false;
  templateId: string | null = null;
  
  // Stages
  stages: ActivityStage[] = [];
  numberOfStages = 1;
  stagesGenerated = false;
  
  // Activity Master selection
  showActivityMasterModal = false;
  availableActivityMasters: ActivityMaster[] = [];
  filteredActivityMasters: ActivityMaster[] = [];
  activityMasterSearchTerm = '';
  selectedStageForActivity: ActivityStage | null = null;
  groupedActivityMasters: Map<string, ActivityMaster[]> = new Map();
  expandedMasterStages: Set<string> = new Set();
  
  // Custom Activity modal
  showCustomActivityModal = false;
  customActivityForm!: FormGroup;
  selectedStageForCustomActivity: ActivityStage | null = null;
  savingCustomActivity = false;
  
  // Teams for activity assignment
  availableTeams: Team[] = [];
  showTeamAssignmentModal = false;
  selectedActivityForTeam: CreateActivityTemplateActivityDto | null = null;
  selectedTeamId: string | null = null;
  
  // Loading and error states
  loading = false;
  loadingActivityMasters = false;
  submitting = false;
  error: string | null = null;
  activityMasterError: string | null = null;
  
  // Confirmation modal
  showConfirmModal = false;
  confirmModalData: {
    title: string;
    message: string;
    confirmText: string;
    cancelText: string;
    onConfirm: () => void;
  } | null = null;
  
  // Step indicator
  currentStep = 1; // 1 = Create Template, 2 = Manage Checklist
  createdTemplateId: string | null = null;
  
  // Checklist Items management
  showChecklistItemsModal = false;
  selectedActivityForChecklist: CreateActivityTemplateActivityDto | null = null;
  selectedStageForChecklist: ActivityStage | null = null;
  availableChecklistItems: PredefinedChecklistItem[] = [];
  groupedChecklists: Array<{
    checklistId: string;
    checklistName: string;
    expanded: boolean;
    sections: Array<{
      sectionId: string;
      sectionName: string;
      expanded: boolean;
      items: PredefinedChecklistItem[];
    }>;
  }> = [];
  loadingChecklistItems = false;
  checklistItemsError: string | null = null;
  tempSelectedChecklistItems: Array<{ predefinedChecklistItemId: string; sequence: number; isMandatory: boolean }> = [];

  constructor(
    private fb: FormBuilder,
    private templateService: ActivityTemplateService,
    private toastService: ToastService,
    private router: Router,
    private route: ActivatedRoute,
    private wirChecklistService: WirChecklistService,
    private http: HttpClient,
    private teamService: TeamService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.initializeCustomActivityForm();
    this.loadActivityMasters();
    this.loadTeams();
    
    // Check if returning from manage checklist page
    const savedStateJson = sessionStorage.getItem('activityTemplateState');
    if (savedStateJson) {
      try {
        const savedState = JSON.parse(savedStateJson);
        
        // Restore form values
        if (savedState.templateForm) {
          this.templateForm.patchValue(savedState.templateForm);
        }
        
        // Restore stages and activities
        if (savedState.stages && savedState.stages.length > 0) {
          this.stages = savedState.stages;
          this.numberOfStages = savedState.numberOfStages || savedState.stages.length;
          this.stagesGenerated = savedState.stagesGenerated !== undefined ? savedState.stagesGenerated : true;
        }
        
        // Restore edit mode state
        if (savedState.templateId) {
          this.templateId = savedState.templateId;
          this.isEditMode = savedState.isEditMode || false;
        }
        
        // Clear the saved state after restoring
        sessionStorage.removeItem('activityTemplateState');
        
        console.log('✅ Template state restored from session storage');
        
        // Skip loading from backend if we restored from session
        return;
      } catch (e) {
        console.error('Error restoring template state:', e);
        sessionStorage.removeItem('activityTemplateState');
      }
    }
    
    // Check if edit mode (only if not restored from session)
    this.templateId = this.route.snapshot.paramMap.get('id');
    if (this.templateId) {
      this.isEditMode = true;
      this.loadTemplate();
    }
  }

  initializeForm(): void {
    this.templateForm = this.fb.group({
      templateName: ['', Validators.required],
      description: [''],
      numberOfStages: [1, [Validators.required, Validators.min(1), Validators.max(20)]]
    });
  }

  initializeCustomActivityForm(): void {
    this.customActivityForm = this.fb.group({
      activityCode: ['', Validators.required],
      activityName: ['', Validators.required],
      description: [''],
      estimatedDurationDays: [1, [Validators.required, Validators.min(1)]],
      isWIRCheckpoint: [false],
      wirCode: [''],
      applicableBoxTypes: [''],
      dependsOnActivities: [''],
      assignedTeamId: [null]
    });
  }

  loadTemplate(): void {
    if (!this.templateId) return;
    
    this.loading = true;
    this.error = null;

    this.templateService.getTemplateById(this.templateId).subscribe({
      next: (template) => {
        this.templateForm.patchValue({
          templateName: template.templateName,
          description: template.description,
          numberOfStages: template.stageCount
        });
        
        this.numberOfStages = template.stageCount;
        this.generateStages();
        
        // Load template activities into stages
        if (template.activities && template.activities.length > 0) {
          template.activities.forEach((activity: any) => {
            const stage = this.stages.find(s => s.stageNumber === activity.stageNumber);
            if (stage) {
              stage.activities.push({
                activityTemplateActivityId: activity.activityTemplateActivityId,
                sourceActivityMasterId: activity.sourceActivityMasterId,
                isCustomActivity: activity.isCustomActivity,
                activityCode: activity.activityCode,
                activityName: activity.activityName,
                stage: activity.stage,
                stageNumber: activity.stageNumber,
                sequenceInStage: activity.sequenceInStage,
                overallSequence: activity.overallSequence,
                description: activity.description,
                estimatedDurationDays: activity.estimatedDurationDays,
                isWIRCheckpoint: activity.isWIRCheckpoint,
                wirCode: activity.wirCode,
                applicableBoxTypes: activity.applicableBoxTypes,
                dependsOnActivities: activity.dependsOnActivities,
                assignedTeamId: activity.assignedTeamId,
                selectedChecklistItemIds: activity.selectedChecklistItems?.map((ci: any) => ci.predefinedChecklistItemId) || [],
                selectedChecklistItems: activity.selectedChecklistItems?.map((ci: any) => ({
                  predefinedChecklistItemId: ci.predefinedChecklistItemId,
                  sequence: ci.sequence,
                  isMandatory: ci.isMandatory
                })) || []
              });
              
              if (activity.isWIRCheckpoint) {
                stage.hasWIR = true;
                stage.wirActivityId = activity.activityTemplateActivityId;
              }
            }
          });
          
          this.stagesGenerated = true;
        }
        
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load template';
        this.loading = false;
        console.error('Activity Template - Error loading template:', err);
      }
    });
  }

  loadActivityMasters(): void {
    this.loadingActivityMasters = true;
    this.activityMasterError = null;

    this.templateService.getAllActivityMasters().subscribe({
      next: (masters) => {
        this.availableActivityMasters = masters || [];
        this.filteredActivityMasters = this.availableActivityMasters;
        this.groupActivityMastersByStage();
        this.loadingActivityMasters = false;
      },
      error: (err) => {
        this.activityMasterError = 'Failed to load activity masters';
        this.loadingActivityMasters = false;
        console.error('Error loading activity masters:', err);
      }
    });
  }

  loadTeams(): void {
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.availableTeams = teams.filter(t => t.isActive) || [];
      },
      error: (err) => {
        console.error('Error loading teams:', err);
      }
    });
  }

  groupActivityMastersByStage(): void {
    this.groupedActivityMasters.clear();
    this.filteredActivityMasters.forEach(master => {
      const stage = master.stage || 'Uncategorized';
      if (!this.groupedActivityMasters.has(stage)) {
        this.groupedActivityMasters.set(stage, []);
      }
      this.groupedActivityMasters.get(stage)!.push(master);
    });
  }

  getActivityMasterStageKeys(): string[] {
    return Array.from(this.groupedActivityMasters.keys()).sort();
  }

  getActivityMastersInStage(stage: string): ActivityMaster[] {
    const masters = this.groupedActivityMasters.get(stage) || [];
    // Filter out activities that are already added to the current stage
    if (!this.selectedStageForActivity) {
      return masters;
    }
    
    return masters.filter(master => {
      const isAlreadyAdded = this.selectedStageForActivity!.activities.some(a => 
        a.sourceActivityMasterId === master.activityMasterId
      );
      return !isAlreadyAdded;
    });
  }
  
  isActivityAlreadyAdded(master: ActivityMaster): boolean {
    if (!this.selectedStageForActivity) return false;
    return this.selectedStageForActivity.activities.some(a => 
      a.sourceActivityMasterId === master.activityMasterId
    );
  }

  isActivityMasterStageExpanded(stage: string): boolean {
    return this.expandedMasterStages.has(stage);
  }

  toggleActivityMasterStage(stage: string): void {
    if (this.expandedMasterStages.has(stage)) {
      this.expandedMasterStages.delete(stage);
    } else {
      this.expandedMasterStages.add(stage);
    }
  }

  expandAllActivityMasterStages(): void {
    this.getActivityMasterStageKeys().forEach(stage => this.expandedMasterStages.add(stage));
  }

  collapseAllActivityMasterStages(): void {
    this.expandedMasterStages.clear();
  }

  generateStages(): void {
    this.numberOfStages = this.templateForm.get('numberOfStages')?.value || 1;
    
    const currentStageCount = this.stages.length;
    
    // If increasing stages, add new ones
    if (this.numberOfStages > currentStageCount) {
      for (let i = currentStageCount + 1; i <= this.numberOfStages; i++) {
        this.stages.push({
          stageNumber: i,
          stageName: `Stage ${i.toString().padStart(2, '0')}`,
          activities: [],
          expanded: true,
          hasWIR: false
        });
      }
    }
    // If decreasing stages, remove extra ones
    else if (this.numberOfStages < currentStageCount) {
      this.stages = this.stages.slice(0, this.numberOfStages);
    }
    // If no existing stages, generate fresh
    else if (currentStageCount === 0) {
      for (let i = 1; i <= this.numberOfStages; i++) {
        this.stages.push({
          stageNumber: i,
          stageName: `Stage ${i.toString().padStart(2, '0')}`,
          activities: [],
          expanded: true,
          hasWIR: false
        });
      }
    }
    
    this.stagesGenerated = true;
    
    // Auto-scroll to stages section after generation
    setTimeout(() => {
      const stagesSection = document.querySelector('.stages-section');
      if (stagesSection) {
        stagesSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    }, 100);
  }

  toggleStage(stage: ActivityStage): void {
    stage.expanded = !stage.expanded;
  }

  recalculateSequences(stage: ActivityStage): void {
    stage.activities.forEach((activity, index) => {
      activity.sequenceInStage = index + 1;
    });
  }

  openActivityMasterModal(stage: ActivityStage): void {
    this.selectedStageForActivity = stage;
    this.showActivityMasterModal = true;
    this.activityMasterSearchTerm = '';
    this.filterActivityMasters();
  }

  closeActivityMasterModal(): void {
    this.showActivityMasterModal = false;
    this.selectedStageForActivity = null;
  }

  filterActivityMasters(): void {
    if (!this.activityMasterSearchTerm) {
      this.filteredActivityMasters = this.availableActivityMasters;
    } else {
      const searchLower = this.activityMasterSearchTerm.toLowerCase();
      this.filteredActivityMasters = this.availableActivityMasters.filter(m =>
        m.activityName.toLowerCase().includes(searchLower) ||
        m.activityCode.toLowerCase().includes(searchLower) ||
        (m.wirCode && m.wirCode.toLowerCase().includes(searchLower))
      );
    }
    this.groupActivityMastersByStage();
  }

  addActivityFromMaster(master: ActivityMaster): void {
    if (!this.selectedStageForActivity) return;
    
    // Check if activity already exists in this stage
    const exists = this.selectedStageForActivity.activities.some(a => 
      a.sourceActivityMasterId === master.activityMasterId
    );
    
    if (exists) {
      this.toastService.warning('This activity is already added to this stage');
      return;
    }
    
    // Check if stage already has a WIR activity
    if (master.isWIRCheckpoint && this.selectedStageForActivity.hasWIR) {
      this.toastService.error('This stage already has a WIR activity. Remove it first.');
      return;
    }
    
    // For WIR activities, use stage code format (e.g., STAGE3-WIR)
    const activityCode = master.isWIRCheckpoint 
      ? `STAGE${this.selectedStageForActivity.stageNumber}-WIR`
      : master.activityCode;
    
    const newActivity: CreateActivityTemplateActivityDto = {
      sourceActivityMasterId: master.activityMasterId,
      isCustomActivity: false,
      activityCode: activityCode,
      activityName: master.activityName,
      stage: this.selectedStageForActivity.stageName,
      stageNumber: this.selectedStageForActivity.stageNumber,
      sequenceInStage: this.selectedStageForActivity.activities.length + 1,
      overallSequence: 0, // Will be calculated on save
      description: master.description,
      estimatedDurationDays: master.estimatedDurationDays,
      isWIRCheckpoint: master.isWIRCheckpoint,
      wirCode: master.wirCode,
      applicableBoxTypes: master.applicableBoxTypes,
      dependsOnActivities: master.dependsOnActivities,
      assignedTeamId: undefined
    };
    
    // WIR activities should always be added at the end of the stage
    if (master.isWIRCheckpoint) {
      this.selectedStageForActivity.activities.push(newActivity);
      this.selectedStageForActivity.hasWIR = true;
      // Update sequence to be last
      newActivity.sequenceInStage = this.selectedStageForActivity.activities.length;
    } else {
      // For non-WIR activities, insert before any WIR activity if it exists
      const wirIndex = this.selectedStageForActivity.activities.findIndex(a => a.isWIRCheckpoint);
      if (wirIndex !== -1) {
        this.selectedStageForActivity.activities.splice(wirIndex, 0, newActivity);
        // Recalculate sequences
        this.recalculateSequences(this.selectedStageForActivity);
      } else {
        this.selectedStageForActivity.activities.push(newActivity);
      }
    }
    
    this.toastService.success(`Added "${master.activityName}" to ${this.selectedStageForActivity.stageName}`);
  }

  addAllActivitiesFromMasterStage(masterStage: string): void {
    if (!this.selectedStageForActivity) return;
    
    const masters = this.groupedActivityMasters.get(masterStage) || [];
    let addedCount = 0;
    let skippedCount = 0;
    let wirSkipped = false;
    
    masters.forEach(master => {
      // Check if activity already exists in this stage
      const exists = this.selectedStageForActivity!.activities.some(a => 
        a.sourceActivityMasterId === master.activityMasterId
      );
      
      if (exists) {
        skippedCount++;
        return;
      }
      
      // Check if stage already has a WIR activity
      if (master.isWIRCheckpoint && this.selectedStageForActivity!.hasWIR) {
        wirSkipped = true;
        skippedCount++;
        return;
      }
      
      const newActivity: CreateActivityTemplateActivityDto = {
        sourceActivityMasterId: master.activityMasterId,
        isCustomActivity: false,
        activityCode: master.activityCode,
        activityName: master.activityName,
        stage: this.selectedStageForActivity!.stageName,
        stageNumber: this.selectedStageForActivity!.stageNumber,
        sequenceInStage: this.selectedStageForActivity!.activities.length + 1,
        overallSequence: 0,
        description: master.description,
        estimatedDurationDays: master.estimatedDurationDays,
        isWIRCheckpoint: master.isWIRCheckpoint,
        wirCode: master.wirCode,
        applicableBoxTypes: master.applicableBoxTypes,
        dependsOnActivities: master.dependsOnActivities
      };
      
      this.selectedStageForActivity!.activities.push(newActivity);
      
      if (master.isWIRCheckpoint) {
        this.selectedStageForActivity!.hasWIR = true;
      }
      
      addedCount++;
    });
    
    if (addedCount > 0) {
      this.toastService.success(`Added ${addedCount} activit${addedCount > 1 ? 'ies' : 'y'} from ${masterStage}`);
    }
    
    if (skippedCount > 0) {
      if (wirSkipped) {
        this.toastService.info(`Skipped ${skippedCount} activit${skippedCount > 1 ? 'ies' : 'y'} (already added or WIR conflict)`);
      } else {
        this.toastService.info(`Skipped ${skippedCount} activit${skippedCount > 1 ? 'ies' : 'y'} (already added)`);
      }
    }
  }

  openCustomActivityModal(stage: ActivityStage): void {
    this.selectedStageForCustomActivity = stage;
    
    // Generate WIR code based on stage number (e.g., STAGE 01 -> WIR-01)
    const stageNumber = stage.stageNumber.toString().padStart(2, '0');
    const wirCode = `WIR-${stageNumber}`;
    
    this.customActivityForm.reset({
      activityCode: '',
      activityName: '',
      description: '',
      estimatedDurationDays: 1,
      isWIRCheckpoint: false,
      wirCode: wirCode,
      applicableBoxTypes: '',
      dependsOnActivities: ''
    });
    this.showCustomActivityModal = true;
  }

  closeCustomActivityModal(): void {
    this.showCustomActivityModal = false;
    this.selectedStageForCustomActivity = null;
    this.customActivityForm.reset();
  }

  saveCustomActivity(): void {
    if (this.customActivityForm.invalid) {
      this.markFormGroupTouched(this.customActivityForm);
      return;
    }
    
    if (!this.selectedStageForCustomActivity) return;
    
    const formValue = this.customActivityForm.value;
    
    // Check if stage already has a WIR activity
    if (formValue.isWIRCheckpoint && this.selectedStageForCustomActivity.hasWIR) {
      this.toastService.error('This stage already has a WIR activity. Remove it first.');
      return;
    }
    
    // For WIR activities, use stage code format (e.g., STAGE3-WIR)
    const activityCode = formValue.isWIRCheckpoint 
      ? `STAGE${this.selectedStageForCustomActivity.stageNumber}-WIR`
      : formValue.activityCode;
    
    const newActivity: CreateActivityTemplateActivityDto = {
      sourceActivityMasterId: undefined,
      isCustomActivity: true,
      activityCode: activityCode,
      activityName: formValue.activityName,
      stage: this.selectedStageForCustomActivity.stageName,
      stageNumber: this.selectedStageForCustomActivity.stageNumber,
      sequenceInStage: this.selectedStageForCustomActivity.activities.length + 1,
      overallSequence: 0, // Will be calculated on save
      description: formValue.description,
      estimatedDurationDays: formValue.estimatedDurationDays,
      isWIRCheckpoint: formValue.isWIRCheckpoint,
      wirCode: formValue.wirCode,
      applicableBoxTypes: formValue.applicableBoxTypes,
      dependsOnActivities: formValue.dependsOnActivities,
      assignedTeamId: formValue.assignedTeamId
    };
    
    // WIR activities should always be added at the end of the stage
    if (formValue.isWIRCheckpoint) {
      this.selectedStageForCustomActivity.activities.push(newActivity);
      this.selectedStageForCustomActivity.hasWIR = true;
      // Update sequence to be last
      newActivity.sequenceInStage = this.selectedStageForCustomActivity.activities.length;
    } else {
      // For non-WIR activities, insert before any WIR activity if it exists
      const wirIndex = this.selectedStageForCustomActivity.activities.findIndex(a => a.isWIRCheckpoint);
      if (wirIndex !== -1) {
        this.selectedStageForCustomActivity.activities.splice(wirIndex, 0, newActivity);
        // Recalculate sequences
        this.recalculateSequences(this.selectedStageForCustomActivity);
      } else {
        this.selectedStageForCustomActivity.activities.push(newActivity);
      }
    }
    
    this.toastService.success(`Added custom activity "${formValue.activityName}" to ${this.selectedStageForCustomActivity.stageName}`);
    this.closeCustomActivityModal();
  }

  removeActivity(stage: ActivityStage, index: number): void {
    const activity = stage.activities[index];
    
    this.confirmModalData = {
      title: 'Remove Activity',
      message: `Are you sure you want to remove "${activity.activityName}"?`,
      confirmText: 'Remove',
      cancelText: 'Cancel',
      onConfirm: () => {
        if (activity.isWIRCheckpoint) {
          stage.hasWIR = false;
          stage.wirActivityId = undefined;
        }
        
        stage.activities.splice(index, 1);
        
        // Update sequence numbers
        stage.activities.forEach((act, idx) => {
          act.sequenceInStage = idx + 1;
        });
        
        this.closeConfirmModal();
        this.toastService.success('Activity removed');
      }
    };
    this.showConfirmModal = true;
  }

  moveActivityUp(stage: ActivityStage, index: number): void {
    if (index === 0) return;
    const activity = stage.activities[index];
    
    // Prevent moving WIR activities
    if (activity.isWIRCheckpoint) {
      this.toastService.warning('WIR activities must remain at the end of the stage');
      return;
    }
    
    // Prevent moving above WIR activity
    const targetActivity = stage.activities[index - 1];
    if (targetActivity.isWIRCheckpoint) {
      this.toastService.warning('Cannot move activity above WIR activity');
      return;
    }
    
    stage.activities.splice(index, 1);
    stage.activities.splice(index - 1, 0, activity);
    
    // Update sequence numbers
    this.recalculateSequences(stage);
  }

  moveActivityDown(stage: ActivityStage, index: number): void {
    if (index === stage.activities.length - 1) return;
    const activity = stage.activities[index];
    
    // Prevent non-WIR activities from moving past WIR activity
    const targetActivity = stage.activities[index + 1];
    if (targetActivity.isWIRCheckpoint) {
      this.toastService.warning('Cannot move activity past WIR activity. WIR must remain at the end.');
      return;
    }
    
    stage.activities.splice(index, 1);
    stage.activities.splice(index + 1, 0, activity);
    
    // Update sequence numbers
    this.recalculateSequences(stage);
  }

  getTotalActivitiesCount(): number {
    return this.stages.reduce((sum, stage) => sum + stage.activities.length, 0);
  }

  closeConfirmModal(): void {
    this.showConfirmModal = false;
    this.confirmModalData = null;
  }

  confirmAction(): void {
    if (this.confirmModalData?.onConfirm) {
      this.confirmModalData.onConfirm();
    }
  }

  onSubmit(): void {
    if (this.templateForm.invalid) {
      this.markFormGroupTouched(this.templateForm);
      return;
    }

    if (!this.stagesGenerated) {
      this.error = 'Please generate stages first';
      return;
    }

    if (this.getTotalActivitiesCount() === 0) {
      this.error = 'Please add at least one activity to the template';
      return;
    }

    this.submitting = true;
    this.error = null;

    const formValue = this.templateForm.value;
    
    // Calculate overall sequences
    let overallSeq = 1;
    const allActivities: CreateActivityTemplateActivityDto[] = [];
    
    this.stages.forEach(stage => {
      stage.activities.forEach(activity => {
        activity.overallSequence = overallSeq++;
        allActivities.push(activity);
      });
    });

    if (this.isEditMode && this.templateId) {
      // Update existing template
      const updateRequest = {
        activityTemplateId: this.templateId,
        templateName: formValue.templateName,
        description: formValue.description,
        stageCount: this.numberOfStages,
        isActive: true,
        activities: allActivities
      };

      this.templateService.updateTemplate(this.templateId, updateRequest).subscribe({
        next: (response) => {
          this.submitting = false;
          this.toastService.success('Activity Template updated successfully');
          this.router.navigate(['/schedule/activity-templates']);
        },
        error: (err) => {
          this.submitting = false;
          this.error = err.error?.error || err.error?.message || 'Failed to update template';
          console.error('Error updating template:', err);
        }
      });
    } else {
      // Create new template
      const createRequest = {
        templateName: formValue.templateName,
        description: formValue.description,
        stageCount: this.numberOfStages,
        activities: allActivities
      };

      this.templateService.createTemplate(createRequest).subscribe({
        next: (response) => {
          this.submitting = false;
          this.createdTemplateId = response.data?.activityTemplateId || response.data?.id;
          this.toastService.success('Activity Template created successfully');
          
          // Navigate to Step 2 (Manage Checklist)
          if (this.createdTemplateId) {
            this.router.navigate(['/schedule/activity-templates', this.createdTemplateId, 'manage-checklist']);
          } else {
            this.router.navigate(['/schedule/activity-templates']);
          }
        },
        error: (err) => {
          this.submitting = false;
          this.error = err.error?.error || err.error?.message || 'Failed to create template';
          console.error('Error creating template:', err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/schedule/activity-templates']);
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
    const field = this.templateForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }

  isCustomFieldInvalid(fieldName: string): boolean {
    const field = this.customActivityForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }

  trackByIndex(index: number): number {
    return index;
  }

  // Checklist Items Management Methods
  openChecklistItemsModal(activity: CreateActivityTemplateActivityDto, stage: ActivityStage): void {
    this.selectedActivityForChecklist = activity;
    this.selectedStageForChecklist = stage;
    this.showChecklistItemsModal = true;
    this.checklistItemsError = null;
    
    // Initialize temp selection from existing data
    this.tempSelectedChecklistItems = activity.selectedChecklistItems 
      ? [...activity.selectedChecklistItems] 
      : [];
    
    this.loadChecklistItems();
    
    console.log('Modal opened for activity:', activity.activityName);
  }

  closeChecklistItemsModal(): void {
    this.showChecklistItemsModal = false;
    this.selectedActivityForChecklist = null;
    this.selectedStageForChecklist = null;
    this.tempSelectedChecklistItems = [];
  }

  loadChecklistItems(): void {
    this.loadingChecklistItems = true;
    this.checklistItemsError = null;

    // Use HTTP client directly to call the correct endpoint
    this.http.get<any>(`${environment.apiUrl}/checklists`).subscribe({
      next: (response) => {
        console.log('📥 API Response received:', {
          success: response.isSuccess,
          checklistCount: response.data?.length || 0
        });
        
        if (response.isSuccess && response.data) {
          // Extract all predefined items from all checklists
          const allItems: PredefinedChecklistItem[] = [];
          
          response.data.forEach((checklist: any) => {
            if (checklist.sections) {
              checklist.sections.forEach((section: any) => {
                if (section.items) {
                  section.items.forEach((item: any) => {
                    allItems.push({
                      predefinedItemId: item.checklistItemId,
                      description: item.description,
                      sequence: item.order,
                      reference: item.reference,
                      checklistSectionId: section.checklistSectionId,
                      checklistSection: {
                        checklistSectionId: section.checklistSectionId,
                        sectionName: section.title,
                        displayOrder: section.order,
                        checklistId: checklist.checklistId,
                        checklist: {
                          checklistId: checklist.checklistId,
                          checklistName: checklist.name,
                          wirCode: checklist.wirCode,
                          isWirStageChecklist: checklist.isWirStageChecklist,
                          createdDate: checklist.createdDate,
                          createdBy: checklist.createdBy
                        }
                      },
                      isActive: item.isActive,
                      createdDate: item.createdDate
                    });
                  });
                }
              });
            }
          });
          
          this.availableChecklistItems = allItems;
          this.groupChecklistItemsByChecklist();
        } else {
          this.checklistItemsError = response.message || 'Failed to load checklist items';
        }
        this.loadingChecklistItems = false;
      },
      error: (err) => {
        this.checklistItemsError = 'Failed to load checklist items';
        this.loadingChecklistItems = false;
        console.error('Error loading checklist items:', err);
      }
    });
  }

  groupChecklistItemsByChecklist(): void {
    this.groupedChecklists = [];
    
    const checklistMap = new Map<string, {
      checklistId: string;
      checklistName: string;
      sections: Map<string, { sectionId: string; sectionName: string; items: PredefinedChecklistItem[] }>;
    }>();
    
    let itemsWithoutChecklist = 0;
    
    this.availableChecklistItems.forEach((item) => {
      if (!item.checklistSection?.checklist) {
        itemsWithoutChecklist++;
        return;
      }
      
      const checklist = item.checklistSection.checklist;
      const checklistId = checklist.checklistId;
      const sectionId = item.checklistSection.checklistSectionId;
      const sectionName = item.checklistSection.sectionName;
      
      // Initialize checklist if doesn't exist
      if (!checklistMap.has(checklistId)) {
        checklistMap.set(checklistId, {
          checklistId: checklistId,
          checklistName: checklist.checklistName,
          sections: new Map()
        });
      }
      
      const checklistGroup = checklistMap.get(checklistId)!;
      
      // Initialize section if doesn't exist
      if (!checklistGroup.sections.has(sectionId)) {
        checklistGroup.sections.set(sectionId, {
          sectionId: sectionId,
          sectionName: sectionName,
          items: []
        });
      }
      
      checklistGroup.sections.get(sectionId)!.items.push(item);
    });
    
    // Convert Map to Array for Angular template
    this.groupedChecklists = Array.from(checklistMap.values()).map(checklist => ({
      checklistId: checklist.checklistId,
      checklistName: checklist.checklistName,
      expanded: false, // Collapsed by default
      sections: Array.from(checklist.sections.values()).map(section => ({
        sectionId: section.sectionId,
        sectionName: section.sectionName,
        expanded: false, // Sections collapsed by default
        items: section.items.sort((a, b) => a.sequence - b.sequence)
      }))
    }));
  }

  toggleChecklist(checklist: any): void {
    checklist.expanded = !checklist.expanded;
  }

  toggleSection(section: any): void {
    section.expanded = !section.expanded;
  }

  expandAllChecklists(): void {
    this.groupedChecklists.forEach(c => c.expanded = true);
  }

  collapseAllChecklists(): void {
    this.groupedChecklists.forEach(c => c.expanded = false);
  }

  getTotalItemsInChecklist(checklist: any): number {
    return checklist.sections.reduce((sum: number, section: any) => sum + section.items.length, 0);
  }

  isChecklistItemSelected(itemId: string): boolean {
    return this.tempSelectedChecklistItems.some(item => item.predefinedChecklistItemId === itemId);
  }

  isChecklistItemMandatory(itemId: string): boolean {
    const item = this.tempSelectedChecklistItems.find(i => i.predefinedChecklistItemId === itemId);
    return item ? item.isMandatory : false;
  }

  toggleChecklistItem(checklistItem: PredefinedChecklistItem): void {
    const index = this.tempSelectedChecklistItems.findIndex(item => item.predefinedChecklistItemId === checklistItem.predefinedItemId);
    
    if (index > -1) {
      // Remove if already selected
      this.tempSelectedChecklistItems.splice(index, 1);
    } else {
      // Add if not selected
      this.tempSelectedChecklistItems.push({
        predefinedChecklistItemId: checklistItem.predefinedItemId,
        sequence: checklistItem.sequence,
        isMandatory: false
      });
    }
  }

  toggleChecklistItemMandatory(itemId: string): void {
    const item = this.tempSelectedChecklistItems.find(i => i.predefinedChecklistItemId === itemId);
    if (item) {
      item.isMandatory = !item.isMandatory;
    }
  }

  selectAllChecklistItems(): void {
    this.tempSelectedChecklistItems = this.availableChecklistItems.map(item => ({
      predefinedChecklistItemId: item.predefinedItemId,
      sequence: item.sequence,
      isMandatory: false
    }));
  }

  deselectAllChecklistItems(): void {
    this.tempSelectedChecklistItems = [];
  }

  getSelectedChecklistItemsCount(): number {
    return this.tempSelectedChecklistItems.length;
  }

  saveChecklistItems(): void {
    if (!this.selectedActivityForChecklist) return;
    
    // Update the activity's checklist items
    this.selectedActivityForChecklist.selectedChecklistItems = [...this.tempSelectedChecklistItems];
    this.selectedActivityForChecklist.selectedChecklistItemIds = this.tempSelectedChecklistItems.map(item => item.predefinedChecklistItemId);
    
    this.toastService.success(`Updated checklist items for "${this.selectedActivityForChecklist.activityName}"`);
    this.closeChecklistItemsModal();
  }

  trackByChecklistId(index: number, checklist: any): string {
    return checklist.checklistId;
  }

  // Team Assignment Methods
  openTeamAssignmentModal(activity: CreateActivityTemplateActivityDto): void {
    this.selectedActivityForTeam = activity;
    this.selectedTeamId = activity.assignedTeamId || null;
    this.showTeamAssignmentModal = true;
  }

  closeTeamAssignmentModal(): void {
    this.showTeamAssignmentModal = false;
    this.selectedActivityForTeam = null;
    this.selectedTeamId = null;
  }

  saveTeamAssignment(): void {
    if (!this.selectedActivityForTeam) return;
    
    this.selectedActivityForTeam.assignedTeamId = this.selectedTeamId || undefined;
    
    const teamName = this.availableTeams.find(t => t.teamId === this.selectedTeamId)?.teamName || 'No Team';
    this.toastService.success(`Team assignment updated for "${this.selectedActivityForTeam.activityName}"`);
    this.closeTeamAssignmentModal();
  }

  getActivityTeamName(activity: CreateActivityTemplateActivityDto): string {
    if (!activity.assignedTeamId) return 'No Team';
    const team = this.availableTeams.find(t => t.teamId === activity.assignedTeamId);
    return team ? team.teamName : 'Unknown Team';
  }

  navigateToManageChecklist(stage: any): void {
    // Save current template state to session storage
    const templateState = {
      templateId: this.templateId,
      isEditMode: this.isEditMode,
      templateForm: this.templateForm.value,
      stages: this.stages,
      numberOfStages: this.numberOfStages,
      stagesGenerated: this.stagesGenerated
    };
    sessionStorage.setItem('activityTemplateState', JSON.stringify(templateState));
    
    // Navigate to manage checklist with stage info
    this.router.navigate(['/schedule/activity-templates/manage-wir-checklist'], {
      queryParams: { stage: stage.stageName }
    });
  }

  getSelectedTeam(): Team | undefined {
    if (!this.selectedTeamId) return undefined;
    return this.availableTeams.find(t => t.teamId === this.selectedTeamId);
  }

  getSelectedTeamName(): string {
    const team = this.getSelectedTeam();
    return team?.teamName || '';
  }

  getSelectedTeamCode(): string {
    const team = this.getSelectedTeam();
    return team?.teamCode || '';
  }

  getSelectedTeamDepartment(): string {
    const team = this.getSelectedTeam();
    return team?.departmentName || '';
  }

  getSelectedTeamSize(): number | undefined {
    const team = this.getSelectedTeam();
    return team?.teamSize;
  }

  // Select all items in a specific checklist
  areAllChecklistItemsSelected(checklist: any): boolean {
    const allItems = checklist.sections.flatMap((s: any) => s.items);
    return allItems.length > 0 && allItems.every((item: any) => 
      this.tempSelectedChecklistItems.some(selected => selected.predefinedChecklistItemId === item.predefinedItemId)
    );
  }

  areSomeChecklistItemsSelected(checklist: any): boolean {
    const allItems = checklist.sections.flatMap((s: any) => s.items);
    const selectedCount = allItems.filter((item: any) => 
      this.tempSelectedChecklistItems.some(selected => selected.predefinedChecklistItemId === item.predefinedItemId)
    ).length;
    return selectedCount > 0 && selectedCount < allItems.length;
  }

  toggleSelectAllInChecklist(checklist: any): void {
    const allItems = checklist.sections.flatMap((s: any) => s.items);
    const allSelected = this.areAllChecklistItemsSelected(checklist);
    
    if (allSelected) {
      // Deselect all items in this checklist
      const itemIds = allItems.map((item: any) => item.predefinedItemId);
      this.tempSelectedChecklistItems = this.tempSelectedChecklistItems.filter(
        selected => !itemIds.includes(selected.predefinedChecklistItemId)
      );
    } else {
      // Select all items in this checklist
      allItems.forEach((item: any) => {
        if (!this.tempSelectedChecklistItems.some(selected => selected.predefinedChecklistItemId === item.predefinedItemId)) {
          this.tempSelectedChecklistItems.push({
            predefinedChecklistItemId: item.predefinedItemId,
            sequence: item.sequence,
            isMandatory: false
          });
        }
      });
    }
  }

  // Select all items in a specific section
  areAllSectionItemsSelected(section: any): boolean {
    return section.items.length > 0 && section.items.every((item: any) => 
      this.tempSelectedChecklistItems.some(selected => selected.predefinedChecklistItemId === item.predefinedItemId)
    );
  }

  areSomeSectionItemsSelected(section: any): boolean {
    const selectedCount = section.items.filter((item: any) => 
      this.tempSelectedChecklistItems.some(selected => selected.predefinedChecklistItemId === item.predefinedItemId)
    ).length;
    return selectedCount > 0 && selectedCount < section.items.length;
  }

  toggleSelectAllInSection(section: any): void {
    const allSelected = this.areAllSectionItemsSelected(section);
    
    if (allSelected) {
      // Deselect all items in this section
      const itemIds = section.items.map((item: any) => item.predefinedItemId);
      this.tempSelectedChecklistItems = this.tempSelectedChecklistItems.filter(
        selected => !itemIds.includes(selected.predefinedChecklistItemId)
      );
    } else {
      // Select all items in this section
      section.items.forEach((item: any) => {
        if (!this.tempSelectedChecklistItems.some(selected => selected.predefinedChecklistItemId === item.predefinedItemId)) {
          this.tempSelectedChecklistItems.push({
            predefinedChecklistItemId: item.predefinedItemId,
            sequence: item.sequence,
            isMandatory: false
          });
        }
      });
    }
  }
}
