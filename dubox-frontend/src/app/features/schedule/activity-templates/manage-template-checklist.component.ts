import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ActivityTemplateService } from '../../../core/services/activity-template.service';
import { ToastService } from '../../../core/services/toast.service';
import { 
  ActivityTemplate,
  ActivityTemplateActivity,
  PredefinedChecklistItem,
  Checklist,
  ChecklistSection
} from '../../../core/models/activity-template.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

interface StageWithActivities {
  stageNumber: number;
  stageName: string;
  wirActivity: ActivityTemplateActivity | null;
  activities: ActivityTemplateActivity[];
  expanded: boolean;
}

interface ChecklistWithSelection extends Checklist {
  items?: PredefinedChecklistItemWithSelection[];
  expanded?: boolean;
}

interface PredefinedChecklistItemWithSelection extends PredefinedChecklistItem {
  selected?: boolean;
}

@Component({
  selector: 'app-manage-template-checklist',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './manage-template-checklist.component.html',
  styleUrl: './manage-template-checklist.component.scss'
})
export class ManageTemplateChecklistComponent implements OnInit {
  templateId: string | null = null;
  template: ActivityTemplate | null = null;
  stages: StageWithActivities[] = [];
  
  // Checklist modal
  showChecklistModal = false;
  selectedActivityForChecklist: ActivityTemplateActivity | null = null;
  selectedStageForChecklist: StageWithActivities | null = null;
  isStageWIRChecklist = false;
  
  availableChecklists: ChecklistWithSelection[] = [];
  filteredChecklists: ChecklistWithSelection[] = [];
  checklistSearchTerm = '';
  
  // Loading states
  loading = false;
  loadingTemplate = false;
  loadingChecklists = false;
  savingChecklists = false;
  error: string | null = null;
  
  // Activity checklist assignments (in-memory)
  activityChecklistAssignments: Map<string, string[]> = new Map(); // activityId -> checklistItemIds[]

  constructor(
    private templateService: ActivityTemplateService,
    private toastService: ToastService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.templateId = this.route.snapshot.paramMap.get('id');
    if (this.templateId) {
      this.loadTemplate();
      this.loadChecklists();
    } else {
      this.error = 'Template ID not found';
    }
  }

  loadTemplate(): void {
    if (!this.templateId) return;
    
    this.loadingTemplate = true;
    this.error = null;

    this.templateService.getTemplateById(this.templateId).subscribe({
      next: (template) => {
        this.template = template;
        this.organizeStagesAndActivities();
        this.loadingTemplate = false;
      },
      error: (err) => {
        this.error = 'Failed to load template';
        this.loadingTemplate = false;
        console.error('Error loading template:', err);
      }
    });
  }

  organizeStagesAndActivities(): void {
    if (!this.template?.activities) return;
    
    // Group activities by stage
    const stageMap = new Map<number, ActivityTemplateActivity[]>();
    
    this.template.activities.forEach(activity => {
      if (!stageMap.has(activity.stageNumber)) {
        stageMap.set(activity.stageNumber, []);
      }
      stageMap.get(activity.stageNumber)!.push(activity);
    });
    
    // Create stage objects
    this.stages = [];
    for (let i = 1; i <= this.template.stageCount; i++) {
      const activities = stageMap.get(i) || [];
      const wirActivity = activities.find(a => a.isWIRCheckpoint) || null;
      
      this.stages.push({
        stageNumber: i,
        stageName: `Stage ${i.toString().padStart(2, '0')}`,
        wirActivity: wirActivity,
        activities: activities,
        expanded: true
      });
    }
  }

  loadChecklists(): void {
    this.loadingChecklists = true;

    this.templateService.getAllChecklists().subscribe({
      next: (checklists) => {
        this.availableChecklists = checklists || [];
        this.filteredChecklists = this.availableChecklists;
        this.loadingChecklists = false;
      },
      error: (err) => {
        this.toastService.error('Failed to load checklists');
        this.loadingChecklists = false;
        console.error('Error loading checklists:', err);
      }
    });
  }

  toggleStage(stage: StageWithActivities): void {
    stage.expanded = !stage.expanded;
  }

  openStageWIRChecklistModal(stage: StageWithActivities): void {
    if (!stage.wirActivity) {
      this.toastService.warning('This stage does not have a WIR activity');
      return;
    }
    
    this.selectedStageForChecklist = stage;
    this.selectedActivityForChecklist = stage.wirActivity;
    this.isStageWIRChecklist = true;
    this.showChecklistModal = true;
    this.checklistSearchTerm = '';
    
    // If WIR code exists, filter checklists
    if (stage.wirActivity.wirCode) {
      this.loadChecklistsByWirCode(stage.wirActivity.wirCode);
    } else {
      this.filterChecklists();
    }
  }

  openActivityChecklistModal(activity: ActivityTemplateActivity): void {
    this.selectedActivityForChecklist = activity;
    this.selectedStageForChecklist = null;
    this.isStageWIRChecklist = false;
    this.showChecklistModal = true;
    this.checklistSearchTerm = '';
    this.filterChecklists();
  }

