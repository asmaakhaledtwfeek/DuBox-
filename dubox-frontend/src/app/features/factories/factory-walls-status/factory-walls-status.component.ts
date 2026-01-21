import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { FactoryService, Factory } from '../../../core/services/factory.service';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxPanel, PanelStatus, PanelType } from '../../../core/models/box.model';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import { PanelTypeService } from '../../../core/services/panel-type.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-factory-walls-status',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './factory-walls-status.component.html',
  styleUrls: ['./factory-walls-status.component.scss']
})
export class FactoryWallsStatusComponent implements OnInit, OnDestroy {
  factoryId: string = '';
  factory: Factory | null = null;
  boxes: Box[] = []; // All boxes for the selected project (before pagination)
  allBoxes: Box[] = []; // Kept for backward compatibility with filter helpers
  factoryBoxes: Box[] = []; // All boxes in this factory (for project filtering)
  filteredBoxes: Box[] = [];
  projects: Project[] = [];
  allProjects: Project[] = []; // Store all projects before filtering
  selectedProjectId: string = '';
  selectedBuildingNumber: string = '';
  selectedFloor: string = '';
  availableBuildingNumbers: string[] = [];
  availableFloors: string[] = [];
  loading = true;
  error = '';
  projectsLoading = false;
  
  // Panel types configured for the selected project
  panelTypes: PanelType[] = [];
  
  // Pagination
  currentPage: number = 1;
  pageSize: number = 50;
  totalCount: number = 0;
  totalPages: number = 0;
  
