import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-boxes-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './boxes-list.component.html',
  styleUrls: ['./boxes-list.component.scss']
})
export class BoxesListComponent implements OnInit {
  projectId: string = '';
  boxes: Box[] = [];
  filteredBoxes: Box[] = [];
  loading = true;
  error = '';
  canCreate = false;
  
  searchControl = new FormControl('');
  selectedStatus: BoxStatus | 'All' = 'All';
  BoxStatus = BoxStatus;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxService: BoxService,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
    this.canCreate = this.permissionService.canCreate('boxes');
    
    this.loadBoxes();
    this.setupSearch();
  }

  private setupSearch(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.applyFilters();
      });
  }

  loadBoxes(): void {
    this.loading = true;
    this.error = '';
    
    this.boxService.getBoxesByProject(this.projectId).subscribe({
      next: (boxes) => {
        this.boxes = boxes;
        this.filteredBoxes = boxes;
        this.loading = false;
        this.applyFilters();
      },
      error: (err) => {
        this.error = err.message || 'Failed to load boxes';
        this.loading = false;
        console.error('Error loading boxes:', err);
      }
    });
  }

  filterByStatus(status: BoxStatus | 'All'): void {
    this.selectedStatus = status;
    this.applyFilters();
  }

  private applyFilters(): void {
    let filtered = [...this.boxes];

    // Apply status filter
    if (this.selectedStatus !== 'All') {
      filtered = filtered.filter(box => box.status === this.selectedStatus);
    }

    // Apply search filter
    const searchTerm = this.searchControl.value?.toLowerCase() || '';
    if (searchTerm) {
      filtered = filtered.filter(box =>
        box.name?.toLowerCase().includes(searchTerm) ||
        box.code?.toLowerCase().includes(searchTerm) ||
        box.type?.toLowerCase().includes(searchTerm)
      );
    }

    this.filteredBoxes = filtered;
  }

  viewBox(boxId: string): void {
    this.router.navigate(['/projects', this.projectId, 'boxes', boxId]);
  }

  createBox(): void {
    this.router.navigate(['/boxes/create'], { 
      queryParams: { projectId: this.projectId }
    });
  }

  getStatusClass(status: BoxStatus): string {
    const statusMap: Record<BoxStatus, string> = {
      [BoxStatus.NotStarted]: 'badge-secondary',
      [BoxStatus.InProgress]: 'badge-warning',
      [BoxStatus.QAReview]: 'badge-info',
      [BoxStatus.Completed]: 'badge-success',
      [BoxStatus.ReadyForDelivery]: 'badge-primary',
      [BoxStatus.Delivered]: 'badge-success',
      [BoxStatus.OnHold]: 'badge-danger'
    };
    return statusMap[status] || 'badge-secondary';
  }

  getStatusLabel(status: BoxStatus): string {
    const labels: Record<BoxStatus, string> = {
      [BoxStatus.NotStarted]: 'Not Started',
      [BoxStatus.InProgress]: 'In Progress',
      [BoxStatus.QAReview]: 'QA Review',
      [BoxStatus.Completed]: 'Completed',
      [BoxStatus.ReadyForDelivery]: 'Ready for Delivery',
      [BoxStatus.Delivered]: 'Delivered',
      [BoxStatus.OnHold]: 'On Hold'
    };
    return labels[status] || status;
  }
}
