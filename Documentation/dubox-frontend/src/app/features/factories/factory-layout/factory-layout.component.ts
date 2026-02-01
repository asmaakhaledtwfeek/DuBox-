import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subscription, forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { FactoryService, Factory } from '../../../core/services/factory.service';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { WIRService } from '../../../core/services/wir.service';
import { WIRRecord } from '../../../core/models/wir.model';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { BoxQuickViewModalComponent } from '../box-quick-view-modal/box-quick-view-modal.component';
import { 
  getCurrentActiveWIRStage, 
  getWIRStageInfo,
  WIRStageInfo,
  COMPLETED_WIR_STAGE,
  WIR_STAGE_COLORS
} from '../../../core/utils/wir-stage.util';

@Component({
  selector: 'app-factory-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent, BoxQuickViewModalComponent],
  templateUrl: './factory-layout.component.html',
  styleUrls: ['./factory-layout.component.scss']
})
export class FactoryLayoutComponent implements OnInit, OnDestroy {
  factoryId: string = '';
  factory: Factory | null = null;
  boxes: Box[] = [];
  filteredBoxes: Box[] = [];
  // Map of boxId to WIR records for that box
  boxWIRRecordsMap: Map<string, WIRRecord[]> = new Map();
  loading = true;
  error = '';
  
  // Filter properties
  projects: Project[] = [];
  allProjects: Project[] = []; // Store all projects before filtering
  selectedProjectId: string = '';
  selectedStage: string = '';
  selectedBuilding: string = '';
  selectedFloor: string = '';
  selectedLevel: string = '';
  selectedZone: string = '';
  availableStages: Array<{ wirCode: string; displayName: string }> = [];
  
  // Available filter options
  availableBuildings: string[] = [];
  availableFloors: string[] = [];
  availableLevels: string[] = [];
  availableZones: string[] = [];
  
  BoxStatus = BoxStatus;
  
  // Modal state
  isBoxQuickViewModalOpen = false;
  selectedBoxId: string = '';
  selectedBoxProjectId: string = '';
  
