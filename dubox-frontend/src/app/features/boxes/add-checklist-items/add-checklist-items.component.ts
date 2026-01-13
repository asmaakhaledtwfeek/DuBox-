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
  checklistId?: string;
  sections: SectionGroup[];
  isExpanded?: boolean;
}

interface SectionGroup {
  sectionTitle: string;
  sectionOrder: number;
  sectionId?: string;
  items: PredefinedChecklistItemWithChecklistDto[];
  isExpanded?: boolean;
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
  
  // Collapsible state - single open index for accordion behavior
  openChecklistIndex: number | null = 0; // First one open by default
  openSectionIndexMap: { [checklistIndex: number]: number | null } = {};

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
          sectionId: firstSectionItem.checklistSectionId || sectionKey,
          items: sectionItems.sort((a, b) => a.sequence - b.sequence)
        });
      });
      
      // Sort sections by order
      sections.sort((a, b) => a.sectionOrder - b.sectionOrder);
      
      result.push({
        checklistName: firstItem.checklistName || 'General Checklist',
        checklistCode: firstItem.checklistCode,
        checklistId: firstItem.checklistId || checklistKey,
        sections
      });
    });
    
    console.log('Final grouped result:', result.length, 'checklists');
    return result;
  }

  toggleChecklistExpanded(index: number, event?: Event): void {
    if (event) {
      event.stopPropagation();
    }
    
    // Accordion behavior: if clicking the open one, close it; otherwise open the clicked one
    if (this.openChecklistIndex === index) {
      this.openChecklistIndex = null;
    } else {
      this.openChecklistIndex = index;
      // Initialize section map for this checklist if not exists
      if (this.openSectionIndexMap[index] === undefined) {
        this.openSectionIndexMap[index] = null;
      }
    }
  }

  toggleSectionExpanded(checklistIndex: number, sectionIndex: number, event?: Event): void {
    if (event) {
      event.stopPropagation();
    }
    
    // Accordion behavior for sections within a checklist
    if (this.openSectionIndexMap[checklistIndex] === sectionIndex) {
      this.openSectionIndexMap[checklistIndex] = null;
    } else {
      this.openSectionIndexMap[checklistIndex] = sectionIndex;
    }
  }

  isChecklistExpanded(index: number): boolean {
    return this.openChecklistIndex === index;
  }

  isSectionExpanded(checklistIndex: number, sectionIndex: number): boolean {
    return this.openSectionIndexMap[checklistIndex] === sectionIndex;
  }

  expandAllChecklists(): void {
    // Open all checklists (set to -1 to indicate all open)
    this.openChecklistIndex = -1;
    this.groupedChecklists.forEach((checklist, checklistIndex) => {
      this.openSectionIndexMap[checklistIndex] = -1; // All sections open
    });
  }

  collapseAllChecklists(): void {
    this.openChecklistIndex = null;
    this.openSectionIndexMap = {};
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

    this.loading = true;
    this.error = '';

    const request = {
      wirId: this.wirCheckpoint.wirId,
      predefinedItemIds: this.selectedPredefinedItemIds
    };

    console.log('Saving selected checklist items:', request);

    this.wirService.addChecklistItems(request).subscribe({
      next: (updatedCheckpoint) => {
        console.log('Checklist items added successfully:', updatedCheckpoint);
        this.loading = false;

        // Show success message
        setTimeout(() => {
          document.dispatchEvent(new CustomEvent('app-toast', { 
            detail: { message: 'Checklist items added successfully!', type: 'success' } 
          }));
        }, 0);

        // Navigate back to qa-qc-checklist page, Step 2 (add-items)
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
            refresh: Date.now() // Add timestamp to force refresh
          }
        });
      },
      error: (error) => {
        console.error('Error adding checklist items:', error);
        this.loading = false;
        this.error = error?.error?.message || 'Failed to add checklist items. Please try again.';
        
        // Show error toast
        setTimeout(() => {
          document.dispatchEvent(new CustomEvent('app-toast', { 
            detail: { message: this.error, type: 'error' } 
          }));
        }, 0);
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

