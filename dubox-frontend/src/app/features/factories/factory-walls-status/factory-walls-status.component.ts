import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { FactoryService, Factory } from '../../../core/services/factory.service';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxPanel, PanelStatus } from '../../../core/models/box.model';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

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
  boxes: Box[] = []; // Current page boxes
  allBoxes: Box[] = []; // All boxes for filter options
  filteredBoxes: Box[] = [];
  projects: Project[] = [];
  allProjects: Project[] = []; // Store all projects before filtering
  selectedProjectId: string = '';
  selectedBuildingNumber: string = '';
  selectedFloor: string = '';
  selectedZone: string = '';
  availableBuildingNumbers: string[] = [];
  availableFloors: string[] = [];
  availableZones: string[] = [];
  loading = true;
  error = '';
  
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
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.factoryId = this.route.snapshot.params['id'];
    this.loadProjects();
    this.loadData();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadData(): void {
    this.loading = true;
    this.error = '';

    // Load factory details
    const factorySub = this.factoryService.getFactoryById(this.factoryId).subscribe({
      next: (factory: Factory) => {
        this.factory = factory;
        this.loadBoxes();
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
    const projectsSub = this.projectService.getProjects().subscribe({
      next: (projects: Project[]) => {
        // Store all non-archived projects
        this.allProjects = projects.filter(p => p.status !== 'Archived');
        // Update projects list based on boxes (will be called after boxes are loaded)
        this.updateProjectsList();
      },
      error: (err: any) => {
        console.error('Error loading projects:', err);
      }
    });

    this.subscriptions.push(projectsSub);
  }

  loadBoxes(): void {
    this.loading = true;
    const allBoxesSub = this.boxService.getBoxesByFactoryPaginated(
      this.factoryId, 
      1, 
      100 // Max page size for filter options
    ).subscribe({
      next: (response) => {
        // Store boxes with position data for filter options
        this.allBoxes = response.items.filter(box => box.bay || box.row || box.position);
        this.updateProjectsList();
        this.updateBuildingsAndLevels();
      },
      error: (err: any) => {
        console.error('Error loading boxes for filters:', err);
      }
    });

    // Load paginated boxes for display
    const boxesSub = this.boxService.getBoxesByFactoryPaginated(
      this.factoryId, 
      this.currentPage, 
      this.pageSize
    ).subscribe({
      next: (response) => {
        // Filter boxes with position data
        this.boxes = response.items.filter(box => box.bay || box.row || box.position);
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.currentPage = response.pageNumber;
        
        // Apply filters to the current page boxes
        this.applyFilters();
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load boxes';
        console.error('Error loading boxes:', err);
        this.loading = false;
      }
    });

    this.subscriptions.push(allBoxesSub);
    this.subscriptions.push(boxesSub);
  }

  /**
   * Handle page change
   */
  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadBoxes();
    }
  }

  /**
   * Handle page size change
   */
  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.currentPage = 1; // Reset to first page when changing page size
    this.loadBoxes();
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
   * Update projects list to only show projects that have boxes displayed on this page
   * Boxes must have position data (bay, row, or position) to be considered
   */
  updateProjectsList(): void {
    if (this.boxes.length === 0 || this.allProjects.length === 0) {
      // If boxes aren't loaded yet, show all projects temporarily
      this.projects = [...this.allProjects];
      return;
    }

    // Get unique project IDs from boxes that have position data
    // Only boxes with position data are displayed on the page
    const projectIdsWithBoxes = new Set<string>();
    this.boxes.forEach(box => {
      // Only include boxes that have position information (bay, row, or position)
      if (box.projectId && (box.bay || box.row || box.position)) {
        projectIdsWithBoxes.add(box.projectId);
      }
    });

    // Filter projects to only show those with boxes on the page
    this.projects = this.allProjects.filter(p => projectIdsWithBoxes.has(p.id));
  }

  /**
   * Update available building numbers, floors, and zones from boxes
   * If a project is selected, only show options for that project
   */
  updateBuildingsAndLevels(): void {
    if (this.allBoxes.length === 0) {
      return;
    }

    // Filter boxes based on selected project if one is selected
    // Use allBoxes for filter options (not just current page)
    const boxesToConsider = this.selectedProjectId
      ? this.allBoxes.filter(box => box.projectId === this.selectedProjectId)
      : this.allBoxes;

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

    // Get unique zones
    const zonesSet = new Set<string>();
    boxesToConsider.forEach(box => {
      if (box.zone) {
        zonesSet.add(box.zone);
      }
    });
    this.availableZones = Array.from(zonesSet).sort();

    // Clear selections if they're no longer valid after project change
    if (this.selectedProjectId) {
      if (this.selectedBuildingNumber && !this.availableBuildingNumbers.includes(this.selectedBuildingNumber)) {
        this.selectedBuildingNumber = '';
      }
      if (this.selectedFloor && !this.availableFloors.includes(this.selectedFloor)) {
        this.selectedFloor = '';
      }
      if (this.selectedZone && !this.availableZones.includes(this.selectedZone)) {
        this.selectedZone = '';
      }
    }
  }

  /**
   * Handle project filter change - update dependent filter options
   */
  onProjectChange(): void {
    // Update building and level options based on selected project
    this.updateBuildingsAndLevels();
    // Apply filters to update the grid
    this.applyFilters();
  }

  applyFilters(): void {
    // Note: Filters are now applied on the backend via pagination
    // For now, we'll filter the current page results client-side
    // In the future, filters could be passed to the backend as query parameters
    let filtered = [...this.boxes];

    // Filter by project
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

    // Filter by zone
    if (this.selectedZone) {
      filtered = filtered.filter(box => box.zone === this.selectedZone);
    }

    this.filteredBoxes = filtered;
    
    // If filters are applied, reload boxes to get fresh data
    // This ensures we're working with the full dataset for filtering
    if (this.selectedProjectId || this.selectedBuildingNumber || this.selectedFloor || this.selectedZone) {
      // Reset to page 1 when filters change
      if (this.currentPage !== 1) {
        this.currentPage = 1;
        this.loadBoxes();
      }
    }
  }

  clearFilters(): void {
    this.selectedProjectId = '';
    this.selectedBuildingNumber = '';
    this.selectedFloor = '';
    this.selectedZone = '';
    // Reset to page 1 when clearing filters
    this.currentPage = 1;
    // Update filter options to show all available options (not filtered by project)
    this.updateBuildingsAndLevels();
    this.loadBoxes();
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
    slabComplete: number;
    soffitComplete: number;
    allComplete: number;
    podDeliverComplete: number;
    panelStats: Map<string, { total: number; yellow: number; green: number }>;
  } {
    const stats = {
      total: this.filteredBoxes.length,
      panelsComplete: 0,
      panelsYellow: 0,
      panelsGreen: 0,
      slabComplete: 0,
      soffitComplete: 0,
      allComplete: 0,
      podDeliverComplete: 0,
      panelStats: new Map<string, { total: number; yellow: number; green: number }>()
    };

    this.filteredBoxes.forEach(box => {
      // Count panels by status
      if (box.boxPanels && box.boxPanels.length > 0) {
        stats.panelsComplete++;
        
        box.boxPanels.forEach((panel: BoxPanel) => {
          if (!stats.panelStats.has(panel.panelName)) {
            stats.panelStats.set(panel.panelName, { total: 0, yellow: 0, green: 0 });
          }
          const panelStat = stats.panelStats.get(panel.panelName)!;
          panelStat.total++;
          
          if (panel.panelStatus === PanelStatus.Yellow) {
            panelStat.yellow++;
            stats.panelsYellow++;
          } else if (panel.panelStatus === PanelStatus.Green) {
            panelStat.green++;
            stats.panelsGreen++;
          }
        });
      }
      
      if (box.slab) stats.slabComplete++;
      if (box.soffit) stats.soffitComplete++;
      if (box.podDeliver) stats.podDeliverComplete++;
      
      // All complete if box has panels (all green), slab, and soffit
      const hasPanels = box.boxPanels && box.boxPanels.length > 0;
      const allPanelsGreen = hasPanels && box.boxPanels!.every((p: BoxPanel) => p.panelStatus === PanelStatus.Green);
      if (allPanelsGreen && box.slab && box.soffit) {
        stats.allComplete++;
      }
    });

    return stats;
  }

  /**
   * Get unique panel names from all boxes
   */
  getUniquePanelNames(): string[] {
    const panelNames = new Set<string>();
    this.filteredBoxes.forEach(box => {
      if (box.boxPanels) {
        box.boxPanels.forEach((panel: BoxPanel) => panelNames.add(panel.panelName));
      }
    });
    return Array.from(panelNames).sort();
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
   * Get panel status for a box
   */
  getPanelStatus(box: Box, panelName: string): PanelStatus | null {
    
    const panel = box.boxPanels?.find((p: BoxPanel) => p.panelName === panelName);
   
    return panel ? panel.panelStatus : null;
  }

  /**
   * Check if panel is complete (Green)
   */
  isPanelComplete(box: Box, panelName: string): boolean {
    const status = this.getPanelStatus(box, panelName);
    return status === PanelStatus.Green;
  }

  /**
   * Check if panel is yellow
   */
  isPanelYellow(box: Box, panelName: string): boolean {
    const status = this.getPanelStatus(box, panelName);
    return status === PanelStatus.Yellow;
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

  // Expose Math to template
  Math = Math;
}