  closeChecklistModal(): void {
    this.showChecklistModal = false;
    this.selectedActivityForChecklist = null;
    this.selectedStageForChecklist = null;
    this.isStageWIRChecklist = false;
  }

  loadChecklistsByWirCode(wirCode: string): void {
    this.loadingChecklists = true;

    this.templateService.getChecklistsByWirCode(wirCode).subscribe({
      next: (checklists) => {
        this.availableChecklists = checklists || [];
        this.filteredChecklists = this.availableChecklists;
        this.loadingChecklists = false;
      },
      error: (err) => {
        this.toastService.error('Failed to load checklists');
        this.loadingChecklists = false;
        console.error('Error loading checklists:', err);
      }
    });
  }

  filterChecklists(): void {
    if (!this.checklistSearchTerm) {
      this.filteredChecklists = this.availableChecklists;
      return;
    }

    const searchLower = this.checklistSearchTerm.toLowerCase();
    this.filteredChecklists = this.availableChecklists.filter(checklist =>
      checklist.checklistName.toLowerCase().includes(searchLower) ||
      (checklist.wirCode && checklist.wirCode.toLowerCase().includes(searchLower))
    );
  }

  toggleChecklistExpansion(checklist: ChecklistWithSelection): void {
    checklist.expanded = !checklist.expanded;
    
    // Load checklist items if not already loaded
    if (checklist.expanded && (!checklist.items || checklist.items.length === 0)) {
      this.loadChecklistItems(checklist);
    }
  }

  loadChecklistItems(checklist: ChecklistWithSelection): void {
    // In a real implementation, this would load checklist items from the backend
    // For now, we'll use the sections if they exist
    if (checklist.sections && checklist.sections.length > 0) {
      checklist.items = [];
      checklist.sections.forEach(section => {
        if (section.items) {
          section.items.forEach(item => {
            checklist.items!.push({
              ...item,
              selected: false
            });
          });
        }
      });
    }
  }

  toggleChecklistItemSelection(item: PredefinedChecklistItemWithSelection): void {
    item.selected = !item.selected;
  }

  selectAllInChecklist(checklist: ChecklistWithSelection): void {
    if (checklist.items) {
      const allSelected = checklist.items.every(item => item.selected);
      checklist.items.forEach(item => {
        item.selected = !allSelected;
      });
    }
  }

  getSelectedChecklistItemsCount(checklist: ChecklistWithSelection): number {
    if (!checklist.items) return 0;
    return checklist.items.filter(item => item.selected).length;
  }

  saveChecklistSelection(): void {
    if (!this.selectedActivityForChecklist) return;
    
    const selectedItemIds: string[] = [];
    
    this.filteredChecklists.forEach(checklist => {
      if (checklist.items) {
        checklist.items.forEach(item => {
          if (item.selected) {
            selectedItemIds.push(item.predefinedItemId);
          }
        });
      }
    });
    
    if (selectedItemIds.length === 0) {
      this.toastService.warning('Please select at least one checklist item');
      return;
    }
    
    this.savingChecklists = true;
    
    // Save to backend
    this.templateService.assignChecklistItemsToActivity(
      this.selectedActivityForChecklist.activityTemplateActivityId,
      selectedItemIds
    ).subscribe({
      next: () => {
        // Store in local map for display
        this.activityChecklistAssignments.set(
          this.selectedActivityForChecklist!.activityTemplateActivityId,
          selectedItemIds
        );
        
        this.savingChecklists = false;
        this.toastService.success(`Assigned ${selectedItemIds.length} checklist item(s)`);
        this.closeChecklistModal();
      },
      error: (err) => {
        this.savingChecklists = false;
        this.toastService.error('Failed to assign checklist items');
        console.error('Error assigning checklist items:', err);
      }
    });
  }

  getActivityChecklistCount(activityId: string): number {
    const items = this.activityChecklistAssignments.get(activityId);
    return items ? items.length : 0;
  }

  hasActivityChecklist(activityId: string): boolean {
    return this.activityChecklistAssignments.has(activityId) && 
           this.activityChecklistAssignments.get(activityId)!.length > 0;
  }

  finishAndNavigate(): void {
    if (!this.template) return;
    
    // Show success message
    const totalActivities = this.stages.reduce((sum, stage) => sum + stage.activities.length, 0);
    const activitiesWithChecklists = Array.from(this.activityChecklistAssignments.keys()).length;
    
    this.toastService.success(
      `Activity Template created successfully! Assigned checklists to ${activitiesWithChecklists} out of ${totalActivities} activities.`
    );
    
    // Navigate to template list or detail view
    this.router.navigate(['/schedule/activity-templates']);
  }

  skipChecklistManagement(): void {
    this.toastService.info('Checklist management skipped. You can manage checklists later from the template details page.');
    this.router.navigate(['/schedule/activity-templates']);
  }

  trackByIndex(index: number): number {
    return index;
  }

  trackByActivityId(index: number, activity: ActivityTemplateActivity): string {
    return activity.activityTemplateActivityId;
  }

  trackByChecklistId(index: number, checklist: ChecklistWithSelection): string {
    return checklist.checklistId;
  }
}