  private subscriptions: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private factoryService: FactoryService,
    private boxService: BoxService,
    private projectService: ProjectService,
    private panelTypeService: PanelTypeService
  ) {}

  ngOnInit(): void {
    this.factoryId = this.route.snapshot.params['id'];
    // Load factory details immediately so header information is available
    this.loadData();
    // Load projects list for project selection (boxes will be loaded after a project is selected)
    this.loadProjects();
    // Load factory boxes metadata to know which projects have boxes in this factory
    this.loadFactoryBoxesMetadata();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadData(): void {
    this.loading = true;
    this.error = '';

    // Load factory details only. Boxes are now loaded after a project is selected.
    const factorySub = this.factoryService.getFactoryById(this.factoryId).subscribe({
      next: (factory: Factory) => {
        this.factory = factory;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load factory details';
        console.error('Error loading factory:', err);
        this.loading = false;
      }
    });

    this.subscriptions.push(factorySub);
  }

  loadProjects(): void {
    this.projectsLoading = true;
    const projectsSub = this.projectService.getProjects().subscribe({
      next: (projects: Project[]) => {
        // Store all non-archived projects
        this.allProjects = projects.filter(p => p.status !== 'Archived');
        // Once we know which projects have boxes in this factory, filter accordingly
        this.filterProjectsByFactoryBoxes();
        this.projectsLoading = false;
      },
      error: (err: any) => {
        console.error('Error loading projects:', err);
        this.projectsLoading = false;
      }
    });

    this.subscriptions.push(projectsSub);
  }

  /**
   * Load boxes for this factory once, so we can determine
   * which projects actually have boxes in this factory.
   */
  private loadFactoryBoxesMetadata(): void {
    const factoryBoxesSub = this.boxService.getBoxesByFactory(this.factoryId).subscribe({
      next: (boxes: Box[]) => {
        this.factoryBoxes = boxes;
        // After we have factory boxes, filter the projects list (if projects are already loaded)
        this.filterProjectsByFactoryBoxes();
      },
      error: (err: any) => {
        console.error('Error loading factory boxes metadata:', err);
      }
    });

    this.subscriptions.push(factoryBoxesSub);
  }

  /**
   * Restrict the project dropdown to only projects that have at least
   * one box in the current factory.
   */
  private filterProjectsByFactoryBoxes(): void {
    if (this.allProjects.length === 0 || this.factoryBoxes.length === 0) {
      // If either side isn't loaded yet, do nothing; this will re-run when both are available.
      return;
    }

    const projectIdsWithBoxesInFactory = new Set<string>();
    this.factoryBoxes.forEach(box => {
      if (box.projectId) {
        projectIdsWithBoxesInFactory.add(box.projectId);
      }
    });

    this.projects = this.allProjects.filter(p => projectIdsWithBoxesInFactory.has(p.id));
  }

  /**
   * Load boxes for the currently selected project.
   * This now mirrors the Schedule page behaviour:
   * data is only loaded once a project is chosen.
   */
  private loadBoxesForSelectedProject(): void {
    if (!this.selectedProjectId) {
      return;
    }

    this.loading = true;
    this.error = '';

    const boxesSub = this.boxService.getBoxesByProject(this.selectedProjectId).subscribe({
      next: (boxes: Box[]) => {
        // Keep only boxes that belong to this factory and have position data
        const boxesInFactory = boxes.filter(
          box => box.factoryId === this.factoryId && (box.bay || box.row || box.position)
        );

        this.boxes = boxesInFactory;
        this.allBoxes = boxesInFactory;

        // Reset pagination for the new project selection
        this.currentPage = 1;

        // Update dependent filter options and apply filters/pagination
        this.updateBuildingsAndLevels();
        this.applyFilters();

        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load boxes for the selected project';
        console.error('Error loading boxes for project:', err);
        this.loading = false;
      }
    });

    this.subscriptions.push(boxesSub);
  }

  /**
   * Load panel types configured for the selected project.
   * These will drive the dynamic panel columns in the table.
   */
  private loadPanelTypesForSelectedProject(): void {
    if (!this.selectedProjectId) {
      this.panelTypes = [];
      return;
    }

    const panelTypesSub = this.panelTypeService
      .getPanelTypesByProject(this.selectedProjectId, true) // fetch all, filter active on client
      .subscribe({
        next: (response) => {
          const allTypes: PanelType[] = response?.data || [];
          // Only use active panel types for the status table and order by displayOrder then name
          this.panelTypes = allTypes
            .filter(pt => pt.isActive)
            .sort((a, b) => {
              const orderDiff = (a.displayOrder ?? 0) - (b.displayOrder ?? 0);
              if (orderDiff !== 0) {
                return orderDiff;
              }
              return a.panelTypeName.localeCompare(b.panelTypeName);
            });
        },
        error: (err: any) => {
          console.error('Error loading panel types for project:', err);
          // Fail silently for UI; keep previous panelTypes if any
        }
      });

    this.subscriptions.push(panelTypesSub);
  }

  /**
   * Handle page change
   */
  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.applyFilters();
    }
  }

  /**
   * Handle page size change
   */
  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.currentPage = 1; // Reset to first page when changing page size
    this.applyFilters();
  }

  /**
   * Get page numbers for pagination display
   */
  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 5;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(this.totalPages, startPage + maxPagesToShow - 1);
    
    // Adjust start page if we're near the end
    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    
    return pages;
  }

  /**
   * Update available building numbers and floors from boxes
   * If a project is selected, only show options for that project
   */
  updateBuildingsAndLevels(): void {
    if (this.boxes.length === 0) {
      return;
    }

    // Filter boxes based on selected project if one is selected
    const boxesToConsider = this.selectedProjectId
      ? this.boxes.filter(box => box.projectId === this.selectedProjectId)
      : this.boxes;

    // Get unique building numbers
    const buildingsSet = new Set<string>();
    boxesToConsider.forEach(box => {
      if (box.buildingNumber) {
        buildingsSet.add(box.buildingNumber);
      }
    });
    this.availableBuildingNumbers = Array.from(buildingsSet).sort();

    // Get unique floors
    const floorsSet = new Set<string>();
    boxesToConsider.forEach(box => {
      if (box.floor) {
        floorsSet.add(box.floor);
      }
    });
    this.availableFloors = Array.from(floorsSet).sort();

    // Clear selections if they're no longer valid after project change
    if (this.selectedProjectId) {
      if (this.selectedBuildingNumber && !this.availableBuildingNumbers.includes(this.selectedBuildingNumber)) {
        this.selectedBuildingNumber = '';
      }
      if (this.selectedFloor && !this.availableFloors.includes(this.selectedFloor)) {
        this.selectedFloor = '';
      }
    }
  }

  /**
   * Handle project selection change - load project-specific data
   */
  onProjectSelect(): void {
    // Reset all filters when project changes
    this.selectedBuildingNumber = '';
    this.selectedFloor = '';
    this.currentPage = 1;

    // Clear current box data
    this.panelTypes = [];
    this.boxes = [];
    this.allBoxes = [];
    this.filteredBoxes = [];
    this.totalCount = 0;
    this.totalPages = 0;

    if (this.selectedProjectId) {
      // Load boxes only after a project has been chosen
      this.loadBoxesForSelectedProject();
      // Load panel types for the selected project
      this.loadPanelTypesForSelectedProject();
    }
  }

  applyFilters(): void {
    // Apply filters client-side to the full in-memory collection for the selected project,
    // then handle pagination.
    let filtered = [...this.boxes];

    // Filter by project (defensive - boxes are already loaded per project)
    if (this.selectedProjectId) {
      filtered = filtered.filter(box => box.projectId === this.selectedProjectId);
    }

    // Filter by building number
    if (this.selectedBuildingNumber) {
      filtered = filtered.filter(box => box.buildingNumber === this.selectedBuildingNumber);
    }

    // Filter by floor
    if (this.selectedFloor) {
      filtered = filtered.filter(box => box.floor === this.selectedFloor);
    }

   

    // Update pagination metadata based on filtered set
    this.totalCount = filtered.length;
    this.totalPages = this.totalCount === 0 ? 0 : Math.ceil(this.totalCount / this.pageSize);

    // Ensure current page is within range
    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages || 1;
    }

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.filteredBoxes = filtered.slice(startIndex, endIndex);
  }

  clearFilters(): void {
    this.selectedProjectId = '';
    this.selectedBuildingNumber = '';
    this.selectedFloor = '';
    // Reset to page 1 when clearing filters
    this.currentPage = 1;
    // Re-apply filters (which now only depend on project selection)
    this.applyFilters();
  }

  goBack(): void {
    this.router.navigate(['/factories', this.factoryId, 'layout']);
  }

  /**
   * Calculate completion statistics
   */
  getCompletionStats(): { 
    total: number;
    panelsComplete: number;
    panelsYellow: number;
    panelsGreen: number;
    panelsRed: number;
    panelsGray: number;
    slabComplete: number;
    soffitComplete: number;
    allComplete: number;
    podDeliverComplete: number;
    panelStats: Map<string, { total: number; yellow: number; green: number; red: number; gray: number }>;
  } {
    const stats = {
      total: this.filteredBoxes.length,
      panelsComplete: 0,
      panelsYellow: 0,
      panelsGreen: 0,
      panelsRed: 0,
      panelsGray: 0,
      slabComplete: 0,
      soffitComplete: 0,
      allComplete: 0,
      podDeliverComplete: 0,
      panelStats: new Map<string, { total: number; yellow: number; green: number; red: number; gray: number }>()
    };

    this.filteredBoxes.forEach(box => {
      // Count panels by status
      if (box.boxPanels && box.boxPanels.length > 0) {
        stats.panelsComplete++;
        
        box.boxPanels.forEach((panel: BoxPanel) => {
          // Find matching panel type for this panel
          let matchingPanelType: PanelType | undefined;
          
          // First try to match by panelTypeId
          if (panel.panelTypeId) {
            matchingPanelType = this.panelTypes.find(pt => pt.panelTypeId === panel.panelTypeId);
          }
          
          // If no match by ID, try to match by typeName
          if (!matchingPanelType && panel.typeName) {
            matchingPanelType = this.panelTypes.find(pt => 
              pt.panelTypeName === panel.typeName || 
              pt.panelTypeCode === panel.typeCode
            );
          }
          
          // If still no match, try to match by panelName (fallback)
          if (!matchingPanelType && panel.panelName) {
            matchingPanelType = this.panelTypes.find(pt => pt.panelTypeName === panel.panelName);
          }
          
          // Use the panel type name as key, or fallback to typeName/panelName
          const key = matchingPanelType?.panelTypeName || panel.typeName || panel.panelName;
          
          if (!key) {
            return;
          }

          // Initialize stats for this panel type if not exists
          if (!stats.panelStats.has(key)) {
            stats.panelStats.set(key, { total: 0, yellow: 0, green: 0, red: 0, gray: 0 });
          }
          
          const panelStat = stats.panelStats.get(key)!;
          panelStat.total++;
          
          // Count by normalized status
          const normalizedStatus = this.normalizePanelStatus(panel.panelStatus);
          if (normalizedStatus === PanelStatus.NotStarted) {
            panelStat.gray++;
            stats.panelsGray++;
          } else if (normalizedStatus === PanelStatus.FirstApprovalApproved) {
            panelStat.yellow++;
            stats.panelsYellow++;
          } else if (normalizedStatus === PanelStatus.SecondApprovalApproved) {
            panelStat.green++;
            stats.panelsGreen++;
          } else if (normalizedStatus === PanelStatus.SecondApprovalRejected) {
            panelStat.red++;
            stats.panelsRed++;
          }
        });
      }
      
      if (box.slab) stats.slabComplete++;
      if (box.soffit) stats.soffitComplete++;
      if (box.podDeliver) stats.podDeliverComplete++;
      
      // All complete if box has panels (all SecondApprovalApproved), slab, and soffit
      const hasPanels = box.boxPanels && box.boxPanels.length > 0;
      const allPanelsGreen = hasPanels && box.boxPanels!.every((p: BoxPanel) => {
        const normalized = this.normalizePanelStatus(p.panelStatus);
        return normalized === PanelStatus.SecondApprovalApproved;
      });
      if (allPanelsGreen && box.slab && box.soffit) {
        stats.allComplete++;
      }
    });

    return stats;
  }

  /**
   * Get the list of panel types (columns) for the selected project.
   * Ordered by displayOrder then name.
   */
  getPanelTypeColumns(): PanelType[] {
    if (this.panelTypes && this.panelTypes.length > 0) {
      return this.panelTypes;
    }

    // Fallback: derive unique keys from existing panels if panel types are not loaded
    const keys = new Map<string, PanelType>();
    this.filteredBoxes.forEach(box => {
      box.boxPanels?.forEach(panel => {
        const key = panel.typeName || panel.panelName;
        if (key && !keys.has(key)) {
          keys.set(key, {
            panelTypeId: key,
            projectId: this.selectedProjectId,
            panelTypeName: key,
            panelTypeCode: key,
            description: '',
            isActive: true,
            displayOrder: keys.size,
            createdDate: new Date(),
            modifiedDate: undefined
          });
        }
      });
    });

    return Array.from(keys.values());
  }

  /**
   * Format panel name for display (e.g., "P-001" -> "Panel 1")
   */
  getPanelDisplayName(panelName: string): string {
    // Extract number from panel name (e.g., "P-001" -> "1", "P-002" -> "2")
    const match = panelName.match(/(\d+)$/);
    if (match) {
      const panelNumber = parseInt(match[1], 10);
      return `Panel ${panelNumber}`;
    }
    // If no number found, return as is
    return panelName;
  }

  /**
   * Get all panels for a specific panel type in a box.
   */
  getPanelsForPanelType(box: Box, panelType: PanelType): BoxPanel[] {
    if (!box.boxPanels || !panelType) {
      return [];
    }

    return box.boxPanels.filter((p: BoxPanel) => {
      // Prefer matching by panelTypeId, but fall back to typeName if necessary
      if (p.panelTypeId && panelType.panelTypeId && p.panelTypeId === panelType.panelTypeId) {
        return true;
      }
      const key = p.typeName || p.panelName;
      return !!key && key === panelType.panelTypeName;
    });
  }

  /**
   * Aggregate a single status for a cell (panel type + box), used for colouring.
   */
  getAggregatedPanelStatus(box: Box, panelType: PanelType): PanelStatus | null {
    const panels = this.getPanelsForPanelType(box, panelType);
    if (!panels.length) {
      return null;
    }

    const normalizedStatuses = panels
      .map(p => this.normalizePanelStatus(p.panelStatus))
      .filter((s): s is PanelStatus => s !== null);

    if (!normalizedStatuses.length) {
      return null;
    }

    // If any panel is SecondApprovalApproved, treat the cell as green
    if (normalizedStatuses.some(s => s === PanelStatus.SecondApprovalApproved)) {
      return PanelStatus.SecondApprovalApproved;
    }

    // If any panel is SecondApprovalRejected, treat the cell as red
    if (normalizedStatuses.some(s => s === PanelStatus.SecondApprovalRejected)) {
      return PanelStatus.SecondApprovalRejected;
    }

    // If any panel is FirstApprovalApproved, treat the cell as yellow
    if (normalizedStatuses.some(s => s === PanelStatus.FirstApprovalApproved)) {
      return PanelStatus.FirstApprovalApproved;
    }

    // Otherwise consider it not started (gray)
    return PanelStatus.NotStarted;
  }

  /**
   * Normalize panel status to handle both number and enum comparisons
   */
  private normalizePanelStatus(status: unknown): PanelStatus | null {
    if (status === null || status === undefined) {
      return null;
    }

    // Already a number (or numeric string)
    if (typeof status === 'number') {
      return status as PanelStatus;
    }
    if (typeof status === 'string') {
      const trimmed = status.trim();
      const asNumber = Number(trimmed);
      if (!Number.isNaN(asNumber)) {
        return asNumber as PanelStatus;
      }

      // Backend may send enum name strings
      const key = trimmed.replace(/\s+/g, '').toLowerCase();
      const map: Record<string, PanelStatus> = {
        notstarted: PanelStatus.NotStarted,
        firstapprovalapproved: PanelStatus.FirstApprovalApproved,
        secondapprovalapproved: PanelStatus.SecondApprovalApproved,
        secondapprovalrejected: PanelStatus.SecondApprovalRejected
      };

      return map[key] ?? null;
    }

    return null;
  }

  /**
   * Get panel status label text
   */
  getPanelStatusLabel(box: Box, panelName: string): string {
    const status = this.getAggregatedPanelStatus(box, {
      panelTypeId: panelName,
      projectId: this.selectedProjectId,
      panelTypeName: panelName,
      panelTypeCode: panelName,
      description: '',
      isActive: true,
      displayOrder: 0,
      createdDate: new Date(),
      modifiedDate: undefined
    });
    if (status === null) {
      return 'Not Have This Panel';
    }

    const normalized = this.normalizePanelStatus(status);
    if (normalized === null) {
      return 'Unknown';
    }

    switch (normalized) {
      case PanelStatus.NotStarted:
        return 'Not Started';
      case PanelStatus.FirstApprovalApproved:
        return 'First Approval Approved';
      case PanelStatus.SecondApprovalApproved:
        return 'Second Approval Approved';
      case PanelStatus.SecondApprovalRejected:
        return 'Second Approval Rejected';
      default:
        return 'Unknown';
    }
  }

  /**
   * Get CSS class for panel status text
   */
  getPanelStatusTextClass(box: Box, panelName: string): string {
    const status = this.getAggregatedPanelStatus(box, {
      panelTypeId: panelName,
      projectId: this.selectedProjectId,
      panelTypeName: panelName,
      panelTypeCode: panelName,
      description: '',
      isActive: true,
      displayOrder: 0,
      createdDate: new Date(),
      modifiedDate: undefined
    });
    if (status === null) {
      return 'status-text-not-have';
    }

    const normalized = this.normalizePanelStatus(status);
    if (normalized === null) {
      return 'status-text-default';
    }

    // Use numeric comparisons to avoid TypeScript type narrowing issues
    const statusNum = Number(normalized);
    
    if (statusNum === PanelStatus.NotStarted) return 'status-text-not-started'; // Gray
    if (statusNum === PanelStatus.FirstApprovalApproved) return 'status-text-first-approval-approved'; // Yellow
    if (statusNum === PanelStatus.SecondApprovalApproved) return 'status-text-second-approval-approved'; // Green
    if (statusNum === PanelStatus.SecondApprovalRejected) return 'status-text-second-approval-rejected'; // Red
    
    return 'status-text-default';
  }

  /**
   * Check if panel is complete (SecondApprovalApproved/Green) for a given key name (legacy helper).
   */
  isPanelComplete(box: Box, panelName: string): boolean {
    const status = this.getAggregatedPanelStatus(box, {
      panelTypeId: panelName,
      projectId: this.selectedProjectId,
      panelTypeName: panelName,
      panelTypeCode: panelName,
      description: '',
      isActive: true,
      displayOrder: 0,
      createdDate: new Date(),
      modifiedDate: undefined
    });
    return status === PanelStatus.SecondApprovalApproved;
  }

  /**
   * Check if panel is yellow (FirstApprovalApproved) for a given key name (legacy helper).
   */
  isPanelYellow(box: Box, panelName: string): boolean {
    const status = this.getAggregatedPanelStatus(box, {
      panelTypeId: panelName,
      projectId: this.selectedProjectId,
      panelTypeName: panelName,
      panelTypeCode: panelName,
      description: '',
      isActive: true,
      displayOrder: 0,
      createdDate: new Date(),
      modifiedDate: undefined
    });
    return status === PanelStatus.FirstApprovalApproved;
  }

  /**
   * Expose PanelStatus enum to template
   */
  PanelStatus = PanelStatus;

  /**
   * Calculate percentage for a specific component
   */
  getCompletionPercentage(completed: number): number {
    if (this.filteredBoxes.length === 0) return 0;
    return Math.round((completed / this.filteredBoxes.length) * 100);
  }

  /**
   * Navigate to box details
   */
  viewBox(boxId: string): void {
    const box = this.filteredBoxes.find(b => b.id === boxId);
    if (box && box.projectId) {
      this.router.navigate(['/projects', box.projectId, 'boxes', boxId]);
    }
  }

  /**
   * Export filtered boxes data to Excel
   * Exports all filtered boxes (not just the current page)
   */
  exportToExcel(): void {
    if (!this.selectedProjectId || this.boxes.length === 0) {
      alert('No data to export. Please select a project and ensure there are boxes available.');
      return;
    }

    // Get all filtered boxes (not paginated)
    let boxesToExport = [...this.boxes];
    
    // Apply filters (same logic as applyFilters but without pagination)
    if (this.selectedProjectId) {
      boxesToExport = boxesToExport.filter(box => box.projectId === this.selectedProjectId);
    }
    if (this.selectedBuildingNumber) {
      boxesToExport = boxesToExport.filter(box => box.buildingNumber === this.selectedBuildingNumber);
    }
    if (this.selectedFloor) {
      boxesToExport = boxesToExport.filter(box => box.floor === this.selectedFloor);
    }

    if (boxesToExport.length === 0) {
      alert('No data to export after applying filters.');
      return;
    }

    // Prepare headers
    const headers: string[] = [
      'Box Tag',
      'Box Number',
      'Serial Number',
      'Building',
      'Floor'
    ];

    // Add dynamic panel type columns
    this.panelTypes.forEach(panelType => {
      headers.push(panelType.panelTypeName);
    });

    // Add delivery columns
    headers.push('Slab', 'Soffit', 'POD Deliver', 'POD Name', 'POD Type');

    // Prepare data rows
    const dataRows: any[] = boxesToExport.map(box => {
      const row: any = {
        'Box Tag': box.code || '',
        'Box Number': box.boxNumber || '',
        'Serial Number': box.serialNumber || '',
        'Building': box.buildingNumber || '-',
        'Floor': box.floor || '-'
      };

      // Add panel statuses for each panel type
      this.panelTypes.forEach(panelType => {
        const panels = this.getPanelsForPanelType(box, panelType);
        if (panels.length === 0) {
          row[panelType.panelTypeName] = 'Not Have This Panel';
        } else {
          // Combine all panels of this type with their statuses
          const panelStatuses = panels.map(panel => {
            const statusLabel = this.getPanelStatusLabel(box, panel.typeName || panel.panelName);
            return `${panel.panelName} ${statusLabel}`;
          });
          row[panelType.panelTypeName] = panelStatuses.join('; ');
        }
      });

      // Add delivery information
      row['Slab'] = box.slab ? 'Yes' : 'No';
      row['Soffit'] = box.soffit ? 'Yes' : 'No';
      row['POD Deliver'] = box.podDeliver ? 'Yes' : 'No';
      row['POD Name'] = box.podName || '-';
      row['POD Type'] = box.podType || '-';

      return row;
    });

    // Create worksheet
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataRows);

    // Set column widths
    const columnWidths: any[] = [
      { wch: 20 }, // Box Tag
      { wch: 15 }, // Box Number
      { wch: 18 }, // Serial Number
      { wch: 12 }, // Building
      { wch: 10 }  // Floor
    ];

    // Add widths for panel type columns
    this.panelTypes.forEach(() => {
      columnWidths.push({ wch: 35 }); // Panel status columns
    });

    // Add widths for delivery columns
    columnWidths.push(
      { wch: 10 }, // Slab
      { wch: 10 }, // Soffit
      { wch: 12 }, // POD Deliver
      { wch: 20 }, // POD Name
      { wch: 15 }  // POD Type
    );

    worksheet['!cols'] = columnWidths;

    // Create workbook
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Panels Status': worksheet },
      SheetNames: ['Panels Status']
    };

    // Generate file name with current date and factory name
    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    const factoryCode = this.factory?.factoryCode || 'factory';
    const fileName = `Panels_Slab_Soffit_Status_${factoryCode}_${dateStr}.xlsx`;

    // Save file
    XLSX.writeFile(workbook, fileName);
  }

  // Expose Math to template
  Math = Math;
}

