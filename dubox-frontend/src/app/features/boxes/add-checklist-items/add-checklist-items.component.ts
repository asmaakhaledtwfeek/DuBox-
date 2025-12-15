import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { WIRService } from '../../../core/services/wir.service';
import { WIRCheckpoint, AddChecklistItemsRequest, PredefinedChecklistItemWithChecklistDto } from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

// Grouped structure for displaying items
interface ChecklistGroup {
  checklistName: string;
  checklistCode?: string;
  sections: SectionGroup[];
}

interface SectionGroup {
  sectionTitle: string;
  sectionOrder: number;
  items: PredefinedChecklistItemWithChecklistDto[];
}

@Component({
  selector: 'app-add-checklist-items',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './add-checklist-items.component.html',
  styleUrls: ['./add-checklist-items.component.scss']
})
export class AddChecklistItemsComponent implements OnInit {
  projectId!: string;
  boxId!: string;
  activityId!: string;
  wirId!: string;
  
  wirCheckpoint: WIRCheckpoint | null = null;
  checklistForm!: FormGroup;
  loading = true;
  error = '';
  submitting = false;
  
  // Predefined checklist items with grouping
  predefinedChecklistItems: PredefinedChecklistItemWithChecklistDto[] = [];
  groupedChecklists: ChecklistGroup[] = [];
  loadingPredefinedItems = false;
  selectedPredefinedItemIds: string[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private wirService: WIRService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['projectId'];
    this.boxId = this.route.snapshot.params['boxId'];
    this.activityId = this.route.snapshot.params['activityId'];
    this.wirId = this.route.snapshot.params['wirId'];
    
    this.initForm();
    this.loadWIRCheckpoint();
    this.loadPredefinedChecklistItems();
  }

  private initForm(): void {
    this.checklistForm = this.fb.group({});
  }

  loadWIRCheckpoint(): void {
    this.loading = true;
    this.error = '';
    
    this.wirService.getWIRCheckpointById(this.wirId).subscribe({
      next: (checkpoint) => {
        this.wirCheckpoint = checkpoint;
        this.loadPredefinedChecklistItems();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load WIR checkpoint';
        this.loading = false;
        console.error('Error loading WIR checkpoint:', err);
      }
    });
  }

  private loadPredefinedChecklistItems(): void {
    if (!this.wirId) {
      console.warn('No wirId available, skipping predefined items load');
      return;
    }
    
    console.log('Loading predefined items for wirId:', this.wirId);
    this.loadingPredefinedItems = true;
    this.wirService.getPredefinedChecklistItemsByCheckpointId(this.wirId).subscribe({
      next: (items) => {
        console.log('Received predefined items:', items);
        this.predefinedChecklistItems = items;
        this.filterAndGroupItems();
        this.loadingPredefinedItems = false;
      },
      error: (err) => {
        console.error('Error loading predefined checklist items:', err);
        this.error = 'Failed to load predefined checklist items';
        this.loadingPredefinedItems = false;
      }
    });
  }

  private filterAndGroupItems(): void {
    console.log('Filtering and grouping items. Total items:', this.predefinedChecklistItems.length);
    console.log('WIR Checkpoint:', this.wirCheckpoint);
    
    if (!this.predefinedChecklistItems.length) {
      console.log('No predefined items available');
      this.groupedChecklists = [];
      return;
    }

    if (!this.wirCheckpoint) {
      console.log('No checkpoint loaded yet, showing all items');
      this.groupedChecklists = this.groupItems(this.predefinedChecklistItems);
      return;
    }

    // Get IDs of items already added to this checkpoint
    const addedPredefinedIds = new Set(
      (this.wirCheckpoint.checklistItems || [])
        .map(item => item.predefinedItemId)
        .filter(id => id != null) as string[]
    );
    
    console.log('Already added item IDs:', Array.from(addedPredefinedIds));

    // Filter out items that are already added
    const availableItems = this.predefinedChecklistItems.filter(
      item => !addedPredefinedIds.has(item.predefinedItemId)
    );
    
    console.log('Available items after filtering:', availableItems.length);

    // Group the available items
    this.groupedChecklists = this.groupItems(availableItems);
    console.log('Grouped checklists:', this.groupedChecklists);
  }

