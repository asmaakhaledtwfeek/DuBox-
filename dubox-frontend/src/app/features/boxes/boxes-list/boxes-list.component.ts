import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Box, BoxStatus, BoxTypeStat } from '../../../core/models/box.model';
import { ProjectService } from '../../../core/services/project.service';
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
  projectName = '';
  projectCode = '';
  boxes: Box[] = [];
  filteredBoxes: Box[] = [];
  boxTypes: BoxTypeStat[] = [];
  selectedBoxType: string | null = null;
  showBoxTypes = true;
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
    private permissionService: PermissionService,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
    const boxType = this.route.snapshot.queryParams['boxType'];
    
    this.canCreate = this.permissionService.canCreate('boxes');
    this.loadProjectDetails();
    
    if (boxType) {
      this.selectedBoxType = boxType;
      this.showBoxTypes = false;
      this.loadBoxes();
    } else {
      this.loadBoxTypes();
    }
    
    this.setupSearch();
  }

  loadProjectDetails(): void {
    if (!this.projectId) {
      return;
    }

    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.projectName = project.name || '';
        this.projectCode = project.code || '';
      },
      error: (err) => {
        console.error('Error loading project details:', err);
      }
    });
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

  loadBoxTypes(): void {
    this.loading = true;
    this.error = '';
    this.showBoxTypes = true;
    this.selectedBoxType = null;
    
    this.boxService.getBoxTypeStatsByProject(this.projectId).subscribe({
      next: (response) => {
        this.boxTypes = response.boxTypeStats || [];
        this.loading = false;
      },
      error: (err) => {
        this.error = err.message || 'Failed to load box types';
        this.loading = false;
        console.error('Error loading box types:', err);
      }
    });
  }

  loadBoxes(): void {
    this.loading = true;
    this.error = '';
    
    this.boxService.getBoxesByProject(this.projectId).subscribe({
      next: (boxes) => {
        // Filter boxes by selected type if a type is selected
        if (this.selectedBoxType) {
          this.boxes = boxes.filter(box => box.type === this.selectedBoxType);
        } else {
          this.boxes = boxes;
        }
        this.filteredBoxes = this.boxes;
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

  viewBoxType(boxType: string): void {
    this.selectedBoxType = boxType;
    this.showBoxTypes = false;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { boxType: boxType },
      queryParamsHandling: 'merge'
    });
    this.loadBoxes();
  }

  backToBoxTypes(): void {
    this.selectedBoxType = null;
    this.showBoxTypes = true;
    this.boxes = [];
    this.filteredBoxes = [];
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {},
      queryParamsHandling: 'merge'
    });
    this.loadBoxTypes();
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

  // Expose Math to template
  Math = Math;
}