  private subscriptions: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private factoryService: FactoryService,
    private boxService: BoxService,
    private wirService: WIRService,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.factoryId = this.route.snapshot.params['id'];
    this.initializeAvailableStages();
    this.loadProjects();
    this.loadFactoryData();
  }
  
  /**
   * Initialize available stages with predefined WIR stages from BUSINESS FLOW INDICATORS
   */
  initializeAvailableStages(): void {
    // Use predefined WIR stages from WIR_STAGE_COLORS
    // Remove the code part (e.g., "- WIR-1") from display name
    this.availableStages = Object.values(WIR_STAGE_COLORS).map(stage => ({
      wirCode: stage.wirCode,
      displayName: stage.displayName.replace(/\s*-\s*WIR-\d+$/, '')
    }));
    
    // Add Completed stage
    this.availableStages.push({
      wirCode: COMPLETED_WIR_STAGE.wirCode,
      displayName: COMPLETED_WIR_STAGE.displayName
    });
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadFactoryData(): void {
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

  /**
   * Update projects list to only show projects that have boxes in the current layout
   * (boxes that have bay, row, or position assigned)
   */
  updateProjectsList(): void {
    if (this.boxes.length === 0 || this.allProjects.length === 0) {
      // If boxes aren't loaded yet, show all projects temporarily
      this.projects = [...this.allProjects];
      return;
    }

    // Get unique project IDs from boxes that have positions in the layout
    const projectIdsWithBoxesInLayout = new Set<string>();
    this.boxes.forEach(box => {
      // Only include boxes that have position information (bay, row, or position)
      if (box.bay || box.row || box.position) {
        projectIdsWithBoxesInLayout.add(box.projectId);
      }
    });

    // Filter projects to only show those with boxes in layout
    this.projects = this.allProjects.filter(p => projectIdsWithBoxesInLayout.has(p.id));
  }

  loadBoxes(): void {
    const boxesSub = this.boxService.getBoxesByFactory(this.factoryId).subscribe({
      next: (boxes: Box[]) => {
        // Backend already filters boxes (InProgress/Completed from active projects)
        // Just use the boxes directly and load WIR records
        this.boxes = boxes;
        this.filteredBoxes = boxes;
        this.updateProjectsList();
        this.updateFilterOptions();
        this.loadWIRRecordsForBoxes(boxes);
      },
      error: (err: any) => {
        this.error = 'Failed to load boxes';
        console.error('Error loading boxes:', err);
        this.loading = false;
      }
    });

    this.subscriptions.push(boxesSub);
  }

  /**
   * Update available filter options based on boxes in the layout
   * If a project is selected, only show options for that project
   */
  updateFilterOptions(): void {
    // Get unique values from boxes that have positions in the layout
    const buildingsSet = new Set<string>();
    const floorsSet = new Set<string>();
    const levelsSet = new Set<string>();
    const zonesSet = new Set<string>();

    // Filter boxes based on selected project if one is selected
    const boxesToConsider = this.selectedProjectId
      ? this.boxes.filter(box => box.projectId === this.selectedProjectId)
      : this.boxes;

    boxesToConsider.forEach(box => {
      // Only consider boxes that have position information
      if (box.bay || box.row || box.position) {
        if (box.buildingNumber) {
          buildingsSet.add(box.buildingNumber);
        }
        if (box.floor) {
          floorsSet.add(box.floor);
        }
        // For level, we'll use floor as level (since there's no separate level property)
        // If level needs to be different, it can be added later
        if (box.floor) {
          levelsSet.add(box.floor);
        }
        if (box.zone) {
          zonesSet.add(box.zone);
        }
      }
    });

    this.availableBuildings = Array.from(buildingsSet).sort();
    this.availableFloors = Array.from(floorsSet).sort();
    this.availableLevels = Array.from(levelsSet).sort();
    this.availableZones = Array.from(zonesSet).sort();

    // Clear selections if they're no longer valid after project change
    if (this.selectedProjectId) {
      if (this.selectedBuilding && !this.availableBuildings.includes(this.selectedBuilding)) {
        this.selectedBuilding = '';
      }
      if (this.selectedFloor && !this.availableFloors.includes(this.selectedFloor)) {
        this.selectedFloor = '';
      }
      if (this.selectedLevel && !this.availableLevels.includes(this.selectedLevel)) {
        this.selectedLevel = '';
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
    // Update filter options based on selected project
    this.updateFilterOptions();
    // Apply filters to update the grid
    this.applyFilters();
  }

  /**
   * Handle click on stage card in BUSINESS FLOW INDICATORS
   * Toggles the stage filter - clicking again clears it
   */
  onStageCardClick(wirCode: string): void {
    // If clicking the same stage, clear the filter
    if (this.selectedStage === wirCode) {
      this.selectedStage = '';
    } else {
      // Set the new stage filter
      this.selectedStage = wirCode;
    }
    // Apply filters to update the grid
    this.applyFilters();
  }

  applyFilters(): void {
    let filtered = [...this.boxes];

    // When filtering by project or stage, show only InProgress or Completed boxes
    // Exception: EMPTY filter should show all boxes (to identify empty cells)
    if (this.selectedProjectId || (this.selectedStage && this.selectedStage !== 'EMPTY')) {
      filtered = filtered.filter(box => 
        box.status === BoxStatus.InProgress || box.status === BoxStatus.Completed
      );
    }

    // Filter by project
    if (this.selectedProjectId) {
      filtered = filtered.filter(box => box.projectId === this.selectedProjectId);
    }

    // Filter by stage
    if (this.selectedStage) {
      if (this.selectedStage === 'EMPTY') {
        // For EMPTY filter, we'll handle it in getGridLayout() by showing empty cells
        // Here we just don't filter boxes - empty cells will be shown in the grid
        // But we still need to filter out boxes that have positions to show empty cells
        // Actually, we want to show ALL positions, but highlight empty ones
        // So we don't filter boxes here - the grid will show empty cells
      } else {
        filtered = filtered.filter(box => {
          const wirStage = this.getBoxWIRStage(box);
          return wirStage && wirStage.wirCode === this.selectedStage;
        });
      }
    }

    // Filter by building
    if (this.selectedBuilding) {
      filtered = filtered.filter(box => box.buildingNumber === this.selectedBuilding);
    }

    // Filter by floor
    if (this.selectedFloor) {
      filtered = filtered.filter(box => box.floor === this.selectedFloor);
    }

    // Filter by level (using floor as level since there's no separate level property)
    if (this.selectedLevel) {
      filtered = filtered.filter(box => box.floor === this.selectedLevel);
    }

    // Filter by zone
    if (this.selectedZone) {
      filtered = filtered.filter(box => box.zone === this.selectedZone);
    }

    this.filteredBoxes = filtered;
  }

  clearFilters(): void {
    this.selectedProjectId = '';
    this.selectedStage = '';
    this.selectedBuilding = '';
    this.selectedFloor = '';
    this.selectedLevel = '';
    this.selectedZone = '';
    // Update filter options to show all available options (not filtered by project)
    this.updateFilterOptions();
    this.filteredBoxes = [...this.boxes];
  }

  /**
   * Check if a stage card is currently active/selected
   */
  isStageCardActive(wirCode: string): boolean {
    return this.selectedStage === wirCode;
  }

  /**
   * Load WIR records for all boxes in parallel
   * This allows us to determine the current WIR stage for color coding
   */
  loadWIRRecordsForBoxes(boxes: Box[]): void {
    if (boxes.length === 0) {
      this.loading = false;
      return;
    }

    // Create observables for loading WIR records for each box
    const wirObservables = boxes.map(box => 
      this.wirService.getWIRRecordsByBox(box.id).pipe(
        map((wirs: WIRRecord[]) => ({ boxId: box.id, wirs })),
        catchError(err => {
          console.warn(`Failed to load WIR records for box ${box.id}:`, err);
          // Return empty array on error
          return of({ boxId: box.id, wirs: [] as WIRRecord[] });
        })
      )
    );

    // Load all WIR records in parallel
    const wirSub = forkJoin(wirObservables).subscribe({
      next: (results) => {
        // Build the map of boxId to WIR records
        this.boxWIRRecordsMap.clear();
        results.forEach(result => {
          this.boxWIRRecordsMap.set(result.boxId, result.wirs);
        });

        // Note: Box positions (bay, row, position) come directly from the Box entity
        // WIR records are only used for color coding based on WIR stage
        
        this.loading = false;
      },
      error: (err: any) => {
        console.error('Error loading WIR records:', err);
        // Continue even if WIR loading fails - boxes will use default colors
        this.loading = false;
      }
    });

    this.subscriptions.push(wirSub);
  }

  /**
   * Box positions (bay, row, position) are now taken directly from the Box entity
   * which is stored in the database. WIR records are only used for color coding.
   * 
   * The getGridLayout() method uses box.bay, box.row, and box.position directly
   * to determine where each box should be displayed in the factory layout grid.
   */

  getGridLayout(): any {
    // Use factory's min/max row and bay values from database to determine the full layout range
    // This ensures we always show the complete factory layout as defined in the database
    if (!this.factory) {
      return { rows: [], columns: [], matrix: [], totalBoxes: 0 };
    }

    // Get min/max values from factory (with fallback defaults if not set)
    const minRow = this.factory.minRow ?? 1;
    const maxRow = this.factory.maxRow ?? 20;
    const minBay = this.factory.minBay ?? 'A';
    const maxBay = this.factory.maxBay ?? 'Z';

    // Get filtered boxes to determine which boxes to display in the grid
    // For EMPTY filter, we need all boxes to identify empty cells
    const allBoxesWithPosition = this.boxes.filter(box => (box.bay || box.row || box.position));
    const boxesForDisplay = this.selectedStage === 'EMPTY' 
      ? allBoxesWithPosition
      : this.filteredBoxes.filter(box => (box.bay || box.row || box.position));

    // Generate all bays from min to max (A-Z range) based on factory configuration
    const allBays: string[] = [];
    if (minBay && maxBay) {
      const startCharCode = minBay.toUpperCase().charCodeAt(0);
      const endCharCode = maxBay.toUpperCase().charCodeAt(0);
      for (let i = startCharCode; i <= endCharCode; i++) {
        allBays.push(String.fromCharCode(i));
      }
    }

    // Generate all rows from min to max based on factory configuration
    const allRows: string[] = [];
    for (let i = minRow; i <= maxRow; i++) {
      allRows.push(i.toString());
    }

    // Use complete range
    const columns = allBays;
    const rows = allRows;

    // Create matrix structure with ALL combinations
    const matrix: any[][] = [];
    let displayedBoxCount = 0;
    
    rows.forEach(row => {
      const rowCells: any[] = [];
      columns.forEach(column => {
        // Find boxes at this position from filtered boxes (for display)
        const boxesAtPosition = boxesForDisplay.filter(b => 
          (b.bay || '') === column && (b.row || '') === row
        );
        
        // For EMPTY filter, only show empty cells (no boxes)
        if (this.selectedStage === 'EMPTY' && boxesAtPosition.length > 0) {
          // Skip this cell - it has a box, so it's not empty
          rowCells.push({
            row,
            column,
            bay: column,
            box: null,
            position: null,
            boxCount: 0,
            allBoxes: [],
            hidden: true // Mark as hidden for EMPTY filter
          });
          return;
        }
        
        // Use the first box for display, but track if there are multiple
        const box = boxesAtPosition.length > 0 ? boxesAtPosition[0] : null;
        const hasMultipleBoxes = boxesAtPosition.length > 1;
        
        if (box) {
          displayedBoxCount++;
          if (hasMultipleBoxes) {
            console.warn(`Multiple boxes (${boxesAtPosition.length}) at position Bay: ${column}, Row: ${row}:`, 
              boxesAtPosition.map(b => b.code).join(', '));
          }
        }
        
        rowCells.push({
          row,
          column,
          bay: column,
          box: box || null,
          position: box?.position || null,
          boxCount: boxesAtPosition.length, // Track how many boxes are at this position
          allBoxes: boxesAtPosition // Keep reference to all boxes at this position
        });
      });
      
      // For EMPTY filter, only add rows that have at least one visible (empty) cell
      // For other filters, always add all rows to show full layout
      if (this.selectedStage === 'EMPTY') {
        const hasVisibleCells = rowCells.some(cell => !cell.hidden);
        if (hasVisibleCells) {
          matrix.push(rowCells);
        }
      } else {
        // Always add all rows to show full layout with filtered boxes
        matrix.push(rowCells);
      }
    });

    return {
      rows,
      columns,
      matrix,
      totalBoxes: boxesForDisplay.length, // Total filtered boxes with positions
      displayedBoxes: displayedBoxCount, // Unique positions with filtered boxes
      totalBoxesInFactory: allBoxesWithPosition.length // Total boxes in factory (for reference)
    };
  }

  getCellTooltip(cell: any): string {
    if (cell.box) {
      const status = this.getStatusLabel(cell.box.status);
      const wirStage = this.getBoxWIRStage(cell.box);
      const wirStageName = wirStage ? `\nWIR Stage: ${wirStage.displayName}` : '';
      let tooltip = `${cell.box.code}\nBay: ${cell.bay}, Row: ${cell.row}\nPosition: ${cell.position || '-'}\nStatus: ${status}${wirStageName}\nProgress: ${cell.box.progress}%`;
      
      // Add warning if multiple boxes share this position
      if (cell.boxCount > 1) {
        const otherBoxes = cell.allBoxes.slice(1).map((b: Box) => b.code).join(', ');
        tooltip += `\n\n⚠️ Position conflict: ${cell.boxCount - 1} more box(es) share this position:\n${otherBoxes}`;
      }
      
      return tooltip;
    }
    return `Available\nBay: ${cell.bay}, Row: ${cell.row}`;
  }

  /**
   * Get the current active WIR stage for a box
   * Used for determining the color class
   * Considers box progress and status to avoid false completion
   */
  getBoxWIRStage(box: Box): WIRStageInfo | null {
    const wirRecords = this.boxWIRRecordsMap.get(box.id) || [];
    return getCurrentActiveWIRStage(wirRecords, box.progress, box.status);
  }

  /**
   * Get the CSS class for a box based on its WIR stage
   * Falls back to box status if no WIR stage is available
   */
  getBoxColorClass(box: Box): string {
    const wirStage = this.getBoxWIRStage(box);
    
    if (wirStage) {
      return wirStage.colorClass;
    }
    
    // Fallback to box status if no WIR records exist
    if (box.status === BoxStatus.Completed) {
      return 'completed';
    } else if (box.status === BoxStatus.InProgress) {
      return 'in-progress';
    }
    
    return 'empty';
  }

  getStatusLabel(status: BoxStatus): string {
    switch (status) {
      case BoxStatus.NotStarted: return 'Not Started';
      case BoxStatus.InProgress: return 'In Progress';
      case BoxStatus.Completed: return 'Completed';
      case BoxStatus.OnHold: return 'On Hold';
      case BoxStatus.Dispatched: return 'Dispatched';
      case BoxStatus.QAReview: return 'QA Review';
      case BoxStatus.ReadyForDelivery: return 'Ready for Delivery';
      case BoxStatus.Delivered: return 'Delivered';
      default: return status;
    }
  }

 
  getWIRStageStats(): Array<{ stage: WIRStageInfo | null; count: number; percentage: number }> {
    const gridLayout = this.getGridLayout();
    const totalSlots = gridLayout.rows.length * gridLayout.columns.length;
    
    if (totalSlots === 0) return [];

    const stageCounts = new Map<string, number>();
    let completedCount = 0;
    let noWIRCount = 0;

    // Count boxes by WIR stage (use ALL boxes with positions, not filtered, for accurate percentages)
    // But only count boxes that are not dispatched
    const boxesToCount = this.boxes.filter(box => 
      (box.bay || box.row || box.position) && box.status !== BoxStatus.Dispatched
    );

    boxesToCount.forEach(box => {
      const wirStage = this.getBoxWIRStage(box);
      if (wirStage) {
        if (wirStage.wirCode === 'COMPLETED') {
          completedCount++;
        } else {
          const count = stageCounts.get(wirStage.wirCode) || 0;
          stageCounts.set(wirStage.wirCode, count + 1);
        }
      } else {
        noWIRCount++;
      }
    });

    const stats: Array<{ stage: WIRStageInfo | null; count: number; percentage: number }> = [];

    // If a stage filter is active (and not EMPTY), only show that stage
    if (this.selectedStage && this.selectedStage !== 'EMPTY') {
      // Find the selected stage and add it to stats
      if (this.selectedStage === 'COMPLETED') {
        if (completedCount > 0) {
          const percentage = totalSlots > 0 ? (completedCount / totalSlots) * 100 : 0;
          const rounded = Math.round(percentage * 10) / 10;
          const finalPercentage = (completedCount > 0 && rounded === 0) ? 0.1 : rounded;
         
          stats.push({
            stage: COMPLETED_WIR_STAGE,
            count: completedCount,
            percentage: finalPercentage
          });
        }
      } else {
        const count = stageCounts.get(this.selectedStage) || 0;
        if (count > 0) {
          const stage = getWIRStageInfo(this.selectedStage);
          const percentage = totalSlots > 0 ? (count / totalSlots) * 100 : 0;
          const rounded = Math.round(percentage * 10) / 10;
          const finalPercentage = (count > 0 && rounded === 0) ? 0.1 : rounded;
          
          stats.push({
            stage,
            count,
            percentage: finalPercentage
          });
        }
      }
    } else {
      // No stage filter - show all stages
      // Add WIR stages that have boxes
      // Percentage is calculated against total slots (cells) in the grid
      stageCounts.forEach((count, wirCode) => {
        const stage = getWIRStageInfo(wirCode);
        const percentage = totalSlots > 0 ? (count / totalSlots) * 100 : 0;
        const rounded = Math.round(percentage * 10) / 10; // Round to 1 decimal place
        
        // If percentage is very small but there are boxes, show at least 0.1%
        const finalPercentage = (count > 0 && rounded === 0) ? 0.1 : rounded;
        
        stats.push({
          stage,
          count,
          percentage: finalPercentage
        });
      });

      // Add completed if there are any
      if (completedCount > 0) {
        const percentage = totalSlots > 0 ? (completedCount / totalSlots) * 100 : 0;
        const rounded = Math.round(percentage * 10) / 10;
        
        // If percentage is very small but there are boxes, show at least 0.1%
        const finalPercentage = (completedCount > 0 && rounded === 0) ? 0.1 : rounded;
        
        stats.push({
          stage: COMPLETED_WIR_STAGE,
          count: completedCount,
          percentage: finalPercentage
        });
      }

      // Sort by WIR number
      stats.sort((a, b) => {
        if (!a.stage || !b.stage) return 0;
        const numA = a.stage.wirCode === 'COMPLETED' ? 999 : parseInt(a.stage.wirCode.replace('WIR-', '')) || 0;
        const numB = b.stage.wirCode === 'COMPLETED' ? 999 : parseInt(b.stage.wirCode.replace('WIR-', '')) || 0;
        return numA - numB;
      });
    }

    return stats;
  }

  /**
   * Get the sum of all stage percentages
   * Used to calculate empty percentage as remainder
   */
  private getTotalStagePercentage(): number {
    const stats = this.getWIRStageStats();
    return stats.reduce((sum, stat) => sum + stat.percentage, 0);
  }

  getInProgressPercentage(): number {
    // Calculate total slots based on factory's min/max row and bay configuration
    const gridLayout = this.getGridLayout();
    const totalSlots = gridLayout.rows.length * gridLayout.columns.length;
    
    if (totalSlots === 0) return 0;
    
    // Count boxes that are in any WIR stage (not completed) - use ALL boxes with positions
    const inProgressCount = this.boxes.filter(b => {
      if (!b.bay && !b.row && !b.position) return false;
      if (b.status === BoxStatus.Dispatched) return false;
      const wirStage = this.getBoxWIRStage(b);
      return wirStage && wirStage.wirCode !== 'COMPLETED';
    }).length;
    
    // Percentage is calculated against total slots from factory configuration
    return Math.round((inProgressCount / totalSlots) * 100);
  }

  getCompletedPercentage(): number {
    // Calculate total slots based on factory's min/max row and bay configuration
    const gridLayout = this.getGridLayout();
    const totalSlots = gridLayout.rows.length * gridLayout.columns.length;
    
    if (totalSlots === 0) return 0;
    
    // Count boxes that have completed all WIRs - use ALL boxes with positions
    const completedCount = this.boxes.filter(b => {
      if (!b.bay && !b.row && !b.position) return false;
      if (b.status === BoxStatus.Dispatched) return false;
      const wirStage = this.getBoxWIRStage(b);
      return wirStage && wirStage.wirCode === 'COMPLETED';
    }).length;
    
    // Percentage is calculated against total slots from factory configuration
    return Math.round((completedCount / totalSlots) * 100);
  }

  getEmptyPercentage(): number {
    const gridLayout = this.getGridLayout();
    // Calculate total cells based on factory's min/max row and bay configuration
    const totalCells = gridLayout.rows.length * gridLayout.columns.length;
    
    if (totalCells === 0) return 0;
    
    // Calculate empty percentage as remainder to ensure sum equals 100%
    // This accounts for rounding errors in individual stage percentages
    // Empty percentage represents unoccupied slots in the factory layout
    const totalStagePercentage = this.getTotalStagePercentage();
    const emptyPercentage = Math.max(0, 100 - totalStagePercentage);
    
    // Round to 1 decimal place to match stage percentages
    return Math.round(emptyPercentage * 10) / 10;
  }

  /**
   * Get the color class for a WIR stage
   * Used for coloring the stage dropdown options
   */
  getStageColorClass(wirCode: string): string {
    if (wirCode === 'COMPLETED') {
      return 'wir-completed';
    }
    const stageInfo = getWIRStageInfo(wirCode);
    return stageInfo.colorClass;
  }

  viewBox(boxId: string): void {
    // Find the box to get projectId
    const box = this.boxes.find(b => b.id === boxId);
    if (box && box.projectId) {
      this.selectedBoxId = boxId;
      this.selectedBoxProjectId = box.projectId;
      this.isBoxQuickViewModalOpen = true;
    } else {
      console.error('Box not found or missing projectId:', boxId);
    }
  }

  closeBoxQuickViewModal(): void {
    this.isBoxQuickViewModalOpen = false;
    this.selectedBoxId = '';
    this.selectedBoxProjectId = '';
  }

  goBack(): void {
    this.router.navigate(['/factories', this.factoryId]);
  }

  /**
   * Navigate to walls status page
   */
  viewWallsStatus(): void {
    this.router.navigate(['/factories', this.factoryId, 'walls-status']);
  }
}