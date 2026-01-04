import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Subscription, forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { FactoryService, Factory } from '../../../core/services/factory.service';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { WIRService } from '../../../core/services/wir.service';
import { WIRRecord } from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { 
  getCurrentActiveWIRStage, 
  getWIRStageInfo,
  WIRStageInfo,
  COMPLETED_WIR_STAGE
} from '../../../core/utils/wir-stage.util';

@Component({
  selector: 'app-factory-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './factory-layout.component.html',
  styleUrls: ['./factory-layout.component.scss']
})
export class FactoryLayoutComponent implements OnInit, OnDestroy {
  factoryId: string = '';
  factory: Factory | null = null;
  boxes: Box[] = [];
  // Map of boxId to WIR records for that box
  boxWIRRecordsMap: Map<string, WIRRecord[]> = new Map();
  loading = true;
  error = '';
  
  BoxStatus = BoxStatus;
  
  private subscriptions: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private factoryService: FactoryService,
    private boxService: BoxService,
    private wirService: WIRService
  ) {}

  ngOnInit(): void {
    this.factoryId = this.route.snapshot.params['id'];
    this.loadFactoryData();
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

  loadBoxes(): void {
    const boxesSub = this.boxService.getBoxesByFactory(this.factoryId).subscribe({
      next: (boxes: Box[]) => {
        // Backend already filters boxes (InProgress/Completed from active projects)
        // Just use the boxes directly and load WIR records
        this.boxes = boxes;
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
    // Backend already filters boxes (InProgress/Completed from active projects)
    // Only filter by position information for grid display
    const boxesWithPosition = this.boxes.filter(box => 
      (box.bay || box.row || box.position)
    );
    
    if (boxesWithPosition.length === 0) {
      return { rows: [], columns: [], matrix: [], totalBoxes: 0 };
    }

    // Get unique bays (columns - A, B, C, D) and rows (1, 2, 3, 4)
    const baysSet = new Set<string>();
    const rowsSet = new Set<string>();
    
    boxesWithPosition.forEach(box => {
      baysSet.add(box.bay || 'A');
      rowsSet.add(box.row || '1');
    });

    // Sort columns (bays) and rows
    const columns = Array.from(baysSet).sort((a, b) => a.localeCompare(b));
    const rows = Array.from(rowsSet).sort((a, b) => {
      const numA = parseInt(a) || 0;
      const numB = parseInt(b) || 0;
      return numA - numB;
    });

    // Create matrix structure
    const matrix: any[][] = [];
    let displayedBoxCount = 0;
    
    rows.forEach(row => {
      const rowCells: any[] = [];
      columns.forEach(column => {
        // Find ALL boxes at this position (there might be duplicates)
        const boxesAtPosition = boxesWithPosition.filter(b => 
          (b.bay || 'A') === column && (b.row || '1') === row
        );
        
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
      matrix.push(rowCells);
    });

    return {
      rows,
      columns,
      matrix,
      totalBoxes: boxesWithPosition.length, // Total boxes with positions
      displayedBoxes: displayedBoxCount // Unique positions with boxes
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

  /**
   * Get WIR stage statistics for legend
   * Returns an array of WIR stages with their counts and percentages
   */
  getWIRStageStats(): Array<{ stage: WIRStageInfo | null; count: number; percentage: number }> {
    const gridLayout = this.getGridLayout();
    if (gridLayout.totalBoxes === 0) return [];

    const stageCounts = new Map<string, number>();
    let completedCount = 0;
    let noWIRCount = 0;

    // Count boxes by WIR stage
    this.boxes.forEach(box => {
      if (!box.bay && !box.row && !box.position) return;
      if (box.status === BoxStatus.Dispatched) return;

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

    // Add WIR stages that have boxes
    stageCounts.forEach((count, wirCode) => {
      const stage = getWIRStageInfo(wirCode);
      stats.push({
        stage,
        count,
        percentage: Math.round((count / gridLayout.totalBoxes) * 100)
      });
    });

    // Add completed if there are any
    if (completedCount > 0) {
      stats.push({
        stage: COMPLETED_WIR_STAGE,
        count: completedCount,
        percentage: Math.round((completedCount / gridLayout.totalBoxes) * 100)
      });
    }

    // Sort by WIR number
    stats.sort((a, b) => {
      if (!a.stage || !b.stage) return 0;
      const numA = a.stage.wirCode === 'COMPLETED' ? 999 : parseInt(a.stage.wirCode.replace('WIR-', '')) || 0;
      const numB = b.stage.wirCode === 'COMPLETED' ? 999 : parseInt(b.stage.wirCode.replace('WIR-', '')) || 0;
      return numA - numB;
    });

    return stats;
  }

  getInProgressPercentage(): number {
    const gridLayout = this.getGridLayout();
    if (gridLayout.totalBoxes === 0) return 0;
    
    // Count boxes that are in any WIR stage (not completed)
    const inProgressCount = this.boxes.filter(b => {
      if (!b.bay && !b.row && !b.position) return false;
      if (b.status === BoxStatus.Dispatched) return false;
      const wirStage = this.getBoxWIRStage(b);
      return wirStage && wirStage.wirCode !== 'COMPLETED';
    }).length;
    
    return Math.round((inProgressCount / gridLayout.totalBoxes) * 100);
  }

  getCompletedPercentage(): number {
    const gridLayout = this.getGridLayout();
    if (gridLayout.totalBoxes === 0) return 0;
    
    // Count boxes that have completed all WIRs
    const completedCount = this.boxes.filter(b => {
      if (!b.bay && !b.row && !b.position) return false;
      if (b.status === BoxStatus.Dispatched) return false;
      const wirStage = this.getBoxWIRStage(b);
      return wirStage && wirStage.wirCode === 'COMPLETED';
    }).length;
    
    return Math.round((completedCount / gridLayout.totalBoxes) * 100);
  }

  getEmptyPercentage(): number {
    const gridLayout = this.getGridLayout();
    const totalCells = gridLayout.rows.length * gridLayout.columns.length;
    
    if (totalCells === 0) return 0;
    
    const occupiedCells = gridLayout.totalBoxes;
    const emptyCells = totalCells - occupiedCells;
    
    return Math.round((emptyCells / totalCells) * 100);
  }

  viewBox(boxId: string): void {
    // Find the box to get projectId
    const box = this.boxes.find(b => b.id === boxId);
    if (box && box.projectId) {
      this.router.navigate(['/projects', box.projectId, 'boxes', boxId]);
    } else {
      console.error('Box not found or missing projectId:', boxId);
    }
  }

  goBack(): void {
    this.router.navigate(['/factories', this.factoryId]);
  }
}