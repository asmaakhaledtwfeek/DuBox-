import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ActivityTemplateService } from '../../../core/services/activity-template.service';
import { ActivityTemplate } from '../../../core/models/activity-template.model';
import { ChecklistItemsModalComponent } from '../../../shared/components/checklist-items-modal/checklist-items-modal.component';
import { ModernAlertComponent, AlertType } from '../../../shared/components/modern-alert/modern-alert.component';

interface SelectedChecklistItem {
  predefinedChecklistItemId: string;
  sequence: number;
  isMandatory: boolean;
}

interface ActivityStage {
  stageNumber: number;
  stageName: string;
  activities: ActivityInfo[];
  isExpanded: boolean;
}

interface ActivityInfo {
  activityTemplateActivityId: string;
  activityCode: string;
  activityName: string;
  description?: string;
  estimatedDurationDays: number;
  isWIRCheckpoint: boolean;
  wirCode?: string;
  sequenceInStage: number;
  overallSequence: number;
  isCustomActivity: boolean;
}

@Component({
  selector: 'app-activity-template-checklist',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent, ChecklistItemsModalComponent, ModernAlertComponent],
  templateUrl: './activity-template-checklist.component.html',
  styleUrls: ['./activity-template-checklist.component.scss']
})
export class ActivityTemplateChecklistComponent implements OnInit {
  templateId: string = '';
  template: ActivityTemplate | null = null;
  stages: ActivityStage[] = [];
  loading = true;
  error = '';
  
  // Modal state
  showManageChecklistModal = false;
  selectedActivity: ActivityInfo | null = null;
  existingChecklistItems: SelectedChecklistItem[] = [];
  savingChecklistItems = false;
  
  // Alert state
  showAlert = false;
  alertType: AlertType = 'info';
  alertTitle = '';
  alertMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private activityTemplateService: ActivityTemplateService
  ) {}

  ngOnInit(): void {
    this.templateId = this.route.snapshot.paramMap.get('templateId') || '';
    if (this.templateId) {
      this.loadTemplateDetails();
    } else {
      this.error = 'Invalid template ID';
      this.loading = false;
    }
  }

  loadTemplateDetails(): void {
    this.loading = true;
    this.error = '';

    this.activityTemplateService.getTemplateById(this.templateId).subscribe({
      next: (template) => {
        this.template = template;
        this.stages = this.groupActivitiesByStage(template.activities || []);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading template details:', err);
        this.error = err.message || 'Failed to load template details';
        this.loading = false;
      }
    });
  }

  private groupActivitiesByStage(activities: any[]): ActivityStage[] {
    const stageMap = new Map<number, ActivityStage>();

    activities.forEach(activity => {
      const stageNumber = activity.stageNumber;
      
      if (!stageMap.has(stageNumber)) {
        stageMap.set(stageNumber, {
          stageNumber,
          stageName: activity.stage || `Stage ${stageNumber}`,
          activities: [],
          isExpanded: false // Changed to false to make stages collapsed by default
        });
      }

      const stage = stageMap.get(stageNumber)!;
      stage.activities.push({
        activityTemplateActivityId: activity.activityTemplateActivityId,
        activityCode: activity.activityCode,
        activityName: activity.activityName,
        description: activity.description,
        estimatedDurationDays: activity.estimatedDurationDays,
        isWIRCheckpoint: activity.isWIRCheckpoint,
        wirCode: activity.wirCode,
        sequenceInStage: activity.sequenceInStage,
        overallSequence: activity.overallSequence,
        isCustomActivity: activity.isCustomActivity
      });
    });

    // Sort stages by stage number and activities by sequence
    const stages = Array.from(stageMap.values()).sort((a, b) => a.stageNumber - b.stageNumber);
    stages.forEach(stage => {
      stage.activities.sort((a, b) => a.sequenceInStage - b.sequenceInStage);
    });

    return stages;
  }

  toggleStage(stage: ActivityStage): void {
    stage.isExpanded = !stage.isExpanded;
  }

  expandAll(): void {
    this.stages.forEach(s => s.isExpanded = true);
  }

  collapseAll(): void {
    this.stages.forEach(s => s.isExpanded = false);
  }

  getTotalActivities(): number {
    return this.stages.reduce((total, stage) => total + stage.activities.length, 0);
  }

  getTotalWIRActivities(): number {
    let count = 0;
    this.stages.forEach(stage => {
      count += stage.activities.filter(a => a.isWIRCheckpoint).length;
    });
    return count;
  }

  manageStageChecklist(stage: ActivityStage): void {
    // Format stage number with leading zero (e.g., "Stage 01", "Stage 02")
    const stageNumberFormatted = stage.stageNumber.toString().padStart(2, '0');
    const stageName = `Stage ${stageNumberFormatted}`;
    
    // Navigate to manage-wir-checklist with stage and templateId parameters
    this.router.navigate(['/schedule/activity-templates/manage-wir-checklist'], {
      queryParams: { 
        stage: stageName,
        templateId: this.templateId,
        source: 'activity-template-checklist'
      }
    });
  }

  openManageChecklistModal(activity: ActivityInfo): void {
    this.selectedActivity = activity;
    this.loadExistingChecklistItems(activity.activityTemplateActivityId);
  }

  loadExistingChecklistItems(activityId: string): void {
    this.activityTemplateService.getChecklistItemsForTemplateActivity(activityId).subscribe({
      next: (items) => {
        this.existingChecklistItems = (items || []).map((item: any) => ({
          predefinedChecklistItemId: item.predefinedChecklistItemId || item.checklistItemId,
          sequence: item.sequence || 0,
          isMandatory: item.isMandatory || false
        }));
        this.showManageChecklistModal = true;
      },
      error: (err) => {
        console.error('Error loading existing checklist items:', err);
        this.existingChecklistItems = [];
        this.showManageChecklistModal = true;
      }
    });
  }

  closeManageChecklistModal(): void {
    this.showManageChecklistModal = false;
    this.selectedActivity = null;
  }

  onSaveChecklistItems(selectedItems: SelectedChecklistItem[]): void {
    if (!this.selectedActivity) {
      return;
    }

    this.savingChecklistItems = true;

    const selectedItemIds = selectedItems.map(item => item.predefinedChecklistItemId);

    this.activityTemplateService.assignChecklistItemsToActivity(
      this.selectedActivity.activityTemplateActivityId,
      selectedItemIds
    ).subscribe({
      next: (response) => {
        this.savingChecklistItems = false;
        console.log('Successfully saved checklist items:', response);
        const message = selectedItems.length === 0 
          ? `All checklist items removed from "${this.selectedActivity?.activityName}"`
          : `Successfully assigned ${selectedItems.length} checklist item(s) to "${this.selectedActivity?.activityName}"`;
        this.showNotification(
          'success', 
          'Success!', 
          message
        );
        this.closeManageChecklistModal();
        // Optionally reload template details to show updated data
        // this.loadTemplateDetails();
      },
      error: (err) => {
        this.savingChecklistItems = false;
        console.error('Error saving checklist items:', err);
        const errorMessage = err.error?.message || err.error?.error || 'Failed to save checklist items. Please try again.';
        this.showNotification('error', 'Error', errorMessage);
      }
    });
  }

  showNotification(type: AlertType, title: string, message: string): void {
    this.alertType = type;
    this.alertTitle = title;
    this.alertMessage = message;
    this.showAlert = true;
  }

  closeAlert(): void {
    this.showAlert = false;
  }

  goBack(): void {
    this.router.navigate(['/qc/predefined-checklists']);
  }
}
