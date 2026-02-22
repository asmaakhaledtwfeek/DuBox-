import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

interface PredefinedChecklistItem {
  predefinedItemId: string;
  checkpointDescription: string;
  reference: string | null;
  sequence: number;
  isActive: boolean;
  checklistSectionId: string | null;
  sectionTitle: string | null;
  sectionOrder: number | null;
  checklistId: string | null;
  checklistName: string | null;
  checklistCode: string | null;
  checklistDiscipline: string | null;
  checklistSubDiscipline: string | null;
  checklistPageNumber: number | null;
  checklistWIRCode: string | null;
  checklistReferenceDocuments: string[] | null;
  checklistSignatureRoles: string[] | null;
  selected?: boolean;
}

interface GroupedChecklist {
  wirCode: string;
  checklists: {
    checklistId: string;
    checklistName: string;
    sections: {
      sectionId: string;
      sectionTitle: string;
      sectionOrder: number;
      items: PredefinedChecklistItem[];
      expanded?: boolean;
    }[];
    expanded?: boolean;
  }[];
  expanded?: boolean;
}

@Component({
  selector: 'app-manage-wir-checklist',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './manage-wir-checklist.component.html',
  styleUrl: './manage-wir-checklist.component.scss'
})
export class ManageWirChecklistComponent implements OnInit {
  stage: string = '';
  groupedChecklists: GroupedChecklist[] = [];
  availableWIRCodes: string[] = ['WIR-1', 'WIR-2', 'WIR-3', 'WIR-4', 'WIR-5', 'WIR-6'];
  selectedWIRCode: string = '';
  currentWIRCode: string = ''; // WIR code for the current stage
  loading = false;
  error: string | null = null;
  searchTerm: string = '';
  
  // Navigation context
  sourceTemplateId: string | null = null;
  navigationSource: string | null = null;
  
  // Custom item form
  showCustomItemForm = false;
  customItemDescription: string = '';
  customItemReference: string = '';
  customItemSection: string = '';

  // Add checklist form
  showAddChecklistForm = false;
  checklistForm = {
    name: '',
    code: '',
    discipline: '',
    subDiscipline: '',
    pageNumber: 1,
    wirCode: '',
    referenceDocuments: '',
    signatureRoles: ''
  };

  showAddSectionForm = false;
  sectionForm = {
    checklistId: '',
    checklistName: '',
    title: '',
    order: 1
  };

  showAddItemForm = false;
  itemForm = {
    checklistSectionId: '',
    sectionTitle: '',
    checklistName: '',
    description: '',
    sequence: 1,
    reference: ''
  };

