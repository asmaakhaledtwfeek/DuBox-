import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { HeaderComponent } from '../../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../../shared/components/sidebar/sidebar.component';

interface ChecklistItem {
  checklistItemId: string;
  description: string;
  reference: string | null;
  order: number;
  checklistSectionId: string;
  selected?: boolean;
}

interface ChecklistSection {
  checklistSectionId: string;
  title: string;
  items: ChecklistItem[];
  expanded?: boolean;
  allSelected?: boolean;
}

interface ChecklistGroup {
  checklistId: string;
  name: string;
  code: string;
  sections: ChecklistSection[];
  expanded?: boolean;
  allSelected?: boolean;
}

@Component({
  selector: 'app-add-custom-activity',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './add-custom-activity.component.html',
  styleUrls: ['./add-custom-activity.component.scss']
})
export class AddCustomActivityComponent implements OnInit {
  customActivityForm!: FormGroup;
  availableChecklists: ChecklistGroup[] = [];
  selectedChecklistItemIds: Set<string> = new Set();
  loadingChecklists = false;
  checklistError: string | null = null;
  
  stageName: string = '';
  templateId: string = '';
  nextSequenceInStage: number = 1;
  nextOverallSequence: number = 1;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    // Get data from route parameters
    this.route.queryParams.subscribe(params => {
      this.stageName = params['stage'] || '';
      this.templateId = params['templateId'] || '';
      this.nextSequenceInStage = parseInt(params['nextSequenceInStage']) || 1;
      this.nextOverallSequence = parseInt(params['nextOverallSequence']) || 1;
    });

    // Initialize form
    this.initializeForm();

