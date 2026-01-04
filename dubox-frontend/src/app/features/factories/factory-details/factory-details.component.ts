import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { FactoryService, Factory, ProjectLocation } from '../../../core/services/factory.service';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-factory-details',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './factory-details.component.html',
  styleUrls: ['./factory-details.component.scss']
})
export class FactoryDetailsComponent implements OnInit, OnDestroy {
  factoryId: string = '';
  factory: Factory | null = null;
  boxes: Box[] = [];
  filteredBoxes: Box[] = [];
  loading = true;
  error = '';
  
  searchControl = new FormControl('');
  selectedStatus: BoxStatus | 'All' = 'All';
  BoxStatus = BoxStatus;
  ProjectLocation = ProjectLocation; // Expose to template
  
  private subscriptions: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private factoryService: FactoryService,
    private boxService: BoxService
  ) {}

  ngOnInit(): void {
    this.factoryId = this.route.snapshot.params['id'];
    this.loadFactoryDetails();
    this.setupSearch();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadFactoryDetails(): void {
    this.loading = true;
    this.error = '';
    
    // Load factory and boxes in parallel
    this.factoryService.getFactoryById(this.factoryId).subscribe({
      next: (factory) => {
        this.factory = factory;
        this.loadBoxes();
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load factory details';
        this.loading = false;
        console.error('Error loading factory:', err);
      }
    });
  }

  loadBoxes(): void {
    this.boxService.getBoxesByFactory(this.factoryId).subscribe({
      next: (boxes) => {
        this.boxes = boxes;
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load boxes';
        this.loading = false;
        console.error('Error loading boxes:', err);
      }
    });
  }

  private setupSearch(): void {
    this.subscriptions.push(
      this.searchControl.valueChanges
        .pipe(
          debounceTime(300),
          distinctUntilChanged()
        )
        .subscribe(() => {
          this.applyFilters();
        })
    );
  }

  applyFilters(): void {
    // Filter out dispatched boxes from display (but keep them for counting)
    let filtered = this.boxes.filter(box => box.status !== BoxStatus.Dispatched);
    const searchTerm = this.searchControl.value?.toLowerCase() || '';
    
    if (searchTerm) {
      filtered = filtered.filter(box => {
        return box.code?.toLowerCase().includes(searchTerm) ||
               box.name?.toLowerCase().includes(searchTerm) ||
               box.serialNumber?.toLowerCase().includes(searchTerm) ||
               box.type?.toLowerCase().includes(searchTerm);
      });
    }
    
    if (this.selectedStatus !== 'All') {
      filtered = filtered.filter(box => box.status === this.selectedStatus);
    }
    
    this.filteredBoxes = filtered;
  }

  onStatusChange(status: BoxStatus | 'All'): void {
    this.selectedStatus = status;
    this.applyFilters();
  }

  goBack(): void {
    this.router.navigate(['/factories']);
  }

  viewBox(boxId: string, event?: Event): void {
    // Prevent event bubbling if event is provided
    if (event) {
      event.stopPropagation();
    }
    
    // Find the box to get projectId
    const box = this.boxes.find(b => b.id === boxId);
    if (box && box.projectId) {
      this.router.navigate(['/projects', box.projectId, 'boxes', boxId]);
    } else {
      console.error('Box not found or missing projectId:', boxId);
    }
  }

  getLocationLabel(location: ProjectLocation): string {
    switch (location) {
      case ProjectLocation.KSA:
        return 'KSA';
      case ProjectLocation.UAE:
        return 'UAE';
      default:
        return 'N/A';
    }
  }

  getCapacityPercentage(): number {
    if (!this.factory || !this.factory.capacity || this.factory.capacity === 0) return 0;
    return Math.min(100, (this.factory.currentOccupancy / this.factory.capacity) * 100);
  }

  getStatusClass(status: BoxStatus): string {
    const statusMap: Partial<Record<BoxStatus, string>> = {
      [BoxStatus.NotStarted]: 'status-not-started',
      [BoxStatus.InProgress]: 'status-in-progress',
      [BoxStatus.Completed]: 'status-completed',
      [BoxStatus.OnHold]: 'status-on-hold',
      [BoxStatus.Dispatched]: 'status-dispatched',
      [BoxStatus.QAReview]: 'status-qa-review',
      [BoxStatus.ReadyForDelivery]: 'status-ready',
      [BoxStatus.Delivered]: 'status-delivered'
    };
    return statusMap[status] || 'status-unknown';
  }

  getDispatchedCount(): number {
    return this.boxes.filter(box => box.status === BoxStatus.Dispatched).length;
  }

  getCompletedCount(): number {
    return this.boxes.filter(box => box.status === BoxStatus.Completed).length;
  }

  getInProgressCount(): number {
    return this.boxes.filter(box => box.status === BoxStatus.InProgress).length;
  }

  getNonDispatchedBoxCount(): number {
    // Return count of boxes excluding dispatched (for display purposes)
    return this.boxes.filter(box => box.status !== BoxStatus.Dispatched).length;
  }

  getStatusLabel(status: BoxStatus): string {
    const labels: Record<BoxStatus, string> = {
      [BoxStatus.NotStarted]: 'Not Started',
      [BoxStatus.InProgress]: 'In Progress',
      [BoxStatus.Completed]: 'Completed',
      [BoxStatus.OnHold]: 'On Hold',
      [BoxStatus.Dispatched]: 'Dispatched',
      [BoxStatus.QAReview]: 'QA Review',
      [BoxStatus.ReadyForDelivery]: 'Ready',
      [BoxStatus.Delivered]: 'Delivered'
    };
    return labels[status] || status;
  }

  navigateToBox(boxId: string): void {
    this.router.navigate(['/boxes', boxId]);
  }

  // Legend percentage calculations
  getInProgressPercentage(): number {
    if (this.boxes.length === 0) return 0;
    return Math.round((this.getInProgressCount() / this.boxes.length) * 100);
  }

  getCompletedPercentage(): number {
    if (this.boxes.length === 0) return 0;
    return Math.round((this.getCompletedCount() / this.boxes.length) * 100);
  }

  getEmptyPercentage(): number {
    if (!this.factory || !this.factory.capacity) return 0;
    const emptySpots = this.factory.availableCapacity || 0;
    return Math.round((emptySpots / this.factory.capacity) * 100);
  }
}