  // Modern alert system
  showAlert = false;
  alertConfig = {
    type: 'success' as 'success' | 'error' | 'warning' | 'info',
    title: '',
    message: '',
    showIcon: true
  };

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) {}

  showModernAlert(type: 'success' | 'error' | 'warning' | 'info', title: string, message: string): void {
    this.alertConfig = {
      type,
      title,
      message,
      showIcon: true
    };
    this.showAlert = true;
  }

  closeAlert(): void {
    this.showAlert = false;
  }

  // Format WIR code for display (e.g., "WIR-1" -> "Stage 1")
  formatStageCode(wirCode: string): string {
    if (!wirCode) return '';
    const match = wirCode.match(/WIR-(\d+)/i);
    if (match) {
      const stageNumber = match[1]; // No zero padding
      return `Stage ${stageNumber}`;
    }
    return wirCode; // Return as-is if format doesn't match
  }

  ngOnInit(): void {
    // Get stage and navigation context from query params
    this.route.queryParams.subscribe(params => {
      this.stage = params['stage'] || 'Stage 01';
      this.sourceTemplateId = params['templateId'] || null;
      this.navigationSource = params['source'] || null;
      
      // Extract stage number and set WIR code
      const stageMatch = this.stage.match(/\d+/);
      if (stageMatch) {
        const stageNumber = stageMatch[0];
        this.currentWIRCode = `WIR-${stageNumber}`;
        this.checklistForm.wirCode = this.currentWIRCode;
      }
    });

    // Load WIR checklists
    this.loadAllWIRChecklists();
  }

  loadAllWIRChecklists(): void {
    this.loading = true;
    this.error = null;
    this.groupedChecklists = [];

    // Get ALL checklists in one call
    this.http.get<any>(`${environment.apiUrl}/Checklists`).subscribe({
      next: (response) => {
        const allChecklists = response?.data || [];
        
        // Group checklists by WIR code
        const wirCodeMap = new Map<string, any[]>();
        
        // Initialize all available WIR codes with empty arrays
        this.availableWIRCodes.forEach(code => {
          wirCodeMap.set(code, []);
        });
        
        // Group checklists by their WIR code
        allChecklists.forEach((checklist: any) => {
          const wirCode = checklist.wirCode || 'Unknown';
          if (!wirCodeMap.has(wirCode)) {
            wirCodeMap.set(wirCode, []);
          }
          
          // Transform checklist to our format
          const transformedChecklist = {
            checklistId: checklist.checklistId,
            checklistName: checklist.name,
            checklistCode: checklist.code,
            discipline: checklist.discipline,
            subDiscipline: checklist.subDiscipline,
            sections: (checklist.sections || []).map((section: any, sectionIndex: number) => ({
              sectionId: section.checklistSectionId,
              sectionTitle: section.title,
              sectionOrder: section.order,
              items: (section.items || []).map((item: any, itemIndex: number) => ({
                predefinedItemId: item.checklistItemId,
                checkpointDescription: item.description,
                sequence: item.order || (itemIndex + 1),
                reference: '',
                selected: false
              })),
              expanded: false
            })),
            expanded: false
          };
          
          wirCodeMap.get(wirCode)!.push(transformedChecklist);
        });
        
        // Create grouped structure
        wirCodeMap.forEach((checklists, wirCode) => {
          this.groupedChecklists.push({
            wirCode: wirCode,
            checklists: checklists,
            expanded: true
          });
        });
        
        // Sort by WIR code
        this.groupedChecklists.sort((a, b) => a.wirCode.localeCompare(b.wirCode));
        
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = 'Failed to load checklists';
        this.loading = false;
        this.cdr.detectChanges();
        console.error('Error loading checklists:', err);
      }
    });
  }

  groupChecklistData(wirCode: string, items: PredefinedChecklistItem[]): GroupedChecklist | null {
    if (!items || items.length === 0) return null;

    const checklistsMap = new Map<string, any>();

    items.forEach(item => {
      if (!item.checklistId) return;

      if (!checklistsMap.has(item.checklistId)) {
        checklistsMap.set(item.checklistId, {
          checklistId: item.checklistId,
          checklistName: item.checklistName || 'Unnamed Checklist',
          sectionsMap: new Map<string, any>(),
          expanded: false
        });
      }

      const checklist = checklistsMap.get(item.checklistId)!;

      if (item.checklistSectionId) {
        if (!checklist.sectionsMap.has(item.checklistSectionId)) {
          checklist.sectionsMap.set(item.checklistSectionId, {
            sectionId: item.checklistSectionId,
            sectionTitle: item.sectionTitle || 'Unnamed Section',
            sectionOrder: item.sectionOrder || 0,
            items: [],
            expanded: false
          });
        }

        checklist.sectionsMap.get(item.checklistSectionId)!.items.push({
          ...item,
          selected: false
        });
      }
    });

    const checklists = Array.from(checklistsMap.values()).map(checklist => ({
      ...checklist,
      sections: Array.from(checklist.sectionsMap.values())
        .sort((a: any, b: any) => a.sectionOrder - b.sectionOrder)
    }));

    return {
      wirCode: wirCode,
      checklists: checklists,
      expanded: true
    };
  }

  toggleWIRCode(wirGroup: GroupedChecklist): void {
    wirGroup.expanded = !wirGroup.expanded;
  }

  toggleChecklist(checklist: any): void {
    checklist.expanded = !checklist.expanded;
  }

  toggleSection(section: any): void {
    section.expanded = !section.expanded;
  }

  selectAllInSection(section: any): void {
    const allSelected = section.items.every((item: PredefinedChecklistItem) => item.selected);
    section.items.forEach((item: PredefinedChecklistItem) => {
      item.selected = !allSelected;
    });
  }

  getSelectedItemsCount(): number {
    let count = 0;
    this.groupedChecklists.forEach(wirGroup => {
      wirGroup.checklists.forEach(checklist => {
        checklist.sections.forEach(section => {
          count += section.items.filter((item: PredefinedChecklistItem) => item.selected).length;
        });
      });
    });
    return count;
  }

  getFilteredChecklists(): GroupedChecklist[] {
    if (!this.searchTerm) {
      return this.groupedChecklists;
    }

    const term = this.searchTerm.toLowerCase();
    return this.groupedChecklists.map(wirGroup => ({
      ...wirGroup,
      checklists: wirGroup.checklists.map(checklist => ({
        ...checklist,
        sections: checklist.sections.map(section => ({
          ...section,
          items: section.items.filter(item => 
            item.checkpointDescription.toLowerCase().includes(term) ||
            (item.reference && item.reference.toLowerCase().includes(term))
          )
        })).filter(section => section.items.length > 0)
      })).filter(checklist => checklist.sections.length > 0)
    })).filter(wirGroup => wirGroup.checklists.length > 0);
  }

  getCurrentWIRChecklists(): GroupedChecklist[] {
    return this.getFilteredChecklists().filter(wirGroup => wirGroup.wirCode === this.currentWIRCode);
  }

  getOtherWIRChecklists(): GroupedChecklist[] {
    return this.getFilteredChecklists().filter(wirGroup => wirGroup.wirCode !== this.currentWIRCode);
  }

  expandAll(): void {
    this.groupedChecklists.forEach(wirGroup => {
      wirGroup.expanded = true;
      wirGroup.checklists.forEach(checklist => {
        checklist.expanded = true;
        checklist.sections.forEach(section => {
          section.expanded = true;
        });
      });
    });
  }

  collapseAll(): void {
    this.groupedChecklists.forEach(wirGroup => {
      wirGroup.expanded = false;
      wirGroup.checklists.forEach(checklist => {
        checklist.expanded = false;
        checklist.sections.forEach(section => {
          section.expanded = false;
        });
      });
    });
  }

  openCustomItemForm(): void {
    this.showCustomItemForm = true;
    this.customItemDescription = '';
    this.customItemReference = '';
    this.customItemSection = '';
  }

  closeCustomItemForm(): void {
    this.showCustomItemForm = false;
  }

  addCustomItem(): void {
    if (!this.customItemDescription.trim()) {
      this.showModernAlert('warning', 'Validation Error', 'Please enter a description for the custom item');
      return;
    }

    // Add custom item to the first checklist or create a new "Custom" group
    // This is a simplified implementation - you might want to handle this differently
    this.showModernAlert('info', 'Coming Soon', 'Custom item functionality will be implemented based on your requirements');
    this.closeCustomItemForm();
  }

  addNewChecklistGlobal(): void {
    // Reset form
    this.checklistForm = {
      name: '',
      code: '',
      discipline: '',
      subDiscipline: '',
      pageNumber: 1,
      wirCode: this.currentWIRCode, // Always use current WIR code
      referenceDocuments: '',
      signatureRoles: ''
    };
    this.showAddChecklistForm = true;
  }

  closeAddChecklistForm(): void {
    this.showAddChecklistForm = false;
  }

  saveNewChecklist(): void {
    // Validate required fields
    if (!this.checklistForm.name.trim()) {
      this.showModernAlert('warning', 'Validation Error', 'Please enter checklist name');
      return;
    }
    if (!this.checklistForm.code.trim()) {
      this.showModernAlert('warning', 'Validation Error', 'Please enter checklist code');
      return;
    }
    if (!this.checklistForm.discipline.trim()) {
      this.showModernAlert('warning', 'Validation Error', 'Please enter discipline');
      return;
    }
    if (this.checklistForm.pageNumber < 1) {
      this.showModernAlert('warning', 'Validation Error', 'Page number must be greater than 0');
      return;
    }

    this.loading = true;
    this.error = null;

    // Prepare the request data
    const requestData = {
      name: this.checklistForm.name.trim(),
      code: this.checklistForm.code.trim(),
      discipline: this.checklistForm.discipline.trim(),
      subDiscipline: this.checklistForm.subDiscipline.trim() || null,
      pageNumber: this.checklistForm.pageNumber,
      wirCode: this.checklistForm.wirCode || null,
      referenceDocuments: this.checklistForm.referenceDocuments ? 
        this.checklistForm.referenceDocuments.split(',').map(d => d.trim()).filter(d => d) : null,
      signatureRoles: this.checklistForm.signatureRoles ? 
        this.checklistForm.signatureRoles.split(',').map(r => r.trim()).filter(r => r) : null
    };

    this.http.post<any>(`${environment.apiUrl}/Checklists`, requestData).subscribe({
      next: (response) => {
        this.loading = false;
        this.showModernAlert('success', 'Success!', 'Checklist created successfully!');
        this.closeAddChecklistForm();
        // Force reload of the page after a short delay to allow backend to process
        setTimeout(() => {
          this.loadAllWIRChecklists();
        }, 500);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || err.error?.error || 'Failed to create checklist';
        this.showModernAlert('error', 'Error', this.error || 'Failed to create checklist');
        console.error('Error creating checklist:', err);
      }
    });
  }

  addNewChecklist(wirGroup: GroupedChecklist): void {
    const checklistName = prompt(`Enter name for new checklist in ${wirGroup.wirCode}:`);
    if (checklistName && checklistName.trim()) {
      // Implementation for adding new checklist
      this.showModernAlert('info', 'Coming Soon', `Add new checklist: "${checklistName}" to ${wirGroup.wirCode}\n\nThis will be implemented with backend API integration.`);
    }
  }

  addNewSection(checklist: any, wirCode: string): void {
    // Only allow adding sections to current WIR code
    if (wirCode !== this.currentWIRCode) {
      this.showModernAlert('warning', 'Permission Denied', `You can only add sections to ${this.currentWIRCode}. To use items from ${wirCode}, please select them and click Save to clone them to ${this.currentWIRCode}.`);
      return;
    }

    // Calculate next order number
    const maxOrder = checklist.sections.length > 0 
      ? Math.max(...checklist.sections.map((s: any) => s.sectionOrder || 0))
      : 0;
    
    this.sectionForm = {
      checklistId: checklist.checklistId,
      checklistName: checklist.checklistName,
      title: '',
      order: maxOrder + 1
    };
    this.showAddSectionForm = true;
  }

  closeAddSectionForm(): void {
    this.showAddSectionForm = false;
    this.sectionForm = {
      checklistId: '',
      checklistName: '',
      title: '',
      order: 1
    };
  }

  saveNewSection(): void {
    // Validate
    if (!this.sectionForm.title || this.sectionForm.title.trim() === '') {
      this.showModernAlert('warning', 'Validation Error', 'Please enter a section title');
      return;
    }

    if (this.sectionForm.order <= 0) {
      this.showModernAlert('warning', 'Validation Error', 'Order must be greater than 0');
      return;
    }

    this.loading = true;

    const requestData = {
      checklistId: this.sectionForm.checklistId,
      title: this.sectionForm.title.trim(),
      order: this.sectionForm.order
    };

    this.http.post<any>(`${environment.apiUrl}/Checklists/sections`, requestData).subscribe({
      next: (response) => {
        this.loading = false;
        this.showModernAlert('success', 'Success!', 'Section created successfully!');
        this.closeAddSectionForm();
        // Reload checklists to show the new section
        setTimeout(() => {
          this.loadAllWIRChecklists();
        }, 500);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || err.error?.error || 'Failed to create section';
        this.showModernAlert('error', 'Error', this.error || 'Failed to create section');
        console.error('Error creating section:', err);
      }
    });
  }

  addNewItem(section: any, checklist?: any, wirCode?: string): void {
    // Only allow adding items to current WIR code
    if (wirCode && wirCode !== this.currentWIRCode) {
      this.showModernAlert('warning', 'Permission Denied', `You can only add items to ${this.currentWIRCode}. To use items from ${wirCode}, please select them and click Save to clone them to ${this.currentWIRCode}.`);
      return;
    }

    // Calculate next sequence number
    const maxSequence = section.items.length > 0 
      ? Math.max(...section.items.map((i: any) => i.sequence || 0))
      : 0;
    
    this.itemForm = {
      checklistSectionId: section.sectionId,
      sectionTitle: section.sectionTitle,
      checklistName: checklist?.checklistName || '',
      description: '',
      sequence: maxSequence + 1,
      reference: ''
    };
    this.showAddItemForm = true;
  }

  closeAddItemForm(): void {
    this.showAddItemForm = false;
    this.itemForm = {
      checklistSectionId: '',
      sectionTitle: '',
      checklistName: '',
      description: '',
      sequence: 1,
      reference: ''
    };
  }

  saveNewItem(): void {
    // Validate
    if (!this.itemForm.description || this.itemForm.description.trim() === '') {
      this.showModernAlert('warning', 'Validation Error', 'Please enter an item description');
      return;
    }

    if (this.itemForm.sequence <= 0) {
      this.showModernAlert('warning', 'Validation Error', 'Sequence must be greater than 0');
      return;
    }

    // Check for duplicate sequence in the current section
    const isDuplicateSequence = this.checkDuplicateSequence(
      this.itemForm.checklistSectionId, 
      this.itemForm.sequence
    );

    if (isDuplicateSequence) {
      this.showModernAlert(
        'warning', 
        'Duplicate Sequence', 
        `An item with sequence number ${this.itemForm.sequence} already exists in this section. Please use a different sequence number.`
      );
      return;
    }

    this.loading = true;

    const requestData = {
      checklistSectionId: this.itemForm.checklistSectionId,
      description: this.itemForm.description.trim(),
      sequence: this.itemForm.sequence,
      reference: this.itemForm.reference.trim() || null
    };

    this.http.post<any>(`${environment.apiUrl}/Checklists/items`, requestData).subscribe({
      next: (response) => {
        this.loading = false;
        this.showModernAlert('success', 'Success!', 'Item created successfully!');
        this.closeAddItemForm();
        // Reload checklists to show the new item
        setTimeout(() => {
          this.loadAllWIRChecklists();
        }, 500);
      },
      error: (err) => {
        this.loading = false;
        // Extract error message from backend validation
        let errorMessage = 'Failed to create item';
        if (err.error?.errors) {
          const validationErrors = Object.values(err.error.errors).flat();
          errorMessage = validationErrors.join(', ');
        } else if (err.error?.error?.message) {
          errorMessage = err.error.error.message;
        } else if (err.error?.message) {
          errorMessage = err.error.message;
        } else if (err.error?.error) {
          errorMessage = err.error.error;
        }
        
        this.error = errorMessage;
        this.showModernAlert('error', 'Error', errorMessage);
        console.error('Error creating item:', err);
      }
    });
  }

  // Helper method to check for duplicate sequence numbers in a section
  checkDuplicateSequence(sectionId: string, sequence: number): boolean {
    for (const wirGroup of this.groupedChecklists) {
      for (const checklist of wirGroup.checklists) {
        const section = checklist.sections.find(s => s.sectionId === sectionId);
        if (section) {
          return section.items.some(item => item.sequence === sequence && item.isActive);
        }
      }
    }
    return false;
  }

  save(): void {
    const selectedItems = this.getSelectedItems();
    
    if (selectedItems.length === 0) {
      this.showModernAlert('warning', 'No Selection', 'Please select at least one checklist item');
      return;
    }

    // Check if any items are selected from WIR codes other than current
    const itemsFromOtherWIRCodes = selectedItems.filter(
      item => item.checklistWIRCode !== this.currentWIRCode
    );

    if (itemsFromOtherWIRCodes.length > 0) {
      // Clone items from other WIR codes to current WIR code
      this.cloneSelectedItems(itemsFromOtherWIRCodes);
    } else {
      // All items are from current WIR code, just navigate back
      this.showModernAlert('info', 'Already Available', 'Items from current Stage are already available');
      this.cancel();
    }
  }

  cloneSelectedItems(items: PredefinedChecklistItem[]): void {
    this.loading = true;
    this.error = null;

    const itemIds = items.map(item => item.predefinedItemId);

    const requestData = {
      itemIds: itemIds,
      targetWIRCode: this.currentWIRCode
    };

    this.http.post<any>(`${environment.apiUrl}/Checklists/clone`, requestData).subscribe({
      next: (response) => {
        this.loading = false;
        const result = response.data;
        this.showModernAlert(
          'success',
          'Successfully Cloned!',
          `Cloned ${result.checklistsCloned} checklist(s), ${result.sectionsCloned} section(s), and ${result.itemsCloned} item(s) to ${this.currentWIRCode}`
        );
        
        // Reload checklists to show the cloned items
        this.loadAllWIRChecklists();
        
        // Clear selections
        this.clearAllSelections();
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || err.error?.error || 'Failed to clone checklist items';
        this.showModernAlert('error', 'Clone Failed', this.error || 'Failed to clone checklist items');
        console.error('Error cloning checklist items:', err);
      }
    });
  }

  clearAllSelections(): void {
    this.groupedChecklists.forEach(wirGroup => {
      wirGroup.checklists.forEach(checklist => {
        checklist.sections.forEach(section => {
          section.items.forEach((item: PredefinedChecklistItem) => {
            item.selected = false;
          });
        });
      });
    });
  }

  getSelectedItems(): PredefinedChecklistItem[] {
    const selected: PredefinedChecklistItem[] = [];
    this.groupedChecklists.forEach(wirGroup => {
      wirGroup.checklists.forEach(checklist => {
        checklist.sections.forEach(section => {
          section.items.forEach((item: PredefinedChecklistItem) => {
            if (item.selected) {
              selected.push(item);
            }
          });
        });
      });
    });
    return selected;
  }

  cancel(): void {
    // Check if we came from Activity Template Checklist (QC)
    if (this.navigationSource === 'activity-template-checklist' && this.sourceTemplateId) {
      // Navigate back to Activity Template Checklist page
      this.router.navigate(['/qc/activity-templates', this.sourceTemplateId, 'checklist']);
      return;
    }
    
    // Check if we came from Create/Edit Activity Template (Schedule)
    const stateJson = sessionStorage.getItem('activityTemplateState');
    if (stateJson) {
      const state = JSON.parse(stateJson);
      // Navigate back to create/edit page
      if (state.templateId) {
        this.router.navigate(['/schedule/activity-templates', state.templateId, 'edit']);
      } else {
        this.router.navigate(['/schedule/activity-templates/create']);
      }
    } else {
      // Fallback navigation
      this.router.navigate(['/schedule/activity-templates']);
    }
  }
}