    // Load checklists for the stage
    if (this.stageName) {
      this.loadChecklistsForStage(this.stageName);
    }
  }

  initializeForm(): void {
    this.customActivityForm = this.fb.group({
      activityCode: ['', Validators.required],
      activityName: ['', Validators.required],
      estimatedDurationDays: [1, [Validators.required, Validators.min(1)]],
      sequenceInStage: [this.nextSequenceInStage, [Validators.required, Validators.min(1)]],
      overallSequence: [this.nextOverallSequence, [Validators.required, Validators.min(1)]],
      description: [''],
      isWIRCheckpoint: [false]
    });
  }

  loadChecklistsForStage(stage: string): void {
    // Extract WIR code from stage (e.g., "Stage 01" -> "WIR-01")
    const stageNumber = stage.replace(/\D/g, '');
    const wirCode = `WIR-${stageNumber.padStart(2, '0')}`;
    
    this.loadingChecklists = true;
    this.checklistError = null;
    this.availableChecklists = [];
    this.selectedChecklistItemIds.clear();
    
    // Fetch checklists with full details by WIR code
    this.http.get<any>(`${environment.apiUrl}/Checklists/by-wir-code/${wirCode}`).subscribe({
      next: (response) => {
        this.loadingChecklists = false;
        
        if (response.isSuccess && response.data && response.data.length > 0) {
          this.availableChecklists = response.data.map((checklist: any) => ({
            checklistId: checklist.checklistId,
            name: checklist.name,
            code: checklist.code,
            expanded: false,
            allSelected: false,
            sections: (checklist.sections || []).map((section: any) => ({
              checklistSectionId: section.checklistSectionId,
              title: section.title,
              expanded: false,
              allSelected: false,
              items: (section.items || []).map((item: any) => ({
                checklistItemId: item.checklistItemId,
                description: item.description,
                reference: item.reference,
                order: item.order,
                checklistSectionId: section.checklistSectionId,
                selected: false
              }))
            }))
          }));
          this.checklistError = null;
        } else {
          this.checklistError = 'No checklists found for this stage. Please add a checklist to the stage first.';
        }
      },
      error: (error) => {
        this.loadingChecklists = false;
        console.error('Error loading checklists:', error);
        this.checklistError = 'Failed to load checklists. Please try again.';
      }
    });
  }

  // Toggle checklist expansion
  toggleChecklistExpansion(checklistId: string): void {
    const checklist = this.availableChecklists.find(c => c.checklistId === checklistId);
    if (checklist) {
      checklist.expanded = !checklist.expanded;
    }
  }

  // Toggle section expansion
  toggleSectionExpansion(checklistId: string, sectionId: string): void {
    const checklist = this.availableChecklists.find(c => c.checklistId === checklistId);
    if (checklist) {
      const section = checklist.sections.find(s => s.checklistSectionId === sectionId);
      if (section) {
        section.expanded = !section.expanded;
      }
    }
  }

  // Toggle individual item selection
  toggleItemSelection(itemId: string): void {
    if (this.selectedChecklistItemIds.has(itemId)) {
      this.selectedChecklistItemIds.delete(itemId);
    } else {
      this.selectedChecklistItemIds.add(itemId);
    }
    
    // Update selection state in the data structure
    this.availableChecklists.forEach(checklist => {
      checklist.sections.forEach(section => {
        const item = section.items.find(i => i.checklistItemId === itemId);
        if (item) {
          item.selected = this.selectedChecklistItemIds.has(itemId);
        }
        // Update section's allSelected state
        section.allSelected = section.items.every(i => i.selected);
      });
      // Update checklist's allSelected state
      checklist.allSelected = checklist.sections.every(s => s.allSelected);
    });
  }

  // Select all items in a section
  selectAllInSection(checklistId: string, sectionId: string): void {
    const checklist = this.availableChecklists.find(c => c.checklistId === checklistId);
    if (checklist) {
      const section = checklist.sections.find(s => s.checklistSectionId === sectionId);
      if (section) {
        const allSelected = section.items.every(item => item.selected);
        section.items.forEach(item => {
          item.selected = !allSelected;
          if (!allSelected) {
            this.selectedChecklistItemIds.add(item.checklistItemId);
          } else {
            this.selectedChecklistItemIds.delete(item.checklistItemId);
          }
        });
        section.allSelected = !allSelected;
        
        // Update checklist's allSelected state
        checklist.allSelected = checklist.sections.every(s => s.allSelected);
      }
    }
  }

  // Select all items in a checklist
  selectAllInChecklist(checklistId: string): void {
    const checklist = this.availableChecklists.find(c => c.checklistId === checklistId);
    if (checklist) {
      const allSelected = checklist.sections.every(s => s.items.every(i => i.selected));
      
      checklist.sections.forEach(section => {
        section.items.forEach(item => {
          item.selected = !allSelected;
          if (!allSelected) {
            this.selectedChecklistItemIds.add(item.checklistItemId);
          } else {
            this.selectedChecklistItemIds.delete(item.checklistItemId);
          }
        });
        section.allSelected = !allSelected;
      });
      checklist.allSelected = !allSelected;
    }
  }

  // Select all items in all checklists
  selectAllChecklists(): void {
    this.availableChecklists.forEach(checklist => {
      checklist.sections.forEach(section => {
        section.items.forEach(item => {
          item.selected = true;
          this.selectedChecklistItemIds.add(item.checklistItemId);
        });
        section.allSelected = true;
      });
      checklist.allSelected = true;
    });
  }

  // Deselect all items
  deselectAllChecklists(): void {
    this.availableChecklists.forEach(checklist => {
      checklist.sections.forEach(section => {
        section.items.forEach(item => {
          item.selected = false;
        });
        section.allSelected = false;
      });
      checklist.allSelected = false;
    });
    this.selectedChecklistItemIds.clear();
  }

  // Get total item count for a checklist
  getChecklistItemCount(checklist: ChecklistGroup): number {
    return checklist.sections.reduce((total, section) => total + section.items.length, 0);
  }

  // Get selected item count for a checklist
  getChecklistSelectedCount(checklist: ChecklistGroup): number {
    return checklist.sections.reduce((total, section) => 
      total + section.items.filter(i => i.selected).length, 0);
  }

  // Check if form field is invalid
  isFieldInvalid(fieldName: string): boolean {
    const field = this.customActivityForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  // Check if can add activity
  canAddActivity(): boolean {
    return this.customActivityForm.valid && this.availableChecklists.length > 0;
  }

  // Save and return to template page
  saveActivity(): void {
    if (!this.canAddActivity()) {
      // Mark all fields as touched to show validation errors
      Object.keys(this.customActivityForm.controls).forEach(key => {
        this.customActivityForm.get(key)?.markAsTouched();
      });
      return;
    }

    const formValue = this.customActivityForm.value;
    const selectedItemIdsArray = Array.from(this.selectedChecklistItemIds);

    // Prepare activity data
    const activityData = {
      activityTemplateActivityId: null,
      sourceActivityMasterId: null,
      isCustomActivity: true,
      activityCode: formValue.activityCode,
      activityName: formValue.activityName,
      description: formValue.description,
      estimatedDurationDays: formValue.estimatedDurationDays,
      sequenceInStage: formValue.sequenceInStage,
      overallSequence: formValue.overallSequence,
      stage: this.stageName,
      isWIRCheckpoint: formValue.isWIRCheckpoint,
      isActive: true,
      isMandatory: true,
      applicableBoxTypes: [],
      dependsOnActivities: [],
      selectedChecklistItemIds: selectedItemIdsArray
    };
console.log('AcitivityData', activityData)
    // Store in session storage to avoid router state issues
    sessionStorage.setItem('newCustomActivity', JSON.stringify(activityData));
    
    // Navigate back to the correct route (create or edit mode)
    const route = this.templateId 
      ? `/schedule/activity-templates/${this.templateId}/edit`
      : '/schedule/activity-templates/create';
    
    this.router.navigate([route]);
  }

  // Cancel and return
  cancel(): void {
    // Navigate back to the correct route (create or edit mode)
    const route = this.templateId 
      ? `/schedule/activity-templates/${this.templateId}/edit`
      : '/schedule/activity-templates/create';
    
    this.router.navigate([route]);
  }
}
