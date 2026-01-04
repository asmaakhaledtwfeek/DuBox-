import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Subscription } from 'rxjs';
import { FactoryService, Factory } from '../../../core/services/factory.service';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

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
  loading = true;
  error = '';
  
  BoxStatus = BoxStatus;
  
  private subscriptions: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private factoryService: FactoryService,
    private boxService: BoxService
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
        this.boxes = boxes;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load boxes';
        console.error('Error loading boxes:', err);
        this.loading = false;
      }
    });

    this.subscriptions.push(boxesSub);
  }

  getGridLayout(): any {
    // Filter boxes that have position information and exclude dispatched boxes
    const boxesWithPosition = this.boxes.filter(box => 
      (box.bay || box.row || box.position) && box.status !== BoxStatus.Dispatched
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
    
    rows.forEach(row => {
      const rowCells: any[] = [];
      columns.forEach(column => {
        // Find box at this position
        const box = boxesWithPosition.find(b => 
          (b.bay || 'A') === column && (b.row || '1') === row
        );
        
        rowCells.push({
          row,
          column,
          bay: column,
          box: box || null,
          position: box?.position || null
        });
      });
      matrix.push(rowCells);
    });

    return {
      rows,
      columns,
      matrix,
      totalBoxes: boxesWithPosition.length
    };
  }

  getCellTooltip(cell: any): string {
    if (cell.box) {
      const status = this.getStatusLabel(cell.box.status);
      return `${cell.box.code}\nBay: ${cell.bay}, Row: ${cell.row}\nPosition: ${cell.position || '-'}\nStatus: ${status}\nProgress: ${cell.box.progress}%`;
    }
    return `Available\nBay: ${cell.bay}, Row: ${cell.row}`;
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

  getInProgressPercentage(): number {
    const gridLayout = this.getGridLayout();
    if (gridLayout.totalBoxes === 0) return 0;
    
    const inProgressCount = this.boxes.filter(b => 
      b.status === BoxStatus.InProgress && 
      (b.bay || b.row || b.position)
    ).length;
    
    return Math.round((inProgressCount / gridLayout.totalBoxes) * 100);
  }

  getCompletedPercentage(): number {
    const gridLayout = this.getGridLayout();
    if (gridLayout.totalBoxes === 0) return 0;
    
    const completedCount = this.boxes.filter(b => 
      b.status === BoxStatus.Completed && 
      (b.bay || b.row || b.position)
    ).length;
    
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