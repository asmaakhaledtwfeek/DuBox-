import { Component, EventEmitter, Input, OnInit, OnChanges, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { IndeterminateDirective } from '../../directives/indeterminate.directive';

interface PredefinedChecklistItem {
  predefinedItemId: string;
  checkpointDescription: string;
  sequence: number;
  reference?: string;
  description?: string;
}

interface SelectedChecklistItem {
  predefinedChecklistItemId: string;
  sequence: number;
  isMandatory: boolean;
}

interface ChecklistSection {
  sectionId: string;
  sectionName: string;
  expanded: boolean;
  items: PredefinedChecklistItem[];
}

interface GroupedChecklist {
  checklistId: string;
  checklistName: string;
  expanded: boolean;
  sections: ChecklistSection[];
}

@Component({
  selector: 'app-checklist-items-modal',
  standalone: true,
  imports: [CommonModule, IndeterminateDirective],
  templateUrl: './checklist-items-modal.component.html',
  styleUrls: ['./checklist-items-modal.component.scss']
})
export class ChecklistItemsModalComponent implements OnInit, OnChanges {
  @Input() show: boolean = false;
  @Input() activityName: string = '';
  @Input() activityId: string = '';
  @Input() existingSelectedItems: SelectedChecklistItem[] = [];
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<SelectedChecklistItem[]>();

  availableChecklistItems: PredefinedChecklistItem[] = [];
  groupedChecklists: GroupedChecklist[] = [];
  tempSelectedChecklistItems: SelectedChecklistItem[] = [];
  loadingChecklistItems = false;
  checklistItemsError: string | null = null;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    if (this.show) {
      this.initializeModal();
    }
  }

  ngOnChanges(): void {
    if (this.show) {
      this.initializeModal();
    }
  }

  initializeModal(): void {
    this.tempSelectedChecklistItems = this.existingSelectedItems ? [...this.existingSelectedItems] : [];
    console.log('initializeModal - existingSelectedItems:', this.existingSelectedItems);
    console.log('initializeModal - tempSelectedChecklistItems:', this.tempSelectedChecklistItems);
    this.loadChecklistItems();
  }

  loadChecklistItems(): void {
    this.loadingChecklistItems = true;
    this.checklistItemsError = null;

    console.log('Loading checklist items. Current tempSelectedChecklistItems:', this.tempSelectedChecklistItems);

    this.http.get<any>(`${environment.apiUrl}/checklists`).subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.availableChecklistItems = [];
          this.groupedChecklists = [];

          response.data.forEach((checklist: any) => {
            const sections: ChecklistSection[] = [];

            (checklist.sections || []).forEach((section: any) => {
              const items: PredefinedChecklistItem[] = (section.items || []).map((item: any) => {
                const mappedItem = {
                  predefinedItemId: item.predefinedChecklistItemId || item.checklistItemId || item.predefinedItemId,
                  checkpointDescription: item.description,
                  description: item.description,
                  sequence: item.order || item.sequence || 0,
                  reference: item.reference
                };
                console.log('Mapped item:', mappedItem.predefinedItemId, 'from raw:', item);
                return mappedItem;
              });

              this.availableChecklistItems.push(...items);

              sections.push({
                sectionId: section.checklistSectionId,
                sectionName: section.title,
                expanded: this.hasSectionSelectedItems(items),
                items: items
              });
            });

            this.groupedChecklists.push({
              checklistId: checklist.checklistId,
              checklistName: checklist.name,
              expanded: sections.some(s => s.expanded),
              sections: sections
            });
          });

          this.loadingChecklistItems = false;
          
          // Debug: Check which items should be selected
          console.log('Loaded checklists. Checking selections...');
          const selectedIds = this.tempSelectedChecklistItems.map(x => x.predefinedChecklistItemId);
          console.log('Selected IDs from tempSelectedChecklistItems:', selectedIds);
          const availableIds = this.availableChecklistItems.map(x => x.predefinedItemId);
          console.log('Available item IDs:', availableIds);
          const matches = selectedIds.filter(id => availableIds.includes(id));
          console.log('Matching IDs:', matches.length, 'out of', selectedIds.length);
        }
      },
      error: (err) => {
        console.error('Error loading checklist items:', err);
        this.checklistItemsError = 'Failed to load checklist items';
        this.loadingChecklistItems = false;
      }
    });
  }

  hasSectionSelectedItems(items: PredefinedChecklistItem[]): boolean {
    return items.some(item => 
      this.tempSelectedChecklistItems.some(selected => 
        selected.predefinedChecklistItemId === item.predefinedItemId
      )
    );
  }

  toggleChecklist(checklist: GroupedChecklist): void {
    checklist.expanded = !checklist.expanded;
  }

  toggleSection(section: ChecklistSection): void {
    section.expanded = !section.expanded;
  }

  expandAllChecklists(): void {
    this.groupedChecklists.forEach(c => {
      c.expanded = true;
      c.sections.forEach(s => s.expanded = true);
    });
  }

  collapseAllChecklists(): void {
    this.groupedChecklists.forEach(c => {
      c.expanded = false;
      c.sections.forEach(s => s.expanded = false);
    });
  }

  getTotalItemsInChecklist(checklist: GroupedChecklist): number {
    return checklist.sections.reduce((sum, section) => sum + section.items.length, 0);
  }

  isChecklistItemSelected(itemId: string): boolean {
    const isSelected = this.tempSelectedChecklistItems.some(item => item.predefinedChecklistItemId === itemId);
    if (this.tempSelectedChecklistItems.length > 0 && this.tempSelectedChecklistItems.length < 10) {
      // Only log for debugging when we have a small number of selections
      console.log(`Checking if ${itemId} is selected:`, isSelected, 'Against:', this.tempSelectedChecklistItems.map(x => x.predefinedChecklistItemId));
    }
    return isSelected;
  }

  isChecklistItemMandatory(itemId: string): boolean {
    const item = this.tempSelectedChecklistItems.find(i => i.predefinedChecklistItemId === itemId);
    return item ? item.isMandatory : false;
  }

  toggleChecklistItem(checklistItem: PredefinedChecklistItem): void {
    const index = this.tempSelectedChecklistItems.findIndex(
      item => item.predefinedChecklistItemId === checklistItem.predefinedItemId
    );

    if (index > -1) {
      this.tempSelectedChecklistItems.splice(index, 1);
    } else {
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

  areAllChecklistItemsSelected(checklist: GroupedChecklist): boolean {
    const checklistItemIds = checklist.sections.flatMap(s => s.items.map(i => i.predefinedItemId));
    return checklistItemIds.length > 0 && checklistItemIds.every(id => this.isChecklistItemSelected(id));
  }

  areSomeChecklistItemsSelected(checklist: GroupedChecklist): boolean {
    const checklistItemIds = checklist.sections.flatMap(s => s.items.map(i => i.predefinedItemId));
    const selectedCount = checklistItemIds.filter(id => this.isChecklistItemSelected(id)).length;
    return selectedCount > 0 && selectedCount < checklistItemIds.length;
  }

  toggleSelectAllInChecklist(checklist: GroupedChecklist): void {
    const allSelected = this.areAllChecklistItemsSelected(checklist);
    const checklistItemIds = checklist.sections.flatMap(s => s.items.map(i => i.predefinedItemId));
    
    if (allSelected) {
      // Remove all items from this checklist
      this.tempSelectedChecklistItems = this.tempSelectedChecklistItems.filter(
        item => !checklistItemIds.includes(item.predefinedChecklistItemId)
      );
    } else {
      // Add items that aren't already selected
      checklist.sections.forEach(section => {
        section.items.forEach(item => {
          if (!this.isChecklistItemSelected(item.predefinedItemId)) {
            this.tempSelectedChecklistItems.push({
              predefinedChecklistItemId: item.predefinedItemId,
              sequence: item.sequence,
              isMandatory: false
            });
          }
        });
      });
    }
  }

  areAllSectionItemsSelected(section: ChecklistSection): boolean {
    return section.items.length > 0 && section.items.every(item => this.isChecklistItemSelected(item.predefinedItemId));
  }

  areSomeSectionItemsSelected(section: ChecklistSection): boolean {
    const selectedCount = section.items.filter(item => this.isChecklistItemSelected(item.predefinedItemId)).length;
    return selectedCount > 0 && selectedCount < section.items.length;
  }

  toggleSelectAllInSection(section: ChecklistSection): void {
    const allSelected = this.areAllSectionItemsSelected(section);
    const sectionItemIds = section.items.map(i => i.predefinedItemId);
    
    if (allSelected) {
      // Remove all items from this section
      this.tempSelectedChecklistItems = this.tempSelectedChecklistItems.filter(
        item => !sectionItemIds.includes(item.predefinedChecklistItemId)
      );
    } else {
      // Add items that aren't already selected
      section.items.forEach(item => {
        if (!this.isChecklistItemSelected(item.predefinedItemId)) {
          this.tempSelectedChecklistItems.push({
            predefinedChecklistItemId: item.predefinedItemId,
            sequence: item.sequence,
            isMandatory: false
          });
        }
      });
    }
  }

  trackByChecklistId(index: number, checklist: GroupedChecklist): string {
    return checklist.checklistId;
  }

  saveChecklistItems(): void {
    this.save.emit([...this.tempSelectedChecklistItems]);
  }

  closeModal(): void {
    this.close.emit();
  }

  onBackdropClick(): void {
    this.closeModal();
  }
}
