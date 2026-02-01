import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ApiService } from '../../../core/services/api.service';

interface PredefinedChecklistItem {
  predefinedItemId: string;
  checkpointDescription: string;
  reference?: string;
  sequence: number;
  isActive: boolean;
  checklistSectionId?: string;
  sectionTitle?: string;
  sectionOrder?: number;
  checklistId?: string;
  checklistName?: string;
  checklistCode?: string;
  checklistDiscipline?: string;
  checklistSubDiscipline?: string;
  checklistPageNumber?: number;
  checklistWIRCode?: string;
}

interface ChecklistSection {
  sectionId: string;
  sectionTitle: string;
  sectionOrder: number;
  items: PredefinedChecklistItem[];
  isExpanded: boolean;
}

interface ChecklistGroup {
  checklistId: string;
  checklistName: string;
  checklistCode: string;
  discipline?: string;
  subDiscipline?: string;
  pageNumber?: number;
  sections: ChecklistSection[];
  isExpanded: boolean;
}

@Component({
  selector: 'app-predefined-checklist-details',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './predefined-checklist-details.component.html',
  styleUrls: ['./predefined-checklist-details.component.scss']
})
export class PredefinedChecklistDetailsComponent implements OnInit {
  wirCode: string = '';
  loading = true;
  error = '';
  checklistGroups: ChecklistGroup[] = [];
  totalItems = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService
  ) {}

  ngOnInit(): void {
    this.wirCode = this.route.snapshot.paramMap.get('wirCode') || '';
    if (this.wirCode) {
      this.loadChecklistItems();
    } else {
      this.error = 'Invalid WIR code';
      this.loading = false;
    }
  }

  loadChecklistItems(): void {
    this.loading = true;
    this.error = '';

    this.apiService.get<any>(`wircheckpoints/predefined-checklist-items/wir/${this.wirCode}`).subscribe({
      next: (response) => {
        const items: PredefinedChecklistItem[] = response.value || response || [];
        this.totalItems = items.length;
        this.checklistGroups = this.groupByChecklistAndSection(items);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading checklist items:', err);
        this.error = err.message || 'Failed to load checklist items';
        this.loading = false;
      }
    });
  }

  private groupByChecklistAndSection(items: PredefinedChecklistItem[]): ChecklistGroup[] {
    const checklistMap = new Map<string, ChecklistGroup>();

    items.forEach(item => {
      const checklistId = item.checklistId || 'unknown';
      
      if (!checklistMap.has(checklistId)) {
        checklistMap.set(checklistId, {
          checklistId,
          checklistName: item.checklistName || 'Unknown Checklist',
          checklistCode: item.checklistCode || '',
          discipline: item.checklistDiscipline,
          subDiscipline: item.checklistSubDiscipline,
          pageNumber: item.checklistPageNumber,
          sections: [],
          isExpanded: true
        });
      }

      const checklist = checklistMap.get(checklistId)!;
      const sectionId = item.checklistSectionId || 'unknown';
      
      let section = checklist.sections.find(s => s.sectionId === sectionId);
      if (!section) {
        section = {
          sectionId,
          sectionTitle: item.sectionTitle || 'General',
          sectionOrder: item.sectionOrder || 0,
          items: [],
          isExpanded: true
        };
        checklist.sections.push(section);
      }

      section.items.push(item);
    });

    // Sort sections by order and items by sequence
    checklistMap.forEach(checklist => {
      checklist.sections.sort((a, b) => a.sectionOrder - b.sectionOrder);
      checklist.sections.forEach(section => {
        section.items.sort((a, b) => a.sequence - b.sequence);
      });
    });

    return Array.from(checklistMap.values());
  }

  toggleChecklist(checklist: ChecklistGroup): void {
    checklist.isExpanded = !checklist.isExpanded;
  }

  toggleSection(section: ChecklistSection): void {
    section.isExpanded = !section.isExpanded;
  }

  expandAll(): void {
    this.checklistGroups.forEach(c => {
      c.isExpanded = true;
      c.sections.forEach(s => s.isExpanded = true);
    });
  }

  collapseAll(): void {
    this.checklistGroups.forEach(c => {
      c.isExpanded = false;
      c.sections.forEach(s => s.isExpanded = false);
    });
  }

  goBack(): void {
    this.router.navigate(['/qc/predefined-checklists']);
  }
}