  private groupItems(items: PredefinedChecklistItemWithChecklistDto[]): ChecklistGroup[] {
    console.log('Grouping items. Input count:', items.length);
    
    if (items.length === 0) {
      return [];
    }
    
    // Group by checklist first
    const checklistMap = new Map<string, Map<string, PredefinedChecklistItemWithChecklistDto[]>>();
    
    items.forEach(item => {
      const checklistKey = item.checklistId || 'ungrouped';
      const sectionKey = item.checklistSectionId || 'no-section';
      
      console.log(`Item: ${item.checkpointDescription?.substring(0, 50)}... | Checklist: ${checklistKey} | Section: ${sectionKey}`);
      
      if (!checklistMap.has(checklistKey)) {
        checklistMap.set(checklistKey, new Map());
      }
      
      const sectionMap = checklistMap.get(checklistKey)!;
      if (!sectionMap.has(sectionKey)) {
        sectionMap.set(sectionKey, []);
      }
      
      sectionMap.get(sectionKey)!.push(item);
    });
    
    console.log('Checklist map keys:', Array.from(checklistMap.keys()));
    
    // Convert to array structure
    const result: ChecklistGroup[] = [];
    
    checklistMap.forEach((sectionMap, checklistKey) => {
      const firstItem = Array.from(sectionMap.values())[0][0];
      const sections: SectionGroup[] = [];
      
      console.log(`Processing checklist: ${firstItem.checklistName} (${checklistKey})`);
      
      sectionMap.forEach((sectionItems, sectionKey) => {
        const firstSectionItem = sectionItems[0];
        console.log(`  Section: ${firstSectionItem.sectionTitle} - ${sectionItems.length} items`);
        sections.push({
          sectionTitle: firstSectionItem.sectionTitle || 'Uncategorized',
          sectionOrder: firstSectionItem.sectionOrder || 0,
          items: sectionItems.sort((a, b) => a.sequence - b.sequence)
        });
      });
      
      // Sort sections by order
      sections.sort((a, b) => a.sectionOrder - b.sectionOrder);
      
      result.push({
        checklistName: firstItem.checklistName || 'General Checklist',
        checklistCode: firstItem.checklistCode,
        sections
      });
    });
    
    console.log('Final grouped result:', result.length, 'checklists');
    return result;
  }

  togglePredefinedItemSelection(itemId: string): void {
    const index = this.selectedPredefinedItemIds.indexOf(itemId);
    if (index > -1) {
      this.selectedPredefinedItemIds.splice(index, 1);
    } else {
      this.selectedPredefinedItemIds.push(itemId);
    }
  }

  isPredefinedItemSelected(itemId: string): boolean {
    return this.selectedPredefinedItemIds.includes(itemId);
  }

  selectAllInChecklist(checklist: ChecklistGroup): void {
    checklist.sections.forEach(section => {
      section.items.forEach(item => {
        if (!this.isPredefinedItemSelected(item.predefinedItemId)) {
          this.selectedPredefinedItemIds.push(item.predefinedItemId);
        }
      });
    });
  }

  deselectAllInChecklist(checklist: ChecklistGroup): void {
    checklist.sections.forEach(section => {
      section.items.forEach(item => {
        const index = this.selectedPredefinedItemIds.indexOf(item.predefinedItemId);
        if (index > -1) {
          this.selectedPredefinedItemIds.splice(index, 1);
        }
      });
    });
  }

  areAllInChecklistSelected(checklist: ChecklistGroup): boolean {
    return checklist.sections.every(section =>
      section.items.every(item => this.isPredefinedItemSelected(item.predefinedItemId))
    );
  }

  selectAllInSection(section: SectionGroup): void {
    section.items.forEach(item => {
      if (!this.isPredefinedItemSelected(item.predefinedItemId)) {
        this.selectedPredefinedItemIds.push(item.predefinedItemId);
      }
    });
  }

  deselectAllInSection(section: SectionGroup): void {
    section.items.forEach(item => {
      const index = this.selectedPredefinedItemIds.indexOf(item.predefinedItemId);
      if (index > -1) {
        this.selectedPredefinedItemIds.splice(index, 1);
      }
    });
  }

  areAllInSectionSelected(section: SectionGroup): boolean {
    return section.items.every(item => this.isPredefinedItemSelected(item.predefinedItemId));
  }

  selectAll(): void {
    this.groupedChecklists.forEach(checklist => {
      this.selectAllInChecklist(checklist);
    });
  }

  deselectAll(): void {
    this.selectedPredefinedItemIds = [];
  }

  areAllSelected(): boolean {
    return this.groupedChecklists.every(checklist => this.areAllInChecklistSelected(checklist));
  }

  get hasItems(): boolean {
    return this.groupedChecklists.length > 0 && 
           this.groupedChecklists.some(c => c.sections.length > 0);
  }

  onSubmit(): void {
    if (!this.wirCheckpoint || this.selectedPredefinedItemIds.length === 0) {
      this.error = 'Please select at least one predefined checklist item to add.';
      return;
    }

    // Get the selected items details for preview
    const selectedItems = this.predefinedChecklistItems.filter(
      item => this.selectedPredefinedItemIds.includes(item.predefinedItemId)
    );

    console.log('Navigating back to Step 2 with selected items:', selectedItems);

    // Navigate back to qa-qc-checklist page, Step 2 (add-items), with selected items
    this.router.navigate([
      '/projects',
      this.projectId,
      'boxes',
      this.boxId,
      'activities',
      this.activityId,
      'qa-qc'
    ], { 
      queryParams: { 
        step: 'add-items',
        selectedItemIds: this.selectedPredefinedItemIds.join(',')
      }
    });
  }

  goBack(): void {
    // Navigate back to the qa-qc-checklist page
    this.router.navigate([
      '/projects',
      this.projectId,
      'boxes',
      this.boxId,
      'activities',
      this.activityId,
      'qa-qc'
    ]);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      if (control instanceof FormArray) {
        control.controls.forEach(item => {
          if (item instanceof FormGroup) {
            this.markFormGroupTouched(item);
          } else {
            item.markAsTouched();
          }
        });
      } else if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      } else {
        control?.markAsTouched();
      }
    });
  }
}

